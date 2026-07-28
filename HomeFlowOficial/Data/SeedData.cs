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

            if (!await context.Empresas.AnyAsync())
            {
                context.Empresas.Add(new Empresa
                {
                    RazonSocial = "HomeFlow",
                    Rut = "76.000.000-0",
                    Correo = "contacto@homeflow.cl",
                    Activa = true
                });

                await context.SaveChangesAsync();
            }

            // EstadoInmueble y TipoOperacion son enum: sus valores viven en el código
            // (Models/Enums), no se siembran filas para ellos.

            // CategoriaInmueble y Caracteristica todavía no existen como entidades.
            // Si más adelante quieres características dinámicas por tipo de inmueble
            // (ej. "Piscina", "Terraza" configurables sin tocar código), avísame y
            // armamos esas clases + su propio DbSet; por ahora TipoInmueble ya cubre
            // la clasificación básica (Casa, Depto, Bodega, etc.).

            await context.SaveChangesAsync();

            // Checklists base: uno para Propietario y uno para Inmueble, que es el
            // primer flujo que estás armando. Se pueden editar/ampliar después desde
            // la propia tabla, sin volver a tocar código.
            if (!await context.ChecklistPlantillas.AnyAsync(p => p.TipoEntidad == TipoEntidadChecklist.Propietario))
            {
                context.ChecklistPlantillas.Add(new HomeFlowOficial.Models.Checklist.ChecklistPlantilla
                {
                    Nombre = "Checklist Propietario",
                    TipoEntidad = TipoEntidadChecklist.Propietario,
                    Items = new List<HomeFlowOficial.Models.Checklist.ChecklistItem>
                    {
                        new() { Descripcion = "Cédula de identidad vigente", Orden = 1, Obligatorio = true },
                        new() { Descripcion = "Verificación de dominio vigente (Conservador de Bienes Raíces)", Orden = 2, Obligatorio = true },
                        new() { Descripcion = "Sin deudas de contribuciones", Orden = 3, Obligatorio = true },
                        new() { Descripcion = "Datos de contacto verificados", Orden = 4, Obligatorio = false },
                    }
                });
            }

            if (!await context.ChecklistPlantillas.AnyAsync(p => p.TipoEntidad == TipoEntidadChecklist.Inmueble))
            {
                context.ChecklistPlantillas.Add(new HomeFlowOficial.Models.Checklist.ChecklistPlantilla
                {
                    Nombre = "Checklist Inmueble",
                    TipoEntidad = TipoEntidadChecklist.Inmueble,
                    Items = new List<HomeFlowOficial.Models.Checklist.ChecklistItem>
                    {
                        new() { Descripcion = "Fotografías del inmueble", Orden = 1, Obligatorio = true },
                        new() { Descripcion = "Gastos comunes al día", Orden = 2, Obligatorio = true },
                        new() { Descripcion = "Estado de instalaciones (agua, luz, gas) revisado", Orden = 3, Obligatorio = true },
                        new() { Descripcion = "Llaves / acceso disponible para visitas", Orden = 4, Obligatorio = false },
                    }
                });
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
