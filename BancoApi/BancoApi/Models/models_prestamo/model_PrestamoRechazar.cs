using BancoApi.DTOs.DTOs_Prestamo;
using System;

namespace BancoApi.Models.models_prestamo
{

    public class model_PrestamoRechazar
    {
        private readonly DTO_PrestamoRechazar _dto;

        public model_PrestamoRechazar(DTO_PrestamoRechazar dto)
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

