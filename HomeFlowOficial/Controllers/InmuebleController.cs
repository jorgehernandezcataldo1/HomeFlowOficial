using HomeFlowOficial.Data;
using HomeFlowOficial.Models;
using HomeFlowOficial.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace HomeFlowOficial.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InmuebleController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var inmuebles = await _context.Inmuebles
                .AsNoTracking()
                .OrderByDescending(i => i.FechaIngreso)
                .Select(i => new InmuebleViewModel
                {
                    Id = i.Id,
                    //TipoInmueble = i.TipoInmueble,

                }).ToListAsync();

            return View(inmuebles);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Crear(InmuebleViewModel modelo)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { exito = false, errores = ObtenerErroresDeModelState() });

        //    var direccion = modelo.Direccion.Replace(".", "").Replace("-", "").Trim().ToUpperInvariant();

        //    var yaExiste = await _context.Inmuebles.AnyAsync(p => p.Direccion == direccion);
        //    if (yaExiste)
        //    {
        //        return BadRequest(new
        //        {
        //            exito = false,
        //            errores = new Dictionary<string, string[]>
        //            {
        //                [nameof(modelo.Direccion)] = new[] { "Ya existe un Inmueble creado con esta direccion." }
        //            }
        //        });
        //    }

        //    var inmueble = new Inmueble
        //    {
        //        ,
        //        Nombres = modelo.Nombres.Trim(),
        //        ApellidoPaterno = modelo.ApellidoPaterno.Trim(),
        //        ApellidoMaterno = modelo.ApellidoMaterno?.Trim(),
        //        Correo = modelo.Correo.Trim(),
        //        Telefono = modelo.Telefono,
        //        Direccion = modelo.Direccion.Trim(),
        //        EstadoCivilId = modelo.EstadoCivilId
        //    };

        //    var propietario = new Propietario { Persona = persona };

        //    // EF Core parametriza automáticamente esta inserción (sin concatenar SQL a mano),
        //    // lo que evita inyección SQL por diseño.
        //    _context.Propietarios.Add(propietario);
        //    await _context.SaveChangesAsync();

        //    return Json(new { exito = true, mensaje = "Propietario registrado correctamente.", id = propietario.Id });
        //}

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
