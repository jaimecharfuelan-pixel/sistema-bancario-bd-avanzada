using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaActualizar
    {
        private readonly DTO_CuentaActualizar _dto;
        private string? _estadoNormalizado;

        public model_CuentaActualizar(DTO_CuentaActualizar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public decimal? saldo => _dto.saldo;
        public string? estado => _estadoNormalizado ?? _dto.estado;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser mayor a cero");
            }

            if (estado != null && estado != "activa" && estado != "inactiva")
            {
                throw new ArgumentException("El estado debe ser 'activa' o 'inactiva'");
            }

            if (saldo.HasValue && saldo.Value < 0)
            {
                throw new ArgumentException("El saldo no puede ser negativo");
            }
        }

public void Normalizar()
        {
            if (_dto.estado != null)
            {
                _estadoNormalizado = _dto.estado.Trim().ToLower();
            }
        }
    }
}
