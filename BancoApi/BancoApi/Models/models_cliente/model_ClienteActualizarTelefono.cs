using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteActualizarTelefono
    {
        private readonly DTO_ClienteActualizarTelefono _dto;
        private string? _idClienteNormalizado;
        private string? _telefonoNormalizado;

        public model_ClienteActualizarTelefono(DTO_ClienteActualizarTelefono dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public string telefono => _telefonoNormalizado ?? _dto.telefono;

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
                throw new ArgumentException("El ID del cliente es requerido");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono es requerido");
        }

        public void Normalizar()
        {
            if (_dto.idCliente != null)
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();

            if (_dto.telefono != null)
                _telefonoNormalizado = _dto.telefono.Trim();
        }
    }
}
