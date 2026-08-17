using HomeFlowOficial.Data;
using HomeFlowOficial.Models;
using HomeFlowOficial.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeFlowOficial.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropietariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Propietarios
        public async Task<IActionResult> Index()
        {
            var corredorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var propietarios = await _context.Propietarios
                .AsNoTracking()
                .Where(x => x.CorredorId == corredorId)
                .Select(p => new PropietarioListItemViewModel
                {
                    Id = p.Id,
                    Rut = p.Persona.Rut,
                    NombreCompleto = p.Persona.Nombres + " " + p.Persona.ApellidoPaterno,
                    Correo = p.Persona.Correo,
                    Telefono = p.Persona.Telefono,
                    CantidadInmuebles = p.Inmuebles.Count,
                    ChecklistAprobado = p.ChecklistAprobado
                })
                .ToListAsync();

            ViewBag.EstadosCiviles = new SelectList(
                await _context.EstadosCiviles.AsNoTracking().Where(e => e.Activo).OrderBy(e => e.Nombre).ToListAsync(),
                "Id", "Nombre");

            return View(propietarios);
        }

        // GET: /Propietarios/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var corredorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var propietario = await _context.Propietarios
                .AsNoTracking()
                .Include(p => p.Persona)
                    .ThenInclude(persona => persona.EstadoCivil)
                .Include(p => p.Inmuebles)
                .FirstOrDefaultAsync(p => p.Id == id && p.CorredorId == corredorId);

            if (propietario is null)
                return NotFound();

            return View(propietario);
        }

        // POST: /Propietarios/Crear
        // Se invoca vía fetch() desde el modal del Index y responde en JSON,
        // tanto para errores de validación como para el caso de éxito.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(PropietarioViewModel modelo)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { exito = false, errores = ObtenerErroresDeModelState() });

            var corredorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(corredorId))
                return Unauthorized();

            var rutNormalizado = modelo.Rut.Replace(".", "").Replace("-", "").Trim().ToUpperInvariant();

            // La Persona es única globalmente por RUT, pero puede tener varias
            // relaciones de Propietario (una por corredor que la gestiona).
            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.Rut == rutNormalizado);

            if (persona is not null)
            {
                var yaEsSuPropietario = await _context.Propietarios
                    .AnyAsync(x => x.PersonaId == persona.Id && x.CorredorId == corredorId);

                if (yaEsSuPropietario)
                {
                    return BadRequest(new
                    {
                        exito = false,
                        errores = new Dictionary<string, string[]>
                        {
                            [nameof(modelo.Rut)] = new[] { "Ya tienes a esta persona registrada como propietario." }
                        }
                    });
                }

                // Refresca datos de contacto por si vienen distintos (opcional, a tu criterio).
                persona.Nombres = modelo.Nombres.Trim();
                persona.ApellidoPaterno = modelo.ApellidoPaterno.Trim();
                persona.ApellidoMaterno = modelo.ApellidoMaterno?.Trim();
                persona.Correo = modelo.Correo.Trim();
                persona.Telefono = modelo.Telefono;
                persona.Direccion = modelo.Direccion.Trim();
                persona.EstadoCivilId = modelo.EstadoCivilId;
            }
            else
            {
                persona = new Persona
                {
                    Rut = rutNormalizado,
                    Nombres = modelo.Nombres.Trim(),
                    ApellidoPaterno = modelo.ApellidoPaterno.Trim(),
                    ApellidoMaterno = modelo.ApellidoMaterno?.Trim(),
                    Correo = modelo.Correo.Trim(),
                    Telefono = modelo.Telefono,
                    Direccion = modelo.Direccion.Trim(),
                    EstadoCivilId = modelo.EstadoCivilId
                };
            }

            var propietario = new Propietario
            {
                Persona = persona,
                CorredorId = corredorId,
                FechaIngreso = DateTime.UtcNow,
                ChecklistAprobado = false
            };

            _context.Propietarios.Add(propietario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Carrera: dos corredores creando la misma persona nueva al mismo tiempo.
                return BadRequest(new { exito = false, mensaje = "Ocurrió un conflicto al guardar, intenta nuevamente." });
            }

            return Json(new { exito = true, mensaje = "Propietario registrado correctamente.", id = propietario.Id });
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
