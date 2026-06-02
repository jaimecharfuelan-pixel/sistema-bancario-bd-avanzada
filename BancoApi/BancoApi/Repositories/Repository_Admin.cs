using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BancoApi.Models.models_admin;

namespace BancoApi.Repositories
{
    public class Repository_Admin
    {
        private readonly string _connectionString;

        public Repository_Admin(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

        public async Task<List<object>> ObtenerSolicitudes()
        {
            var solicitudes = new List<object>();

            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_clientes.procedure_obtener_solicitudes", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var refCursor = (OracleRefCursor)cmd.Parameters["result"].Value;
            using var reader = refCursor.GetDataReader();

            while (reader.Read())
            {
                solicitudes.Add(new
                {
                    idCliente = reader["ID_CLIENTE"].ToString(),
                    nombre = reader["NOMBRE_CLIENTE"].ToString(),
                    email = reader["EMAIL_CLIENTE"].ToString(),
                    telefono = reader["TELEFONO_CLIENTE"].ToString(),
                    cedula = reader["CEDULA_CLIENTE"].ToString()
                });
            }

            return solicitudes;
        }

        public async Task<int> CrearCuenta(model_AdminCrearCuenta model)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand("pkg_cuenta.procedure_crear_cuenta", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = model.idCliente;
            cmd.Parameters.Add("p_id_administrador", OracleDbType.Int32).Value = model.idAdministrador;
            cmd.Parameters.Add("o_id_cuenta", OracleDbType.Decimal).Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            var idCuenta = cmd.Parameters["o_id_cuenta"].Value;
            if (idCuenta != null && idCuenta != DBNull.Value)
            {
                if (idCuenta is OracleDecimal oracleDecimal)
                {
                    return (int)oracleDecimal.Value;
                }
                return int.Parse(idCuenta.ToString() ?? "0");
            }

            throw new Exception("No se pudo obtener el ID de la cuenta creada");
        }

    }
}

