using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Validation;

namespace HomeFlowOficial.Models.ViewModels
{
    // Lo que llega desde el formulario/modal de creación de propietario.
    public class PropietarioViewModel
    {
        [Required(ErrorMessage = "El RUT es obligatorio.")]
        [Rut(ErrorMessage = "El RUT ingresado no es válido.")]
        [Display(Name = "RUT")]
        public string Rut { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los nombres son obligatorios."), StringLength(100)]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio."), StringLength(100)]
        [Display(Name = "Apellido paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Apellido materno")]
        public string? ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress]
        [Display(Name = "Correo")]
        public string Correo { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria."), StringLength(250)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona el estado civil.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecciona el estado civil.")]
        [Display(Name = "Estado civil")]
        public int EstadoCivilId { get; set; }
    }
}
