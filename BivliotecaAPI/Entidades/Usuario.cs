using Microsoft.AspNetCore.Identity;

namespace BivliotecaAPI.Entidades
{
    public class Usuario: IdentityUser
    {
        public DateTime FechaNacimiento { get; set; }

    }
}
