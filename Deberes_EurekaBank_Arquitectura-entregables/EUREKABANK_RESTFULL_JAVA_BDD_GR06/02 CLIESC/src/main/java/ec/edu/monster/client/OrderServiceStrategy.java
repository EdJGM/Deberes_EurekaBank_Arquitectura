package ec.edu.monster.client;

import java.util.Map;

/**
 * Estrategia para el servidor SOAP Java de EUREKABANK (Servicio 3)
 * Puerto: 8080, Protocolo: SOAP, Métodos: minúsculas (login, health, traerMovimientos, etc.)
 */
public class OrderServiceStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(ClientConfig config, Map<String, String> parameters) {
        // Para SOAP, retornamos la URL base del WSDL
        return config.getServiceBaseUrl(3) + config.getServiceEndpoint(3);
    }

    /**
     * Construye el mensaje SOAP para el servidor Java
     * Los métodos van en minúsculas: login, health, traerMovimientos, regDeposito, regRetiro, regTransferencia
     */
    public String buildSoapMessage(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if (operation == null) {
            operation = "health";
        }

        StringBuilder soap = new StringBuilder();
        soap.append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        soap.append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" ")
            .append("xmlns:ws=\"http://ws.monster.edu.ec/\">");
        soap.append("<soap:Header/>");
        soap.append("<soap:Body>");

        switch (operation.toLowerCase()) {
            case "health":
                soap.append("<ws:health/>");
                break;

            case "login":
                String username = parameters.get("username");
                String password = parameters.get("password");
                soap.append("<ws:login>");
                if (username != null) soap.append("<arg0>").append(username).append("</arg0>");
                if (password != null) soap.append("<arg1>").append(password).append("</arg1>");
                soap.append("</ws:login>");
                break;

            case "movimientos":
                String cuenta = parameters.get("cuenta");
                soap.append("<ws:traerMovimientos>");
                if (cuenta != null) soap.append("<arg0>").append(cuenta).append("</arg0>");
                soap.append("</ws:traerMovimientos>");
                break;

            case "deposito":
                String cuentaDep = parameters.get("cuenta");
                String importeDep = parameters.get("importe");
                soap.append("<ws:regDeposito>");
                if (cuentaDep != null) soap.append("<arg0>").append(cuentaDep).append("</arg0>");
                if (importeDep != null) soap.append("<arg1>").append(importeDep).append("</arg1>");
                soap.append("</ws:regDeposito>");
                break;

            case "retiro":
                String cuentaRet = parameters.get("cuenta");
                String importeRet = parameters.get("importe");
                soap.append("<ws:regRetiro>");
                if (cuentaRet != null) soap.append("<arg0>").append(cuentaRet).append("</arg0>");
                if (importeRet != null) soap.append("<arg1>").append(importeRet).append("</arg1>");
                soap.append("</ws:regRetiro>");
                break;

            case "transferencia":
                String cuentaOrigen = parameters.get("cuentaOrigen");
                String cuentaDestino = parameters.get("cuentaDestino");
                String importeTrans = parameters.get("importe");
                soap.append("<ws:regTransferencia>");
                if (cuentaOrigen != null) soap.append("<arg0>").append(cuentaOrigen).append("</arg0>");
                if (cuentaDestino != null) soap.append("<arg1>").append(cuentaDestino).append("</arg1>");
                if (importeTrans != null) soap.append("<arg2>").append(importeTrans).append("</arg2>");
                soap.append("</ws:regTransferencia>");
                break;
        }

        soap.append("</soap:Body>");
        soap.append("</soap:Envelope>");

        return soap.toString();
    }
}
