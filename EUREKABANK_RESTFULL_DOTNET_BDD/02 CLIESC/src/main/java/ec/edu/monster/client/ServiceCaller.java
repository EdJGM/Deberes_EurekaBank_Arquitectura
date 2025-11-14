package ec.edu.monster.client;

import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

import java.util.Map;

/**
 * Orquestador principal que coordina las llamadas a los 4 servidores EUREKABANK
 * Maneja REST Java, REST .NET, SOAP Java y SOAP .NET con sus formatos específicos
 */
public class ServiceCaller {
    private final ClientConfig config;
    private final RestClient restClient;
    private final StrategyFactory strategyFactory;

    public ServiceCaller() {
        this.config = new ClientConfig();
        this.restClient = new RestClient();
        this.strategyFactory = new StrategyFactory();
    }

    /**
     * Llama al servicio especificado con los parámetros proporcionados
     */
    public RestClient.RestResult callService(int serviceNumber, Map<String, String> parameters) {
        try {
            ServiceStrategy strategy = strategyFactory.getStrategy(serviceNumber);

            switch (serviceNumber) {
                case 1: // REST Java
                    return callRestJavaService(strategy, parameters);

                case 2: // REST .NET
                    return callRestDotNetService(strategy, parameters);

                case 3: // SOAP Java
                    return callSoapJavaService(strategy, parameters);

                case 4: // SOAP .NET
                    return callSoapDotNetService(strategy, parameters);

                default:
                    return new RestClient.RestResult(400, "Número de servicio no válido: " + serviceNumber);
            }

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error interno: " + e.getMessage());
        }
    }

    /**
     * Llama al servidor REST Java (puerto 8080)
     * Query parameters, respuestas JSON directas
     */
    private RestClient.RestResult callRestJavaService(ServiceStrategy strategy, Map<String, String> parameters) {
        try {
            String url = strategy.buildUrl(config, parameters);
            String operation = parameters.get("operation");

            System.out.println("REST Java URL: " + url);

            // GET para health y movimientos, POST para operaciones de transacción
            if ("health".equals(operation) || "movimientos".equals(operation)) {
                return restClient.get(url);
            } else {
                // Para login y transacciones usar POST con JSON body
                UserServiceStrategy javaStrategy = (UserServiceStrategy) strategy;
                String jsonBody = buildJsonBodyForJava(parameters);
                return restClient.post(url, jsonBody);
            }

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error REST Java: " + e.getMessage());
        }
    }

    /**
     * Llama al servidor REST .NET (puerto 5000)
     * JSON en body, respuestas estructuradas con success/message/data
     */
    private RestClient.RestResult callRestDotNetService(ServiceStrategy strategy, Map<String, String> parameters) {
        try {
            String url = strategy.buildUrl(config, parameters);
            String operation = parameters.get("operation");

            System.out.println("REST .NET URL: " + url);

            ProductServiceStrategy dotNetStrategy = (ProductServiceStrategy) strategy;

            if ("health".equals(operation) || "movimientos".equals(operation)) {
                return restClient.get(url);
            } else {
                // Usar JSON body para operaciones POST
                String jsonBody = dotNetStrategy.buildJsonBody(parameters);
                return restClient.post(url, jsonBody);
            }

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error REST .NET: " + e.getMessage());
        }
    }

    /**
     * Llama al servidor SOAP Java (puerto 8080)
     * Métodos en minúsculas, namespace http://ws.monster.edu.ec/
     */
    private RestClient.RestResult callSoapJavaService(ServiceStrategy strategy, Map<String, String> parameters) {
        try {
            String endpointUrl = config.getServiceBaseUrl(3); // Sin ?wsdl para endpoint
            OrderServiceStrategy soapStrategy = (OrderServiceStrategy) strategy;
            String soapMessage = soapStrategy.buildSoapMessage(parameters);

            System.out.println("SOAP Java Endpoint: " + endpointUrl);
            System.out.println("SOAP Java Message: " + soapMessage);

            return sendSoapRequest(endpointUrl, soapMessage, null);

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error SOAP Java: " + e.getMessage());
        }
    }

    /**
     * Llama al servidor SOAP .NET (puerto 57199)
     * Métodos en mayúsculas, requiere SOAPAction header
     */
    private RestClient.RestResult callSoapDotNetService(ServiceStrategy strategy, Map<String, String> parameters) {
        try {
            String endpointUrl = config.getServiceBaseUrl(4); // Sin ?wsdl para endpoint
            ReportServiceStrategy soapStrategy = (ReportServiceStrategy) strategy;
            String soapMessage = soapStrategy.buildSoapMessage(parameters);
            String soapAction = soapStrategy.getSoapAction(parameters.get("operation"));

            System.out.println("SOAP .NET Endpoint: " + endpointUrl);
            System.out.println("SOAP .NET Action: " + soapAction);
            System.out.println("SOAP .NET Message: " + soapMessage);

            return sendSoapRequest(endpointUrl, soapMessage, soapAction);

        } catch (Exception e) {
            return new RestClient.RestResult(500, "Error SOAP .NET: " + e.getMessage());
        }
    }

    /**
     * Envía una petición SOAP with los headers apropiados
     */
    private RestClient.RestResult sendSoapRequest(String endpointUrl, String soapMessage, String soapAction) {
        try (CloseableHttpClient httpClient = HttpClients.createDefault()) {
            HttpPost request = new HttpPost(endpointUrl);

            // Headers SOAP
            request.setHeader("Content-Type", "text/xml; charset=utf-8");
            if (soapAction != null) {
                request.setHeader("SOAPAction", "\"" + soapAction + "\"");
            }

            // Body SOAP
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

    /**
     * Construye JSON body para el servidor REST Java
     */
    private String buildJsonBodyForJava(Map<String, String> parameters) {
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
