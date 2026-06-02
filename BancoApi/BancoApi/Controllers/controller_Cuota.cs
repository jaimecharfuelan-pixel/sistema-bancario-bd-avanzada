using BancoApi.DTOs.DTOs_Cuota;
using BancoApi.Models.models_cuota;
using BancoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Cuota")]
    public class controller_Cuota : ControllerBase
    {
        private readonly service_Cuota att_serviceCuota;

        public controller_Cuota(service_Cuota service)
        {
            att_serviceCuota = service;
        }

[HttpPost("service_generarCuotas")]
        public async Task<IActionResult> service_generarCuotas([FromBody] DTO_CuotaGenerar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuotaGenerar(dto);

                await att_serviceCuota.function_generarCuotas(model);
                return Ok(new { message = "Cuotas generadas exitosamente" });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró el préstamo especificado.";
                }

                else if (oracleEx.Number == -20601)
                {
                    mensajeError = "El préstamo no existe.";
                }
                else if (oracleEx.Number == -20602)
                {
                    mensajeError = "El préstamo debe estar activo para generar cuotas.";
                }
                else if (oracleEx.Number == -20603)
                {
                    mensajeError = "El plazo del préstamo no es válido.";
                }
                else if (oracleEx.Number == -20604)
                {
                    mensajeError = "Ya existen cuotas generadas para este préstamo.";
                }
                else if (oracleEx.Number == 1)
                {

                    if (mensajeError.Contains("UQ_CUOTA_PRESTAMO") || mensajeError.Contains("cuota"))
                    {
                        mensajeError = "Ya existen cuotas generadas para este préstamo. No se pueden generar cuotas duplicadas.";
                    }
                    else
                    {
                        mensajeError = "Error de restricción única: " + mensajeError;
                    }
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
                return StatusCode(500, new { error = "Error al generar las cuotas", detalle = ex.Message });
            }
        }

[HttpPost("service_pagarCuota")]
        public async Task<IActionResult> service_pagarCuota([FromBody] DTO_CuotaPagar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuotaPagar(dto);

                var idTransaccion = await att_serviceCuota.function_pagarCuota(model);
                return Ok(new
                {
                    message = "Cuota pagada exitosamente",
                    idTransaccion = idTransaccion
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios. Verifique que la cuota y la cuenta existan.";
                }

                else if (oracleEx.Number == -20612)
                {
                    mensajeError = "La cuota ya está pagada.";
                }
                else if (oracleEx.Number == -20613)
                {
                    mensajeError = "La cuenta no existe o no está activa.";
                }
                else if (oracleEx.Number == -20614)
                {
                    mensajeError = "Saldo insuficiente para pagar la cuota.";
                }
                else if (oracleEx.Number == -20615)
                {
                    mensajeError = "La cuota no existe.";
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
                return StatusCode(500, new { error = "Error al pagar la cuota", detalle = ex.Message });
            }
        }

[HttpPost("service_listarCuotas")]
        public async Task<IActionResult> service_listarCuotas([FromBody] DTO_CuotaListar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CuotaListar(dto);

                var lista = await att_serviceCuota.function_listarCuotas(model);
                return Ok(lista);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró el préstamo especificado.";
                }

                else if (oracleEx.Number == -20620)
                {
                    mensajeError = "El préstamo no existe.";
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
                return StatusCode(500, new { error = "Error al listar las cuotas", detalle = ex.Message });
            }
        }
    }
}

