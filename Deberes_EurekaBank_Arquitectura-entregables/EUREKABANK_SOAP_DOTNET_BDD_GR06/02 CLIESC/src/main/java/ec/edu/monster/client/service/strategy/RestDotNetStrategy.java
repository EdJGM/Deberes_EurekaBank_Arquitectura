package ec.edu.monster.client.service.strategy;

import ec.edu.monster.client.config.ServerConfig;
import java.util.Map;

/**
 * Estrategia para el servidor REST .NET de EUREKABANK
 * Puerto: 5000, Formato: JSON en body, Respuestas estructuradas con success/message/data
 */
public class RestDotNetStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(Map<String, String> parameters) {
        StringBuilder url = new StringBuilder();
        String operation = parameters.get("operation");

        switch (operation.toLowerCase()) {
            case "health":
                url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Health");
                break;

            case "login":
                url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Auth/login");
                break;

            case "movimientos":
                String cuenta = parameters.get("cuenta");
                if (cuenta != null && !cuenta.isEmpty()) {
                    url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Movimientos/cuenta/").append(cuenta);
                } else {
                    throw new IllegalArgumentException("Cuenta es requerida para obtener movimientos");
                }
                break;

            case "deposito":
                url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Movimientos/deposito");
                break;

            case "retiro":
                url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Movimientos/retiro");
                break;

            case "transferencia":
                url.append(ServerConfig.REST_DOTNET_BASE_URL).append("/Movimientos/transferencia");
                break;

            default:
                throw new IllegalArgumentException("Operación no soportada: " + operation);
        }

        return url.toString();
    }

    public String buildJsonBody(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if (operation == null) return null;

        StringBuilder json = new StringBuilder("{");

        switch (operation.toLowerCase()) {
            case "login":
                String username = parameters.get("username");
                String password = parameters.get("password");
                if (username != null && password != null) {
                    json.append("\"username\":\"").append(username).append("\",")
                        .append("\"password\":\"").append(password).append("\"");
                }
                break;

            case "deposito":
            case "retiro":
                String cuenta = parameters.get("cuenta");
                String importe = parameters.get("importe");
                if (cuenta != null && importe != null) {
                    json.append("\"cuenta\":\"").append(cuenta).append("\",")
                        .append("\"importe\":").append(importe);
                }
                break;

            case "transferencia":
                String cuentaOrigen = parameters.get("cuentaOrigen");
                String cuentaDestino = parameters.get("cuentaDestino");
                String importeTrans = parameters.get("importe");
                if (cuentaOrigen != null && cuentaDestino != null && importeTrans != null) {
                    json.append("\"cuentaOrigen\":\"").append(cuentaOrigen).append("\",")
                        .append("\"cuentaDestino\":\"").append(cuentaDestino).append("\",")
                        .append("\"importe\":").append(importeTrans);
                }
                break;
        }

        json.append("}");
        return json.length() > 2 ? json.toString() : null;
    }
}
