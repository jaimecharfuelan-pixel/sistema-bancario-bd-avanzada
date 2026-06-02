using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_cuota;

namespace BancoApi.Repositories
{

    public class Repository_Cuota
    {
        private readonly string _connectionString;

        public Repository_Cuota(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task GenerarCuotas(int idPrestamo)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cuotas.procedure_generar_cuotas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_prestamo", OracleDbType.Int32).Value = idPrestamo;

            await cmd.ExecuteNonQueryAsync();
        }

public async Task<int> PagarCuota(int idCuota, int idCuenta)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cuotas.procedure_pagar_cuota", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cuota", OracleDbType.Int32).Value = idCuota;
            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
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

public async Task<List<model_CuotaItem>> ListarCuotas(int idPrestamo)
        {
            var lista = new List<model_CuotaItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_cuotas.procedure_listar_cuotas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_prestamo", OracleDbType.Int32).Value = idPrestamo;
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
                var item = new model_CuotaItem
                {
                    idCuota = Convert.ToInt32(reader["ID_CUOTA"]),
                    idPrestamo = idPrestamo,
                    numeroCuota = Convert.ToInt32(reader["NUMERO_CUOTA"]),
                    montoDeCuota = Convert.ToDecimal(reader["MONTO_DE_CUOTA"]),
                    capitalCuota = Convert.ToDecimal(reader["CAPITAL_CUOTA"]),
                    fechaDeVencimientoCuota = reader["FECHA_DE_VENCIMIENTO_CUOTA"] == DBNull.Value
                        ? null
                        : reader["FECHA_DE_VENCIMIENTO_CUOTA"].ToString(),
                    fechaDePagoCuota = reader["FECHA_DE_PAGO_CUOTA"] == DBNull.Value
                        ? null
                        : reader["FECHA_DE_PAGO_CUOTA"].ToString(),
                    estadoCuota = reader["ESTADO_CUOTA"].ToString() ?? ""
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

