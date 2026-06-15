using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayaAutos.API.Data;
using PlayaAutos.API.DTOs;
using PlayaAutos.API.Models;
using System.Security.Claims;

namespace PlayaAutos.API.Controllers
{
    [ApiController]
    [Route("api/micuenta")]
    [Authorize(Roles = "Cliente")]
    public class MiCuentaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MiCuentaController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetUsuarioId()
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
                return id;
            return null;
        }

        // GET: api/micuenta/perfil
        [HttpGet("perfil")]
        public async Task<ActionResult<ClienteDto>> GetPerfil()
        {
            var uid = GetUsuarioId();
            if (uid == null) return Unauthorized();

            var c = await _context.Clientes.FirstOrDefaultAsync(x => x.UsuarioId == uid);
            if (c == null) return NotFound("No se encontró perfil de cliente.");

            return new ClienteDto
            {
                ClienteId = c.ClienteId,
                Nombre    = c.Nombre,
                CI_RUC    = c.CI_RUC,
                Telefono  = c.Telefono,
                Email     = c.Email,
                Direccion = c.Direccion,
                Activo    = c.Activo
            };
        }

        // PUT: api/micuenta/perfil
        [HttpPut("perfil")]
        public async Task<IActionResult> PutPerfil(ActualizarPerfilClienteDto dto)
        {
            var uid = GetUsuarioId();
            if (uid == null) return Unauthorized();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.UsuarioId == uid);
            if (cliente == null) return NotFound("No se encontró perfil de cliente.");

            cliente.Nombre    = dto.Nombre;
            cliente.Telefono  = dto.Telefono;
            cliente.Direccion = dto.Direccion;

            await _context.SaveChangesAsync();
            return Ok(new { cliente.ClienteId });
        }

        // GET: api/micuenta/citas
        [HttpGet("citas")]
        public async Task<ActionResult<IEnumerable<CitaDto>>> GetMisCitas()
        {
            var uid = GetUsuarioId();
            if (uid == null) return Unauthorized();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == uid);
            if (cliente == null) return Ok(new List<CitaDto>());

            var citas = await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(c => c.EstadoCita)
                .Where(c => c.ClienteId == cliente.ClienteId)
                .OrderByDescending(c => c.FechaHora)
                .Select(c => new CitaDto
                {
                    CitaId       = c.CitaId,
                    ClienteId    = c.ClienteId,
                    Cliente      = c.Cliente.Nombre,
                    VehiculoId   = c.VehiculoId,
                    Vehiculo     = c.Vehiculo != null
                        ? c.Vehiculo.Modelo.Marca.Nombre + " " + c.Vehiculo.Modelo.Nombre
                        : null,
                    VendedorId   = c.VendedorId,
                    Vendedor     = _context.Usuarios
                        .Where(u => u.UsuarioId == c.VendedorId)
                        .Select(u => u.UsuarioNombre)
                        .FirstOrDefault() ?? "Sin asignar",
                    EstadoCitaId = c.EstadoCitaId,
                    Estado       = c.EstadoCita.Descripcion,
                    EstadoColor  = c.EstadoCita.Color,
                    FechaHora    = c.FechaHora,
                    TipoCita     = c.TipoCita,
                    Observaciones = c.Observaciones
                })
                .ToListAsync();

            return Ok(citas);
        }

        // POST: api/micuenta/citas/agendar
        [HttpPost("citas/agendar")]
        public async Task<ActionResult> AgendarCita(AgendarCitaClienteDto dto)
        {
            var uid = GetUsuarioId();
            if (uid == null) return Unauthorized();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == uid);
            if (cliente == null) return BadRequest("No se encontró perfil de cliente.");

            bool changed = cliente.Nombre    != dto.Nombre
                        || cliente.Telefono  != dto.Telefono
                        || cliente.Direccion != dto.Direccion;
            if (changed)
            {
                cliente.Nombre    = dto.Nombre;
                cliente.Telefono  = dto.Telefono;
                cliente.Direccion = dto.Direccion;
            }

            if (string.IsNullOrEmpty(cliente.CI_RUC) && !string.IsNullOrEmpty(dto.CI_RUC))
            {
                bool ciEnUso = await _context.Clientes.AnyAsync(c => c.CI_RUC == dto.CI_RUC);
                if (ciEnUso)
                    return BadRequest("El CI/RUC ingresado ya está registrado. Verificá el número.");
                cliente.CI_RUC = dto.CI_RUC;
            }

            var vendedorId = await _context.Usuarios
                .Where(u => u.Rol == "Vendedor" || u.Rol == "AdministradorP")
                .Select(u => (int?)u.UsuarioId)
                .FirstOrDefaultAsync();

            if (vendedorId == null)
                return BadRequest("No hay vendedores disponibles en el sistema.");

            var cita = new Cita
            {
                ClienteId     = cliente.ClienteId,
                VehiculoId    = dto.VehiculoId,
                VendedorId    = vendedorId.Value,
                EstadoCitaId  = 1,
                FechaHora     = dto.FechaHora,
                TipoCita      = dto.TipoCita,
                Observaciones = dto.Observaciones,
                FechaAlta     = DateTime.Now
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return Ok(new { cita.CitaId });
        }

        // PUT: api/micuenta/citas/{id}/cancelar
        [HttpPut("citas/{id}/cancelar")]
        public async Task<IActionResult> CancelarCita(int id)
        {
            var uid = GetUsuarioId();
            if (uid == null) return Unauthorized();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == uid);
            if (cliente == null) return Unauthorized();

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();
            if (cita.ClienteId != cliente.ClienteId) return Forbid();

            if (cita.EstadoCitaId == 3 || cita.EstadoCitaId == 4)
                return BadRequest("No se puede cancelar una cita ya realizada o cancelada.");

            cita.EstadoCitaId = 4;
            await _context.SaveChangesAsync();

            return Ok(new { cita.CitaId });
        }
    }
}
