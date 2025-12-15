/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.servicio;

import ec.edu.monster.db.AccesoDB;
import ec.edu.monster.modelo.Movimiento;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
/**
 *
 * @author josue
 */
public class EurekaService {
    
        public List<Movimiento> leerMovimientos(String cuenta) {
        Connection cn = null;
        List<Movimiento> lista = new ArrayList<>();
        String sql = "SELECT \n"
            + " m.chr_cuencodigo cuenta, \n"
            + " m.int_movinumero nromov, \n"
            + " m.dtt_movifecha fecha, \n"
            + " t.vch_tipodescripcion tipo, \n"
            + " t.vch_tipoaccion accion, \n"
            + " m.dec_moviimporte importe \n"
            + "FROM tipomovimiento t INNER JOIN movimiento m \n"
            + "ON t.chr_tipocodigo = m.chr_tipocodigo \n"
            + "WHERE m.chr_cuencodigo = ? \n"
            + "ORDER BY m.int_movinumero DESC";

        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, cuenta);
            ResultSet rs = pstm.executeQuery();

            while (rs.next()) {
                Movimiento rec = new Movimiento();
                rec.setCuenta(rs.getString("cuenta"));
                rec.setNromov(rs.getInt("nromov"));
                rec.setFecha(rs.getDate("fecha"));
                rec.setTipo(rs.getString("tipo"));
                rec.setAccion(rs.getString("accion"));
                rec.setImporte(rs.getDouble("importe"));

                lista.add(rec);
            }
            rs.close();
        } catch (SQLException e) {
            throw new RuntimeException(e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
        return lista;
    }
        
    public void registrarDeposito(String cuenta, double importe, String codEmp) {
        ejecutarMovimiento(cuenta, importe, codEmp, "003", true);
    }

    public void registrarRetiro(String cuenta, double importe, String codEmp) {
        ejecutarMovimientoDescuento(cuenta, importe, codEmp, "004", false);
    }    
    
    public void registrarTransferencia(String cuentaOrigen, String cuentaDestino, double importe, String codEmp) {
        Connection cn = null;
        try {
            cn = AccesoDB.getConnection();
            cn.setAutoCommit(false);

            // Retirar de la cuenta de origen
            ejecutarMovimientoInternoDescuento(cuentaOrigen, importe, codEmp, "009", cn, false);

            // Depositar en la cuenta de destino
            ejecutarMovimientoInterno(cuentaDestino, importe, codEmp, "008", cn, true);

            // Confirmar la transacción
            cn.commit();
        } catch (SQLException e) {
            try {
                if (cn != null) cn.rollback();
            } catch (Exception ex) {
            }
            throw new RuntimeException(e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
    }
    
    private void ejecutarMovimiento(String cuenta, double importe, String codEmp, String tipoMov, boolean permiteNegativo) {
        Connection cn = null;
        try {
            cn = AccesoDB.getConnection();
            cn.setAutoCommit(false);
            ejecutarMovimientoInterno(cuenta, importe, codEmp, tipoMov, cn, permiteNegativo);
            cn.commit();
        } catch (SQLException e) {
            try {
                if (cn != null) cn.rollback();
            } catch (Exception ex) {
            }
            throw new RuntimeException(e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
    }

    private void ejecutarMovimientoDescuento(String cuenta, double importe, String codEmp, String tipoMov, boolean permiteNegativo) {
        Connection cn = null;
        try {
            cn = AccesoDB.getConnection();
            cn.setAutoCommit(false);
            ejecutarMovimientoInternoDescuento(cuenta, importe, codEmp, tipoMov, cn, permiteNegativo);
            cn.commit();
        } catch (SQLException e) {
            try {
                if (cn != null) cn.rollback();
            } catch (Exception ex) {
            }
            throw new RuntimeException(e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }
    }

    private void ejecutarMovimientoInterno(String cuenta, double importe, String codEmp, String tipoMov, Connection cn, boolean permiteNegativo) throws SQLException {
        String sql = "SELECT dec_cuensaldo, int_cuencontmov "
                + "FROM cuenta "
                + "WHERE chr_cuencodigo = ? AND vch_cuenestado = 'ACTIVO' FOR UPDATE";
        PreparedStatement pstm = cn.prepareStatement(sql);
        pstm.setString(1, cuenta);
        ResultSet rs = pstm.executeQuery();

        if (!rs.next()) {
            throw new SQLException("ERROR: Cuenta no existe o no está activa.");
        }
        double saldo = rs.getDouble("dec_cuensaldo");
        int cont = rs.getInt("int_cuencontmov");
        rs.close();
        pstm.close();

        saldo += importe;
        if (!permiteNegativo && saldo < 0) {
            throw new SQLException("ERROR: Saldo insuficiente.");
        }

        cont++;
        sql = "UPDATE cuenta SET dec_cuensaldo = ?, int_cuencontmov = ? WHERE chr_cuencodigo = ?";
        pstm = cn.prepareStatement(sql);
        pstm.setDouble(1, saldo);
        pstm.setInt(2, cont);
        pstm.setString(3, cuenta);
        pstm.executeUpdate();
        pstm.close();

        sql = "INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte) "
                + "VALUES (?, ?, SYSDATE(), ?, ?, ?)";
        pstm = cn.prepareStatement(sql);
        pstm.setString(1, cuenta);
        pstm.setInt(2, cont);
        pstm.setString(3, codEmp);
        pstm.setString(4, tipoMov);
        pstm.setDouble(5, importe);
        pstm.executeUpdate();
        pstm.close();
    }
    
    
    private void ejecutarMovimientoInternoDescuento(String cuenta, double importe, String codEmp, String tipoMov, Connection cn, boolean permiteNegativo) throws SQLException {
        String sql = "SELECT dec_cuensaldo, int_cuencontmov "
                + "FROM cuenta "
                + "WHERE chr_cuencodigo = ? AND vch_cuenestado = 'ACTIVO' FOR UPDATE";
        PreparedStatement pstm = cn.prepareStatement(sql);
        pstm.setString(1, cuenta);
        ResultSet rs = pstm.executeQuery();

        if (!rs.next()) {
            throw new SQLException("ERROR: Cuenta no existe o no está activa.");
        }
        double saldo = rs.getDouble("dec_cuensaldo");
        int cont = rs.getInt("int_cuencontmov");
        rs.close();
        pstm.close();

        saldo -= importe;
        if (!permiteNegativo && saldo < 0) {
            throw new SQLException("ERROR: Saldo insuficiente.");
        }

        cont++;
        sql = "UPDATE cuenta SET dec_cuensaldo = ?, int_cuencontmov = ? WHERE chr_cuencodigo = ?";
        pstm = cn.prepareStatement(sql);
        pstm.setDouble(1, saldo);
        pstm.setInt(2, cont);
        pstm.setString(3, cuenta);
        pstm.executeUpdate();
        pstm.close();

        sql = "INSERT INTO movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte) "
                + "VALUES (?, ?, SYSDATE(), ?, ?, ?)";
        pstm = cn.prepareStatement(sql);
        pstm.setString(1, cuenta);
        pstm.setInt(2, cont);
        pstm.setString(3, codEmp);
        pstm.setString(4, tipoMov);
        pstm.setDouble(5, importe);
        pstm.executeUpdate();
        pstm.close();
    }    
}
