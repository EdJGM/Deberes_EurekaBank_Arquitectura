package ec.edu.monster.client;

import java.util.Map;

/**
 * Estrategia para el servidor SOAP .NET de EUREKABANK (Servicio 4)
 * Puerto: 57199, Protocolo: SOAP, Métodos: Mayúsculas (Login, Health, ObtenerPorCuenta, etc.)
 * Requiere headers SOAPAction específicos
 */
public class ReportServiceStrategy implements ServiceStrategy {

    @Override
    public String buildUrl(ClientConfig config, Map<String, String> parameters) {
        // Para SOAP, retornamos la URL base del WSDL
        return config.getServiceBaseUrl(4) + config.getServiceEndpoint(4);
    }

    /**
     * Construye el mensaje SOAP para el servidor .NET
     * Los métodos van en mayúsculas: Login, Health, ObtenerPorCuenta, RegistrarDeposito, etc.
     */
    public String buildSoapMessage(Map<String, String> parameters) {
        String operation = parameters.get("operation");
        if (operation == null) {
            operation = "health";
        }

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
                String cuentaDep = parameters.get("cuenta");
                String importeDep = parameters.get("importe");
                soap.append("<tem:RegistrarDeposito>");
                if (cuentaDep != null) soap.append("<tem:cuenta>").append(cuentaDep).append("</tem:cuenta>");
                if (importeDep != null) soap.append("<tem:importe>").append(importeDep).append("</tem:importe>");
                soap.append("</tem:RegistrarDeposito>");
                break;

            case "retiro":
                String cuentaRet = parameters.get("cuenta");
                String importeRet = parameters.get("importe");
                soap.append("<tem:RegistrarRetiro>");
                if (cuentaRet != null) soap.append("<tem:cuenta>").append(cuentaRet).append("</tem:cuenta>");
                if (importeRet != null) soap.append("<tem:importe>").append(importeRet).append("</tem:importe>");
                soap.append("</tem:RegistrarRetiro>");
                break;

            case "transferencia":
                String cuentaOrigen = parameters.get("cuentaOrigen");
                String cuentaDestino = parameters.get("cuentaDestino");
                String importeTrans = parameters.get("importe");
                soap.append("<tem:RegistrarTransferencia>");
                if (cuentaOrigen != null) soap.append("<tem:cuentaOrigen>").append(cuentaOrigen).append("</tem:cuentaOrigen>");
                if (cuentaDestino != null) soap.append("<tem:cuentaDestino>").append(cuentaDestino).append("</tem:cuentaDestino>");
                if (importeTrans != null) soap.append("<tem:importe>").append(importeTrans).append("</tem:importe>");
                soap.append("</tem:RegistrarTransferencia>");
                break;
        }

        soap.append("</soap:Body>");
        soap.append("</soap:Envelope>");

        return soap.toString();
    }

    /**
     * Obtiene la SOAPAction requerida para cada operación del servidor .NET
     */
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
