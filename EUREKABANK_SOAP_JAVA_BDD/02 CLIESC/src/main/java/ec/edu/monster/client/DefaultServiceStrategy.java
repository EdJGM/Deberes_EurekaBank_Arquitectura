package ec.edu.monster.client;

import java.util.Map;

/**
 * Estrategia por defecto para servicios básicos
 * Construye URLs simples con query parameters
 */
public class DefaultServiceStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(ClientConfig config, Map<String, String> parameters) {
        StringBuilder url = new StringBuilder();

        // Construir URL base
        url.append(config.getServiceBaseUrl(1)); // Usar servicio 1 como base por defecto
        url.append("/").append(config.getServiceEndpoint(1));

        // Agregar parámetros como query parameters
        if (parameters != null && !parameters.isEmpty()) {
            url.append("?");
            boolean first = true;
            for (Map.Entry<String, String> entry : parameters.entrySet()) {
                if (!first) {
                    url.append("&");
                }
                url.append(entry.getKey()).append("=").append(entry.getValue());
                first = false;
            }
        }

        return url.toString();
    }
}
