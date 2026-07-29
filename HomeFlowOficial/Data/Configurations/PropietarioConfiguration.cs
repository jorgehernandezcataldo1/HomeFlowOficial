using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class PropietarioConfiguration : IEntityTypeConfiguration<Propietario>
    {
        public void Configure(EntityTypeBuilder<Propietario> builder)
        {
            builder.ToTable("Propietarios");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Persona)
                .WithOne(x => x.Propietario)
                .HasForeignKey<Propietario>(x => x.PersonaId);

            builder.HasOne(x => x.Corredor)
                .WithMany(x => x.Propietarios)
                .HasForeignKey(x => x.CorredorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.CorredorId);
        }
    }
}
