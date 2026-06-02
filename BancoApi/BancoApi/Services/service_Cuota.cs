using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_cuota;
using BancoApi.Repositories;

namespace BancoApi.Services
{
    public class service_Cuota
    {
        private readonly Repository_Cuota _repository;

        public service_Cuota(Repository_Cuota repository)
        {
            _repository = repository;
        }

public async Task function_generarCuotas(model_CuotaGenerar req)
        {

            req.Normalizar();

req.Validar();

await _repository.GenerarCuotas(req.idPrestamo);
        }

public async Task<int> function_pagarCuota(model_CuotaPagar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.PagarCuota(req.idCuota, req.idCuenta);
        }

public async Task<List<model_CuotaItem>> function_listarCuotas(model_CuotaListar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarCuotas(req.idPrestamo);
        }
    }
}

