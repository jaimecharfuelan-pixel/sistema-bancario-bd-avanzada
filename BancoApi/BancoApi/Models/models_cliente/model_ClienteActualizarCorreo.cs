using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteActualizarCorreo
    {
        private readonly DTO_ClienteActualizarCorreo _dto;
        private string? _idClienteNormalizado;
        private string? _nuevoCorreoNormalizado;

        public model_ClienteActualizarCorreo(DTO_ClienteActualizarCorreo dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public string nuevoCorreo => _nuevoCorreoNormalizado ?? _dto.nuevoCorreo;

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
                throw new ArgumentException("El ID del cliente es requerido");

            if (string.IsNullOrWhiteSpace(nuevoCorreo))
                throw new ArgumentException("El nuevo correo es requerido");

            if (!nuevoCorreo.Contains("@") || !nuevoCorreo.Contains("."))
                throw new ArgumentException("El email debe tener un formato válido");
        }

        public void Normalizar()
        {
            if (_dto.idCliente != null)
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();

            if (_dto.nuevoCorreo != null)
                _nuevoCorreoNormalizado = _dto.nuevoCorreo.Trim().ToLower();
        }
    }
}
