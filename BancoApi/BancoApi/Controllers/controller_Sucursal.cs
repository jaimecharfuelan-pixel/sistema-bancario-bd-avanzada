using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.Models.models_sucursal;
using BancoApi.DTOs.DTOs_Sucursal;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Sucursal")]
    public class controller_Sucursal : ControllerBase
    {
        private readonly service_Sucursal service;

        public controller_Sucursal(service_Sucursal srv)
        {
            service = srv;
        }

        [HttpGet("service_listar")]
        public async Task<IActionResult> service_listar()
        {
            try
            {
                var dto = new DTO_SucursalListar();
                var model = new model_SucursalListar(dto);
                return Ok(await service.function_listarSucursales(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar sucursales" });
            }
        }

[HttpPost("service_crear")]
        public async Task<IActionResult> service_crear([FromBody] DTO_SucursalCrear? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_SucursalCrear(dto);

                int id = await service.function_crearSucursal(model);
                return Ok(new { message = "Sucursal creada exitosamente", idSucursal = id });
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

[HttpPut("service_editarEstado")]
        public async Task<IActionResult> service_editarEstado([FromBody] DTO_SucursalCambiarEstado? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_SucursalCambiarEstado(dto);

                await service.function_editarEstado(model);
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

[HttpDelete("service_eliminar/{idSucursal}")]
        public async Task<IActionResult> service_eliminar(int idSucursal)
        {
            try
            {
                var dto = new DTO_SucursalEliminar { idSucursal = idSucursal };
                var model = new model_SucursalEliminar(dto);
                await service.function_eliminarSucursal(model);
                return Ok(new { message = "Sucursal eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] List<int>? ids)
        {
            try
            {
                if (ids == null)
                {
                    return BadRequest(new { error = "La lista de IDs es requerida" });
                }

                var dto = new DTO_SucursalObtenerPorIds { ids = ids };
                var model = new model_SucursalObtenerPorIds(dto);
                var result = await service.function_obtenerSucursalesPorIds(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las sucursales", detalle = ex.Message });
            }
        }

[HttpGet("service_listarAbiertas")]
        public async Task<IActionResult> service_listarAbiertas()
        {
            try
            {
                var dto = new DTO_SucursalListarAbiertas();
                var model = new model_SucursalListarAbiertas(dto);
                var result = await service.function_listarSucursalesAbiertas(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar las sucursales abiertas", detalle = ex.Message });
            }
        }
    }
}
