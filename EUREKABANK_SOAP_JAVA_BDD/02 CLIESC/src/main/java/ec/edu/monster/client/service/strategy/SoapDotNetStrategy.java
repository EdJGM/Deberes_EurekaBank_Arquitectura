package ec.edu.monster.client.service.strategy;

import ec.edu.monster.client.config.ServerConfig;
import java.util.Map;

/**
 * Estrategia para el servidor SOAP .NET de EUREKABANK
 * Puerto: 57199, Protocolo: SOAP, Métodos: Mayúsculas (Login, Health, ObtenerPorCuenta, etc.)
 * Requiere headers SOAPAction específicos
 */
public class SoapDotNetStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(Map<String, String> parameters) {
        return ServerConfig.SOAP_DOTNET_WSDL_URL;
    }

    public String buildSoapMessage(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if (operation == null) operation = "health";

        StringBuilder soap = new StringBuilder();
        soap.append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        soap.append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" ")
            .append("xmlns:tem=\"http://tempuri.org/\">");
        soap.append("<soap:Header/>");
        soap.append("<soap:Body>");

        switch (operation.toLowerCase()) {
            case "health":
                soap.append("<tem:Health/>");
                break;

            case "login":
                String username = parameters.get("username");
                String password = parameters.get("password");
                soap.append("<tem:Login>");
                if (username != null) soap.append("<tem:username>").append(username).append("</tem:username>");
                if (password != null) soap.append("<tem:password>").append(password).append("</tem:password>");
                soap.append("</tem:Login>");
                break;

            case "movimientos":
                String cuenta = parameters.get("cuenta");
                soap.append("<tem:ObtenerPorCuenta>");
                if (cuenta != null) soap.append("<tem:cuenta>").append(cuenta).append("</tem:cuenta>");
                soap.append("</tem:ObtenerPorCuenta>");
                break;

            case "deposito":
                addDotNetSoapOperation(soap, "RegistrarDeposito", parameters, new String[]{"cuenta", "importe"});
                break;

            case "retiro":
                addDotNetSoapOperation(soap, "RegistrarRetiro", parameters, new String[]{"cuenta", "importe"});
                break;

            case "transferencia":
                addDotNetSoapOperation(soap, "RegistrarTransferencia", parameters, new String[]{"cuentaOrigen", "cuentaDestino", "importe"});
                break;
        }

        soap.append("</soap:Body>");
        soap.append("</soap:Envelope>");

        return soap.toString();
    }

    private void addDotNetSoapOperation(StringBuilder soap, String operation, Map<String, String> parameters, String[] paramNames) {
        soap.append("<tem:").append(operation).append(">");
        for (String paramName : paramNames) {
            String value = parameters.get(paramName);
            if (value != null) {
                soap.append("<tem:").append(paramName).append(">").append(value).append("</tem:").append(paramName).append(">");
            }
        }
        soap.append("</tem:").append(operation).append(">");
    }

    public String getSoapAction(String operation) {
        if (operation == null) operation = "health";

        switch (operation.toLowerCase()) {
            case "health":
                return "http://tempuri.org/IEurekabankWS/Health";
            case "login":
                return "http://tempuri.org/IEurekabankWS/Login";
            case "movimientos":
                return "http://tempuri.org/IEurekabankWS/ObtenerPorCuenta";
            case "deposito":
                return "http://tempuri.org/IEurekabankWS/RegistrarDeposito";
            case "retiro":
                return "http://tempuri.org/IEurekabankWS/RegistrarRetiro";
            case "transferencia":
                return "http://tempuri.org/IEurekabankWS/RegistrarTransferencia";
            default:
                return "http://tempuri.org/IEurekabankWS/Health";
        }
    }
}
