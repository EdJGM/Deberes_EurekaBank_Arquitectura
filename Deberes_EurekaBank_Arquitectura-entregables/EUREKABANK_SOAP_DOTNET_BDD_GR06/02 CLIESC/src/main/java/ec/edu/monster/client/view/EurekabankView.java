package ec.edu.monster.client.view;

import ec.edu.monster.client.controller.EurekabankController;
import ec.edu.monster.client.model.BankingOperation;
import ec.edu.monster.client.model.ServerType;
import ec.edu.monster.client.service.ServiceResponse;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;
import java.util.Map;
import java.util.Observable;
import java.util.Observer;

/**
 * Vista principal de EUREKABANK - Arquitectura MVC
 * Interfaz gráfica para interactuar con todos los servidores bancarios
 */
public class EurekabankView extends JFrame implements Observer {

    private final EurekabankController controller;

    // Componentes de la interfaz
    private JComboBox<ServerType> serverCombo;
    private JComboBox<BankingOperation> operationCombo;
    private JPanel parameterPanel;
    private JTextArea outputArea;
    private JButton executeButton;
    private JButton clearButton;
    private JLabel statusLabel;
    private Map<String, JTextField> parameterFields;

    public EurekabankView(EurekabankController controller) {
        this.controller = controller;
        this.parameterFields = new HashMap<>();

        // Registrarse como observador del controlador
        controller.addObserver(this);

        initializeComponents();
        setupLayout();
        setupEventHandlers();
        setupWindow();

        // Inicializar la vista
        updateParameterPanel();
    }

