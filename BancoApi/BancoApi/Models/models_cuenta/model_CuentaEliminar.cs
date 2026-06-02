using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaEliminar
    {
        private readonly DTO_CuentaEliminar _dto;

        public model_CuentaEliminar(DTO_CuentaEliminar dto)
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

