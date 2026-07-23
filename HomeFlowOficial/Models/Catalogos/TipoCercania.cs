using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Catalogos
{
    // Metro, Colegio, Supermercado, Parque, Locomoción colectiva...
    public class TipoCercania
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Icono { get; set; }

        public bool Activo { get; set; } = true;
    }
}
