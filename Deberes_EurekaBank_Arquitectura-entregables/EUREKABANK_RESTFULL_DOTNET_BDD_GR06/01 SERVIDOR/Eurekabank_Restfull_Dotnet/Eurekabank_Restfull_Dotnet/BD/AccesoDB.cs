using System.Data.SqlClient;

namespace Eurekabank_Restfull_Dotnet.BD
{
    public class AccesoDB
    {
        private readonly IConfiguration _configuration;

        public AccesoDB(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection ObtenerConexion()
        {
            string connectionString = _configuration.GetConnectionString("EurekaDB");
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
