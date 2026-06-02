using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaCambiarSaldo
    {
        private readonly DTO_CuentaCambiarSaldo _dto;

        public model_CuentaCambiarSaldo(DTO_CuentaCambiarSaldo dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public decimal monto => _dto.monto;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser mayor a cero");
            }

            if (monto < 0)
            {
                throw new ArgumentException("El saldo no puede ser negativo");
            }
        }

public void Normalizar()
        {

        }
    }
}
