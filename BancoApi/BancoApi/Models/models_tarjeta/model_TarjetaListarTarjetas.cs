using BancoApi.DTOs.DTOs_Tarjeta;
using System;

namespace BancoApi.Models.models_tarjeta
{

public class model_TarjetaListarTarjetas
    {
        private readonly DTO_TarjetaListarTarjetas _dto;

        public model_TarjetaListarTarjetas(DTO_TarjetaListarTarjetas dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser un número válido mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

