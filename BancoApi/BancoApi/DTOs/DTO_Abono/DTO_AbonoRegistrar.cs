namespace BancoApi.DTOs.DTO_Abono
{

public class DTO_AbonoRegistrar
    {
        public int idPrestamo { get; set; }
        public decimal montoAbono { get; set; }
        public string tipoAbono { get; set; } = string.Empty;
        public int idCuenta { get; set; }
    }
}
