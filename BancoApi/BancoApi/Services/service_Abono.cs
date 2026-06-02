using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_abono;
using BancoApi.Repositories;

namespace BancoApi.Services
{
    public class service_Abono
    {
        private readonly Repository_Abono _repository;

        public service_Abono(Repository_Abono repository)
        {
            _repository = repository;
        }

public async Task<(int idAbono, int idTransaccion)> function_registrarAbono(
            model_AbonoRegistrar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.RegistrarAbono(
                req.idPrestamo,
                req.montoAbono,
                req.tipoAbono,
                req.idCuenta
            );
        }

public async Task<List<model_AbonoItem>> function_listarAbonos(
            model_AbonoListar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarAbonos(req.idPrestamo);
        }
    }
}
