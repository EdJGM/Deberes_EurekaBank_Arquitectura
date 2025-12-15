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
        private readonly string _baseUrl;

        public RestDotNetService(HttpClient httpClient)
        {
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.RestDotNet);
            
            // Asegurar que la URL base termine con /
            _baseUrl = _config.Url.TrimEnd('/') + "/";
            
            // Crear nuevo HttpClient con BaseAddress configurado
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
            
            // Bypass SSL validation for development
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

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
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                System.Diagnostics.Debug.WriteLine($"🔍 REST .NET - Health check: {_baseUrl}Health");
                
                var response = await _httpClient.GetAsync("Health", cts.Token);
                
                System.Diagnostics.Debug.WriteLine($"✅ REST .NET Health: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Health - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                username = username.Trim();
                password = password.Trim();
                
                var loginRequest = new { username, password };
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json");

                System.Diagnostics.Debug.WriteLine($"🔐 REST .NET - Login: {username}");
                
                var response = await _httpClient.PostAsync("Auth/login", content);

                System.Diagnostics.Debug.WriteLine($"📨 REST .NET Login: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
                    System.Diagnostics.Debug.WriteLine($"✅ REST .NET - Authenticated: {result?.Data?.Authenticated}");
                    return result?.Data?.Authenticated ?? false;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Login - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📋 REST .NET - Movimientos: {cuenta}");
                
                var response = await _httpClient.GetAsync($"Movimientos/cuenta/{cuenta}");

                System.Diagnostics.Debug.WriteLine($"📨 REST .NET Movimientos: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Movimiento>>>();
                    System.Diagnostics.Debug.WriteLine($"✅ REST .NET - Movimientos: {result?.Data?.Count ?? 0}");
                    return result?.Data ?? new List<Movimiento>();
                }
                return new List<Movimiento>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Movimientos - Error: {ex.Message}");
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

                System.Diagnostics.Debug.WriteLine($"💵 REST .NET - Deposito: {cuenta}, ${importe}");
                
                var response = await _httpClient.PostAsync("Movimientos/deposito", content);
                
                System.Diagnostics.Debug.WriteLine($"📨 REST .NET Deposito: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Deposito - Error: {ex.Message}");
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

                System.Diagnostics.Debug.WriteLine($"💸 REST .NET - Retiro: {cuenta}, ${importe}");
                
                var response = await _httpClient.PostAsync("Movimientos/retiro", content);
                
                System.Diagnostics.Debug.WriteLine($"📨 REST .NET Retiro: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Retiro - Error: {ex.Message}");
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

                System.Diagnostics.Debug.WriteLine($"🔄 REST .NET - Transferencia: {cuentaOrigen} → {cuentaDestino}, ${importe}");
                
                var response = await _httpClient.PostAsync("Movimientos/transferencia", content);
                
                System.Diagnostics.Debug.WriteLine($"📨 REST .NET Transferencia: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST .NET Transferencia - Error: {ex.Message}");
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
