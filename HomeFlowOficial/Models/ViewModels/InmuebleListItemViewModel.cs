using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.ViewModels
{
    // Proyección liviana para la grilla del Index (evita traer toda la entidad).
    public class InmuebleListItemViewModel
    {
        public int Id { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Comuna { get; set; } = string.Empty;
        public string TipoInmuebleNombre { get; set; } = string.Empty;
        public string PropietarioNombre { get; set; } = string.Empty;
        public TipoOperacion TipoOperacion { get; set; }
        public EstadoInmueble Estado { get; set; }
        public decimal Precio { get; set; }
        public bool ChecklistAprobado { get; set; }
    }
}
