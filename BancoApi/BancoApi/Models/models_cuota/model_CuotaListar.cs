using BancoApi.DTOs.DTOs_Cuota;
using System;

namespace BancoApi.Models.models_cuota
{

    public class model_CuotaListar
    {
        private readonly DTO_CuotaListar _dto;

        public model_CuotaListar(DTO_CuotaListar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idPrestamo => _dto.idPrestamo;

public void Validar()
        {
            if (idPrestamo <= 0)
            {
                throw new ArgumentException("El ID del préstamo debe ser mayor que cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

