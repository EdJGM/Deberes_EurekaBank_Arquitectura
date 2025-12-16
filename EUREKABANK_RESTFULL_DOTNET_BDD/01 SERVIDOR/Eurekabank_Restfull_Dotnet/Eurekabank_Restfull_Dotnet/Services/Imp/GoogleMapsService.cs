using System.Text.Json;
using Eurekabank_Restfull_Dotnet.Models;

namespace Eurekabank_Restfull_Dotnet.Services.Imp
{
    /// <summary>
    /// Servicio para integración con Google Maps API
    /// </summary>
    public class GoogleMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string GEOCODING_URL = "https://maps.googleapis.com/maps/api/geocode/json";
        private const string DIRECTIONS_URL = "https://maps.googleapis.com/maps/api/directions/json";
        private const string DISTANCE_MATRIX_URL = "https://maps.googleapis.com/maps/api/distancematrix/json";

        public GoogleMapsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string GoogleApiKey => _configuration["GoogleMaps:ApiKey"] ?? "YOUR_API_KEY_HERE";

        /// <summary>
        /// Verificar si la API Key está configurada
        /// </summary>
        public bool IsApiKeyConfigured => !string.IsNullOrEmpty(GoogleApiKey) && GoogleApiKey != "YOUR_API_KEY_HERE";

