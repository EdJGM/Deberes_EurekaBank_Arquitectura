using System.Text;
using System.Xml.Linq;

namespace Eurekabank_Maui.Helpers
{
    /// <summary>
    /// Helper para realizar llamadas SOAP HTTP
    /// </summary>
    public class SoapHelper
    {
        private readonly HttpClient _httpClient;

        public SoapHelper(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Realiza una llamada SOAP y devuelve el XML de respuesta
        /// </summary>
        public async Task<XDocument> CallSoapServiceAsync(string url, string action, string soapEnvelope, CancellationToken cancellationToken = default)
        {
            try
            {
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                
                // Agregar headers SOAP
                content.Headers.Add("SOAPAction", action);

                var response = await _httpClient.PostAsync(url, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return XDocument.Parse(responseContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en llamada SOAP: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Construye un sobre SOAP 1.1 básico
        /// </summary>
        public static string BuildSoapEnvelope(string soapNamespace, string methodName, params (string name, string value)[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" ");
            sb.Append($"xmlns:tem=\"{soapNamespace}\">");
            sb.Append("<soap:Header/>");
            sb.Append("<soap:Body>");
            sb.Append($"<tem:{methodName}>");

            foreach (var param in parameters)
            {
                sb.Append($"<tem:{param.name}>{param.value}</tem:{param.name}>");
            }

            sb.Append($"</tem:{methodName}>");
            sb.Append("</soap:Body>");
            sb.Append("</soap:Envelope>");

            return sb.ToString();
        }

        /// <summary>
        /// Extrae un valor de un elemento del XML de respuesta SOAP
        /// </summary>
        public static string ExtractValue(XDocument doc, string elementName)
        {
            try
            {
                // Buscar en todos los namespaces
                var element = doc.Descendants()
                    .FirstOrDefault(e => e.Name.LocalName == elementName);
                
                return element?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Extrae múltiples elementos de un XML de respuesta SOAP
        /// </summary>
        public static List<XElement> ExtractElements(XDocument doc, string elementName)
        {
            try
            {
                return doc.Descendants()
                    .Where(e => e.Name.LocalName == elementName)
                    .ToList();
            }
            catch
            {
                return new List<XElement>();
            }
        }
    }
}
