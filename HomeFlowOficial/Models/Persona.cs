using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Catalogos;

namespace HomeFlowOficial.Models
{
    // Datos personales base. Tanto Propietario como Arrendatario apuntan aquí,
    // así una misma persona (mismo RUT) puede tener ambos roles sin duplicar datos.
    public class Persona
    {
        public int Id { get; set; }

        [Required, StringLength(12)]
        public string Rut { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ApellidoMaterno { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Phone, StringLength(20)]
        public string? Telefono { get; set; }

        [Required, StringLength(250)]
        public string Direccion { get; set; } = string.Empty;

        public int EstadoCivilId { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }

        // Solo se guarda la ruta del archivo (almacenado fuera de wwwroot, servido por un
        // controlador con [Authorize] que valida permisos antes de entregarlo).
        [StringLength(300)]
        public string? FotoCarnetUrl { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;

        public Propietario? Propietario { get; set; }
        public Arrendatario? Arrendatario { get; set; }
    }
}
