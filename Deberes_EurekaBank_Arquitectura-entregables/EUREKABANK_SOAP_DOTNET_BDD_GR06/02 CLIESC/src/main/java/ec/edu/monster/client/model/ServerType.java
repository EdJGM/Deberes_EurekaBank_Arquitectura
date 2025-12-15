package ec.edu.monster.client.model;

/**
 * Enumeración de los 4 tipos de servidores EUREKABANK disponibles
 * Modelo para los diferentes tipos de servidor
 */
public enum ServerType {
    REST_JAVA(1, "REST Java", "Puerto 8080 - JSON directo"),
    REST_DOTNET(2, "REST .NET", "Puerto 5000 - Respuestas estructuradas"),
    SOAP_JAVA(3, "SOAP Java", "Puerto 8080 - Métodos minúsculas"),
    SOAP_DOTNET(4, "SOAP .NET", "Puerto 57199 - Métodos mayúsculas + SOAPAction");

    private final int id;
    private final String displayName;
    private final String description;

    ServerType(int id, String displayName, String description) {
        this.id = id;
        this.displayName = displayName;
        this.description = description;
    }

    public int getId() { return id; }
    public String getDisplayName() { return displayName; }
    public String getDescription() { return description; }

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

    public static ServerType getById(int id) {
        for (ServerType type : values()) {
            if (type.getId() == id) {
                return type;
            }
        }
        throw new IllegalArgumentException("Tipo de servidor no válido: " + id);
    }
}
