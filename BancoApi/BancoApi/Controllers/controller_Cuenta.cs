using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.Models.models_cuenta;
using BancoApi.DTOs.DTOs_Cuenta;
using Oracle.ManagedDataAccess.Client;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Cuenta")]
    public class controller_Cuenta : ControllerBase
    {
        private readonly service_Cuenta att_serviceCuenta;

        public controller_Cuenta(service_Cuenta service)
        {
            att_serviceCuenta = service;
        }

        [HttpPost("service_consultarPorCliente")]
        public async Task<IActionResult> service_consultarPorCliente([FromBody] DTO_CuentaConsultarPorCliente? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaConsultarPorCliente(dto);

                var result = await att_serviceCuenta.function_consultarPorCliente(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Error al consultar cuenta por cliente",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("service_consultarPorId")]
        public async Task<IActionResult> service_consultarPorId([FromBody] DTO_CuentaConsultarPorId? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaConsultarPorId(dto);

                var result = await att_serviceCuenta.function_consultarPorId(model);
                return Ok(result ?? new { message = "Cuenta no encontrada" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al consultar cuenta por ID" });
            }
        }

        [HttpPut("service_actualizarCuenta")]
        public async Task<IActionResult> service_actualizarCuenta([FromBody] DTO_CuentaActualizar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaActualizar(dto);

                await att_serviceCuenta.function_actualizarCuenta(model);
                return Ok(new { message = "Cuenta actualizada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("service_cambiarSaldo")]
        public async Task<IActionResult> service_cambiarSaldo([FromBody] DTO_CuentaCambiarSaldo? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaCambiarSaldo(dto);

                await att_serviceCuenta.function_cambiarSaldo(model);
                return Ok(new { message = "Saldo actualizado correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("service_cambiarContrasena")]
        public async Task<IActionResult> service_cambiarContrasena([FromBody] DTO_CuentaCambiarContrasena? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaCambiarContrasenaCuenta(dto);

                await att_serviceCuenta.function_cambiarContrasena(model);
                return Ok(new { message = "Contraseña actualizada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("service_eliminarCuenta/{idCuenta}")]
        public async Task<IActionResult> service_eliminarCuenta(int idCuenta)
        {
            try
            {
                var dto = new DTO_CuentaEliminar { idCuenta = idCuenta };
                var model = new model_CuentaEliminar(dto);
                await att_serviceCuenta.function_eliminarCuenta(model);
                return Ok(new { message = "Cuenta eliminada (inactivada) correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

[HttpGet("service_listarActivas")]
        public async Task<IActionResult> service_listarActivas()
        {
            try
            {

                var model = new model_CuentaListarActivas(null);

                var lista = await att_serviceCuenta.function_listarActivas(model);
                return Ok(lista);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                var primeraLinea = mensajeError.Split('\n')[0];
                if (primeraLinea.Contains("ORA-"))
                {
                    var partes = primeraLinea.Split(':');
                    if (partes.Length > 1)
                    {
                        mensajeError = partes[1].Trim();
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
                return StatusCode(500, new { error = "Error al listar cuentas activas", detalle = ex.Message });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_CuentaObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuentaObtenerPorIds(dto);

                var lista = await att_serviceCuenta.function_obtenerPorIds(model);
                return Ok(lista);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                var primeraLinea = mensajeError.Split('\n')[0];
                if (primeraLinea.Contains("ORA-"))
                {
                    var partes = primeraLinea.Split(':');
                    if (partes.Length > 1)
                    {
                        mensajeError = partes[1].Trim();
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
                return StatusCode(500, new { error = "Error al obtener cuentas por IDs", detalle = ex.Message });
            }
        }
    }
}
