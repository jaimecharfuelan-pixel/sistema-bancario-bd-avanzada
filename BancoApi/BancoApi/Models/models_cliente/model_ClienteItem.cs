namespace BancoApi.Models.models_cliente
{

public class model_ClienteItem
    {
        public string idCliente { get; set; } = string.Empty;
        public string nombreCliente { get; set; } = string.Empty;
        public string emailCliente { get; set; } = string.Empty;
        public string telefonoCliente { get; set; } = string.Empty;
        public int cedulaCliente { get; set; }
    }
}

