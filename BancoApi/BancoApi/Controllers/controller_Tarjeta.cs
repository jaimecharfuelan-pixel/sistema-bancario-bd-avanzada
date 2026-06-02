using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.Models.models_tarjeta;
using BancoApi.DTOs.DTOs_Tarjeta;
using Oracle.ManagedDataAccess.Client;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Tarjeta")]
    public class controller_Tarjeta : ControllerBase
    {
        private readonly service_Tarjeta att_serviceTarjeta;

        public controller_Tarjeta(service_Tarjeta service)
        {
            att_serviceTarjeta = service;
        }

[HttpPost("service_crearTarjetaDebito")]
        public async Task<IActionResult> service_crearTarjetaDebito([FromBody] DTO_TarjetaCrearDebito? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TarjetaCrearDebito(dto);

var idTarjeta = await att_serviceTarjeta.function_crearTarjetaDebito(model);

                return Ok(new
                {
                    message = "Tarjeta de débito creada exitosamente",
                    idTarjeta = idTarjeta
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == -20101)
                {
                    mensajeError = "La cuenta no existe o no está activa.";
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
                return StatusCode(500, new { error = "Error al crear la tarjeta de débito", detalle = ex.Message });
            }
        }

[HttpPost("service_crearTarjetaCredito")]
        public async Task<IActionResult> service_crearTarjetaCredito([FromBody] DTO_TarjetaCrearCredito? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TarjetaCrearCredito(dto);

var idTarjeta = await att_serviceTarjeta.function_crearTarjetaCredito(model);

                return Ok(new
                {
                    message = "Tarjeta de crédito creada exitosamente",
                    idTarjeta = idTarjeta
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == -20111)
                {
                    mensajeError = "La cuenta no existe o no está activa.";
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
                return StatusCode(500, new { error = "Error al crear la tarjeta de crédito", detalle = ex.Message });
            }
        }

[HttpPut("service_cambiarEstadoTarjeta")]
        public async Task<IActionResult> service_cambiarEstadoTarjeta([FromBody] DTO_TarjetaCambiarEstado? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TarjetaCambiarEstado(dto);

await att_serviceTarjeta.function_cambiarEstadoTarjeta(model);

                return Ok(new { message = "Estado de tarjeta actualizado exitosamente" });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == -20121)
                {
                    mensajeError = "Estado inválido.";
                }
                else if (oracleEx.Number == -20122)
                {
                    mensajeError = "La tarjeta no existe.";
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
                return StatusCode(500, new { error = "Error al cambiar el estado de la tarjeta", detalle = ex.Message });
            }
        }

[HttpPost("service_listarTarjetas")]
        public async Task<IActionResult> service_listarTarjetas([FromBody] DTO_TarjetaListarTarjetas? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TarjetaListarTarjetas(dto);

var tarjetas = await att_serviceTarjeta.function_listarTarjetas(model);

                return Ok(tarjetas);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró la cuenta especificada.";
                }
                else if (oracleEx.Number == -20130)
                {
                    mensajeError = "La cuenta no existe.";
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
                return StatusCode(500, new { error = "Error al consultar tarjetas por cuenta", detalle = ex.Message });
            }
        }

[HttpGet("service_listarActivas")]
        public async Task<IActionResult> service_listarActivas()
        {
            try
            {

                var model = new model_TarjetaListarActivas(null);

                var lista = await att_serviceTarjeta.function_listarActivas(model);
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
                return StatusCode(500, new { error = "Error al listar tarjetas activas", detalle = ex.Message });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_TarjetaObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TarjetaObtenerPorIds(dto);

                var lista = await att_serviceTarjeta.function_obtenerPorIds(model);
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
                return StatusCode(500, new { error = "Error al obtener tarjetas por IDs", detalle = ex.Message });
            }
        }
    }
}

