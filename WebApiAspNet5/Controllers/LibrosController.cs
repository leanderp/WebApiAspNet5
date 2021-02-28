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
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetAsync()
        {
            return await _context.Libros.Include(x => x.Autor).ToListAsync();
        }

        // GET: api/Libros/1
        [HttpGet("{id}", Name = "ObtenerLibro")]
        public async Task<ActionResult<Libro>> GetAsync(int id)
        {
            var libro = await _context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Libro libro)
        {
            await _context.Libros.AddAsync(libro);
            await _context.SaveChangesAsync();
            
            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
        }

        // PUT: api/Libros/1
        [HttpPut("{id}")]
        public ActionResult Put(int id , [FromBody] Libro value)
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
            catch (DbUpdateConcurrencyException) when (!LibroExiste(id))
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/Libros/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<Libro>> DeleteAsync(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return libro;
        }


        private bool LibroExiste(long id) =>
            _context.Libros.Any(a => a.Id == id);
    }
}
