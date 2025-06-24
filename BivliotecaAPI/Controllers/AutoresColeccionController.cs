using AutoMapper;
using BivliotecaAPI.Datos;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores-coleccion")]
    public class AutoresColeccionController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresColeccionController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       [HttpPost]
        public async Task<ActionResult> Post(IEnumerable<AutorCreacionDTO> autoresCreacionDTO)
        {
            var autores = mapper.Map<IEnumerable<Autor>>(autoresCreacionDTO);
            context.AddRange(autores);
            await context.SaveChangesAsync();
            return Ok();
            //var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);
            //return CreatedAtRoute("ObtenerAutoresColeccion", new { }, autoresDTO);
        }
        [HttpGet(Name = "ObtenerAutoresColeccion")]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);
            return Ok(autoresDTO);
        }

    }
}
