using Microsoft.EntityFrameworkCore;
using PlayaAutos.API.Models;

namespace PlayaAutos.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Catálogos
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<TipoVehiculo> TiposVehiculo { get; set; }
        public DbSet<CondicionVehiculo> CondicionesVehiculo { get; set; }
        public DbSet<EstadoVehiculo> EstadosVehiculo { get; set; }
        public DbSet<OrigenVehiculo> OrigenesVehiculo { get; set; }

        // Gastos
        public DbSet<TipoGasto> TiposGasto { get; set; }
        public DbSet<GastoVehiculo> GastosVehiculo { get; set; }

        // Vehículos
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<FotoVehiculo> FotosVehiculo { get; set; }
        public DbSet<Consignante> Consignantes { get; set; }

        // Usuarios y clientes
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        // Citas
        public DbSet<EstadoCita> EstadosCita { get; set; }
        public DbSet<Cita> Citas { get; set; }

        // Ventas
        public DbSet<TipoVenta> TiposVenta { get; set; }
        public DbSet<FormaPago> FormasPago { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }

        // Caja
        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }
        public DbSet<MovimientoCaja> MovimientosCaja { get; set; }

        // Documentos
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<NotaCredito> NotasCredito { get; set; }
        public DbSet<ContratoConsigna> ContratosConsigna { get; set; }
        public DbSet<Timbrado> Timbrados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Claves primarias explícitas
            modelBuilder.Entity<Marca>().HasKey(m => m.MarcaId);
            modelBuilder.Entity<Modelo>().HasKey(m => m.ModeloId);
            modelBuilder.Entity<TipoVehiculo>().HasKey(t => t.TipoId);
            modelBuilder.Entity<CondicionVehiculo>().HasKey(c => c.CondicionId);
            modelBuilder.Entity<EstadoVehiculo>().HasKey(e => e.EstadoId);
            modelBuilder.Entity<OrigenVehiculo>().HasKey(o => o.OrigenId);
            modelBuilder.Entity<TipoGasto>().HasKey(t => t.TipoGastoId);
            modelBuilder.Entity<GastoVehiculo>().HasKey(g => g.GastoId);
            modelBuilder.Entity<Vehiculo>().HasKey(v => v.VehiculoId);
            modelBuilder.Entity<FotoVehiculo>().HasKey(f => f.FotoId);
            modelBuilder.Entity<Consignante>().HasKey(c => c.ConsignanteId);
            modelBuilder.Entity<Usuario>().HasKey(u => u.UsuarioId);
            modelBuilder.Entity<Cliente>().HasKey(c => c.ClienteId);
            modelBuilder.Entity<EstadoCita>().HasKey(e => e.EstadoCitaId);
            modelBuilder.Entity<Cita>().HasKey(c => c.CitaId);
            modelBuilder.Entity<TipoVenta>().HasKey(t => t.TipoVentaId);
            modelBuilder.Entity<FormaPago>().HasKey(f => f.FormaPagoId);
            modelBuilder.Entity<Venta>().HasKey(v => v.VentaId);
            modelBuilder.Entity<Cuota>().HasKey(c => c.CuotaId);
            modelBuilder.Entity<TipoMovimiento>().HasKey(t => t.TipoMovimientoId);
            modelBuilder.Entity<MovimientoCaja>().HasKey(m => m.MovimientoId);
            modelBuilder.Entity<Factura>().HasKey(f => f.FacturaId);
            modelBuilder.Entity<NotaCredito>().HasKey(n => n.NotaCreditoId);
            modelBuilder.Entity<ContratoConsigna>().HasKey(c => c.ContratoId);
            modelBuilder.Entity<Timbrado>().HasKey(t => t.TimbradoId);

            // Índices únicos
            modelBuilder.Entity<Marca>().HasIndex(m => m.Nombre).IsUnique();
            modelBuilder.Entity<TipoVehiculo>().HasIndex(t => t.Descripcion).IsUnique();
            modelBuilder.Entity<CondicionVehiculo>().HasIndex(c => c.Descripcion).IsUnique();
            modelBuilder.Entity<EstadoVehiculo>().HasIndex(e => e.Descripcion).IsUnique();
            modelBuilder.Entity<TipoGasto>().HasIndex(t => t.Descripcion).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.UsuarioEmail).IsUnique();
            modelBuilder.Entity<Cliente>().HasIndex(c => c.CI_RUC).IsUnique();
            modelBuilder.Entity<Consignante>().HasIndex(c => c.ClienteId).IsUnique();
            modelBuilder.Entity<Factura>().HasIndex(f => f.NumeroFactura).IsUnique();
            modelBuilder.Entity<Factura>().HasIndex(f => f.VentaId).IsUnique();
            modelBuilder.Entity<NotaCredito>().HasIndex(n => n.NumeroNota).IsUnique();
            modelBuilder.Entity<Vehiculo>().HasIndex(v => v.CodigoInterno).IsUnique();

            // Montos en Guaraníes
            modelBuilder.Entity<Vehiculo>().Property(v => v.PrecioVenta).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Vehiculo>().Property(v => v.CostoAdquisicion).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Vehiculo>().Property(v => v.Kilometraje).HasColumnType("numeric(10,0)");
            modelBuilder.Entity<GastoVehiculo>().Property(g => g.Monto).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Venta>().Property(v => v.MontoTotal).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Venta>().Property(v => v.MontoEntrada).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Venta>().Property(v => v.SaldoFinanciado).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Venta>().Property(v => v.ValorPermuta).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Cuota>().Property(c => c.Monto).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Cuota>().Property(c => c.MontoPagado).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Cuota>().Property(c => c.MontoRecargo).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<MovimientoCaja>().Property(m => m.Monto).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Factura>().Property(f => f.Subtotal).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Factura>().Property(f => f.IVA).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Factura>().Property(f => f.Total).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<NotaCredito>().Property(n => n.Monto).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Cliente>().Property(c => c.RangoPrecioMin).HasColumnType("numeric(15,0)");
            modelBuilder.Entity<Cliente>().Property(c => c.RangoPrecioMax).HasColumnType("numeric(15,0)");

            // Relaciones Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Vendedor)
                .WithMany(u => u.Ventas)
                .HasForeignKey(v => v.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Vehiculo)
                .WithMany(ve => ve.Ventas)
                .HasForeignKey(v => v.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Cita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany(cl => cl.Citas)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones MovimientoCaja
            modelBuilder.Entity<MovimientoCaja>()
                .HasOne(m => m.GastoVehiculo)
                .WithOne(g => g.MovimientoCaja)
                .HasForeignKey<MovimientoCaja>(m => m.GastoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cuota>()
                .HasOne(c => c.MovimientoCaja)
                .WithOne(m => m.Cuota)
                .HasForeignKey<MovimientoCaja>(m => m.CuotaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.MovimientosCaja)
                .WithOne(m => m.Venta)
                .HasForeignKey(m => m.VentaId)
                .OnDelete(DeleteBehavior.Restrict);
            // Decimales con precisión explícita
            modelBuilder.Entity<Consignante>()
                .Property(c => c.PorcentajeComisionDefault)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.PorcentajeComision)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Venta>()
                .Property(v => v.TasaInteres)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<ContratoConsigna>()
                .Property(c => c.PorcentajeComision)
                .HasColumnType("decimal(5,2)");
        }
    }
}