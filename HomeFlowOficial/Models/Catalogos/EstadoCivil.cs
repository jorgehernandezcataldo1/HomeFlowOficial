using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Catalogos
{
    // Catálogo editable desde el backoffice, no un enum,
    // porque el corredor podría necesitar agregar variantes (ej. "Conviviente Civil").
    public class EstadoCivil
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
