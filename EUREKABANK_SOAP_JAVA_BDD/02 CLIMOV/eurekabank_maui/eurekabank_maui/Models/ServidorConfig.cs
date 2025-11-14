namespace Eurekabank_Maui.Models
{
    public enum TipoServidor
    {
        SoapDotNet,
        SoapJava,
        RestDotNet,
        RestJava
    }

    public class ServidorConfig : IEquatable<ServidorConfig>
    {
        public TipoServidor Tipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string IconoColor { get; set; }
        public string Tecnologia { get; set; }

        // Implementar IEquatable para comparación correcta en el selector
        public bool Equals(ServidorConfig? other)
        {
            if (other is null) return false;
            return Tipo == other.Tipo;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ServidorConfig);
        }

        public override int GetHashCode()
        {
            return Tipo.GetHashCode();
        }

        public static List<ServidorConfig> ObtenerServidores()
        {
            return new List<ServidorConfig>
            {
                new ServidorConfig
                {
                    Tipo = TipoServidor.SoapDotNet,
                    Nombre = "SOAP .NET",
                    Descripcion = "Windows Communication Foundation",
                    Url = "http://10.40.15.218:57199/ec.edu.monster.ws/EurekabankWS.svc",
                    IconoColor = "#512BD4",
                    Tecnologia = ".NET Framework 4.6"
                },
                new ServidorConfig
                {
                    Tipo = TipoServidor.SoapJava,
                    Nombre = "SOAP Java",
                    Descripcion = "Jakarta EE Web Services",
                    Url = "http://10.40.15.218:8080/Eurobank_Soap_Java/EurekabankWS",
                    IconoColor = "#007396",
                    Tecnologia = "JAX-WS / GlassFish"
                },
                new ServidorConfig
                {
                    Tipo = TipoServidor.RestDotNet,
                    Nombre = "REST .NET",
                    Descripcion = "ASP.NET Core Web API",
                    Url = "http://10.40.15.218:5111/api/eureka",
                    IconoColor = "#68217A",
                    Tecnologia = ".NET 8.0"
                },
                new ServidorConfig
                {
                    Tipo = TipoServidor.RestJava,
                    Nombre = "REST Java",
                    Descripcion = "Jakarta RESTful Web Services",
                    Url = "http://10.40.15.218:8080/Eurobank_Restfull_Java/api/eureka",
                    IconoColor = "#F89820",
                    Tecnologia = "JAX-RS / Jersey"
                }
            };
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TransaccionRequest
    {
        public string Cuenta { get; set; }
        public double Importe { get; set; }
        public string CuentaDestino { get; set; } // Solo para transferencias
    }
}
