package ec.edu.monster.client;

import ec.edu.monster.client.controller.EurekabankController;
import ec.edu.monster.client.view.ServerSelectionView;
import ec.edu.monster.client.view.LoginView;
import ec.edu.monster.client.view.MainBankingPanel;
import ec.edu.monster.client.config.ServerConfig;

import javax.swing.*;
import java.awt.*;

/**
 * Aplicación principal de EUREKABANK con arquitectura MVC
 * Interfaz que replica el diseño web con múltiples pantallas
 */
public class EurekabankMVCApp extends JFrame {

    private CardLayout cardLayout;
    private JPanel mainPanel;
    private EurekabankController controller;
    private ServerSelectionView serverSelectionView;
    private LoginView loginView;
    private MainBankingPanel mainBankingPanel;

    public EurekabankMVCApp() {
        initializeApplication();
    }

    private void initializeApplication() {
        // Configurar Look and Feel
        configureLookAndFeel();

        // Mostrar configuración del servidor
        ServerConfig.printConfiguration();

        // Crear el controlador
        controller = new EurekabankController();

        // Configurar la ventana principal
        setupMainWindow();

        // Crear las vistas
        createViews();

        // Mostrar la pantalla inicial de selección de servidor
        cardLayout.show(mainPanel, "SERVER_SELECTION");

        System.out.println("=== EUREKABANK MVC INICIADO ===");
        System.out.println("Interfaz web replicada en escritorio");
        System.out.println("Para cambiar la IP del servidor, configure la variable de entorno:");
        System.out.println("EUREKABANK_SERVER_IP=<su_ip_del_servidor>");
        System.out.println("IP actual: " + ServerConfig.getServerIp());
        System.out.println("===============================");
    }

    private void setupMainWindow() {
        setTitle("EurekaBank - Sistema Bancario Unificado");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setExtendedState(JFrame.MAXIMIZED_BOTH); // Pantalla completa para mejor experiencia
        setMinimumSize(new Dimension(1024, 768));

        // Configurar el panel principal con CardLayout
        cardLayout = new CardLayout();
        mainPanel = new JPanel(cardLayout);
        mainPanel.setBackground(new Color(240, 242, 247));

        add(mainPanel);
        setLocationRelativeTo(null);
    }

    private void createViews() {
        // Vista de selección de servidor
        serverSelectionView = new ServerSelectionView(controller, cardLayout, mainPanel);
        mainPanel.add(serverSelectionView, "SERVER_SELECTION");

        // Vista de login
        loginView = new LoginView(controller, cardLayout, mainPanel);
        mainPanel.add(loginView, "LOGIN");

        // Panel principal de operaciones bancarias
        mainBankingPanel = new MainBankingPanel(controller, cardLayout, mainPanel);
        mainPanel.add(mainBankingPanel, "MAIN_PANEL");
    }

    public void updateLoginScreen() {
        if (loginView != null) {
            loginView.updateServerCombo();
        }
    }

    private static void configureLookAndFeel() {
        try {
            // Usar Look and Feel del sistema para mejor integración
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());

            // Configuraciones adicionales para mejorar la apariencia
            UIManager.put("Button.arc", 8);
            UIManager.put("Component.arc", 8);
            UIManager.put("TextComponent.arc", 8);

        } catch (Exception e) {
            System.err.println("No se pudo configurar Look and Feel: " + e.getMessage());
            try {
                UIManager.setLookAndFeel(UIManager.getCrossPlatformLookAndFeelClassName());
            } catch (Exception ex) {
                System.err.println("Usando Look and Feel por defecto");
            }
        }
    }

    public static void main(String[] args) {
        // Configurar propiedades del sistema para mejor rendimiento de Swing
        System.setProperty("awt.useSystemAAFontSettings", "on");
        System.setProperty("swing.aatext", "true");
        System.setProperty("sun.java2d.xrender", "true");

        SwingUtilities.invokeLater(() -> {
            try {
                EurekabankMVCApp app = new EurekabankMVCApp();
                app.setVisible(true);

            } catch (Exception e) {
                System.err.println("Error al iniciar la aplicación: " + e.getMessage());
                e.printStackTrace();
                JOptionPane.showMessageDialog(null,
                    "Error al iniciar EUREKABANK: " + e.getMessage(),
                    "Error de Inicialización",
                    JOptionPane.ERROR_MESSAGE);
                System.exit(1);
            }
        });
    }
}
