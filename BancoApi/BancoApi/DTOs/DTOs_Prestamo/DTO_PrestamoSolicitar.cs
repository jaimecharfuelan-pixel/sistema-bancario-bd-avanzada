namespace BancoApi.DTOs.DTOs_Prestamo
{

public class DTO_PrestamoSolicitar
    {
        public int idCuenta { get; set; }
        public int idSucursal { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}

