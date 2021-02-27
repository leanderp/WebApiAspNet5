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
        public ActionResult<IEnumerable<Libro>> Get()
        {
            return _context.Libros.Include(x => x.Autor).ToList();
        }

        // GET: api/Libros/1
        [HttpGet("{id}", Name = "ObtenerLibro")]
        public ActionResult<Libro> Get(int id)
        {
            var libro = _context.Libros.Include(x => x.Autor).FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // POST: api/Libros
        [HttpPost]
        public ActionResult Post([FromBody] Libro libro)
        {
            _context.Libros.Add(libro);
            _context.SaveChanges();
            
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
        public ActionResult<Libro> Delete(int id)
        {
            var libro = _context.Libros.Find(id);

            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            _context.SaveChanges();

            return libro;
        }


        private bool LibroExiste(long id) =>
            _context.Libros.Any(a => a.Id == id);
    }
}
