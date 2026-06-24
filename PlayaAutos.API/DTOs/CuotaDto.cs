namespace PlayaAutos.API.DTOs
{
    public class CuotaDto
    {
        public int CuotaId { get; set; }
        public int VentaId { get; set; }
        public int NumeroCuota { get; set; }
        public long Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaPago { get; set; }
        public long? MontoPagado { get; set; }
        public long? MontoRecargo { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Vehiculo { get; set; } = string.Empty;
        public int? CantidadCuotas { get; set; }
        public decimal? RecargoAutomatico { get; set; }
        public string? ComprobanteImagen { get; set; }
    }

    public class PagarCuotaDto
    {
        public DateTime FechaPago { get; set; }
        public long MontoPagado { get; set; }
        public long? MontoRecargo { get; set; }
        public int FormaPagoId { get; set; }
        public int UsuarioRegistro { get; set; }
        public string? ObservacionPago { get; set; }
        public string? ComprobanteImagen { get; set; }
    }
}