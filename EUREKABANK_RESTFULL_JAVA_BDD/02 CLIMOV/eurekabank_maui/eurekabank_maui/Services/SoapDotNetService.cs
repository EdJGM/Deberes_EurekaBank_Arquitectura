using Eurekabank_Maui.Helpers;
using Eurekabank_Maui.Models;
using System.Xml.Linq;

namespace Eurekabank_Maui.Services
{
    public class SoapDotNetService : IEurekabankService
    {
        private readonly SoapHelper _soapHelper;
        private readonly ServidorConfig _config;
        private const string SOAP_NAMESPACE = "http://tempuri.org/";

        public SoapDotNetService(HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            _soapHelper = new SoapHelper(httpClient);
            _config = ServidorConfig.ObtenerServidores()
                .First(s => s.Tipo == TipoServidor.SoapDotNet);
        }

        public ServidorConfig GetServidorInfo() => _config;

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var envelope = SoapHelper.BuildSoapEnvelope(SOAP_NAMESPACE, "Health");
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "http://tempuri.org/IEurekabankWS/Health", envelope, cts.Token);
                var result = SoapHelper.ExtractValue(response, "HealthResult");
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
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "Login",
                    ("username", username),
                    ("password", password)
                );

                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/Login",
                    envelope
                );

                var result = SoapHelper.ExtractValue(response, "LoginResult");
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
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "ObtenerPorCuenta",
                    ("cuenta", cuenta)
                );

                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/ObtenerPorCuenta",
                    envelope
                );

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
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "RegistrarDeposito",
                    ("cuenta", cuenta),
                    ("importe", importe.ToString())
                );

                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/RegistrarDeposito",
                    envelope
                );

                var result = SoapHelper.ExtractValue(response, "RegistrarDepositoResult");
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
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "RegistrarRetiro",
                    ("cuenta", cuenta),
                    ("importe", importe.ToString())
                );

                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/RegistrarRetiro",
                    envelope
                );

                var result = SoapHelper.ExtractValue(response, "RegistrarRetiroResult");
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
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "RegistrarTransferencia",
                    ("cuentaOrigen", cuentaOrigen),
                    ("cuentaDestino", cuentaDestino),
                    ("importe", importe.ToString())
                );

                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/RegistrarTransferencia",
                    envelope
                );

                var result = SoapHelper.ExtractValue(response, "RegistrarTransferenciaResult");
                return result == "1";
            }
            catch
            {
                return false;
            }
        }

        private List<Movimiento> ParseMovimientos(XDocument doc)
        {
            var movimientos = new List<Movimiento>();

            try
            {
                var movimientoElements = SoapHelper.ExtractElements(doc, "movimiento");

                foreach (var element in movimientoElements)
                {
                    var movimiento = new Movimiento
                    {
                        Cuenta = element.Element(XName.Get("Cuenta", element.GetDefaultNamespace().NamespaceName))?.Value ?? "",
                        NroMov = int.Parse(element.Element(XName.Get("NroMov", element.GetDefaultNamespace().NamespaceName))?.Value ?? "0"),
                        Fecha = DateTime.Parse(element.Element(XName.Get("Fecha", element.GetDefaultNamespace().NamespaceName))?.Value ?? DateTime.Now.ToString()),
                        Tipo = element.Element(XName.Get("Tipo", element.GetDefaultNamespace().NamespaceName))?.Value ?? "",
                        Accion = element.Element(XName.Get("Accion", element.GetDefaultNamespace().NamespaceName))?.Value ?? "",
                        Importe = double.Parse(element.Element(XName.Get("Importe", element.GetDefaultNamespace().NamespaceName))?.Value ?? "0")
                    };
                    movimientos.Add(movimiento);
                }
            }
            catch { }

            return movimientos;
        }
    }
}
