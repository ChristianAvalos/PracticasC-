using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BivliotecaAPI.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor contextAccessor;

        public ServicioUsuarios(UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
        }
            public async Task<IdentityUser?> ObtenerUsuario()
            {
                var email = contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                if (email == null)
                {
                    throw new Exception("El usuario no está autenticado");
                }
                var usuario = await userManager.FindByEmailAsync(email);

                if (usuario == null)
                {
                    throw new Exception($"No se encontró un usuario con el email: {email}");
                }
                return usuario;
            }

    }
}
