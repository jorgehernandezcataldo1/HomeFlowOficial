using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models
{
    public class Arrendatario
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona { get; set; } = null!;

        public TipoContratoLaboral TipoContratoLaboral { get; set; }

        [Range(0, 99999999)]
        public decimal? IngresoLiquido { get; set; }

        public int? AntiguedadLaboralMeses { get; set; }

        public bool TieneHijos { get; set; }
        public int NumeroHijos { get; set; }

        public bool TieneMascota { get; set; }
        [StringLength(150)]
        public string? DetalleMascota { get; set; }

        public bool ChecklistAprobado { get; set; } = false;

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    }
}
