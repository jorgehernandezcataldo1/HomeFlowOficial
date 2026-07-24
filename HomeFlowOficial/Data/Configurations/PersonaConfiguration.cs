using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Personas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rut)
                .HasMaxLength(12)
                .IsRequired();

            builder.HasIndex(x => x.Rut)
                .IsUnique();

            builder.HasIndex(x => x.Correo);

            builder.Property(x => x.Nombres)
                .HasMaxLength(100);

            builder.Property(x => x.ApellidoPaterno)
                .HasMaxLength(100);

            builder.Property(x => x.ApellidoMaterno)
                .HasMaxLength(100);

            builder.Property(x => x.Correo)
                .HasMaxLength(150);

            builder.Property(x => x.Telefono)
                .HasMaxLength(20);

            builder.Property(x => x.Direccion)
                .HasMaxLength(250);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

            builder.HasOne(x => x.EstadoCivil)
                .WithMany()
                .HasForeignKey(x => x.EstadoCivilId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Propietario)
                .WithOne(x => x.Persona)
                .HasForeignKey<Propietario>(x => x.PersonaId);

            builder.HasOne(x => x.Arrendatario)
                .WithOne(x => x.Persona)
                .HasForeignKey<Arrendatario>(x => x.PersonaId);
        }
    }
}
