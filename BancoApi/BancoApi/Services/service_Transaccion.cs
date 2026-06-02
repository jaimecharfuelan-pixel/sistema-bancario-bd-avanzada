using BancoApi.Models.models_transaccion;
using BancoApi.Repositories;
using System;

namespace BancoApi.Services
{
    public class service_Transaccion
    {
        private readonly Repository_Transaccion _repository;

        public service_Transaccion(Repository_Transaccion repository)
        {
            _repository = repository;
        }

public async Task<int> function_transferencia(model_TransaccionTransferencia req)
        {

            req.Normalizar();

req.Validar();

return await _repository.Transferencia(req.cuentaOrigen, req.cuentaDestino, req.monto);
        }

public async Task<int> function_retiroDebito(model_TransaccionRetiroDebito req)
        {

            req.Normalizar();

req.Validar();

return await _repository.RetiroDebito(req.idTarjeta, req.idCajero, req.monto);
        }

public async Task<int> function_retiroCredito(model_TransaccionRetiroCredito req)
        {

            req.Normalizar();

req.Validar();

return await _repository.RetiroCredito(req.idTarjeta, req.idCajero, req.monto);
        }

public async Task<List<model_TransaccionItem>> function_historialCuenta(model_TransaccionHistorialCuenta req)
        {

            req.Normalizar();

req.Validar();

return await _repository.HistorialCuenta(req.idCuenta);
        }

public async Task<List<model_TransaccionItem>> function_historialCajero(model_TransaccionHistorialCajero req)
        {

            req.Normalizar();

req.Validar();

return await _repository.HistorialCajero(req.idCuenta);
        }

public async Task<List<model_TransaccionItem>> function_obtenerTransaccionesPorIds(model_TransaccionObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerTransaccionesPorIds(req.ids);
        }

public async Task<List<model_TransaccionItem>> function_listarTransaccionesHoy(
            model_TransaccionListarHoy req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarTransaccionesHoy();
        }
    }
}

