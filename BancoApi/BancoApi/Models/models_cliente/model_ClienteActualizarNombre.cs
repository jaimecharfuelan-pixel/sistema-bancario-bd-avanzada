using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteActualizarNombre
    {
        private readonly DTO_ClienteActualizarNombre _dto;
        private string? _idClienteNormalizado;
        private string? _nuevoNombreNormalizado;

        public model_ClienteActualizarNombre(DTO_ClienteActualizarNombre dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public string nuevoNombre => _nuevoNombreNormalizado ?? _dto.nuevoNombre;

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
                throw new ArgumentException("El ID del cliente es requerido");

            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nuevo nombre es requerido");
        }

        public void Normalizar()
        {
            if (_dto.idCliente != null)
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();

            if (_dto.nuevoNombre != null)
                _nuevoNombreNormalizado = _dto.nuevoNombre.Trim();
        }
    }
}
