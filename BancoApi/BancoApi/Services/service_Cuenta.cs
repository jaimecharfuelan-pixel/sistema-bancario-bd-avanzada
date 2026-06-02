using BancoApi.Models.models_cuenta;
using BancoApi.Repositories;
using System;

namespace BancoApi.Services
{
    public class service_Cuenta
    {
        private readonly Repository_Cuenta _repository;

        public service_Cuenta(Repository_Cuenta repository)
        {
            _repository = repository;
        }

public async Task<List<object>> function_consultarPorCliente(model_CuentaConsultarPorCliente req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ConsultarPorCliente(req.idCliente);
        }

public async Task<object?> function_consultarPorId(model_CuentaConsultarPorId req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ConsultarPorId(req.idCuenta);
        }

public async Task function_actualizarCuenta(model_CuentaActualizar req)
        {

            req.Normalizar();

req.Validar();

var cuentaExistente = await _repository.ConsultarPorId(req.idCuenta);
            if (cuentaExistente == null)
            {
                throw new ArgumentException("La cuenta no existe");
            }

await _repository.ActualizarCuenta(req.idCuenta, req.saldo, req.estado);
        }

public async Task function_cambiarSaldo(model_CuentaCambiarSaldo req)
        {

            req.Normalizar();

req.Validar();

var cuenta = await _repository.ConsultarPorId(req.idCuenta);
            if (cuenta == null)
            {
                throw new ArgumentException("La cuenta no existe");
            }

await _repository.CambiarSaldo(req.idCuenta, req.monto);
        }

public async Task function_cambiarContrasena(model_CuentaCambiarContrasenaCuenta req)
        {

            req.Normalizar();

req.Validar();

var cuenta = await _repository.ConsultarPorId(req.idCuenta);
            if (cuenta == null)
            {
                throw new ArgumentException("La cuenta no existe");
            }

await _repository.CambiarContrasena(req.idCuenta, req.nuevaContrasena);
        }

public async Task function_eliminarCuenta(model_CuentaEliminar req)
        {

            req.Normalizar();

req.Validar();

var cuenta = await _repository.ConsultarPorId(req.idCuenta);
            if (cuenta == null)
            {
                throw new ArgumentException("La cuenta no existe");
            }

await _repository.EliminarCuenta(req.idCuenta);
        }

public async Task<List<model_CuentaItem>> function_listarActivas(
            model_CuentaListarActivas req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarActivas();
        }

public async Task<List<model_CuentaItem>> function_obtenerPorIds(
            model_CuentaObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerPorIds(req.ids);
        }
    }
}
