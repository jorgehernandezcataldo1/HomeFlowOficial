namespace HomeFlowOficial.Models.ViewModels
{
    // Proyección liviana para la grilla del Index (evita traer toda la entidad).
    public class PropietarioListItemViewModel
    {
        public int Id { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public int CantidadInmuebles { get; set; }
        public bool ChecklistAprobado { get; set; }
    }
}
