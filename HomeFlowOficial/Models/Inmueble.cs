using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Catalogos;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models
{
    public class Inmueble
    {
        public int Id { get; set; }

        public int PropietarioId { get; set; }
        public Propietario Propietario { get; set; } = null!;

        public int TipoInmuebleId { get; set; }
        public TipoInmueble TipoInmueble { get; set; } = null!;

        public TipoOperacion TipoOperacion { get; set; }
        public EstadoInmueble Estado { get; set; } = EstadoInmueble.Pendiente;

        [Required, StringLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Comuna { get; set; } = string.Empty;

        [StringLength(100)]
        public string Region { get; set; } = "Metropolitana";

        // Solo aplica a departamentos
        public int? Piso { get; set; }
        [StringLength(50)]
        public string? Torre { get; set; }
        [StringLength(20)]
        public string? NumeroDepto { get; set; }

        public int Habitaciones { get; set; }
        public int Banos { get; set; }
        public decimal? SuperficieUtilM2 { get; set; }
        public decimal? SuperficieTotalM2 { get; set; }
        public int Estacionamientos { get; set; }
        public bool TieneBodega { get; set; }

        public decimal? GastoComun { get; set; }
        public decimal? GastoAguaEstimado { get; set; }
        public decimal? GastoLuzEstimado { get; set; }
        public decimal? GastoGasEstimado { get; set; }

        public bool PagaContribuciones { get; set; }
        public decimal? MontoContribuciones { get; set; }

        public bool Amoblado { get; set; }
        public bool Equipado { get; set; }
        public bool AceptaMascotas { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [StringLength(2000)]
        public string? Descripcion { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
        public DateTime? FechaPublicacion { get; set; }
        public bool Activo { get; set; } = true;
        public bool ChecklistAprobado { get; set; } = false;

        public ICollection<InmuebleCercania> Cercanias { get; set; } = new List<InmuebleCercania>();
        public bool TieneExclusividad { get; set; } = false;
    }
}
