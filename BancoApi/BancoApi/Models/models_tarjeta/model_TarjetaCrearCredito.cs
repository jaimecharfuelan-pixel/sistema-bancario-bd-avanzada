using BancoApi.DTOs.DTOs_Tarjeta;
using System;

namespace BancoApi.Models.models_tarjeta
{

    public class model_TarjetaCrearCredito
    {
        private DTO_TarjetaCrearCredito _dto;

        public model_TarjetaCrearCredito(DTO_TarjetaCrearCredito dto)
        {
            _dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }

public int idCuenta => _dto.idCuenta;
        public decimal limiteCredito => _dto.limiteCredito;
        public decimal tasaInteres => _dto.tasaInteres;
        public decimal cuotaManejo => _dto.cuotaManejo;
        public int fechaCorte => _dto.fechaCorte;

public void Validar()
        {
            if (idCuenta <= 0)
            {
                throw new ArgumentException("El ID de cuenta debe ser un número válido mayor a cero");
            }

            if (limiteCredito < 0)
            {
                throw new ArgumentException("El límite de crédito no puede ser negativo");
            }

            if (limiteCredito == 0)
            {
                throw new ArgumentException("El límite de crédito debe ser mayor a cero");
            }

            if (tasaInteres < 0 || tasaInteres > 100)
            {
                throw new ArgumentException("La tasa de interés debe estar entre 0 y 100");
            }

            if (cuotaManejo < 0)
            {
                throw new ArgumentException("La cuota de manejo no puede ser negativa");
            }

            if (fechaCorte < 1 || fechaCorte > 31)
            {
                throw new ArgumentException("La fecha de corte debe ser un día válido (1-31)");
            }
        }

public void Normalizar()
        {

        }
    }
}

