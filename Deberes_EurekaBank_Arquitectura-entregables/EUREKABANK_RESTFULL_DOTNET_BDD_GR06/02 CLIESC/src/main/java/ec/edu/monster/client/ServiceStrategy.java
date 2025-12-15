package ec.edu.monster.client;

import java.util.Map;

/**
 * Interfaz que define la estrategia para construir URLs de diferentes servicios
 * Cada servicio puede tener una lógica diferente para formar las peticiones
 */
public interface ServiceStrategy {

    /**
     * Construye la URL completa para el servicio basado en la configuración y parámetros
     */
    String buildUrl(ClientConfig config, Map<String, String> parameters);
}
