using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using BancoApi.DTOs.DTOs_Tarjeta;
using BancoApi.Models.models_tarjeta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BancoApi.Repositories
{

    public class Repository_Tarjeta
    {
        private readonly string _connectionString;

        public Repository_Tarjeta(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<int> CrearTarjetaDebito(int idCuenta, decimal limiteRetiro)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_crear_tarjeta_debito", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_limite_retiro", OracleDbType.Decimal).Value = limiteRetiro;
            cmd.Parameters.Add("o_id_tarjeta", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idTarjeta = cmd.Parameters["o_id_tarjeta"].Value;
            if (idTarjeta != null && idTarjeta != DBNull.Value)
            {

                if (idTarjeta is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idTarjeta.ToString());
            }

            throw new Exception("No se pudo obtener el ID de la tarjeta");
        }

public async Task<int> CrearTarjetaCredito(
            int idCuenta,
            decimal limiteCredito,
            decimal tasaInteres,
            decimal cuotaManejo,
            int fechaCorte)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_crear_tarjeta_credito", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("p_limite_credito", OracleDbType.Decimal).Value = limiteCredito;
            cmd.Parameters.Add("p_tasa_interes", OracleDbType.Decimal).Value = tasaInteres;
            cmd.Parameters.Add("p_cuota_manejo", OracleDbType.Decimal).Value = cuotaManejo;
            cmd.Parameters.Add("p_fecha_corte", OracleDbType.Int32).Value = fechaCorte;
            cmd.Parameters.Add("o_id_tarjeta", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idTarjeta = cmd.Parameters["o_id_tarjeta"].Value;
            if (idTarjeta != null && idTarjeta != DBNull.Value)
            {

                if (idTarjeta is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idTarjeta.ToString());
            }

            throw new Exception("No se pudo obtener el ID de la tarjeta");
        }

public async Task CambiarEstadoTarjeta(int idTarjeta, string estado)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_cambiar_estado_tarjeta", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_tarjeta", OracleDbType.Int32).Value = idTarjeta;
            cmd.Parameters.Add("p_estado", OracleDbType.Varchar2).Value = estado;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<model_TarjetaItem>> ListarTarjetas(int idCuenta)
        {
            var resultado = new List<model_TarjetaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_listar_tarjetas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("o_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var refCursor = (OracleRefCursor)cmd.Parameters["o_cursor"].Value;
            if (refCursor == null)
            {
                return resultado;
            }

            using var reader = refCursor.GetDataReader();

            while (reader.Read())
            {
                var tarjeta = new model_TarjetaItem
                {
                    idTarjeta = Convert.ToInt32(reader["ID_TARJETA"]),
                    idCuenta = idCuenta,
                    numeroTarjeta = reader["NUMERO_TARJETA"].ToString() ?? string.Empty,
                    cvvTarjeta = reader["CVV_TARJETA"].ToString() ?? string.Empty,
                    estadoTarjeta = reader["ESTADO_TARJETA"].ToString() ?? string.Empty,
                    fechaEmision = Convert.ToDateTime(reader["FECHA_EMISION_TARJETA"]),
                    fechaVencimiento = Convert.ToDateTime(reader["FECHA_VENCIMIENTO_TARJETA"]),
                    tipoTarjeta = reader["TIPO_TARJETA"].ToString() ?? string.Empty
                };

                resultado.Add(tarjeta);
            }

            return resultado;
        }

public async Task<List<model_TarjetaItem>> ListarActivas()
        {
            var lista = new List<model_TarjetaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_listar_tarjetas_activas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

var outParam = new OracleParameter("o_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_TARJETAS.T_LISTA_TARJETAS"
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

var idTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                        var idCuenta = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                        var numeroTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                        var estadoTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                        var tipoTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                        
                                        lista.Add(new model_TarjetaItem
                                        {
                                            idTarjeta = idTarjeta != null && idTarjeta != DBNull.Value 
                                                ? Convert.ToInt32(idTarjeta) : 0,
                                            idCuenta = idCuenta != null && idCuenta != DBNull.Value 
                                                ? Convert.ToInt32(idCuenta) : 0,
                                            numeroTarjeta = numeroTarjeta?.ToString() ?? "",
                                            estadoTarjeta = estadoTarjeta?.ToString() ?? "",
                                            tipoTarjeta = tipoTarjeta?.ToString() ?? "",
                                            cvvTarjeta = "",
                                            fechaEmision = DateTime.MinValue,
                                            fechaVencimiento = DateTime.MinValue
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                System.Diagnostics.Debug.WriteLine($"Error procesando elemento {i}: {ex.Message}");
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

public async Task<List<model_TarjetaItem>> ObtenerPorIds(List<int> ids)
        {
            var lista = new List<model_TarjetaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_tarjetas.procedure_obtener_tarjetas_por_ids", conn);
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
                UdtTypeName = "PKGC_TARJETAS.T_LISTA_IDS_TARJETA",
                Value = idsArray,
                Size = ids.Count
            };
            cmd.Parameters.Add(inParam);

var outParam = new OracleParameter("p_resultado", OracleDbType.Array)
            {
                Direction = ParameterDirection.Output,
                UdtTypeName = "PKGC_TARJETAS.T_LISTA_TARJETAS"
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

var idTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 0 });
                                        var idCuenta = getStructValueMethod.Invoke(structValue, new object[] { 1 });
                                        var numeroTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 2 });
                                        var estadoTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 3 });
                                        var tipoTarjeta = getStructValueMethod.Invoke(structValue, new object[] { 4 });
                                        
                                        lista.Add(new model_TarjetaItem
                                        {
                                            idTarjeta = idTarjeta != null && idTarjeta != DBNull.Value 
                                                ? Convert.ToInt32(idTarjeta) : 0,
                                            idCuenta = idCuenta != null && idCuenta != DBNull.Value 
                                                ? Convert.ToInt32(idCuenta) : 0,
                                            numeroTarjeta = numeroTarjeta?.ToString() ?? "",
                                            estadoTarjeta = estadoTarjeta?.ToString() ?? "",
                                            tipoTarjeta = tipoTarjeta?.ToString() ?? "",
                                            cvvTarjeta = "",
                                            fechaEmision = DateTime.MinValue,
                                            fechaVencimiento = DateTime.MinValue
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                System.Diagnostics.Debug.WriteLine($"Error procesando elemento {i}: {ex.Message}");
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

