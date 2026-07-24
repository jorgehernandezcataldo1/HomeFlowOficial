using HomeFlowOficial.Models.Catalogos;
using HomeFlowOficial.Models.Enums;
using HomeFlowOficial.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace HomeFlowOficial.Data
{
    // Se ejecuta al levantar la app: aplica migraciones pendientes y siembra
    // catálogos base + un usuario Admin inicial para poder entrar por primera vez.
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider servicios)
        {
            var context = servicios.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            var roleManager = servicios.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Corredor" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new IdentityRole(rol));
            }

            if (!await context.EstadosCiviles.AnyAsync())
            {
                context.EstadosCiviles.AddRange(
                    new EstadoCivil { Nombre = "Soltero/a" },
                    new EstadoCivil { Nombre = "Casado/a" },
                    new EstadoCivil { Nombre = "Divorciado/a" },
                    new EstadoCivil { Nombre = "Viudo/a" },
                    new EstadoCivil { Nombre = "Conviviente Civil" });
            }

            if (!await context.TiposInmueble.AnyAsync())
            {
                context.TiposInmueble.AddRange(
                    new TipoInmueble { Nombre = "Casa" },
                    new TipoInmueble { Nombre = "Departamento" },
                    new TipoInmueble { Nombre = "Oficina" },
                    new TipoInmueble { Nombre = "Bodega" },
                    new TipoInmueble { Nombre = "Local Comercial" },
                    new TipoInmueble { Nombre = "Parcela" },
                    new TipoInmueble { Nombre = "Sitio" });
            }

            if (!await context.TiposCercania.AnyAsync())
            {
                context.TiposCercania.AddRange(
                    new TipoCercania { Nombre = "Metro" },
                    new TipoCercania { Nombre = "Colegio" },
                    new TipoCercania { Nombre = "Supermercado" },
                    new TipoCercania { Nombre = "Parque" },
                    new TipoCercania { Nombre = "Hospital / Consultorio" },
                    new TipoCercania { Nombre = "Locomoción colectiva" });
            }

            if (!await context.EstadosInmueble.AnyAsync())
            {
                context.EstadosInmueble.AddRange(

                    new EstadoInmueble { Nombre = "Pendiente" },

                    new EstadoInmueble { Nombre = "En trámite" },

                    new EstadoInmueble { Nombre = "Publicado" },

                    new EstadoInmueble { Nombre = "Reservado" },

                    new EstadoInmueble { Nombre = "Arrendado" },

                    new EstadoInmueble { Nombre = "Vendido" }

                );
            }

            if (!await context.TiposOperacion.AnyAsync())
            {
                context.TiposOperacion.AddRange(

                    new TipoOperacion { Nombre = "Arriendo" },

                    new TipoOperacion { Nombre = "Venta" }

                );
            }

            if (!await context.CategoriasInmueble.AnyAsync())
            {
                context.CategoriasInmueble.AddRange(

                    new CategoriaInmueble { Nombre = "Habitacional" },

                    new CategoriaInmueble { Nombre = "Comercial" },

                    new CategoriaInmueble { Nombre = "Industrial" },

                    new CategoriaInmueble { Nombre = "Agrícola" }

                );
            }

            if (!await context.Caracteristicas.AnyAsync())
            {
                context.Caracteristicas.AddRange(

                    new Caracteristica { Nombre = "Dormitorios", TipoDato = "Numero" },

                    new Caracteristica { Nombre = "Baños", TipoDato = "Numero" },

                    new Caracteristica { Nombre = "Bodega", TipoDato = "Boolean" },

                    new Caracteristica { Nombre = "Estacionamiento", TipoDato = "Boolean" },

                    new Caracteristica { Nombre = "Terraza", TipoDato = "Boolean" },

                    new Caracteristica { Nombre = "Piscina", TipoDato = "Boolean" },

                    new Caracteristica { Nombre = "Metros Construidos", TipoDato = "Decimal" },

                    new Caracteristica { Nombre = "Metros Totales", TipoDato = "Decimal" }

                );
            }

            await context.SaveChangesAsync();

            var userManager = servicios.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByEmailAsync("admin@homeflow.cl") is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@homeflow.cl",
                    Email = "admin@homeflow.cl",
                    NombreCompleto = "Administrador",
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(admin, "CambiarClave123!");
                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
