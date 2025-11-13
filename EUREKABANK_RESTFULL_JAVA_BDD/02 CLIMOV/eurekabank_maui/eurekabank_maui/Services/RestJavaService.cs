using Eurekabank_Maui.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Eurekabank_Maui.Services
{
    public class RestJavaService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly ServidorConfig _config;
        private readonly Uri _baseAddress;

        public RestJavaService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.RestJava);
            _baseAddress = new Uri(_config.Url);
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
                System.Diagnostics.Debug.WriteLine($"Error en HealthCheck (REST Java): {ex.Message}");
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
                    var result = await response.Content.ReadAsStringAsync();
                    return result.ToLower().Contains("true");
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Login (REST Java): {ex.Message}");
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
                    var movimientos = await response.Content.ReadFromJsonAsync<List<Movimiento>>();
                    return movimientos ?? new List<Movimiento>();
                }
                return new List<Movimiento>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerMovimientos (REST Java): {ex.Message}");
                return new List<Movimiento>();
            }
        }

        public async Task<bool> RegistrarDepositoAsync(string cuenta, double importe)
        {
            try
            {
                var url = new Uri(_baseAddress, $"/deposito?cuenta={cuenta}&importe={importe}");
                var response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarDeposito (REST Java): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarRetiroAsync(string cuenta, double importe)
        {
            try
            {
                var url = new Uri(_baseAddress, $"/retiro?cuenta={cuenta}&importe={importe}");
                var response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarRetiro (REST Java): {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var url = new Uri(_baseAddress, $"/transferencia?cuentaOrigen={cuentaOrigen}&cuentaDestino={cuentaDestino}&importe={importe}");
                var response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RegistrarTransferencia (REST Java): {ex.Message}");
                return false;
            }
        }
    }
}
