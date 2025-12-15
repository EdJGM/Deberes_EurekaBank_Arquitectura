package ec.edu.monster.client.service.strategy;

import java.util.Map;

/**
 * Interfaz base para las estrategias de comunicación con servidores EUREKABANK
 * Parte del patrón Strategy en la arquitectura MVC
 */
public interface ServiceStrategy {

    /**
     * Construye la URL para el servicio basado en los parámetros
     */
    String buildUrl(Map<String, String> parameters);
}
