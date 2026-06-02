using BancoApi.DTOs.DTOs_Sucursal;
using System;

namespace BancoApi.Models.models_sucursal
{
    
    public class model_SucursalCrear
    {
        private readonly DTO_SucursalCrear _dto;

private string? _nombreNormalizado;
        private string? _direccionNormalizada;
        private string? _telefonoNormalizado;

        public model_SucursalCrear(DTO_SucursalCrear dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public string nombreSucursal => _nombreNormalizado ?? _dto.nombreSucursal;
        public string direccionSucursal => _direccionNormalizada ?? _dto.direccionSucursal;
        public string telefonoSucursal => _telefonoNormalizado ?? _dto.telefonoSucursal;
        public int? idAdministrador => _dto.idAdministrador;

public void Validar()
        {
            if (string.IsNullOrWhiteSpace(nombreSucursal))
            {
                throw new ArgumentException("El nombre de la sucursal es requerido");
            }

            if (string.IsNullOrWhiteSpace(direccionSucursal))
            {
                throw new ArgumentException("La dirección de la sucursal es requerida");
            }

            if (string.IsNullOrWhiteSpace(telefonoSucursal))
            {
                throw new ArgumentException("El teléfono de la sucursal es requerido");
            }
        }

public void Normalizar()
        {
            if (_dto.nombreSucursal != null)
            {
                _nombreNormalizado = _dto.nombreSucursal.Trim();
            }

            if (_dto.direccionSucursal != null)
            {
                _direccionNormalizada = _dto.direccionSucursal.Trim();
            }

            if (_dto.telefonoSucursal != null)
            {
                _telefonoNormalizado = _dto.telefonoSucursal.Trim();
            }
        }
    }
}
