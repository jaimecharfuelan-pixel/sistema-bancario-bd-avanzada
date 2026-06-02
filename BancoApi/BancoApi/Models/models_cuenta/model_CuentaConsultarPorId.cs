using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaConsultarPorId
    {
        private readonly DTO_CuentaConsultarPorId _dto;

        public model_CuentaConsultarPorId(DTO_CuentaConsultarPorId dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}
