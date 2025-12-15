package ec.edu.monster.client;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * Configuración del cliente que carga las propiedades de los servicios
 * desde el archivo services.properties
 */
public class ClientConfig {
    private Properties properties;

    public ClientConfig() {
        loadProperties();
    }

    private void loadProperties() {
        properties = new Properties();
        try (InputStream input = getClass().getClassLoader().getResourceAsStream("services.properties")) {
            if (input == null) {
                throw new RuntimeException("No se pudo encontrar el archivo services.properties");
            }
            properties.load(input);
        } catch (IOException e) {
            throw new RuntimeException("Error al cargar services.properties", e);
        }
    }

    public String getServiceName(int serviceNumber) {
        return properties.getProperty("service" + serviceNumber + ".name");
    }

    public String getServiceBaseUrl(int serviceNumber) {
        return properties.getProperty("service" + serviceNumber + ".baseUrl");
    }

    public String getServiceEndpoint(int serviceNumber) {
        return properties.getProperty("service" + serviceNumber + ".endpoint");
    }

    public String[] getServiceParams(int serviceNumber) {
        String params = properties.getProperty("service" + serviceNumber + ".params");
        if (params == null || params.trim().isEmpty()) {
            return new String[0];
        }
        return params.split(",");
    }

    public String getProperty(String key) {
        return properties.getProperty(key);
    }
}
