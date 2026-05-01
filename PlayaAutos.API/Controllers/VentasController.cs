using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayaAutos.API.Data;
using PlayaAutos.API.DTOs;
using PlayaAutos.API.Models;

namespace PlayaAutos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "AdministradorP,Vendedor")]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Vehiculo).ThenInclude(ve => ve.Modelo).ThenInclude(m => m.Marca)
                .Include(v => v.Vendedor)
                .Include(v => v.TipoVenta)
                .Include(v => v.FormaPago)
                .Select(v => new VentaDto
                {
                    VentaId = v.VentaId,
                    Cliente = v.Cliente.Nombre,
                    Vehiculo = v.Vehiculo.Modelo.Marca.Nombre + " " + v.Vehiculo.Modelo.Nombre,
                    Vendedor = v.Vendedor.UsuarioNombre,
                    TipoVenta = v.TipoVenta.Descripcion,
                    FormaPago = v.FormaPago.Descripcion,
                    FechaVenta = v.FechaVenta,
                    MontoTotal = v.MontoTotal,
                    Estado = v.Estado
                })
                .ToListAsync();
        }

        // GET: api/ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetVenta(int id)
        {
            var v = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Vehiculo).ThenInclude(ve => ve.Modelo).ThenInclude(m => m.Marca)
                .Include(v => v.Vendedor)
                .Include(v => v.TipoVenta)
                .Include(v => v.FormaPago)
                .FirstOrDefaultAsync(v => v.VentaId == id);

            if (v == null) return NotFound();

            return new VentaDto
            {
                VentaId = v.VentaId,
                Cliente = v.Cliente.Nombre,
                Vehiculo = v.Vehiculo.Modelo.Marca.Nombre + " " + v.Vehiculo.Modelo.Nombre,
                Vendedor = v.Vendedor.UsuarioNombre,
                TipoVenta = v.TipoVenta.Descripcion,
                FormaPago = v.FormaPago.Descripcion,
                FechaVenta = v.FechaVenta,
                MontoTotal = v.MontoTotal,
                Estado = v.Estado
            };
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> PostVenta(CrearVentaDto dto)
        {
            // ── Validar vehículo ─────────────────────────────────────────
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Estado)
                .FirstOrDefaultAsync(v => v.VehiculoId == dto.VehiculoId);

            if (vehiculo == null)
                return BadRequest("El vehículo no existe.");

            if (!vehiculo.Estado.PermiteVenta)
                return BadRequest($"El vehículo no está disponible para la venta. Estado actual: {vehiculo.Estado.Descripcion}");

            // ── Validar cliente ──────────────────────────────────────────
            if (!await _context.Clientes.AnyAsync(c => c.ClienteId == dto.ClienteId))
                return BadRequest("El cliente no existe.");

            // ── Validar timbrado activo ──────────────────────────────────
            var timbrado = await _context.Timbrados
                .FirstOrDefaultAsync(t =>
                    t.Activo &&
                    t.FechaInicio <= DateTime.Now &&
                    t.FechaVencimiento >= DateTime.Now &&
                    t.UltimoNumeroUsado < t.NumeroHasta);

            if (timbrado == null)
                return BadRequest("No hay timbrado activo disponible. Contacte al administrador.");

            // ── Crear venta ──────────────────────────────────────────────
            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                VehiculoId = dto.VehiculoId,
                VendedorId = dto.VendedorId,
                TipoVentaId = dto.TipoVentaId,
                FormaPagoId = dto.FormaPagoId,
                FechaVenta = dto.FechaVenta,
                MontoTotal = dto.MontoTotal,
                MontoEntrada = dto.MontoEntrada,
                SaldoFinanciado = dto.SaldoFinanciado,
                TasaInteres = dto.TasaInteres,
                CantidadCuotas = dto.CantidadCuotas,
                VehiculoPermutaId = dto.VehiculoPermutaId,
                ValorPermuta = dto.ValorPermuta,
                Estado = "Registrada"
            };

            _context.Ventas.Add(venta);

            // ── Cambiar estado del vehículo a Vendido ────────────────────
            var estadoVendido = await _context.EstadosVehiculo
                .FirstOrDefaultAsync(e => e.Descripcion == "Vendido");

            if (estadoVendido != null)
                vehiculo.EstadoId = estadoVendido.EstadoId;

            // ── Generar cuotas si es crédito ─────────────────────────────
            if (dto.CantidadCuotas.HasValue && dto.CantidadCuotas > 0 && dto.SaldoFinanciado.HasValue)
            {
                var montoCuota = dto.SaldoFinanciado.Value / dto.CantidadCuotas.Value;
                for (int i = 1; i <= dto.CantidadCuotas.Value; i++)
                {
                    _context.Cuotas.Add(new Cuota
                    {
                        Venta = venta,
                        NumeroCuota = i,
                        Monto = montoCuota,
                        FechaVencimiento = dto.FechaVenta.AddMonths(i),
                        Estado = "Pendiente"
                    });
                }
            }

            // ── Registrar movimiento de caja ─────────────────────────────
            var tipoIngreso = await _context.TiposMovimiento
                .FirstOrDefaultAsync(t => t.Signo == "+");

            if (tipoIngreso != null)
            {
                _context.MovimientosCaja.Add(new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoIngreso.TipoMovimientoId,
                    Descripcion = $"Venta de vehículo - Cliente ID {dto.ClienteId}",
                    Monto = dto.MontoEntrada ?? dto.MontoTotal,
                    Venta = venta,
                    UsuarioRegistro = dto.VendedorId
                });
            }

            // ── Generar factura automáticamente ─────────────────────────
            // IVA incluido al 10% (estándar Paraguay):
            // Subtotal = Total / 1.10  →  IVA = Total - Subtotal
            timbrado.UltimoNumeroUsado++;
            var numeroFactura = $"{timbrado.NumeroTimbrado}-{timbrado.UltimoNumeroUsado:D8}";

            var subtotal = (long)Math.Round(dto.MontoTotal / 1.10m);
            var iva = dto.MontoTotal - subtotal;

            _context.Facturas.Add(new Factura
            {
                Venta = venta,
                TimbradoId = timbrado.TimbradoId,
                NumeroFactura = numeroFactura,
                FechaEmision = dto.FechaVenta,
                Subtotal = subtotal,
                IVA = iva,
                Total = dto.MontoTotal,
                FechaGeneracion = DateTime.Now,
                UsuarioGeneracion = dto.VendedorId
            });

            // ── Persistir todo en una sola transacción ───────────────────
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = venta.VentaId },
                new { venta.VentaId, NumeroFactura = numeroFactura });
        }

        // PUT: api/ventas/5/anular
        [HttpPut("{id}/anular")]
        public async Task<IActionResult> AnularVenta(int id, [FromBody] string motivo)
        {
            var venta = await _context.Ventas
                .Include(v => v.Vehiculo)
                .FirstOrDefaultAsync(v => v.VentaId == id);

            if (venta == null) return NotFound();
            if (venta.Estado == "Anulada") return BadRequest("La venta ya está anulada.");

            venta.Estado = "Anulada";
            venta.FechaAnulacion = DateTime.Now;
            venta.MotivoAnulacion = motivo;

            // Revertir estado del vehículo a Disponible
            var estadoDisponible = await _context.EstadosVehiculo
                .FirstOrDefaultAsync(e => e.Descripcion == "Disponible");

            if (estadoDisponible != null)
                venta.Vehiculo.EstadoId = estadoDisponible.EstadoId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}