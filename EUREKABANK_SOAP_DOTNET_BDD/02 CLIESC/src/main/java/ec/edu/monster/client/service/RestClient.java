package ec.edu.monster.client.service;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

import java.io.IOException;

/**
 * Cliente REST para comunicación HTTP con los servidores EUREKABANK
 * Parte de la capa de servicio en la arquitectura MVC
 */
public class RestClient {
    private final CloseableHttpClient httpClient;
    private final ObjectMapper objectMapper;

    public RestClient() {
        this.httpClient = HttpClients.createDefault();
        this.objectMapper = new ObjectMapper();
    }

    public RestResult get(String url) throws IOException {
        System.out.println("=== INICIO RestClient.get ===");
        System.out.println("URL: " + url);

        try {
            HttpGet request = new HttpGet(url);
            request.setHeader("Accept", "application/json");
            System.out.println("Headers configurados para GET");

            System.out.println("Ejecutando petición HTTP GET...");
            RestResult result = httpClient.execute(request, httpResponse -> {
                int statusCode = httpResponse.getStatusLine().getStatusCode();
                String body = EntityUtils.toString(httpResponse.getEntity());
                System.out.println("GET Response - StatusCode: " + statusCode);
                System.out.println("GET Response - Body: " + body);
                return new RestResult(statusCode, body);
            });

            System.out.println("=== FIN RestClient.get ===");
            return result;

        } catch (Exception e) {
            System.err.println("=== ERROR EN RestClient.get ===");
            System.err.println("Exception: " + e.getClass().getSimpleName());
            System.err.println("Message: " + e.getMessage());
            e.printStackTrace();
            throw e;
        }
    }

    public RestResult post(String url, String jsonBody) throws IOException {
        System.out.println("=== INICIO RestClient.post ===");
        System.out.println("URL: " + url);
        System.out.println("JSON Body: " + jsonBody);

        try {
            HttpPost request = new HttpPost(url);
            request.setHeader("Content-Type", "application/json");
            request.setHeader("Accept", "application/json");
            System.out.println("Headers configurados para POST");

            if (jsonBody != null && !jsonBody.isEmpty()) {
                request.setEntity(new StringEntity(jsonBody, "UTF-8"));
                System.out.println("Entity configurada con JSON body");
            }

            System.out.println("Ejecutando petición HTTP POST...");
            RestResult result = httpClient.execute(request, httpResponse -> {
                int statusCode = httpResponse.getStatusLine().getStatusCode();
                String body = EntityUtils.toString(httpResponse.getEntity());
                System.out.println("POST Response - StatusCode: " + statusCode);
                System.out.println("POST Response - Body: " + body);
                return new RestResult(statusCode, body);
            });

            System.out.println("=== FIN RestClient.post ===");
            return result;

        } catch (Exception e) {
            System.err.println("=== ERROR EN RestClient.post ===");
            System.err.println("Exception: " + e.getClass().getSimpleName());
            System.err.println("Message: " + e.getMessage());
            e.printStackTrace();
            throw e;
        }
    }

    public void close() throws IOException {
        httpClient.close();
    }

    /**
     * Clase que encapsula el resultado de una petición REST
     */
    public static class RestResult {
        private final int statusCode;
        private final String body;

        public RestResult(int statusCode, String body) {
            this.statusCode = statusCode;
            this.body = body;
        }

        public int getStatusCode() { return statusCode; }
        public String getBody() { return body; }

        public boolean isSuccessful() {
            return statusCode >= 200 && statusCode < 300;
        }

        @Override
        public String toString() {
            return "RestResult{statusCode=" + statusCode + ", body='" + body + "'}";
        }
    }
}
