/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package ec.edu.monster.service;

import ec.edu.monster.db.AccesoDB;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 *
 * @author josue
 */
public class LoginService {
    public boolean login(String username, String password) {
        Connection cn = null;
        boolean acceso = false;
        String sql = "SELECT COUNT(1) AS total " +
                     "FROM usuario " +
                     "WHERE vch_emplusuario = ? " +
                     "AND vch_emplclave = SHA(?) " +
                     "AND vch_emplestado = 'ACTIVO'";

        try {
            cn = AccesoDB.getConnection();
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, username);
            pstm.setString(2, password);
            ResultSet rs = pstm.executeQuery();

            if (rs.next()) {
                acceso = rs.getInt("total") == 1;
            }
            rs.close();
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al validar login: " + e.getMessage());
        } finally {
            try {
                if (cn != null) cn.close();
            } catch (Exception e) {
            }
        }

        return acceso;
    }    
}
