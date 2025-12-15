using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    public class GoogleDirectionsService
    {
        private readonly HttpClient _httpClient;
        private const string GOOGLE_API_KEY = "KEY"; // Reemplazar con tu API Key
        private const string DIRECTIONS_URL = "https://maps.googleapis.com/maps/api/directions/json";

        public GoogleDirectionsService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<RutaDetallada> ObtenerDirecciones(double origenLat, double origenLng,
                                                           double destinoLat, double destinoLng,
                                                           string modoViaje = "driving")
        {
            try
            {
                if (!IsApiKeyConfigured())
                {
                    Console.WriteLine("⚠️ Google API Key no configurada - usando cálculos básicos");
                    return CrearRutaBasica(origenLat, origenLng, destinoLat, destinoLng);
                }

                string origen = $"{origenLat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)},{origenLng.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}";
                string destino = $"{destinoLat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)},{destinoLng.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}";

                string url = $"{DIRECTIONS_URL}?origin={origen}&destination={destino}&mode={modoViaje}&language=es&key={GOOGLE_API_KEY}";

                Console.WriteLine($"🔍 Consultando Google Directions API...");
                //Console.WriteLine($"📡 URL: {url}"); // Para debug

                var response = await _httpClient.GetStringAsync(url);
                Console.WriteLine($"📦 Respuesta recibida: {response.Substring(0, Math.Min(200, response.Length))}..."); // Para debug

                var direcciones = JsonConvert.DeserializeObject<GoogleDirectionsResponse>(response);
                if (direcciones?.Status == "OK" && direcciones.Routes?.Count > 0)
                {
                    Console.WriteLine("✅ Direcciones obtenidas desde Google Maps");
                    return ProcesarRuta(direcciones.Routes[0]);
                }
                return CrearRutaBasica(origenLat, origenLng, destinoLat, destinoLng);
                //else
                //{
                //    Console.WriteLine($"⚠️ Google API Status: {direcciones?.Status ?? "UNKNOWN"}");

                //    // Mostrar mensaje específico según el error
                //    string mensajeError = direcciones?.Status switch
                //    {
                //        "REQUEST_DENIED" => "❌ API Key inválida o restricciones de acceso",
                //        "INVALID_REQUEST" => "❌ Parámetros de solicitud inválidos",
                //        "ZERO_RESULTS" => "❌ No se encontró ruta entre los puntos",
                //        "OVER_DAILY_LIMIT" => "❌ Límite diario de API excedido",
                //        "OVER_QUERY_LIMIT" => "❌ Límite de consultas por segundo excedido",
                //        "UNKNOWN_ERROR" => "❌ Error del servidor de Google",
                //        _ => $"❌ Error desconocido: {direcciones?.Status}"
                //    };

                //    Console.WriteLine(mensajeError);
                //    return CrearRutaBasica(origenLat, origenLng, destinoLat, destinoLng);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error obteniendo direcciones de Google: {ex.Message}");
                return CrearRutaBasica(origenLat, origenLng, destinoLat, destinoLng);
            }
        }

        private RutaDetallada ProcesarRuta(Route ruta)
        {
            var rutaDetallada = new RutaDetallada
            {
                DistanciaTotal = ruta.Legs[0].Distance?.Text ?? "N/A",
                TiempoTotal = ruta.Legs[0].Duration?.Text ?? "N/A",
                DireccionInicio = ruta.Legs[0].StartAddress ?? "",
                DireccionDestino = ruta.Legs[0].EndAddress ?? "",
                Pasos = new List<PasoRuta>()
            };

            int numeroPaso = 1;
            foreach (var step in ruta.Legs[0].Steps)
            {
                var paso = new PasoRuta
                {
                    Numero = numeroPaso++,
                    Instruccion = LimpiarHTML(step.HtmlInstructions ?? ""),
                    Distancia = step.Distance?.Text ?? "",
                    Tiempo = step.Duration?.Text ?? "",
                    Maniobra = ObtenerIconoManiobra(step.Maneuver)
                };

                rutaDetallada.Pasos.Add(paso);
            }

            return rutaDetallada;
        }

        private RutaDetallada CrearRutaBasica(double origenLat, double origenLng, double destinoLat, double destinoLng)
        {
            // Calcular distancia básica usando fórmula Haversine
            double distancia = CalcularDistanciaHaversine(origenLat, origenLng, destinoLat, destinoLng);

            return new RutaDetallada
            {
                DistanciaTotal = $"{distancia:F1} km",
                TiempoTotal = $"≈ {(distancia * 2):F0} min", // Estimación básica
                DireccionInicio = $"Ubicación actual ({origenLat:F4}, {origenLng:F4})",
                DireccionDestino = $"Sucursal destino ({destinoLat:F4}, {destinoLng:F4})",
                Pasos = new List<PasoRuta>
                {
                    new PasoRuta
                    {
                        Numero = 1,
                        Instruccion = "Dirígete hacia el destino usando tu aplicación de navegación preferida",
                        Distancia = $"{distancia:F1} km",
                        Tiempo = $"≈ {(distancia * 2):F0} min",
                        Maniobra = "📍"
                    }
                }
            };
        }

        private string LimpiarHTML(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText)) return "";

            // Reemplazar tags HTML comunes con texto plano
            return htmlText
                .Replace("<b>", "")
                .Replace("</b>", "")
                .Replace("<div>", "")
                .Replace("</div>", "")
                .Replace("&nbsp;", " ")
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Trim();
        }

        private string ObtenerIconoManiobra(string maniobra)
        {
            return maniobra?.ToLower() switch
            {
                "turn-left" => "⬅️",
                "turn-right" => "➡️",
                "turn-slight-left" => "↖️",
                "turn-slight-right" => "↗️",
                "turn-sharp-left" => "↩️",
                "turn-sharp-right" => "↪️",
                "uturn-left" => "🔄",
                "uturn-right" => "🔄",
                "straight" => "⬆️",
                "merge" => "🔀",
                "fork-left" => "↖️",
                "fork-right" => "↗️",
                "ferry" => "⛴️",
                "roundabout-left" => "🔄",
                "roundabout-right" => "🔄",
                _ => "📍"
            };
        }

        private double CalcularDistanciaHaversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radio de la Tierra en km

            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public static bool IsApiKeyConfigured()
        {
            return !GOOGLE_API_KEY.Equals("TU_API_KEY_AQUI");
        }
    }

    // Modelos para la respuesta de Google Directions API
    public class RutaDetallada
    {
        public string DistanciaTotal { get; set; } = "";
        public string TiempoTotal { get; set; } = "";
        public string DireccionInicio { get; set; } = "";
        public string DireccionDestino { get; set; } = "";
        public List<PasoRuta> Pasos { get; set; } = new List<PasoRuta>();
    }

    public class PasoRuta
    {
        public int Numero { get; set; }
        public string Instruccion { get; set; } = "";
        public string Distancia { get; set; } = "";
        public string Tiempo { get; set; } = "";
        public string Maniobra { get; set; } = "📍";
    }

    // Modelos para deserializar respuesta JSON de Google
    public class GoogleDirectionsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }
    }

    public class Route
    {
        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }
    }

    public class Leg
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("html_instructions")]
        public string HtmlInstructions { get; set; }

        [JsonProperty("maneuver")]
        public string Maneuver { get; set; }
    }

    public class Distance
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}