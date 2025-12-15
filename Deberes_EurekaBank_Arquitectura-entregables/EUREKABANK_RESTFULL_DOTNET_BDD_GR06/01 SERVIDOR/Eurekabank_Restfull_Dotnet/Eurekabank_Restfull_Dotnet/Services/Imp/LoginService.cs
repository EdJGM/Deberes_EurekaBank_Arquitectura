using Eurekabank_Restfull_Dotnet.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Eurekabank_Restfull_Dotnet.Services.Imp
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("EurekaDB");

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Login(string username, string password)
        {
            string sql = @"
                SELECT COUNT(1)
                FROM usuario
                WHERE vch_emplusuario = @user
                  AND vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', CAST(@pass AS VARCHAR(100))), 2)
                  AND vch_emplestado = 'ACTIVO'";

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@user", SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@pass", SqlDbType.VarChar, 100).Value = password;

                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result) == 1;
                }
            }
        }
    }
}
