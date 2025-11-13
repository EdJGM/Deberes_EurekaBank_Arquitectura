package ec.edu.monster.client;

/**
 * Enumeración de los 4 tipos de servidores EUREKABANK disponibles
 */
public enum ServerType {
    REST_JAVA("REST Java", "http://localhost:8080/Eurobank_Restfull_Java/api/eureka"),
    REST_DOTNET("REST .NET", "http://localhost:5000/api"),
    SOAP_JAVA("SOAP Java", "http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl"),
    SOAP_DOTNET("SOAP .NET", "http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl");

    private final String displayName;
    private final String url;

    ServerType(String displayName, String url) {
        this.displayName = displayName;
        this.url = url;
    }

    public String getDisplayName() { return displayName; }
    public String getUrl() { return url; }

    public boolean isSOAP() {
        return this == SOAP_JAVA || this == SOAP_DOTNET;
    }

    public boolean isREST() {
        return this == REST_JAVA || this == REST_DOTNET;
    }

    public boolean isDotNet() {
        return this == REST_DOTNET || this == SOAP_DOTNET;
    }

    public boolean isJava() {
        return this == REST_JAVA || this == SOAP_JAVA;
    }
}
