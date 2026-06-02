namespace BancoApi.DTOs.DTOs_Tarjeta
{

public class DTO_TarjetaCrearCredito
    {
        public int idCuenta { get; set; }
        public decimal limiteCredito { get; set; }
        public decimal tasaInteres { get; set; }
        public decimal cuotaManejo { get; set; }
        public int fechaCorte { get; set; }
    }
}

