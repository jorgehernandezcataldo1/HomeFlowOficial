using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Validation;

namespace HomeFlowOficial.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RUT es obligatorio.")]
        [Rut(ErrorMessage = "El RUT ingresado no es válido.")]
        [StringLength(12)]
        public string Rut { get; set; } = string.Empty;

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;

        public ICollection<Propietario> Propietarios { get; set; } = new List<Propietario>();
        public ICollection<Arrendatario> Arrendatarios { get; set; } = new List<Arrendatario>();
    }
}