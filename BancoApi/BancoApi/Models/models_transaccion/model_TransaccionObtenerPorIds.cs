using BancoApi.DTOs.DTOs_Transaccion;
using System;
using System.Linq;

namespace BancoApi.Models.models_transaccion
{

    public class model_TransaccionObtenerPorIds
    {
        private readonly DTO_TransaccionObtenerPorIds _dto;

        public model_TransaccionObtenerPorIds(DTO_TransaccionObtenerPorIds dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public List<int> ids => _dto.ids ?? new List<int>();

public void Validar()
        {
            if (ids == null || ids.Count == 0)
            {
                throw new ArgumentException("La lista de IDs no puede estar vacía");
            }

            if (ids.Any(id => id <= 0))
            {
                throw new ArgumentException("Todos los IDs deben ser mayores a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

