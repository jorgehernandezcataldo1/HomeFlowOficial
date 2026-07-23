using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            // Un RUT no puede repetirse en el sistema.
            builder.HasIndex(p => p.Rut).IsUnique();

            builder.HasOne(p => p.EstadoCivil)
                   .WithMany()
                   .HasForeignKey(p => p.EstadoCivilId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
