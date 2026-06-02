using BancoApi.DTOs.DTOs_Prestamo;
using System;

namespace BancoApi.Models.models_prestamo
{

    public class model_PrestamoSolicitar
    {
        private readonly DTO_PrestamoSolicitar _dto;

        public model_PrestamoSolicitar(DTO_PrestamoSolicitar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public int idSucursal => _dto.idSucursal;
        public decimal monto => _dto.monto;
        public DateTime fechaInicio => _dto.fechaInicio;
        public DateTime fechaFin => _dto.fechaFin;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de la cuenta debe ser mayor que cero");
            }

            if (idSucursal <= 0)
            {
                throw new ArgumentException("El ID de la sucursal debe ser mayor que cero");
            }

            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor que cero");
            }

            if (fechaFin <= fechaInicio)
            {
                throw new ArgumentException("La fecha fin debe ser mayor que la fecha inicio");
            }
        }

public void Normalizar()
        {

        }
    }
}

