using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.Checklist
{
    // Registro de un checklist ya realizado sobre un Propietario, Inmueble o Arrendatario.
    // TipoEntidad + EntidadId apuntan a la fila correspondiente en la tabla que toque.
    public class ChecklistRespuesta
    {
        public int Id { get; set; }

        public int ChecklistPlantillaId { get; set; }
        public ChecklistPlantilla ChecklistPlantilla { get; set; } = null!;

        public int EntidadId { get; set; }
        public TipoEntidadChecklist TipoEntidad { get; set; }

        // Id del usuario (AspNetUsers) que realizó el checklist
        public string CorredorId { get; set; } = string.Empty;

        public DateTime FechaRealizacion { get; set; } = DateTime.UtcNow;
        public bool Aprobado { get; set; }

        public ICollection<ChecklistRespuestaItem> Respuestas { get; set; } = new List<ChecklistRespuestaItem>();
    }
}
