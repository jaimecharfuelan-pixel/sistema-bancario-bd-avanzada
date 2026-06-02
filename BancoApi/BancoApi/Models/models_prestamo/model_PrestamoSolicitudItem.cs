namespace BancoApi.Models.models_prestamo
{

public class model_PrestamoSolicitudItem
    {
        public int idPrestamo { get; set; }
        public int idCuenta { get; set; }
        public decimal montoPrestamo { get; set; }
        public string? fechaInicioPrestamo { get; set; }
        public string? fechaVencimientoPrestamo { get; set; }
        public string estadoPrestamo { get; set; } = string.Empty;
    }
}

