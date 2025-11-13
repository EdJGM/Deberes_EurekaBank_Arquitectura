package ec.edu.monster.client;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;
import java.util.Map;

/**
 * Aplicación de escritorio principal para EUREKABANK
 * Permite al usuario seleccionar entre 4 servidores diferentes y acceder a todas las operaciones bancarias
 */
public class DesktopClientApp extends JFrame {
    private JComboBox<String> serviceCombo;
    private JComboBox<String> operationCombo;
    private JPanel parameterPanel;
    private JTextArea outputArea;
    private JButton invokeButton;
    private JButton clearButton;
    private ServiceCaller serviceCaller;
    private ClientConfig config;
    private Map<String, JTextField> parameterFields;

    // Operaciones disponibles en EUREKABANK
    private final String[] operations = {
        "health", "login", "movimientos", "deposito", "retiro", "transferencia"
    };

    public DesktopClientApp() {
        initializeComponents();
        setupLayout();
        setupEventHandlers();
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setTitle("EUREKABANK - Cliente de Escritorio Unificado");
        setSize(900, 700);
        setLocationRelativeTo(null);
    }

    private void initializeComponents() {
        config = new ClientConfig();
        serviceCaller = new ServiceCaller();
        parameterFields = new HashMap<>();

        // Combo box para seleccionar el servicio
        serviceCombo = new JComboBox<>();
        loadServiceOptions();

        // Combo box para seleccionar la operación
        operationCombo = new JComboBox<>(operations);
        operationCombo.setSelectedItem("health");

        // Panel dinámico para parámetros
        parameterPanel = new JPanel();
        parameterPanel.setLayout(new GridBagLayout());
        parameterPanel.setBorder(BorderFactory.createTitledBorder("Parámetros de la Operación"));

        // Área de salida
        outputArea = new JTextArea(20, 60);
        outputArea.setEditable(false);
        outputArea.setFont(new Font(Font.MONOSPACED, Font.PLAIN, 12));

        // Botones
        invokeButton = new JButton("Invocar Operación");
        clearButton = new JButton("Limpiar");

        // Cargar parámetros de la operación por defecto
        updateParameterPanel();
    }

    private void loadServiceOptions() {
        serviceCombo.addItem("Servicio 1: REST Java (Puerto 8080)");
        serviceCombo.addItem("Servicio 2: REST .NET (Puerto 5000)");
        serviceCombo.addItem("Servicio 3: SOAP Java (Puerto 8080)");
        serviceCombo.addItem("Servicio 4: SOAP .NET (Puerto 57199)");
    }

    private void setupLayout() {
        setLayout(new BorderLayout());

        // Panel superior con selectores
        JPanel topPanel = new JPanel(new GridBagLayout());
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(5, 5, 5, 5);

        gbc.gridx = 0; gbc.gridy = 0;
        topPanel.add(new JLabel("Servidor EUREKABANK:"), gbc);
        gbc.gridx = 1;
        topPanel.add(serviceCombo, gbc);

        gbc.gridx = 0; gbc.gridy = 1;
        topPanel.add(new JLabel("Operación Bancaria:"), gbc);
        gbc.gridx = 1;
        topPanel.add(operationCombo, gbc);

        add(topPanel, BorderLayout.NORTH);

        // Panel central con parámetros y botones
        JPanel centerPanel = new JPanel(new BorderLayout());

        JScrollPane paramScrollPane = new JScrollPane(parameterPanel);
        paramScrollPane.setPreferredSize(new Dimension(500, 250));
        centerPanel.add(paramScrollPane, BorderLayout.CENTER);

        JPanel buttonPanel = new JPanel(new FlowLayout());
        buttonPanel.add(invokeButton);
        buttonPanel.add(clearButton);
        centerPanel.add(buttonPanel, BorderLayout.SOUTH);

        add(centerPanel, BorderLayout.CENTER);

        // Panel inferior con área de salida
        JScrollPane outputScrollPane = new JScrollPane(outputArea);
        outputScrollPane.setBorder(BorderFactory.createTitledBorder("Respuesta del Servidor EUREKABANK"));
        add(outputScrollPane, BorderLayout.SOUTH);
    }

    private void setupEventHandlers() {
        serviceCombo.addActionListener(e -> updateParameterPanel());
        operationCombo.addActionListener(e -> updateParameterPanel());

        invokeButton.addActionListener(e -> invokeSelectedService());
        clearButton.addActionListener(e -> clearFields());
    }

