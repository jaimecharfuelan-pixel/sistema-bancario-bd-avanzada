using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaCambiarContrasenaCuenta
    {
        private readonly DTO_CuentaCambiarContrasena _dto;
        private string? _contrasenaNormalizada;

        public model_CuentaCambiarContrasenaCuenta(DTO_CuentaCambiarContrasena dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public string nuevaContrasena => _contrasenaNormalizada ?? _dto.nuevaContrasena;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser mayor a cero");
            }

            if (string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                throw new ArgumentException("La nueva contraseña es requerida");
            }

            if (nuevaContrasena.Length < 4)
            {
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres");
            }
        }

public void Normalizar()
        {
            if (_dto.nuevaContrasena != null)
            {
                _contrasenaNormalizada = _dto.nuevaContrasena.Trim();
            }
        }
    }
}
