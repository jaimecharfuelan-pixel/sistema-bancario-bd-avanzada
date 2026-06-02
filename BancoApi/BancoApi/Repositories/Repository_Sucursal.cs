using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BancoApi.Models;

namespace BancoApi.Repositories
{
    public class Repository_Sucursal
    {
        private readonly string _connectionString;

        public Repository_Sucursal(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

        public async Task<List<model_SucursalLista>> ListarSucursales()
        {
            var lista = new List<model_SucursalLista>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.procedure_listar_sucursales", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var refCursor = (OracleRefCursor)cmd.Parameters["o_cursor"].Value;
            using var reader = refCursor.GetDataReader();

            while (reader.Read())
            {
                lista.Add(new model_SucursalLista
                {
                    idSucursal = Convert.ToInt32(reader["ID_SUCURSAL"]),
                    nombreSucursal = reader["NOMBRE_SUCURSAL"].ToString(),
                    direccionSucursal = reader["DIRECCION_SUCURSAL"].ToString(),
                    telefonoSucursal = reader["TELEFONO_SUCURSAL"].ToString(),
                    estadoSucursal = reader["ESTADO_SUCURSAL"].ToString(),
                    idAdministrador = reader["ID_ADMINISTRADOR"] == DBNull.Value
                                        ? (int?)null
                                        : Convert.ToInt32(reader["ID_ADMINISTRADOR"])
                });
            }

            return lista;
        }

public async Task<int> CrearSucursal(string nombre, string direccion, string telefono, int? idAdministrador)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.function_crear_sucursal", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;

            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = nombre;
            cmd.Parameters.Add("p_direccion", OracleDbType.Varchar2).Value = direccion;
            cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = telefono;

            cmd.Parameters.Add("p_id_admin", OracleDbType.Int32).Value =
                idAdministrador.HasValue
                    ? idAdministrador.Value
                    : (object)DBNull.Value;

            var output = new OracleParameter("RETURN_VALUE", OracleDbType.Int32)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(output);

            await cmd.ExecuteNonQueryAsync();

            return int.Parse(output.Value.ToString());
        }

public async Task EditarEstado(int idSucursal, string estado)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.procedure_editar_estado", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;
            cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = estado;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task EliminarSucursal(int idSucursal)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.procedure_eliminar_sucursal", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_sucursal", OracleDbType.Int32).Value = idSucursal;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_SucursalLista>> ObtenerSucursalesPorIds(List<int> ids)
        {
            var lista = new List<model_SucursalLista>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.procedure_obtener_sucursales_por_ids", conn);
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
                UdtTypeName = "PKG_SUCURSAL.T_LISTA_IDS_SUCURSAL",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_SUCURSAL.T_LISTA_SUCURSALES"
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

int idSucursal = 0;
                                        string nombreSucursal = "";
                                        string direccionSucursal = "";
                                        string telefonoSucursal = "";
                                        string estadoSucursal = "";
                                        int? idAdministrador = null;
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idSucursal = (int)oracleDec0.Value;
                                                else
                                                    idSucursal = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                nombreSucursal = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo nombre_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                                direccionSucursal = Convert.ToString(value2) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo direccion_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                telefonoSucursal = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo telefono_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                estadoSucursal = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value5 = getStructValueMethod.Invoke(structValue, new object[] { 5 });
                                            if (value5 != null && !Convert.IsDBNull(value5))
                                            {
                                                if (value5 is OracleDecimal oracleDec5)
                                                    idAdministrador = (int)oracleDec5.Value;
                                                else
                                                    idAdministrador = Convert.ToInt32(value5);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_administrador: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_SucursalLista
                                        {
                                            idSucursal = idSucursal,
                                            nombreSucursal = nombreSucursal,
                                            direccionSucursal = direccionSucursal,
                                            telefonoSucursal = telefonoSucursal,
                                            estadoSucursal = estadoSucursal,
                                            idAdministrador = idAdministrador
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

public async Task<List<model_SucursalLista>> ListarSucursalesAbiertas()
        {
            var lista = new List<model_SucursalLista>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_sucursal.procedure_listar_sucursales_abiertas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_SUCURSAL.T_LISTA_SUCURSALES"
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

int idSucursal = 0;
                                        string nombreSucursal = "";
                                        string direccionSucursal = "";
                                        string telefonoSucursal = "";
                                        string estadoSucursal = "";
                                        int? idAdministrador = null;
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                            {
                                                if (value0 is OracleDecimal oracleDec0)
                                                    idSucursal = (int)oracleDec0.Value;
                                                else
                                                    idSucursal = Convert.ToInt32(value0);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                nombreSucursal = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo nombre_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                                direccionSucursal = Convert.ToString(value2) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo direccion_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                telefonoSucursal = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo telefono_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value4 = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                            if (value4 != null && !Convert.IsDBNull(value4))
                                                estadoSucursal = Convert.ToString(value4) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo estado_sucursal: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value5 = getStructValueMethod.Invoke(structValue, new object[] { 5 });
                                            if (value5 != null && !Convert.IsDBNull(value5))
                                            {
                                                if (value5 is OracleDecimal oracleDec5)
                                                    idAdministrador = (int)oracleDec5.Value;
                                                else
                                                    idAdministrador = Convert.ToInt32(value5);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_administrador: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_SucursalLista
                                        {
                                            idSucursal = idSucursal,
                                            nombreSucursal = nombreSucursal,
                                            direccionSucursal = direccionSucursal,
                                            telefonoSucursal = telefonoSucursal,
                                            estadoSucursal = estadoSucursal,
                                            idAdministrador = idAdministrador
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

