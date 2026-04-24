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
    public class VehiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehiculosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/vehiculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetVehiculos()
        {
            return await _context.Vehiculos
                .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(v => v.Tipo)
                .Include(v => v.Condicion)
                .Include(v => v.Estado)
                .Include(v => v.Fotos)
                .Select(v => new VehiculoDto
                {
                    VehiculoId = v.VehiculoId,
                    CodigoInterno = v.CodigoInterno,
                    Marca = v.Modelo.Marca.Nombre,
                    Modelo = v.Modelo.Nombre,
                    Tipo = v.Tipo.Descripcion,
                    Condicion = v.Condicion.Descripcion,
                    Estado = v.Estado.Descripcion,
                    Anio = v.Anio,
                    Color = v.Color,
                    Kilometraje = v.Kilometraje,
                    PrecioVenta = v.PrecioVenta,
                    Descripcion = v.Descripcion,
                    FotoPrincipal = v.Fotos
                        .Where(f => f.EsPrincipal)
                        .Select(f => f.URL)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        // GET: api/vehiculos/catalogo (solo visibles al público)
        [AllowAnonymous]
        [HttpGet("catalogo")]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> GetCatalogo()
        {
            return await _context.Vehiculos
                .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(v => v.Tipo)
                .Include(v => v.Condicion)
                .Include(v => v.Estado)
                .Include(v => v.Fotos)
                .Where(v => v.Estado.VisibleEnCatalogo)
                .Select(v => new VehiculoDto
                {
                    VehiculoId = v.VehiculoId,
                    CodigoInterno = v.CodigoInterno,
                    Marca = v.Modelo.Marca.Nombre,
                    Modelo = v.Modelo.Nombre,
                    Tipo = v.Tipo.Descripcion,
                    Condicion = v.Condicion.Descripcion,
                    Estado = v.Estado.Descripcion,
                    Anio = v.Anio,
                    Color = v.Color,
                    Kilometraje = v.Kilometraje,
                    PrecioVenta = v.PrecioVenta,
                    Descripcion = v.Descripcion,
                    FotoPrincipal = v.Fotos
                        .Where(f => f.EsPrincipal)
                        .Select(f => f.URL)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        // GET: api/vehiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoDto>> GetVehiculo(int id)
        {
            var v = await _context.Vehiculos
                .Include(v => v.Modelo).ThenInclude(m => m.Marca)
                .Include(v => v.Tipo)
                .Include(v => v.Condicion)
                .Include(v => v.Estado)
                .Include(v => v.Fotos)
                .FirstOrDefaultAsync(v => v.VehiculoId == id);

            if (v == null) return NotFound();

            return new VehiculoDto
            {
                VehiculoId = v.VehiculoId,
                CodigoInterno = v.CodigoInterno,
                Marca = v.Modelo.Marca.Nombre,
                Modelo = v.Modelo.Nombre,
                Tipo = v.Tipo.Descripcion,
                Condicion = v.Condicion.Descripcion,
                Estado = v.Estado.Descripcion,
                Anio = v.Anio,
                Color = v.Color,
                Kilometraje = v.Kilometraje,
                PrecioVenta = v.PrecioVenta,
                Descripcion = v.Descripcion,
                FotoPrincipal = v.Fotos
                    .Where(f => f.EsPrincipal)
                    .Select(f => f.URL)
                    .FirstOrDefault()
            };
        }

        // POST: api/vehiculos
        [HttpPost]
        public async Task<ActionResult<VehiculoDto>> PostVehiculo(CrearVehiculoDto dto)
        {
            var vehiculo = new Vehiculo
            {
                CodigoInterno = dto.CodigoInterno,
                ModeloId = dto.ModeloId,
                TipoId = dto.TipoId,
                CondicionId = dto.CondicionId,
                EstadoId = dto.EstadoId,
                OrigenId = dto.OrigenId,
                Anio = dto.Anio,
                Color = dto.Color,
                Kilometraje = dto.Kilometraje,
                PrecioVenta = dto.PrecioVenta,
                CostoAdquisicion = dto.CostoAdquisicion,
                Descripcion = dto.Descripcion,
                FechaAlta = DateTime.Now,
                UsuarioAlta = 1 // temporal hasta implementar JWT
            };

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.VehiculoId }, null);
        }

        // PUT: api/vehiculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, CrearVehiculoDto dto)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return NotFound();

            vehiculo.CodigoInterno = dto.CodigoInterno;
            vehiculo.ModeloId = dto.ModeloId;
            vehiculo.TipoId = dto.TipoId;
            vehiculo.CondicionId = dto.CondicionId;
            vehiculo.EstadoId = dto.EstadoId;
            vehiculo.OrigenId = dto.OrigenId;
            vehiculo.Anio = dto.Anio;
            vehiculo.Color = dto.Color;
            vehiculo.Kilometraje = dto.Kilometraje;
            vehiculo.PrecioVenta = dto.PrecioVenta;
            vehiculo.CostoAdquisicion = dto.CostoAdquisicion;
            vehiculo.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/vehiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return NotFound();
            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}