using Microsoft.AspNetCore.Mvc;
using PlayaAutos.Web.Models;
using PlayaAutos.Web.Services;

namespace PlayaAutos.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Token") != null)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/Auth/login", new
            {
                email = model.Email,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Email o contraseña incorrectos.");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result == null)
            {
                ModelState.AddModelError("", "Error al procesar la respuesta.");
                return View(model);
            }

            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Session.SetString("Nombre", result.Nombre);
            HttpContext.Session.SetString("Rol", result.Rol);
            HttpContext.Session.SetInt32("UsuarioId", result.UsuarioId);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
