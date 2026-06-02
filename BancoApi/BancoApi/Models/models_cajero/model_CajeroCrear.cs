using BancoApi.DTOs.DTOs_Cajero;
using System;

namespace BancoApi.Models.models_cajero
{

    public class model_CajeroCrear
    {
        private readonly DTO_CajeroCrear _dto;

        public model_CajeroCrear(DTO_CajeroCrear dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idSucursal => _dto.idSucursal;
        public int idAdministrador => _dto.idAdministrador;
        public decimal dineroInicial => _dto.dineroInicial;

public void Validar()
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de sucursal debe ser mayor a cero");
            }

            if (idAdministrador <= 0)
            {
                throw new ArgumentException("El ID de administrador debe ser mayor a cero");
            }

            if (dineroInicial < 0)
            {
                throw new ArgumentException("El dinero inicial no puede ser negativo");
            }
        }

public void Normalizar()
        {

        }
    }
}
