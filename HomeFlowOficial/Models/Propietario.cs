using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Identity;


namespace HomeFlowOficial.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }

        public Persona Persona { get; set; } = null!;

        public string? Observaciones { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

        public bool ChecklistAprobado { get; set; }

        public string? UsuarioIngresoId { get; set; }

        public ApplicationUser? UsuarioIngreso { get; set; }

        public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}
