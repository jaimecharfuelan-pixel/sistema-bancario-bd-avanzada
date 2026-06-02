namespace BancoApi.DTOs.DTOs_Cliente
{

public class DTO_ClienteProcesarMasivos
    {

public class ClienteInfo
        {
            public string idCliente { get; set; } = string.Empty;
            public string nombreCliente { get; set; } = string.Empty;
            public string emailCliente { get; set; } = string.Empty;
            public string telefonoCliente { get; set; } = string.Empty;
        }

        public List<ClienteInfo> clientes { get; set; } = new List<ClienteInfo>();
    }
}

