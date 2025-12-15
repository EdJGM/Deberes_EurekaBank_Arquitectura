using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eurekabank_Cliente_Consola_Unificado.Models;
using Eurekabank_Cliente_Consola_Unificado.services;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    public static class GlobalConfigREST
    {
        public const string IpServidorREST = "10.40.15.218";
    }
    /// <summary>
    /// Cliente REST para servidor .NET
    /// </summary>
    public class RestDotNetService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public RestDotNetService(string baseUrl = $"http://{GlobalConfigREST.IpServidorREST}:5111/api")
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
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> Login(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest { Username = username, Password = password };
                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/Auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseContent);
                    return new OperacionResult
                    {
                        Exito = apiResponse.Success,
                        Mensaje = apiResponse.Message ?? "Login exitoso"
                    };
                }

                return new OperacionResult { Exito = false, Mensaje = "Credenciales inválidas" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerMovimientos(string cuenta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Movimientos/cuenta/{cuenta}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Movimiento>>>(content);
                    return new OperacionResult
                    {
                        Exito = true,
                        Mensaje = "Movimientos obtenidos correctamente",
                        Data = apiResponse.Data
                    };
                }

                return new OperacionResult { Exito = false, Mensaje = "No se pudieron obtener los movimientos" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarDeposito(string cuenta, double importe)
        {
            try
            {
                var depositoRequest = new DepositoRequest { Cuenta = cuenta, Importe = importe };
                var json = JsonConvert.SerializeObject(depositoRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/Movimientos/deposito", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Depósito registrado exitosamente" : "Error al registrar depósito"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarRetiro(string cuenta, double importe)
        {
            try
            {
                var retiroRequest = new RetiroRequest { Cuenta = cuenta, Importe = importe };
                var json = JsonConvert.SerializeObject(retiroRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/Movimientos/retiro", content);

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Retiro registrado exitosamente" : "Error al registrar retiro"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var transferenciaRequest = new TransferenciaRequest
                {
                    CuentaOrigen = cuentaOrigen,
                    CuentaDestino = cuentaDestino,
                    Importe = importe
                };
                var json = JsonConvert.SerializeObject(transferenciaRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/Movimientos/transferencia", content);

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Transferencia registrada exitosamente" : "Error al registrar transferencia"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }
    }

    /// <summary>
    /// Cliente REST para servidor Java
    /// </summary>
    public class RestJavaService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public RestJavaService(string baseUrl = "http://10.40.15.218:8080/Eurobank_Restfull_Java/api")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<OperacionResult> Health()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/eureka/health");
                var content = await response.Content.ReadAsStringAsync();

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = content
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> Login(string username, string password)
        {
            try
            {
                var usuario = new { username = username, password = password };
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/eureka/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerMovimientos(string cuenta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/eureka/movimientos/{cuenta}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var movimientos = JsonConvert.DeserializeObject<List<Movimiento>>(content);
                    return new OperacionResult
                    {
                        Exito = true,
                        Mensaje = "Movimientos obtenidos correctamente",
                        Data = movimientos
                    };
                }

                return new OperacionResult { Exito = false, Mensaje = "No se pudieron obtener los movimientos" };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarDeposito(string cuenta, double importe)
        {
            try
            {
                var response = await _httpClient.PostAsync(
                    $"{_baseUrl}/eureka/deposito?cuenta={cuenta}&importe={importe}",
                    null
                );

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Depósito registrado exitosamente" : "Error al registrar depósito"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarRetiro(string cuenta, double importe)
        {
            try
            {
                var response = await _httpClient.PostAsync(
                    $"{_baseUrl}/eureka/retiro?cuenta={cuenta}&importe={importe}",
                    null
                );

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Retiro registrado exitosamente" : "Error al registrar retiro"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var transferencia = new
                {
                    cuentaOrigen = cuentaOrigen,
                    cuentaDestino = cuentaDestino,
                    importe = importe
                };
                var json = JsonConvert.SerializeObject(transferencia);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/eureka/transferencia", content);

                return new OperacionResult
                {
                    Exito = response.IsSuccessStatusCode,
                    Mensaje = response.IsSuccessStatusCode ? "Transferencia registrada exitosamente" : "Error al registrar transferencia"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }
    }
}
