using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eurekabank_Soap_Dotnet.ec.edu.monster.servicio
{
    public class LoginService
    {
        public static bool Login(string username, string password)
        {
            string sql = @"
                SELECT COUNT(1)
                FROM [dbo].[usuario]
                WHERE vch_emplusuario = @user
                  AND vch_emplclave = CONVERT(VARCHAR(40), HASHBYTES('SHA1', CAST(@pass AS VARCHAR(100))), 2)
                  AND vch_emplestado = 'ACTIVO'";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["EurekaDB"].ConnectionString))
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