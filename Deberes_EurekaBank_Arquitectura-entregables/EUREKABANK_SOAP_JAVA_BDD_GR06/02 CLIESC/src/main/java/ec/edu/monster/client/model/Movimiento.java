package ec.edu.monster.client.model;

import com.fasterxml.jackson.annotation.JsonAlias;
import com.fasterxml.jackson.annotation.JsonProperty;

/**
 * Modelo de datos para movimientos bancarios de EUREKABANK
 * Compatible con todos los servidores (REST Java, REST .NET, SOAP Java, SOAP .NET)
 */
public class Movimiento {
    @JsonProperty("cuenta")
    private String cuenta;

    @JsonProperty("nromov")
    @JsonAlias("nroMov") // Para compatibilidad con .NET
    private int nromov;

    @JsonProperty("fecha")
    private String fecha;

    @JsonProperty("tipo")
    private String tipo;

    @JsonProperty("accion")
    private String accion;

    @JsonProperty("importe")
    private double importe;

    // Constructor vacío para Jackson
    public Movimiento() {}

    // Constructor completo
    public Movimiento(String cuenta, int nromov, String fecha, String tipo, String accion, double importe) {
        this.cuenta = cuenta;
        this.nromov = nromov;
        this.fecha = fecha;
        this.tipo = tipo;
        this.accion = accion;
        this.importe = importe;
    }

    // Getters y Setters
    public String getCuenta() { return cuenta; }
    public void setCuenta(String cuenta) { this.cuenta = cuenta; }

    public int getNromov() { return nromov; }
    public void setNromov(int nromov) { this.nromov = nromov; }

    public String getFecha() { return fecha; }
    public void setFecha(String fecha) { this.fecha = fecha; }

    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }

    public String getAccion() { return accion; }
    public void setAccion(String accion) { this.accion = accion; }

    public double getImporte() { return importe; }
    public void setImporte(double importe) { this.importe = importe; }

    @Override
    public String toString() {
        return String.format("Movimiento{cuenta='%s', nromov=%d, fecha='%s', tipo='%s', accion='%s', importe=%.2f}",
                            cuenta, nromov, fecha, tipo, accion, importe);
    }
}
