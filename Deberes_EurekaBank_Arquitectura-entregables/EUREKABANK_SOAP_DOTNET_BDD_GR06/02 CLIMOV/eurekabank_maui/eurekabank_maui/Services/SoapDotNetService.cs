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
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                
                var envelope = SoapHelper.BuildSoapEnvelope(SOAP_NAMESPACE, "Health");
                
                System.Diagnostics.Debug.WriteLine($"🔍 SOAP .NET - Health check");
                
                var response = await _soapHelper.CallSoapServiceAsync(_config.Url, "http://tempuri.org/IEurekabankWS/Health", envelope, cts.Token);
                
                var result = SoapHelper.ExtractValue(response, "HealthResult");
                System.Diagnostics.Debug.WriteLine($"✅ SOAP .NET Health: {result}");
                
                return !string.IsNullOrEmpty(result);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP .NET Health - Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                username = username.Trim();
                password = password.Trim();
                
                var envelope = SoapHelper.BuildSoapEnvelope(
                    SOAP_NAMESPACE,
                    "Login",
                    ("username", username),
                    ("password", password)
                );

                System.Diagnostics.Debug.WriteLine($"🔐 SOAP .NET - Login: {username}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/Login",
                    envelope
                );

                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var result = SoapHelper.ExtractValue(response, "LoginResult");
                System.Diagnostics.Debug.WriteLine($"📊 SOAP .NET - Login result: {result}");
                
                return result.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP .NET Login - Error: {ex.Message}");
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

                System.Diagnostics.Debug.WriteLine($"📋 SOAP .NET - Movimientos: {cuenta}");
                System.Diagnostics.Debug.WriteLine($"📤 Envelope:");
                System.Diagnostics.Debug.WriteLine(envelope);
                
                var response = await _soapHelper.CallSoapServiceAsync(
                    _config.Url,
                    "http://tempuri.org/IEurekabankWS/ObtenerPorCuenta",
                    envelope
                );

                System.Diagnostics.Debug.WriteLine($"📨 Response:");
                System.Diagnostics.Debug.WriteLine(response.ToString());
                
                var movimientos = ParseMovimientos(response);
                System.Diagnostics.Debug.WriteLine($"✅ SOAP .NET - Movimientos: {movimientos.Count}");
                
                return movimientos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SOAP .NET Movimientos - Error: {ex.Message}");
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
                // Buscar elementos movimiento (con 'm' minúscula) en cualquier namespace
                var movimientoElements = doc.Descendants()
                    .Where(e => e.Name.LocalName == "movimiento")
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"🔍 SOAP .NET - Encontrados {movimientoElements.Count} elementos movimiento");

                foreach (var element in movimientoElements)
                {
                    try
                    {
                        // Los elementos hijos están en el mismo namespace que el elemento padre
                        var ns = element.Name.Namespace;
                        
                        var fechaStr = element.Element(ns + "Fecha")?.Value;
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
                            Cuenta = element.Element(ns + "Cuenta")?.Value ?? "",
                            NroMov = int.Parse(element.Element(ns + "NroMov")?.Value ?? "0"),
                            Fecha = fecha,
                            Tipo = element.Element(ns + "Tipo")?.Value ?? "",
                            Accion = element.Element(ns + "Accion")?.Value ?? "",
                            Importe = double.Parse(element.Element(ns + "Importe")?.Value ?? "0", System.Globalization.CultureInfo.InvariantCulture)
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
    }
}
