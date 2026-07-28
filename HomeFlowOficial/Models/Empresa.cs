using HomeFlowOficial.Models;
using System.ComponentModel.DataAnnotations;


namespace HomeFlowOficial.Models
{ 
    public class Empresa
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [Required, StringLength(12)]
        public string Rut { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Correo { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<Persona> Personas { get; set; } = new List<Persona>();

        public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}
