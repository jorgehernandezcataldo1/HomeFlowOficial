using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Validation
{
    // Valida el formato y dígito verificador de un RUT chileno (módulo 11).
    // Se usa como [Rut] sobre cualquier propiedad string en un ViewModel.
    public class RutAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string rutOriginal || string.IsNullOrWhiteSpace(rutOriginal))
                return ValidationResult.Success; // [Required] se encarga de la obligatoriedad

            var rut = rutOriginal.Replace(".", "").Replace("-", "").Trim().ToUpperInvariant();
            if (rut.Length < 2)
                return new ValidationResult("El RUT ingresado no es válido.");

            var cuerpo = rut[..^1];
            var dvIngresado = rut[^1];

            if (!long.TryParse(cuerpo, out _))
                return new ValidationResult("El RUT ingresado no es válido.");

            var suma = 0;
            var multiplo = 2;
            for (var i = cuerpo.Length - 1; i >= 0; i--)
            {
                suma += (cuerpo[i] - '0') * multiplo;
                multiplo = multiplo == 7 ? 2 : multiplo + 1;
            }

            var resto = 11 - (suma % 11);
            var dvEsperado = resto switch
            {
                11 => '0',
                10 => 'K',
                _ => (char)(resto + '0')
            };

            return dvIngresado == dvEsperado
                ? ValidationResult.Success
                : new ValidationResult("El RUT ingresado no es válido.");
        }
    }
}
