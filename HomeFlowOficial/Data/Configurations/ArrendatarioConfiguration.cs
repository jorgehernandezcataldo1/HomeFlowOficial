using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class ArrendatarioConfiguration : IEntityTypeConfiguration<Arrendatario>
    {
        public void Configure(EntityTypeBuilder<Arrendatario> builder)
        {
            builder.Property(a => a.IngresoLiquido).HasColumnType("decimal(12,2)");

            builder.HasOne(a => a.Persona)
                   .WithOne(pe => pe.Arrendatario)
                   .HasForeignKey<Arrendatario>(a => a.PersonaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.PersonaId).IsUnique();
        }
    }
}
