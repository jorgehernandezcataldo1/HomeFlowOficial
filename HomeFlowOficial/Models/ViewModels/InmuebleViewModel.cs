using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Enums;

namespace HomeFlowOficial.Models.ViewModels
{
    // Lo que llega desde el formulario/modal de creación de inmueble.
    // El Id, Estado, FechaIngreso y ChecklistAprobado los fija el servidor, no el cliente.
    public class InmuebleViewModel
    {
        [Required(ErrorMessage = "Selecciona el propietario.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el propietario.")]
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }

        [Required(ErrorMessage = "Selecciona el tipo de inmueble.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el tipo de inmueble.")]
        [Display(Name = "Tipo de inmueble")]
        public int TipoInmuebleId { get; set; }

        [Required(ErrorMessage = "Selecciona la operación.")]
        [Display(Name = "Operación")]
        public TipoOperacion TipoOperacion { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria."), StringLength(250)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La comuna es obligatoria."), StringLength(100)]
        [Display(Name = "Comuna")]
        public string Comuna { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Región")]
        public string Region { get; set; } = "Metropolitana";

        // Solo aplica a departamentos/oficinas
        [Range(-5, 200)]
        [Display(Name = "Piso")]
        public int? Piso { get; set; }

        [StringLength(50)]
        [Display(Name = "Torre")]
        public string? Torre { get; set; }

        [StringLength(20)]
        [Display(Name = "N° depto")]
        public string? NumeroDepto { get; set; }

        [Required, Range(0, 30, ErrorMessage = "Ingresa un número de dormitorios válido.")]
        [Display(Name = "Dormitorios")]
        public int Habitaciones { get; set; }

        [Required, Range(0, 20, ErrorMessage = "Ingresa un número de baños válido.")]
        [Display(Name = "Baños")]
        public int Banos { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Superficie útil (m²)")]
        public decimal? SuperficieUtilM2 { get; set; }

        [Range(0, 100000)]
        [Display(Name = "Superficie total (m²)")]
        public decimal? SuperficieTotalM2 { get; set; }

        [Range(0, 20)]
        [Display(Name = "Estacionamientos")]
        public int Estacionamientos { get; set; }

        [Display(Name = "Tiene bodega")]
        public bool TieneBodega { get; set; }

        [Range(0, 99999999)]
        [Display(Name = "Gasto común")]
        public decimal? GastoComun { get; set; }

        [Range(0, 99999999)]
        [Display(Name = "Agua (aprox.)")]
        public decimal? GastoAguaEstimado { get; set; }

        [Range(0, 99999999)]
        [Display(Name = "Luz (aprox.)")]
        public decimal? GastoLuzEstimado { get; set; }

        [Range(0, 99999999)]
        [Display(Name = "Gas (aprox.)")]
        public decimal? GastoGasEstimado { get; set; }

        [Display(Name = "Paga contribuciones")]
        public bool PagaContribuciones { get; set; }

        [Range(0, 99999999)]
        [Display(Name = "Monto contribuciones")]
        public decimal? MontoContribuciones { get; set; }

        [Display(Name = "Amoblado")]
        public bool Amoblado { get; set; }

        [Display(Name = "Equipado")]
        public bool Equipado { get; set; }

        [Display(Name = "Acepta mascotas")]
        public bool AceptaMascotas { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ingresa un precio válido.")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [StringLength(2000)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
