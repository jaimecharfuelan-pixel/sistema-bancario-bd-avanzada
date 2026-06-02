namespace BancoApi.Models.models_cuota
{

public class model_CuotaItem
    {
        public int idCuota { get; set; }
        public int idPrestamo { get; set; }
        public int numeroCuota { get; set; }
        public decimal montoDeCuota { get; set; }
        public decimal capitalCuota { get; set; }
        public string? fechaDeVencimientoCuota { get; set; }
        public string? fechaDePagoCuota { get; set; }
        public string estadoCuota { get; set; } = string.Empty;
    }
}

