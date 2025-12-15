/*
 * Utilidad para integración con Google Maps API
 */
package ec.edu.monster.util;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonArray;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;

/**
 *
 * @author edgar
 */
public class GoogleMapsUtil {
    
    // Reemplaza con tu API Key de Google Maps
    private static final String GOOGLE_MAPS_API_KEY = "KEY";
    private static final String GEOCODING_URL = "https://maps.googleapis.com/maps/api/geocode/json";
    private static final String DISTANCE_MATRIX_URL = "https://maps.googleapis.com/maps/api/distancematrix/json";
    
    /**
     * Obtener coordenadas (latitud, longitud) de una dirección
     * @param direccion Dirección completa
     * @return Array con [latitud, longitud] o null si no se encuentra
     */
    public static double[] obtenerCoordenadas(String direccion) {
        try {
            String encodedAddress = URLEncoder.encode(direccion, StandardCharsets.UTF_8.toString());
            String urlString = GEOCODING_URL + "?address=" + encodedAddress + "&key=" + GOOGLE_MAPS_API_KEY;
            
            String response = realizarPeticionHTTP(urlString);
            if (response == null) return null;
            
            Gson gson = new Gson();
            JsonObject jsonResponse = gson.fromJson(response, JsonObject.class);
            
            if (!"OK".equals(jsonResponse.get("status").getAsString())) {
                return null;
            }
            
            JsonArray results = jsonResponse.getAsJsonArray("results");
            if (results.size() > 0) {
                JsonObject location = results.get(0)
                                .getAsJsonObject()
                        .getAsJsonObject("geometry")
                        .getAsJsonObject("location");
                
                double lat = location.get("lat").getAsDouble();
                double lng = location.get("lng").getAsDouble();
                
                return new double[]{lat, lng};
            }
            
        } catch (Exception e) {
            System.err.println("Error al obtener coordenadas: " + e.getMessage());
        }
        
        return null;
    }
    
    /**
     * Obtener dirección desde coordenadas (Reverse Geocoding)
     * @param latitud Latitud
     * @param longitud Longitud
     * @return Dirección formateada o null si no se encuentra
     */
    public static String obtenerDireccion(double latitud, double longitud) {
        try {
            String urlString = GEOCODING_URL + "?latlng=" + latitud + "," + longitud + "&key=" + GOOGLE_MAPS_API_KEY;
            
            String response = realizarPeticionHTTP(urlString);
            if (response == null) return null;
            
            Gson gson = new Gson();
            JsonObject jsonResponse = gson.fromJson(response, JsonObject.class);
            
            if (!"OK".equals(jsonResponse.get("status").getAsString())) {
                return null;
            }
            
            JsonArray results = jsonResponse.getAsJsonArray("results");
            if (results.size() > 0) {
                return results.get(0).getAsJsonObject()
                    .get("formatted_address").getAsString();
            }
            
        } catch (Exception e) {
            System.err.println("Error al obtener dirección: " + e.getMessage());
        }
        
        return null;
    }
    
