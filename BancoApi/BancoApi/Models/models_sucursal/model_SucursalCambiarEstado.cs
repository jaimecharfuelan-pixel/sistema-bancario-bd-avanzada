using BancoApi.DTOs.DTOs_Sucursal;
using System;

namespace BancoApi.Models.models_sucursal
{

    public class model_SucursalCambiarEstado
    {
        private readonly DTO_SucursalCambiarEstado _dto;
        private string? _estadoNormalizado;

        public model_SucursalCambiarEstado(DTO_SucursalCambiarEstado dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idSucursal => _dto.idSucursal;
        public string estado => _estadoNormalizado ?? _dto.estado;

public void Validar()
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de sucursal debe ser mayor a cero");
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new ArgumentException("El estado es requerido");
            }

            if (estado != "abierta" && estado != "cerrada")
            {
                throw new ArgumentException("El estado debe ser 'abierta' o 'cerrada'");
            }
        }

public void Normalizar()
        {
            if (_dto.estado != null)
            {
                _estadoNormalizado = _dto.estado.Trim().ToLower();
            }
        }
    }
}
