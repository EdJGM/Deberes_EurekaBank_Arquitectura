using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.services
{
    public static class GlobalConfigSOAP
    {
        public const string IpServidorSOAP = "10.40.15.218";
    }
    /// <summary>
    /// Cliente SOAP para servidor .NET
    /// </summary>
    public class SoapDotNetService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SoapDotNetService(string baseUrl = $"http://{GlobalConfigSOAP.IpServidorSOAP}:57199/ec.edu.monster.ws/EurekabankWS.svc")
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
                content.Headers.Add("SOAPAction", $"http://tempuri.org/IEurekabankWS/{soapAction}");

                var response = await _httpClient.PostAsync(_baseUrl, content);
                return await response.Content.ReadAsStringAsync();
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
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:Health/>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("Health", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var result = doc.Descendants(ns + "HealthResult").FirstOrDefault()?.Value ?? "";

                return new OperacionResult
                {
                    Exito = !string.IsNullOrEmpty(result),
                    Mensaje = result
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
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:Login>
            <tem:username>{username}</tem:username>
            <tem:password>{password}</tem:password>
        </tem:Login>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("Login", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var result = doc.Descendants(ns + "LoginResult").FirstOrDefault()?.Value ?? "false";
                bool loginExitoso = result.Equals("true", StringComparison.OrdinalIgnoreCase);

                return new OperacionResult
                {
                    Exito = loginExitoso,
                    Mensaje = loginExitoso ? "Login exitoso" : "Credenciales inválidas"
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
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:ObtenerPorCuenta>
            <tem:cuenta>{cuenta}</tem:cuenta>
        </tem:ObtenerPorCuenta>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("ObtenerPorCuenta", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var nsModel = XNamespace.Get("http://schemas.datacontract.org/2004/07/Eurekabank_Soap_Dotnet.ec.edu.monster.modelo");

                var movimientos = new List<Movimiento>();
                var resultNode = doc.Descendants(ns + "ObtenerPorCuentaResult").FirstOrDefault();
                if (resultNode != null)
                {
                    var movimientosNodes = resultNode.Elements(nsModel + "movimiento");
                    foreach (var mov in movimientosNodes)
                    {
                        movimientos.Add(new Movimiento
                        {
                            Cuenta = mov.Element(nsModel + "Cuenta")?.Value ?? "",
                            NroMov = int.Parse(mov.Element(nsModel + "NroMov")?.Value ?? "0"),
                            Fecha = DateTime.Parse(mov.Element(nsModel + "Fecha")?.Value ?? DateTime.Now.ToString()),
                            Tipo = mov.Element(nsModel + "Tipo")?.Value ?? "",
                            Accion = mov.Element(nsModel + "Accion")?.Value ?? "",
                            Importe = double.Parse(mov.Element(nsModel + "Importe")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture)
                        });
                    }
                }

                return new OperacionResult
                {
                    Exito = true,
                    Mensaje = movimientos.Count > 0
                        ? $"Se encontraron {movimientos.Count} movimientos"
                        : "No se encontraron movimientos para esta cuenta",
                    Data = movimientos
                };
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
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:RegistrarDeposito>
            <tem:cuenta>{cuenta}</tem:cuenta>
            <tem:importe>{importe}</tem:importe>
        </tem:RegistrarDeposito>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("RegistrarDeposito", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var result = doc.Descendants(ns + "RegistrarDepositoResult").FirstOrDefault()?.Value ?? "0";
                bool exito = result == "1";

                return new OperacionResult
                {
                    Exito = exito,
                    Mensaje = exito ? "Depósito registrado exitosamente" : "Error al registrar depósito"
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
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:RegistrarRetiro>
            <tem:cuenta>{cuenta}</tem:cuenta>
            <tem:importe>{importe}</tem:importe>
        </tem:RegistrarRetiro>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("RegistrarRetiro", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var result = doc.Descendants(ns + "RegistrarRetiroResult").FirstOrDefault()?.Value ?? "0";
                bool exito = result == "1";

                return new OperacionResult
                {
                    Exito = exito,
                    Mensaje = exito ? "Retiro registrado exitosamente" : "Error al registrar retiro"
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
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:RegistrarTransferencia>
            <tem:cuentaOrigen>{cuentaOrigen}</tem:cuentaOrigen>
            <tem:cuentaDestino>{cuentaDestino}</tem:cuentaDestino>
            <tem:importe>{importe}</tem:importe>
        </tem:RegistrarTransferencia>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("RegistrarTransferencia", soapEnvelope);

                var doc = XDocument.Parse(response);
                var ns = XNamespace.Get("http://tempuri.org/");
                var result = doc.Descendants(ns + "RegistrarTransferenciaResult").FirstOrDefault()?.Value ?? "0";
                bool exito = result == "1";

                return new OperacionResult
                {
                    Exito = exito,
                    Mensaje = exito ? "Transferencia registrada exitosamente" : "Error al registrar transferencia"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }
    }

    /// <summary>
    /// Cliente SOAP para servidor Java
    /// </summary>
    public class SoapJavaService : IEurekabankService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SoapJavaService(string baseUrl = "http://10.40.15.218:8080/Eurobank_Soap_Java/EurekabankWS")
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
                // Según XSD: <xs:complexType name="health"> con <xs:sequence/> vacía
                // Respuesta: <xs:element name="status" type="xs:string"/>
                string soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:health/>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                // Buscar el elemento <status> en la respuesta
                bool exitoso = response.Contains("<status>") || response.Contains("healthResponse");

                return new OperacionResult
                {
                    Exito = exitoso,
                    Mensaje = exitoso ? "Servidor SOAP Java activo" : "No se pudo verificar el estado del servidor"
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
                // Según XSD Schema: <xs:element name="username"/> y <xs:element name="password"/>
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:login>
            <username>{username}</username>
            <password>{password}</password>
        </ws:login>
    </soap:Body>
</soap:Envelope>";



                var response = await SendSoapRequest("", soapEnvelope);
                bool loginExitoso = response.Contains("<return>true</return>");

                return new OperacionResult
                {
                    Exito = loginExitoso,
                    Mensaje = loginExitoso ? "Login exitoso" : "Credenciales inválidas"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error en Login: {ex.Message}");
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperacionResult> ObtenerMovimientos(string cuenta)
        {
            try
            {
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:traerMovimientos>
            <cuenta>{cuenta}</cuenta>
        </ws:traerMovimientos>
    </soap:Body>
</soap:Envelope>";



                var response = await SendSoapRequest("", soapEnvelope);
                var movimientos = ParsearMovimientos(response);

                return new OperacionResult
                {
                    Exito = movimientos.Count > 0,
                    Mensaje = movimientos.Count > 0
                        ? $"Se encontraron {movimientos.Count} movimientos"
                        : "No se encontraron movimientos para esta cuenta",
                    Data = movimientos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error obteniendo movimientos: {ex.Message}");
                return new OperacionResult
                {
                    Exito = false,
                    Mensaje = $"Error: {ex.Message}",
                    Data = new List<Movimiento>()
                };
            }
        }

        public async Task<OperacionResult> RegistrarDeposito(string cuenta, double importe)
        {
            try
            {
                // Según XSD: <xs:element name="cuenta"/> y <xs:element name="importe"/>
                // Respuesta: <xs:element name="estado" type="xs:int"/>
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:regDeposito>
            <cuenta>{cuenta}</cuenta>
            <importe>{importe}</importe>
        </ws:regDeposito>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                return new OperacionResult
                {
                    Exito = response.Contains("<estado>1</estado>"),
                    Mensaje = "Depósito registrado exitosamente"
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
                // Según XSD: <xs:element name="cuenta"/> y <xs:element name="importe"/>
                // Respuesta: <xs:element name="estado" type="xs:int"/>
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:regRetiro>
            <cuenta>{cuenta}</cuenta>
            <importe>{importe}</importe>
        </ws:regRetiro>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                return new OperacionResult
                {
                    Exito = response.Contains("<estado>1</estado>"),
                    Mensaje = "Retiro registrado exitosamente"
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
                // Según XSD: <xs:element name="cuentaOrigen"/>, <xs:element name="cuentaDestino"/> y <xs:element name="importe"/>
                // Respuesta: <xs:element name="estado" type="xs:int"/>
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
    <soap:Body>
        <ws:regTransferencia>
            <cuentaOrigen>{cuentaOrigen}</cuentaOrigen>
            <cuentaDestino>{cuentaDestino}</cuentaDestino>
            <importe>{importe}</importe>
        </ws:regTransferencia>
    </soap:Body>
</soap:Envelope>";

                var response = await SendSoapRequest("", soapEnvelope);

                return new OperacionResult
                {
                    Exito = response.Contains("<estado>1</estado>"),
                    Mensaje = "Transferencia registrada exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new OperacionResult { Exito = false, Mensaje = $"Error: {ex.Message}" };
            }
        }

        private List<Movimiento> ParsearMovimientos(string xmlResponse)
        {
            var movimientos = new List<Movimiento>();

            try
            {

                var doc = XDocument.Parse(xmlResponse);

                // Definir el namespace correcto
                XNamespace ns2 = "http://ws.monster.edu.ec/";

                // Los elementos <movimiento> están dentro de <ns2:traerMovimientosResponse> 
                // pero SIN prefijo de namespace (están en el namespace por herencia)
                var movimientosElements = doc.Descendants(ns2 + "movimiento");

                // Si no encuentra con namespace, intentar sin namespace
                if (!movimientosElements.Any())
                {
                    //Console.WriteLine("⚠️ No se encontraron con namespace, intentando sin namespace...");
                    movimientosElements = doc.Descendants("movimiento");
                }

                foreach (var mov in movimientosElements)
                {
                    try
                    {
                        // Los elementos hijos tampoco tienen prefijo, así que buscamos sin namespace
                        var movimiento = new Movimiento
                        {
                            Accion = mov.Element("accion")?.Value ?? "",
                            Cuenta = mov.Element("cuenta")?.Value ?? "",
                            Fecha = DateTime.Parse(mov.Element("fecha")?.Value ?? DateTime.Now.ToString()),
                            Importe = double.Parse(mov.Element("importe")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture),
                            NroMov = int.Parse(mov.Element("nromov")?.Value ?? "0"),
                            Tipo = mov.Element("tipo")?.Value ?? ""
                        };

                        movimientos.Add(movimiento);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error parseando movimiento individual: {ex.Message}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en alalisis de movimientos: {ex.Message}");
            }

            return movimientos;
        }
    }
}
