using BivliotecaAPI.Controllers;

namespace BivliotecaAPI.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task Borrar (string ruta,string contenedor)
        {
            // Implementación para borrar un archivo
            throw new NotImplementedException();
        }
        Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            // Implementación para almacenar un archivo
            throw new NotImplementedException();
        }
        async Task<string> Editar(string ruta, string contenedor, IFormFile archivo)
        {
            await Borrar(ruta, contenedor);
            return await Almacenar(contenedor, archivo);
        }
    }
}
