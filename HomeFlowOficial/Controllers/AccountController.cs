using HomeFlowOficial.Models.Identity;
using HomeFlowOficial.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlowOficial.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

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

            // PasswordSignInAsync valida el hash de forma segura y maneja bloqueo por intentos fallidos.
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

        // Solo un Admin puede crear nuevos usuarios/corredores del sistema.
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

            var usuario = new ApplicationUser
            {
                UserName = modelo.Correo,
                Email = modelo.Correo,
                NombreCompleto = modelo.NombreCompleto,
                EmailConfirmed = true
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, modelo.Rol);
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
