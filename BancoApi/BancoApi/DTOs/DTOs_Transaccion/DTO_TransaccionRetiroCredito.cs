namespace BancoApi.DTOs.DTOs_Transaccion
{

public class DTO_TransaccionRetiroCredito
    {
        public int idTarjeta { get; set; }
        public int idCajero { get; set; }
        public decimal monto { get; set; }
    }
}

