using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using BancoApi.Models.models_cuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BancoApi.Repositories
{

    public class Repository_Cuenta
    {
        private readonly string _connectionString;

        public Repository_Cuenta(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<List<object>> ConsultarPorCliente(string idCliente)
        {
            var lista = new List<object>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_consultar_cuentas_por_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

await cmd.ExecuteNonQueryAsync();

            var refCursor = (Oracle.ManagedDataAccess.Types.OracleRefCursor)cmd.Parameters["result"].Value;
            if (refCursor == null)
            {
                return lista;
            }

            using var reader = refCursor.GetDataReader();

            while (reader.Read())
            {
                lista.Add(new
                {
                    idCuenta = Convert.ToInt32(reader["ID_CUENTA"]),
                    saldo = Convert.ToDecimal(reader["SALDO_CUENTA"]),
                    estado = reader["ESTADO_CUENTA"].ToString(),
                    fecha = reader["FECHA_CREACION_CUENTA"]?.ToString()
                });
            }

            return lista;
        }

public async Task<object?> ConsultarPorId(int idCuenta)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_consultar_cuentas_por_id", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var refCursor = (OracleRefCursor)cmd.Parameters["result"].Value;
            if (refCursor == null)
            {
                return null;
            }

            using var reader = refCursor.GetDataReader();

            if (reader.Read())
            {
                return new
                {
                    idCuenta = Convert.ToInt32(reader["ID_CUENTA"]),
                    idCliente = reader["ID_CLIENTE"].ToString(),
                    saldo = Convert.ToDecimal(reader["SALDO_CUENTA"]),
                    estado = reader["ESTADO_CUENTA"].ToString(),
                    fechaCreacion = reader["FECHA_CREACION_CUENTA"]?.ToString(),
                    fechaUltTrans = reader["FECHA_ULTIMA_TRANSACCION_CUENTA"]?.ToString()
                };
            }

            return null;
        }

public async Task ActualizarCuenta(int idCuenta, decimal? saldo, string? estado)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_actualizar_cuenta", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_saldo", OracleDbType.Decimal).Value = (object?)saldo ?? DBNull.Value;
            cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = (object?)estado ?? DBNull.Value;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task CambiarSaldo(int idCuenta, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_cambiar_saldo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task CambiarContrasena(int idCuenta, string nuevaContrasena)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_cambiar_contrasena", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_nueva_contrasena", OracleDbType.Varchar2).Value = nuevaContrasena;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task EliminarCuenta(int idCuenta)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_eliminar_cuenta", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_CuentaItem>> ListarActivas()
        {
            var lista = new List<model_CuentaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_listar_cuentas_activas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_CUENTA.T_LISTA_CUENTAS"
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

int idCuenta = 0;
                                        string idCliente = "";
                                        decimal saldoCuenta = 0;
                                        string estadoCuenta = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idCuenta = (int)oracleDec0.Value;
                                                else
                                                    idCuenta = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                idCliente = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    saldoCuenta = oracleDec2.Value;
                                                else
                                                    saldoCuenta = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo saldo_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estadoCuenta = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_cuenta: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_CuentaItem
                                        {
                                            idCuenta = idCuenta,
                                            idCliente = idCliente,
                                            saldoCuenta = saldoCuenta,
                                            estadoCuenta = estadoCuenta
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

public async Task<List<model_CuentaItem>> ObtenerPorIds(List<int> ids)
        {
            var lista = new List<model_CuentaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_obtener_cuentas_por_ids", conn);
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
                UdtTypeName = "PKG_CUENTA.T_LISTA_IDS_CUENTA",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_CUENTA.T_LISTA_CUENTAS"
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

int idCuenta = 0;
                                        string idCliente = "";
                                        decimal saldoCuenta = 0;
                                        string estadoCuenta = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idCuenta = (int)oracleDec0.Value;
                                                else
                                                    idCuenta = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                idCliente = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    saldoCuenta = oracleDec2.Value;
                                                else
                                                    saldoCuenta = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo saldo_cuenta: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estadoCuenta = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_cuenta: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_CuentaItem
                                        {
                                            idCuenta = idCuenta,
                                            idCliente = idCliente,
                                            saldoCuenta = saldoCuenta,
                                            estadoCuenta = estadoCuenta
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

