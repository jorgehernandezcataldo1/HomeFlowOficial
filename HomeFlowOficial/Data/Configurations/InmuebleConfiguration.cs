using HomeFlowOficial.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeFlowOficial.Data.Configurations
{
    public class InmuebleConfiguration : IEntityTypeConfiguration<Inmueble>
    {
        public void Configure(EntityTypeBuilder<Inmueble> builder)
        {
            builder.Property(i => i.Precio).HasColumnType("decimal(12,2)");
            builder.Property(i => i.GastoComun).HasColumnType("decimal(10,2)");
            builder.Property(i => i.GastoAguaEstimado).HasColumnType("decimal(10,2)");
            builder.Property(i => i.GastoLuzEstimado).HasColumnType("decimal(10,2)");
            builder.Property(i => i.GastoGasEstimado).HasColumnType("decimal(10,2)");
            builder.Property(i => i.MontoContribuciones).HasColumnType("decimal(10,2)");
            builder.Property(i => i.SuperficieUtilM2).HasColumnType("decimal(8,2)");
            builder.Property(i => i.SuperficieTotalM2).HasColumnType("decimal(8,2)");

            builder.HasOne(i => i.Propietario)
                   .WithMany(p => p.Inmuebles)
                   .HasForeignKey(i => i.PropietarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.TipoInmueble)
                   .WithMany()
                   .HasForeignKey(i => i.TipoInmuebleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Acelera los filtros típicos del listado / matching: por comuna, estado y operación.
            builder.HasIndex(i => new { i.Comuna, i.Estado, i.TipoOperacion });
        }
    }
}
