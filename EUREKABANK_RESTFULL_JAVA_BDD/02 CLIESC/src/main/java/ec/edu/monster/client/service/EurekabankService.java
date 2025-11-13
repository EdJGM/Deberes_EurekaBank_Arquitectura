package ec.edu.monster.client.service;

import ec.edu.monster.client.config.ServerConfig;
import ec.edu.monster.client.model.ServerType;
import ec.edu.monster.client.service.strategy.*;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

import java.util.Map;

/**
 * Servicio principal de EUREKABANK que maneja la comunicación con todos los servidores
 * Utiliza la configuración de IP desde variables de entorno
 */
public class EurekabankService {

    private final RestClient restClient;
    private final ServiceStrategyFactory strategyFactory;

    public EurekabankService() {
        this.restClient = new RestClient();
        this.strategyFactory = new ServiceStrategyFactory();

        // Mostrar configuración al inicializar
        ServerConfig.printConfiguration();
    }

    /**
     * Llamar al servicio especificado con los parámetros proporcionados
     */
    public ServiceResponse callService(int serverTypeId, Map<String, String> parameters) {
        System.out.println("=== INICIO EurekabankService.callService ===");
        System.out.println("ServerTypeId: " + serverTypeId);
        System.out.println("Parameters: " + parameters);

        try {
            System.out.println("Obteniendo ServerType por ID...");
            ServerType serverType = ServerType.getById(serverTypeId);
            System.out.println("ServerType obtenido: " + serverType);

            System.out.println("Obteniendo strategy de factory...");
            ServiceStrategy strategy = strategyFactory.getStrategy(serverType);
            System.out.println("Strategy obtenida: " + strategy.getClass().getSimpleName());

            System.out.println("Llamando a servidor: " + serverType.getDisplayName());

            ServiceResponse response = null;

            switch (serverType) {
                case REST_JAVA:
                    System.out.println("Procesando REST_JAVA...");
                    response = callRestJavaService(strategy, parameters, serverType);
                    break;

                case REST_DOTNET:
                    System.out.println("Procesando REST_DOTNET...");
                    response = callRestDotNetService(strategy, parameters, serverType);
                    break;

                case SOAP_JAVA:
                    System.out.println("Procesando SOAP_JAVA...");
                    response = callSoapJavaService(strategy, parameters, serverType);
                    break;

                case SOAP_DOTNET:
                    System.out.println("Procesando SOAP_DOTNET...");
                    response = callSoapDotNetService(strategy, parameters, serverType);
                    break;

                default:
                    System.err.println("Tipo de servidor no válido: " + serverTypeId);
                    response = new ServiceResponse(400, "Tipo de servidor no válido: " + serverTypeId, false);
            }

            System.out.println("Respuesta del servicio: " + response);
            System.out.println("=== FIN EurekabankService.callService ===");
            return response;

        } catch (Exception e) {
            System.err.println("=== ERROR EN EurekabankService.callService ===");
            System.err.println("Exception: " + e.getClass().getSimpleName());
            System.err.println("Message: " + e.getMessage());
            e.printStackTrace();

            ServiceResponse errorResponse = new ServiceResponse(500, "Error interno: " + e.getMessage(), false);
            System.out.println("=== FIN EurekabankService.callService (ERROR) ===");
            return errorResponse;
        }
    }

    private ServiceResponse callRestJavaService(ServiceStrategy strategy, Map<String, String> parameters, ServerType serverType) {
        System.out.println("=== INICIO callRestJavaService ===");
        try {
            System.out.println("Construyendo URL...");
            String url = strategy.buildUrl(parameters);
            String operation = parameters.get("operation");
            System.out.println("URL construida: " + url);
            System.out.println("Operación: " + operation);

            RestClient.RestResult result;
            if ("health".equals(operation) || "movimientos".equals(operation)) {
                System.out.println("Ejecutando GET request...");
                result = restClient.get(url);
            } else {
                System.out.println("Ejecutando POST request...");
                RestJavaStrategy javaStrategy = (RestJavaStrategy) strategy;
                String jsonBody = javaStrategy.buildJsonBody(parameters);
                System.out.println("JSON Body: " + jsonBody);
                result = restClient.post(url, jsonBody);
            }

            System.out.println("RestResult recibido: " + result);

            ServiceResponse response = new ServiceResponse(
                result.getStatusCode(),
                result.getBody(),
                result.isSuccessful(),
                serverType.getDisplayName(),
                operation
            );

            System.out.println("=== FIN callRestJavaService ===");
            return response;

        } catch (Exception e) {
            System.err.println("=== ERROR EN callRestJavaService ===");
            System.err.println("Exception: " + e.getClass().getSimpleName());
            System.err.println("Message: " + e.getMessage());
            e.printStackTrace();
            return new ServiceResponse(500, "Error REST Java: " + e.getMessage(), false);
        }
    }

