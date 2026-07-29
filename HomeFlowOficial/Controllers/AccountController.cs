using HomeFlowOficial.Data;
using HomeFlowOficial.Models;
using HomeFlowOficial.Models.Identity;
using HomeFlowOficial.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeFlowOficial.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        private static readonly string[] RolesPermitidos = { "Admin", "Corredor" };

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        // ---------- Login ----------

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(modelo);

            // Chequeo de cuenta desactivada ANTES de validar la contraseña: si no,
            // el mensaje de error filtraría si el correo existe o no según el timing.
            var usuarioExistente = await _userManager.FindByEmailAsync(modelo.Correo);
            if (usuarioExistente is not null && !usuarioExistente.Activo)
            {
                ModelState.AddModelError(string.Empty, "Tu cuenta está desactivada. Contacta al administrador de tu empresa.");
                return View(modelo);
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                modelo.Correo, modelo.Password, modelo.Recordar, lockoutOnFailure: true);

            if (resultado.Succeeded)
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")!);

            if (resultado.IsLockedOut)
                ModelState.AddModelError(string.Empty, "Cuenta bloqueada temporalmente por intentos fallidos.");
            else
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");

            return View(modelo);
        }

        // ---------- Alta de empresa (público, primer ingreso al sistema) ----------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegistrarEmpresa() => View(new EmpresaRegistroViewModel());

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarEmpresa(EmpresaRegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var rutNormalizado = modelo.Rut.Replace(".", "").Replace("-", "").Trim().ToUpperInvariant();

            if (await _context.Empresas.AnyAsync(e => e.Rut == rutNormalizado))
            {
                ModelState.AddModelError(nameof(modelo.Rut), "Ya existe una empresa registrada con este RUT.");
                return View(modelo);
            }

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var empresa = new Empresa
                {
                    RazonSocial = modelo.RazonSocial.Trim(),
                    Rut = rutNormalizado,
                    Correo = modelo.Correo?.Trim(),
                    Telefono = modelo.Telefono?.Trim(),
                    Activa = true
                };

                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync(); // necesitamos empresa.Id antes de crear el admin

                var admin = new ApplicationUser
                {
                    UserName = modelo.CorreoAdmin.Trim(),
                    Email = modelo.CorreoAdmin.Trim(),
                    NombreCompleto = modelo.NombreCompletoAdmin.Trim(),
                    EmpresaId = empresa.Id,
                    EmailConfirmed = true,
                    Activo = true
                };

                var resultado = await _userManager.CreateAsync(admin, modelo.Password);
                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    await transaccion.RollbackAsync();
                    return View(modelo);
                }

                var resultadoRol = await _userManager.AddToRoleAsync(admin, "Admin");
                if (!resultadoRol.Succeeded)
                {
                    foreach (var error in resultadoRol.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    await transaccion.RollbackAsync();
                    return View(modelo);
                }

                await transaccion.CommitAsync();

                await _signInManager.SignInAsync(admin, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {
                // Carrera: dos personas registrando la misma empresa al mismo tiempo,
                // el índice único de Rut es la última línea de defensa.
                await transaccion.RollbackAsync();
                ModelState.AddModelError(nameof(modelo.Rut), "Ya existe una empresa registrada con este RUT.");
                return View(modelo);
            }
        }

        // ---------- Alta de corredores (solo Admin, dentro de su propia empresa) ----------

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            if (!RolesPermitidos.Contains(modelo.Rol))
            {
                ModelState.AddModelError(nameof(modelo.Rol), "Rol inválido.");
                return View(modelo);
            }

            var adminActual = await _userManager.GetUserAsync(User);
            if (adminActual is null)
                return Unauthorized();

            var usuario = new ApplicationUser
            {
                UserName = modelo.Correo,
                Email = modelo.Correo,
                NombreCompleto = modelo.NombreCompleto,
                EmpresaId = adminActual.EmpresaId, // heredado del admin logueado, nunca del form
                EmailConfirmed = true,
                Activo = true
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
            if (resultado.Succeeded)
            {
                var resultadoRol = await _userManager.AddToRoleAsync(usuario, modelo.Rol);
                if (!resultadoRol.Succeeded)
                {
                    foreach (var error in resultadoRol.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(modelo);
                }

                TempData["Mensaje"] = "Usuario creado correctamente.";
                return RedirectToAction(nameof(Register));
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}