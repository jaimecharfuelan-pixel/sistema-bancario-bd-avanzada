using BancoApi.DTOs.DTOs_Cajero;
using System;

namespace BancoApi.Models.models_cajero
{

    public class model_CajeroCambiarEstado
    {
        private readonly DTO_CajeroCambiarEstado _dto;
        private string? _estadoNormalizado;

        public model_CajeroCambiarEstado(DTO_CajeroCambiarEstado dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCajero => _dto.idCajero;
        public string estado => _estadoNormalizado ?? _dto.estado;

public void Validar()
        {
            if (idCajero <= 0)
            {
                throw new ArgumentException("El ID de cajero debe ser mayor a cero");
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new ArgumentException("El estado es requerido");
            }

            if (estado != "activo" && estado != "inactivo")
            {
                throw new ArgumentException("El estado debe ser 'activo' o 'inactivo'");
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
