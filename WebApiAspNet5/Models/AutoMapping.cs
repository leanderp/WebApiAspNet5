using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAspNet5.Entities;

namespace WebApiAspNet5.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Autor, AutorDTO>();
            CreateMap<Libro, LibroDTO>();
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorCreacionDTO>();
        }
    }
}
