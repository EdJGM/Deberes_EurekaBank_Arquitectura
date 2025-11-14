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
        private readonly string _baseUrl;
        private string? _sessionId;

        public RestJavaService(HttpClient httpClient)
        {
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.RestJava);
            
            // ASEGURAR que la URL termina con /
            _baseUrl = _config.Url.TrimEnd('/') + "/";
            
            // CREAR UN NUEVO HttpClient
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl), // AHORA SÍ termina con /
                Timeout = TimeSpan.FromSeconds(30)
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            System.Diagnostics.Debug.WriteLine($"🔧 RestJavaService inicializado: {_baseUrl}");
            System.Diagnostics.Debug.WriteLine($"🔧 BaseAddress configurada: {_httpClient.BaseAddress}");
        }

        public ServidorConfig GetServidorInfo() => _config;

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var response = await _httpClient.GetAsync("health", cts.Token);
                
                System.Diagnostics.Debug.WriteLine($"📊 REST Java Health - Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"✅ REST Java Health: {content}");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ REST Java Health - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                // Limpiar espacios en blanco
                username = username.Trim();
                password = password.Trim();
                
                var loginRequest = new { username, password };
                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine($"🔐 REST Java - Login con: {username}");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var response = await _httpClient.PostAsync("login", content, cts.Token);

                System.Diagnostics.Debug.WriteLine($"📊 Login Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"✅ REST Java - Login Response: {result}");
                    
                    try
                    {
                        var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);
                        
                        if (jsonResponse != null)
                        {
                            bool success = false;
                            
                            if (jsonResponse.ContainsKey("success"))
                            {
                                if (jsonResponse["success"].ValueKind == JsonValueKind.True ||
                                    jsonResponse["success"].ValueKind == JsonValueKind.False)
                                {
                                    success = jsonResponse["success"].GetBoolean();
                                }
                                else if (jsonResponse["success"].ValueKind == JsonValueKind.String)
                                {
                                    success = jsonResponse["success"].GetString()?.ToLower() == "true";
                                }
                            }
                            
                            if (success)
                            {
                                // Guardar sessionId si existe
                                if (jsonResponse.ContainsKey("sessionId"))
                                {
                                    _sessionId = jsonResponse["sessionId"].GetString();
                                    System.Diagnostics.Debug.WriteLine($"🔑 SessionId guardado: {_sessionId}");
                                }
                                else if (jsonResponse.ContainsKey("token"))
                                {
                                    _sessionId = jsonResponse["token"].GetString();
                                    System.Diagnostics.Debug.WriteLine($"🔑 Token guardado: {_sessionId}");
                                }
                            }
                            
                            return success;
                        }
                    }
                    catch (JsonException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ Error parseando JSON: {ex.Message}");
                        // Si no es JSON, verificar si la respuesta contiene "true"
                        return result.ToLower().Contains("true");
                    }
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"❌ REST Java - Login Error: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Login Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta)
        {
            try
            {
                var url = $"movimientos/{cuenta}";
                if (!string.IsNullOrEmpty(_sessionId))
                    url += $"?sessionId={_sessionId}";
                
                System.Diagnostics.Debug.WriteLine($"📋 Obteniendo movimientos de: {cuenta}");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var response = await _httpClient.GetAsync(url, cts.Token);

                System.Diagnostics.Debug.WriteLine($"📊 URL: {response.RequestMessage?.RequestUri}");
                System.Diagnostics.Debug.WriteLine($"📊 Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"📨 JSON: {jsonContent.Substring(0, Math.Min(200, jsonContent.Length))}...");
                    
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new FlexibleDateTimeConverter() }
                    };
                    
                    var movimientos = JsonSerializer.Deserialize<List<Movimiento>>(jsonContent, options);
                    System.Diagnostics.Debug.WriteLine($"✅ Movimientos: {movimientos?.Count ?? 0}");
                    return movimientos ?? new List<Movimiento>();
                }
                
                System.Diagnostics.Debug.WriteLine($"❌ Error: {response.StatusCode}");
                return new List<Movimiento>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return new List<Movimiento>();
            }
        }

        public async Task<bool> RegistrarDepositoAsync(string cuenta, double importe)
        {
            try
            {
                // Usar query params en lugar de JSON body
                var url = $"deposito?cuenta={cuenta}&importe={importe}";
                
                System.Diagnostics.Debug.WriteLine($"💰 Depósito: {cuenta} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 URL: {url}");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                // Enviar con body vacío
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, emptyContent, cts.Token);
                
                System.Diagnostics.Debug.WriteLine($"📊 URL: {response.RequestMessage?.RequestUri}");
                System.Diagnostics.Debug.WriteLine($"📊 Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"✅ Respuesta: {result}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Error: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarRetiroAsync(string cuenta, double importe)
        {
            try
            {
                // Usar query params en lugar de JSON body
                var url = $"retiro?cuenta={cuenta}&importe={importe}";
                
                System.Diagnostics.Debug.WriteLine($"🏧 Retiro: {cuenta} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 URL: {url}");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                // Enviar con body vacío
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, emptyContent, cts.Token);
                
                System.Diagnostics.Debug.WriteLine($"📊 URL: {response.RequestMessage?.RequestUri}");
                System.Diagnostics.Debug.WriteLine($"📊 Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"✅ Respuesta: {result}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Error: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                // Usar query params en lugar de JSON body
                var url = $"transferencia?cuentaOrigen={cuentaOrigen}&cuentaDestino={cuentaDestino}&importe={importe}";
                
                System.Diagnostics.Debug.WriteLine($"🔄 Transferencia: {cuentaOrigen} → {cuentaDestino} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 URL: {url}");
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                // Enviar con body vacío
                var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, emptyContent, cts.Token);
                
                System.Diagnostics.Debug.WriteLine($"📊 URL: {response.RequestMessage?.RequestUri}");
                System.Diagnostics.Debug.WriteLine($"📊 Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"✅ Respuesta: {result}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ Error: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
                return false;
            }
        }
    }

    // Convertidor flexible para DateTime que maneja múltiples formatos
    public class FlexibleDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        private readonly string[] _formats = new[]
        {
            "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",   // Formato Java completo
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.ffZ",
            "yyyy-MM-ddTHH:mm:ss.fZ",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd",
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateString = reader.GetString();
            
            if (string.IsNullOrWhiteSpace(dateString))
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Fecha vacía");
                return DateTime.MinValue;
            }

            System.Diagnostics.Debug.WriteLine($"🔍 Parseando: '{dateString}'");

            // Intentar parseo automático primero (más rápido)
            if (DateTime.TryParse(dateString, System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.RoundtripKind, out DateTime autoResult))
            {
                System.Diagnostics.Debug.WriteLine($"✅ Parseado automáticamente: {autoResult}");
                return autoResult;
            }

            // Intentar con formatos específicos
            foreach (var format in _formats)
            {
                if (DateTime.TryParseExact(dateString, format, 
                    System.Globalization.CultureInfo.InvariantCulture, 
                    System.Globalization.DateTimeStyles.RoundtripKind, 
                    out DateTime result))
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Parseado con '{format}': {result}");
                    return result;
                }
            }

            // Intentar timestamp Java
            if (long.TryParse(dateString, out long timestamp))
            {
                var result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp);
                System.Diagnostics.Debug.WriteLine($"✅ Parseado timestamp: {result}");
                return result;
            }

            System.Diagnostics.Debug.WriteLine($"❌ No se pudo parsear: '{dateString}'");
            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
    }
}
