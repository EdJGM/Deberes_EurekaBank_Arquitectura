package ec.edu.monster.client.view;

import ec.edu.monster.client.controller.EurekabankController;
import ec.edu.monster.client.model.ServerType;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

/**
 * Pantalla de selección de servidor que replica el diseño web de EUREKABANK
 */
public class ServerSelectionView extends JPanel {

    private final EurekabankController controller;
    private final CardLayout parentLayout;
    private final JPanel parentPanel;

    public ServerSelectionView(EurekabankController controller, CardLayout parentLayout, JPanel parentPanel) {
        this.controller = controller;
        this.parentLayout = parentLayout;
        this.parentPanel = parentPanel;
        initializeComponents();
    }

    private void initializeComponents() {
        setLayout(new BorderLayout());
        setBackground(new Color(240, 242, 247));

        // Header con título
        JPanel headerPanel = createHeaderPanel();
        add(headerPanel, BorderLayout.NORTH);

        // Panel central con las tarjetas de servidores
        JPanel centerPanel = createServerCardsPanel();
        add(centerPanel, BorderLayout.CENTER);
    }

    private JPanel createHeaderPanel() {
        JPanel headerPanel = new JPanel();
        headerPanel.setLayout(new BoxLayout(headerPanel, BoxLayout.Y_AXIS));
        headerPanel.setBackground(new Color(240, 242, 247));
        headerPanel.setBorder(BorderFactory.createEmptyBorder(40, 20, 20, 20));

        // Ícono del banco (simulado con rectángulos)
        JPanel iconPanel = createBankIcon();
        iconPanel.setAlignmentX(Component.CENTER_ALIGNMENT);
        headerPanel.add(iconPanel);

        headerPanel.add(Box.createVerticalStrut(20));

        // Título principal
        JLabel titleLabel = new JLabel("EurekaBank - Selección de Servidor");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 28));
        titleLabel.setForeground(new Color(24, 51, 122));
        titleLabel.setAlignmentX(Component.CENTER_ALIGNMENT);
        headerPanel.add(titleLabel);

        headerPanel.add(Box.createVerticalStrut(10));

        // Subtítulo
        JLabel subtitleLabel = new JLabel("Seleccione el servidor y protocolo que desea utilizar");
        subtitleLabel.setFont(new Font("Segoe UI", Font.PLAIN, 16));
        subtitleLabel.setForeground(new Color(108, 117, 125));
        subtitleLabel.setAlignmentX(Component.CENTER_ALIGNMENT);
        headerPanel.add(subtitleLabel);

        return headerPanel;
    }

    private JPanel createBankIcon() {
        JPanel iconPanel = new JPanel() {
            @Override
            protected void paintComponent(Graphics g) {
                super.paintComponent(g);
                Graphics2D g2d = (Graphics2D) g.create();
                g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

                // Dibujar dos rectángulos redondeados para simular el ícono del banco
                g2d.setColor(new Color(13, 110, 253));
                g2d.fillRoundRect(10, 5, 60, 15, 8, 8);
                g2d.fillRoundRect(10, 25, 60, 15, 8, 8);

                g2d.dispose();
            }
        };
        iconPanel.setPreferredSize(new Dimension(80, 50));
        iconPanel.setOpaque(false);
        return iconPanel;
    }

    private JPanel createServerCardsPanel() {
        JPanel mainPanel = new JPanel(new BorderLayout());
        mainPanel.setBackground(new Color(240, 242, 247));
        mainPanel.setBorder(BorderFactory.createEmptyBorder(20, 40, 40, 40));

        // Panel con grid de 2x2 para las tarjetas
        JPanel cardsPanel = new JPanel(new GridLayout(2, 2, 30, 30));
        cardsPanel.setBackground(new Color(240, 242, 247));

        // Crear tarjetas para cada servidor
        cardsPanel.add(createServerCard(
            "Java SOAP Server",
            "SOAP",
            "Java",
            "Servidor SOAP implementado en Java con JAX-WS",
            "http://10.40.15.218:8080/Eurobank_Soap_Java/EurekabankWS?wsdl",
            ServerType.SOAP_JAVA
        ));

        cardsPanel.add(createServerCard(
            ".NET SOAP Server",
            "SOAP",
            ".NET",
            "Servidor SOAP implementado en .NET con WCF",
            "http://10.40.15.218:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl",
            ServerType.SOAP_DOTNET
        ));

        cardsPanel.add(createServerCard(
            "Java RESTful Server",
            "REST",
            "Java",
            "API RESTful implementada en Java con JAX-RS",
            "http://10.40.15.218:8080/Eurobank_Restfull_Java/api/eureka",
            ServerType.REST_JAVA
        ));

        cardsPanel.add(createServerCard(
            ".NET RESTful Server",
            "REST",
            ".NET",
            "API RESTful implementada en .NET Core",
            "http://10.40.15.218:5111/api",
            ServerType.REST_DOTNET
        ));

        mainPanel.add(cardsPanel, BorderLayout.CENTER);

        // Botón cerrar en la esquina superior derecha
        JButton closeButton = new JButton("Close");
        closeButton.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        closeButton.setBackground(Color.WHITE);
        closeButton.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(206, 212, 218)),
            BorderFactory.createEmptyBorder(5, 15, 5, 15)
        ));
        closeButton.addActionListener(e -> System.exit(0));

        JPanel topPanel = new JPanel(new FlowLayout(FlowLayout.RIGHT));
        topPanel.setBackground(new Color(240, 242, 247));
        topPanel.add(closeButton);
        mainPanel.add(topPanel, BorderLayout.NORTH);

        return mainPanel;
    }

    private JPanel createServerCard(String title, String protocol, String technology,
                                   String description, String url, ServerType serverType) {
        JPanel card = new JPanel();
        card.setLayout(new BorderLayout());
        card.setBackground(Color.WHITE);
        card.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(222, 226, 230), 1),
            BorderFactory.createEmptyBorder(25, 25, 25, 25)
        ));

        // Radio button
        JRadioButton radioButton = new JRadioButton();
        radioButton.setBackground(Color.WHITE);
        radioButton.addActionListener(e -> {
            log("RadioButton seleccionado para servidor=" + serverType + ", title=" + title);
            selectServer(serverType);
        });

        // Panel superior con radio button y título
        JPanel topPanel = new JPanel(new BorderLayout());
        topPanel.setBackground(Color.WHITE);
        topPanel.add(radioButton, BorderLayout.WEST);

        JLabel titleLabel = new JLabel(title);
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 18));
        titleLabel.setForeground(new Color(33, 37, 41));
        titleLabel.setBorder(BorderFactory.createEmptyBorder(0, 10, 0, 0));
        topPanel.add(titleLabel, BorderLayout.CENTER);

        card.add(topPanel, BorderLayout.NORTH);

        // Panel central con etiquetas de protocolo y tecnología
        JPanel centerPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 0, 10));
        centerPanel.setBackground(Color.WHITE);

        // Etiqueta de protocolo
        JLabel protocolLabel = createTagLabel(protocol,
            protocol.equals("SOAP") ? new Color(111, 66, 193) : new Color(25, 135, 84));
        centerPanel.add(protocolLabel);

        centerPanel.add(Box.createHorizontalStrut(10));

        // Etiqueta de tecnología
        JLabel techLabel = createTagLabel(technology, new Color(13, 110, 253));
        centerPanel.add(techLabel);

        card.add(centerPanel, BorderLayout.CENTER);

        // Panel inferior con descripción y URL
        JPanel bottomPanel = new JPanel();
        bottomPanel.setLayout(new BoxLayout(bottomPanel, BoxLayout.Y_AXIS));
        bottomPanel.setBackground(Color.WHITE);
        bottomPanel.setBorder(BorderFactory.createEmptyBorder(10, 0, 0, 0));

        JLabel descLabel = new JLabel(description);
        descLabel.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        descLabel.setForeground(new Color(108, 117, 125));
        bottomPanel.add(descLabel);

        bottomPanel.add(Box.createVerticalStrut(15));

        JLabel urlLabel = new JLabel("<html><u>" + url + "</u></html>");
        urlLabel.setFont(new Font("Segoe UI", Font.PLAIN, 11));
        urlLabel.setForeground(new Color(108, 117, 125));
        bottomPanel.add(urlLabel);

        card.add(bottomPanel, BorderLayout.SOUTH);

        // Hacer toda la tarjeta clickeable
        card.addMouseListener(new java.awt.event.MouseAdapter() {
            @Override
            public void mouseClicked(java.awt.event.MouseEvent e) {
                log("Click en tarjeta servidor=" + serverType + ", title=" + title);
                radioButton.setSelected(true);
                selectServer(serverType);
            }

            @Override
            public void mouseEntered(java.awt.event.MouseEvent e) {
                log("Mouse enter tarjeta servidor=" + serverType);
                card.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(13, 110, 253), 2),
                    BorderFactory.createEmptyBorder(24, 24, 24, 24)
                ));
            }

            @Override
            public void mouseExited(java.awt.event.MouseEvent e) {
                log("Mouse exit tarjeta servidor=" + serverType);
                card.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(222, 226, 230), 1),
                    BorderFactory.createEmptyBorder(25, 25, 25, 25)
                ));
            }
        });

        return card;
    }

    private JLabel createTagLabel(String text, Color backgroundColor) {
        JLabel label = new JLabel(text);
        label.setFont(new Font("Segoe UI", Font.BOLD, 12));
        label.setForeground(Color.WHITE);
        label.setOpaque(true);
        label.setBackground(backgroundColor);
        label.setBorder(BorderFactory.createEmptyBorder(4, 8, 4, 8));
        return label;
    }

    private void selectServer(ServerType serverType) {
        log("Seleccionando servidor=" + serverType + " id=" + serverType.getId());
        controller.setServerType(serverType);
        log("Servidor establecido en controlador. Navegando a LOGIN");
        parentLayout.show(parentPanel, "LOGIN");
    }

    private void log(String msg) {
        System.out.println("[ServerSelectionView] " + java.time.LocalDateTime.now() + " - " + msg);
    }
}