    /**
     * Calcular distancia y tiempo de viaje entre dos puntos usando Google Maps
     * @param origenLat Latitud de origen
     * @param origenLng Longitud de origen
     * @param destinoLat Latitud de destino
     * @param destinoLng Longitud de destino
     * @return Array con [distanciaEnKm, tiempoEnMinutos] o null si hay error
     */
    public static double[] calcularDistanciaTiempo(double origenLat, double origenLng, 
                                                   double destinoLat, double destinoLng) {
        try {
            String origins = origenLat + "," + origenLng;
            String destinations = destinoLat + "," + destinoLng;
            
            String urlString = DISTANCE_MATRIX_URL + "?origins=" + origins + 
                              "&destinations=" + destinations + 
                              "&units=metric&mode=driving&key=" + GOOGLE_MAPS_API_KEY;
            
            String response = realizarPeticionHTTP(urlString);
            if (response == null) return null;
            
            Gson gson = new Gson();
            JsonObject jsonResponse = gson.fromJson(response, JsonObject.class);
            
            if (!"OK".equals(jsonResponse.get("status").getAsString())) {
                return null;
            }
            
            JsonArray rows = jsonResponse.getAsJsonArray("rows");
            if (rows.size() > 0) {
                JsonArray elements = rows.get(0).getAsJsonObject().getAsJsonArray("elements");
                if (elements.size() > 0) {
                    JsonObject element = elements.get(0).getAsJsonObject();
                    
                    if ("OK".equals(element.get("status").getAsString())) {
                        // Distancia en metros
                        double distanceMeters = element.getAsJsonObject("distance")
                            .get("value").getAsDouble();
                        double distanceKm = distanceMeters / 1000.0;
                        
                        // Tiempo en segundos
                        double timeSeconds = element.getAsJsonObject("duration")
                            .get("value").getAsDouble();
                        double timeMinutes = timeSeconds / 60.0;
                        
                        return new double[]{
                            Math.round(distanceKm * 100.0) / 100.0,  // Redondear a 2 decimales
                            Math.round(timeMinutes * 100.0) / 100.0  // Redondear a 2 decimales
                        };
                    }
                }
            }
            
        } catch (Exception e) {
            System.err.println("Error al calcular distancia y tiempo: " + e.getMessage());
        }
        
        return null;
    }
    
    /**
     * Generar URL para mapa estático de Google Maps
     * @param latitud Latitud central
     * @param longitud Longitud central
     * @param zoom Nivel de zoom (1-20)
     * @param width Ancho de la imagen
     * @param height Alto de la imagen
     * @return URL del mapa estático
     */
    public static String generarURLMapaEstatico(double latitud, double longitud, 
                                               int zoom, int width, int height) {
        return "https://maps.googleapis.com/maps/api/staticmap?" +
               "center=" + latitud + "," + longitud +
               "&zoom=" + zoom +
               "&size=" + width + "x" + height +
               "&markers=color:red%7C" + latitud + "," + longitud +
               "&key=" + GOOGLE_MAPS_API_KEY;
    }
    
    /**
     * Generar URL para mapa estático con múltiples marcadores
     * @param marcadores Array de coordenadas [lat, lng, label]
     * @param zoom Nivel de zoom
     * @param width Ancho de la imagen
     * @param height Alto de la imagen
     * @return URL del mapa estático
     */
    public static String generarURLMapaConMarcadores(double[][] marcadores, 
                                                    int zoom, int width, int height) {
        StringBuilder url = new StringBuilder();
        url.append("https://maps.googleapis.com/maps/api/staticmap?");
        url.append("size=").append(width).append("x").append(height);
        url.append("&zoom=").append(zoom);
        
        // Agregar marcadores
        for (int i = 0; i < marcadores.length; i++) {
            double lat = marcadores[i][0];
            double lng = marcadores[i][1];
            char label = (char) ('A' + i); // A, B, C, etc.
            
            url.append("&markers=color:red%7Clabel:").append(label)
               .append("%7C").append(lat).append(",").append(lng);
        }
        
        url.append("&key=").append(GOOGLE_MAPS_API_KEY);
        return url.toString();
    }
    
    /**
     * Realizar petición HTTP GET
     * @param urlString URL de la petición
     * @return Respuesta como String o null si hay error
     */
    private static String realizarPeticionHTTP(String urlString) {
        try {
            URL url = new URL(urlString);
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setConnectTimeout(5000);
            connection.setReadTimeout(5000);
            
            int responseCode = connection.getResponseCode();
            if (responseCode == 200) {
                BufferedReader reader = new BufferedReader(
                    new InputStreamReader(connection.getInputStream()));
                StringBuilder response = new StringBuilder();
                String line;
                
                while ((line = reader.readLine()) != null) {
                    response.append(line);
                }
                reader.close();
                
                return response.toString();
            }
            
        } catch (IOException e) {
            System.err.println("Error en petición HTTP: " + e.getMessage());
        }
        
        return null;
    }
    
    /**
     * Validar si hay una API Key configurada
     * @return true si la API Key está configurada
     */
    public static boolean isApiKeyConfigured() {
        return !GOOGLE_MAPS_API_KEY.equals("KEY");
    }
}