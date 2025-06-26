using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI
{
    public class PersonaOpciones
    {   public const string Seccion = "seccion_1";
        [Required]
        public required string Nombre { get; set; }
        public required int Edad { get; set; }
    }
}
