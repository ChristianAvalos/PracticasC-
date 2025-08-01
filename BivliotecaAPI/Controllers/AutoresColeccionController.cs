﻿using AutoMapper;
using BivliotecaAPI.Datos;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores-coleccion")]
    [Authorize(Policy = "EsAdmin")]
    public class AutoresColeccionController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresColeccionController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet("{ids}", Name = "ObtenerAutoresPorIds")]
        public async Task<ActionResult<List<AutorConLibrosDTO>>> Get([FromRoute] string ids)
        {
            var idsColeccion = new List<int>();
            foreach (var id in ids.Split(","))
            {
                if (int.TryParse(id, out int idInt))
                {
                    idsColeccion.Add(idInt);
                }
            } 
            if (!idsColeccion.Any())
            {
                ModelState.AddModelError(nameof(ids), "Nungun Id fue encontrado");
                return ValidationProblem();
            }
            var autores = await context.Autores
                .Include(x => x.Libros)
                .ThenInclude(x => x.Libro)
                .Where(x => idsColeccion.Contains(x.Id))
                .ToListAsync();
            if (autores.Count != idsColeccion.Count)
                {
                return NotFound();
                }
            var autoresDTO = mapper.Map<IEnumerable<AutorConLibrosDTO>>(autores);
            return Ok(autoresDTO);
        } 

        [HttpPost]
        public async Task<ActionResult> Post(IEnumerable<AutorCreacionDTO> autoresCreacionDTO)
        {
            var autores = mapper.Map<IEnumerable<Autor>>(autoresCreacionDTO);
            context.AddRange(autores);
            await context.SaveChangesAsync();
            var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);
            var ids = autores.Select(x => x.Id);
            var idsString = string.Join(",", ids);
            return CreatedAtRoute("ObtenerAutoresPorIds", new { ids = idsString }, autoresDTO);
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
