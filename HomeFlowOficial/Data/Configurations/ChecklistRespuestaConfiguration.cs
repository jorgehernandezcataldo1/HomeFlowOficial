using HomeFlowOficial.Models.Checklist;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class ChecklistRespuestaConfiguration : IEntityTypeConfiguration<ChecklistRespuesta>
    {
        public void Configure(EntityTypeBuilder<ChecklistRespuesta> builder)
        {
            // Búsqueda típica: "dame el checklist de este Inmueble/Id"
            builder.HasIndex(r => new { r.TipoEntidad, r.EntidadId });
        }
    }
}