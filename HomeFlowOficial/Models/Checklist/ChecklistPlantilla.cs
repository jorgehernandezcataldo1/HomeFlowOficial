using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.Checklist
{
    // Plantilla reutilizable: "Checklist Propietario v1", "Checklist Depto v1", etc.
    public class ChecklistPlantilla
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public TipoEntidadChecklist TipoEntidad { get; set; }
        public int Version { get; set; } = 1;
        public bool Activo { get; set; } = true;

        public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
    }
}
