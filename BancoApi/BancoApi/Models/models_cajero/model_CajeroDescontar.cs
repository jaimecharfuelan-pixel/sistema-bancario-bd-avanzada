using BancoApi.DTOs.DTOs_Cajero;
using System;

namespace BancoApi.Models.models_cajero
{

    public class model_CajeroDescontar
    {
        private readonly DTO_CajeroDescontar _dto;

        public model_CajeroDescontar(DTO_CajeroDescontar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCajero => _dto.idCajero;
        public decimal monto => _dto.monto;

public void Validar()
        {
            if (idCajero <= 0)
            {
                throw new ArgumentException("El ID de cajero debe ser mayor a cero");
            }

            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}
