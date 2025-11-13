package ec.edu.monster.client.service;

/**
 * Clase para encapsular las respuestas de los servicios EUREKABANK
 * Unifica las respuestas de todos los tipos de servidor
 */
public class ServiceResponse {
    private final int statusCode;
    private final String body;
    private final boolean successful;
    private final String serverType;
    private final String operation;

    public ServiceResponse(int statusCode, String body, boolean successful) {
        this.statusCode = statusCode;
        this.body = body;
        this.successful = successful;
        this.serverType = "";
        this.operation = "";
    }

    public ServiceResponse(int statusCode, String body, boolean successful, String serverType, String operation) {
        this.statusCode = statusCode;
        this.body = body;
        this.successful = successful;
        this.serverType = serverType;
        this.operation = operation;
    }

    public int getStatusCode() { return statusCode; }
    public String getBody() { return body; }
    public boolean isSuccessful() { return successful; }
    public String getServerType() { return serverType; }
    public String getOperation() { return operation; }

    @Override
    public String toString() {
        return String.format("ServiceResponse{statusCode=%d, successful=%s, serverType='%s', operation='%s'}",
                            statusCode, successful, serverType, operation);
    }
}
