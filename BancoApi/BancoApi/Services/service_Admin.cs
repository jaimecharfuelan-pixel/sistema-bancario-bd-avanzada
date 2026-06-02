using BancoApi.Repositories;
using BancoApi.Models.models_admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoApi.Services
{
    public class service_Admin
    {
        private readonly Repository_Admin _repository;

        public service_Admin(Repository_Admin repository)
        {
            _repository = repository;
        }

        public async Task<List<object>> function_obtenerSolicitudes()
        {
            return await _repository.ObtenerSolicitudes();
        }

        public async Task<int> function_crearCuenta(model_AdminCrearCuenta model)
        {
            model.Normalizar();
            model.Validar();

            return await _repository.CrearCuenta(model);
        }

    }
}