        /// <summary>
        /// Obtener coordenadas (latitud, longitud) de una dirección
        /// </summary>
        /// <param name="direccion">Dirección completa</param>
        /// <returns>Coordenadas o null si no se encuentra</returns>
        public async Task<Coordenadas?> ObtenerCoordenadasAsync(string direccion)
        {
            if (!IsApiKeyConfigured)
                return null;

            try
            {
                var url = $"{GEOCODING_URL}?address={Uri.EscapeDataString(direccion)}&key={GoogleApiKey}";
                var response = await _httpClient.GetStringAsync(url);

                using var document = JsonDocument.Parse(response);
                var root = document.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    var results = root.GetProperty("results");
                    if (results.GetArrayLength() > 0)
                    {
                        var location = results[0].GetProperty("geometry").GetProperty("location");
                        var lat = location.GetProperty("lat").GetDouble();
                        var lng = location.GetProperty("lng").GetDouble();

                        return new Coordenadas(lat, lng);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - return null for graceful degradation
                Console.WriteLine($"Error en geocoding: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Obtener dirección desde coordenadas (Reverse Geocoding)
        /// </summary>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <returns>Dirección formateada o null</returns>
        public async Task<string?> ObtenerDireccionAsync(double latitud, double longitud)
        {
            if (!IsApiKeyConfigured)
                return null;

            try
            {
                var url = $"{GEOCODING_URL}?latlng={latitud},{longitud}&key={GoogleApiKey}";
                var response = await _httpClient.GetStringAsync(url);

                using var document = JsonDocument.Parse(response);
                var root = document.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    var results = root.GetProperty("results");
                    if (results.GetArrayLength() > 0)
                    {
                        return results[0].GetProperty("formatted_address").GetString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en reverse geocoding: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Calcular distancia y tiempo usando Google Distance Matrix API
        /// </summary>
        /// <param name="origenLat">Latitud origen</param>
        /// <param name="origenLng">Longitud origen</param>
        /// <param name="destinoLat">Latitud destino</param>
        /// <param name="destinoLng">Longitud destino</param>
        /// <returns>Información de distancia y tiempo</returns>
        public async Task<GoogleDistanceResult?> CalcularDistanciaTiempoAsync(
            double origenLat, double origenLng,
            double destinoLat, double destinoLng)
        {
            if (!IsApiKeyConfigured)
                return null;

            try
            {
                var origins = $"{origenLat},{origenLng}";
                var destinations = $"{destinoLat},{destinoLng}";
                var url = $"{DISTANCE_MATRIX_URL}?origins={origins}&destinations={destinations}&units=metric&mode=driving&key={GoogleApiKey}";

                var response = await _httpClient.GetStringAsync(url);

                using var document = JsonDocument.Parse(response);
                var root = document.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    var rows = root.GetProperty("rows");
                    if (rows.GetArrayLength() > 0)
                    {
                        var elements = rows[0].GetProperty("elements");
                        if (elements.GetArrayLength() > 0)
                        {
                            var element = elements[0];
                            if (element.GetProperty("status").GetString() == "OK")
                            {
                                var distance = element.GetProperty("distance");
                                var duration = element.GetProperty("duration");

                                return new GoogleDistanceResult
                                {
                                    DistanciaMetros = distance.GetProperty("value").GetInt32(),
                                    DistanciaTexto = distance.GetProperty("text").GetString() ?? "",
                                    TiempoSegundos = duration.GetProperty("value").GetInt32(),
                                    TiempoTexto = duration.GetProperty("text").GetString() ?? ""
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en distance matrix: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Generar URL para mapa estático de Google Maps
        /// </summary>
        /// <param name="latitud">Latitud central</param>
        /// <param name="longitud">Longitud central</param>
        /// <param name="zoom">Nivel de zoom (1-20)</param>
        /// <param name="width">Ancho de la imagen</param>
        /// <param name="height">Alto de la imagen</param>
        /// <returns>URL del mapa estático</returns>
        public string GenerarURLMapaEstatico(double latitud, double longitud, int zoom = 15, int width = 400, int height = 300)
        {
            if (!IsApiKeyConfigured)
                return "";

            return $"https://maps.googleapis.com/maps/api/staticmap?" +
                   $"center={latitud},{longitud}&zoom={zoom}&size={width}x{height}" +
                   $"&markers=color:red%7C{latitud},{longitud}&key={GoogleApiKey}";
        }

        /// <summary>
        /// Generar URL para mapa estático con múltiples marcadores
        /// </summary>
        /// <param name="marcadores">Lista de coordenadas para marcadores</param>
        /// <param name="zoom">Nivel de zoom</param>
        /// <param name="width">Ancho de la imagen</param>
        /// <param name="height">Alto de la imagen</param>
        /// <returns>URL del mapa estático</returns>
        public string GenerarURLMapaConMarcadores(List<Coordenadas> marcadores, int zoom = 12, int width = 600, int height = 400)
        {
            if (!IsApiKeyConfigured || !marcadores.Any())
                return "";

            var url = $"https://maps.googleapis.com/maps/api/staticmap?size={width}x{height}&zoom={zoom}";

            // Agregar marcadores
            for (int i = 0; i < marcadores.Count && i < 26; i++) // Máximo 26 marcadores (A-Z)
            {
                var marcador = marcadores[i];
                char label = (char)('A' + i);
                url += $"&markers=color:red%7Clabel:{label}%7C{marcador.Latitud},{marcador.Longitud}";
            }

            url += $"&key={GoogleApiKey}";
            return url;
        }

        /// <summary>
        /// Generar URL de Google Maps para navegación
        /// </summary>
        /// <param name="destinoLat">Latitud de destino</param>
        /// <param name="destinoLng">Longitud de destino</param>
        /// <param name="origenLat">Latitud de origen (opcional)</param>
        /// <param name="origenLng">Longitud de origen (opcional)</param>
        /// <returns>URL de Google Maps para navegación</returns>
        public string GenerarURLNavegacion(double destinoLat, double destinoLng, double? origenLat = null, double? origenLng = null)
        {
            var url = "https://maps.google.com/maps?";

            if (origenLat.HasValue && origenLng.HasValue)
            {
                url += $"saddr={origenLat},{origenLng}&";
            }

            url += $"daddr={destinoLat},{destinoLng}";

            return url;
        }
    }

    /// <summary>
    /// Resultado de Google Distance Matrix API
    /// </summary>
    public class GoogleDistanceResult
    {
        public int DistanciaMetros { get; set; }
        public string DistanciaTexto { get; set; } = "";
        public int TiempoSegundos { get; set; }
        public string TiempoTexto { get; set; } = "";

        public double DistanciaKm => Math.Round(DistanciaMetros / 1000.0, 2);
        public double TiempoMinutos => Math.Round(TiempoSegundos / 60.0, 1);
    }
}