using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Catalogos;

namespace HomeFlowOficial.Models
{
    // Ej: Metro Baquedano a 400m, Colegio San Ignacio a 800m
    public class InmuebleCercania
    {
        public int Id { get; set; }

        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; } = null!;

        public int TipoCercaniaId { get; set; }
        public TipoCercania TipoCercania { get; set; } = null!;

        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        public int DistanciaMetros { get; set; }
    }
}
