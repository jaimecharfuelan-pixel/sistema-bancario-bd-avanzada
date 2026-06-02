using BancoApi.DTOs.DTOs_Cuenta;
using System;

namespace BancoApi.Models.models_cuenta
{

    public class model_CuentaConsultarPorCliente
    {
        private readonly DTO_CuentaConsultarPorCliente _dto;
        private string? _idClienteNormalizado;

        public model_CuentaConsultarPorCliente(DTO_CuentaConsultarPorCliente dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string idCliente => _idClienteNormalizado ?? _dto.idCliente;

public void Normalizar()
        {
            if (!string.IsNullOrWhiteSpace(_dto.idCliente))
            {
                _idClienteNormalizado = _dto.idCliente.Trim().ToUpper();
            }
        }

public void Validar()
        {
            if (string.IsNullOrWhiteSpace(idCliente))
            {
                throw new ArgumentException("El ID del cliente es requerido");
            }

            if (idCliente.Length > 10)
            {
                throw new ArgumentException("El ID del cliente no puede tener más de 10 caracteres");
            }
        }
    }
}
