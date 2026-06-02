using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteActualizarCedula
    {
        private readonly DTO_ClienteActualizarCedula _dto;
        private string? _idClienteNormalizado;

        public model_ClienteActualizarCedula(DTO_ClienteActualizarCedula dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public int cedula => _dto.cedula;

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
                throw new ArgumentException("El ID del cliente es requerido");

            if (cedula <= 0)
                throw new ArgumentException("La cédula debe ser un número válido");
        }

        public void Normalizar()
        {
            if (_dto.idCliente != null)
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();
        }
    }
}
