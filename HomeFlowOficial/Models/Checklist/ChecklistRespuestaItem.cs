using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Checklist
{
    public class ChecklistRespuestaItem
    {
        public int Id { get; set; }

        public int ChecklistRespuestaId { get; set; }
        public ChecklistRespuesta ChecklistRespuesta { get; set; } = null!;

        public int ChecklistItemId { get; set; }
        public ChecklistItem ChecklistItem { get; set; } = null!;

        public bool Cumple { get; set; }

        [StringLength(300)]
        public string? Observacion { get; set; }
    }
}
