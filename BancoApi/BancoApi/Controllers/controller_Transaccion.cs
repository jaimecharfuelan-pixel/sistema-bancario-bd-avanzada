using Microsoft.AspNetCore.Mvc;
using BancoApi.Services;
using BancoApi.Models.models_transaccion;
using BancoApi.DTOs.DTOs_Transaccion;

namespace BancoApi.Controllers
{
    [ApiController]
    [Route("api/controller_Transaccion")]
    public class controller_Transaccion : ControllerBase
    {
        private readonly service_Transaccion service;

        public controller_Transaccion(service_Transaccion srv)
        {
            service = srv;
        }

[HttpPost("service_transferencia")]
        public async Task<IActionResult> service_transferencia([FromBody] DTO_TransaccionTransferencia? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionTransferencia(dto);

                int idTransaccion = await service.function_transferencia(model);
                return Ok(new { message = "Transferencia realizada exitosamente", idTransaccion = idTransaccion });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron las cuentas especificadas. Verifique que ambas cuentas existan y estén activas.";
                }

                else if (oracleEx.Number == -20001)
                {
                    mensajeError = "El monto debe ser mayor que cero.";
                }
                else if (oracleEx.Number == -20002)
                {
                    mensajeError = "La cuenta origen y destino no pueden ser la misma.";
                }
                else if (oracleEx.Number == -20005)
                {
                    mensajeError = "La cuenta origen no está activa.";
                }
                else if (oracleEx.Number == -20006)
                {
                    mensajeError = "La cuenta destino no está activa.";
                }
                else if (oracleEx.Number == -20007)
                {
                    mensajeError = "Saldo insuficiente en la cuenta origen.";
                }
                else
                {

                    mensajeError = oracleEx.Message;

                    if (mensajeError.Contains("ORA-20001") || mensajeError.Contains("-20001"))
                    {
                        mensajeError = "El monto debe ser mayor que cero.";
                    }
                    else if (mensajeError.Contains("ORA-20002") || mensajeError.Contains("-20002"))
                    {
                        mensajeError = "La cuenta origen y destino no pueden ser la misma.";
                    }
                    else if (mensajeError.Contains("ORA-20005") || mensajeError.Contains("-20005"))
                    {
                        mensajeError = "La cuenta origen no está activa.";
                    }
                    else if (mensajeError.Contains("ORA-20006") || mensajeError.Contains("-20006"))
                    {
                        mensajeError = "La cuenta destino no está activa.";
                    }
                    else if (mensajeError.Contains("ORA-20007") || mensajeError.Contains("-20007"))
                    {
                        mensajeError = "Saldo insuficiente en la cuenta origen.";
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
                }
                
                return BadRequest(new { error = mensajeError });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al realizar la transferencia", detalle = ex.Message });
            }
        }

[HttpPost("service_retiroDebito")]
        public async Task<IActionResult> service_retiroDebito([FromBody] DTO_TransaccionRetiroDebito? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionRetiroDebito(dto);

                int idTransaccion = await service.function_retiroDebito(model);
                return Ok(new { message = "Retiro realizado exitosamente", idTransaccion = idTransaccion });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios. Verifique que la tarjeta de débito, el cajero y la cuenta asociada existan.";
                }

                else if (oracleEx.Number == -20101)
                {
                    mensajeError = "El monto debe ser mayor que cero.";
                }
                else if (oracleEx.Number == -20103)
                {
                    mensajeError = "La tarjeta no está activa.";
                }
                else if (oracleEx.Number == -20104)
                {
                    mensajeError = "Excede el límite de retiro de la tarjeta.";
                }
                else if (oracleEx.Number == -20105)
                {
                    mensajeError = "Saldo insuficiente en la cuenta.";
                }
                else if (oracleEx.Number == -20106)
                {
                    mensajeError = "Cajero fuera de servicio.";
                }
                else if (oracleEx.Number == -20107)
                {
                    mensajeError = "Cajero sin suficiente dinero disponible.";
                }
                else
                {

                    mensajeError = oracleEx.Message;

                    if (mensajeError.Contains("ORA-20103") || mensajeError.Contains("-20103"))
                    {
                        mensajeError = "La tarjeta no está activa.";
                    }
                    else if (mensajeError.Contains("ORA-20101") || mensajeError.Contains("-20101"))
                    {
                        mensajeError = "El monto debe ser mayor que cero.";
                    }
                    else if (mensajeError.Contains("ORA-20104") || mensajeError.Contains("-20104"))
                    {
                        mensajeError = "Excede el límite de retiro de la tarjeta.";
                    }
                    else if (mensajeError.Contains("ORA-20105") || mensajeError.Contains("-20105"))
                    {
                        mensajeError = "Saldo insuficiente en la cuenta.";
                    }
                    else if (mensajeError.Contains("ORA-20106") || mensajeError.Contains("-20106"))
                    {
                        mensajeError = "Cajero fuera de servicio.";
                    }
                    else if (mensajeError.Contains("ORA-20107") || mensajeError.Contains("-20107"))
                    {
                        mensajeError = "Cajero sin suficiente dinero disponible.";
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
                }
                
                return BadRequest(new { error = mensajeError });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al realizar el retiro", detalle = ex.Message });
            }
        }

