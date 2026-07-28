using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Catalogos;
using HomeFlowOficial.Models.Checklist;
using HomeFlowOficial.Models.Enums;
using HomeFlowOficial.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace HomeFlowOficial.Data
{
    // Hereda de IdentityDbContext para que las tablas de usuarios/roles convivan
    // en la misma base y el mismo mecanismo de migraciones que el resto del dominio.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Persona> Personas => Set<Persona>();
        public DbSet<Empresa> Empresas => Set<Empresa>();
        public DbSet<Propietario> Propietarios => Set<Propietario>();
        public DbSet<Arrendatario> Arrendatarios => Set<Arrendatario>();
        public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
        public DbSet<InmuebleCercania> InmuebleCercanias => Set<InmuebleCercania>();

        public DbSet<EstadoCivil> EstadosCiviles => Set<EstadoCivil>();
        public DbSet<TipoInmueble> TiposInmueble => Set<TipoInmueble>();
        public DbSet<TipoCercania> TiposCercania => Set<TipoCercania>();

        // EstadoInmueble y TipoOperacion son enum (Models/Enums), no catálogos en BD:
        // se guardan como columna int directamente en Inmueble (ver Inmueble.Estado / Inmueble.TipoOperacion).
        // Un DbSet<T> exige que T sea class, por eso no pueden declararse acá.

        //public DbSet<CategoriaInmueble> CategoriasInmueble => Set<CategoriaInmueble>();

        //public DbSet<Caracteristica> Caracteristicas => Set<Caracteristica>();

        //public DbSet<InmuebleCaracteristica> InmuebleCaracteristicas => Set<InmuebleCaracteristica>();

        public DbSet<PersonaRol> PersonaRoles => Set<PersonaRol>();

        public DbSet<ChecklistPlantilla> ChecklistPlantillas => Set<ChecklistPlantilla>();
        public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();
        public DbSet<ChecklistRespuesta> ChecklistRespuestas => Set<ChecklistRespuesta>();
        public DbSet<ChecklistRespuestaItem> ChecklistRespuestaItems => Set<ChecklistRespuestaItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Carga automáticamente todas las clases IEntityTypeConfiguration<T>
            // del ensamblado, así cada entidad grande vive en su propio archivo
            // (Data/Configurations/*) en vez de amontonar todo aquí.
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
