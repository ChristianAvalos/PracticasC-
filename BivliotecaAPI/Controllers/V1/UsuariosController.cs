using AutoMapper;
using BivliotecaAPI.Datos;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;
using BivliotecaAPI.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BivliotecaAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/usuarios")]
    
    public class UsuariosController: ControllerBase
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Usuario> signInManager;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UsuariosController(UserManager<Usuario> userManager,IConfiguration configuration,
                SignInManager<Usuario> signInManager,IServicioUsuarios servicioUsuarios,
                ApplicationDbContext context,IMapper mapper
            )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.servicioUsuarios = servicioUsuarios;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Policy ="esAdmin")]
        public async Task<IEnumerable<UsuarioDTO>> Get()
        {
            var usuarios = await context.Users.ToListAsync();
            var usuariosDTO = mapper.Map<List<UsuarioDTO>>(usuarios);
            return usuariosDTO;
        }

        [HttpPost("registro")]
        
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(
            CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var usuario = new Usuario
            { 
                UserName = credencialesUsuarioDTO.Email, 
                Email = credencialesUsuarioDTO.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDTO.Password!);
            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = await ConstruirToken(credencialesUsuarioDTO);
                return Ok(respuestaAutenticacion);
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return ValidationProblem();
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(
            CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            if (usuario is null)
            {
                return RetornarloginIncorrecto();
            }
            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuarioDTO.Password!,
                            lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return Ok(await ConstruirToken(credencialesUsuarioDTO));
            }
            else
            {
                return RetornarloginIncorrecto();
            }
        }

        private ActionResult RetornarloginIncorrecto()
        {
            ModelState.AddModelError(string.Empty, "Login incorrectos");
            return ValidationProblem();
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Put(ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            var usuario = await servicioUsuarios.ObtenerUsuario();
            if (usuario is null)
            {
                return Unauthorized();
            }
            usuario.FechaNacimiento = actualizarUsuarioDTO.FechaNacimiento;
            var resultado = await userManager.UpdateAsync(usuario);
            if (resultado.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpGet("renovar-token")]
        [Authorize]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> RenovarToken()
        {
            var usuario = await servicioUsuarios.ObtenerUsuario();
            if (usuario is null)
            {
                return Unauthorized();
            }

            var credencialesUsuarioDTO = new CredencialesUsuarioDTO
            {
                Email = usuario.Email!
            };
            return Ok(await ConstruirToken(credencialesUsuarioDTO));
        }
        [HttpPost("hacer-admin")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarClaimDTO editarClaimDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email!);
            if (usuario is null)
            {
                return NotFound();
            }
            var resultado = await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "true"));
            if (resultado.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("remover-admin")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarClaimDTO editarClaimDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);
            if (usuario is null)
            {
                return NotFound();
            }
            var resultado = await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "true"));
            if (resultado.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credencialesUsuarioDTO.Email),
                new Claim("lo qie yo quiera", "cualquier valor")
            };
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario!);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJWT"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenDeSeguridad = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDTO
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }

}
