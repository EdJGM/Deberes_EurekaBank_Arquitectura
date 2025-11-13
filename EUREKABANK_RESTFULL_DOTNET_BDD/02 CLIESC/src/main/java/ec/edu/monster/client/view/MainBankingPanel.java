package ec.edu.monster.client.view;

import ec.edu.monster.client.controller.EurekabankController;
import ec.edu.monster.client.model.BankingOperation;
import ec.edu.monster.client.service.ServiceResponse;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.util.HashMap;
import java.util.Map;

/**
 * Panel principal de operaciones bancarias que replica el diseño web de EUREKABANK
 */
public class MainBankingPanel extends JPanel {

    private final EurekabankController controller;
    private final CardLayout parentLayout;
    private final JPanel parentPanel;
    private JTabbedPane tabbedPane;
    private JTextField movimientosAccountField;
    private JTextField depositoAccountField;
    private JTextField depositoAmountField;
    private JTextField retiroAccountField;
    private JTextField retiroAmountField;
    private JTextField transferenciaOrigenField;
    private JTextField transferenciaDestinoField;
    private JTextField transferenciaAmountField;

    public MainBankingPanel(EurekabankController controller, CardLayout parentLayout, JPanel parentPanel) {
        this.controller = controller;
        this.parentLayout = parentLayout;
        this.parentPanel = parentPanel;
        initializeComponents();
    }

    private void initializeComponents() {
        setLayout(new BorderLayout());
        setBackground(new Color(240, 242, 247));

        // Header con información del usuario y servidor
        JPanel headerPanel = createHeaderPanel();
        add(headerPanel, BorderLayout.NORTH);

        // Panel principal con scroll y contenido bancario centrado
        JScrollPane mainScrollPane = createScrollableMainPanel();
        add(mainScrollPane, BorderLayout.CENTER);
    }

    private JScrollPane createScrollableMainPanel() {
        // Panel principal con contenido centrado - usando BorderLayout para mejor control
        JPanel contentPanel = new JPanel(new BorderLayout());
        contentPanel.setBackground(new Color(240, 242, 247));

        // Panel wrapper para centrar el contenido
        JPanel wrapperPanel = new JPanel(new GridBagLayout());
        wrapperPanel.setBackground(new Color(240, 242, 247));

        GridBagConstraints gbc = new GridBagConstraints();
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.insets = new Insets(20, 20, 20, 20);
        gbc.anchor = GridBagConstraints.CENTER;
        gbc.fill = GridBagConstraints.BOTH;
        gbc.weightx = 1.0;
        gbc.weighty = 0.0;

        // Panel de control bancario centrado
        JPanel bankingPanel = createCenteredBankingPanel();
        wrapperPanel.add(bankingPanel, gbc);

        contentPanel.add(wrapperPanel, BorderLayout.CENTER);

        // Crear scroll pane con configuración mejorada
        JScrollPane scrollPane = new JScrollPane(contentPanel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_AS_NEEDED);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);
        scrollPane.getHorizontalScrollBar().setUnitIncrement(16);
        scrollPane.setBorder(null);
        // Permitir que el viewport se ajuste al contenido
        scrollPane.setPreferredSize(null);

