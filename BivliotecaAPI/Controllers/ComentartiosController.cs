using AutoMapper;
using BivliotecaAPI.Datos;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;
using BivliotecaAPI.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    [Authorize]


    public class ComentartiosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IServicioUsuarios serviciosUsuarios;

        public ComentartiosController(ApplicationDbContext context, IMapper mapper,
            IServicioUsuarios serviciosUsuarios) {

            this.context = context;
            this.mapper = mapper;
            this.serviciosUsuarios = serviciosUsuarios;
        }
        [HttpGet]
        [AllowAnonymous]
        [OutputCache]
        public async Task<ActionResult<List<ComentarioDTO>>> get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();

            }


            var comentarios = await context.Comentarios
                .Include(x => x.Usuario)
                .Where(x => x.LibroId == libroId)
                .OrderByDescending(x => x.FechaPublicacion)
                .ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }
        [HttpGet("{id}", Name = "ObtenerComentario")]
        [AllowAnonymous]
        [OutputCache]
        public async Task<ActionResult<ComentarioDTO>> get(Guid id)
        {
            var comentario = await context.Comentarios
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (comentario == null)
            {

                return NotFound();
            }
            return mapper.Map<ComentarioDTO>(comentario);

        }
        [HttpPost]
        public async Task<ActionResult> Post(int libroId,ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();

            }
            var usuario = await serviciosUsuarios.ObtenerUsuario();
            if (usuario is null)
            {
                return NotFound();
            }
            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.FechaPublicacion = DateTime.UtcNow;
            comentario.UsuarioId = usuario.Id;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new {id = comentario.Id,libroId}, comentarioDTO);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id,int libroId, JsonPatchDocument<ComentarioPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();

            }
            var usuario = await serviciosUsuarios.ObtenerUsuario();
            if (usuario is null)
            {
                return NotFound();
            }

            var comnentarioDB = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comnentarioDB is null)
            {
                return BadRequest();
            }

            if (comnentarioDB.UsuarioId != usuario.Id)
            {
                return Forbid();
            }


            var comentarioPatchDTO = mapper.Map<ComentarioPatchDTO>(comnentarioDB);
            patchDoc.ApplyTo(comentarioPatchDTO, ModelState);

            var esValido = TryValidateModel(comentarioPatchDTO);
            if (!esValido)
            {
                return ValidationProblem();
            }
            mapper.Map(comentarioPatchDTO, comnentarioDB);

            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id,int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();

            }
            var usuario = await serviciosUsuarios.ObtenerUsuario();
            if (usuario is null)
            {
                return NotFound();
            }

            var comentarioDB = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
            if (comentarioDB is null)
            {
                return NotFound();
            }
            if (comentarioDB.UsuarioId != usuario.Id)
            {
                return Forbid();
            }
            comentarioDB.EstaBorrado = true;
            context.Update(comentarioDB);

            await context.SaveChangesAsync();


            return NoContent();
        }


    }
}
