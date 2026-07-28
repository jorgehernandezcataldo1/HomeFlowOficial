using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.ViewModels.Checklist
{
    public class ChecklistPlantillaAdminListItemViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoEntidadChecklist TipoEntidad { get; set; }
        public int Version { get; set; }
        public bool Activo { get; set; }
        public int CantidadItems { get; set; }
        public int CantidadRespuestasHistoricas { get; set; }
    }

    public class ChecklistPlantillaAdminDetalleViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoEntidadChecklist TipoEntidad { get; set; }
        public int Version { get; set; }
        public List<ChecklistItemAdminViewModel> Items { get; set; } = new();
    }

    public class ChecklistItemAdminViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Orden { get; set; }
        public bool Obligatorio { get; set; } = true;
    }

    // Body del POST /ChecklistAdmin/Guardar
    public class ChecklistPlantillaGuardarDto
    {
        public int? PlantillaBaseId { get; set; } // null = plantilla nueva para ese TipoEntidad

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public TipoEntidadChecklist TipoEntidad { get; set; }

        [Required, MinLength(1, ErrorMessage = "Agrega al menos un ítem.")]
        public List<ChecklistItemAdminViewModel> Items { get; set; } = new();
    }
}