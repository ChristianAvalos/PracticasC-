using BivliotecaAPI.Entidades;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BivliotecaAPI.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IHttpContextAccessor contextAccessor;

        public ServicioUsuarios(UserManager<Usuario> userManager, IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this.contextAccessor = contextAccessor;
        }
            public async Task<Usuario?> ObtenerUsuario()
            {
                var emailClaim = contextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
                if (emailClaim == null)
                {
                    throw new Exception("El usuario no está autenticado");
                }
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);

                if (usuario == null)
                {
                    throw new Exception($"No se encontró un usuario con el email: {email}");
                }
                return usuario;
            }

    }
}
