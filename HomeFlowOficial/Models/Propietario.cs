namespace HomeFlowOficial.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona { get; set; } = null!;

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

        public bool ChecklistAprobado { get; set; } = false;

        public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    }
}
