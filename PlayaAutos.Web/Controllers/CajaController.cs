using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayaAutos.Web.Models;
using PlayaAutos.Web.Services;
using System.Text.Json;

namespace PlayaAutos.Web.Controllers
{
    public class CajaController : Controller
    {
        private readonly ApiService _api;

        public CajaController(ApiService api)
        {
            _api = api;
        }

        // ── GET /Caja ────────────────────────────────────────────────────
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, int? tipoId)
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            desde ??= DateTime.Today.AddDays(-30);
            hasta ??= DateTime.Today;

            var queryString = $"api/Caja/movimientos?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}";
            if (tipoId.HasValue)
                queryString += $"&tipoId={tipoId}";

            var movimientos = await _api.GetAsync<List<CajaListItem>>(queryString)
                              ?? new List<CajaListItem>();

            var balance = await _api.GetAsync<BalanceViewModel>(
                $"api/Caja/balance?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}")
                ?? new BalanceViewModel();

            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;
            ViewBag.TipoId = tipoId;
            ViewBag.Balance = balance;

            return View(movimientos);
        }

        // ── GET /Caja/Crear ──────────────────────────────────────────────
        public async Task<IActionResult> Crear()
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            var vm = new CrearMovimientoViewModel();
            await CargarTiposMovimiento(vm);
            return View(vm);
        }

        // ── POST /Caja/Crear ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearMovimientoViewModel vm)
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                await CargarTiposMovimiento(vm);
                return View(vm);
            }

            var vendedorId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            var resp = await _api.PostAsync("api/Caja/movimientos", new
            {
                vm.Fecha,
                vm.TipoMovimientoId,
                vm.Descripcion,
                vm.Monto,
                vm.Comentarios,
                UsuarioRegistro = vendedorId
            });

            TempData[resp.IsSuccessStatusCode ? "Exito" : "Error"] = resp.IsSuccessStatusCode
                ? "Movimiento registrado correctamente."
                : "Error al registrar el movimiento.";

            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ──────────────────────────────────────────────────────
        private bool EstaAutenticado() =>
            HttpContext.Session.GetString("Token") != null;

        private async Task CargarTiposMovimiento(CrearMovimientoViewModel vm)
        {
            try
            {
                var tipos = await _api.GetAsync<List<JsonElement>>("api/Caja/tipos") ?? new();

                if (tipos.Any())
                {
                    vm.TiposMovimiento = tipos
                        .Select(t => new SelectListItem(
                            $"{Get(t, "descripcion")} ({Get(t, "signo")})",
                            Get(t, "tipoMovimientoId")))
                        .ToList();
                }
                else
                {
                    // Fallback si la API no retorna datos
                    vm.TiposMovimiento = ObtenerTiposPorDefecto();
                }
            }
            catch
            {
                // Fallback si hay error en la llamada a la API
                vm.TiposMovimiento = ObtenerTiposPorDefecto();
            }
        }

        private static List<SelectListItem> ObtenerTiposPorDefecto()
        {
            return new()
            {
                new SelectListItem("Ingreso por Venta (+)", "1"),
                new SelectListItem("Cobro de Cuota (+)", "2"),
                new SelectListItem("Egreso por Gasto (-)", "3"),
                new SelectListItem("Egreso Manual (-)", "4"),
                new SelectListItem("Ingreso Manual (+)", "5")
            };
        }

        private static string Get(JsonElement el, string prop)
        {
            if (el.TryGetProperty(prop, out var val))
                return val.ValueKind == JsonValueKind.Number
                    ? val.GetRawText()
                    : val.GetString() ?? "";

            var camel = char.ToLower(prop[0]) + prop[1..];
            if (el.TryGetProperty(camel, out var val2))
                return val2.ValueKind == JsonValueKind.Number
                    ? val2.GetRawText()
                    : val2.GetString() ?? "";

            return "";
        }
    }
}