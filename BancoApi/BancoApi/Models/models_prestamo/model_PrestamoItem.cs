namespace BancoApi.Models.models_prestamo
{

public class model_PrestamoItem
    {
        public int idPrestamo { get; set; }
        public int idCuenta { get; set; }
        public decimal montoPrestamo { get; set; }
        public decimal saldoPrestamo { get; set; }
        public string estadoPrestamo { get; set; } = string.Empty;
    }
}

