using System;

namespace BancoApi.Models.models_tarjeta
{

public class model_TarjetaItem
    {
        public int idTarjeta { get; set; }
        public int idCuenta { get; set; }
        public string numeroTarjeta { get; set; } = string.Empty;
        public string cvvTarjeta { get; set; } = string.Empty;
        public string estadoTarjeta { get; set; } = string.Empty;
        public DateTime fechaEmision { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string tipoTarjeta { get; set; } = string.Empty;
    }
}

