using Microsoft.AspNetCore.Identity;

namespace HomeFlowOficial.Models.Identity
{
    // Extiende el usuario estándar de Identity con los campos propios del corredor/sistema.
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string NombreCompleto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
    }
}
