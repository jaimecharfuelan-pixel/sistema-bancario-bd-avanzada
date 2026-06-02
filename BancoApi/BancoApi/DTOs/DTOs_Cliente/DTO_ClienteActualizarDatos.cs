namespace BancoApi.DTOs.DTOs_Cliente
{

public class DTO_ClienteActualizarDatos
    {
        public string idCliente { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public int cedula { get; set; }
    }
}

