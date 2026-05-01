#pragma warning disable CS8602
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
    [Authorize(Roles = "AdministradorP,Vendedor,Cliente")]
    public class CitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetCitas()
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(c => c.EstadoCita)
                .Select(c => new CitaDto
                {
                    CitaId = c.CitaId,
                    Cliente = c.Cliente!.Nombre,
                    Vehiculo = c.Vehiculo != null
                        ? c.Vehiculo!.Modelo.Marca.Nombre + " " + c.Vehiculo.Modelo.Nombre
                        : null,
                    Vendedor = c.VendedorId.ToString(),
                    EstadoCita = c.EstadoCita.Descripcion,
                    FechaHora = c.FechaHora,
                    TipoCita = c.TipoCita,
                    Observaciones = c.Observaciones
                })
                .ToListAsync();
        }

        // GET: api/citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CitaDto>> GetCita(int id)
        {
            var c = await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(c => c.EstadoCita)
                .FirstOrDefaultAsync(c => c.CitaId == id);

            if (c == null) return NotFound();

            return new CitaDto
            {
                CitaId = c.CitaId,
                Cliente = c.Cliente.Nombre,
                Vehiculo = c.Vehiculo != null
                    ? c.Vehiculo!.Modelo.Marca.Nombre + " " + c.Vehiculo.Modelo.Nombre
                    : null,
                Vendedor = c.VendedorId.ToString(),
                EstadoCita = c.EstadoCita.Descripcion,
                FechaHora = c.FechaHora,
                TipoCita = c.TipoCita,
                Observaciones = c.Observaciones
            };
        }

        // GET: api/citas/cliente/5
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetCitasPorCliente(int clienteId)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(c => c.EstadoCita)
                .Where(c => c.ClienteId == clienteId)
                .Select(c => new CitaDto
                {
                    CitaId = c.CitaId,
                    Cliente = c.Cliente!.Nombre,
                    Vehiculo = c.Vehiculo != null
                        ? c.Vehiculo!.Modelo.Marca.Nombre + " " + c.Vehiculo.Modelo.Nombre
                        : null,
                    Vendedor = c.VendedorId.ToString(),
                    EstadoCita = c.EstadoCita.Descripcion,
                    FechaHora = c.FechaHora,
                    TipoCita = c.TipoCita,
                    Observaciones = c.Observaciones
                })
                .ToListAsync();
        }

        // POST: api/citas
        [HttpPost]
        public async Task<ActionResult<CitaDto>> PostCita(CrearCitaDto dto)
        {
            var cita = new Cita
            {
                ClienteId = dto.ClienteId,
                VehiculoId = dto.VehiculoId,
                VendedorId = dto.VendedorId,
                EstadoCitaId = dto.EstadoCitaId,
                FechaHora = dto.FechaHora,
                TipoCita = dto.TipoCita,
                Observaciones = dto.Observaciones,
                FechaAlta = DateTime.Now
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCita), new { id = cita.CitaId }, null);
        }

        // PUT: api/citas/5/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] int estadoCitaId)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            cita.EstadoCitaId = estadoCitaId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}