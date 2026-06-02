using BancoApi.DTOs.DTOs_Prestamo;
using System;

namespace BancoApi.Models.models_prestamo
{

    public class model_PrestamoListarSolicitudes
    {
        private readonly DTO_PrestamoListarSolicitudes _dto;

        public model_PrestamoListarSolicitudes(DTO_PrestamoListarSolicitudes dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idSucursal => _dto.idSucursal;

public void Validar()
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de la sucursal debe ser mayor que cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

