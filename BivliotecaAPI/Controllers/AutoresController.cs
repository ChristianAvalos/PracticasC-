﻿using AutoMapper;
using Azure;
using BivliotecaAPI.Datos;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;
using BivliotecaAPI.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper) 
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous] //Permite el acceso sin autenticación
        public async Task<IEnumerable<AutorDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {

            var queryable =  context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var autores = await queryable.OrderBy(x => x.Nombres).Paginar(paginacionDTO).ToListAsync();
            var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);
            return autoresDTO;
        }

        [HttpGet("{id:int}",Name ="ObtenerAutor")] //api/autores/id
        [AllowAnonymous]
        [EndpointSummary("Obtiene autor por ID")]
        [EndpointDescription("Obtiene un autor específico junto con sus libros asociados por su ID.")]
        [ProducesResponseType<AutorConLibrosDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AutorConLibrosDTO>> Get([Description("El id del autor")]int id)
        {
            var autor = await context.Autores.Include(x => x.Libros)
                .ThenInclude(x => x.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {

                return NotFound();

            }
            var autorDTO = mapper.Map<AutorConLibrosDTO>(autor);

            return autorDTO ;
        }



        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)

        {
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            context.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutor",new {id = autor.Id}, autorDTO);   
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AutorCreacionDTO autorCreacionDTO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;  
            context.Update(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDTO);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id,JsonPatchDocument<AutorPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var autorDB = await context.Autores.FirstOrDefaultAsync(x=> x.Id == id);
            if (autorDB is null)
            {
                return BadRequest();
            }
            var autorPatchDTO = mapper.Map<AutorPatchDTO>(autorDB);
            patchDoc.ApplyTo(autorPatchDTO, ModelState);

            var esValido = TryValidateModel(autorPatchDTO);
            if (!esValido)
            {
                return ValidationProblem();
            }
            mapper.Map(autorPatchDTO, autorDB);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Autores.Where(x =>  x.Id == id).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
