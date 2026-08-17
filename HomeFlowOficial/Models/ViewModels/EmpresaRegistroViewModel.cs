using System.ComponentModel.DataAnnotations;
using HomeFlowOficial.Models.Validation;

namespace HomeFlowOficial.Models.ViewModels
{
    public class EmpresaRegistroViewModel
    {
        [Required(ErrorMessage = "La razón social es obligatoria."), StringLength(150)]
        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El RUT de la empresa es obligatorio.")]
        [Rut(ErrorMessage = "El RUT ingresado no es válido.")]
        [Display(Name = "RUT empresa")]
        public string Rut { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
        [Display(Name = "Correo de contacto")]
        public string? Correo { get; set; }

        [Phone]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Tu nombre es obligatorio."), StringLength(150)]
        [Display(Name = "Tu nombre completo")]
        public string NombreCompletoAdmin { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo de acceso es obligatorio."), EmailAddress, StringLength(150)]
        [Display(Name = "Correo de acceso")]
        public string CorreoAdmin { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria."), DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tu RUT es obligatorio."), Rut(ErrorMessage = "El RUT ingresado no es válido.")]
        [Display(Name = "Tu RUT")]
        public string RutAdmin { get; set; } = string.Empty;
    }
}