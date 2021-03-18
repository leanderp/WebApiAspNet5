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
        private readonly IUrlHelper _urlHelper;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IUrlHelper urlHelper)
        {
            _context = context;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        // GET: api/Autores
        [HttpGet(Name ="ObtenerAutores")]
        public async Task<IActionResult> GetAsync(bool incluirEnlacesHATEOAS = false)
        {
            var autores = await _context.Autores.Include(x => x.Libros).ToListAsync();
            var autoresDTO = _mapper.Map<List<AutorDTO>>(autores);
            var resultado = new ColeccionDeRecursos<AutorDTO>(autoresDTO);

            if (incluirEnlacesHATEOAS)
            {
                autoresDTO.ForEach(a => GenerarEnlaces(a));
                resultado.Enlaces.Add(new Enlace(href: _urlHelper.Link("ObtenerAutores", new { }), rel: "self", metodo: "GET"));
                resultado.Enlaces.Add(new Enlace(href: _urlHelper.Link("CrearAutor", new { }), rel: "CrearAutor", metodo: "POST"));
                return Ok(resultado);
            }

            return Ok(autoresDTO);
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

            GenerarEnlaces(autorDTO);

            return autorDTO;
        }

        // POST: api/Autores
        [HttpPost(Name ="CrearAutor")]
        public async Task<IActionResult> PostAsync([FromBody]AutorCreacionDTO autorCreacion)
        {
            var autor = _mapper.Map<Autor>(autorCreacion);
            await _context.Autores.AddAsync(autor);
            await _context.SaveChangesAsync();
            var autorDTO = _mapper.Map<AutorDTO>(autor);
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autorDTO);
        }

        // PUT: api/Autores/1
        [HttpPut("{id}",Name ="ActualizarAutor")]
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
        [HttpPatch("{id}", Name ="ModificarAutor")]
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
        [HttpDelete("{id}", Name ="BorrarAutor")]
        public async Task<ActionResult<Autor>> DeleteAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AutorExiste(long id) =>
            _context.Autores.Any(a => a.Id == id);

        private void GenerarEnlaces(AutorDTO autor)
        {
            autor.Enlaces.Add(new Enlace(href: _urlHelper.Link("ObtenerAutor", new { id=autor.Id }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(href: _urlHelper.Link("ActualizarAutor", new { id = autor.Id }), rel: "actualizar-autor", metodo: "PUT"));
            autor.Enlaces.Add(new Enlace(href: _urlHelper.Link("ModificarAutor", new { id = autor.Id }), rel: "modificar-autor", metodo: "PATCH"));
            autor.Enlaces.Add(new Enlace(href: _urlHelper.Link("BorrarAutor", new { id = autor.Id }), rel: "borrar-autor", metodo: "DELETE"));
        }

    }
}
