using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI.DTOs
{
    public class ComentarioCreacionDTO
    {
        [Required]
        public required string Cuerpo { get; set; }
    }
}
