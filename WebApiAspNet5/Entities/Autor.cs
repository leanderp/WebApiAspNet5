using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Helpers;

namespace WebApiAspNet5.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="El Nombre del autor debe tener {1} carateres o menos")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
