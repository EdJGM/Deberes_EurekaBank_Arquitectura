package ec.edu.monster.client.service.strategy;

import ec.edu.monster.client.config.ServerConfig;
import java.util.Map;

/**
 * Estrategia para el servidor REST Java de EUREKABANK
 * Puerto: 8080, Formato: JSON directo, Query parameters para transacciones
 */
public class RestJavaStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(Map<String, String> parameters) {
        StringBuilder url = new StringBuilder();
        String operation = parameters.get("operation");

        switch (operation.toLowerCase()) {
            case "health":
                url.append(ServerConfig.REST_JAVA_BASE_URL).append("/health");
                break;

            case "login":
                url.append(ServerConfig.REST_JAVA_BASE_URL).append("/login");
                break;

            case "movimientos":
                String cuenta = parameters.get("cuenta");
                if (cuenta != null && !cuenta.isEmpty()) {
                    url.append(ServerConfig.REST_JAVA_BASE_URL).append("/movimientos/").append(cuenta);
                } else {
                    throw new IllegalArgumentException("Cuenta es requerida para obtener movimientos");
                }
                break;

            case "deposito":
                url.append(ServerConfig.REST_JAVA_BASE_URL).append("/deposito");
                addQueryParameters(url, parameters, new String[]{"cuenta", "importe"});
                break;

            case "retiro":
                url.append(ServerConfig.REST_JAVA_BASE_URL).append("/retiro");
                addQueryParameters(url, parameters, new String[]{"cuenta", "importe"});
                break;

            case "transferencia":
                url.append(ServerConfig.REST_JAVA_BASE_URL).append("/transferencia");
                addQueryParameters(url, parameters, new String[]{"cuentaOrigen", "cuentaDestino", "importe"});
                break;

            default:
                throw new IllegalArgumentException("Operación no soportada: " + operation);
        }

        return url.toString();
    }

    private void addQueryParameters(StringBuilder url, Map<String, String> parameters, String[] paramNames) {
        url.append("?");
        boolean first = true;
        for (String paramName : paramNames) {
            String value = parameters.get(paramName);
            if (value != null && !value.isEmpty()) {
                if (!first) url.append("&");
                url.append(paramName).append("=").append(value);
                first = false;
            }
        }
    }

    public String buildJsonBody(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if ("login".equals(operation)) {
            String username = parameters.get("username");
            String password = parameters.get("password");
            if (username != null && password != null) {
                return String.format("{\"username\":\"%s\",\"password\":\"%s\"}", username, password);
            }
        }
        return null;
    }
}
