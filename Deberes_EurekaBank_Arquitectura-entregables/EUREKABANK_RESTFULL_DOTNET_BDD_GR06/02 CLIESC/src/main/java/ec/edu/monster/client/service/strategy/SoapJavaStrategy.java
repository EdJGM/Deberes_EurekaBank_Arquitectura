package ec.edu.monster.client.service.strategy;

import ec.edu.monster.client.config.ServerConfig;
import java.util.Map;

/**
 * Estrategia para el servidor SOAP Java de EUREKABANK
 * Puerto: 8080, Protocolo: SOAP, Métodos: minúsculas (login, health, traerMovimientos, etc.)
 */
public class SoapJavaStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(Map<String, String> parameters) {
        return ServerConfig.SOAP_JAVA_WSDL_URL;
    }

    public String buildSoapMessage(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if (operation == null) operation = "health";

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
                addSoapOperation(soap, "regDeposito", parameters, new String[]{"cuenta", "importe"});
                break;

            case "retiro":
                addSoapOperation(soap, "regRetiro", parameters, new String[]{"cuenta", "importe"});
                break;

            case "transferencia":
                addSoapOperation(soap, "regTransferencia", parameters, new String[]{"cuentaOrigen", "cuentaDestino", "importe"});
                break;
        }

        soap.append("</soap:Body>");
        soap.append("</soap:Envelope>");

        return soap.toString();
    }

    private void addSoapOperation(StringBuilder soap, String operation, Map<String, String> parameters, String[] paramNames) {
        soap.append("<ws:").append(operation).append(">");
        for (int i = 0; i < paramNames.length; i++) {
            String value = parameters.get(paramNames[i]);
            if (value != null) {
                soap.append("<arg").append(i).append(">").append(value).append("</arg").append(i).append(">");
            }
        }
        soap.append("</ws:").append(operation).append(">");
    }
}