    private ServiceResponse callRestDotNetService(ServiceStrategy strategy, Map<String, String> parameters, ServerType serverType) {
        try {
            String url = strategy.buildUrl(parameters);
            String operation = parameters.get("operation");

            System.out.println("REST .NET URL: " + url);

            RestClient.RestResult result;
            if ("health".equals(operation) || "movimientos".equals(operation)) {
                result = restClient.get(url);
            } else {
                RestDotNetStrategy dotNetStrategy = (RestDotNetStrategy) strategy;
                String jsonBody = dotNetStrategy.buildJsonBody(parameters);
                result = restClient.post(url, jsonBody);
            }

            return new ServiceResponse(
                result.getStatusCode(),
                result.getBody(),
                result.isSuccessful(),
                serverType.getDisplayName(),
                operation
            );

        } catch (Exception e) {
            return new ServiceResponse(500, "Error REST .NET: " + e.getMessage(), false);
        }
    }

    private ServiceResponse callSoapJavaService(ServiceStrategy strategy, Map<String, String> parameters, ServerType serverType) {
        try {
            SoapJavaStrategy soapStrategy = (SoapJavaStrategy) strategy;
            String soapMessage = soapStrategy.buildSoapMessage(parameters);
            String operation = parameters.get("operation");

            System.out.println("SOAP Java Message: " + soapMessage);

            RestClient.RestResult result = sendSoapRequest(ServerConfig.SOAP_JAVA_ENDPOINT_URL, soapMessage, null);

            return new ServiceResponse(
                result.getStatusCode(),
                result.getBody(),
                result.isSuccessful(),
                serverType.getDisplayName(),
                operation
            );

        } catch (Exception e) {
            return new ServiceResponse(500, "Error SOAP Java: " + e.getMessage(), false);
        }
    }

    private ServiceResponse callSoapDotNetService(ServiceStrategy strategy, Map<String, String> parameters, ServerType serverType) {
        try {
            SoapDotNetStrategy soapStrategy = (SoapDotNetStrategy) strategy;
            String soapMessage = soapStrategy.buildSoapMessage(parameters);
            String soapAction = soapStrategy.getSoapAction(parameters.get("operation"));
            String operation = parameters.get("operation");

            System.out.println("SOAP .NET Message: " + soapMessage);
            System.out.println("SOAP Action: " + soapAction);

            RestClient.RestResult result = sendSoapRequest(ServerConfig.SOAP_DOTNET_ENDPOINT_URL, soapMessage, soapAction);

            return new ServiceResponse(
                result.getStatusCode(),
                result.getBody(),
                result.isSuccessful(),
                serverType.getDisplayName(),
                operation
            );

        } catch (Exception e) {
            return new ServiceResponse(500, "Error SOAP .NET: " + e.getMessage(), false);
        }
    }

    private RestClient.RestResult sendSoapRequest(String endpointUrl, String soapMessage, String soapAction) {
        try (CloseableHttpClient httpClient = HttpClients.createDefault()) {
            HttpPost request = new HttpPost(endpointUrl);

            request.setHeader("Content-Type", "text/xml; charset=utf-8");
            if (soapAction != null) {
                request.setHeader("SOAPAction", "\"" + soapAction + "\"");
            }

            request.setEntity(new StringEntity(soapMessage, "UTF-8"));

            return httpClient.execute(request, httpResponse -> {
                int statusCode = httpResponse.getStatusLine().getStatusCode();
                String body = EntityUtils.toString(httpResponse.getEntity());
                return new RestClient.RestResult(statusCode, body);
            });

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error enviando SOAP: " + e.getMessage());
        }
    }
}
