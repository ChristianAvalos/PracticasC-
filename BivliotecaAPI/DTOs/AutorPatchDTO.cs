using BivliotecaAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI.DTOs
{
    public class AutorPatchDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayusculaAtributte]
        public required string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        [PrimeraLetraMayusculaAtributte]
        public required string Apellidos { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public string? Identificacion { get; set; }
    }
}
