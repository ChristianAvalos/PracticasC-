using AutoMapper;
using BivliotecaAPI.Datos;
using BivliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using BivliotecaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace BivliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var libros =  await context.Libros.ToListAsync();
            var librosDTO = mapper.Map<IEnumerable<LibroDTO>>(libros);
            return librosDTO;
        }
        [HttpGet("{id:int}",Name = "ObtenerLibros")] //api/libros/id
        public async Task<ActionResult<LibroConAutorDTO>> Get(int id)
        {
            var libro = await context.Libros.Include(x => x.Autores).FirstOrDefaultAsync(x => x.Id == id);
            if (libro is null)
            {

                return NotFound();

            }
            var libroDTO = mapper.Map<LibroConAutorDTO>(libro);

            return libroDTO;
        }

        [HttpPost]
        
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)

        {
            if (libroCreacionDTO.AutoresIds is null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds),
                    "No se puede crear libros sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await context.Autores
                                    .Where(x => libroCreacionDTO.AutoresIds.Contains(x.Id))
                                    .Select(x =>  x.Id).ToListAsync(); 

            if (autoresIdsExisten.Count != libroCreacionDTO.AutoresIds.Count )
            {
                var autoresNoExisten = libroCreacionDTO.AutoresIds.Except(autoresIdsExisten);
                var autoresNoExistenString = string.Join(",",autoresNoExisten);
                var mensajeDeError = $"Los siguientes autores no existen: {autoresNoExistenString}";
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds),mensajeDeError);
                return ValidationProblem();
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();
            var libroDTO = mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("ObtenerLibros", new { id = libro.Id }, libroDTO);
        }
        public void AsignarOrdenAutores(Libro libro)
        {
            if (libro.Autores is not null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden=i;
                }

            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds is null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds),
                    "No se puede crear libros sin autores");
                return ValidationProblem();
            }

            var autoresIdsExisten = await context.Autores
                                    .Where(x => libroCreacionDTO.AutoresIds.Contains(x.Id))
                                    .Select(x => x.Id).ToListAsync();

            if (autoresIdsExisten.Count != libroCreacionDTO.AutoresIds.Count)
            {
                var autoresNoExisten = libroCreacionDTO.AutoresIds.Except(autoresIdsExisten);
                var autoresNoExistenString = string.Join(",", autoresNoExisten);
                var mensajeDeError = $"Los siguientes autores no existen: {autoresNoExistenString}";
                ModelState.AddModelError(nameof(libroCreacionDTO.AutoresIds), mensajeDeError);
                return ValidationProblem();
            }


            var libroDB = await context.Libros
                .Include(x => x.Autores)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
