using BancoApi.DTOs.DTOs_Cliente;
using System;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteEliminar
    {
        private readonly DTO_ClienteEliminar _dto;
        private string? _idClienteNormalizado;

        public model_ClienteEliminar(DTO_ClienteEliminar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? (_dto.idCliente ?? string.Empty);

public void Validar()
        {
            var idParaValidar = _idClienteNormalizado ?? _dto.idCliente;
            if (string.IsNullOrWhiteSpace(idParaValidar))
            {
                throw new ArgumentException("El ID del cliente es requerido");
            }
        }

public void Normalizar()
        {
            if (_dto.idCliente != null)
            {
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();
            }
        }
    }
}

