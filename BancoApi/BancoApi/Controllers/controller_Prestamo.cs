using BancoApi.Models.models_prestamo;
using BancoApi.Services;
using BancoApi.DTOs.DTOs_Prestamo;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Prestamo")]
    public class controller_Prestamo : ControllerBase
    {
        private readonly service_Prestamo att_servicePrestamo;

        public controller_Prestamo(service_Prestamo service)
        {
            att_servicePrestamo = service;
        }

        [HttpPost("service_solicitarPrestamo")]
        public async Task<IActionResult> service_solicitarPrestamo([FromBody] DTO_PrestamoSolicitar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_PrestamoSolicitar(dto);

                var idPrestamo = await att_servicePrestamo.function_solicitarPrestamo(model);
                return Ok(new
                {
                    message = "Préstamo solicitado exitosamente",
                    idPrestamo = idPrestamo
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios. Verifique que la cuenta y la sucursal existan.";
                }
                else if (oracleEx.Number == -20001)
                {
                    mensajeError = "El monto solicitado debe ser mayor que 0.";
                }
                else if (oracleEx.Number == -20002)
                {
                    mensajeError = "La fecha fin debe ser mayor que la fecha inicio.";
                }
                else if (oracleEx.Number == -20003)
                {
                    mensajeError = "La cuenta no existe o no está activa.";
                }
                else if (oracleEx.Number == -20004)
                {
                    mensajeError = "La sucursal no existe.";
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
                return StatusCode(500, new { error = "Error al solicitar el préstamo", detalle = ex.Message });
            }
        }

        [HttpPost("service_listarSolicitudes")]
        public async Task<IActionResult> service_listarSolicitudes([FromBody] DTO_PrestamoListarSolicitudes? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_PrestamoListarSolicitudes(dto);

                var lista = await att_servicePrestamo.function_listarSolicitudes(model);
                return Ok(lista);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró la sucursal especificada.";
                }
                else if (oracleEx.Number == -20010)
                {
                    mensajeError = "La sucursal no existe.";
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
                return StatusCode(500, new { error = "Error al listar las solicitudes", detalle = ex.Message });
            }
        }

        [HttpPost("service_aceptarPrestamo")]
        public async Task<IActionResult> service_aceptarPrestamo([FromBody] DTO_PrestamoAceptar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_PrestamoAceptar(dto);

                await att_servicePrestamo.function_aceptarPrestamo(model);
                return Ok(new { message = "Préstamo aceptado exitosamente" });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró el préstamo especificado.";
                }
                else if (oracleEx.Number == -20020)
                {
                    mensajeError = "El préstamo no existe.";
                }
                else if (oracleEx.Number == -20021)
                {
                    mensajeError = "El préstamo no está en estado solicitado.";
                }
                else if (oracleEx.Number == -20022)
                {
                    mensajeError = "El plazo calculado del préstamo no es válido.";
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
                return StatusCode(500, new { error = "Error al aceptar el préstamo", detalle = ex.Message });
            }
        }

        [HttpPost("service_rechazarPrestamo")]
        public async Task<IActionResult> service_rechazarPrestamo([FromBody] DTO_PrestamoRechazar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_PrestamoRechazar(dto);

                await att_servicePrestamo.function_rechazarPrestamo(model);
                return Ok(new { message = "Préstamo rechazado exitosamente" });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontró el préstamo especificado.";
                }
                else if (oracleEx.Number == -20030)
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
                return StatusCode(500, new { error = "Error al rechazar el préstamo", detalle = ex.Message });
            }
        }

        [HttpGet("service_listarActivos")]
        public async Task<IActionResult> service_listarActivos()
        {
            try
            {
                var model = new model_PrestamoListarActivos(null);

                var lista = await att_servicePrestamo.function_listarActivos(model);
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
                return StatusCode(500, new { error = "Error al listar préstamos activos", detalle = ex.Message });
            }
        }

        [HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_PrestamoObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_PrestamoObtenerPorIds(dto);

                var lista = await att_servicePrestamo.function_obtenerPorIds(model);
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
                return StatusCode(500, new { error = "Error al obtener préstamos por IDs", detalle = ex.Message });
            }
        }
    }
}