        return scrollPane;
    }

    private JPanel createHeaderPanel() {
        JPanel headerPanel = new JPanel(new BorderLayout());
        headerPanel.setBackground(Color.WHITE);
        headerPanel.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createMatteBorder(0, 0, 1, 0, new Color(222, 226, 230)),
            BorderFactory.createEmptyBorder(20, 30, 20, 30)
        ));

        // Lado izquierdo - Título y usuario
        JPanel leftPanel = new JPanel();
        leftPanel.setLayout(new BoxLayout(leftPanel, BoxLayout.Y_AXIS));
        leftPanel.setBackground(Color.WHITE);

        JLabel titleLabel = new JLabel("EurekaBank");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 24));
        titleLabel.setForeground(new Color(24, 51, 122));
        leftPanel.add(titleLabel);

        JLabel userLabel = new JLabel("Bienvenido, MONSTER");
        userLabel.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        userLabel.setForeground(new Color(108, 117, 125));
        leftPanel.add(userLabel);

        // Información del servidor
        JPanel serverInfoPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 0, 5));
        serverInfoPanel.setBackground(Color.WHITE);

        JLabel serverIcon = new JLabel("💻");
        serverIcon.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        serverInfoPanel.add(serverIcon);

        JLabel serverLabel = new JLabel(controller.getCurrentServerType().getDisplayName() + " Server");
        serverLabel.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        serverLabel.setForeground(new Color(108, 117, 125));
        serverInfoPanel.add(serverLabel);

        leftPanel.add(serverInfoPanel);
        headerPanel.add(leftPanel, BorderLayout.WEST);

        // Lado derecho - Botón cerrar
        JPanel rightPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        rightPanel.setBackground(Color.WHITE);

        JButton closeButton = new JButton("Close");
        closeButton.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        closeButton.setBackground(Color.WHITE);
        closeButton.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(5, 15, 5, 15)
        ));
        closeButton.addActionListener(e -> System.exit(0));
        rightPanel.add(closeButton);

        headerPanel.add(rightPanel, BorderLayout.EAST);

        return headerPanel;
    }

    private JPanel createCenteredBankingPanel() {
        // Panel contenedor con tamaño adaptable
        JPanel containerPanel = new JPanel(new BorderLayout());
        containerPanel.setBackground(Color.WHITE);
        containerPanel.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(222, 226, 230), 1),
            BorderFactory.createEmptyBorder(30, 30, 30, 30)
        ));
        // Remover tamaños fijos para permitir adaptación
        containerPanel.setPreferredSize(new Dimension(Math.min(800, 900), Math.min(600, 700)));
        containerPanel.setMinimumSize(new Dimension(600, 400));
        // No establecer máximo para permitir crecimiento

        // Título del panel
        JPanel titlePanel = new JPanel();
        titlePanel.setLayout(new BoxLayout(titlePanel, BoxLayout.Y_AXIS));
        titlePanel.setBackground(Color.WHITE);

        JLabel panelTitle = new JLabel("Panel de Control Bancario");
        panelTitle.setFont(new Font("Segoe UI", Font.BOLD, 20));
        panelTitle.setForeground(new Color(33, 37, 41));
        panelTitle.setAlignmentX(Component.CENTER_ALIGNMENT);
        titlePanel.add(panelTitle);

        titlePanel.add(Box.createVerticalStrut(10));

        JLabel panelSubtitle = new JLabel("Administre sus cuentas y realice transacciones");
        panelSubtitle.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        panelSubtitle.setForeground(new Color(108, 117, 125));
        panelSubtitle.setAlignmentX(Component.CENTER_ALIGNMENT);
        titlePanel.add(panelSubtitle);

        containerPanel.add(titlePanel, BorderLayout.NORTH);

        // Pestañas de operaciones - simplificado sin scroll anidado
        tabbedPane = createTabbedPane();
        containerPanel.add(tabbedPane, BorderLayout.CENTER);

        return containerPanel;
    }

    private JTabbedPane createTabbedPane() {
        JTabbedPane tabPane = new JTabbedPane();
        tabPane.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        tabPane.setBackground(Color.WHITE);

        // Agregar pestañas sin scroll panes anidados para evitar conflictos
        tabPane.addTab("Movimientos", createMovimientosPanel());
        tabPane.addTab("Depósito", createDepositoPanel());
        tabPane.addTab("Retiro", createRetiroPanel());
        tabPane.addTab("Transferencia", createTransferenciaPanel());

        // Configurar tamaño mínimo para las pestañas
        tabPane.setPreferredSize(new Dimension(700, 400));
        tabPane.setMinimumSize(new Dimension(500, 300));

        return tabPane;
    }

    private JPanel createMovimientosPanel() {
        // Panel principal con scroll interno
        JPanel mainPanel = new JPanel(new BorderLayout());
        mainPanel.setBackground(Color.WHITE);

        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(Color.WHITE);
        panel.setBorder(BorderFactory.createEmptyBorder(30, 40, 50, 40)); // Aumentar padding inferior

        // Título de la operación centrado
        JPanel titlePanel = new JPanel();
        titlePanel.setLayout(new BoxLayout(titlePanel, BoxLayout.X_AXIS));
        titlePanel.setBackground(Color.WHITE);
        titlePanel.add(Box.createHorizontalGlue());

        JLabel icon = new JLabel("📊");
        icon.setFont(new Font("Segoe UI", Font.PLAIN, 20));
        titlePanel.add(icon);

        titlePanel.add(Box.createHorizontalStrut(10));

        JLabel title = new JLabel("Consultar Movimientos");
        title.setFont(new Font("Segoe UI", Font.BOLD, 18));
        title.setForeground(new Color(13, 110, 253));
        titlePanel.add(title);

        titlePanel.add(Box.createHorizontalGlue());
        panel.add(titlePanel);
        panel.add(Box.createVerticalStrut(30));

        // Campo de número de cuenta centrado
        JPanel fieldPanel = createCenteredFieldPanel("Número de Cuenta", "00100001",
            movimientosAccountField = new JTextField());
        panel.add(fieldPanel);
        panel.add(Box.createVerticalStrut(25));

        // Botón centrado
        JPanel buttonPanel = new JPanel();
        buttonPanel.setBackground(Color.WHITE);
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.add(Box.createHorizontalGlue());

        JButton buscarButton = createActionButton("🔍 Buscar", new Color(13, 110, 253));
        buscarButton.addActionListener(e -> performMovimientos());
        buttonPanel.add(buscarButton);

        buttonPanel.add(Box.createHorizontalGlue());
        panel.add(buttonPanel);

        // Espacio adicional más grande para scroll
        panel.add(Box.createVerticalStrut(100));

        // Envolver en scroll pane si es necesario
        JScrollPane scrollPane = new JScrollPane(panel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        scrollPane.setBorder(null);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);

        mainPanel.add(scrollPane, BorderLayout.CENTER);
        return mainPanel;
    }

    private JPanel createDepositoPanel() {
        JPanel mainPanel = new JPanel(new BorderLayout());
        mainPanel.setBackground(Color.WHITE);

        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(Color.WHITE);
        panel.setBorder(BorderFactory.createEmptyBorder(30, 40, 50, 40));

        // Título centrado
        JPanel titlePanel = new JPanel();
        titlePanel.setLayout(new BoxLayout(titlePanel, BoxLayout.X_AXIS));
        titlePanel.setBackground(Color.WHITE);
        titlePanel.add(Box.createHorizontalGlue());

        JLabel icon = new JLabel("💰");
        icon.setFont(new Font("Segoe UI", Font.PLAIN, 20));
        titlePanel.add(icon);

        titlePanel.add(Box.createHorizontalStrut(10));

        JLabel title = new JLabel("Realizar Depósito");
        title.setFont(new Font("Segoe UI", Font.BOLD, 18));
        title.setForeground(new Color(13, 110, 253));
        titlePanel.add(title);

        titlePanel.add(Box.createHorizontalGlue());
        panel.add(titlePanel);
        panel.add(Box.createVerticalStrut(30));

        // Campos centrados
        panel.add(createCenteredFieldPanel("Número de Cuenta", "00100001",
            depositoAccountField = new JTextField()));
        panel.add(Box.createVerticalStrut(20));
        panel.add(createCenteredFieldPanel("Importe", "100.00",
            depositoAmountField = new JTextField()));
        panel.add(Box.createVerticalStrut(30));

        // Botón centrado
        JPanel buttonPanel = new JPanel();
        buttonPanel.setBackground(Color.WHITE);
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.add(Box.createHorizontalGlue());

        JButton depositoButton = createActionButton("Realizar Depósito", new Color(13, 110, 253));
        depositoButton.addActionListener(e -> performDeposito());
        buttonPanel.add(depositoButton);

        buttonPanel.add(Box.createHorizontalGlue());
        panel.add(buttonPanel);

        panel.add(Box.createVerticalStrut(100));

        JScrollPane scrollPane = new JScrollPane(panel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        scrollPane.setBorder(null);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);

        mainPanel.add(scrollPane, BorderLayout.CENTER);
        return mainPanel;
    }

    private JPanel createRetiroPanel() {
        JPanel mainPanel = new JPanel(new BorderLayout());
        mainPanel.setBackground(Color.WHITE);

        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(Color.WHITE);
        panel.setBorder(BorderFactory.createEmptyBorder(30, 40, 50, 40));

        // Título centrado
        JPanel titlePanel = new JPanel();
        titlePanel.setLayout(new BoxLayout(titlePanel, BoxLayout.X_AXIS));
        titlePanel.setBackground(Color.WHITE);
        titlePanel.add(Box.createHorizontalGlue());

        JLabel icon = new JLabel("💳");
        icon.setFont(new Font("Segoe UI", Font.PLAIN, 20));
        titlePanel.add(icon);

        titlePanel.add(Box.createHorizontalStrut(10));

        JLabel title = new JLabel("Realizar Retiro");
        title.setFont(new Font("Segoe UI", Font.BOLD, 18));
        title.setForeground(new Color(13, 110, 253));
        titlePanel.add(title);

        titlePanel.add(Box.createHorizontalGlue());
        panel.add(titlePanel);
        panel.add(Box.createVerticalStrut(30));

        // Campos centrados
        panel.add(createCenteredFieldPanel("Número de Cuenta", "00100001",
            retiroAccountField = new JTextField()));
        panel.add(Box.createVerticalStrut(20));
        panel.add(createCenteredFieldPanel("Importe", "100.00",
            retiroAmountField = new JTextField()));
        panel.add(Box.createVerticalStrut(30));

        // Botón centrado
        JPanel buttonPanel = new JPanel();
        buttonPanel.setBackground(Color.WHITE);
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.add(Box.createHorizontalGlue());

        JButton retiroButton = createActionButton("Realizar Retiro", new Color(13, 110, 253));
        retiroButton.addActionListener(e -> performRetiro());
        buttonPanel.add(retiroButton);

        buttonPanel.add(Box.createHorizontalGlue());
        panel.add(buttonPanel);

        panel.add(Box.createVerticalStrut(100));

        JScrollPane scrollPane = new JScrollPane(panel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        scrollPane.setBorder(null);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);

        mainPanel.add(scrollPane, BorderLayout.CENTER);
        return mainPanel;
    }

    private JPanel createTransferenciaPanel() {
        JPanel mainPanel = new JPanel(new BorderLayout());
        mainPanel.setBackground(Color.WHITE);

        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(Color.WHITE);
        panel.setBorder(BorderFactory.createEmptyBorder(30, 40, 50, 40));

        // Título centrado
        JPanel titlePanel = new JPanel();
        titlePanel.setLayout(new BoxLayout(titlePanel, BoxLayout.X_AXIS));
        titlePanel.setBackground(Color.WHITE);
        titlePanel.add(Box.createHorizontalGlue());

        JLabel icon = new JLabel("🔄");
        icon.setFont(new Font("Segoe UI", Font.PLAIN, 20));
        titlePanel.add(icon);

        titlePanel.add(Box.createHorizontalStrut(10));

        JLabel title = new JLabel("Realizar Transferencia");
        title.setFont(new Font("Segoe UI", Font.BOLD, 18));
        title.setForeground(new Color(13, 110, 253));
        titlePanel.add(title);

        titlePanel.add(Box.createHorizontalGlue());
        panel.add(titlePanel);
        panel.add(Box.createVerticalStrut(30));

        // Campos centrados
        panel.add(createCenteredFieldPanel("Cuenta Origen", "00100001",
            transferenciaOrigenField = new JTextField()));
        panel.add(Box.createVerticalStrut(20));
        panel.add(createCenteredFieldPanel("Cuenta Destino", "00100002",
            transferenciaDestinoField = new JTextField()));
        panel.add(Box.createVerticalStrut(20));
        panel.add(createCenteredFieldPanel("Importe", "100.00",
            transferenciaAmountField = new JTextField()));
        panel.add(Box.createVerticalStrut(30));

        // Botón centrado
        JPanel buttonPanel = new JPanel();
        buttonPanel.setBackground(Color.WHITE);
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.add(Box.createHorizontalGlue());

        JButton transferenciaButton = createActionButton("Realizar Transferencia", new Color(13, 110, 253));
        transferenciaButton.addActionListener(e -> performTransferencia());
        buttonPanel.add(transferenciaButton);

        buttonPanel.add(Box.createHorizontalGlue());
        panel.add(buttonPanel);

        panel.add(Box.createVerticalStrut(100));

        JScrollPane scrollPane = new JScrollPane(panel);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_NEVER);
        scrollPane.setBorder(null);
        scrollPane.getVerticalScrollBar().setUnitIncrement(16);

        mainPanel.add(scrollPane, BorderLayout.CENTER);
        return mainPanel;
    }

    private JPanel createCenteredFieldPanel(String labelText, String placeholder, JTextField field) {
        JPanel containerPanel = new JPanel();
        containerPanel.setLayout(new BoxLayout(containerPanel, BoxLayout.Y_AXIS));
        containerPanel.setBackground(Color.WHITE);

        // Label centrado
        JLabel label = new JLabel(labelText);
        label.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        label.setForeground(new Color(33, 37, 41));
        label.setAlignmentX(Component.CENTER_ALIGNMENT);
        containerPanel.add(label);

        containerPanel.add(Box.createVerticalStrut(8));

        // Campo centrado con tamaño adaptable
        field.setText(placeholder);
        field.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        // Usar tamaños más flexibles
        field.setPreferredSize(new Dimension(Math.min(400, 350), 40));
        field.setMaximumSize(new Dimension(450, 40));
        field.setMinimumSize(new Dimension(250, 40));
        field.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(10, 15, 10, 15)
        ));
        field.setAlignmentX(Component.CENTER_ALIGNMENT);
        containerPanel.add(field);

        return containerPanel;
    }

    private JButton createActionButton(String text, Color backgroundColor) {
        JButton button = new JButton(text);
        button.setFont(new Font("Segoe UI", Font.BOLD, 14));
        button.setForeground(Color.WHITE);
        button.setBackground(backgroundColor);
        // Hacer botones más flexibles
        button.setPreferredSize(new Dimension(Math.min(400, 350), 45));
        button.setMaximumSize(new Dimension(450, 45));
        button.setMinimumSize(new Dimension(250, 45));
        button.setBorder(BorderFactory.createEmptyBorder(12, 20, 12, 20));
        return button;
    }

    private void performMovimientos() {
        log("Iniciando operación MOVIMIENTOS");
        String cuenta = movimientosAccountField.getText().trim();
        log("Cuenta ingresada=" + cuenta);
        if (cuenta.isEmpty()) {
            log("Validación fallida: cuenta vacía");
            showError("Por favor ingrese un número de cuenta válido");
            return;
        }
        controller.setBankingOperation(BankingOperation.MOVIMIENTOS);
        Map<String, String> parameters = new HashMap<>();
        parameters.put("cuenta", cuenta);
        log("Parámetros preparados=" + parameters);
        executeOperation(parameters, "Consultando movimientos...");
    }

    private void performDeposito() {
        log("Iniciando operación DEPOSITO");
        String cuenta = depositoAccountField.getText().trim();
        String importe = depositoAmountField.getText().trim();
        log("Cuenta=" + cuenta + ", importe=" + importe);
        if (cuenta.isEmpty() || importe.isEmpty()) {
            log("Validación fallida: campos vacíos");
            showError("Por favor complete todos los campos");
            return;
        }
        controller.setBankingOperation(BankingOperation.DEPOSITO);
        Map<String, String> parameters = new HashMap<>();
        parameters.put("cuenta", cuenta);
        parameters.put("importe", importe);
        log("Parámetros preparados=" + parameters);
        executeOperation(parameters, "Procesando depósito...");
    }

    private void performRetiro() {
        log("Iniciando operación RETIRO");
        String cuenta = retiroAccountField.getText().trim();
        String importe = retiroAmountField.getText().trim();
        log("Cuenta=" + cuenta + ", importe=" + importe);
        if (cuenta.isEmpty() || importe.isEmpty()) {
            log("Validación fallida: campos vacíos");
            showError("Por favor complete todos los campos");
            return;
        }
        controller.setBankingOperation(BankingOperation.RETIRO);
        Map<String, String> parameters = new HashMap<>();
        parameters.put("cuenta", cuenta);
        parameters.put("importe", importe);
        log("Parámetros preparados=" + parameters);
        executeOperation(parameters, "Procesando retiro...");
    }

    private void performTransferencia() {
        log("Iniciando operación TRANSFERENCIA");
        String cuentaOrigen = transferenciaOrigenField.getText().trim();
        String cuentaDestino = transferenciaDestinoField.getText().trim();
        String importe = transferenciaAmountField.getText().trim();
        log("Origen=" + cuentaOrigen + ", Destino=" + cuentaDestino + ", Importe=" + importe);
        if (cuentaOrigen.isEmpty() || cuentaDestino.isEmpty() || importe.isEmpty()) {
            log("Validación fallida: campos vacíos");
            showError("Por favor complete todos los campos");
            return;
        }
        controller.setBankingOperation(BankingOperation.TRANSFERENCIA);
        Map<String, String> parameters = new HashMap<>();
        parameters.put("cuentaOrigen", cuentaOrigen);
        parameters.put("cuentaDestino", cuentaDestino);
        parameters.put("importe", importe);
        log("Parámetros preparados=" + parameters);
        executeOperation(parameters, "Procesando transferencia...");
    }

    private void executeOperation(Map<String, String> parameters, String loadingMessage) {
        log("Ejecutando operación. Mensaje de carga='" + loadingMessage + "' parámetros=" + parameters);
        JDialog loadingDialog = createLoadingDialog(loadingMessage); // crear pero NO mostrar aún

        Thread worker = new Thread(() -> {
            long inicio = System.currentTimeMillis();
            try {
                log("[BG] Invocando controller.executeOperation...");
                ServiceResponse response = controller.executeOperation(parameters);
                long duracion = System.currentTimeMillis() - inicio;
                log("[BG] Operación completada en " + duracion + "ms. HTTP=" + response.getStatusCode() + " success=" + response.isSuccessful());
                log("[BG] Respuesta cuerpo (truncada): " + safeBody(response.getBody()));
                SwingUtilities.invokeLater(() -> {
                    loadingDialog.dispose();
                    if (response.isSuccessful()) {
                        log("Operación exitosa. Mostrando diálogo éxito");
                        showResponseDialog("Éxito", response.getBody(), false);
                    } else {
                        log("Operación fallida. Mostrando diálogo error");
                        showResponseDialog("Error", "Error en la operación: " + response.getBody(), true);
                    }
                });
            } catch (Exception e) {
                log("[BG] Excepción durante operación: " + e.getClass().getSimpleName() + " - " + e.getMessage());
                SwingUtilities.invokeLater(() -> {
                    loadingDialog.dispose();
                    showError("Error inesperado: " + e.getMessage());
                });
            }
        }, "BankOpWorkerThread");
        worker.start();

        // Mostrar diálogo después de arrancar el hilo
        loadingDialog.setVisible(true);
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

        // Agregar una barra de progreso indeterminada
        JProgressBar progressBar = new JProgressBar();
        progressBar.setIndeterminate(true);
        panel.add(progressBar, BorderLayout.SOUTH);

        dialog.add(panel);
        return dialog;
    }

    private void showResponseDialog(String title, String message, boolean isError) {
        log("Mostrando diálogo de respuesta title=" + title + " isError=" + isError + " len=" + (message == null ? 0 : message.length()));
        // Crear un diálogo personalizado para mostrar las respuestas del servidor
        JDialog responseDialog = new JDialog((Frame) SwingUtilities.getWindowAncestor(this), true);
        responseDialog.setTitle(title);
        responseDialog.setSize(600, 400);
        responseDialog.setLocationRelativeTo(this);

        JPanel panel = new JPanel(new BorderLayout());
        panel.setBorder(BorderFactory.createEmptyBorder(20, 20, 20, 20));

        // Área de texto para mostrar la respuesta
        JTextArea responseArea = new JTextArea(message);
        responseArea.setEditable(false);
        responseArea.setFont(new Font("Consolas", Font.PLAIN, 12));
        responseArea.setLineWrap(true);
        responseArea.setWrapStyleWord(true);

        // Color de fondo según el tipo de respuesta
        if (isError) {
            responseArea.setBackground(new Color(255, 245, 245));
            responseArea.setForeground(new Color(139, 0, 0));
        } else {
            responseArea.setBackground(new Color(245, 255, 245));
            responseArea.setForeground(new Color(0, 100, 0));
        }

        JScrollPane scrollPane = new JScrollPane(responseArea);
        scrollPane.setVerticalScrollBarPolicy(JScrollPane.VERTICAL_SCROLLBAR_AS_NEEDED);
        scrollPane.setBorder(BorderFactory.createLoweredBevelBorder());
        panel.add(scrollPane, BorderLayout.CENTER);

        // Panel de botones
        JPanel buttonPanel = new JPanel(new FlowLayout());
        JButton closeButton = new JButton("Cerrar");
        closeButton.addActionListener(e -> responseDialog.dispose());

        JButton copyButton = new JButton("Copiar");
        copyButton.addActionListener(e -> {
            responseArea.selectAll();
            responseArea.copy();
            JOptionPane.showMessageDialog(responseDialog, "Respuesta copiada al portapapeles",
                                        "Información", JOptionPane.INFORMATION_MESSAGE);
        });

        buttonPanel.add(copyButton);
        buttonPanel.add(closeButton);
        panel.add(buttonPanel, BorderLayout.SOUTH);

        responseDialog.add(panel);
        responseDialog.setVisible(true);
    }

    private void showError(String message) {
        log("Mostrando error: " + message);
        JOptionPane.showMessageDialog(this, message, "Error", JOptionPane.ERROR_MESSAGE);
    }

    private void showSuccess(String message) {
        log("Mostrando éxito: " + message);
        JOptionPane.showMessageDialog(this, message, "Éxito", JOptionPane.INFORMATION_MESSAGE);
    }

    private String safeBody(String body) {
        if (body == null) return "<null>";
        return body.length() > 500 ? body.substring(0, 500) + "... (truncado)" : body;
    }

    private void log(String msg) {
        System.out.println("[MainBankingPanel] " + java.time.LocalDateTime.now() + " - " + msg);
    }
}
