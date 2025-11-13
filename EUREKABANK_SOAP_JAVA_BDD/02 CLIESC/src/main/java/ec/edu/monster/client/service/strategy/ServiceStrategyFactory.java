package ec.edu.monster.client.service.strategy;

import ec.edu.monster.client.model.ServerType;

/**
 * Fábrica de estrategias para los diferentes tipos de servidor EUREKABANK
 * Parte del patrón Strategy en la arquitectura MVC
 */
public class ServiceStrategyFactory {

    public ServiceStrategy getStrategy(ServerType serverType) {
        switch (serverType) {
            case REST_JAVA:
                return new RestJavaStrategy();
            case REST_DOTNET:
                return new RestDotNetStrategy();
            case SOAP_JAVA:
                return new SoapJavaStrategy();
            case SOAP_DOTNET:
                return new SoapDotNetStrategy();
            default:
                throw new IllegalArgumentException("Tipo de servidor no soportado: " + serverType);
        }
    }
}
