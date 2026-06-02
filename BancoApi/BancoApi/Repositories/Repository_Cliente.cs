using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using BancoApi.Models.models_cliente;
using BancoApi.DTOs.DTOs_Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BancoApi.Repositories
{

    public class Repository_Cliente
    {
        private readonly string _connectionString;

        public Repository_Cliente(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

        public async Task<string> CrearCliente(
            string nombre,
            string email,
            string telefono,
            int cedula,
            string estado)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_crear_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_nombre_cliente", OracleDbType.Varchar2).Value = nombre;
            cmd.Parameters.Add("p_email_cliente", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("p_telefono_cliente", OracleDbType.Varchar2).Value = telefono;
            cmd.Parameters.Add("p_cedula_cliente", OracleDbType.Int32).Value = cedula;
            cmd.Parameters.Add("p_estado_cliente", OracleDbType.Varchar2).Value = estado;

            var outputId = new OracleParameter("o_id_cliente", OracleDbType.Varchar2, 20)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputId);

            await cmd.ExecuteNonQueryAsync();

            return outputId.Value?.ToString() ?? string.Empty;
        }

        public async Task ActualizarDatosCliente(
            string idCliente,
            string nombre,
            string email,
            string telefono,
            int cedula)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_actualizar_datos_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("p_nombre_cliente", OracleDbType.Varchar2).Value = nombre;
            cmd.Parameters.Add("p_email_cliente", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("p_telefono_cliente", OracleDbType.Varchar2).Value = telefono;
            cmd.Parameters.Add("p_cedula_cliente", OracleDbType.Int32).Value = cedula;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarCorreo(string idCliente, string nuevoCorreo)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_actualizar_correo_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("p_email_cliente", OracleDbType.Varchar2).Value = nuevoCorreo;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarNombre(string idCliente, string nuevoNombre)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_actualizar_nombre_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("p_nombre_cliente", OracleDbType.Varchar2).Value = nuevoNombre;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarTelefono(string idCliente, string telefono)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_actualizar_telefono_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("p_telefono_cliente", OracleDbType.Varchar2).Value = telefono;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarCedula(string idCliente, int cedula)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_actualizar_cedula_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;
            cmd.Parameters.Add("p_cedula_cliente", OracleDbType.Int32).Value = cedula;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task EliminarCliente(string idCliente)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_eliminar_cliente", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = idCliente;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_ClienteItem>> ListarActivos()
        {
            var lista = new List<model_ClienteItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

using var cmd = new OracleCommand("pkg_clientes.procedure_listar_clientes_activos", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_CLIENTES.T_LISTA_CLIENTES"
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

string idCliente = "";
                                        string nombreCliente = "";
                                        string emailCliente = "";
                                        string telefonoCliente = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                                idCliente = Convert.ToString(value0) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                nombreCliente = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo nombre_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                                emailCliente = Convert.ToString(value2) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo email_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                telefonoCliente = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo telefono_cliente: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_ClienteItem
                                        {
                                            idCliente = idCliente,
                                            nombreCliente = nombreCliente,
                                            emailCliente = emailCliente,
                                            telefonoCliente = telefonoCliente,
                                            cedulaCliente = 0
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

public async Task<List<model_ClienteItem>> ObtenerPorIds(List<string> ids)
        {
            var lista = new List<model_ClienteItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_obtener_clientes_por_ids", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var idsArray = new string[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                idsArray[i] = ids[i];
            }

            var inParam = new OracleParameter("p_ids", OracleDbType.Array)
            {
                Direction = ParameterDirection.Input,
                UdtTypeName = "PKG_CLIENTES.T_LISTA_IDS_CLIENTE",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKG_CLIENTES.T_LISTA_CLIENTES"
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

string idCliente = "";
                                        string nombreCliente = "";
                                        string emailCliente = "";
                                        string telefonoCliente = "";
                                        
                                        try
                                        {
                                            var value0 = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                            if (value0 != null && !Convert.IsDBNull(value0))
                                                idCliente = Convert.ToString(value0) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo id_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value1 = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                            if (value1 != null && !Convert.IsDBNull(value1))
                                                nombreCliente = Convert.ToString(value1) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo nombre_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value2 = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                            if (value2 != null && !Convert.IsDBNull(value2))
                                                emailCliente = Convert.ToString(value2) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo email_cliente: {ex.Message}");
                                        }
                                        
                                        try
                                        {
                                            var value3 = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                            if (value3 != null && !Convert.IsDBNull(value3))
                                                telefonoCliente = Convert.ToString(value3) ?? "";
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Error obteniendo telefono_cliente: {ex.Message}");
                                        }
                                        
                                        lista.Add(new model_ClienteItem
                                        {
                                            idCliente = idCliente,
                                            nombreCliente = nombreCliente,
                                            emailCliente = emailCliente,
                                            telefonoCliente = telefonoCliente,
                                            cedulaCliente = 0
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

public async Task<int> ProcesarMasivos(List<DTO_ClienteProcesarMasivos.ClienteInfo> clientes)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_procesar_clientes_masivos", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var clientesArray = new object[clientes.Count];
            for (int i = 0; i < clientes.Count; i++)
            {

var clienteStruct = new object[]
                {
                    clientes[i].idCliente ?? "",
                    clientes[i].nombreCliente ?? "",
                    clientes[i].emailCliente ?? "",
                    clientes[i].telefonoCliente ?? ""
                };
                clientesArray[i] = clienteStruct;
            }

            var inParam = new OracleParameter("p_clientes", OracleDbType.Array)
            {
                Direction = ParameterDirection.Input,
                UdtTypeName = "PKG_CLIENTES.T_LISTA_CLIENTES",
                Value = clientesArray,
                Size = clientes.Count
            };
            cmd.Parameters.Add(inParam);

            var outParam = new OracleParameter("o_resultado", OracleDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            await cmd.ExecuteNonQueryAsync();

            var resultado = outParam.Value;
            if (resultado != null && resultado != DBNull.Value)
            {
                if (resultado is OracleDecimal oracleDec)
                    return (int)oracleDec.Value;
                return Convert.ToInt32(resultado);
            }

            return 0;
        }
    }
}

