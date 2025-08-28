using BivliotecaAPI.DTOs;

namespace BivliotecaAPI.Servicios.V1
{
    public interface IGeneradorEnlaces
    {
        Task GenerarEnlaces(AutorDTO autorDTO);
    }
}