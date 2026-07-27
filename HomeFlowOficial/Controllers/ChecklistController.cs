using System.Security.Claims;
using HomeFlowOficial.Data;
using HomeFlowOficial.Models.Checklist;
using HomeFlowOficial.Models.Enums;
using HomeFlowOficial.Models.ViewModels.Checklist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeFlowOficial.Controllers
{
    // Un único controlador para los 3 tipos de checklist (Propietario/Inmueble/Arrendatario).
    // La plantilla activa se busca por TipoEntidad, así que agregar un checklist nuevo
    // para otra entidad de negocio no requiere tocar este código, solo sembrar la plantilla.
    [Authorize]
    public class ChecklistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Checklist/Obtener?tipoEntidad=2&entidadId=5
        [HttpGet]
        public async Task<IActionResult> Obtener(TipoEntidadChecklist tipoEntidad, int entidadId)
        {
            var plantilla = await _context.ChecklistPlantillas
                .AsNoTracking()
                .Include(p => p.Items)
                .Where(p => p.TipoEntidad == tipoEntidad && p.Activo)
                .OrderByDescending(p => p.Version)
                .FirstOrDefaultAsync();

            if (plantilla is null)
                return NotFound(new { exito = false, mensaje = "No hay un checklist configurado para este tipo todavía." });

            // Última respuesta guardada para esta entidad puntual (si ya se completó antes,
            // se precarga para poder revisarla/editarla en vez de partir de cero).
            var respuestaPrevia = await _context.ChecklistRespuestas
                .AsNoTracking()
                .Include(r => r.Respuestas)
                .Where(r => r.ChecklistPlantillaId == plantilla.Id
                         && r.TipoEntidad == tipoEntidad
                         && r.EntidadId == entidadId)
                .OrderByDescending(r => r.FechaRealizacion)
                .FirstOrDefaultAsync();

            var resultado = new ChecklistCargaViewModel
            {
                ChecklistPlantillaId = plantilla.Id,
                Nombre = plantilla.Nombre,
                TipoEntidad = tipoEntidad,
                EntidadId = entidadId,
                Aprobado = respuestaPrevia?.Aprobado ?? false,
                Items = plantilla.Items
                    .OrderBy(i => i.Orden)
                    .Select(i => new ChecklistItemCargaViewModel
                    {
                        Id = i.Id,
                        Descripcion = i.Descripcion,
                        Obligatorio = i.Obligatorio,
                        Cumple = respuestaPrevia != null && respuestaPrevia.Respuestas.Any(r => r.ChecklistItemId == i.Id && r.Cumple),
                        Observacion = respuestaPrevia?.Respuestas.FirstOrDefault(r => r.ChecklistItemId == i.Id)?.Observacion
                    })
                    .ToList()
            };

            return Json(resultado);
        }

        // POST: /Checklist/Guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar([FromBody] ChecklistGuardarDto modelo)
        {
            if (modelo is null || !ModelState.IsValid)
                return BadRequest(new { exito = false, mensaje = "Datos de checklist inválidos." });

            var plantilla = await _context.ChecklistPlantillas
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == modelo.ChecklistPlantillaId && p.TipoEntidad == modelo.TipoEntidad);

            if (plantilla is null)
                return NotFound(new { exito = false, mensaje = "El checklist ya no existe." });

            // Nunca confiamos en los Id que manda el cliente sin validarlos contra la plantilla real.
            var itemsPlantilla = plantilla.Items.ToDictionary(i => i.Id);
            if (modelo.Items.Any(i => !itemsPlantilla.ContainsKey(i.ChecklistItemId)))
                return BadRequest(new { exito = false, mensaje = "El checklist contiene un ítem inválido." });

            var obligatoriosCumplidos = itemsPlantilla.Values
                .Where(i => i.Obligatorio)
                .All(i => modelo.Items.Any(r => r.ChecklistItemId == i.Id && r.Cumple));

            var respuesta = new ChecklistRespuesta
            {
                ChecklistPlantillaId = plantilla.Id,
                TipoEntidad = modelo.TipoEntidad,
                EntidadId = modelo.EntidadId,
                CorredorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                Aprobado = obligatoriosCumplidos
            };

            foreach (var item in modelo.Items)
            {
                respuesta.Respuestas.Add(new ChecklistRespuestaItem
                {
                    ChecklistItemId = item.ChecklistItemId,
                    Cumple = item.Cumple,
                    Observacion = item.Observacion?.Trim()
                });
            }

            _context.ChecklistRespuestas.Add(respuesta);

            // Refleja el resultado en el flag de la entidad de negocio correspondiente,
            // que es lo que usan las listas (Propietarios/Inmuebles) para mostrar el badge.
            switch (modelo.TipoEntidad)
            {
                case TipoEntidadChecklist.Propietario:
                    var propietario = await _context.Propietarios.FindAsync(modelo.EntidadId);
                    if (propietario is not null) propietario.ChecklistAprobado = obligatoriosCumplidos;
                    break;

                case TipoEntidadChecklist.Inmueble:
                    var inmueble = await _context.Inmuebles.FindAsync(modelo.EntidadId);
                    if (inmueble is not null) inmueble.ChecklistAprobado = obligatoriosCumplidos;
                    break;

                case TipoEntidadChecklist.Arrendatario:
                    var arrendatario = await _context.Arrendatarios.FindAsync(modelo.EntidadId);
                    if (arrendatario is not null) arrendatario.ChecklistAprobado = obligatoriosCumplidos;
                    break;
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                exito = true,
                mensaje = obligatoriosCumplidos ? "Checklist guardado y aprobado." : "Checklist guardado (quedan ítems obligatorios pendientes).",
                aprobado = obligatoriosCumplidos
            });
        }
    }
}
