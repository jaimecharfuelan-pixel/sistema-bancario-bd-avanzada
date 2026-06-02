using BancoApi.Models.models_cliente;
using BancoApi.Services;
using BancoApi.DTOs.DTOs_Cliente;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Linq;

namespace BancoApi.Controllers
{

    [ApiController]

[Route("api/controller_Cliente")]
    public class controller_Cliente : ControllerBase
    {
        private readonly service_Cliente att_serviceCliente;

public controller_Cliente(service_Cliente prm_service)
        {
            att_serviceCliente = prm_service;
        }

[HttpPost("service_solicitarCuenta")]
        public async Task<IActionResult> service_solicitarCuenta([FromBody] DTO_ClienteCrear? dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(new { 
                        error = "Error de validación",
                        detalles = errors
                    });
                }

if (dto == null)
                {
                    return BadRequest(new { error = "Los datos del cliente son requeridos" });
                }

var model = new model_ClienteCrear(dto);

var att_clienteId = await att_serviceCliente.function_crearCliente(model);

                return Ok(new
                {
                    message = "Cliente creado exitosamente",
                    id_cliente = att_clienteId
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError = oracleEx.Message;

if (oracleEx.Number == -20004)
                {
                    mensajeError = "El email ya está registrado. Por favor, use otro email.";
                }
                else if (oracleEx.Number == -20005)
                {
                    mensajeError = "La cédula ya está registrada. Por favor, verifique los datos.";
                }
                else if (oracleEx.Number == -20002)
                {
                    mensajeError = "El formato del email no es válido.";
                }
                else if (oracleEx.Number == 1)
                {

                    if (mensajeError.Contains("PK_CLIENTE"))
                    {
                        mensajeError = "Ya existe un cliente con estos datos. Por favor, verifique el email o la cédula.";
                    }
                    else if (mensajeError.Contains("EMAIL") || mensajeError.Contains("email"))
                    {
                        mensajeError = "El email ya está registrado. Por favor, use otro email.";
                    }
                    else if (mensajeError.Contains("CEDULA") || mensajeError.Contains("cedula"))
                    {
                        mensajeError = "La cédula ya está registrada. Por favor, verifique los datos.";
                    }
                    else
                    {
                        mensajeError = "Los datos proporcionados ya existen en el sistema. Por favor, verifique el email o la cédula.";
                    }
                }
                
                return BadRequest(new { error = mensajeError });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

[HttpPut("service_actualizarDatosCliente")]
        public async Task<IActionResult> service_actualizarDatosCliente(
            [FromBody] DTO_ClienteActualizarDatos dto)
        {
            try
            {
                var model = new model_ClienteActualizarDatos(dto);
                await att_serviceCliente.function_actualizarDatos(model);
                return Ok(new { message = "Datos del cliente actualizados correctamente" });
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

[HttpPut("service_actualizarCorreo")]
        public async Task<IActionResult> service_actualizarCorreo(
            [FromBody] DTO_ClienteActualizarCorreo dto)
        {
            try
            {
                var model = new model_ClienteActualizarCorreo(dto);
                await att_serviceCliente.function_actualizarCorreo(model);
                return Ok(new { message = "Correo actualizado correctamente" });
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

[HttpPut("service_actualizarNombre")]
        public async Task<IActionResult> service_actualizarNombre(
            [FromBody] DTO_ClienteActualizarNombre dto)
        {
            try
            {
                var model = new model_ClienteActualizarNombre(dto);
                await att_serviceCliente.function_actualizarNombre(model);
                return Ok(new { message = "Nombre actualizado correctamente" });
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

[HttpPut("service_actualizarTelefono")]
        public async Task<IActionResult> service_actualizarTelefono(
            [FromBody] DTO_ClienteActualizarTelefono dto)
        {
            try
            {
                var model = new model_ClienteActualizarTelefono(dto);
                await att_serviceCliente.function_actualizarTelefono(model);
                return Ok(new { message = "Teléfono actualizado correctamente" });
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

[HttpPut("service_actualizarCedula")]
        public async Task<IActionResult> service_actualizarCedula(
            [FromBody] DTO_ClienteActualizarCedula dto)
        {
            try
            {
                var model = new model_ClienteActualizarCedula(dto);
                await att_serviceCliente.function_actualizarCedula(model);
                return Ok(new { message = "Cédula actualizada correctamente" });
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

[HttpDelete("service_eliminarCliente")]
        public async Task<IActionResult> service_eliminarCliente([FromBody] DTO_ClienteEliminar? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_ClienteEliminar(dto);
                await att_serviceCliente.function_eliminarCliente(model);
                return Ok(new { message = "Cliente eliminado (inactivado) exitosamente" });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;
                
                if (oracleEx.Number == -20003)
                {
                    mensajeError = "El cliente no existe.";
                }
                else if (oracleEx.Number == -20009)
                {
                    mensajeError = "El cliente tiene cuentas asociadas. No se puede eliminar.";
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
                return StatusCode(500, new { error = "Error al eliminar el cliente", detalle = ex.Message });
            }
        }

[HttpGet("service_listarActivos")]
        public async Task<IActionResult> service_listarActivos()
        {
            try
            {
                var model = new model_ClienteListarActivos(null);
                var lista = await att_serviceCliente.function_listarActivos(model);
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

                return StatusCode(500, new { error = $"Error de Oracle: {mensajeError}", detalle = oracleEx.Message, numero = oracleEx.Number });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = "Error al listar clientes activos", detalle = ex.Message, stackTrace = ex.StackTrace });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_ClienteObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_ClienteObtenerPorIds(dto);
                var lista = await att_serviceCliente.function_obtenerPorIds(model);
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
                return StatusCode(500, new { error = "Error al obtener clientes por IDs", detalle = ex.Message });
            }
        }

[HttpPost("service_procesarMasivos")]
        public async Task<IActionResult> service_procesarMasivos([FromBody] DTO_ClienteProcesarMasivos? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

                var model = new model_ClienteProcesarMasivos(dto);
                var exitosos = await att_serviceCliente.function_procesarMasivos(model);
                return Ok(new { 
                    message = "Procesamiento masivo completado",
                    clientesProcesados = exitosos,
                    totalEnviados = dto.clientes?.Count ?? 0
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {
                string mensajeError = oracleEx.Message;

if (oracleEx.Number == 1)
                {
                    if (mensajeError.Contains("PK_CLIENTE") || mensajeError.Contains("pk_cliente"))
                    {
                        mensajeError = "Uno o más IDs de cliente ya existen en la base de datos. Verifique que los IDs sean únicos antes de intentar insertarlos.";
                    }
                    else if (mensajeError.Contains("EMAIL") || mensajeError.Contains("email"))
                    {
                        mensajeError = "Uno o más emails ya están registrados. Verifique que los emails sean únicos.";
                    }
                    else if (mensajeError.Contains("CEDULA") || mensajeError.Contains("cedula"))
                    {
                        mensajeError = "Uno o más números de cédula ya están registrados. Verifique que las cédulas sean únicas.";
                    }
                    else
                    {
                        mensajeError = "Error de restricción única: uno o más registros ya existen en la base de datos. Verifique los datos antes de intentar insertarlos.";
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
                return StatusCode(500, new { error = "Error al procesar clientes masivos", detalle = ex.Message });
            }
        }
    }
}
