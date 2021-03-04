using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Helpers;

namespace WebApiAspNet5.Models
{
    public class AutorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "El Nombre del autor debe tener {1} carateres o menos")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<LibroDTO> Libros { get; set; }
    }
}
