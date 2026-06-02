using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.DTOs.DTOs_Admin;
using BancoApi.Models.models_admin;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Admin")]
    public class controller_admin : ControllerBase
    {
        private readonly service_Admin att_serviceAdmin;

        public controller_admin(service_Admin service)
        {
            att_serviceAdmin = service;
        }

        [HttpGet("service_solicitudes")]
        public async Task<IActionResult> service_solicitudes()
        {
            try
            {
                var solicitudes = await att_serviceAdmin.function_obtenerSolicitudes();
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener solicitudes" });
            }
        }

        [HttpPost("service_crearCuenta")]
        public async Task<IActionResult> service_crearCuenta([FromBody] DTO_AdminCrearCuenta? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_AdminCrearCuenta(dto);

                var idCuenta = await att_serviceAdmin.function_crearCuenta(model);
                return Ok(new
                {
                    message = "Cuenta creada exitosamente",
                    idCuenta = idCuenta
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;

                if (oracleEx.Number == -20001)
                {
                    mensajeError = "El cliente no existe. Verifique que el ID del cliente sea válido.";
                }
                else if (oracleEx.Number == -20002)
                {
                    mensajeError = "El administrador no existe. Verifique que el ID del administrador sea válido.";
                }
                else if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios para crear la cuenta.";
                }
                else
                {
                    var primeraLinea = mensajeError.Split('\n')[0];
                    if (primeraLinea.Contains("ORA-"))
                    {
                        var partes = primeraLinea.Split(':');
                        if (partes.Length > 1)
                        {
                            mensajeError = partes[1].Trim();
                        }
                    }
                }

                return BadRequest(new { error = mensajeError });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear la cuenta", detalle = ex.Message });
            }
        }
    }
}