    private void updateParameterPanel() {
        parameterPanel.removeAll();
        parameterFields.clear();

        String selectedOperation = (String) operationCombo.getSelectedItem();
        String[] params = getParametersForOperation(selectedOperation);

        if (params.length > 0) {
            GridBagConstraints gbc = new GridBagConstraints();
            gbc.insets = new Insets(5, 5, 5, 5);
            gbc.anchor = GridBagConstraints.WEST;

            for (int i = 0; i < params.length; i++) {
                String param = params[i];

                gbc.gridx = 0;
                gbc.gridy = i;
                parameterPanel.add(new JLabel(param + ":"), gbc);

                gbc.gridx = 1;
                gbc.fill = GridBagConstraints.HORIZONTAL;
                gbc.weightx = 1.0;

                JTextField field = new JTextField(20);
                // Valores por defecto para facilitar las pruebas
                if ("username".equals(param)) field.setText("admin");
                if ("password".equals(param)) field.setText("admin");
                if ("cuenta".equals(param)) field.setText("00100001");
                if ("importe".equals(param)) field.setText("100.00");
                if ("cuentaOrigen".equals(param)) field.setText("00100001");
                if ("cuentaDestino".equals(param)) field.setText("00100002");

                parameterFields.put(param, field);
                parameterPanel.add(field, gbc);
                gbc.fill = GridBagConstraints.NONE;
                gbc.weightx = 0;
            }
        } else {
            parameterPanel.add(new JLabel("Esta operación no requiere parámetros adicionales"));
        }

        parameterPanel.revalidate();
        parameterPanel.repaint();
    }

    private String[] getParametersForOperation(String operation) {
        switch (operation.toLowerCase()) {
            case "health":
                return new String[0];
            case "login":
                return new String[]{"username", "password"};
            case "movimientos":
                return new String[]{"cuenta"};
            case "deposito":
            case "retiro":
                return new String[]{"cuenta", "importe"};
            case "transferencia":
                return new String[]{"cuentaOrigen", "cuentaDestino", "importe"};
            default:
                return new String[0];
        }
    }

    private void invokeSelectedService() {
        try {
            int serviceNumber = serviceCombo.getSelectedIndex() + 1;
            String operation = (String) operationCombo.getSelectedItem();
            Map<String, String> parameters = new HashMap<>();

            // Agregar la operación seleccionada
            parameters.put("operation", operation);

            // Recopilar parámetros ingresados
            for (Map.Entry<String, JTextField> entry : parameterFields.entrySet()) {
                String value = entry.getValue().getText().trim();
                if (!value.isEmpty()) {
                    parameters.put(entry.getKey(), value);
                }
            }

            String serverInfo = getServerInfo(serviceNumber);
            outputArea.setText("=== EUREKABANK - Invocando Operación ===\n");
            outputArea.append("Servidor: " + serverInfo + "\n");
            outputArea.append("Operación: " + operation + "\n");
            outputArea.append("Parámetros: " + parameters + "\n");
            outputArea.append("Estado: Conectando...\n\n");

            // Llamar al servicio usando ServiceCaller
            RestClient.RestResult result = serviceCaller.callService(serviceNumber, parameters);

            // Mostrar resultado
            outputArea.append("=== RESPUESTA DEL SERVIDOR ===\n");
            outputArea.append("Código HTTP: " + result.getStatusCode() + "\n");
            outputArea.append("Estado: " + (result.isSuccessful() ? "EXITOSO" : "ERROR") + "\n");
            outputArea.append("Respuesta:\n");
            outputArea.append(formatResponse(result.getBody(), serviceNumber));

        } catch (Exception e) {
            outputArea.setText("=== ERROR ===\n");
            outputArea.append("Error al invocar el servicio: " + e.getMessage() + "\n");
            outputArea.append("Detalles: " + e.toString());
            e.printStackTrace();
        }
    }

    private String getServerInfo(int serviceNumber) {
        switch (serviceNumber) {
            case 1: return "REST Java (puerto 8080) - JSON directo";
            case 2: return "REST .NET (puerto 5000) - Respuestas estructuradas";
            case 3: return "SOAP Java (puerto 8080) - Métodos minúsculas";
            case 4: return "SOAP .NET (puerto 57199) - Métodos mayúsculas + SOAPAction";
            default: return "Servidor desconocido";
        }
    }

    private String formatResponse(String response, int serviceNumber) {
        if (response == null || response.isEmpty()) {
            return "Respuesta vacía";
        }

        // Formatear según el tipo de servidor
        if (serviceNumber <= 2) {
            // Servidores REST - intentar formatear JSON
            return formatJson(response);
        } else {
            // Servidores SOAP - mostrar XML formateado
            return formatXml(response);
        }
    }

    private String formatJson(String json) {
        // Formateo básico de JSON
        return json.replace(",", ",\n  ")
                  .replace("{", "{\n  ")
                  .replace("}", "\n}");
    }

    private String formatXml(String xml) {
        // Formateo básico de XML
        return xml.replace("><", ">\n<")
                  .replace("<?xml", "\n<?xml");
    }

    private void clearFields() {
        for (JTextField field : parameterFields.values()) {
            field.setText("");
        }
        outputArea.setText("");
    }

    public static void main(String[] args) {
        // Configurar Look and Feel - versión compatible con Java 21
        try {
            // Usar el look and feel cross-platform por defecto
            UIManager.setLookAndFeel(UIManager.getCrossPlatformLookAndFeelClassName());
        } catch (Exception e) {
            // Si falla, continuar con el look and feel por defecto
            System.err.println("Usando look and feel por defecto");
        }

        SwingUtilities.invokeLater(() -> {
            new DesktopClientApp().setVisible(true);
        });
    }
}
