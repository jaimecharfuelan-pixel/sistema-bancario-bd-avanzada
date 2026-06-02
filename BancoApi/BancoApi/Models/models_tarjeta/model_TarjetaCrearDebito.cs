using BancoApi.DTOs.DTOs_Tarjeta;
using System;

namespace BancoApi.Models.models_tarjeta
{

    public class model_TarjetaCrearDebito
    {
        private DTO_TarjetaCrearDebito _dto;

        public model_TarjetaCrearDebito(DTO_TarjetaCrearDebito dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public decimal limiteRetiro => _dto.limiteRetiro;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser un número válido mayor a cero");
            }

            if (limiteRetiro < 0)
            {
                throw new ArgumentException("El límite de retiro no puede ser negativo");
            }

            if (limiteRetiro == 0)
            {
                throw new ArgumentException("El límite de retiro debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

