using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.ViewModels.Checklist
{
    // Lo que llega en el body (JSON) al guardar un checklist completado desde el modal.
    public class ChecklistGuardarDto
    {
        [Required, Range(1, int.MaxValue)]
        public int ChecklistPlantillaId { get; set; }

        [Required]
        public TipoEntidadChecklist TipoEntidad { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int EntidadId { get; set; }

        [Required, MinLength(1, ErrorMessage = "El checklist no tiene ítems para guardar.")]
        public List<ChecklistItemRespuestaDto> Items { get; set; } = new();
    }

    public class ChecklistItemRespuestaDto
    {
        [Required, Range(1, int.MaxValue)]
        public int ChecklistItemId { get; set; }

        public bool Cumple { get; set; }

        [StringLength(300)]
        public string? Observacion { get; set; }
    }
}
