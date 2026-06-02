using BancoApi.DTOs.DTOs_Cliente;
using System;
using System.Linq;

namespace BancoApi.Models.models_cliente
{

    public class model_ClienteObtenerPorIds
    {
        private readonly DTO_ClienteObtenerPorIds _dto;
        private List<string>? _idsNormalizados;

        public model_ClienteObtenerPorIds(DTO_ClienteObtenerPorIds dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public List<string> ids => _idsNormalizados ?? (_dto.ids ?? new List<string>());

public void Validar()
        {
            var idsParaValidar = _idsNormalizados ?? _dto.ids;
            if (idsParaValidar == null || idsParaValidar.Count == 0)
            {
                throw new ArgumentException("Debe proporcionar al menos un ID de cliente");
            }

            if (idsParaValidar.Any(id => string.IsNullOrWhiteSpace(id)))
            {
                throw new ArgumentException("Todos los IDs deben ser válidos");
            }
        }

public void Normalizar()
        {

            if (_dto.ids != null)
            {
                _idsNormalizados = _dto.ids
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => id.Trim().ToUpper())
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
            }
            else
            {
                _idsNormalizados = new List<string>();
            }
        }
    }
}

