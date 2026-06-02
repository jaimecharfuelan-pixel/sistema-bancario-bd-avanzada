using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.Models.models_autenticacion;
using BancoApi.DTOs.DTOs_Autenticacion;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class controller_autenticacion : ControllerBase
    {
        private readonly service_Autenticacion att_serviceAutenticacion;

        public controller_autenticacion(service_Autenticacion service)
        {
            att_serviceAutenticacion = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTO_AutenticacionLogin? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_LoginRequest(dto);

                var result = await att_serviceAutenticacion.function_login(model);

                if (result == null)
                {
                    return Unauthorized(new { error = "Credenciales incorrectas" });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}
