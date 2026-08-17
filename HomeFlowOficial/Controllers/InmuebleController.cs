using HomeFlowOficial.Data;
using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Enums;
using HomeFlowOficial.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeFlowOficial.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InmuebleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Inmueble
        public async Task<IActionResult> Index()
        {
            var inmuebles = await _context.Inmuebles
                .AsNoTracking()
                .OrderByDescending(i => i.FechaIngreso)
                .Select(i => new InmuebleListItemViewModel
                {
                    Id = i.Id,
                    Direccion = i.Direccion,
                    Comuna = i.Comuna,
                    TipoInmuebleNombre = i.TipoInmueble.Nombre,
                    PropietarioNombre = i.Propietario.Persona.Nombres + " " + i.Propietario.Persona.ApellidoPaterno,
                    TipoOperacion = i.TipoOperacion,
                    Estado = i.Estado,
                    Precio = i.Precio,
                    ChecklistAprobado = i.ChecklistAprobado
                })
                .ToListAsync();

            await CargarListasSelect();

            return View(inmuebles);
        }

        // POST: /Inmueble/Crear
        // Se invoca vía $.ajax desde el modal del Index y responde en JSON,
        // tanto para errores de validación como para el caso de éxito.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(InmuebleViewModel modelo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { exito = false, errores = ObtenerErroresDeModelState() });

            var propietario = await _context.Propietarios
                .Include(p => p.Persona)
                .FirstOrDefaultAsync(p => p.Id == modelo.PropietarioId);

            if (propietario is null)
            {
                return BadRequest(new
                {
                    exito = false,
                    errores = new Dictionary<string, string[]>
                    {
                        [nameof(modelo.PropietarioId)] = new[] { "El propietario seleccionado no existe." }
                    }
                });
            }

            var tipoInmuebleValido = await _context.TiposInmueble.AnyAsync(t => t.Id == modelo.TipoInmuebleId && t.Activo);
            if (!tipoInmuebleValido)
            {
                return BadRequest(new
                {
                    exito = false,
                    errores = new Dictionary<string, string[]>
                    {
                        [nameof(modelo.TipoInmuebleId)] = new[] { "Selecciona un tipo de inmueble válido." }
                    }
                });
            }

            // Exclusividad: misma persona (RUT) + misma dirección/comuna, en CUALQUIER
            // corredor/empresa del sistema. Si ya existe con exclusividad, se bloquea.
            var direccionNormalizada = modelo.Direccion.Trim().ToUpperInvariant();
            var comunaNormalizada = modelo.Comuna.Trim().ToUpperInvariant();

            var bloqueadoPorExclusividad = await _context.Inmuebles
                .Include(i => i.Propietario).ThenInclude(p => p.Persona)
                .AnyAsync(i =>
                    i.Propietario.Persona.Rut == propietario.Persona.Rut &&
                    i.Direccion.ToUpper() == direccionNormalizada &&
                    i.Comuna.ToUpper() == comunaNormalizada &&
                    i.TieneExclusividad);

            if (bloqueadoPorExclusividad)
            {
                return BadRequest(new
                {
                    exito = false,
                    mensaje = "Esta propiedad ya tiene un acuerdo de exclusividad vigente con otro corredor."
                });
            }

            var inmueble = new Inmueble
            {
                PropietarioId = modelo.PropietarioId,
                TipoInmuebleId = modelo.TipoInmuebleId,
                TipoOperacion = modelo.TipoOperacion,
                Estado = EstadoInmueble.Pendiente,
                Direccion = modelo.Direccion.Trim(),
                Comuna = modelo.Comuna.Trim(),
                Region = string.IsNullOrWhiteSpace(modelo.Region) ? "Metropolitana" : modelo.Region.Trim(),
                Piso = modelo.Piso,
                Torre = modelo.Torre?.Trim(),
                NumeroDepto = modelo.NumeroDepto?.Trim(),
                Habitaciones = modelo.Habitaciones,
                Banos = modelo.Banos,
                SuperficieUtilM2 = modelo.SuperficieUtilM2,
                SuperficieTotalM2 = modelo.SuperficieTotalM2,
                Estacionamientos = modelo.Estacionamientos,
                TieneBodega = modelo.TieneBodega,
                GastoComun = modelo.GastoComun,
                GastoAguaEstimado = modelo.GastoAguaEstimado,
                GastoLuzEstimado = modelo.GastoLuzEstimado,
                GastoGasEstimado = modelo.GastoGasEstimado,
                PagaContribuciones = modelo.PagaContribuciones,
                MontoContribuciones = modelo.MontoContribuciones,
                Amoblado = modelo.Amoblado,
                Equipado = modelo.Equipado,
                AceptaMascotas = modelo.AceptaMascotas,
                Precio = modelo.Precio,
                Descripcion = modelo.Descripcion?.Trim(),
                TieneExclusividad = modelo.TieneExclusividad
            };

            _context.Inmuebles.Add(inmueble);
            await _context.SaveChangesAsync();

            return Json(new { exito = true, mensaje = "Inmueble registrado correctamente.", id = inmueble.Id });
        }

        // GET: /Inmueble/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var inmueble = await _context.Inmuebles
                .AsNoTracking()
                .Include(i => i.TipoInmueble)
                .Include(i => i.Propietario)
                    .ThenInclude(p => p.Persona)
                .Include(i => i.Cercanias)
                    .ThenInclude(c => c.TipoCercania)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inmueble is null)
                return NotFound();

            return View(inmueble);
        }

        private async Task CargarListasSelect()
        {
            var corredorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var propietarios = await _context.Propietarios
                .AsNoTracking()
                .Where(p => p.CorredorId == corredorId) // <- solo los suyos
                .Select(p => new { p.Id, Nombre = p.Persona.Nombres + " " + p.Persona.ApellidoPaterno + " (" + p.Persona.Rut + ")" })
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Propietarios = new SelectList(propietarios, "Id", "Nombre");

            ViewBag.TiposInmueble = new SelectList(
                await _context.TiposInmueble.AsNoTracking().Where(t => t.Activo).OrderBy(t => t.Nombre).ToListAsync(),
                "Id", "Nombre");
        }

        private Dictionary<string, string[]> ObtenerErroresDeModelState()
        {
            return ModelState
                .Where(par => par.Value?.Errors.Count > 0)
                .ToDictionary(
                    par => par.Key,
                    par => par.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
        }
    }
}
