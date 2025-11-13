using Eurekabank_Maui.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Eurekabank_Maui.Services
{
    public class RestDotNetService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly ServidorConfig _config;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Uri _baseAddress;

        public RestDotNetService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.RestDotNet);
            _baseAddress = new Uri(_config.Url);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public ServidorConfig GetServidorInfo() => _config;

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var url = new Uri(_baseAddress, "/health");
                var response = await _httpClient.GetAsync(url, cts.Token);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en HealthCheck (REST .NET): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new { username, password };
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json");

                var url = new Uri(_baseAddress, "/login");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                    return result?.Data?.Authenticated ?? false;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Login (REST .NET): {ex.Message}");
                return false;
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta)
        {
            try
            {
                var url = new Uri(_baseAddress, $"/movimientos/{cuenta}");
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Movimiento>>>();
                    return result?.Data ?? new List<Movimiento>();
                }
                return new List<Movimiento>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerMovimientos (REST .NET): {ex.Message}");
                return new List<Movimiento>();
            }
        }

        public async Task<bool> RegistrarDepositoAsync(string cuenta, double importe)
        {
            try
            {
                var request = new { cuenta, importe };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var url = new Uri(_baseAddress, "/deposito");
                var response = await _httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarDeposito (REST .NET): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarRetiroAsync(string cuenta, double importe)
        {
            try
            {
                var request = new { cuenta, importe };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var url = new Uri(_baseAddress, "/retiro");
                var response = await _httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarRetiro (REST .NET): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var request = new { cuentaOrigen, cuentaDestino, importe };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var url = new Uri(_baseAddress, "/transferencia");
                var response = await _httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarTransferencia (REST .NET): {ex.Message}");
                return false;
            }
        }

        // Clases de respuesta para deserialización
        private class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        private class LoginResponse
        {
            public bool Authenticated { get; set; }
            public string Username { get; set; }
        }
    }
}
