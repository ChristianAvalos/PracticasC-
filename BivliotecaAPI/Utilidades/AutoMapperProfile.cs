using AutoMapper;
using BivliotecaAPI.DTOs;
using BivliotecaAPI.Entidades;

namespace BivliotecaAPI.Utilidades
{
    public class AutoMapperProfile: Profile

    {
        public AutoMapperProfile() 
        { 
            CreateMap<Autor,AutorDTO>()
                .ForMember(dto => dto.NombreCompleto, 
                config => config.MapFrom(autor => MapearNombreYApellidoAutor(autor)));

            CreateMap<Autor, AutorConLibrosDTO>()
                .ForMember(dto => dto.NombreCompleto,
                config => config.MapFrom(autor => MapearNombreYApellidoAutor(autor)));

            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorPatchDTO>().ReverseMap();

            CreateMap<AutorLibro,LibroDTO>()
                .ForMember(dto => dto.Id,config => config.MapFrom(ent => ent.LibroId))
                .ForMember(dto => dto.Titulo,config => config.MapFrom(ent => ent.Libro!.Titulo));


            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(ent => ent.Autores, config => config.MapFrom(dto => dto.AutoresIds.Select(id => new AutorLibro { AutorId = id })));

            CreateMap<Libro, LibroConAutoresDTO>();

            CreateMap<AutorLibro, AutorDTO>()
                .ForMember(dto => dto.Id, config => config.MapFrom(ent => ent.AutorId))
                .ForMember(dto => dto.NombreCompleto, config => config.MapFrom(ent => MapearNombreYApellidoAutor(ent.Autor!)));

            CreateMap<ComentarioCreacionDTO,Comentario>();
            CreateMap<Comentario,ComentarioDTO>();
            CreateMap<ComentarioPatchDTO,Comentario>().ReverseMap();
        }
        private string MapearNombreYApellidoAutor(Autor autor) => $"{autor.Nombres} {autor.Apellidos}";
    }
}
