using BancoApi.DTOs.DTOs_Cajero;
using System;

namespace BancoApi.Models.models_cajero
{

    public class model_CajeroListar
    {
        private readonly DTO_CajeroListar _dto;

        public model_CajeroListar(DTO_CajeroListar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idSucursal => _dto.idSucursal;

public void Validar()
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de sucursal debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

