using BancoApi.Models.models_autenticacion;
using BancoApi.Repositories;
using System;

namespace BancoApi.Services
{
    public class service_Autenticacion
    {
        private readonly Repository_Autenticacion _repository;
        private readonly service_JWT _jwtService;

        public service_Autenticacion(Repository_Autenticacion repository, service_JWT jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<object?> function_login(model_LoginRequest req)
        {

            req.Normalizar();

req.Validar();

var adminId = await _repository.LoginAdmin(req.Email, req.Contrasena);

            if (!string.IsNullOrEmpty(adminId))
            {
                var token = _jwtService.GenerarToken(adminId, "ADMIN");

                return new
                {
                    tipoUsuario = "ADMIN",
                    id = adminId,
                    token = token
                };
            }

var cuentaId = await _repository.LoginCuenta(req.Email, req.Contrasena);

            if (!string.IsNullOrEmpty(cuentaId))
            {
                var token = _jwtService.GenerarToken(cuentaId, "CUENTA");

                return new
                {
                    tipoUsuario = "CUENTA",
                    id = cuentaId,
                    token = token
                };
            }

return null;
        }
    }
}
