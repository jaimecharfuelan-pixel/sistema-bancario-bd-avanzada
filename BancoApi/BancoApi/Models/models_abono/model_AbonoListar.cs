using BancoApi.DTOs.DTO_Abono;
using System;

namespace BancoApi.Models.models_abono
{

    public class model_AbonoListar
    {
        private readonly DTO_AbonoListar _dto;

        public model_AbonoListar(DTO_AbonoListar dto)
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
