using Eurekabank_Maui.Helpers;
using Eurekabank_Maui.Models;

namespace Eurekabank_Maui.Services
{
    /// <summary>
    /// Factory para crear instancias del servicio apropiado según el tipo de servidor
    /// </summary>
    public static class EurekabankServiceFactory
    {
        /// <summary>
        /// Crea una instancia del servicio según el tipo de servidor especificado
        /// </summary>
        public static IEurekabankService Create(TipoServidor tipo, HttpClient httpClient)
        {
            return tipo switch
            {
                TipoServidor.SoapDotNet => new SoapDotNetService(httpClient),
                TipoServidor.SoapJava => new SoapJavaService(httpClient),
                TipoServidor.RestDotNet => new RestDotNetService(httpClient),
                TipoServidor.RestJava => new RestJavaService(httpClient),
                _ => throw new ArgumentException($"Tipo de servidor no soportado: {tipo}")
            };
        }

        /// <summary>
        /// Crea una instancia del servicio desde una configuración
        /// </summary>
        public static IEurekabankService Create(ServidorConfig config, HttpClient httpClient)
        {
            return Create(config.Tipo, httpClient);
        }
    }
}
