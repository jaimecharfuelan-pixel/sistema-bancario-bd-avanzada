using BancoApi.Models.models_cliente;
using BancoApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BancoApi.Services
{

    public class service_Cliente
    {
        private readonly Repository_Cliente _repository;

        public service_Cliente(Repository_Cliente repository)
        {
            _repository = repository;
        }

public async Task<string> function_crearCliente(model_ClienteCrear prm_request)
        {

            prm_request.Normalizar();

prm_request.Validar();

return await _repository.CrearCliente(
                prm_request.prm_nombre_cliente,
                prm_request.prm_email_cliente,
                prm_request.prm_telefono_cliente,
                prm_request.prm_cedula_cliente,
                "ACTIVO"
            );
        }

public async Task function_actualizarDatos(model_ClienteActualizarDatos req)
        {

            req.Normalizar();
            req.Validar();

await _repository.ActualizarDatosCliente(
                req.idCliente,
                req.nombre,
                req.email,
                req.telefono,
                req.cedula
            );
        }

public async Task function_actualizarCorreo(model_ClienteActualizarCorreo req)
        {
            req.Normalizar();
            req.Validar();
            await _repository.ActualizarCorreo(req.idCliente, req.nuevoCorreo);
        }

public async Task function_actualizarNombre(model_ClienteActualizarNombre req)
        {
            req.Normalizar();
            req.Validar();
            await _repository.ActualizarNombre(req.idCliente, req.nuevoNombre);
        }

public async Task function_actualizarTelefono(model_ClienteActualizarTelefono req)
        {
            req.Normalizar();
            req.Validar();
            await _repository.ActualizarTelefono(req.idCliente, req.telefono);
        }

public async Task function_actualizarCedula(model_ClienteActualizarCedula req)
        {

            req.Normalizar();

req.Validar();

await _repository.ActualizarCedula(req.idCliente, req.cedula);
        }

public async Task function_eliminarCliente(model_ClienteEliminar req)
        {

            req.Normalizar();

req.Validar();

await _repository.EliminarCliente(req.idCliente);
        }

public async Task<List<model_ClienteItem>> function_listarActivos(
            model_ClienteListarActivos req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ListarActivos();
        }

public async Task<List<model_ClienteItem>> function_obtenerPorIds(
            model_ClienteObtenerPorIds req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ObtenerPorIds(req.ids);
        }

public async Task<int> function_procesarMasivos(
            model_ClienteProcesarMasivos req)
        {

            req.Normalizar();

req.Validar();

return await _repository.ProcesarMasivos(req.clientes);
        }
    }
}
