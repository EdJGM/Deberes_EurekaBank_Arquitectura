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
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var envelope = SoapHelper.BuildSoapEnvelope(SOAP_NAMESPACE, "health");
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope, cts.Token);
                var result = SoapHelper.ExtractValue(response, "return");
                return !string.IsNullOrEmpty(result);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var envelope = BuildJavaSoapEnvelope(
                    "login",
                    $"<arg0>{username}</arg0><arg1>{password}</arg1>"
                );

                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                var result = SoapHelper.ExtractValue(response, "return");
                return result.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta)
        {
            try
            {
                var envelope = BuildJavaSoapEnvelope(
                    "traerMovimientos",
                    $"<arg0>{cuenta}</arg0>"
                );

                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                return ParseMovimientos(response);
            }
            catch
            {
                return new List<Movimiento>();
            }
        }

        public async Task<bool> RegistrarDepositoAsync(string cuenta, double importe)
        {
            try
            {
                var envelope = BuildJavaSoapEnvelope(
                    "regDeposito",
                    $"<arg0>{cuenta}</arg0><arg1>{importe}</arg1>"
                );

                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                var result = SoapHelper.ExtractValue(response, "return");
                return result == "1";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegistrarRetiroAsync(string cuenta, double importe)
        {
            try
            {
                var envelope = BuildJavaSoapEnvelope(
                    "regRetiro",
                    $"<arg0>{cuenta}</arg0><arg1>{importe}</arg1>"
                );

                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                var result = SoapHelper.ExtractValue(response, "return");
                return result == "1";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe)
        {
            try
            {
                var envelope = BuildJavaSoapEnvelope(
                    "regTransferencia",
                    $"<arg0>{cuentaOrigen}</arg0><arg1>{cuentaDestino}</arg1><arg2>{importe}</arg2>"
                );

                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "", envelope);
                var result = SoapHelper.ExtractValue(response, "return");
                return result == "1";
            }
            catch
            {
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
                // Buscar elementos "return" que contienen los movimientos en Java SOAP
                var returnElements = doc.Descendants()
                    .Where(e => e.Name.LocalName == "return")
                    .ToList();

                foreach (var element in returnElements)
                {
                    var movimiento = new Movimiento
                    {
                        Cuenta = GetElementValue(element, "cuenta"),
                        NroMov = int.Parse(GetElementValue(element, "nroMov") ?? "0"),
                        Fecha = DateTime.Parse(GetElementValue(element, "fecha") ?? DateTime.Now.ToString()),
                        Tipo = GetElementValue(element, "tipo"),
                        Accion = GetElementValue(element, "accion"),
                        Importe = double.Parse(GetElementValue(element, "importe") ?? "0")
                    };
                    movimientos.Add(movimiento);
                }
            }
            catch { }

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
