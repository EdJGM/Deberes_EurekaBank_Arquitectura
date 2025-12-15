/*
 * Servicio de negocio para Sucursales
 */
package ec.edu.monster.servicio;

import ec.edu.monster.db.AccesoDB;
import ec.edu.monster.modelo.Sucursal;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author edgar
 */
public class SucursalService {
    
    // Obtener todas las sucursales
    public List<Sucursal> listarSucursales() {
        Connection cn = null;
        List<Sucursal> lista = new ArrayList<>();
        String sql = "SELECT chr_sucucodigo, vch_sucunombre, vch_sucuciudad, " +
                    "vch_sucudireccion, int_sucucontcuenta, " +
                    "COALESCE(dec_sucuclatitud, 0) as latitud, " +
                    "COALESCE(dec_sucuclongitud, 0) as longitud, " +
                    "COALESCE(vch_sucutelefono, '') as telefono, " +
                    "COALESCE(vch_sucuemail, '') as email, " +
                    "COALESCE(vch_sucuestado, 'ACTIVO') as estado " +
                    "FROM sucursal ORDER BY chr_sucucodigo";
        
        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            ResultSet rs = pstm.executeQuery();
            
            while (rs.next()) {
                Sucursal sucursal = new Sucursal();
                sucursal.setCodigo(rs.getString("chr_sucucodigo"));
                sucursal.setNombre(rs.getString("vch_sucunombre"));
                sucursal.setCiudad(rs.getString("vch_sucuciudad"));
                sucursal.setDireccion(rs.getString("vch_sucudireccion"));
                sucursal.setContadorCuentas(rs.getInt("int_sucucontcuenta"));
                sucursal.setLatitud(rs.getDouble("latitud"));
                sucursal.setLongitud(rs.getDouble("longitud"));
                sucursal.setTelefono(rs.getString("telefono"));
                sucursal.setEmail(rs.getString("email"));
                sucursal.setEstado(rs.getString("estado"));
                
                lista.add(sucursal);
            }
            rs.close();
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al listar sucursales: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return lista;
    }
    
    // Obtener sucursal por código
    public Sucursal obtenerSucursal(String codigo) {
        Connection cn = null;
        Sucursal sucursal = null;
        String sql = "SELECT chr_sucucodigo, vch_sucunombre, vch_sucuciudad, " +
                    "vch_sucudireccion, int_sucucontcuenta, " +
                    "COALESCE(dec_sucuclatitud, 0) as latitud, " +
                    "COALESCE(dec_sucuclongitud, 0) as longitud, " +
                    "COALESCE(vch_sucutelefono, '') as telefono, " +
                    "COALESCE(vch_sucuemail, '') as email, " +
                    "COALESCE(vch_sucuestado, 'ACTIVO') as estado " +
                    "FROM sucursal WHERE chr_sucucodigo = ?";
        
        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, codigo);
            ResultSet rs = pstm.executeQuery();
            
