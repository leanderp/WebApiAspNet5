using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Helpers;

namespace WebApiAspNet5.Models
{
    public class LibroDTO: Recurso
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int AutorId { get; set; }
        public AutorDTO Autor { get; set; }
    }
}
