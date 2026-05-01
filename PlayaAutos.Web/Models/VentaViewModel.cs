using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PlayaAutos.Web.Models
{
    public class VentaViewModel
    {
        // ── Dropdowns ────────────────────────────────────────────────────
        [Required(ErrorMessage = "Seleccione un cliente.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Seleccione un vehículo.")]
        [Display(Name = "Vehículo")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "Seleccione un tipo de venta.")]
        [Display(Name = "Tipo de venta")]
        public int TipoVentaId { get; set; }

        [Required(ErrorMessage = "Seleccione una forma de pago.")]
        [Display(Name = "Forma de pago")]
        public int FormaPagoId { get; set; }

        // VendedorId se completa desde la sesión, no lo elige el usuario
        public int VendedorId { get; set; }

        // ── Datos de la venta ────────────────────────────────────────────
        [Required(ErrorMessage = "La fecha de venta es obligatoria.")]
        [Display(Name = "Fecha de venta")]
        [DataType(DataType.Date)]
        public DateTime FechaVenta { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "El monto total debe ser mayor a cero.")]
        [Display(Name = "Monto total (Gs.)")]
        public long MontoTotal { get; set; }

        // ── Financiación (opcional) ──────────────────────────────────────
        [Display(Name = "Monto de entrada (Gs.)")]
        [Range(0, long.MaxValue, ErrorMessage = "El monto de entrada debe ser positivo.")]
        public long? MontoEntrada { get; set; }

        [Display(Name = "Saldo financiado (Gs.)")]
        [Range(0, long.MaxValue, ErrorMessage = "El saldo financiado debe ser positivo.")]
        public long? SaldoFinanciado { get; set; }

        [Display(Name = "Tasa de interés (%)")]
        [Range(0, 100, ErrorMessage = "La tasa debe estar entre 0 y 100.")]
        public decimal? TasaInteres { get; set; }

        [Display(Name = "Cantidad de cuotas")]
        [Range(1, 120, ErrorMessage = "Las cuotas deben estar entre 1 y 120.")]
        public int? CantidadCuotas { get; set; }

        // ── Permuta (opcional) ───────────────────────────────────────────
        [Display(Name = "Vehículo en permuta")]
        public int? VehiculoPermutaId { get; set; }

        [Display(Name = "Valor de permuta (Gs.)")]
        [Range(0, long.MaxValue, ErrorMessage = "El valor de permuta debe ser positivo.")]
        public long? ValorPermuta { get; set; }

        // ── Listas para dropdowns ────────────────────────────────────────
        public List<SelectListItem> Clientes { get; set; } = new();
        public List<SelectListItem> Vehiculos { get; set; } = new();
        public List<SelectListItem> TiposVenta { get; set; } = new();
        public List<SelectListItem> FormasPago { get; set; } = new();
        public List<SelectListItem> VehiculosPermuta { get; set; } = new();
    }

    public class VentaListItem
    {
        public int VentaId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Vehiculo { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
        public string TipoVenta { get; set; } = string.Empty;
        public string FormaPago { get; set; } = string.Empty;
        public DateTime FechaVenta { get; set; }
        public long MontoTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
