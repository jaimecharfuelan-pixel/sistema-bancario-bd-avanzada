using BancoApi.DTOs.DTOs_Autenticacion;
using System;

namespace BancoApi.Models.models_autenticacion
{

    public class model_LoginRequest
    {
        private readonly DTO_AutenticacionLogin _dto;
        private string? _emailNormalizado;
        private string? _contrasenaNormalizada;

        public model_LoginRequest(DTO_AutenticacionLogin dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string Email => _emailNormalizado ?? _dto.email;
        public string Contrasena => _contrasenaNormalizada ?? _dto.contrasena;

public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentException("El email es requerido");
            }

            if (!Email.Contains("@") || !Email.Contains("."))
            {
                throw new ArgumentException("El email debe tener un formato válido");
            }

            if (string.IsNullOrWhiteSpace(Contrasena))
            {
                throw new ArgumentException("La contraseña es requerida");
            }
        }

public void Normalizar()
        {
            if (_dto.email != null)
            {
                _emailNormalizado = _dto.email.Trim().ToLower();
            }

            if (_dto.contrasena != null)
            {
                _contrasenaNormalizada = _dto.contrasena.Trim();
            }
        }
    }
}
