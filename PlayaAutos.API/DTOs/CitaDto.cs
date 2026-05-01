namespace PlayaAutos.API.DTOs
{
    public class CitaDto
    {
        public int CitaId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string? Vehiculo { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string EstadoCita { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string TipoCita { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }

    public class CrearCitaDto
    {
        public int ClienteId { get; set; }
        public int? VehiculoId { get; set; }
        public int VendedorId { get; set; }
        public int EstadoCitaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoCita { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }
}