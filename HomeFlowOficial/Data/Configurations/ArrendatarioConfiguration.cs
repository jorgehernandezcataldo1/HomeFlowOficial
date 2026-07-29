using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class ArrendatarioConfiguration : IEntityTypeConfiguration<Arrendatario>
    {
        public void Configure(EntityTypeBuilder<Arrendatario> builder)
        {
            builder.ToTable("Arrendatarios");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona)
                .WithOne(x => x.Arrendatario)
                .HasForeignKey<Arrendatario>(x => x.PersonaId);

            builder.HasOne(x => x.Corredor)
                .WithMany(x => x.Arrendatarios)
                .HasForeignKey(x => x.CorredorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IngresoLiquido)
                .HasPrecision(18, 2);

            builder.HasIndex(x => x.CorredorId);
        }
    }
}
