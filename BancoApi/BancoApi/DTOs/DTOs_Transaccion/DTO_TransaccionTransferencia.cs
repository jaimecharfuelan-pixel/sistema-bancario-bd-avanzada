namespace BancoApi.DTOs.DTOs_Transaccion
{

public class DTO_TransaccionTransferencia
    {
        public int cuentaOrigen { get; set; }
        public int cuentaDestino { get; set; }
        public decimal monto { get; set; }
    }
}

