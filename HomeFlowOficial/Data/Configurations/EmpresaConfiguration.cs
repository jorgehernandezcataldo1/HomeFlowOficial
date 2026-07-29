using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HomeFlowOficial.Models;

namespace HomeFlowOficial.Data.Configurations
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Rut)
                .IsUnique();

            builder.Property(x => x.RazonSocial)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Rut)
                .HasMaxLength(12)
                .IsRequired();

            builder.Property(x => x.Correo)
                .HasMaxLength(150);

            builder.Property(x => x.Telefono)
                .HasMaxLength(20);
        }
    }
}
