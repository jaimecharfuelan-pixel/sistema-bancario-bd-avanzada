using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BancoApi.Models.models_transaccion;

namespace BancoApi.Repositories
{

    public class Repository_Transaccion
    {
        private readonly string _connectionString;

        public Repository_Transaccion(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<int> Transferencia(int cuentaOrigen, int cuentaDestino, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_transferencia", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_cuenta_origen", OracleDbType.Int32).Value = cuentaOrigen;
            cmd.Parameters.Add("p_cuenta_destino", OracleDbType.Int32).Value = cuentaDestino;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;
            cmd.Parameters.Add("o_id_transaccion", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idTransaccion = cmd.Parameters["o_id_transaccion"].Value;
            if (idTransaccion != null && idTransaccion != DBNull.Value)
            {

                if (idTransaccion is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idTransaccion.ToString());
            }

            throw new Exception("No se pudo obtener el ID de la transacción");
        }

public async Task<int> RetiroDebito(int idTarjeta, int idCajero, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_retiro_debito", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_tarjeta", OracleDbType.Int32).Value = idTarjeta;
            cmd.Parameters.Add("p_id_cajero", OracleDbType.Int32).Value = idCajero;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;
            cmd.Parameters.Add("o_id_transaccion", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idTransaccion = cmd.Parameters["o_id_transaccion"].Value;
            if (idTransaccion != null && idTransaccion != DBNull.Value)
            {

                if (idTransaccion is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idTransaccion.ToString());
            }

            throw new Exception("No se pudo obtener el ID de la transacción");
        }

public async Task<int> RetiroCredito(int idTarjeta, int idCajero, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_retiro_credito", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_tarjeta", OracleDbType.Int32).Value = idTarjeta;
            cmd.Parameters.Add("p_id_cajero", OracleDbType.Int32).Value = idCajero;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;
            cmd.Parameters.Add("o_id_transaccion", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idTransaccion = cmd.Parameters["o_id_transaccion"].Value;
            if (idTransaccion != null && idTransaccion != DBNull.Value)
            {

                if (idTransaccion is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idTransaccion.ToString());
            }

            throw new Exception("No se pudo obtener el ID de la transacción");
        }

public async Task<List<model_TransaccionItem>> HistorialCuenta(int idCuenta)
        {
            var lista = new List<model_TransaccionItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_historial_cuenta", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
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
                lista.Add(new model_TransaccionItem
                {
                    idTransaccion = Convert.ToInt32(reader["ID_TRANSACCION"]),
                    idCajero = reader["ID_CAJERO"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CAJERO"]),
                    idTarjeta = reader["ID_TARJETA"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_TARJETA"]),
                    idCuentaOrigen = reader["ID_CUENTA_ORIGEN"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CUENTA_ORIGEN"]),
                    idCuentaDestino = reader["ID_CUENTA_DESTINO"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CUENTA_DESTINO"]),
                    fechaTransaccion = reader["FECHA_TRANSACCION"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_TRANSACCION"]),
                    tipoTransaccion = reader["TIPO_TRANSACCION"].ToString() ?? "",
                    montoTransaccion = Convert.ToDecimal(reader["MONTO_TRANSACCION"]),
                    estadoTransaccion = reader["ESTADO_TRANSACCION"].ToString() ?? "",
                    descripcionTransaccion = reader["DESCRIPCION_TRANSACCION"] == DBNull.Value ? null : reader["DESCRIPCION_TRANSACCION"].ToString()
                });
            }

            return lista;
        }

public async Task<List<model_TransaccionItem>> HistorialCajero(int idCuenta)
        {
            var lista = new List<model_TransaccionItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_historial_cajero", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
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
                lista.Add(new model_TransaccionItem
                {
                    idTransaccion = Convert.ToInt32(reader["ID_TRANSACCION"]),
                    idCajero = reader["ID_CAJERO"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CAJERO"]),
                    idTarjeta = reader["ID_TARJETA"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_TARJETA"]),
                    idCuentaOrigen = reader["ID_CUENTA_ORIGEN"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CUENTA_ORIGEN"]),
                    idCuentaDestino = reader["ID_CUENTA_DESTINO"] == DBNull.Value ? null : Convert.ToInt32(reader["ID_CUENTA_DESTINO"]),
                    fechaTransaccion = reader["FECHA_TRANSACCION"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FECHA_TRANSACCION"]),
                    tipoTransaccion = reader["TIPO_TRANSACCION"].ToString() ?? "",
                    montoTransaccion = Convert.ToDecimal(reader["MONTO_TRANSACCION"]),
                    estadoTransaccion = reader["ESTADO_TRANSACCION"].ToString() ?? "",
                    descripcionTransaccion = reader["DESCRIPCION_TRANSACCION"] == DBNull.Value ? null : reader["DESCRIPCION_TRANSACCION"].ToString()
                });
            }

            return lista;
        }

public async Task<List<model_TransaccionItem>> ObtenerTransaccionesPorIds(List<int> ids)
        {
            var lista = new List<model_TransaccionItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_obtener_transacciones_por_ids", conn);
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
                UdtTypeName = "PKGC_TRANSACCIONES.T_LISTA_IDS_TRANSACCION",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_TRANSACCIONES.T_LISTA_TRANSACCIONES"
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

int idTransaccion = 0;
                                        decimal montoTransaccion = 0;
                                        DateTime fechaTransaccion = DateTime.MinValue;
                                        string estadoTransaccion = "";
                                        string tipoTransaccion = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idTransaccion = (int)oracleDec0.Value;
                                                else
                                                    idTransaccion = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    montoTransaccion = oracleDec1.Value;
                                                else
                                                    montoTransaccion = Convert.ToDecimal(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo monto_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDate oracleDate)
                                                    fechaTransaccion = oracleDate.Value;
                                                else if (value2 is DateTime dt)
                                                    fechaTransaccion = dt;
                                                else
                                                    fechaTransaccion = Convert.ToDateTime(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo fecha_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estadoTransaccion = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                tipoTransaccion = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo tipo_transaccion: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_TransaccionItem
                                        {
                                            idTransaccion = idTransaccion,
                                            idCajero = null,
                                            idTarjeta = null,
                                            idCuentaOrigen = null,
                                            idCuentaDestino = null,
                                            fechaTransaccion = fechaTransaccion,
                                            tipoTransaccion = tipoTransaccion,
                                            montoTransaccion = montoTransaccion,
                                            estadoTransaccion = estadoTransaccion,
                                            descripcionTransaccion = null
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

public async Task<List<model_TransaccionItem>> ListarTransaccionesHoy()
        {
            var lista = new List<model_TransaccionItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_transacciones.procedure_listar_transacciones_hoy", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_TRANSACCIONES.T_LISTA_TRANSACCIONES"
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

int idTransaccion = 0;
                                        decimal montoTransaccion = 0;
                                        DateTime fechaTransaccion = DateTime.MinValue;
                                        string estadoTransaccion = "";
                                        string tipoTransaccion = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idTransaccion = (int)oracleDec0.Value;
                                                else
                                                    idTransaccion = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    montoTransaccion = oracleDec1.Value;
                                                else
                                                    montoTransaccion = Convert.ToDecimal(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo monto_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDate oracleDate)
                                                    fechaTransaccion = oracleDate.Value;
                                                else if (value2 is DateTime dt)
                                                    fechaTransaccion = dt;
                                                else
                                                    fechaTransaccion = Convert.ToDateTime(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo fecha_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estadoTransaccion = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_transaccion: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                tipoTransaccion = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo tipo_transaccion: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_TransaccionItem
                                        {
                                            idTransaccion = idTransaccion,
                                            idCajero = null,
                                            idTarjeta = null,
                                            idCuentaOrigen = null,
                                            idCuentaDestino = null,
                                            fechaTransaccion = fechaTransaccion,
                                            tipoTransaccion = tipoTransaccion,
                                            montoTransaccion = montoTransaccion,
                                            estadoTransaccion = estadoTransaccion,
                                            descripcionTransaccion = null
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

