using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_prestamo;
using BancoApi.Repositories;

namespace BancoApi.Services
{
    public class service_Prestamo
    {
        private readonly Repository_Prestamo _repository;

        public service_Prestamo(Repository_Prestamo repository)
        {
            _repository = repository;
        }

public async Task<int> function_solicitarPrestamo(model_PrestamoSolicitar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.SolicitarPrestamo(
                req.idCuenta,
                req.idSucursal,
                req.monto,
                req.fechaInicio,
                req.fechaFin
            );
        }

public async Task<List<model_PrestamoSolicitudItem>> function_listarSolicitudes(
            model_PrestamoListarSolicitudes req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarSolicitudes(req.idSucursal);
        }

public async Task function_aceptarPrestamo(model_PrestamoAceptar req)
        {

            req.Normalizar();

req.Validar();

await _repository.AceptarPrestamo(req.idPrestamo);
        }

public async Task function_rechazarPrestamo(model_PrestamoRechazar req)
        {

            req.Normalizar();

req.Validar();

await _repository.RechazarPrestamo(req.idPrestamo);
        }

public async Task<List<model_PrestamoItem>> function_listarActivos(
            model_PrestamoListarActivos req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarActivos();
        }

public async Task<List<model_PrestamoItem>> function_obtenerPorIds(
            model_PrestamoObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerPorIds(req.ids);
        }
    }
}

