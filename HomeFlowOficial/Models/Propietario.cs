using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Identity;


namespace HomeFlowOficial.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }

        public Persona Persona { get; set; } = null!;

        public string CorredorId { get; set; } = string.Empty;

        public ApplicationUser Corredor { get; set; } = null!;

        public bool ChecklistAprobado { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

        public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}
