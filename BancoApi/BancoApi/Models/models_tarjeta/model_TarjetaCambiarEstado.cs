using BancoApi.DTOs.DTOs_Tarjeta;
using System;
using System.Linq;

namespace BancoApi.Models.models_tarjeta
{

    public class model_TarjetaCambiarEstado
    {
        private readonly DTO_TarjetaCambiarEstado _dto;
        private string? _estadoNormalizado;

        public model_TarjetaCambiarEstado(DTO_TarjetaCambiarEstado dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idTarjeta => _dto.idTarjeta;
        public string estado => _estadoNormalizado ?? _dto.estado;

public void Validar()
        {
            if (idTarjeta <= 0)
            {
                throw new ArgumentException("El ID de tarjeta debe ser un número válido mayor a cero");
            }

            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new ArgumentException("El estado es requerido");
            }

var estadosValidos = new[] { "ACTIVA", "INACTIVA", "BLOQUEADA", "SUSPENDIDA" };
            var estadoUpper = estado.ToUpper().Trim();

            if (!estadosValidos.Contains(estadoUpper))
            {
                throw new ArgumentException($"El estado debe ser uno de los siguientes: {string.Join(", ", estadosValidos)}");
            }
        }

public void Normalizar()
        {
            if (_dto.estado != null)
            {
                _estadoNormalizado = _dto.estado.Trim().ToUpper();
            }
        }
    }
}
