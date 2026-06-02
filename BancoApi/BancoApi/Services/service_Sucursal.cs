using BancoApi.Models.models_sucursal;
using BancoApi.Models;
using BancoApi.Repositories;
using System;
using System.Linq;

namespace BancoApi.Services
{
    public class service_Sucursal
    {
        private readonly Repository_Sucursal _repository;

        public service_Sucursal(Repository_Sucursal repository)
        {
            _repository = repository;
        }

public async Task<List<model_SucursalLista>> function_listarSucursales(
            model_SucursalListar req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarSucursales();
        }

public async Task<int> function_crearSucursal(model_SucursalCrear request)
        {
           
            request.Normalizar();

request.Validar();

return await _repository.CrearSucursal(
                request.nombreSucursal,
                request.direccionSucursal,
                request.telefonoSucursal,
                request.idAdministrador
            );
        }

public async Task function_editarEstado(model_SucursalCambiarEstado req)
        {
            
            req.Normalizar();

            req.Validar();

var sucursales = await _repository.ListarSucursales();
            var sucursalExiste = sucursales.Any(s => s.idSucursal == req.idSucursal);
            if (!sucursalExiste)
            {
                throw new ArgumentException("La sucursal no existe");
            }

await _repository.EditarEstado(req.idSucursal, req.estado);
        }

public async Task function_eliminarSucursal(model_SucursalEliminar req)
        {

            req.Normalizar();

req.Validar();

var sucursales = await _repository.ListarSucursales();
            var sucursalExiste = sucursales.Any(s => s.idSucursal == req.idSucursal);
            if (!sucursalExiste)
            {
                throw new ArgumentException("La sucursal no existe");
            }

await _repository.EliminarSucursal(req.idSucursal);
        }

public async Task<List<model_SucursalLista>> function_obtenerSucursalesPorIds(
            model_SucursalObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerSucursalesPorIds(req.ids);
        }

public async Task<List<model_SucursalLista>> function_listarSucursalesAbiertas(
            model_SucursalListarAbiertas req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarSucursalesAbiertas();
        }
    }
}
