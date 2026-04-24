using Microsoft.AspNetCore.Mvc;

namespace PlayaAutos.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Token") == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Rol = HttpContext.Session.GetString("Rol");

            return View();
        }
    }
}