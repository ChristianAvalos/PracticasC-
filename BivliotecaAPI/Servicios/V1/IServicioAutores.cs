using BivliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BivliotecaAPI.Servicios.V1
{
    public interface IServicioAutores
    {
        Task<IEnumerable<AutorDTO>> Get(PaginacionDTO paginacionDTO);
    }
}