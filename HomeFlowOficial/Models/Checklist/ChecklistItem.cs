using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Checklist
{
    public class ChecklistItem
    {
        public int Id { get; set; }

        public int ChecklistPlantillaId { get; set; }
        public ChecklistPlantilla ChecklistPlantilla { get; set; } = null!;

        [Required, StringLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        public int Orden { get; set; }
        public bool Obligatorio { get; set; } = true;
    }
}