    private void initializeComponents() {
        // Combo box para seleccionar servidor
        serverCombo = new JComboBox<>(controller.getAvailableServerTypes());
        serverCombo.setSelectedItem(controller.getCurrentServerType());

        // Combo box para seleccionar operación
        operationCombo = new JComboBox<>(controller.getAvailableBankingOperations());
        operationCombo.setSelectedItem(controller.getCurrentOperation());

        // Panel dinámico para parámetros
        parameterPanel = new JPanel(new GridBagLayout());
        parameterPanel.setBorder(BorderFactory.createTitledBorder("Parámetros de la Operación"));

        // Área de salida
        outputArea = new JTextArea(18, 70);
        outputArea.setEditable(false);
        outputArea.setFont(new Font(Font.MONOSPACED, Font.PLAIN, 12));
        outputArea.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));

        // Botones
        executeButton = new JButton("Ejecutar Operación");
        executeButton.setFont(new Font(Font.SANS_SERIF, Font.BOLD, 12));
        executeButton.setBackground(new Color(0, 120, 180));
        executeButton.setForeground(Color.WHITE);

        clearButton = new JButton("Limpiar");
        clearButton.setFont(new Font(Font.SANS_SERIF, Font.PLAIN, 12));

        // Label de estado
        statusLabel = new JLabel("Listo para ejecutar operaciones");
        statusLabel.setFont(new Font(Font.SANS_SERIF, Font.ITALIC, 11));
        statusLabel.setForeground(new Color(100, 100, 100));
    }

    private void setupLayout() {
        setLayout(new BorderLayout());

        // Panel superior - Header con información
        JPanel headerPanel = createHeaderPanel();
        add(headerPanel, BorderLayout.NORTH);

        // Panel central - Controles y parámetros
        JPanel centerPanel = createCenterPanel();
        add(centerPanel, BorderLayout.CENTER);

        // Panel inferior - Salida y estado
        JPanel bottomPanel = createBottomPanel();
        add(bottomPanel, BorderLayout.SOUTH);
    }

    private JPanel createHeaderPanel() {
        JPanel headerPanel = new JPanel(new GridBagLayout());
        headerPanel.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createMatteBorder(0, 0, 1, 0, Color.LIGHT_GRAY),
            BorderFactory.createEmptyBorder(15, 15, 15, 15)
        ));

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(5, 10, 5, 10);

        // Título
        JLabel titleLabel = new JLabel("EUREKABANK - Cliente Unificado MVC");
        titleLabel.setFont(new Font(Font.SANS_SERIF, Font.BOLD, 16));
        titleLabel.setForeground(new Color(0, 120, 180));
        gbc.gridx = 0; gbc.gridy = 0; gbc.gridwidth = 4;
        headerPanel.add(titleLabel, gbc);

        gbc.gridwidth = 1;
        gbc.gridy = 1;

        // Selector de servidor
        gbc.gridx = 0;
        headerPanel.add(new JLabel("Servidor:"), gbc);
        gbc.gridx = 1;
        headerPanel.add(serverCombo, gbc);

        // Selector de operación
        gbc.gridx = 2;
        headerPanel.add(new JLabel("Operación:"), gbc);
        gbc.gridx = 3;
        headerPanel.add(operationCombo, gbc);

        return headerPanel;
    }

    private JPanel createCenterPanel() {
        JPanel centerPanel = new JPanel(new BorderLayout());
        centerPanel.setBorder(BorderFactory.createEmptyBorder(10, 15, 10, 15));

        // Panel de parámetros
        JScrollPane paramScrollPane = new JScrollPane(parameterPanel);
        paramScrollPane.setPreferredSize(new Dimension(600, 200));
        paramScrollPane.setBorder(BorderFactory.createEmptyBorder());
        centerPanel.add(paramScrollPane, BorderLayout.CENTER);

        // Panel de botones
        JPanel buttonPanel = new JPanel(new FlowLayout(FlowLayout.CENTER, 15, 10));
        buttonPanel.add(executeButton);
        buttonPanel.add(clearButton);
        centerPanel.add(buttonPanel, BorderLayout.SOUTH);

        return centerPanel;
    }

    private JPanel createBottomPanel() {
        JPanel bottomPanel = new JPanel(new BorderLayout());
        bottomPanel.setBorder(BorderFactory.createEmptyBorder(0, 15, 15, 15));

        // Área de salida
        JScrollPane outputScrollPane = new JScrollPane(outputArea);
        outputScrollPane.setBorder(BorderFactory.createTitledBorder("Respuesta del Servidor"));
        bottomPanel.add(outputScrollPane, BorderLayout.CENTER);

        // Panel de estado
        JPanel statusPanel = new JPanel(new FlowLayout(FlowLayout.LEFT));
        statusPanel.add(statusLabel);
        bottomPanel.add(statusPanel, BorderLayout.SOUTH);

        return bottomPanel;
    }

    private void setupEventHandlers() {
        serverCombo.addActionListener(e -> {
            ServerType selectedServer = (ServerType) serverCombo.getSelectedItem();
            controller.setServerType(selectedServer);
        });

        operationCombo.addActionListener(e -> {
            BankingOperation selectedOperation = (BankingOperation) operationCombo.getSelectedItem();
            controller.setBankingOperation(selectedOperation);
        });

        executeButton.addActionListener(e -> executeOperation());
        clearButton.addActionListener(e -> clearFields());
    }

    private void setupWindow() {
        setTitle("EUREKABANK - Sistema Bancario Unificado");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(1000, 800);
        setLocationRelativeTo(null);
        setMinimumSize(new Dimension(800, 600));

        // Configurar Look and Feel
        try {
            UIManager.setLookAndFeel(UIManager.getCrossPlatformLookAndFeelClassName());
        } catch (Exception e) {
            System.err.println("No se pudo establecer Look and Feel: " + e.getMessage());
        }
    }

    private void updateParameterPanel() {
        parameterPanel.removeAll();
        parameterFields.clear();

        BankingOperation operation = controller.getCurrentOperation();
        String[] parameters = operation.getParameters();

        if (parameters.length > 0) {
            GridBagConstraints gbc = new GridBagConstraints();
            gbc.insets = new Insets(8, 10, 8, 10);
            gbc.anchor = GridBagConstraints.WEST;

            for (int i = 0; i < parameters.length; i++) {
                String param = parameters[i];

                gbc.gridx = 0; gbc.gridy = i;
                JLabel label = new JLabel(param + ":");
                label.setFont(new Font(Font.SANS_SERIF, Font.PLAIN, 12));
                parameterPanel.add(label, gbc);

                gbc.gridx = 1;
                gbc.fill = GridBagConstraints.HORIZONTAL;
                gbc.weightx = 1.0;

                JTextField field = new JTextField(25);
                field.setFont(new Font(Font.SANS_SERIF, Font.PLAIN, 12));

                // Valores por defecto para pruebas
                setDefaultValues(field, param);

                parameterFields.put(param, field);
                parameterPanel.add(field, gbc);

                gbc.fill = GridBagConstraints.NONE;
                gbc.weightx = 0;
            }
        } else {
            JLabel noParamsLabel = new JLabel("Esta operación no requiere parámetros adicionales");
            noParamsLabel.setFont(new Font(Font.SANS_SERIF, Font.ITALIC, 12));
            noParamsLabel.setForeground(Color.GRAY);
            parameterPanel.add(noParamsLabel);
        }

        parameterPanel.revalidate();
        parameterPanel.repaint();
    }

    private void setDefaultValues(JTextField field, String param) {
        switch (param) {
            case "username": field.setText("admin"); break;
            case "password": field.setText("admin"); break;
            case "cuenta": field.setText("00100001"); break;
            case "importe": field.setText("100.00"); break;
            case "cuentaOrigen": field.setText("00100001"); break;
            case "cuentaDestino": field.setText("00100002"); break;
        }
    }

    private void executeOperation() {
        log("Iniciando ejecución desde EurekabankView");
        Map<String, String> parameters = collectParameters();
        log("Parámetros recolectados=" + parameters);
        if (!controller.validateParameters(parameters)) {
            log("Validación fallida de parámetros");
            showError("Parámetros inválidos. Verifique los datos ingresados.");
            return;
        }
        updateStatus("Ejecutando operación...");
        executeButton.setEnabled(false);
        long inicio = System.currentTimeMillis();
        SwingUtilities.invokeLater(() -> {
            log("Invocando controller.executeOperation...");
            ServiceResponse response = controller.executeOperation(parameters);
            long duracion = System.currentTimeMillis() - inicio;
            log("Operación completada en " + duracion + "ms. HTTP=" + response.getStatusCode() + " success=" + response.isSuccessful());
            log("Cuerpo respuesta (truncado): " + safeBody(response.getBody()));
            displayResponse(response);
            executeButton.setEnabled(true);
            updateStatus("Operación completada");
        });
    }

    private Map<String, String> collectParameters() {
        Map<String, String> parameters = new HashMap<>();
        for (Map.Entry<String, JTextField> entry : parameterFields.entrySet()) {
            String value = entry.getValue().getText().trim();
            if (!value.isEmpty()) {
                parameters.put(entry.getKey(), value);
            }
        }
        return parameters;
    }

    private void displayResponse(ServiceResponse response) {
        log("Mostrando respuesta en EurekabankView status=" + response.getStatusCode());
        StringBuilder output = new StringBuilder();
        output.append("=== RESPUESTA EUREKABANK ===\n");
        output.append("Servidor: ").append(response.getServerType()).append("\n");
        output.append("Operación: ").append(response.getOperation()).append("\n");
        output.append("Código HTTP: ").append(response.getStatusCode()).append("\n");
        output.append("Estado: ").append(response.isSuccessful() ? "EXITOSO" : "ERROR").append("\n");
        output.append("Fecha: ").append(java.time.LocalDateTime.now()).append("\n\n");
        output.append("Respuesta:\n");
        output.append(formatResponse(response.getBody()));

        outputArea.setText(output.toString());
        outputArea.setCaretPosition(0);
    }

    private String formatResponse(String response) {
        if (response == null || response.isEmpty()) {
            return "Respuesta vacía";
        }

        // Formateo básico según el tipo de contenido
        if (response.trim().startsWith("{") || response.trim().startsWith("[")) {
            return formatJson(response);
        } else if (response.trim().startsWith("<")) {
            return formatXml(response);
        } else {
            return response;
        }
    }

    private String formatJson(String json) {
        return json.replace(",", ",\n  ")
                  .replace("{", "{\n  ")
                  .replace("}", "\n}");
    }

    private String formatXml(String xml) {
        return xml.replace("><", ">\n<")
                  .replace("<?xml", "\n<?xml");
    }

    private void clearFields() {
        log("Limpiando campos y área de salida");
        for (JTextField field : parameterFields.values()) {
            field.setText("");
        }
        outputArea.setText("");
        updateStatus("Campos limpiados");

        // Restaurar valores por defecto
        for (Map.Entry<String, JTextField> entry : parameterFields.entrySet()) {
            setDefaultValues(entry.getValue(), entry.getKey());
        }
    }

    private void showError(String message) {
        log("Mostrando diálogo de error: " + message);
        JOptionPane.showMessageDialog(this, message, "Error", JOptionPane.ERROR_MESSAGE);
        updateStatus("Error: " + message);
    }

    private void updateStatus(String message) {
        statusLabel.setText(message);
        log("Estado actualizado: " + message);
    }

    private String safeBody(String body) {
        if (body == null) return "<null>";
        return body.length() > 800 ? body.substring(0, 800) + "... (truncado)" : body;
    }

    private void log(String msg) {
        System.out.println("[EurekabankView] " + java.time.LocalDateTime.now() + " - " + msg);
    }

    @Override
    public void update(Observable o, Object arg) {
        String event = (String) arg;

        switch (event) {
            case "OPERATION_CHANGED":
                updateParameterPanel();
                break;
            case "SERVER_CHANGED":
                updateStatus("Servidor cambiado a: " + controller.getCurrentServerInfo());
                break;
            case "OPERATION_EXECUTING":
                updateStatus("Ejecutando operación...");
                break;
            case "OPERATION_COMPLETED":
                updateStatus("Operación completada exitosamente");
                break;
            case "OPERATION_ERROR":
                updateStatus("Error en la operación");
                break;
        }
    }
}
