/*
 * Web Service SOAP para Sucursales
 */
package ec.edu.monster.ws;

import ec.edu.monster.modelo.Sucursal;
import ec.edu.monster.servicio.SucursalService;
import java.util.ArrayList;
import java.util.List;
import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebResult;

/**
 *
 * @author edgar
 */
@WebService(serviceName = "SucursalWS")
public class SucursalWS {
    
    private final SucursalService sucursalService = new SucursalService();

    @WebMethod(operationName = "health")
    @WebResult(name = "status")
    public String health() {
        return "Servicio Sucursales SOAP activo y funcionando correctamente";
    }
    
    /**
     * Obtener todas las sucursales
     * @return Lista de sucursales
     */
    @WebMethod(operationName = "listarSucursales")
    @WebResult(name = "sucursales")
    public List<Sucursal> listarSucursales() {
        List<Sucursal> lista;
        try {
            lista = sucursalService.listarSucursales();
        } catch (Exception e) {
            lista = new ArrayList<>();
        }
        return lista;
    }
    
    /**
     * Obtener sucursal por código
     * @param codigo Código de la sucursal
     * @return Sucursal encontrada
     */
    @WebMethod(operationName = "obtenerSucursal")
    @WebResult(name = "sucursal")
    public Sucursal obtenerSucursal(@WebParam(name = "codigo") String codigo) {
        try {
            return sucursalService.obtenerSucursal(codigo);
        } catch (Exception e) {
            return null;
        }
    }
    
    /**
     * Crear nueva sucursal
     * @param sucursal Datos de la sucursal a crear
     * @return 1 si se creó correctamente, -1 si hubo error
     */
    @WebMethod(operationName = "crearSucursal")
    @WebResult(name = "resultado")
    public int crearSucursal(@WebParam(name = "sucursal") Sucursal sucursal) {
        try {
            boolean resultado = sucursalService.crearSucursal(sucursal);
            return resultado ? 1 : -1;
        } catch (Exception e) {
            return -1;
        }
    }
    
    /**
     * Actualizar sucursal existente
     * @param sucursal Datos de la sucursal a actualizar
     * @return 1 si se actualizó correctamente, -1 si hubo error
     */
    @WebMethod(operationName = "actualizarSucursal")
    @WebResult(name = "resultado")
    public int actualizarSucursal(@WebParam(name = "sucursal") Sucursal sucursal) {
        try {
            boolean resultado = sucursalService.actualizarSucursal(sucursal);
            return resultado ? 1 : -1;
        } catch (Exception e) {
            return -1;
        }
    }
    
    /**
     * Eliminar sucursal (cambiar estado a INACTIVO)
     * @param codigo Código de la sucursal a eliminar
     * @return 1 si se eliminó correctamente, -1 si hubo error
     */
    @WebMethod(operationName = "eliminarSucursal")
    @WebResult(name = "resultado")
    public int eliminarSucursal(@WebParam(name = "codigo") String codigo) {
        try {
            boolean resultado = sucursalService.eliminarSucursal(codigo);
            return resultado ? 1 : -1;
        } catch (Exception e) {
            return -1;
        }
    }
    
    /**
     * Calcular distancia entre dos sucursales
     * @param codigoSucursal1 Código de la primera sucursal
     * @param codigoSucursal2 Código de la segunda sucursal
     * @return Distancia en kilómetros
     */
    @WebMethod(operationName = "calcularDistanciaEntreSucursales")
    @WebResult(name = "distancia")
    public double calcularDistanciaEntreSucursales(
            @WebParam(name = "codigoSucursal1") String codigoSucursal1,
            @WebParam(name = "codigoSucursal2") String codigoSucursal2) {
        try {
            return sucursalService.calcularDistanciaEntreSucursales(codigoSucursal1, codigoSucursal2);
        } catch (Exception e) {
            return -1;
        }
    }
    
    /**
     * Calcular distancia desde posición actual a una sucursal
     * @param codigoSucursal Código de la sucursal
     * @param latitud Latitud de la posición actual
     * @param longitud Longitud de la posición actual
     * @return Distancia en kilómetros
     */
    @WebMethod(operationName = "calcularDistanciaASucursal")
    @WebResult(name = "distancia")
    public double calcularDistanciaASucursal(
            @WebParam(name = "codigoSucursal") String codigoSucursal,
            @WebParam(name = "latitud") double latitud,
            @WebParam(name = "longitud") double longitud) {
        try {
            Sucursal sucursal = sucursalService.obtenerSucursal(codigoSucursal);
            if (sucursal == null) {
                return -1;
            }
            return sucursalService.calcularDistancia(latitud, longitud,
                    sucursal.getLatitud(), sucursal.getLongitud());
        } catch (Exception e) {
            return -1;
        }
    }
    
    /**
     * Encontrar sucursal más cercana a una posición
     * @param latitud Latitud de la posición actual
     * @param longitud Longitud de la posición actual
     * @return Sucursal más cercana
     */
    @WebMethod(operationName = "encontrarSucursalMasCercana")
    @WebResult(name = "sucursal")
    public Sucursal encontrarSucursalMasCercana(
            @WebParam(name = "latitud") double latitud,
            @WebParam(name = "longitud") double longitud) {
        try {
            return sucursalService.encontrarSucursalMasCercana(latitud, longitud);
        } catch (Exception e) {
            return null;
        }
    }
    
    /**
     * Obtener sucursales con sus distancias desde una posición
     * @param latitud Latitud de la posición actual
     * @param longitud Longitud de la posición actual
     * @return Lista de sucursales ordenadas por distancia
     */
    @WebMethod(operationName = "obtenerSucursalesConDistancias")
    @WebResult(name = "sucursalesConDistancia")
    public List<String> obtenerSucursalesConDistancias(
            @WebParam(name = "latitud") double latitud,
            @WebParam(name = "longitud") double longitud) {
        List<String> resultado = new ArrayList<>();
        try {
            List<Sucursal> sucursales = sucursalService.listarSucursales();
            
            for (Sucursal sucursal : sucursales) {
                if ("ACTIVO".equals(sucursal.getEstado()) && 
                    sucursal.getLatitud() != 0 && sucursal.getLongitud() != 0) {
                    
                    double distancia = sucursalService.calcularDistancia(latitud, longitud,
                            sucursal.getLatitud(), sucursal.getLongitud());
                    
                    String info = sucursal.getCodigo() + "|" + sucursal.getNombre() + "|" +
                                 sucursal.getCiudad() + "|" + sucursal.getDireccion() + "|" +
                                 sucursal.getLatitud() + "|" + sucursal.getLongitud() + "|" +
                                 distancia;
                    resultado.add(info);
                }
            }
        } catch (Exception e) {
            // Retorna lista vacía en caso de error
        }
        return resultado;
    }
}