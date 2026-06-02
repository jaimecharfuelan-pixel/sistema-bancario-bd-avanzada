using Oracle.ManagedDataAccess.Client;
using System.Data;
using System;
using System.Threading.Tasks;

namespace BancoApi.Repositories
{

    public class Repository_Autenticacion
    {
        private readonly string _connectionString;

        public Repository_Autenticacion(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("OracleDb");
        }

public async Task<string?> LoginAdmin(string email, string contrasena)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand(
                "BEGIN :result := pkg_administrador.function_login_admin(:email, :pass); END;",
                conn);

            cmd.Parameters.Add("result", OracleDbType.Decimal).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("pass", OracleDbType.Varchar2).Value = contrasena;

            await cmd.ExecuteNonQueryAsync();

            var adminResult = cmd.Parameters["result"].Value;

            if (adminResult != null && adminResult.ToString() != "" && adminResult.ToString() != "null")
            {
                return adminResult.ToString();
            }

            return null;
        }

public async Task<string?> LoginCuenta(string email, string contrasena)
        {
            using var conn = new OracleConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new OracleCommand(
                "BEGIN :result := pkg_cuenta.function_login_cuenta(:email, :pass); END;",
                conn);

            cmd.Parameters.Add("result", OracleDbType.Decimal).Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("pass", OracleDbType.Varchar2).Value = contrasena;

            await cmd.ExecuteNonQueryAsync();

            var cuentaResult = cmd.Parameters["result"].Value;

            if (cuentaResult != null && cuentaResult.ToString() != "" && cuentaResult.ToString() != "null")
            {
                return cuentaResult.ToString();
            }

            return null;
        }
    }
}

