/*
 * Modelo de datos para Sucursal
 */
package ec.edu.monster.modelo;

import java.io.Serializable;
import jakarta.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author edgar
 */
@XmlRootElement(name = "sucursal")
public class Sucursal implements Serializable {
    private String codigo;
    private String nombre;
    private String ciudad;
    private String direccion;
    private int contadorCuentas;
    private double latitud;
    private double longitud;
    private String telefono;
    private String email;
    private String estado;

    public Sucursal() {
    }

    public Sucursal(String codigo, String nombre, String ciudad, String direccion, 
                   int contadorCuentas, double latitud, double longitud, 
                   String telefono, String email, String estado) {
        this.codigo = codigo;
        this.nombre = nombre;
        this.ciudad = ciudad;
        this.direccion = direccion;
        this.contadorCuentas = contadorCuentas;
        this.latitud = latitud;
        this.longitud = longitud;
        this.telefono = telefono;
        this.email = email;
        this.estado = estado;
    }

    // Getters and Setters
    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getCiudad() {
        return ciudad;
    }

    public void setCiudad(String ciudad) {
        this.ciudad = ciudad;
    }

    public String getDireccion() {
        return direccion;
    }

    public void setDireccion(String direccion) {
        this.direccion = direccion;
    }

    public int getContadorCuentas() {
        return contadorCuentas;
    }

    public void setContadorCuentas(int contadorCuentas) {
        this.contadorCuentas = contadorCuentas;
    }

    public double getLatitud() {
        return latitud;
    }

    public void setLatitud(double latitud) {
        this.latitud = latitud;
    }

    public double getLongitud() {
        return longitud;
    }

    public void setLongitud(double longitud) {
        this.longitud = longitud;
    }

    public String getTelefono() {
        return telefono;
    }

    public void setTelefono(String telefono) {
        this.telefono = telefono;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    @Override
    public String toString() {
        return "Sucursal{" +
                "codigo='" + codigo + '\'' +
                ", nombre='" + nombre + '\'' +
                ", ciudad='" + ciudad + '\'' +
                ", direccion='" + direccion + '\'' +
                ", latitud=" + latitud +
                ", longitud=" + longitud +
                '}';
    }
}