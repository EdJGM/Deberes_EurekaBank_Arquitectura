using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    /// <summary>
    /// Servicio de Google Directions específico para REST .NET
    /// Integra con el servicio de sucursales REST .NET
    /// </summary>
    public class GoogleDirectionsRestDotNetService
    {
        private readonly HttpClient _httpClient;
        private readonly SucursalesRestDotNetService _sucursalesService;
        private const string GOOGLE_API_KEY = "KEY"; // Reemplazar con tu API Key

        public GoogleDirectionsRestDotNetService(SucursalesRestDotNetService sucursalesService)
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _sucursalesService = sucursalesService;
        }

        /// <summary>
        /// Obtener direcciones detalladas a una sucursal usando REST .NET
        /// </summary>
        public async Task<RutaDetallada> ObtenerDireccionesASucursal(string codigoSucursal, double origenLat, double origenLng, string modoViaje = "driving")
        {
            try
            {
                // Primero obtener datos de la sucursal usando REST .NET
                var resultadoSucursal = await _sucursalesService.ObtenerSucursal(codigoSucursal);

                if (!resultadoSucursal.Exito || resultadoSucursal.Data == null)
                {
                    return CrearRutaBasica(origenLat, origenLng, 0, 0, "Sucursal no encontrada");
                }

                var sucursal = (Sucursal)resultadoSucursal.Data;

                if (sucursal.Latitud == 0 && sucursal.Longitud == 0)
                {
                    return CrearRutaBasica(origenLat, origenLng, 0, 0, $"Sucursal {sucursal.Nombre} sin coordenadas");
                }

                // Intentar obtener direcciones de Google Maps
                if (IsApiKeyConfigured())
                {
                    var rutaGoogle = await ObtenerDireccionesGoogle(origenLat, origenLng, sucursal.Latitud, sucursal.Longitud, modoViaje);
                    if (rutaGoogle != null)
                    {
                        rutaGoogle.SucursalDestino = sucursal;
                        return rutaGoogle;
                    }
                }

                // Fallback a cálculo básico
                return CrearRutaBasicaSucursal(origenLat, origenLng, sucursal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error obteniendo direcciones: {ex.Message}");
                return CrearRutaBasica(origenLat, origenLng, 0, 0, "Error al obtener direcciones");
            }
        }

        /// <summary>
        /// Obtener direcciones desde Google Maps API
        /// </summary>
        private async Task<RutaDetallada?> ObtenerDireccionesGoogle(double origenLat, double origenLng, double destinoLat, double destinoLng, string modoViaje)
        {
            try
            {
                if (!IsApiKeyConfigured())
                    return null;

                string origen = $"{origenLat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)},{origenLng.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}";
                string destino = $"{destinoLat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)},{destinoLng.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}";

                string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origen}&destination={destino}&mode={modoViaje}&language=es&key={GOOGLE_API_KEY}";

                Console.WriteLine($"🔍 Consultando Google Directions API...");
                var response = await _httpClient.GetStringAsync(url);

                var direcciones = JsonConvert.DeserializeObject<GoogleDirectionsResponse>(response);
                if (direcciones?.Status == "OK" && direcciones.Routes?.Count > 0)
                {
                    Console.WriteLine("✅ Direcciones obtenidas desde Google Maps");
                    return ProcesarRutaGoogle(direcciones.Routes[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error con Google Directions API: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Procesar respuesta de Google Directions
        /// </summary>
        private RutaDetallada ProcesarRutaGoogle(Route ruta)
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

        /// <summary>
        /// Crear ruta básica con datos de sucursal REST .NET
        /// </summary>
        private RutaDetallada CrearRutaBasicaSucursal(double origenLat, double origenLng, Sucursal sucursal)
        {
            double distancia = CalcularDistanciaHaversine(origenLat, origenLng, sucursal.Latitud, sucursal.Longitud);

            return new RutaDetallada
            {
                DistanciaTotal = $"{distancia:F1} km",
                TiempoTotal = $"≈ {(distancia * 2):F0} min",
                DireccionInicio = $"Ubicación actual ({origenLat:F4}, {origenLng:F4})",
                DireccionDestino = $"{sucursal.Nombre} - {sucursal.Direccion}",
                SucursalDestino = sucursal,
                Pasos = new List<PasoRuta>
                {
                    new PasoRuta
                    {
                        Numero = 1,
                        Instruccion = $"Dirígete hacia {sucursal.Nombre} en {sucursal.Direccion}",
                        Distancia = $"{distancia:F1} km",
                        Tiempo = $"≈ {(distancia * 2):F0} min",
                        Maniobra = "🏢"
                    },
                    new PasoRuta
                    {
                        Numero = 2,
                        Instruccion = $"📞 Teléfono: {sucursal.Telefono}",
                        Distancia = "",
                        Tiempo = "",
                        Maniobra = "📞"
                    }
                }
            };
        }

        /// <summary>
        /// Obtener las 5 sucursales más cercanas con direcciones
        /// </summary>
        public async Task<List<SucursalConRuta>> ObtenerSucursalesCercanasConRutas(double latitud, double longitud)
        {
            var resultados = new List<SucursalConRuta>();

            try
            {
                // Obtener sucursales con distancias desde REST .NET
                var resultado = await _sucursalesService.ObtenerSucursalesConDistancias(latitud, longitud, 5);

                if (resultado.Exito && resultado.Data is List<SucursalConDistancia> sucursales)
                {
                    foreach (var sucursalDist in sucursales)
                    {
                        var ruta = await ObtenerDireccionesASucursal(sucursalDist.Sucursal.Codigo, latitud, longitud);

                        resultados.Add(new SucursalConRuta
                        {
                            Sucursal = sucursalDist.Sucursal,
                            DistanciaKm = sucursalDist.DistanciaKm,
                            TiempoMinutos = sucursalDist.TiempoMinutos,
                            Ruta = ruta
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error obteniendo sucursales cercanas: {ex.Message}");
            }

            return resultados;
        }

        /// <summary>
        /// Generar URL de Google Maps para navegación a sucursal
        /// </summary>
        public async Task<string> GenerarURLNavegacionASucursal(string codigoSucursal, double? origenLat = null, double? origenLng = null)
        {
            try
            {
                var resultadoSucursal = await _sucursalesService.ObtenerSucursal(codigoSucursal);

                if (!resultadoSucursal.Exito || resultadoSucursal.Data == null)
                {
                    return "";
                }

                var sucursal = (Sucursal)resultadoSucursal.Data;

                string url = "https://maps.google.com/maps?";

                if (origenLat.HasValue && origenLng.HasValue)
                {
                    url += $"saddr={origenLat},{origenLng}&";
                }

                url += $"daddr={sucursal.Latitud},{sucursal.Longitud}";

                return url;
            }
            catch (Exception)
            {
                return "";
            }
        }

        // MÉTODOS DE UTILIDAD (mismos que el servicio SOAP)

        private RutaDetallada CrearRutaBasica(double origenLat, double origenLng, double destinoLat, double destinoLng, string mensaje = "")
        {
            double distancia = destinoLat == 0 && destinoLng == 0 ? 0 :
                CalcularDistanciaHaversine(origenLat, origenLng, destinoLat, destinoLng);

            return new RutaDetallada
            {
                DistanciaTotal = distancia > 0 ? $"{distancia:F1} km" : "N/A",
                TiempoTotal = distancia > 0 ? $"≈ {(distancia * 2):F0} min" : "N/A",
                DireccionInicio = $"Ubicación actual ({origenLat:F4}, {origenLng:F4})",
                DireccionDestino = !string.IsNullOrEmpty(mensaje) ? mensaje : "Destino desconocido",
                Pasos = new List<PasoRuta>
                {
                    new PasoRuta
                    {
                        Numero = 1,
                        Instruccion = !string.IsNullOrEmpty(mensaje) ? mensaje : "Usa tu aplicación de navegación preferida",
                        Distancia = distancia > 0 ? $"{distancia:F1} km" : "",
                        Tiempo = distancia > 0 ? $"≈ {(distancia * 2):F0} min" : "",
                        Maniobra = "📍"
                    }
                }
            };
        }

        private string LimpiarHTML(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText)) return "";

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
            const double R = 6371;

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
            return !GOOGLE_API_KEY.Equals("KEY");
        }
    }

    // MODELOS ADICIONALES PARA REST .NET

    public class SucursalConRuta
    {
        public Sucursal Sucursal { get; set; }
        public double DistanciaKm { get; set; }
        public double? TiempoMinutos { get; set; }
        public RutaDetallada Ruta { get; set; }
    }

    // Extender RutaDetallada para incluir información de sucursal
    public partial class RutaDetallada
    {
        public Sucursal SucursalDestino { get; set; }
    }
}