[HttpPost("service_retiroCredito")]
        public async Task<IActionResult> service_retiroCredito([FromBody] DTO_TransaccionRetiroCredito? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionRetiroCredito(dto);

                int idTransaccion = await service.function_retiroCredito(model);
                return Ok(new { message = "Retiro realizado exitosamente", idTransaccion = idTransaccion });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException oracleEx)
            {

                string mensajeError;

if (oracleEx.Number == 1403)
                {
                    mensajeError = "No se encontraron los datos necesarios. Verifique que la tarjeta de crédito, el cajero y la cuenta asociada existan.";
                }

                else if (oracleEx.Number == -20201)
                {
                    mensajeError = "El monto debe ser mayor que cero.";
                }
                else if (oracleEx.Number == -20203)
                {
                    mensajeError = "La tarjeta no está activa.";
                }
                else if (oracleEx.Number == -20204)
                {
                    mensajeError = "Excede el límite de crédito de la tarjeta.";
                }
                else if (oracleEx.Number == -20206)
                {
                    mensajeError = "Cajero fuera de servicio.";
                }
                else if (oracleEx.Number == -20207)
                {
                    mensajeError = "Cajero sin suficiente dinero disponible.";
                }
                else
                {

                    mensajeError = oracleEx.Message;

                    if (mensajeError.Contains("ORA-20203"))
                    {
                        mensajeError = "La tarjeta no está activa.";
                    }
                    else if (mensajeError.Contains("ORA-20201"))
                    {
                        mensajeError = "El monto debe ser mayor que cero.";
                    }
                    else if (mensajeError.Contains("ORA-20204"))
                    {
                        mensajeError = "Excede el límite de crédito de la tarjeta.";
                    }
                    else if (mensajeError.Contains("ORA-20206"))
                    {
                        mensajeError = "Cajero fuera de servicio.";
                    }
                    else if (mensajeError.Contains("ORA-20207"))
                    {
                        mensajeError = "Cajero sin suficiente dinero disponible.";
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
                }
                
                return BadRequest(new { error = mensajeError });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al realizar el retiro", detalle = ex.Message });
            }
        }

[HttpPost("service_historialCuenta")]
        public async Task<IActionResult> service_historialCuenta([FromBody] DTO_TransaccionHistorialCuenta? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionHistorialCuenta(dto);

                var result = await service.function_historialCuenta(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el historial", detalle = ex.Message });
            }
        }

[HttpPost("service_historialCajero")]
        public async Task<IActionResult> service_historialCajero([FromBody] DTO_TransaccionHistorialCajero? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionHistorialCajero(dto);

                var result = await service.function_historialCajero(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el historial", detalle = ex.Message });
            }
        }

[HttpPost("service_obtenerPorIds")]
        public async Task<IActionResult> service_obtenerPorIds([FromBody] DTO_TransaccionObtenerPorIds? dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { error = "Los datos son requeridos" });
                }

var model = new model_TransaccionObtenerPorIds(dto);

                var result = await service.function_obtenerTransaccionesPorIds(model);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las transacciones", detalle = ex.Message });
            }
        }

[HttpGet("service_listarTransaccionesHoy")]
        public async Task<IActionResult> service_listarTransaccionesHoy()
        {
            try
            {
                var dto = new DTO_TransaccionListarHoy();
                var model = new model_TransaccionListarHoy(dto);
                var result = await service.function_listarTransaccionesHoy(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al listar las transacciones", detalle = ex.Message });
            }
        }
    }
}

