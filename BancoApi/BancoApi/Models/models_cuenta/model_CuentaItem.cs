namespace BancoApi.Models.models_cuenta
{

public class model_CuentaItem
    {
        public int idCuenta { get; set; }
        public string idCliente { get; set; } = string.Empty;
        public decimal saldoCuenta { get; set; }
        public string estadoCuenta { get; set; } = string.Empty;
    }
}

