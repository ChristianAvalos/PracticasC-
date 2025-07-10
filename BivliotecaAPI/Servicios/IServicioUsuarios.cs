using BivliotecaAPI.Entidades;
using Microsoft.AspNetCore.Identity;

namespace BivliotecaAPI.Servicios
{
    public interface IServicioUsuarios
    {
        Task<Usuario?> ObtenerUsuario();
    }
}