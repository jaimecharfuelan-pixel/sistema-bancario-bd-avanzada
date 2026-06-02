using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_abono;

namespace BancoApi.Repositories
{

    public class Repository_Abono
    {
        private readonly string _connectionString;

        public Repository_Abono(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<(int idAbono, int idTransaccion)> RegistrarAbono(
            int idPrestamo,
            decimal montoAbono,
            string tipoAbono,
            int idCuenta)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_abonos.procedure_registrar_abono", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_prestamo", OracleDbType.Int32).Value = idPrestamo;
            cmd.Parameters.Add("p_monto_abono", OracleDbType.Decimal).Value = montoAbono;
            cmd.Parameters.Add("p_tipo_abono", OracleDbType.Varchar2).Value = tipoAbono;
            cmd.Parameters.Add("p_id_cuenta", OracleDbType.Int32).Value = idCuenta;
            cmd.Parameters.Add("o_id_abono", OracleDbType.Decimal).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("o_id_transaccion", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

int idAbono = 0;
            var idAbonoValue = cmd.Parameters["o_id_abono"].Value;
            if (idAbonoValue != null && idAbonoValue != DBNull.Value)
            {
                if (idAbonoValue is OracleDecimal oracleDecimalAbono)
                {
                    idAbono = (int)oracleDecimalAbono.Value;
                }
                else
                {
                    idAbono = int.Parse(idAbonoValue.ToString());
                }
            }

            int idTransaccion = 0;
            var idTransaccionValue = cmd.Parameters["o_id_transaccion"].Value;
            if (idTransaccionValue != null && idTransaccionValue != DBNull.Value)
            {
                if (idTransaccionValue is OracleDecimal oracleDecimalTrans)
                {
                    idTransaccion = (int)oracleDecimalTrans.Value;
                }
                else
                {
                    idTransaccion = int.Parse(idTransaccionValue.ToString());
                }
            }

            return (idAbono, idTransaccion);
        }

public async Task<List<model_AbonoItem>> ListarAbonos(int idPrestamo)
        {
            var lista = new List<model_AbonoItem>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkgc_abonos.procedure_listar_abonos", conn);
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
                var item = new model_AbonoItem
                {
                    idAbono = Convert.ToInt32(reader["ID_ABONO"]),
                    idPrestamo = idPrestamo,
                    idTransaccion = reader["ID_TRANSACCION"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(reader["ID_TRANSACCION"]),
                    montoAbono = Convert.ToDecimal(reader["MONTO_ABONO"]),
                    fechaAbono = reader["FECHA_ABONO"] == DBNull.Value
                        ? null
                        : reader["FECHA_ABONO"].ToString(),
                    tipoAbono = reader["TIPO_ABONO"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

