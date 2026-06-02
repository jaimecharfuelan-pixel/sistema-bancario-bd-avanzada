using BancoApi.DTOs.DTOs_Transaccion;
using System;

namespace BancoApi.Models.models_transaccion
{

    public class model_TransaccionTransferencia
    {
        private readonly DTO_TransaccionTransferencia _dto;

        public model_TransaccionTransferencia(DTO_TransaccionTransferencia dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int cuentaOrigen => _dto.cuentaOrigen;
        public int cuentaDestino => _dto.cuentaDestino;
        public decimal monto => _dto.monto;

public void Validar()
        {
            if (cuentaOrigen <= 0)
            {
                throw new ArgumentException("La cuenta origen es requerida y debe ser mayor a cero");
            }

            if (cuentaDestino <= 0)
            {
                throw new ArgumentException("La cuenta destino es requerida y debe ser mayor a cero");
            }

            if (cuentaOrigen == cuentaDestino)
            {
                throw new ArgumentException("La cuenta origen y destino no pueden ser la misma");
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

