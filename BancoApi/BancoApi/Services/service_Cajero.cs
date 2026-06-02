using BancoApi.Models.models_cajero;
using BancoApi.Models;
using BancoApi.Repositories;
using System;
using System.Collections.Generic;

namespace BancoApi.Services
{
    public class service_Cajero
    {
        private readonly Repository_Cajero _repository;

        public service_Cajero(Repository_Cajero repository)
        {
            _repository = repository;
        }

public async Task<int> function_crearCajero(model_CajeroCrear req)
        {

            req.Normalizar();

req.Validar();

return await _repository.CrearCajero(req.idSucursal, req.idAdministrador, req.dineroInicial);
        }

public async Task function_cambiarEstado(model_CajeroCambiarEstado req)
        {

            req.Normalizar();

req.Validar();

await _repository.CambiarEstado(req.idCajero, req.estado);
        }

public async Task function_recargar(model_CajeroRecargar req)
        {

            req.Normalizar();

req.Validar();

await _repository.Recargar(req.idCajero, req.monto);
        }

public async Task function_descontar(model_CajeroDescontar req)
        {

            req.Normalizar();

req.Validar();

await _repository.Descontar(req.idCajero, req.monto);
        }

public async Task<List<model_CajeroLista>> function_listar(model_CajeroListar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.Listar(req.idSucursal);
        }

public async Task<List<model_CajeroItem>> function_listarActivos(model_CajeroListarActivos req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarActivos();
        }

public async Task<List<model_CajeroItem>> function_obtenerPorIds(model_CajeroObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerPorIds(req.ids);
        }
    }
}
