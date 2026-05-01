using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayaAutos.Web.Models;
using PlayaAutos.Web.Services;
using System.Text.Json;

namespace PlayaAutos.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApiService _api;

        public VentasController(ApiService api)
        {
            _api = api;
        }

        // ── GET /Ventas ──────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            var ventas = await _api.GetAsync<List<VentaListItem>>("api/Ventas")
                         ?? new List<VentaListItem>();
            return View(ventas);
        }

        // ── GET /Ventas/Crear ────────────────────────────────────────────
        public async Task<IActionResult> Crear()
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            var vm = new VentaViewModel { FechaVenta = DateTime.Today };
            await CargarDropdowns(vm);
            return View(vm);
        }

        // ── POST /Ventas/Crear ───────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(VentaViewModel vm)
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            // Validaciones cruzadas de financiación
            if (vm.MontoEntrada.HasValue && vm.MontoEntrada > vm.MontoTotal)
                ModelState.AddModelError("MontoEntrada", "La entrada no puede superar el monto total.");

            if (vm.SaldoFinanciado.HasValue && vm.CantidadCuotas is null or <= 0)
                ModelState.AddModelError("CantidadCuotas", "Indique la cantidad de cuotas para el saldo financiado.");

            if (vm.ValorPermuta.HasValue && vm.VehiculoPermutaId is null)
                ModelState.AddModelError("VehiculoPermutaId", "Seleccione el vehículo en permuta.");

            if (!ModelState.IsValid)
            {
                await CargarDropdowns(vm);
                return View(vm);
            }

            // VendedorId desde la sesión
            var vendedorId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            var response = await _api.PostAsync("api/Ventas", new
            {
                vm.ClienteId,
                vm.VehiculoId,
                VendedorId = vendedorId,
                vm.TipoVentaId,
                vm.FormaPagoId,
                vm.FechaVenta,
                vm.MontoTotal,
                vm.MontoEntrada,
                vm.SaldoFinanciado,
                vm.TasaInteres,
                vm.CantidadCuotas,
                vm.VehiculoPermutaId,
                vm.ValorPermuta
            });

            if (!response.IsSuccessStatusCode)
            {
                var detalle = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", detalle.Contains("disponible")
                    ? detalle
                    : "Error al registrar la venta. Verifique los datos e intente nuevamente.");
                await CargarDropdowns(vm);
                return View(vm);
            }

            TempData["Exito"] = "Venta registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Ventas/Anular ──────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id, string motivo)
        {
            if (!EstaAutenticado()) return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["Error"] = "Debe ingresar un motivo para anular la venta.";
                return RedirectToAction(nameof(Index));
            }

            var resp = await _api.PutAsync($"api/Ventas/{id}/anular", motivo);

            TempData[resp.IsSuccessStatusCode ? "Exito" : "Error"] = resp.IsSuccessStatusCode
                ? "Venta anulada correctamente."
                : "No se pudo anular la venta. Intente nuevamente.";

            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private bool EstaAutenticado() =>
            HttpContext.Session.GetString("Token") != null;

        private async Task CargarDropdowns(VentaViewModel vm)
        {
            var clientes = await _api.GetAsync<List<JsonElement>>("api/Clientes") ?? new();
            var vehiculos = await _api.GetAsync<List<JsonElement>>("api/Vehiculos") ?? new();
            var tiposVenta = await _api.GetAsync<List<JsonElement>>("api/Catalogos/tipos-venta") ?? new();
            var formasPago = await _api.GetAsync<List<JsonElement>>("api/Catalogos/formas-pago") ?? new();

            vm.Clientes = clientes
                .Where(c => c.TryGetProperty("activo", out var a) && a.GetBoolean())
                .Select(c => new SelectListItem(
                    Get(c, "nombre") + " — " + Get(c, "ci_RUC"),
                    Get(c, "clienteId")))
                .ToList();

            // Solo vehículos disponibles (estado que permiteVenta)
            vm.Vehiculos = vehiculos
                .Where(v =>
                {
                    if (!v.TryGetProperty("estado", out var est)) return false;
                    if (est.ValueKind == JsonValueKind.Object)
                        return est.TryGetProperty("permiteVenta", out var pv) && pv.GetBoolean();
                    // Si el estado viene como string, filtramos "Disponible"
                    var estadoStr = est.ValueKind == JsonValueKind.String ? est.GetString() ?? "" : "";
                    return estadoStr.Equals("Disponible", StringComparison.OrdinalIgnoreCase);
                })
                .Select(v => new SelectListItem(
                    Get(v, "codigoInterno") + " — " + Get(v, "marca") + " " + Get(v, "modelo") +
                    " (" + Get(v, "anio") + ")" +
                    "  Gs. " + (long.TryParse(Get(v, "precioVenta"), out var p) ? p.ToString("N0") : ""),
                    Get(v, "vehiculoId")))
                .ToList();

            // Todos los vehículos para permuta (cualquier estado)
            vm.VehiculosPermuta = vehiculos
                .Select(v => new SelectListItem(
                    Get(v, "codigoInterno") + " — " + Get(v, "marca") + " " + Get(v, "modelo"),
                    Get(v, "vehiculoId")))
                .ToList();

            vm.TiposVenta = tiposVenta
                .Select(t => new SelectListItem(
                    Get(t, "descripcion"),
                    Get(t, "tipoVentaId"),
                    false,
                    false)
                {
                    // Usamos Group para marcar si requiere financiación (lo leerá el JS)
                    Group = new SelectListGroup
                    {
                        Name = t.TryGetProperty("requiereFinanciacion", out var rf) && rf.GetBoolean()
                            ? "credito" : "contado"
                    }
                })
                .ToList();

            vm.FormasPago = formasPago
                .Select(f => new SelectListItem(Get(f, "descripcion"), Get(f, "formaPagoId")))
                .ToList();
        }

        private static string Get(JsonElement el, string prop)
        {
            if (el.TryGetProperty(prop, out var val))
                return val.ValueKind == JsonValueKind.Number
                    ? val.GetRawText()
                    : val.GetString() ?? "";
            var lower = char.ToLower(prop[0]) + prop[1..];
            if (el.TryGetProperty(lower, out var val2))
                return val2.ValueKind == JsonValueKind.Number
                    ? val2.GetRawText()
                    : val2.GetString() ?? "";
            return "";
        }
    }
}
