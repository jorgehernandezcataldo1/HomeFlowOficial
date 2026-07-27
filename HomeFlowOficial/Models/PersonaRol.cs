using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models
{
    public class PersonaRol
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }

        public Persona Persona { get; set; } = null!;

        public TipoRolPersona TipoRol { get; set; }
    }
}
