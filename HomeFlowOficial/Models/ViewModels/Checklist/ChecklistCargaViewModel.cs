using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.ViewModels.Checklist
{
    // Lo que el controlador devuelve como JSON para pintar el modal de checklist,
    // ya sea que se esté completando por primera vez o reabriendo uno ya guardado.
    public class ChecklistCargaViewModel
    {
        public int ChecklistPlantillaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoEntidadChecklist TipoEntidad { get; set; }
        public int EntidadId { get; set; }
        public bool Aprobado { get; set; }
        public List<ChecklistItemCargaViewModel> Items { get; set; } = new();
    }

    public class ChecklistItemCargaViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Obligatorio { get; set; }
        public bool Cumple { get; set; }
        public string? Observacion { get; set; }
    }
}
