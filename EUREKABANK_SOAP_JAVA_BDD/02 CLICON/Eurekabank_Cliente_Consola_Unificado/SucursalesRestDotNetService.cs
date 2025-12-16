using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    /// <summary>
    /// Cliente REST .NET para operaciones de Sucursales
    /// Se integra con el sistema existente de selección de servidores
    /// </summary>
    public class SucursalesRestDotNetService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SucursalesRestDotNetService(string baseUrl = $"http://{GlobalConfigREST.IpServidorREST}:5111/api/sucursales")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<OperacionResult> Health()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/health");
                var content = await response.Content.ReadAsStringAsync();

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Servicio REST .NET Sucursales activo" : "Servicio no disponible"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ListarSucursales()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sucursal>>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = apiResponse.Message ?? $"Se encontraron {apiResponse.Data.Count} sucursales",
                            Data = apiResponse.Data
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "No se pudieron obtener las sucursales" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerSucursal(string codigo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{codigo}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Sucursal>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = "Sucursal encontrada",
                            Data = apiResponse.Data
                        };
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new OperacionResult { Exito = false, Mensaje = $"Sucursal con código {codigo} no encontrada" };
                }

                return new OperacionResult { Exito = false, Mensaje = "Error al obtener sucursal" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> CrearSucursal(SucursalRequest sucursal)
        {
            try
            {
                var json = JsonConvert.SerializeObject(sucursal);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Sucursal>>(responseContent);

                    if (apiResponse?.Success == true)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = "Sucursal creada exitosamente",
                            Data = apiResponse.Data
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "Error al crear sucursal" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ActualizarSucursal(string codigo, SucursalRequest sucursal)
        {
            try
            {
                var json = JsonConvert.SerializeObject(sucursal);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_baseUrl}/{codigo}", content);

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Sucursal actualizada exitosamente" : "Error al actualizar sucursal"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> EliminarSucursal(string codigo)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{codigo}");

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Sucursal eliminada exitosamente" : "Error al eliminar sucursal"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> CalcularDistanciaEntreSucursales(string codigoOrigen, string codigoDestino)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/distancia/{codigoOrigen}/{codigoDestino}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DistanciaResponseRest>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = $"Distancia: {apiResponse.Data.DistanciaKm:F2} km",
                            Data = new DistanciaResponse
                            {
                                Distancia = apiResponse.Data.DistanciaKm,
                                SucursalOrigen = apiResponse.Data.SucursalOrigen,
                                SucursalDestino = apiResponse.Data.SucursalDestino
                            }
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "No se pudo calcular la distancia" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> CalcularDistanciaASucursal(string codigoSucursal, double latitud, double longitud)
        {
            try
            {
                var request = new
                {
                    latitudOrigen = latitud,
                    longitudOrigen = longitud,
                    codigoSucursal = codigoSucursal
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/distancia-a-sucursal", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<DistanciaResponseRest>>(responseContent);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = $"Distancia: {apiResponse.Data.DistanciaKm:F2} km",
                            Data = new DistanciaResponse
                            {
                                Distancia = apiResponse.Data.DistanciaKm,
                                SucursalDestino = apiResponse.Data.SucursalDestino
                            }
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "No se pudo calcular la distancia" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> EncontrarSucursalMasCercana(double latitud, double longitud)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/mas-cercana?latitud={latitud}&longitud={longitud}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<SucursalConDistanciaRest>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = $"Sucursal más cercana: {apiResponse.Data.Sucursal.Nombre} - {apiResponse.Data.DistanciaKm:F2} km",
                            Data = new SucursalConDistancia
                            {
                                Sucursal = apiResponse.Data.Sucursal,
                                DistanciaKm = apiResponse.Data.DistanciaKm,
                                TiempoMinutos = apiResponse.Data.TiempoEstimadoMinutos
                            }
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "No se encontró sucursal cercana" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerSucursalesConDistancias(double latitud, double longitud, int limite = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/con-distancias?latitud={latitud}&longitud={longitud}&limite={limite}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<SucursalConDistanciaRest>>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        var sucursales = apiResponse.Data.ConvertAll(s => new SucursalConDistancia
                        {
                            Sucursal = s.Sucursal,
                            DistanciaKm = s.DistanciaKm,
                            TiempoMinutos = s.TiempoEstimadoMinutos
                        });

                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = $"Se encontraron {sucursales.Count} sucursales",
                            Data = sucursales
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = "No se encontraron sucursales" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerSucursalesPorCiudad(string ciudad)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/ciudad/{ciudad}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sucursal>>>(content);

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        return new OperacionResult
                        {
                            Exito = true,
                            Mensaje = $"Se encontraron {apiResponse.Data.Count} sucursales en {ciudad}",
                            Data = apiResponse.Data
                        };
                    }
                }

                return new OperacionResult { Exito = false, Mensaje = $"No se encontraron sucursales en {ciudad}" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }
    }

    // ============= MODELOS PARA REST .NET =============

    public class SucursalRequest
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("ciudad")]
        public string Ciudad { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("contadorCuentas")]
        public int ContadorCuentas { get; set; }

        [JsonProperty("latitud")]
        public double Latitud { get; set; }

        [JsonProperty("longitud")]
        public double Longitud { get; set; }

        [JsonProperty("telefono")]
        public string Telefono { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; } = "ACTIVO";
    }

    public class DistanciaResponseRest
    {
        [JsonProperty("distanciaKm")]
        public double DistanciaKm { get; set; }

        [JsonProperty("sucursalOrigen")]
        public string SucursalOrigen { get; set; }

        [JsonProperty("sucursalDestino")]
        public string SucursalDestino { get; set; }

        [JsonProperty("coordenadasOrigen")]
        public Coordenadas CoordenadasOrigen { get; set; }

        [JsonProperty("coordenadasDestino")]
        public Coordenadas CoordenadasDestino { get; set; }

        [JsonProperty("tiempoEstimadoMinutos")]
        public double? TiempoEstimadoMinutos { get; set; }
    }

    public class SucursalConDistanciaRest
    {
        [JsonProperty("sucursal")]
        public Sucursal Sucursal { get; set; }

        [JsonProperty("distanciaKm")]
        public double DistanciaKm { get; set; }

        [JsonProperty("tiempoEstimadoMinutos")]
        public double? TiempoEstimadoMinutos { get; set; }
    }
}