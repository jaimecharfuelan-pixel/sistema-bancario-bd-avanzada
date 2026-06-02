using BancoApi.DTOs.DTOs_Transaccion;
using System;

namespace BancoApi.Models.models_transaccion
{

    public class model_TransaccionRetiroDebito
    {
        private readonly DTO_TransaccionRetiroDebito _dto;

        public model_TransaccionRetiroDebito(DTO_TransaccionRetiroDebito dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idTarjeta => _dto.idTarjeta;
        public int idCajero => _dto.idCajero;
        public decimal monto => _dto.monto;

public void Validar()
        {
            if (idTarjeta <= 0)
            {
                throw new ArgumentException("El ID de tarjeta es requerido y debe ser mayor a cero");
            }

            if (idCajero <= 0)
            {
                throw new ArgumentException("El ID de cajero es requerido y debe ser mayor a cero");
            }

            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor que cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

