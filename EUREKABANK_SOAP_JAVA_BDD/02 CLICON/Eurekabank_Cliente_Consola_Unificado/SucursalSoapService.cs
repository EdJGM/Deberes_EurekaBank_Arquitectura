using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    /// <summary>
    /// Cliente SOAP para operaciones de Sucursales Java
    /// </summary>
    public class SucursalSoapService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SucursalSoapService(string baseUrl = $"http://{GlobalConfigSOAP.IpServidorSOAP}:8080/Eurobank_Soap_Java/SucursalWS")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        private async Task<string> SendSoapRequest(string soapAction, string soapEnvelope)
        {
            try
            {
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                if (!string.IsNullOrEmpty(soapAction))
                {
                    content.Headers.Add("SOAPAction", soapAction);
                }

                var response = await _httpClient.PostAsync(_baseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Status Code: {response.StatusCode}");
                    Console.WriteLine($"⚠️ Response: {responseContent}");
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en petición SOAP: {ex.Message}");
            }
        }

        public async Task<OperacionResult> Health()
        {
            try
            {
                string soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:health/>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                bool exitoso = response.Contains("<status>") || response.Contains("Servicio Sucursales SOAP activo");

                return new OperacionResult
                {
                    Exito = exitoso,
                    Mensaje = exitoso ? "Servicio Sucursales SOAP activo" : "No se pudo verificar el estado del servidor"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<SucursalOperacionResponse> ListarSucursales()
        {
            try
            {
                string soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:listarSucursales/>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                var sucursales = ParsearSucursales(response);

                return new SucursalOperacionResponse
                {
                    Exito = sucursales.Count >= 0,
                    Mensaje = sucursales.Count > 0 ? $"Se encontraron {sucursales.Count} sucursales" : "No hay sucursales registradas",
                    Sucursales = sucursales
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}",
                    Sucursales = new List<Sucursal>()
                };
            }
        }

        public async Task<SucursalOperacionResponse> ObtenerSucursal(string codigo)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:obtenerSucursal>
            <codigo>{codigo}</codigo>
        </ws:obtenerSucursal>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                var sucursal = ParsearSucursal(response);

                return new SucursalOperacionResponse
                {
                    Exito = sucursal != null,
                    Mensaje = sucursal != null ? "Sucursal encontrada" : "Sucursal no encontrada",
                    Sucursal = sucursal
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SucursalOperacionResponse> CrearSucursal(Sucursal sucursal)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:crearSucursal>
            <sucursal>
                <codigo>{sucursal.Codigo}</codigo>
                <nombre>{sucursal.Nombre}</nombre>
                <ciudad>{sucursal.Ciudad}</ciudad>
                <direccion>{sucursal.Direccion}</direccion>
                <contadorCuentas>{sucursal.ContadorCuentas}</contadorCuentas>
                <latitud>{sucursal.Latitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</latitud>
                <longitud>{sucursal.Longitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</longitud>
                <telefono>{sucursal.Telefono}</telefono>
                <email>{sucursal.Email}</email>
                <estado>{sucursal.Estado}</estado>
            </sucursal>
        </ws:crearSucursal>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                bool exito = response.Contains("<resultado>1</resultado>");

                return new SucursalOperacionResponse
                {
                    Exito = exito,
                    Mensaje = exito ? "Sucursal creada exitosamente" : "Error al crear sucursal"
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SucursalOperacionResponse> ActualizarSucursal(Sucursal sucursal)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:actualizarSucursal>
            <sucursal>
                <codigo>{sucursal.Codigo}</codigo>
                <nombre>{sucursal.Nombre}</nombre>
                <ciudad>{sucursal.Ciudad}</ciudad>
                <direccion>{sucursal.Direccion}</direccion>
                <contadorCuentas>{sucursal.ContadorCuentas}</contadorCuentas>
                <latitud>{sucursal.Latitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</latitud>
                <longitud>{sucursal.Longitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</longitud>
                <telefono>{sucursal.Telefono}</telefono>
                <email>{sucursal.Email}</email>
                <estado>{sucursal.Estado}</estado>
            </sucursal>
        </ws:actualizarSucursal>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                bool exito = response.Contains("<resultado>1</resultado>");

                return new SucursalOperacionResponse
                {
                    Exito = exito,
                    Mensaje = exito ? "Sucursal actualizada exitosamente" : "Error al actualizar sucursal"
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SucursalOperacionResponse> EliminarSucursal(string codigo)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:eliminarSucursal>
            <codigo>{codigo}</codigo>
        </ws:eliminarSucursal>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                bool exito = response.Contains("<resultado>1</resultado>");

                return new SucursalOperacionResponse
                {
                    Exito = exito,
                    Mensaje = exito ? "Sucursal eliminada exitosamente" : "Error al eliminar sucursal"
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<DistanciaResponse> CalcularDistanciaEntreSucursales(string codigoSucursal1, string codigoSucursal2)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:calcularDistanciaEntreSucursales>
            <codigoSucursal1>{codigoSucursal1}</codigoSucursal1>
            <codigoSucursal2>{codigoSucursal2}</codigoSucursal2>
        </ws:calcularDistanciaEntreSucursales>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                var doc = XDocument.Parse(response);
                XNamespace ns = "http://ws.monster.edu.ec/";
                var distanciaElement = doc.Descendants("distancia").FirstOrDefault();

                if (distanciaElement != null && double.TryParse(distanciaElement.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double distancia) && distancia >= 0)
                {
                    return new DistanciaResponse
                    {
                        Exito = true,
                        Distancia = distancia,
                        SucursalOrigen = codigoSucursal1,
                        SucursalDestino = codigoSucursal2,
                        Mensaje = $"Distancia calculada: {distancia:F2} km"
                    };
                }

                return new DistanciaResponse
                {
                    Exito = false,
                    Mensaje = "No se pudo calcular la distancia"
                };
            }
            catch (Exception ex)
            {
                return new DistanciaResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<DistanciaResponse> CalcularDistanciaASucursal(string codigoSucursal, double latitud, double longitud)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:calcularDistanciaASucursal>
            <codigoSucursal>{codigoSucursal}</codigoSucursal>
            <latitud>{latitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</latitud>
            <longitud>{longitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</longitud>
        </ws:calcularDistanciaASucursal>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                var doc = XDocument.Parse(response);
                var distanciaElement = doc.Descendants("distancia").FirstOrDefault();

                if (distanciaElement != null && double.TryParse(distanciaElement.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double distancia) && distancia >= 0)
                {
                    return new DistanciaResponse
                    {
                        Exito = true,
                        Distancia = distancia,
                        SucursalDestino = codigoSucursal,
                        Mensaje = $"Distancia a sucursal: {distancia:F2} km"
                    };
                }

                return new DistanciaResponse
                {
                    Exito = false,
                    Mensaje = "No se pudo calcular la distancia"
                };
            }
            catch (Exception ex)
            {
                return new DistanciaResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SucursalOperacionResponse> EncontrarSucursalMasCercana(double latitud, double longitud)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:encontrarSucursalMasCercana>
            <latitud>{latitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</latitud>
            <longitud>{longitud.ToString("F8", System.Globalization.CultureInfo.InvariantCulture)}</longitud>
        </ws:encontrarSucursalMasCercana>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);
                var sucursal = ParsearSucursal(response);

                return new SucursalOperacionResponse
                {
                    Exito = sucursal != null,
                    Mensaje = sucursal != null ? "Sucursal más cercana encontrada" : "No se encontró sucursal cercana",
                    Sucursal = sucursal
                };
            }
            catch (Exception ex)
            {
                return new SucursalOperacionResponse
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        private List<Sucursal> ParsearSucursales(string xmlResponse)
        {
            var sucursales = new List<Sucursal>();

            try
            {
                var doc = XDocument.Parse(xmlResponse);
                XNamespace ns = "http://ws.monster.edu.ec/";

                var sucursalesElements = doc.Descendants("sucursales");

                foreach (var suc in sucursalesElements)
                {
                    var sucursal = new Sucursal
                    {
                        Codigo = suc.Element("codigo")?.Value ?? "",
                        Nombre = suc.Element("nombre")?.Value ?? "",
                        Ciudad = suc.Element("ciudad")?.Value ?? "",
                        Direccion = suc.Element("direccion")?.Value ?? "",
                        ContadorCuentas = int.Parse(suc.Element("contadorCuentas")?.Value ?? "0"),
                        Latitud = double.Parse(suc.Element("latitud")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture),
                        Longitud = double.Parse(suc.Element("longitud")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture),
                        Telefono = suc.Element("telefono")?.Value ?? "",
                        Email = suc.Element("email")?.Value ?? "",
                        Estado = suc.Element("estado")?.Value ?? "ACTIVO"
                    };

                    sucursales.Add(sucursal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parseando sucursales: {ex.Message}");
            }

            return sucursales;
        }

        private Sucursal ParsearSucursal(string xmlResponse)
        {
            try
            {
                var doc = XDocument.Parse(xmlResponse);
                var sucElement = doc.Descendants("sucursal").FirstOrDefault();

                if (sucElement != null)
                {
                    return new Sucursal
                    {
                        Codigo = sucElement.Element("codigo")?.Value ?? "",
                        Nombre = sucElement.Element("nombre")?.Value ?? "",
                        Ciudad = sucElement.Element("ciudad")?.Value ?? "",
                        Direccion = sucElement.Element("direccion")?.Value ?? "",
                        ContadorCuentas = int.Parse(sucElement.Element("contadorCuentas")?.Value ?? "0"),
                        Latitud = double.Parse(sucElement.Element("latitud")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture),
                        Longitud = double.Parse(sucElement.Element("longitud")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture),
                        Telefono = sucElement.Element("telefono")?.Value ?? "",
                        Email = sucElement.Element("email")?.Value ?? "",
                        Estado = sucElement.Element("estado")?.Value ?? "ACTIVO"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parseando sucursal: {ex.Message}");
            }

            return null;
        }
    }
}