using BancoApi.DTOs.DTOs_Admin;
using System;

namespace BancoApi.Models.models_admin
{

    public class model_AdminCrearCuenta
    {
        private readonly DTO_AdminCrearCuenta _dto;
        private string? _idClienteNormalizado;

        public model_AdminCrearCuenta(DTO_AdminCrearCuenta? dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;
        public int idAdministrador => _dto.idAdministrador;

public void Validar()
        {
            var idParaValidar = _idClienteNormalizado ?? _dto.idCliente;
            if (string.IsNullOrWhiteSpace(idParaValidar))
            {
                throw new ArgumentException("El ID del cliente es requerido");
            }

            if (idParaValidar.Length > 10)
            {
                throw new ArgumentException("El ID del cliente no puede exceder 10 caracteres");
            }

            if (idAdministrador <= 0)
            {
                throw new ArgumentException("El ID del administrador debe ser mayor a cero");
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

