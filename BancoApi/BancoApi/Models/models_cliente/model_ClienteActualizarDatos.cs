using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteActualizarDatos
    {
        private readonly DTO_ClienteActualizarDatos _dto;

private string? _idClienteNormalizado;
        private string? _nombreNormalizado;
        private string? _emailNormalizado;
        private string? _telefonoNormalizado;

        public model_ClienteActualizarDatos(DTO_ClienteActualizarDatos dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public string nombre => _nombreNormalizado ?? _dto.nombre;
        public string email => _emailNormalizado ?? _dto.email;
        public string telefono => _telefonoNormalizado ?? _dto.telefono;
        public int cedula => _dto.cedula;

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
                throw new ArgumentException("El ID del cliente es requerido");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es requerido");

            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("El email debe tener un formato válido");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono es requerido");

            if (cedula <= 0)
                throw new ArgumentException("La cédula debe ser un número válido");
        }

        public void Normalizar()
        {
            if (_dto.idCliente != null)
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();

            if (_dto.nombre != null)
                _nombreNormalizado = _dto.nombre.Trim();

            if (_dto.email != null)
                _emailNormalizado = _dto.email.Trim().ToLower();

            if (_dto.telefono != null)
                _telefonoNormalizado = _dto.telefono.Trim();
        }
    }
}
