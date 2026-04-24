namespace PlayaAutos.API.DTOs
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CI_RUC { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
    }

    public class CrearClienteDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string CI_RUC { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public long? RangoPrecioMin { get; set; }
        public long? RangoPrecioMax { get; set; }
    }
}