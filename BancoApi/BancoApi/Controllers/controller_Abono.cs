using BancoApi.Models.models_abono;
using BancoApi.Services;
using BancoApi.DTOs.DTO_Abono;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Abono")]
    public class controller_Abono : ControllerBase
    {
        private readonly service_Abono att_serviceAbono;

        public controller_Abono(service_Abono service)
        {
            att_serviceAbono = service;
        }

[HttpPost("service_registrarAbono")]
        public async Task<IActionResult> service_registrarAbono(
            [FromBody] DTO_AbonoRegistrar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_AbonoRegistrar(dto);

                var (idAbono, idTransaccion) = await att_serviceAbono.function_registrarAbono(model);

                return Ok(new
                {
                    message = "Abono registrado exitosamente",
                    idAbono,
                    idTransaccion
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios. Verifique que el préstamo y la cuenta existan.";
                }

                else if (oracleEx.Number == -20701)
                {
                    mensajeError = "El préstamo no existe.";
                }
                else if (oracleEx.Number == -20702)
                {
                    mensajeError = "El préstamo no está activo.";
                }
                else if (oracleEx.Number == -20703)
                {
                    mensajeError = "El monto del abono debe ser mayor a 0.";
                }
                else if (oracleEx.Number == -20704)
                {
                    mensajeError = "El abono no puede ser mayor al saldo del préstamo.";
                }
                else if (oracleEx.Number == -20705)
                {
                    mensajeError = "La cuenta no existe o no está activa.";
                }
                else if (oracleEx.Number == -20706)
                {
                    mensajeError = "Saldo insuficiente en la cuenta.";
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
                return StatusCode(500, new { error = "Error al registrar el abono", detalle = ex.Message });
            }
        }

[HttpPost("service_listarAbonos")]
        public async Task<IActionResult> service_listarAbonos([FromBody] DTO_AbonoListar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_AbonoListar(dto);

                var lista = await att_serviceAbono.function_listarAbonos(model);
                return Ok(lista);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró el préstamo especificado.";
                }

                else if (oracleEx.Number == -20720)
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
                return StatusCode(500, new { error = "Error al listar los abonos", detalle = ex.Message });
            }
        }
    }
}
