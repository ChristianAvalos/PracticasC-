namespace BivliotecaAPI.DTOs
{
    public class LibroConAutorDTO: LibroDTO
    {
        public int autorId {  get; set; }
        public required string AutorNombre { get; set; }
    }
}
