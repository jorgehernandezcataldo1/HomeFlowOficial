using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Identity;
using System.ComponentModel.DataAnnotations;


namespace HomeFlowOficial.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        public string RazonSocial { get; set; } = string.Empty;

        public string Rut { get; set; } = string.Empty;

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<ApplicationUser> Usuarios { get; set; } = new List<ApplicationUser>();
    }
}
