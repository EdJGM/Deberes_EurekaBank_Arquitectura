package ec.edu.monster.client;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

import java.io.IOException;
import java.util.List;
import java.util.Map;

/**
 * Cliente REST que maneja las peticiones HTTP a los servicios web de EUREKABANK
 * Soporta tanto servidores Java como .NET con sus diferentes formatos de respuesta
 */
public class RestClient {
    private final CloseableHttpClient httpClient;
    private final ObjectMapper objectMapper;

    public RestClient() {
        this.httpClient = HttpClients.createDefault();
        this.objectMapper = new ObjectMapper();
    }

    public RestResult get(String url) throws IOException {
        HttpGet request = new HttpGet(url);
        request.setHeader("Accept", "application/json");

        return httpClient.execute(request, httpResponse -> {
            int statusCode = httpResponse.getStatusLine().getStatusCode();
            String body = EntityUtils.toString(httpResponse.getEntity());
            return new RestResult(statusCode, body);
        });
    }

    public RestResult post(String url, String jsonBody) throws IOException {
        HttpPost request = new HttpPost(url);
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");

        if (jsonBody != null && !jsonBody.isEmpty()) {
            request.setEntity(new StringEntity(jsonBody, "UTF-8"));
        }

        return httpClient.execute(request, httpResponse -> {
            int statusCode = httpResponse.getStatusLine().getStatusCode();
            String body = EntityUtils.toString(httpResponse.getEntity());
            return new RestResult(statusCode, body);
        });
    }

    public void close() throws IOException {
        httpClient.close();
    }

    /**
     * Clase interna que encapsula el resultado de una petición REST
     */
    public static class RestResult {
        private final int statusCode;
        private final String body;

        public RestResult(int statusCode, String body) {
            this.statusCode = statusCode;
            this.body = body;
        }

        public int getStatusCode() {
            return statusCode;
        }

        public String getBody() {
            return body;
        }

        public boolean isSuccessful() {
            return statusCode >= 200 && statusCode < 300;
        }

        @Override
        public String toString() {
            return "RestResult{statusCode=" + statusCode + ", body='" + body + "'}";
        }
    }

    /**
     * Clase para manejar respuestas estándar .NET con estructura success/message/data
     */
    public static class DotNetResponse<T> {
        public boolean success;
        public String message;
        public T data;
    }
}
