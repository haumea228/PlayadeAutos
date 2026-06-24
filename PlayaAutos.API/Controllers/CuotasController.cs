using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayaAutos.API.Data;
using PlayaAutos.API.DTOs;
using PlayaAutos.API.Models;
using PlayaAutos.API.Services;

namespace PlayaAutos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "AdministradorP,Vendedor,Cajero")]
    public class CuotasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CuotasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cuotas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuotaDto>>> GetCuotas([FromQuery] string? estado)
        {
            var query = _context.Cuotas
                .Include(c => c.Venta).ThenInclude(v => v.Cliente)
                .Include(c => c.Venta).ThenInclude(v => v.Vehiculo)
                    .ThenInclude(ve => ve.Modelo).ThenInclude(m => m.Marca)
                .Where(c => c.Venta.Estado != "Anulada")
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                if (estado == "Vencida")
                {
                    // Vencidas = Pendientes con fecha de vencimiento pasada
                    query = query.Where(c => c.Estado == "Pendiente" && c.FechaVencimiento < DateTime.Now);
                }
                else
                {
                    query = query.Where(c => c.Estado == estado);
                }
            }

            return await query
                .OrderBy(c => c.FechaVencimiento)
                .Select(c => new CuotaDto
                {
                    CuotaId = c.CuotaId,
                    VentaId = c.VentaId,
                    NumeroCuota = c.NumeroCuota,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    FechaPago = c.FechaPago,
                    MontoPagado = c.MontoPagado,
                    MontoRecargo = c.MontoRecargo,
                    Estado = c.Estado,
                    Cliente = c.Venta.Cliente.Nombre,
                    Vehiculo = c.Venta.Vehiculo.Modelo.Marca.Nombre + " " + c.Venta.Vehiculo.Modelo.Nombre,
                    CantidadCuotas = c.Venta.CantidadCuotas,
                    ComprobanteImagen = c.ComprobanteImagen
                })
                .ToListAsync();
        }

        // GET: api/cuotas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuotaDto>> GetCuota(int id)
        {
            var c = await _context.Cuotas
                .Include(c => c.Venta).ThenInclude(v => v.Cliente)
                .Include(c => c.Venta).ThenInclude(v => v.Vehiculo)
                    .ThenInclude(ve => ve.Modelo).ThenInclude(m => m.Marca)
                .FirstOrDefaultAsync(c => c.CuotaId == id);

            if (c == null) return NotFound();

            //Calcular recargo automático si la cuota está vencida
            decimal? recargoAuto = null;
            if (c.Estado == "Pendiente" && c.FechaVencimiento < DateTime.Now)
            {
                if (c.Venta.PorcentajeRecargo.HasValue && c.Venta.PorcentajeRecargo > 0)
                {
                    recargoAuto = (long)(c.Monto * (c.Venta.PorcentajeRecargo.Value / 100m));
                }
            }

            return new CuotaDto
            {
                CuotaId = c.CuotaId,
                VentaId = c.VentaId,
                NumeroCuota = c.NumeroCuota,
                Monto = c.Monto,
                FechaVencimiento = c.FechaVencimiento,
                FechaPago = c.FechaPago,
                MontoPagado = c.MontoPagado,
                MontoRecargo = c.MontoRecargo,
                Estado = c.Estado,
                Cliente = c.Venta.Cliente.Nombre,
                Vehiculo = c.Venta.Vehiculo.Modelo.Marca.Nombre + " " + c.Venta.Vehiculo.Modelo.Nombre,
                CantidadCuotas = c.Venta.CantidadCuotas,
                RecargoAutomatico = recargoAuto,
                ComprobanteImagen = c.ComprobanteImagen
            };
        }

        // GET: api/cuotas/venta/5
        [HttpGet("venta/{ventaId}")]
        public async Task<ActionResult<IEnumerable<CuotaDto>>> GetCuotasPorVenta(int ventaId)
        {
            return await _context.Cuotas
                .Where(c => c.VentaId == ventaId)
                .OrderBy(c => c.NumeroCuota)
                .Select(c => new CuotaDto
                {
                    CuotaId = c.CuotaId,
                    NumeroCuota = c.NumeroCuota,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    FechaPago = c.FechaPago,
                    MontoPagado = c.MontoPagado,
                    MontoRecargo = c.MontoRecargo,
                    Estado = c.Estado,
                    RecargoAutomatico = null
                })
                .ToListAsync();
        }

        // PUT: api/cuotas/5/pagar
        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> PagarCuota(int id, [FromBody] PagarCuotaDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cuota = await _context.Cuotas
                    .Include(c => c.Venta)
                    .FirstOrDefaultAsync(c => c.CuotaId == id);

                if (cuota == null) return NotFound();
                if (cuota.Estado == "Pagada") return BadRequest("Esta cuota ya está pagada.");
                if (cuota.Venta.Estado == "Anulada") return BadRequest("La venta asociada está anulada.");

                cuota.FechaPago = dto.FechaPago;
                cuota.MontoPagado = dto.MontoPagado;
                cuota.MontoRecargo = dto.MontoRecargo ?? 0;
                cuota.FormaPagoId = dto.FormaPagoId;
                cuota.ObservacionPago = dto.ObservacionPago;
                cuota.ComprobanteImagen = dto.ComprobanteImagen;
                cuota.Estado = "Pagada";
                cuota.FechaRegistroPago = DateTime.Now;

                var tipoIngreso = await _context.TiposMovimiento
                    .FirstOrDefaultAsync(t => t.Signo == "+");

                if (tipoIngreso == null)
                {
                    return BadRequest("No se encontró el tipo de movimiento 'Ingreso' en la base de datos.");
                }

                _context.MovimientosCaja.Add(new MovimientoCaja
                {
                    Fecha = DateTime.Now,
                    TipoMovimientoId = tipoIngreso.TipoMovimientoId,
                    Descripcion = $"Cobro Cuota #{cuota.NumeroCuota} - Venta #{cuota.VentaId}",
                    Monto = dto.MontoPagado + (dto.MontoRecargo ?? 0),
                    CuotaId = cuota.CuotaId,
                    FormaPagoId = dto.FormaPagoId,
                    UsuarioRegistro = dto.UsuarioRegistro,
                    Comentarios = dto.MontoRecargo > 0 ? $"Recargo por mora: Gs. {dto.MontoRecargo:N0}" : null
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Cuota pagada correctamente" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al pagar la cuota: {ex.Message}");
            }
        }

        // GET: api/cuotas/5/ticket
        [HttpGet("{id}/ticket")]
        public async Task<IActionResult> DescargarTicket(int id)
        {
            var cuota = await _context.Cuotas
                .Include(c => c.Venta).ThenInclude(v => v.Cliente)
                .Include(c => c.Venta).ThenInclude(v => v.Vehiculo)
                    .ThenInclude(ve => ve.Modelo).ThenInclude(m => m.Marca)
                .Include(c => c.Venta).ThenInclude(v => v.Cuotas)
                .Include(c => c.Venta).ThenInclude(v => v.Vendedor)
                .Include(c => c.FormaPago)
                .FirstOrDefaultAsync(c => c.CuotaId == id);

            if (cuota == null) return NotFound();
            if (cuota.Estado != "Pagada") return BadRequest("La cuota aún no ha sido pagada.");

            var venta = cuota.Venta;
            var saldoInicial = venta.SaldoFinanciado ?? venta.MontoTotal;

            var pagosAnteriores = venta.Cuotas
                .Where(c => c.Estado == "Pagada" && c.NumeroCuota < cuota.NumeroCuota)
                .Sum(c => (c.MontoPagado ?? 0) + (c.MontoRecargo ?? 0));

            var saldoAnterior = saldoInicial - pagosAnteriores;
            var abono = cuota.MontoPagado ?? cuota.Monto;
            var recargo = cuota.MontoRecargo ?? 0;
            var saldoActual = saldoAnterior - abono;

            var data = new TicketCuotaData
            {
                CuotaId = cuota.CuotaId,
                NumeroCuota = cuota.NumeroCuota,
                CantidadCuotas = venta.CantidadCuotas ?? venta.Cuotas.Count,
                Cliente = venta.Cliente.Nombre,
                Vehiculo = $"{venta.Vehiculo.Modelo.Marca.Nombre} {venta.Vehiculo.Modelo.Nombre}",
                FormaPago = cuota.FormaPago?.Descripcion ?? "—",
                Observacion = cuota.ObservacionPago,
                FechaPago = cuota.FechaRegistroPago ?? cuota.FechaPago ?? DateTime.Now,
                SaldoInicial = saldoInicial,
                SaldoAnterior = saldoAnterior,
                Abono = abono,
                Recargo = recargo,
                SaldoActual = Math.Max(0, saldoActual),
                CI_RUC = cuota.Venta.Cliente.CI_RUC ?? "",
                UsuarioRegistro = venta.Vendedor?.UsuarioNombre ?? "Sistema"
            };

            var pdfBytes = TicketCuotaPdfService.Generar(data);
            var fileName = $"Ticket-Cuota{cuota.NumeroCuota}-Venta{venta.VentaId}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}