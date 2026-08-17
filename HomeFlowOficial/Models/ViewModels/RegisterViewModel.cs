using HomeFlowOficial.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.ViewModels
{
    public class RegisterViewModel
    {
        // RegisterViewModel.cs
        [Required(ErrorMessage = "El RUT es obligatorio."), Rut(ErrorMessage = "El RUT ingresado no es válido.")]
        [Display(Name = "RUT")]
        public string Rut { get; set; } = string.Empty;
        [Required, StringLength(150)]
        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 8)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "Corredor";
    }
}
