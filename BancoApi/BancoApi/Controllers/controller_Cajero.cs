using BancoApi.Models.models_cajero;
using BancoApi.Services;
using BancoApi.DTOs.DTOs_Cajero;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Cajero")]
    public class controller_Cajero : ControllerBase
    {
        private readonly service_Cajero servicio;

        public controller_Cajero(service_Cajero svc)
        {
            servicio = svc;
        }

        [HttpPost("service_crear")]
        public async Task<IActionResult> service_crear([FromBody] DTO_CajeroCrear? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CajeroCrear(dto);

                var result = await servicio.function_crearCajero(model);
                return Ok(new { message = "Cajero creado exitosamente", idCajero = result });
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

        [HttpPut("service_cambiarEstado")]
        public async Task<IActionResult> service_cambiarEstado([FromBody] DTO_CajeroCambiarEstado? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CajeroCambiarEstado(dto);

                await servicio.function_cambiarEstado(model);
                return Ok(new { message = "Estado actualizado correctamente" });
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

        [HttpPut("service_recargar")]
        public async Task<IActionResult> service_recargar([FromBody] DTO_CajeroRecargar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CajeroRecargar(dto);

                await servicio.function_recargar(model);
                return Ok(new { message = "Cajero recargado exitosamente" });
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

        [HttpPut("service_descontar")]
        public async Task<IActionResult> service_descontar([FromBody] DTO_CajeroDescontar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CajeroDescontar(dto);

                await servicio.function_descontar(model);
                return Ok(new { message = "Dinero descontado exitosamente" });
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

        [HttpGet("service_listar/{idSucursal}")]
        public async Task<IActionResult> service_listar(int idSucursal)
        {
            try
            {
                var dto = new DTO_CajeroListar { idSucursal = idSucursal };
                var model = new model_CajeroListar(dto);
                return Ok(await servicio.function_listar(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar cajeros" });
            }
        }

[HttpGet("service_listarActivos")]
        public async Task<IActionResult> service_listarActivos()
        {
            try
            {

                var model = new model_CajeroListarActivos(null);

                var lista = await servicio.function_listarActivos(model);
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
                return StatusCode(500, new { error = "Error al listar cajeros activos", detalle = ex.Message });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_CajeroObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_CajeroObtenerPorIds(dto);

                var lista = await servicio.function_obtenerPorIds(model);
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
                return StatusCode(500, new { error = "Error al obtener cajeros por IDs", detalle = ex.Message });
            }
        }
    }
}
