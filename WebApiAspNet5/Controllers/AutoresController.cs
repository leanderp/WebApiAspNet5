using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Context;
using WebApiAspNet5.Entities;

namespace WebApiAspNet5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAsync()
        {
            return await _context.Autores.Include(x => x.Libros).ToListAsync();
        }

        // GET: api/Autores/1
        [HttpGet("{id}", Name ="ObtenerAutor")]
        public async Task<ActionResult<Autor>> GetAsync(int id)
        {
            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
            if(autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // POST: api/Autores
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Autor autor)
        {
            await _context.Autores.AddAsync(autor);
            await _context.SaveChangesAsync();
            
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autor);
        }

        // PUT: api/Autores/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Autor value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _context.Entry(value).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) when (!AutorExiste(id))
            {
                return NotFound();
            }

            return Ok();
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
