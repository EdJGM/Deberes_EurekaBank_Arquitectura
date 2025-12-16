using Eurekabank_Restfull_Dotnet.Models;
using Eurekabank_Restfull_Dotnet.Services.Interfaces;
using System.Data.SqlClient;

namespace Eurekabank_Restfull_Dotnet.Services.Imp
{
    /// <summary>
    /// Implementación del servicio de Sucursales
    /// </summary>
    public class SucursalService : ISucursalService
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("EurekaDB");

        public SucursalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Sucursal>> ListarSucursalesAsync()
        {
            var sucursales = new List<Sucursal>();
            string sql = @"
                SELECT 
                    chr_sucucodigo,
                    vch_sucunombre,
                    vch_sucuciudad,
                    vch_sucudireccion,
                    int_sucucontcuenta,
                    COALESCE(dec_sucuclatitud, 0) as latitud,
                    COALESCE(dec_sucuclongitud, 0) as longitud,
                    COALESCE(vch_sucutelefono, '') as telefono,
                    COALESCE(vch_sucuemail, '') as email,
                    COALESCE(vch_sucuestado, 'ACTIVO') as estado
                FROM sucursal 
                ORDER BY chr_sucucodigo";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sucursales.Add(new Sucursal
                            {
                                Codigo = reader["chr_sucucodigo"].ToString() ?? "",
                                Nombre = reader["vch_sucunombre"].ToString() ?? "",
                                Ciudad = reader["vch_sucuciudad"].ToString() ?? "",
                                Direccion = reader["vch_sucudireccion"].ToString() ?? "",
                                ContadorCuentas = Convert.ToInt32(reader["int_sucucontcuenta"]),
                                Latitud = Convert.ToDouble(reader["latitud"]),
                                Longitud = Convert.ToDouble(reader["longitud"]),
                                Telefono = reader["telefono"].ToString() ?? "",
                                Email = reader["email"].ToString() ?? "",
                                Estado = reader["estado"].ToString() ?? "ACTIVO"
                            });
                        }
                    }
                }
            }

            return sucursales;
        }

        public async Task<Sucursal?> ObtenerSucursalAsync(string codigo)
        {
            string sql = @"
                SELECT 
                    chr_sucucodigo,
                    vch_sucunombre,
                    vch_sucuciudad,
                    vch_sucudireccion,
                    int_sucucontcuenta,
                    COALESCE(dec_sucuclatitud, 0) as latitud,
                    COALESCE(dec_sucuclongitud, 0) as longitud,
                    COALESCE(vch_sucutelefono, '') as telefono,
                    COALESCE(vch_sucuemail, '') as email,
                    COALESCE(vch_sucuestado, 'ACTIVO') as estado
                FROM sucursal 
                WHERE chr_sucucodigo = @codigo";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@codigo", codigo);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Sucursal
                            {
                                Codigo = reader["chr_sucucodigo"].ToString() ?? "",
                                Nombre = reader["vch_sucunombre"].ToString() ?? "",
                                Ciudad = reader["vch_sucuciudad"].ToString() ?? "",
                                Direccion = reader["vch_sucudireccion"].ToString() ?? "",
                                ContadorCuentas = Convert.ToInt32(reader["int_sucucontcuenta"]),
                                Latitud = Convert.ToDouble(reader["latitud"]),
                                Longitud = Convert.ToDouble(reader["longitud"]),
                                Telefono = reader["telefono"].ToString() ?? "",
                                Email = reader["email"].ToString() ?? "",
                                Estado = reader["estado"].ToString() ?? "ACTIVO"
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> CrearSucursalAsync(SucursalRequest sucursal)
        {
            string sql = @"
                INSERT INTO sucursal (
                    chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion,
                    int_sucucontcuenta, dec_sucuclatitud, dec_sucuclongitud,
                    vch_sucutelefono, vch_sucuemail, vch_sucuestado
                ) VALUES (
                    @codigo, @nombre, @ciudad, @direccion,
                    @contadorCuentas, @latitud, @longitud,
                    @telefono, @email, @estado
                )";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@codigo", sucursal.Codigo);
                    command.Parameters.AddWithValue("@nombre", sucursal.Nombre);
                    command.Parameters.AddWithValue("@ciudad", sucursal.Ciudad);
                    command.Parameters.AddWithValue("@direccion", sucursal.Direccion);
                    command.Parameters.AddWithValue("@contadorCuentas", sucursal.ContadorCuentas);
                    command.Parameters.AddWithValue("@latitud", sucursal.Latitud);
                    command.Parameters.AddWithValue("@longitud", sucursal.Longitud);
                    command.Parameters.AddWithValue("@telefono", sucursal.Telefono);
                    command.Parameters.AddWithValue("@email", sucursal.Email);
                    command.Parameters.AddWithValue("@estado", sucursal.Estado);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> ActualizarSucursalAsync(SucursalRequest sucursal)
        {
            string sql = @"
                UPDATE sucursal SET
                    vch_sucunombre = @nombre,
                    vch_sucuciudad = @ciudad,
                    vch_sucudireccion = @direccion,
                    int_sucucontcuenta = @contadorCuentas,
                    dec_sucuclatitud = @latitud,
                    dec_sucuclongitud = @longitud,
                    vch_sucutelefono = @telefono,
                    vch_sucuemail = @email,
                    vch_sucuestado = @estado
                WHERE chr_sucucodigo = @codigo";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@codigo", sucursal.Codigo);
                    command.Parameters.AddWithValue("@nombre", sucursal.Nombre);
                    command.Parameters.AddWithValue("@ciudad", sucursal.Ciudad);
                    command.Parameters.AddWithValue("@direccion", sucursal.Direccion);
                    command.Parameters.AddWithValue("@contadorCuentas", sucursal.ContadorCuentas);
                    command.Parameters.AddWithValue("@latitud", sucursal.Latitud);
                    command.Parameters.AddWithValue("@longitud", sucursal.Longitud);
                    command.Parameters.AddWithValue("@telefono", sucursal.Telefono);
                    command.Parameters.AddWithValue("@email", sucursal.Email);
                    command.Parameters.AddWithValue("@estado", sucursal.Estado);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> EliminarSucursalAsync(string codigo)
        {
            string sql = @"
                UPDATE sucursal 
                SET vch_sucuestado = 'INACTIVO' 
                WHERE chr_sucucodigo = @codigo";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@codigo", codigo);
                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<DistanciaResponse?> CalcularDistanciaEntreSucursalesAsync(string codigoSucursal1, string codigoSucursal2)
        {
            var sucursal1 = await ObtenerSucursalAsync(codigoSucursal1);
            var sucursal2 = await ObtenerSucursalAsync(codigoSucursal2);

            if (sucursal1 == null || sucursal2 == null)
                return null;

            double distancia = CalcularDistanciaHaversine(
                sucursal1.Latitud, sucursal1.Longitud,
                sucursal2.Latitud, sucursal2.Longitud);

            return new DistanciaResponse
            {
                DistanciaKm = Math.Round(distancia, 2),
                SucursalOrigen = $"{sucursal1.Codigo} - {sucursal1.Nombre}",
                SucursalDestino = $"{sucursal2.Codigo} - {sucursal2.Nombre}",
                CoordenadasOrigen = new Coordenadas(sucursal1.Latitud, sucursal1.Longitud),
                CoordenadasDestino = new Coordenadas(sucursal2.Latitud, sucursal2.Longitud),
                TiempoEstimadoMinutos = Math.Round(distancia * 2, 0) // Estimación básica: 30 km/h promedio
            };
        }

        public async Task<DistanciaResponse?> CalcularDistanciaASucursalAsync(DistanciaRequest request)
        {
            var sucursal = await ObtenerSucursalAsync(request.CodigoSucursal);

            if (sucursal == null)
                return null;

            double distancia = CalcularDistanciaHaversine(
                request.LatitudOrigen, request.LongitudOrigen,
                sucursal.Latitud, sucursal.Longitud);

            return new DistanciaResponse
            {
                DistanciaKm = Math.Round(distancia, 2),
                SucursalDestino = $"{sucursal.Codigo} - {sucursal.Nombre}",
                CoordenadasOrigen = new Coordenadas(request.LatitudOrigen, request.LongitudOrigen),
                CoordenadasDestino = new Coordenadas(sucursal.Latitud, sucursal.Longitud),
                TiempoEstimadoMinutos = Math.Round(distancia * 2, 0)
            };
        }

        public async Task<SucursalConDistancia?> EncontrarSucursalMasCercanaAsync(double latitud, double longitud)
        {
            var sucursales = await ListarSucursalesAsync();
            var sucursalesActivas = sucursales.Where(s => s.Estado == "ACTIVO" && s.Latitud != 0 && s.Longitud != 0).ToList();

            if (!sucursalesActivas.Any())
                return null;

            SucursalConDistancia? sucursalMasCercana = null;
            double distanciaMinima = double.MaxValue;

            foreach (var sucursal in sucursalesActivas)
            {
                double distancia = CalcularDistanciaHaversine(latitud, longitud, sucursal.Latitud, sucursal.Longitud);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    sucursalMasCercana = new SucursalConDistancia
                    {
                        Sucursal = sucursal,
                        DistanciaKm = Math.Round(distancia, 2),
                        TiempoEstimadoMinutos = Math.Round(distancia * 2, 0)
                    };
                }
            }

            return sucursalMasCercana;
        }

        public async Task<List<SucursalConDistancia>> ObtenerSucursalesConDistanciasAsync(double latitud, double longitud, int limite = 10)
        {
            var sucursales = await ListarSucursalesAsync();
            var sucursalesActivas = sucursales.Where(s => s.Estado == "ACTIVO" && s.Latitud != 0 && s.Longitud != 0).ToList();

            var sucursalesConDistancia = sucursalesActivas.Select(sucursal =>
            {
                double distancia = CalcularDistanciaHaversine(latitud, longitud, sucursal.Latitud, sucursal.Longitud);
                return new SucursalConDistancia
                {
                    Sucursal = sucursal,
                    DistanciaKm = Math.Round(distancia, 2),
                    TiempoEstimadoMinutos = Math.Round(distancia * 2, 0)
                };
            })
            .OrderBy(s => s.DistanciaKm)
            .Take(limite)
            .ToList();

            return sucursalesConDistancia;
        }

        public async Task<List<Sucursal>> ObtenerSucursalesPorCiudadAsync(string ciudad)
        {
            var sucursales = new List<Sucursal>();
            string sql = @"
                SELECT 
                    chr_sucucodigo, vch_sucunombre, vch_sucuciudad, vch_sucudireccion,
                    int_sucucontcuenta,
                    COALESCE(dec_sucuclatitud, 0) as latitud,
                    COALESCE(dec_sucuclongitud, 0) as longitud,
                    COALESCE(vch_sucutelefono, '') as telefono,
                    COALESCE(vch_sucuemail, '') as email,
                    COALESCE(vch_sucuestado, 'ACTIVO') as estado
                FROM sucursal 
                WHERE vch_sucuciudad = @ciudad
                ORDER BY chr_sucucodigo";

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ciudad", ciudad);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sucursales.Add(new Sucursal
                            {
                                Codigo = reader["chr_sucucodigo"].ToString() ?? "",
                                Nombre = reader["vch_sucunombre"].ToString() ?? "",
                                Ciudad = reader["vch_sucuciudad"].ToString() ?? "",
                                Direccion = reader["vch_sucudireccion"].ToString() ?? "",
                                ContadorCuentas = Convert.ToInt32(reader["int_sucucontcuenta"]),
                                Latitud = Convert.ToDouble(reader["latitud"]),
                                Longitud = Convert.ToDouble(reader["longitud"]),
                                Telefono = reader["telefono"].ToString() ?? "",
                                Email = reader["email"].ToString() ?? "",
                                Estado = reader["estado"].ToString() ?? "ACTIVO"
                            });
                        }
                    }
                }
            }

            return sucursales;
        }

        /// <summary>
        /// Calcula la distancia entre dos puntos usando la fórmula de Haversine
        /// </summary>
        /// <param name="lat1">Latitud del primer punto</param>
        /// <param name="lon1">Longitud del primer punto</param>
        /// <param name="lat2">Latitud del segundo punto</param>
        /// <param name="lon2">Longitud del segundo punto</param>
        /// <returns>Distancia en kilómetros</returns>
        private static double CalcularDistanciaHaversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radio de la Tierra en kilómetros

            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }
    }
}