using BancoApi.Models.models_tarjeta;
using BancoApi.Repositories;
using BancoApi.DTOs.DTOs_Tarjeta;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoApi.Services
{

    public class service_Tarjeta
    {
        private readonly Repository_Tarjeta _repository;

        public service_Tarjeta(Repository_Tarjeta repository)
        {
            _repository = repository;
        }

public async Task<int> function_crearTarjetaDebito(model_TarjetaCrearDebito req)
        {

            req.Normalizar();

req.Validar();

var idTarjeta = await _repository.CrearTarjetaDebito(
                req.idCuenta,
                req.limiteRetiro
            );

if (idTarjeta <= 0)
            {
                throw new Exception("Error al crear la tarjeta de débito. No se obtuvo un ID válido.");
            }

            return idTarjeta;
        }

public async Task<int> function_crearTarjetaCredito(model_TarjetaCrearCredito req)
        {

            req.Normalizar();

req.Validar();

var idTarjeta = await _repository.CrearTarjetaCredito(
                req.idCuenta,
                req.limiteCredito,
                req.tasaInteres,
                req.cuotaManejo,
                req.fechaCorte
            );

if (idTarjeta <= 0)
            {
                throw new Exception("Error al crear la tarjeta de crédito. No se obtuvo un ID válido.");
            }

            return idTarjeta;
        }

public async Task function_cambiarEstadoTarjeta(model_TarjetaCambiarEstado req)
        {

            req.Normalizar();

req.Validar();

await _repository.CambiarEstadoTarjeta(
                req.idTarjeta,
                req.estado
            );
        }

public async Task<List<model_TarjetaItem>> function_listarTarjetas(model_TarjetaListarTarjetas req)
        {

            req.Normalizar();

req.Validar();

var tarjetas = await _repository.ListarTarjetas(req.idCuenta);

            return tarjetas;
        }

public async Task<List<model_TarjetaItem>> function_listarActivas(
            model_TarjetaListarActivas req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarActivas();
        }

public async Task<List<model_TarjetaItem>> function_obtenerPorIds(
            model_TarjetaObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerPorIds(req.ids);
        }
    }
}
