using BancoApi.DTOs.DTOs_Transaccion;
using System;

namespace BancoApi.Models.models_transaccion
{

    public class model_TransaccionHistorialCuenta
    {
        private readonly DTO_TransaccionHistorialCuenta _dto;

        public model_TransaccionHistorialCuenta(DTO_TransaccionHistorialCuenta dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta es requerido y debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

