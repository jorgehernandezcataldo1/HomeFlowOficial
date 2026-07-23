using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class PropietarioConfiguration : IEntityTypeConfiguration<Propietario>
    {
        public void Configure(EntityTypeBuilder<Propietario> builder)
        {
            builder.HasOne(p => p.Persona)
                   .WithOne(pe => pe.Propietario)
                   .HasForeignKey<Propietario>(p => p.PersonaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.PersonaId).IsUnique();
        }
    }
}
