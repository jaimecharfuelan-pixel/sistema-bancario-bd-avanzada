namespace BancoApi.Models.models_transaccion
{

public class model_TransaccionItem
    {
        public int idTransaccion { get; set; }
        public int? idCajero { get; set; }
        public int? idTarjeta { get; set; }
        public int? idCuentaOrigen { get; set; }
        public int? idCuentaDestino { get; set; }
        public DateTime fechaTransaccion { get; set; }
        public string tipoTransaccion { get; set; } = "";
        public decimal montoTransaccion { get; set; }
        public string estadoTransaccion { get; set; } = "";
        public string? descripcionTransaccion { get; set; }
    }
}

