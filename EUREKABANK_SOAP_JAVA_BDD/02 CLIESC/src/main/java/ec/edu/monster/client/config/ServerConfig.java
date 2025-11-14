package ec.edu.monster.client.config;

/**
 * Configuración del servidor EUREKABANK usando variables de entorno
 * Centraliza la configuración de IPs y puertos para todos los servicios
 */
public class ServerConfig {

    // Variable de entorno para la IP del servidor
    private static final String SERVER_IP = System.getenv("EUREKABANK_SERVER_IP") != null
        ? System.getenv("EUREKABANK_SERVER_IP")
        : "localhost"; // Valor por defecto

    // URLs de los 4 servidores EUREKABANK
    public static final String REST_JAVA_BASE_URL =
        "http://" + SERVER_IP + ":8080/Eurobank_Restfull_Java/api/eureka";

    public static final String REST_DOTNET_BASE_URL =
        "http://" + SERVER_IP + ":5000/api";

    public static final String SOAP_JAVA_WSDL_URL =
        "http://" + SERVER_IP + ":8080/Eurobank_Soap_Java/EurekabankWS?wsdl";

    public static final String SOAP_JAVA_ENDPOINT_URL =
        "http://" + SERVER_IP + ":8080/Eurobank_Soap_Java/EurekabankWS";

    public static final String SOAP_DOTNET_WSDL_URL =
        "http://" + SERVER_IP + ":57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl";

    public static final String SOAP_DOTNET_ENDPOINT_URL =
        "http://" + SERVER_IP + ":57199/ec.edu.monster.ws/EurekabankWS.svc";

    public static String getServerIp() {
        return SERVER_IP;
    }

    public static void printConfiguration() {
        System.out.println("=== CONFIGURACIÓN EUREKABANK ===");
        System.out.println("IP del Servidor: " + SERVER_IP);
        System.out.println("REST Java: " + REST_JAVA_BASE_URL);
        System.out.println("REST .NET: " + REST_DOTNET_BASE_URL);
        System.out.println("SOAP Java: " + SOAP_JAVA_WSDL_URL);
        System.out.println("SOAP .NET: " + SOAP_DOTNET_WSDL_URL);
        System.out.println("===============================");
    }
}
