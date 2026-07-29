using HomeFlowOficial.Models.Enums;
using HomeFlowOficial.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models
{
    public class Arrendatario
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }

        public Persona Persona { get; set; } = null!;

        public string CorredorId { get; set; } = string.Empty;

        public ApplicationUser Corredor { get; set; } = null!;

        public TipoContratoLaboral TipoContratoLaboral { get; set; }

        public decimal IngresoLiquido { get; set; }

        public int? AntiguedadLaboralMeses { get; set; }

        public bool TieneHijos { get; set; }

        public int NumeroHijos { get; set; }

        public bool TieneMascota { get; set; }

        public string? DetalleMascota { get; set; }

        public bool ChecklistAprobado { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    }
}
