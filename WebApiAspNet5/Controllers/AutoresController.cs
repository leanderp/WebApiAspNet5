using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Context;
using WebApiAspNet5.Entities;
using WebApiAspNet5.Models;

namespace WebApiAspNet5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAsync()
        {
            var autores = await _context.Autores.Include(x => x.Libros).ToListAsync();
            var autoresDTO = _mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }

        // GET: api/Autores/1
        [HttpGet("{id}", Name ="ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> GetAsync(int id)
        {
            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
            if(autor == null)
            {
                return NotFound();
            }

            var autorDTO = _mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }

        // POST: api/Autores
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]AutorCreacionDTO autorCreacion)
        {
            var autor = _mapper.Map<Autor>(autorCreacion);
            await _context.Autores.AddAsync(autor);
            await _context.SaveChangesAsync();
            var autorDTO = _mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autorDTO);
        }

        // PUT: api/Autores/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = _mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AutorExiste(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH: api/Autores/1
        // [{"op":"replace","path":"/fechaNacimiento","value": "0001-01-01"}]
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument==null)
            {
                return BadRequest();
            }

            var autorDeLaDB = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autorDeLaDB==null)
            {
                return NotFound();
            }

            var autorDTO = _mapper.Map<AutorCreacionDTO>(autorDeLaDB);

            patchDocument.ApplyTo(autorDTO, ModelState);

            var isValid = TryValidateModel(autorDeLaDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(autorDTO, autorDeLaDB);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Autores/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> DeleteAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return autor;
        }

        private bool AutorExiste(long id) =>
            _context.Autores.Any(a => a.Id == id);

    }
}
