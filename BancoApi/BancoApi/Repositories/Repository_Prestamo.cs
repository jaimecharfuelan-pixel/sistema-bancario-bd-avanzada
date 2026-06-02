using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BancoApi.Models.models_prestamo;

namespace BancoApi.Repositories
{

    public class Repository_Prestamo
    {
        private readonly string _connectionString;

        public Repository_Prestamo(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<int> SolicitarPrestamo(
            int idCuenta,
            int idSucursal,
            decimal monto,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_solicitar_prestamo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;
            cmd.Parameters.Add("p_fecha_inicio", OracleDbType.Date).Value = fechaInicio;
            cmd.Parameters.Add("p_fecha_fin", OracleDbType.Date).Value = fechaFin;
            cmd.Parameters.Add("o_id_prestamo", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idPrestamo = cmd.Parameters["o_id_prestamo"].Value;
            if (idPrestamo != null && idPrestamo != DBNull.Value)
            {

                if (idPrestamo is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idPrestamo.ToString());
            }

            throw new Exception("No se pudo obtener el ID del préstamo");
        }

public async Task<List<model_PrestamoSolicitudItem>> ListarSolicitudes(int idSucursal)
        {
            var lista = new List<model_PrestamoSolicitudItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_listar_solicitudes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;
            cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var refCursor = (OracleRefCursor)cmd.Parameters["o_cursor"].Value;
            if (refCursor == null)
            {
                return lista;
            }

            using var reader = refCursor.GetDataReader();

            while (reader.Read())
            {
                lista.Add(new model_PrestamoSolicitudItem
                {
                    idPrestamo = Convert.ToInt32(reader["ID_PRESTAMO"]),
                    idCuenta = Convert.ToInt32(reader["ID_CUENTA"]),
                    montoPrestamo = Convert.ToDecimal(reader["MONTO_PRESTAMO"]),
                    fechaInicioPrestamo = reader["FECHA_INICIO_PRESTAMO"] == DBNull.Value
                        ? null
                        : reader["FECHA_INICIO_PRESTAMO"].ToString(),
                    fechaVencimientoPrestamo = reader["FECHA_VENCIMIENTO_PRESTAMO"] == DBNull.Value
                        ? null
                        : reader["FECHA_VENCIMIENTO_PRESTAMO"].ToString(),
                    estadoPrestamo = reader["ESTADO_PRESTAMO"].ToString() ?? ""
                });
            }

            return lista;
        }

public async Task AceptarPrestamo(int idPrestamo)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_aceptar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_prestamo", OracleDbType.Int32).Value = idPrestamo;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task RechazarPrestamo(int idPrestamo)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_rechazar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_prestamo", OracleDbType.Int32).Value = idPrestamo;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_PrestamoItem>> ListarActivos()
        {
            var lista = new List<model_PrestamoItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_listar_prestamos_activos", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_PRESTAMOS.T_LISTA_PRESTAMOS"
            };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();

var arrayValue = outParam.Value;
            
            if (arrayValue != null)
            {
                try
                {
                    var arrayType = arrayValue.GetType();
                    var lengthProperty = arrayType.GetProperty("Length");
                    var getValueMethod = arrayType.GetMethod("GetValue", new[] { typeof(int) });
                    
                    if (lengthProperty != null && getValueMethod != null)
                    {
                        var length = (int)lengthProperty.GetValue(arrayValue);
                        
                        for (int i = 0; i < length; i++)
                        {
                            try
                            {
                                var structValue = getValueMethod.Invoke(arrayValue, new object[] { i });
                                
                                if (structValue != null)
                                {
                                    var structType = structValue.GetType();
                                    var getStructValueMethod = structType.GetMethod("GetValue", new[] { typeof(int) });
                                    
                                    if (getStructValueMethod != null)
                                    {

int idPrestamo = 0;
                                        int idCuenta = 0;
                                        decimal montoPrestamo = 0;
                                        decimal saldoPrestamo = 0;
                                        string estadoPrestamo = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idPrestamo = (int)oracleDec0.Value;
                                                else
                                                    idPrestamo = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    idCuenta = (int)oracleDec1.Value;
                                                else
                                                    idCuenta = Convert.ToInt32(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    montoPrestamo = oracleDec2.Value;
                                                else
                                                    montoPrestamo = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo monto_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                            {
                                                if (value3 is OracleDecimal oracleDec3)
                                                    saldoPrestamo = oracleDec3.Value;
                                                else
                                                    saldoPrestamo = Convert.ToDecimal(value3);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo saldo_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                estadoPrestamo = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_prestamo: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_PrestamoItem
                                        {
                                            idPrestamo = idPrestamo,
                                            idCuenta = idCuenta,
                                            montoPrestamo = montoPrestamo,
                                            saldoPrestamo = saldoPrestamo,
                                            estadoPrestamo = estadoPrestamo
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error procesando elemento {i}: {ex.Message}");
                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error procesando array: {ex.Message}");
                }
            }

            return lista;
        }

public async Task<List<model_PrestamoItem>> ObtenerPorIds(List<int> ids)
        {
            var lista = new List<model_PrestamoItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_prestamos.procedure_obtener_prestamos_por_ids", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var idsArray = new OracleDecimal[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                idsArray[i] = new OracleDecimal(ids[i]);
            }

            var inParam = new OracleParameter("p_ids", OracleDbType.Array)
            {
                Direction = ParameterDirection.Input,
                UdtTypeName = "PKGC_PRESTAMOS.T_LISTA_IDS_PRESTAMO",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_PRESTAMOS.T_LISTA_PRESTAMOS"
            };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();

var arrayValue = outParam.Value;
            
            if (arrayValue != null)
            {
                try
                {
                    var arrayType = arrayValue.GetType();
                    var lengthProperty = arrayType.GetProperty("Length");
                    var getValueMethod = arrayType.GetMethod("GetValue", new[] { typeof(int) });
                    
                    if (lengthProperty != null && getValueMethod != null)
                    {
                        var length = (int)lengthProperty.GetValue(arrayValue);
                        
                        for (int i = 0; i < length; i++)
                        {
                            try
                            {
                                var structValue = getValueMethod.Invoke(arrayValue, new object[] { i });
                                
                                if (structValue != null)
                                {
                                    var structType = structValue.GetType();
                                    var getStructValueMethod = structType.GetMethod("GetValue", new[] { typeof(int) });
                                    
                                    if (getStructValueMethod != null)
                                    {

int idPrestamo = 0;
                                        int idCuenta = 0;
                                        decimal montoPrestamo = 0;
                                        decimal saldoPrestamo = 0;
                                        string estadoPrestamo = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idPrestamo = (int)oracleDec0.Value;
                                                else
                                                    idPrestamo = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    idCuenta = (int)oracleDec1.Value;
                                                else
                                                    idCuenta = Convert.ToInt32(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    montoPrestamo = oracleDec2.Value;
                                                else
                                                    montoPrestamo = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo monto_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                            {
                                                if (value3 is OracleDecimal oracleDec3)
                                                    saldoPrestamo = oracleDec3.Value;
                                                else
                                                    saldoPrestamo = Convert.ToDecimal(value3);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo saldo_prestamo: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                estadoPrestamo = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_prestamo: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_PrestamoItem
                                        {
                                            idPrestamo = idPrestamo,
                                            idCuenta = idCuenta,
                                            montoPrestamo = montoPrestamo,
                                            saldoPrestamo = saldoPrestamo,
                                            estadoPrestamo = estadoPrestamo
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error procesando elemento {i}: {ex.Message}");
                                continue;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error procesando array: {ex.Message}");
                }
            }

            return lista;
        }
    }
}

