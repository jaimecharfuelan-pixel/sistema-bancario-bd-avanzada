using BancoApi.DTOs.DTO_Abono;
using System;

namespace BancoApi.Models.models_abono
{

    public class model_AbonoRegistrar
    {
        private readonly DTO_AbonoRegistrar _dto;
        private string? _tipoAbonoNormalizado;

        public model_AbonoRegistrar(DTO_AbonoRegistrar dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idPrestamo => _dto.idPrestamo;
        public decimal montoAbono => _dto.montoAbono;
        public string tipoAbono => _tipoAbonoNormalizado ?? _dto.tipoAbono;
        public int idCuenta => _dto.idCuenta;

public void Validar()
        {
            if (idPrestamo <= 0)
            {
                throw new ArgumentException("El ID del préstamo debe ser mayor que cero");
            }

            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de la cuenta debe ser mayor que cero");
            }

            if (montoAbono <= 0)
            {
                throw new ArgumentException("El monto del abono debe ser mayor que cero");
            }

            if (string.IsNullOrWhiteSpace(tipoAbono))
            {
                throw new ArgumentException("El tipo de abono es requerido");
            }
        }

public void Normalizar()
        {
            if (_dto.tipoAbono != null)
            {
                _tipoAbonoNormalizado = _dto.tipoAbono.Trim();
            }
        }
    }
}
