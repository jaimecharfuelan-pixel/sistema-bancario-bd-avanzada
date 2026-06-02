namespace BancoApi.Models.models_cajero
{

public class model_CajeroItem
    {
        public int idCajero { get; set; }
        public int idSucursal { get; set; }
        public decimal dineroDisponible { get; set; }
        public string estado { get; set; } = string.Empty;
    }
}

