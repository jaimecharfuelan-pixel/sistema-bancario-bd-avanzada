using BancoApi.DTOs.DTOs_Sucursal;
using System;
using System.Linq;

namespace BancoApi.Models.models_sucursal
{

    public class model_SucursalObtenerPorIds
    {
        private readonly DTO_SucursalObtenerPorIds _dto;
        private List<int>? _idsNormalizados;

        public model_SucursalObtenerPorIds(DTO_SucursalObtenerPorIds dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public List<int> ids => _idsNormalizados ?? (_dto.ids ?? new List<int>());

public void Validar()
        {
            var idsParaValidar = _idsNormalizados ?? _dto.ids;
            if (idsParaValidar == null || idsParaValidar.Count == 0)
            {
                throw new ArgumentException("Debe proporcionar al menos un ID de sucursal");
            }

            if (idsParaValidar.Any(id => id <= 0))
            {
                throw new ArgumentException("Todos los IDs deben ser mayores que cero");
            }
        }

public void Normalizar()
        {

            if (_dto.ids != null)
            {
                _idsNormalizados = _dto.ids.Distinct().OrderBy(x => x).ToList();
            }
            else
            {
                _idsNormalizados = new List<int>();
            }
        }
    }
}

