package ec.edu.monster.client;

import java.util.Map;

/**
 * Estrategia para el servidor REST Java de EUREKABANK (Servicio 1)
 * Puerto: 8080, Formato: JSON directo, Query parameters para transacciones
 */
public class UserServiceStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(ClientConfig config, Map<String, String> parameters) {
        StringBuilder url = new StringBuilder();
        String baseUrl = config.getServiceBaseUrl(1);

        // Obtener la operación solicitada
        String operation = parameters.get("operation");
        if (operation == null) {
            operation = "health"; // Operación por defecto
        }

        switch (operation.toLowerCase()) {
            case "health":
                url.append(baseUrl).append("/health");
                break;

            case "login":
                url.append(baseUrl).append("/login");
                break;

            case "movimientos":
                String cuenta = parameters.get("cuenta");
                if (cuenta != null && !cuenta.isEmpty()) {
                    url.append(baseUrl).append("/movimientos/").append(cuenta);
                } else {
                    throw new IllegalArgumentException("Cuenta es requerida para obtener movimientos");
                }
                break;

            case "deposito":
                url.append(baseUrl).append("/deposito");
                String cuentaDep = parameters.get("cuenta");
                String importeDep = parameters.get("importe");
                if (cuentaDep != null && importeDep != null) {
                    url.append("?cuenta=").append(cuentaDep)
                       .append("&importe=").append(importeDep);
                } else {
                    throw new IllegalArgumentException("Cuenta e importe son requeridos para depósito");
                }
                break;

            case "retiro":
                url.append(baseUrl).append("/retiro");
                String cuentaRet = parameters.get("cuenta");
                String importeRet = parameters.get("importe");
                if (cuentaRet != null && importeRet != null) {
                    url.append("?cuenta=").append(cuentaRet)
                       .append("&importe=").append(importeRet);
                } else {
                    throw new IllegalArgumentException("Cuenta e importe son requeridos para retiro");
                }
                break;

            case "transferencia":
                url.append(baseUrl).append("/transferencia");
                String cuentaOrigen = parameters.get("cuentaOrigen");
                String cuentaDestino = parameters.get("cuentaDestino");
                String importeTrans = parameters.get("importe");
                if (cuentaOrigen != null && cuentaDestino != null && importeTrans != null) {
                    url.append("?cuentaOrigen=").append(cuentaOrigen)
                       .append("&cuentaDestino=").append(cuentaDestino)
                       .append("&importe=").append(importeTrans);
                } else {
                    throw new IllegalArgumentException("Cuenta origen, destino e importe son requeridos para transferencia");
                }
                break;

            default:
                throw new IllegalArgumentException("Operación no soportada: " + operation);
        }

        return url.toString();
    }
}
