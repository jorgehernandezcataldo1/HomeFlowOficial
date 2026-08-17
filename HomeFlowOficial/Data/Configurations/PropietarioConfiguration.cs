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

            // Antes WithOne (1 persona = 1 propietario global). Ahora N:1: una persona
            // puede tener muchas relaciones de Propietario, una por corredor.
            builder.HasOne(x => x.Persona)
                .WithMany(x => x.RelacionesPropietario)
                .HasForeignKey(x => x.PersonaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Corredor)
                .WithMany(x => x.Propietarios)
                .HasForeignKey(x => x.CorredorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un corredor no puede tener 2 veces a la misma persona en su cartera.
            builder.HasIndex(x => new { x.PersonaId, x.CorredorId }).IsUnique();
        }
    }
}
