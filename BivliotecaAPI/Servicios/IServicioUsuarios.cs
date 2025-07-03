using Microsoft.AspNetCore.Identity;

namespace BivliotecaAPI.Servicios
{
    public interface IServicioUsuarios
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}