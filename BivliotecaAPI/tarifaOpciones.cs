using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI
{
    public class tarifaOpciones
    {
        public const string Seccion = "tarifas";
        [Required]
        public required decimal dia { get; set; }
        [Required]
        public required decimal noche { get; set; }
    }
}
