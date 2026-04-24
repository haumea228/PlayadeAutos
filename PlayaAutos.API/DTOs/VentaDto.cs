namespace PlayaAutos.API.DTOs
{
    public class VentaDto
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

    public class CrearVentaDto
    {
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int VendedorId { get; set; }
        public int TipoVentaId { get; set; }
        public int FormaPagoId { get; set; }
        public DateTime FechaVenta { get; set; }
        public long MontoTotal { get; set; }
        public long? MontoEntrada { get; set; }
        public long? SaldoFinanciado { get; set; }
        public decimal? TasaInteres { get; set; }
        public int? CantidadCuotas { get; set; }
        public int? VehiculoPermutaId { get; set; }
        public long? ValorPermuta { get; set; }
    }
}