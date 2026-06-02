using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BancoApi.Models;
using BancoApi.Models.models_cajero;

namespace BancoApi.Repositories
{

    public class Repository_Cajero
    {
        private readonly string _connectionString;

        public Repository_Cajero(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<int> CrearCajero(int idSucursal, int idAdministrador, decimal dineroInicial)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_crear_cajero", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;
            cmd.Parameters.Add("p_id_admin", OracleDbType.Int32).Value = idAdministrador;
            cmd.Parameters.Add("p_dinero_inicial", OracleDbType.Decimal).Value = dineroInicial;

            var output = new OracleParameter("o_id_cajero", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            await cmd.ExecuteNonQueryAsync();

            return Convert.ToInt32(output.Value.ToString());
        }

public async Task CambiarEstado(int idCajero, string estado)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_cambiar_estado_cajero", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cajero", OracleDbType.Int32).Value = idCajero;
            cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = estado;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task Recargar(int idCajero, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_recargar_cajero", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cajero", OracleDbType.Int32).Value = idCajero;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task Descontar(int idCajero, decimal monto)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_descontar_cajero", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cajero", OracleDbType.Int32).Value = idCajero;
            cmd.Parameters.Add("p_monto", OracleDbType.Decimal).Value = monto;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_CajeroLista>> Listar(int idSucursal)
        {
            var lista = new List<model_CajeroLista>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_listar_cajeros", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;
            cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var cursor = (OracleRefCursor)cmd.Parameters["o_cursor"].Value;

            using var reader = cursor.GetDataReader();

            while (reader.Read())
            {
                lista.Add(new model_CajeroLista
                {
                    idCajero = Convert.ToInt32(reader["ID_CAJERO"]),
                    dineroDisponible = Convert.ToDecimal(reader["DINERO_DISPONIBLE_CAJERO"]),
                    estado = reader["ESTADO_CAJERO"].ToString(),
                    fechaUltimaRecarga = Convert.ToDateTime(reader["FECHA_ULTIMA_RECARGA_CAJERO"])
                });
            }

            return lista;
        }

public async Task<List<model_CajeroItem>> ListarActivos()
        {
            var lista = new List<model_CajeroItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_listar_cajeros_activos", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_CAJEROS.T_LISTA_CAJEROS"
            };
            cmd.Parameters.Add(outParam);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException oraEx)
            {

                if (oraEx.Number == 50032 || oraEx.Message.Contains("ORA-50032") || oraEx.Message.Contains("Column contains NULL data"))
                {
                    throw new Exception($"Error al ejecutar procedure_listar_cajeros_activos: El procedimiento devolvió datos con valores NULL. Verifique que el paquete PKGC_CAJEROS tenga NVL en el cursor c_cajeros_activos. Detalle: {oraEx.Message}", oraEx);
                }
                throw;
            }

var arrayValue = outParam.Value;
            
            if (arrayValue is OracleRefCursor refCursor)
            {

                using var reader = refCursor.GetDataReader();
                while (reader.Read())
                {
                    lista.Add(new model_CajeroItem
                    {
                        idCajero = Convert.ToInt32(reader["ID_CAJERO"]),
                        idSucursal = Convert.ToInt32(reader["ID_SUCURSAL"]),
                        dineroDisponible = Convert.ToDecimal(reader["DINERO_DISPONIBLE_CAJERO"]),
                        estado = reader["ESTADO_CAJERO"].ToString() ?? ""
                    });
                }
                return lista;
            }

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

int idCajero = 0;
                                        int idSucursal = 0;
                                        decimal dineroDisponible = 0;
                                        string estado = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idCajero = (int)oracleDec0.Value;
                                                else
                                                    idCajero = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cajero: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    idSucursal = (int)oracleDec1.Value;
                                                else
                                                    idSucursal = Convert.ToInt32(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    dineroDisponible = oracleDec2.Value;
                                                else
                                                    dineroDisponible = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo dinero_disponible: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estado = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_CajeroItem
                                        {
                                            idCajero = idCajero,
                                            idSucursal = idSucursal,
                                            dineroDisponible = dineroDisponible,
                                            estado = estado
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

public async Task<List<model_CajeroItem>> ObtenerPorIds(List<int> ids)
        {
            var lista = new List<model_CajeroItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cajeros.procedure_obtener_cajeros_por_ids", conn);
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
                UdtTypeName = "PKGC_CAJEROS.T_LISTA_IDS_CAJERO",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_CAJEROS.T_LISTA_CAJEROS"
            };
            cmd.Parameters.Add(outParam);

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (OracleException oraEx)
            {

                if (oraEx.Number == 50032 || oraEx.Message.Contains("ORA-50032") || oraEx.Message.Contains("Column contains NULL data"))
                {
                    throw new Exception($"Error al ejecutar procedure_obtener_cajeros_por_ids: El procedimiento devolvió datos con valores NULL. Verifique que el paquete PKGC_CAJEROS tenga NVL en el SELECT. Detalle: {oraEx.Message}", oraEx);
                }
                throw;
            }

var arrayValue = outParam.Value;
            
            if (arrayValue is OracleRefCursor refCursor)
            {

                using var reader = refCursor.GetDataReader();
                while (reader.Read())
                {
                    lista.Add(new model_CajeroItem
                    {
                        idCajero = Convert.ToInt32(reader["ID_CAJERO"]),
                        idSucursal = Convert.ToInt32(reader["ID_SUCURSAL"]),
                        dineroDisponible = Convert.ToDecimal(reader["DINERO_DISPONIBLE_CAJERO"]),
                        estado = reader["ESTADO_CAJERO"].ToString() ?? ""
                    });
                }
                return lista;
            }

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

int idCajero = 0;
                                        int idSucursal = 0;
                                        decimal dineroDisponible = 0;
                                        string estado = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idCajero = (int)oracleDec0.Value;
                                                else
                                                    idCajero = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cajero: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                            {
                                                if (value1 is OracleDecimal oracleDec1)
                                                    idSucursal = (int)oracleDec1.Value;
                                                else
                                                    idSucursal = Convert.ToInt32(value1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                            {
                                                if (value2 is OracleDecimal oracleDec2)
                                                    dineroDisponible = oracleDec2.Value;
                                                else
                                                    dineroDisponible = Convert.ToDecimal(value2);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo dinero_disponible: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                estado = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_CajeroItem
                                        {
                                            idCajero = idCajero,
                                            idSucursal = idSucursal,
                                            dineroDisponible = dineroDisponible,
                                            estado = estado
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

