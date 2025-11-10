using Eurekabank_Restfull_Dotnet.Models;
using Eurekabank_Restfull_Dotnet.Services.Interfaces;
using System.Data.SqlClient;

namespace Eurekabank_Restfull_Dotnet.Services.Imp
{
    public class EurekaService : IEurekaService
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("EurekaDB");

        public EurekaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Movimiento> ListarPorCuenta(string cuenta)
        {
            var lista = new List<Movimiento>();
            string sql = @"
                SELECT
                    m.chr_cuencodigo cuenta,
                    m.int_movinumero nromov,
                    m.dtt_movifecha fecha,
                    t.vch_tipodescripcion tipo,
                    t.vch_tipoaccion accion,
                    m.dec_moviimporte importe
                FROM tipomovimiento t
                INNER JOIN movimiento m ON t.chr_tipocodigo = m.chr_tipocodigo
                WHERE m.chr_cuencodigo = @cuenta";

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Movimiento
                    {
                        Cuenta = dr["cuenta"].ToString(),
                        NroMov = Convert.ToInt32(dr["nromov"]),
                        Fecha = Convert.ToDateTime(dr["fecha"]),
                        Tipo = dr["tipo"].ToString(),
                        Accion = dr["accion"].ToString(),
                        Importe = Convert.ToDouble(dr["importe"])
                    });
                }
            }
            return lista;
        }

        public void RegistrarDeposito(string cuenta, double importe, string codEmp = "0001")
        {
            EjecutarMovimiento(cuenta, importe, codEmp, "003", true);
        }

        public void RegistrarRetiro(string cuenta, double importe, string codEmp = "0001")
        {
            EjecutarMovimiento(cuenta, -importe, codEmp, "004", false);
        }

        public void RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe, string codEmp = "0001")
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    EjecutarMovimientoInterno(cuentaOrigen, -importe, codEmp, "009", cn, tx, false);
                    EjecutarMovimientoInterno(cuentaDestino, importe, codEmp, "008", cn, tx, true);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        private void EjecutarMovimiento(string cuenta, double importe, string codEmp, string tipoMov, bool permiteNegativo)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    EjecutarMovimientoInterno(cuenta, importe, codEmp, tipoMov, cn, tx, permiteNegativo);
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        private void EjecutarMovimientoInterno(string cuenta, double importe, string codEmp, string tipoMov,
                                              SqlConnection cn, SqlTransaction tx, bool permiteNegativo)
        {
            string sql = "SELECT dec_cuensaldo, int_cuencontmov FROM cuenta WHERE chr_cuencodigo = @cuenta AND vch_cuenestado = 'ACTIVO'";
            SqlCommand cmd = new SqlCommand(sql, cn, tx);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);
            SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                dr.Close();
                throw new Exception("ERROR: Cuenta no existe o no está activa.");
            }

            double saldo = Convert.ToDouble(dr["dec_cuensaldo"]);
            int cont = Convert.ToInt32(dr["int_cuencontmov"]);
            dr.Close();

            saldo += importe;
            if (!permiteNegativo && saldo < 0)
            {
                throw new Exception("ERROR: Saldo insuficiente.");
            }

            cont++;

            cmd = new SqlCommand("UPDATE cuenta SET dec_cuensaldo = @saldo, int_cuencontmov = @cont WHERE chr_cuencodigo = @cuenta", cn, tx);
            cmd.Parameters.AddWithValue("@saldo", saldo);
            cmd.Parameters.AddWithValue("@cont", cont);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand(@"
                INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte)
                VALUES (@cuenta, @nro, GETDATE(), @codEmp, @tipoMov, @importe)", cn, tx);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);
            cmd.Parameters.AddWithValue("@nro", cont);
            cmd.Parameters.AddWithValue("@codEmp", codEmp);
            cmd.Parameters.AddWithValue("@tipoMov", tipoMov);
            cmd.Parameters.AddWithValue("@importe", Math.Abs(importe));
            cmd.ExecuteNonQuery();
        }
    }
}
