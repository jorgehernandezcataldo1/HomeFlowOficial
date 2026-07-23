using HomeFlowOficial.Models.Checklist;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class ChecklistRespuestaItemConfiguration : IEntityTypeConfiguration<ChecklistRespuestaItem>
    {
        public void Configure(EntityTypeBuilder<ChecklistRespuestaItem> builder)
        {
            // Borrar la "sesión" de checklist sí arrastra sus items.
            builder.HasOne(item => item.ChecklistRespuesta)
                   .WithMany(r => r.Respuestas)
                   .HasForeignKey(item => item.ChecklistRespuestaId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Pero borrar un ítem de la plantilla NO debe arrastrar respuestas históricas.
            // Restrict además protege: no te deja borrar un ChecklistItem si ya tiene
            // respuestas asociadas (evita perder el dato de auditoría por accidente).
            builder.HasOne(item => item.ChecklistItem)
                   .WithMany()
                   .HasForeignKey(item => item.ChecklistItemId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}