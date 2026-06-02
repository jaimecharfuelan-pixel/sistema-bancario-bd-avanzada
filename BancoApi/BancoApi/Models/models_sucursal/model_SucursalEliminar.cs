using BancoApi.DTOs.DTOs_Sucursal;
using System;

namespace BancoApi.Models.models_sucursal
{

    public class model_SucursalEliminar
    {
        private readonly DTO_SucursalEliminar _dto;

        public model_SucursalEliminar(DTO_SucursalEliminar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idSucursal => _dto.idSucursal;

public void Validar()
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de sucursal debe ser mayor a cero");
            }
        }

public void Normalizar()
        {

        }
    }
}

