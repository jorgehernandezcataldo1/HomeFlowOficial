using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Catalogos;

namespace HomeFlowOficial.Models
{
    public class Persona
    {
        public int Id { get; set; }

        public string Rut { get; set; } = "";

        public string Nombres { get; set; } = "";

        public string ApellidoPaterno { get; set; } = "";

        public string? ApellidoMaterno { get; set; }

        public string Correo { get; set; } = "";

        public string? Telefono { get; set; }

        public string Direccion { get; set; } = "";

        public int EstadoCivilId { get; set; }

        public EstadoCivil EstadoCivil { get; set; } = null!;

        public string? FotoCarnetUrl { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<PersonaRol> Roles { get; set; } = new List<PersonaRol>();

        public Propietario? Propietario { get; set; }

        public Arrendatario? Arrendatario { get; set; }
    }
}
