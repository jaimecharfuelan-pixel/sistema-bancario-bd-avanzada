using BancoApi.DTOs.DTOs_Cuota;
using System;

namespace BancoApi.Models.models_cuota
{

    public class model_CuotaPagar
    {
        private readonly DTO_CuotaPagar _dto;

        public model_CuotaPagar(DTO_CuotaPagar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuota => _dto.idCuota;
        public int idCuenta => _dto.idCuenta;

public void Validar()
        {
            if (idCuota <= 0)
            {
                throw new ArgumentException("El ID de la cuota debe ser mayor que cero");
            }

            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de la cuenta debe ser mayor que cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

