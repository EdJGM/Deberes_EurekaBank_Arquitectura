package ec.edu.monster.client.view;

import ec.edu.monster.client.controller.EurekabankController;
import ec.edu.monster.client.model.ServerType;
import ec.edu.monster.client.service.ServiceResponse;

import javax.swing.*;
import java.awt.*;

/**
 * Pantalla de login que replica el diseño web de EUREKABANK
 */
public class LoginView extends JPanel {

    private final EurekabankController controller;
    private final CardLayout parentLayout;
    private final JPanel parentPanel;
    private JTextField usernameField;
    private JPasswordField passwordField;
    private JComboBox<String> serverCombo;
    private JButton loginButton;
    private JButton changeServerButton;

    public LoginView(EurekabankController controller, CardLayout parentLayout, JPanel parentPanel) {
        this.controller = controller;
        this.parentLayout = parentLayout;
        this.parentPanel = parentPanel;
        initializeComponents();
    }

    private void initializeComponents() {
        setLayout(new BorderLayout());
        setBackground(new Color(240, 242, 247));

        // Panel principal con scroll
        JScrollPane scrollPane = createScrollableLoginPanel();
        add(scrollPane, BorderLayout.CENTER);

        // Botón cerrar
        JPanel topPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        topPanel.setBackground(new Color(240, 242, 247));
        JButton closeButton = new JButton("Close");
        closeButton.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        closeButton.setBackground(Color.WHITE);
        closeButton.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(5, 15, 5, 15)
        ));
        closeButton.addActionListener(e -> System.exit(0));
        topPanel.add(closeButton);
        add(topPanel, BorderLayout.NORTH);
    }

    private JScrollPane createScrollableLoginPanel() {
        // Panel principal centrado con GridBagLayout
        JPanel mainPanel = new JPanel(new GridBagLayout());
        mainPanel.setBackground(new Color(240, 242, 247));

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.insets = new Insets(50, 20, 50, 20);
        gbc.anchor = GridBagConstraints.CENTER;

        // Contenedor del formulario centrado
        JPanel formContainer = createCenteredFormContainer();
        mainPanel.add(formContainer, gbc);

        // Crear scroll pane
        JScrollPane scrollPane = new JScrollPane(mainPanel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);
        scrollPane.setBorder(null);

        return scrollPane;
    }

    private JPanel createCenteredFormContainer() {
        JPanel formContainer = new JPanel();
        formContainer.setLayout(new BoxLayout(formContainer, BoxLayout.Y_AXIS));
        formContainer.setBackground(Color.WHITE);
        formContainer.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(222, 226, 230), 1),
            BorderFactory.createEmptyBorder(40, 40, 40, 40)
        ));
        formContainer.setPreferredSize(new Dimension(450, 500));
        formContainer.setMaximumSize(new Dimension(450, 500));

        // Título centrado
        JLabel titleLabel = new JLabel("EurekaBank");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 32));
        titleLabel.setForeground(new Color(24, 51, 122));
        titleLabel.setAlignmentX(Component.CENTER_ALIGNMENT);
        formContainer.add(titleLabel);

        formContainer.add(Box.createVerticalStrut(10));

        // Subtítulo
        JLabel subtitleLabel = new JLabel("Ingrese sus credenciales para acceder");
        subtitleLabel.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        subtitleLabel.setForeground(new Color(108, 117, 125));
        subtitleLabel.setAlignmentX(Component.CENTER_ALIGNMENT);
        formContainer.add(subtitleLabel);

        formContainer.add(Box.createVerticalStrut(40));

        // Selector de servidor
        JPanel serverPanel = createFieldPanel("Servidor:", createServerCombo());
        formContainer.add(serverPanel);
        formContainer.add(Box.createVerticalStrut(20));

        // Campo usuario
        JPanel userPanel = createFieldPanel("Usuario", createUsernameField());
        formContainer.add(userPanel);
        formContainer.add(Box.createVerticalStrut(20));

        // Campo contraseña
        JPanel passwordPanel = createFieldPanel("Contraseña", createPasswordField());
        formContainer.add(passwordPanel);
        formContainer.add(Box.createVerticalStrut(30));

        // Botón de login
        JPanel buttonPanel = createButtonPanel();
        formContainer.add(buttonPanel);
        formContainer.add(Box.createVerticalStrut(20));

        // Botón cambiar servidor
        JPanel changeServerPanel = createChangeServerPanel();
        formContainer.add(changeServerPanel);

        return formContainer;
    }

    private JComboBox<String> createServerCombo() {
        String[] servers = {
            "REST Java Server",
            "REST .NET Server",
            "SOAP Java Server",
            "SOAP .NET Server"
        };
        serverCombo = new JComboBox<>(servers);
        serverCombo.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        serverCombo.setPreferredSize(new Dimension(350, 40));
        serverCombo.setMaximumSize(new Dimension(350, 40));
        serverCombo.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(5, 10, 5, 10)
        ));
        return serverCombo;
    }

    private JTextField createUsernameField() {
        usernameField = new JTextField("MONSTER");
        usernameField.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        usernameField.setPreferredSize(new Dimension(350, 40));
        usernameField.setMaximumSize(new Dimension(350, 40));
        usernameField.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(10, 15, 10, 15)
        ));
        return usernameField;
    }

    private JPasswordField createPasswordField() {
        passwordField = new JPasswordField("123456");
        passwordField.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        passwordField.setPreferredSize(new Dimension(350, 40));
        passwordField.setMaximumSize(new Dimension(350, 40));
        passwordField.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(10, 15, 10, 15)
        ));
        return passwordField;
    }

    private JPanel createFieldPanel(String labelText, JComponent field) {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(Color.WHITE);

        JLabel label = new JLabel(labelText);
        label.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        label.setForeground(new Color(33, 37, 41));
        label.setAlignmentX(Component.CENTER_ALIGNMENT);
        panel.add(label);

        panel.add(Box.createVerticalStrut(8));

        field.setAlignmentX(Component.CENTER_ALIGNMENT);
        panel.add(field);

        return panel;
    }

    private JPanel createButtonPanel() {
        JPanel panel = new JPanel();
        panel.setBackground(Color.WHITE);
        panel.setLayout(new BoxLayout(panel, BoxLayout.X_AXIS));
        panel.add(Box.createHorizontalGlue());

        loginButton = new JButton("Iniciar Sesión");
        loginButton.setFont(new Font("Segoe UI", Font.BOLD, 14));
        loginButton.setForeground(Color.WHITE);
        loginButton.setBackground(new Color(13, 110, 253));
        loginButton.setPreferredSize(new Dimension(350, 45));
        loginButton.setMaximumSize(new Dimension(350, 45));
        loginButton.setBorder(BorderFactory.createEmptyBorder(12, 20, 12, 20));
        loginButton.addActionListener(e -> performLogin());
        panel.add(loginButton);

        panel.add(Box.createHorizontalGlue());
        return panel;
    }

    private JPanel createChangeServerPanel() {
        JPanel panel = new JPanel();
        panel.setBackground(Color.WHITE);
        panel.setLayout(new BoxLayout(panel, BoxLayout.X_AXIS));
        panel.add(Box.createHorizontalGlue());

        changeServerButton = new JButton("Cambiar Servidor");
        changeServerButton.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        changeServerButton.setForeground(new Color(108, 117, 125));
        changeServerButton.setBackground(Color.WHITE);
        changeServerButton.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(8, 15, 8, 15)
        ));
        changeServerButton.addActionListener(e -> showServerSelection());
        panel.add(changeServerButton);

        panel.add(Box.createHorizontalGlue());
        return panel;
    }

    private void performLogin() {
        log("Iniciando proceso de login");
        String username = usernameField.getText().trim();
        String password = new String(passwordField.getPassword()).trim();
        log("Credenciales ingresadas username=" + username + " (longitud password=" + password.length() + ")");

        if (username.isEmpty() || password.isEmpty()) {
            log("Validación fallida: campos vacíos");
            showError("Por favor complete todos los campos");
            return;
        }

        ServerType serverType = getSelectedServerType();
        log("Servidor seleccionado para login: " + serverType);

        // Crear diálogo (NO mostrar aún para no bloquear)
        JDialog loadingDialog = createLoadingDialog("Autenticando usuario...");

        // Iniciar el trabajo en segundo plano ANTES de mostrar el diálogo modal
        Thread worker = new Thread(() -> {
            long inicio = System.currentTimeMillis();
            try {
                log("[BG] Invocando controller.login...");
                ServiceResponse response = controller.login(username, password, serverType);
                long duracion = System.currentTimeMillis() - inicio;
                log("[BG] Login completado en " + duracion + "ms. HTTP=" + response.getStatusCode() + " success=" + response.isSuccessful());
                log("[BG] Cuerpo de respuesta: " + safeBody(response.getBody()));

                SwingUtilities.invokeLater(() -> {
                    loadingDialog.dispose();
                    if (response.isSuccessful()) {
                        log("Login exitoso. Cambiando a pantalla MAIN_PANEL");
                        try { parentLayout.show(parentPanel, "MAIN_PANEL"); } catch (Exception ex) { log("Error al mostrar MAIN_PANEL: " + ex.getMessage()); }
                    } else {
                        log("Login fallido. Mostrando diálogo de error");
                        showResponseDialog("Error de Autenticación", "Error: " + response.getBody(), true);
                    }
                });
            } catch (Exception e) {
                log("[BG] Excepción en login: " + e.getClass().getSimpleName() + " - " + e.getMessage());
                SwingUtilities.invokeLater(() -> {
                    loadingDialog.dispose();
                    showError("Error inesperado: " + e.getMessage());
                });
            }
        }, "LoginWorkerThread");
        worker.start();

        // Mostrar luego el diálogo (bloquea el EDT mientras el hilo trabaja)
        loadingDialog.setVisible(true);
    }

    private ServerType getSelectedServerType() {
        int selectedIndex = serverCombo.getSelectedIndex();
        switch (selectedIndex) {
            case 0: return ServerType.REST_JAVA;
            case 1: return ServerType.REST_DOTNET;
            case 2: return ServerType.SOAP_JAVA;
            case 3: return ServerType.SOAP_DOTNET;
            default: return ServerType.REST_JAVA;
        }
    }

    private void showServerSelection() {
        log("Navegando a selección de servidor");
        parentLayout.show(parentPanel, "SERVER_SELECTION");
    }

    private JDialog createLoadingDialog(String message) {
        JDialog dialog = new JDialog((Frame) SwingUtilities.getWindowAncestor(this), true);
        dialog.setTitle("Procesando");
        dialog.setSize(350, 120);
        dialog.setLocationRelativeTo(this);
        dialog.setDefaultCloseOperation(JDialog.DO_NOTHING_ON_CLOSE);

        JPanel panel = new JPanel(new BorderLayout());
        panel.setBorder(BorderFactory.createEmptyBorder(20, 20, 20, 20));

        JLabel messageLabel = new JLabel(message);
        messageLabel.setHorizontalAlignment(SwingConstants.CENTER);
        messageLabel.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        panel.add(messageLabel, BorderLayout.CENTER);

        JProgressBar progressBar = new JProgressBar();
        progressBar.setIndeterminate(true);
        panel.add(progressBar, BorderLayout.SOUTH);

        dialog.add(panel);
        return dialog;
    }

    private void showResponseDialog(String title, String message, boolean isError) {
        log("Mostrando diálogo de respuesta title=" + title + " isError=" + isError + " longitudMensaje=" + (message == null ? 0 : message.length()));
        JDialog responseDialog = new JDialog((Frame) SwingUtilities.getWindowAncestor(this), true);
        responseDialog.setTitle(title);
        responseDialog.setSize(500, 300);
        responseDialog.setLocationRelativeTo(this);

        JPanel panel = new JPanel(new BorderLayout());
        panel.setBorder(BorderFactory.createEmptyBorder(20, 20, 20, 20));

        JTextArea responseArea = new JTextArea(message);
        responseArea.setEditable(false);
        responseArea.setFont(new Font("Consolas", Font.PLAIN, 12));
        responseArea.setLineWrap(true);
        responseArea.setWrapStyleWord(true);

        if (isError) {
            responseArea.setBackground(new Color(255, 245, 245));
            responseArea.setForeground(new Color(139, 0, 0));
        } else {
            responseArea.setBackground(new Color(245, 255, 245));
            responseArea.setForeground(new Color(0, 100, 0));
        }

        JScrollPane scrollPane = new JScrollPane(responseArea);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        panel.add(scrollPane, BorderLayout.CENTER);

        JPanel buttonPanel = new JPanel(new FlowLayout());
        JButton closeButton = new JButton("Cerrar");
        closeButton.addActionListener(e -> responseDialog.dispose());
        buttonPanel.add(closeButton);
        panel.add(buttonPanel, BorderLayout.SOUTH);

        responseDialog.add(panel);
        responseDialog.setVisible(true);
    }

    private void showError(String message) {
        log("Mostrando error al usuario: " + message);
        JOptionPane.showMessageDialog(this, message, "Error", JOptionPane.ERROR_MESSAGE);
    }

    /**
     * Update server combo box - method called by EurekabankMVCApp
     */
    public void updateServerCombo() {
        if (serverCombo != null) {
            ServerType currentServer = controller.getCurrentServerType();
            // Update combo selection based on current server type
            switch (currentServer) {
                case REST_JAVA:
                    serverCombo.setSelectedIndex(0);
                    break;
                case REST_DOTNET:
                    serverCombo.setSelectedIndex(1);
                    break;
                case SOAP_JAVA:
                    serverCombo.setSelectedIndex(2);
                    break;
                case SOAP_DOTNET:
                    serverCombo.setSelectedIndex(3);
                    break;
                default:
                    serverCombo.setSelectedIndex(0);
                    break;
            }
        }
        log("Combo de servidor actualizado a índice=" + serverCombo.getSelectedIndex());
    }

    private String safeBody(String body) {
        if (body == null) return "<null>";
        return body.length() > 500 ? body.substring(0, 500) + "... (truncado)" : body;
    }

    private void log(String msg) {
        System.out.println("[LoginView] " + java.time.LocalDateTime.now() + " - " + msg);
    }
}
