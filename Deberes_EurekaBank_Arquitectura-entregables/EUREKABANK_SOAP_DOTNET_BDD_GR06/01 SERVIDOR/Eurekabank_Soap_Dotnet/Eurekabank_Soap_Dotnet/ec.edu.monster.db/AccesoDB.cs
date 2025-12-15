using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Eurekabank_Soap_Dotnet.ec.edu.monster.db
{
    public class AccesoDB
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["EurekaDB"].ConnectionString;

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                return cn;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al conectar a la base de datos: " + ex.Message);
            }
        }
    }
}