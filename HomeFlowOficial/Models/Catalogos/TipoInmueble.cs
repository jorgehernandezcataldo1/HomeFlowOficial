using System.ComponentModel.DataAnnotations;

namespace HomeFlowOficial.Models.Catalogos
{
    // Casa, Departamento, Oficina, Bodega, Local Comercial, Parcela, Sitio...
    public class TipoInmueble
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