            if (rs.next()) {
                sucursal = new Sucursal();
                sucursal.setCodigo(rs.getString("chr_sucucodigo"));
                sucursal.setNombre(rs.getString("vch_sucunombre"));
                sucursal.setCiudad(rs.getString("vch_sucuciudad"));
                sucursal.setDireccion(rs.getString("vch_sucudireccion"));
                sucursal.setContadorCuentas(rs.getInt("int_sucucontcuenta"));
                sucursal.setLatitud(rs.getDouble("latitud"));
                sucursal.setLongitud(rs.getDouble("longitud"));
                sucursal.setTelefono(rs.getString("telefono"));
                sucursal.setEmail(rs.getString("email"));
                sucursal.setEstado(rs.getString("estado"));
            }
            rs.close();
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al obtener sucursal: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return sucursal;
    }
    
    // Crear nueva sucursal
    public boolean crearSucursal(Sucursal sucursal) {
        Connection cn = null;
        boolean resultado = false;
        String sql = "INSERT INTO sucursal (chr_sucucodigo, vch_sucunombre, vch_sucuciudad, " +
                    "vch_sucudireccion, int_sucucontcuenta, dec_sucuclatitud, dec_sucuclongitud, " +
                    "vch_sucutelefono, vch_sucuemail, vch_sucuestado) " +
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
        
        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, sucursal.getCodigo());
            pstm.setString(2, sucursal.getNombre());
            pstm.setString(3, sucursal.getCiudad());
            pstm.setString(4, sucursal.getDireccion());
            pstm.setInt(5, sucursal.getContadorCuentas());
            pstm.setDouble(6, sucursal.getLatitud());
            pstm.setDouble(7, sucursal.getLongitud());
            pstm.setString(8, sucursal.getTelefono());
            pstm.setString(9, sucursal.getEmail());
            pstm.setString(10, sucursal.getEstado());
            
            int filasAfectadas = pstm.executeUpdate();
            resultado = filasAfectadas > 0;
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al crear sucursal: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return resultado;
    }
    
    // Actualizar sucursal
    public boolean actualizarSucursal(Sucursal sucursal) {
        Connection cn = null;
        boolean resultado = false;
        String sql = "UPDATE sucursal SET vch_sucunombre = ?, vch_sucuciudad = ?, " +
                    "vch_sucudireccion = ?, int_sucucontcuenta = ?, " +
                    "dec_sucuclatitud = ?, dec_sucuclongitud = ?, " +
                    "vch_sucutelefono = ?, vch_sucuemail = ?, vch_sucuestado = ? " +
                    "WHERE chr_sucucodigo = ?";
        
        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, sucursal.getNombre());
            pstm.setString(2, sucursal.getCiudad());
            pstm.setString(3, sucursal.getDireccion());
            pstm.setInt(4, sucursal.getContadorCuentas());
            pstm.setDouble(5, sucursal.getLatitud());
            pstm.setDouble(6, sucursal.getLongitud());
            pstm.setString(7, sucursal.getTelefono());
            pstm.setString(8, sucursal.getEmail());
            pstm.setString(9, sucursal.getEstado());
            pstm.setString(10, sucursal.getCodigo());
            
            int filasAfectadas = pstm.executeUpdate();
            resultado = filasAfectadas > 0;
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al actualizar sucursal: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return resultado;
    }
    
    // Eliminar sucursal (cambiar estado a INACTIVO)
    public boolean eliminarSucursal(String codigo) {
        Connection cn = null;
        boolean resultado = false;
        String sql = "UPDATE sucursal SET vch_sucuestado = 'INACTIVO' WHERE chr_sucucodigo = ?";
        
        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, codigo);
            
            int filasAfectadas = pstm.executeUpdate();
            resultado = filasAfectadas > 0;
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al eliminar sucursal: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return resultado;
    }
    
    // Calcular distancia entre dos puntos usando fórmula de Haversine
    public double calcularDistancia(double lat1, double lon1, double lat2, double lon2) {
        final int R = 6371; // Radio de la Tierra en kilómetros
        
        double latDistance = Math.toRadians(lat2 - lat1);
        double lonDistance = Math.toRadians(lon2 - lon1);
        double a = Math.sin(latDistance / 2) * Math.sin(latDistance / 2)
                + Math.cos(Math.toRadians(lat1)) * Math.cos(Math.toRadians(lat2))
                * Math.sin(lonDistance / 2) * Math.sin(lonDistance / 2);
        double c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        double distance = R * c; // Distancia en kilómetros
        
        return Math.round(distance * 100.0) / 100.0; // Redondear a 2 decimales
    }
    
    // Calcular distancia entre dos sucursales
    public double calcularDistanciaEntreSucursales(String codigoSucursal1, String codigoSucursal2) {
        Sucursal sucursal1 = obtenerSucursal(codigoSucursal1);
        Sucursal sucursal2 = obtenerSucursal(codigoSucursal2);
        
        if (sucursal1 == null || sucursal2 == null) {
            throw new RuntimeException("Una o ambas sucursales no fueron encontradas");
        }
        
        return calcularDistancia(sucursal1.getLatitud(), sucursal1.getLongitud(),
                               sucursal2.getLatitud(), sucursal2.getLongitud());
    }
    
    // Encontrar sucursal más cercana a una posición
    public Sucursal encontrarSucursalMasCercana(double latitud, double longitud) {
        List<Sucursal> sucursales = listarSucursales();
        Sucursal sucursalMasCercana = null;
        double distanciaMinima = Double.MAX_VALUE;
        
        for (Sucursal sucursal : sucursales) {
            if ("ACTIVO".equals(sucursal.getEstado()) && 
                sucursal.getLatitud() != 0 && sucursal.getLongitud() != 0) {
                
                double distancia = calcularDistancia(latitud, longitud,
                                                   sucursal.getLatitud(), sucursal.getLongitud());
                
                if (distancia < distanciaMinima) {
                    distanciaMinima = distancia;
                    sucursalMasCercana = sucursal;
                }
            }
        }
        
        return sucursalMasCercana;
    }
}