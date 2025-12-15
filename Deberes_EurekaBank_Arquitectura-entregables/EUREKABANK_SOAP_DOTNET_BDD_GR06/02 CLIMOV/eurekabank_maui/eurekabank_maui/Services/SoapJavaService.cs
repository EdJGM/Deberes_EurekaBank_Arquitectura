using Eurekabank_Maui.Helpers;
using Eurekabank_Maui.Models;
using System.Xml.Linq;

namespace Eurekabank_Maui.Services
{
    public class SoapJavaService : IEurekabankService
    {
        private readonly SoapHelper _soapHelper;
        private readonly ServidorConfig _config;
        private const string SOAP_NAMESPACE = "http://ws.monster.edu.ec/";

        public SoapJavaService(HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            _soapHelper = new SoapHelper(httpClient);
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.SoapJava);
        }

        public ServidorConfig GetServidorInfo() => _config;

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                var envelope = BuildJavaSoapEnvelope("health", "");
                
                System.Diagnostics.Debug.WriteLine($"🔍 SOAP Java - Health check");
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope, cts.Token);
                
                var status = SoapHelper.ExtractValue(response, "status");
                System.Diagnostics.Debug.WriteLine($"✅ SOAP Java Health: {status}");
                
                return !string.IsNullOrEmpty(status);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Health - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                username = username.Trim();
                password = password.Trim();
                
                // Intentar primero SIN el prefijo ws: en los parámetros
                var envelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:login>
            <username>{username}</username>
            <password>{password}</password>
        </ws:login>
    </soap:Body>
</soap:Envelope>";

                System.Diagnostics.Debug.WriteLine($"🔐 SOAP Java - Login: {username}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope (sin ws: en params):");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                
                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var result = SoapHelper.ExtractValue(response, "return");
                System.Diagnostics.Debug.WriteLine($"📊 SOAP Java - Login result: {result}");
                
                return result.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Login - Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta)
        {
            try
            {
                var envelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:traerMovimientos>
            <cuenta>{cuenta}</cuenta>
        </ws:traerMovimientos>
    </soap:Body>
</soap:Envelope>";

                System.Diagnostics.Debug.WriteLine($"📋 SOAP Java - Movimientos: {cuenta}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                
                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var movimientos = ParseMovimientos(response);
                
                System.Diagnostics.Debug.WriteLine($"✅ SOAP Java - Movimientos: {movimientos.Count}");
                
                return movimientos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Movimientos - Error: {ex.Message}");
                return new List<Movimiento>();
            }
        }

        public async Task<bool> RegistrarDepositoAsync(string cuenta, double importe)
        {
            try
            {
                var envelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:regDeposito>
            <cuenta>{cuenta}</cuenta>
            <importe>{importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}</importe>
        </ws:regDeposito>
    </soap:Body>
</soap:Envelope>";

                System.Diagnostics.Debug.WriteLine($"💰 SOAP Java Depósito: {cuenta} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                
                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var estado = SoapHelper.ExtractValue(response, "estado");
                System.Diagnostics.Debug.WriteLine($"📊 SOAP Java - Estado depósito: {estado}");
                
                return estado == "1";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Depósito - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarRetiroAsync(string cuenta, double importe)
        {
            try
            {
                var envelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:regRetiro>
            <cuenta>{cuenta}</cuenta>
            <importe>{importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}</importe>
        </ws:regRetiro>
    </soap:Body>
</soap:Envelope>";

                System.Diagnostics.Debug.WriteLine($"🏧 SOAP Java Retiro: {cuenta} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                
                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var estado = SoapHelper.ExtractValue(response, "estado");
                System.Diagnostics.Debug.WriteLine($"📊 SOAP Java - Estado retiro: {estado}");
                
                return estado == "1";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Retiro - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var envelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:regTransferencia>
            <cuentaOrigen>{cuentaOrigen}</cuentaOrigen>
            <cuentaDestino>{cuentaDestino}</cuentaDestino>
            <importe>{importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}</importe>
        </ws:regTransferencia>
    </soap:Body>
</soap:Envelope>";

                System.Diagnostics.Debug.WriteLine($"🔄 SOAP Java Transferencia: {cuentaOrigen} → {cuentaDestino} - ${importe}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                
                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var estado = SoapHelper.ExtractValue(response, "estado");
                System.Diagnostics.Debug.WriteLine($"📊 SOAP Java - Estado transferencia: {estado}");
                
                return estado == "1";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP Java Transferencia - Error: {ex.Message}");
                return false;
            }
        }

        private string BuildJavaSoapEnvelope(string methodName, string parameters)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:ws=""{SOAP_NAMESPACE}"">
    <soap:Header/>
    <soap:Body>
        <ws:{methodName}>
            {parameters}
        </ws:{methodName}>
    </soap:Body>
</soap:Envelope>";
        }

        private List<Movimiento> ParseMovimientos(XDocument doc)
        {
            var movimientos = new List<Movimiento>();

            try
            {
                // Buscar elementos "movimiento" en la respuesta
                var movimientoElements = doc.Descendants()
                    .Where(e => e.Name.LocalName == "movimiento")
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"🔍 SOAP Java - Encontrados {movimientoElements.Count} elementos movimiento");

                foreach (var element in movimientoElements)
                {
                    try
                    {
                        var fechaStr = GetElementValue(element, "fecha");
                        DateTime fecha = DateTime.Now;
                        
                        if (!string.IsNullOrEmpty(fechaStr))
                        {
                            if (!DateTime.TryParse(fechaStr, out fecha))
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠️ No se pudo parsear fecha: {fechaStr}");
                            }
                        }

                        var movimiento = new Movimiento
                        {
                            Cuenta = GetElementValue(element, "cuenta"),
                            NroMov = int.Parse(GetElementValue(element, "nromov") ?? "0"),
                            Fecha = fecha,
                            Tipo = GetElementValue(element, "tipo"),
                            Accion = GetElementValue(element, "accion"),
                            Importe = double.Parse(GetElementValue(element, "importe") ?? "0", System.Globalization.CultureInfo.InvariantCulture)
                        };
                        movimientos.Add(movimiento);
                        
                        System.Diagnostics.Debug.WriteLine($"✅ Movimiento parseado: {movimiento.Tipo} - ${movimiento.Importe}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error parseando movimiento: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en ParseMovimientos: {ex.Message}");
            }

            return movimientos;
        }

        private string GetElementValue(XElement parent, string elementName)
        {
            return parent.Elements()
                .FirstOrDefault(e => e.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase))
                ?.Value ?? "";
        }
    }
}
