using System.ComponentModel.DataAnnotations;

namespace BivliotecaAPI.DTOs
{
    public class EditarClaimDTO
    {
        [EmailAddress]
        [Required]
        public required string? Email { get; set; }

    }
}
