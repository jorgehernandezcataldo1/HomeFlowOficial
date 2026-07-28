using HomeFlowOficial.Data;
using HomeFlowOficial.Models.Checklist;
using HomeFlowOficial.Models.ViewModels.Checklist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeFlowOficial.Controllers
{
    // Mantenedor de plantillas. Solo Admin: qué se exige en cada checklist es
    // decisión de negocio, no de cualquier corredor.
    //
    // Nunca se edita una plantilla activa "in place": ChecklistRespuestaItem
    // referencia ChecklistItem con OnDelete=Restrict, así que tocar items ya
    // respondidos rompería el historial. "Editar" crea version+1 y desactiva
    // la anterior — para eso ya existía el campo Version en el modelo.
    [Authorize(Roles = "Admin")]
    public class ChecklistAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var plantillas = await _context.ChecklistPlantillas
                .AsNoTracking()
                .OrderBy(p => p.TipoEntidad).ThenByDescending(p => p.Version)
                .Select(p => new ChecklistPlantillaAdminListItemViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    TipoEntidad = p.TipoEntidad,
                    Version = p.Version,
                    Activo = p.Activo,
                    CantidadItems = p.Items.Count,
                    CantidadRespuestasHistoricas = _context.ChecklistRespuestas.Count(r => r.ChecklistPlantillaId == p.Id)
                })
                .ToListAsync();

            return View(plantillas);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var plantilla = await _context.ChecklistPlantillas
                .AsNoTracking()
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plantilla is null) return NotFound();

            return Json(new ChecklistPlantillaAdminDetalleViewModel
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre,
                TipoEntidad = plantilla.TipoEntidad,
                Version = plantilla.Version,
                Items = plantilla.Items
                    .OrderBy(i => i.Orden)
                    .Select(i => new ChecklistItemAdminViewModel
                    {
                        Id = i.Id,
                        Descripcion = i.Descripcion,
                        Orden = i.Orden,
                        Obligatorio = i.Obligatorio
                    })
                    .ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar([FromBody] ChecklistPlantillaGuardarDto modelo)
        {
            if (modelo is null || string.IsNullOrWhiteSpace(modelo.Nombre))
                return BadRequest(new { exito = false, mensaje = "El nombre de la plantilla es obligatorio." });

            if (modelo.Items is null || modelo.Items.Count == 0)
                return BadRequest(new { exito = false, mensaje = "Agrega al menos un ítem al checklist." });

            if (modelo.Items.Any(i => string.IsNullOrWhiteSpace(i.Descripcion)))
                return BadRequest(new { exito = false, mensaje = "Todos los ítems necesitan una descripción." });

            var nuevaVersion = 1;

            if (modelo.PlantillaBaseId.HasValue)
            {
                var baseExistente = await _context.ChecklistPlantillas
                    .FirstOrDefaultAsync(p => p.Id == modelo.PlantillaBaseId.Value);

                if (baseExistente is null)
                    return NotFound(new { exito = false, mensaje = "La plantilla base ya no existe." });

                nuevaVersion = baseExistente.Version + 1;
                baseExistente.Activo = false; // las respuestas históricas siguen apuntando a esta, intacta
            }
            else
            {
                // Evita 2 plantillas activas compitiendo para el mismo TipoEntidad.
                var activaActual = await _context.ChecklistPlantillas
                    .FirstOrDefaultAsync(p => p.TipoEntidad == modelo.TipoEntidad && p.Activo);
                if (activaActual is not null)
                {
                    nuevaVersion = activaActual.Version + 1;
                    activaActual.Activo = false;
                }
            }

            var nueva = new ChecklistPlantilla
            {
                Nombre = modelo.Nombre.Trim(),
                TipoEntidad = modelo.TipoEntidad,
                Version = nuevaVersion,
                Activo = true,
                Items = modelo.Items
                    .Select((i, indice) => new ChecklistItem
                    {
                        Descripcion = i.Descripcion.Trim(),
                        Orden = i.Orden > 0 ? i.Orden : indice + 1,
                        Obligatorio = i.Obligatorio
                    })
                    .ToList()
            };

            _context.ChecklistPlantillas.Add(nueva);
            await _context.SaveChangesAsync();

            return Json(new { exito = true, mensaje = "Checklist guardado correctamente.", id = nueva.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archivar(int id)
        {
            var plantilla = await _context.ChecklistPlantillas.FindAsync(id);
            if (plantilla is null) return NotFound(new { exito = false });

            plantilla.Activo = false;
            await _context.SaveChangesAsync();

            return Json(new { exito = true, mensaje = "Checklist archivado." });
        }
    }
}