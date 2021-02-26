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
        public ActionResult<IEnumerable<Autor>> Get()
        {
            return _context.Autores.ToList();
        }

        // GET: api/Autores/1
        [HttpGet("{id}", Name ="ObtenerAutor")]
        public ActionResult<Autor> Get(int id)
        {
            var autor = _context.Autores.Find(id);
            if(autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // POST: api/Autores
        [HttpPost]
        public ActionResult Post([FromBody]Autor autor)
        {
            _context.Autores.Add(autor);
            _context.SaveChanges();
            
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autor);
        }

        // PUT: api/Autores/1
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
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
        public ActionResult<Autor> Delete(int id)
        {
            var autor = _context.Autores.Find(id);

            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            _context.SaveChanges();
            return autor;
        }

        private bool AutorExiste(long id) =>
            _context.Autores.Any(a => a.Id == id);

    }
}
