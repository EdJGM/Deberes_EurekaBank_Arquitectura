# Documentación para Cliente Java de Escritorio - EurekaBank

Esta documentación explica cómo implementar un cliente Java de escritorio que se conecte a todos los servidores EurekaBank (SOAP y REST, tanto Java como .NET).

## Tabla de Contenido

1. [Configuración General](#configuración-general)
2. [Cliente REST Java](#cliente-rest-java)
3. [Cliente REST .NET](#cliente-rest-net)
4. [Cliente SOAP Java](#cliente-soap-java)
5. [Cliente SOAP .NET](#cliente-soap-net)
6. [Modelos de Datos](#modelos-de-datos)
7. [Interfaz Gráfica - Replicar Apariencia Web](#interfaz-gráfica---replicar-apariencia-web)
8. [Ejemplo de Implementación Completa](#ejemplo-de-implementación-completa)

---

## Configuración General

### URLs de los Servidores

```java
public class ServerConfig {
    // Configuración de servidores
    public static final String SERVER_IP = "localhost"; // o "10.40.15.139"
    
    // URLs REST
    public static final String REST_JAVA_BASE_URL = 
        "http://" + SERVER_IP + ":8080/Eurobank_Restfull_Java/api/eureka";
    public static final String REST_DOTNET_BASE_URL = 
        "http://" + SERVER_IP + ":5000/api";
    
    // URLs SOAP (WSDL)
    public static final String SOAP_JAVA_WSDL_URL = 
        "http://" + SERVER_IP + ":8080/Eurobank_Soap_Java/EurekabankWS?wsdl";
    public static final String SOAP_DOTNET_WSDL_URL = 
        "http://" + SERVER_IP + ":57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl";
}
```

### Dependencias Maven Necesarias

```xml
<dependencies>
    <!-- Para clientes REST -->
    <dependency>
        <groupId>com.fasterxml.jackson.core</groupId>
        <artifactId>jackson-databind</artifactId>
        <version>2.15.2</version>
    </dependency>
    
    <!-- Para cliente HTTP -->
    <dependency>
        <groupId>org.apache.httpcomponents</groupId>
        <artifactId>httpclient</artifactId>
        <version>4.5.14</version>
    </dependency>
    
    <!-- Para clientes SOAP -->
    <dependency>
        <groupId>javax.xml.ws</groupId>
        <artifactId>jaxws-api</artifactId>
        <version>2.3.1</version>
    </dependency>
    
    <!-- Para generación de clientes SOAP -->
    <dependency>
        <groupId>com.sun.xml.ws</groupId>
        <artifactId>jaxws-rt</artifactId>
        <version>2.3.5</version>
    </dependency>
</dependencies>
```

---

## Cliente REST Java

### Características
- **Puerto:** 8080
- **Content-Type:** `application/json`
- **Parámetros:** Query parameters para transacciones
- **Respuestas:** JSON directo (strings o objetos simples)

### Implementación

```java
import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.http.client.methods.*;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;

public class RestJavaClient {
    private final CloseableHttpClient httpClient;
    private final ObjectMapper objectMapper;
    private final String baseUrl;
    
    public RestJavaClient() {
        this.httpClient = HttpClients.createDefault();
        this.objectMapper = new ObjectMapper();
        this.baseUrl = ServerConfig.REST_JAVA_BASE_URL;
    }
    
    // 1. Health Check
    public String health() throws Exception {
        HttpGet request = new HttpGet(baseUrl + "/health");
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        // Respuesta directa como string
        return response.replace("\"", ""); // Remover comillas JSON
    }
    
    // 2. Login
    public boolean login(String username, String password) throws Exception {
        HttpPost request = new HttpPost(baseUrl + "/login");
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        // Crear JSON para el cuerpo
        String json = String.format("{\"username\":\"%s\",\"password\":\"%s\"}", 
                                   username, password);
        request.setEntity(new StringEntity(json));
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        // Respuesta directa como boolean
        return Boolean.parseBoolean(response);
    }
    
    // 3. Obtener Movimientos
    public List<Movimiento> traerMovimientos(String cuenta) throws Exception {
        HttpGet request = new HttpGet(baseUrl + "/movimientos/" + cuenta);
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        // Respuesta como array JSON
        return objectMapper.readValue(response, 
            objectMapper.getTypeFactory().constructCollectionType(List.class, Movimiento.class));
    }
    
    // 4. Registrar Depósito
    public String regDeposito(String cuenta, double importe) throws Exception {
        String url = String.format("%s/deposito?cuenta=%s&importe=%.2f", 
                                  baseUrl, cuenta, importe);
        HttpPost request = new HttpPost(url);
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> {
            // El estado se determina por el código HTTP
            int statusCode = httpResponse.getStatusLine().getStatusCode();
            String body = EntityUtils.toString(httpResponse.getEntity());
            
            if (statusCode == 200) {
                return "1:" + body.replace("\"", ""); // Estado exitoso
            } else {
                return "-1:" + body.replace("\"", ""); // Estado error
            }
        });
        
        return response;
    }
    
    // 5. Registrar Retiro
    public String regRetiro(String cuenta, double importe) throws Exception {
        String url = String.format("%s/retiro?cuenta=%s&importe=%.2f", 
                                  baseUrl, cuenta, importe);
        HttpPost request = new HttpPost(url);
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> {
            int statusCode = httpResponse.getStatusLine().getStatusCode();
            String body = EntityUtils.toString(httpResponse.getEntity());
            
            if (statusCode == 200) {
                return "1:" + body.replace("\"", "");
            } else {
                return "-1:" + body.replace("\"", "");
            }
        });
        
        return response;
    }
    
    // 6. Registrar Transferencia
    public String regTransferencia(String cuentaOrigen, String cuentaDestino, double importe) throws Exception {
        String url = String.format("%s/transferencia?cuentaOrigen=%s&cuentaDestino=%s&importe=%.2f", 
                                  baseUrl, cuentaOrigen, cuentaDestino, importe);
        HttpPost request = new HttpPost(url);
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> {
            int statusCode = httpResponse.getStatusLine().getStatusCode();
            String body = EntityUtils.toString(httpResponse.getEntity());
            
            if (statusCode == 200) {
                return "1:" + body.replace("\"", "");
            } else {
                return "-1:" + body.replace("\"", "");
            }
        });
        
        return response;
    }
}
```

---

## Cliente REST .NET

### Características
- **Puerto:** 5000
- **Content-Type:** `application/json`
- **Parámetros:** JSON en el cuerpo para todas las operaciones POST
- **Respuestas:** Estructura estándar con `success`, `message`, `data`

### Implementación

```java
public class RestDotNetClient {
    private final CloseableHttpClient httpClient;
    private final ObjectMapper objectMapper;
    private final String baseUrl;
    
    public RestDotNetClient() {
        this.httpClient = HttpClients.createDefault();
        this.objectMapper = new ObjectMapper();
        this.baseUrl = ServerConfig.REST_DOTNET_BASE_URL;
    }
    
    // Clase para manejar respuestas estándar .NET
    public static class DotNetResponse<T> {
        public boolean success;
        public String message;
        public T data;
    }
    
    // 1. Health Check
    public String health() throws Exception {
        HttpGet request = new HttpGet(baseUrl + "/Health");
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<Map<String, Object>> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<Map<String, Object>>>() {});
        
        if (result.success && result.data != null) {
            return (String) result.data.get("status");
        }
        
        return "Error: " + result.message;
    }
    
    // 2. Login
    public boolean login(String username, String password) throws Exception {
        HttpPost request = new HttpPost(baseUrl + "/Auth/login");
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String json = String.format("{\"username\":\"%s\",\"password\":\"%s\"}", 
                                   username, password);
        request.setEntity(new StringEntity(json));
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<Map<String, Object>> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<Map<String, Object>>>() {});
        
        if (result.success && result.data != null) {
            return (Boolean) result.data.get("authenticated");
        }
        
        return false;
    }
    
    // 3. Obtener Movimientos
    public List<Movimiento> traerMovimientos(String cuenta) throws Exception {
        HttpGet request = new HttpGet(baseUrl + "/Movimientos/cuenta/" + cuenta);
        request.setHeader("Accept", "application/json");
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<List<Movimiento>> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<List<Movimiento>>>() {});
        
        if (result.success && result.data != null) {
            return result.data;
        }
        
        return new ArrayList<>();
    }
    
    // 4. Registrar Depósito
    public int regDeposito(String cuenta, double importe) throws Exception {
        HttpPost request = new HttpPost(baseUrl + "/Movimientos/deposito");
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String json = String.format("{\"cuenta\":\"%s\",\"importe\":%.2f}", 
                                   cuenta, importe);
        request.setEntity(new StringEntity(json));
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<Object> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<Object>>() {});
        
        return result.success ? 1 : -1;
    }
    
    // 5. Registrar Retiro
    public int regRetiro(String cuenta, double importe) throws Exception {
        HttpPost request = new HttpPost(baseUrl + "/Movimientos/retiro");
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String json = String.format("{\"cuenta\":\"%s\",\"importe\":%.2f}", 
                                   cuenta, importe);
        request.setEntity(new StringEntity(json));
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<Object> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<Object>>() {});
        
        return result.success ? 1 : -1;
    }
    
    // 6. Registrar Transferencia
    public int regTransferencia(String cuentaOrigen, String cuentaDestino, double importe) throws Exception {
        HttpPost request = new HttpPost(baseUrl + "/Movimientos/transferencia");
        request.setHeader("Content-Type", "application/json");
        request.setHeader("Accept", "application/json");
        
        String json = String.format("{\"cuentaOrigen\":\"%s\",\"cuentaDestino\":\"%s\",\"importe\":%.2f}", 
                                   cuentaOrigen, cuentaDestino, importe);
        request.setEntity(new StringEntity(json));
        
        String response = httpClient.execute(request, httpResponse -> 
            EntityUtils.toString(httpResponse.getEntity()));
        
        DotNetResponse<Object> result = objectMapper.readValue(response, 
            new TypeReference<DotNetResponse<Object>>() {});
        
        return result.success ? 1 : -1;
    }
}
```

---

## Cliente SOAP Java

### Características
- **Puerto:** 8080
- **Namespace:** `http://ws.monster.edu.ec/`
- **Métodos:** Nombres en minúsculas (login, health, traerMovimientos, etc.)
- **Respuestas:** Objetos Java generados desde WSDL

### Generación del Cliente

```bash
# Generar clases Java desde WSDL
wsimport -keep -verbose http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl
```

### Implementación

```java
// Importar clases generadas (ejemplo de nombres)
import ec.monster.edu.ws.EurekabankWS;
import ec.monster.edu.ws.EurekabankWSService;
import ec.monster.edu.ws.Movimiento;

public class SoapJavaClient {
    private final EurekabankWS port;
    
    public SoapJavaClient() {
        EurekabankWSService service = new EurekabankWSService();
        this.port = service.getEurekabankWSPort();
        
        // Configurar endpoint si es diferente
        BindingProvider bp = (BindingProvider) port;
        bp.getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, 
            "http://localhost:8080/Eurobank_Soap_Java/EurekabankWS");
    }
    
    // 1. Health Check
    public String health() {
        return port.health();
    }
    
    // 2. Login
    public boolean login(String username, String password) {
        return port.login(username, password);
    }
    
    // 3. Obtener Movimientos
    public List<Movimiento> traerMovimientos(String cuenta) {
        return port.traerMovimientos(cuenta);
    }
    
    // 4. Registrar Depósito
    public int regDeposito(String cuenta, double importe) {
        return port.regDeposito(cuenta, importe);
    }
    
    // 5. Registrar Retiro
    public int regRetiro(String cuenta, double importe) {
        return port.regRetiro(cuenta, importe);
    }
    
    // 6. Registrar Transferencia
    public int regTransferencia(String cuentaOrigen, String cuentaDestino, double importe) {
        return port.regTransferencia(cuentaOrigen, cuentaDestino, importe);
    }
}
```

---

## Cliente SOAP .NET

### Características
- **Puerto:** 57199
- **Namespace:** `http://tempuri.org/`
- **Métodos:** Nombres con mayúscula inicial (Login, Health, ObtenerPorCuenta, etc.)
- **Headers requeridos:** SOAPAction específico para cada operación
- **Respuestas:** Requieren normalización de campos

### Generación del Cliente

```bash
# Generar clases Java desde WSDL .NET
wsimport -keep -verbose http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl
```

### Implementación con Headers Personalizados

```java
import javax.xml.ws.BindingProvider;
import javax.xml.ws.handler.MessageContext;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SoapDotNetClient {
    private final IEurekabankWS port; // Interfaz generada
    
    public SoapDotNetClient() {
        EurekabankWS service = new EurekabankWS(); // Clase de servicio generada
        this.port = service.getBasicHttpBindingIEurekabankWS();
        
        // Configurar endpoint
        BindingProvider bp = (BindingProvider) port;
        bp.getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, 
            "http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc");
    }
    
    // Método auxiliar para establecer SOAPAction
    private void setSOAPAction(String action) {
        BindingProvider bp = (BindingProvider) port;
        Map<String, Object> requestContext = bp.getRequestContext();
        
        Map<String, List<String>> headers = new HashMap<>();
        headers.put("Content-Type", Collections.singletonList("text/xml; charset=utf-8"));
        headers.put("SOAPAction", Collections.singletonList("\"" + action + "\""));
        
        requestContext.put(MessageContext.HTTP_REQUEST_HEADERS, headers);
    }
    
    // 1. Health Check
    public String health() {
        setSOAPAction("http://tempuri.org/IEurekabankWS/Health");
        return port.health();
    }
    
    // 2. Login
    public boolean login(String username, String password) {
        setSOAPAction("http://tempuri.org/IEurekabankWS/Login");
        return port.login(username, password);
    }
    
    // 3. Obtener Movimientos - IMPORTANTE: Usar ObtenerPorCuenta
    public List<MovimientoDotNet> traerMovimientos(String cuenta) {
        setSOAPAction("http://tempuri.org/IEurekabankWS/ObtenerPorCuenta");
        
        // Llamar al método .NET (nombre diferente)
        ArrayOfMovimiento resultado = port.obtenerPorCuenta(cuenta);
        
        // Normalizar campos (mayúsculas → minúsculas)
        List<MovimientoDotNet> movimientos = new ArrayList<>();
        if (resultado != null && resultado.getMovimiento() != null) {
            for (MovimientoDotNet mov : resultado.getMovimiento()) {
                // Los campos ya vienen normalizados desde el cliente generado
                // pero si es necesario, hacer conversiones aquí
                movimientos.add(mov);
            }
        }
        
        return movimientos;
    }
    
    // 4. Registrar Depósito
    public int regDeposito(String cuenta, double importe) {
        setSOAPAction("http://tempuri.org/IEurekabankWS/RegistrarDeposito");
        
        String resultado = port.registrarDeposito(cuenta, importe);
        
        // Convertir string a int (el servidor .NET devuelve "1" o "-1")
        try {
            return Integer.parseInt(resultado);
        } catch (NumberFormatException e) {
            return -1;
        }
    }
    
    // 5. Registrar Retiro
    public int regRetiro(String cuenta, double importe) {
        setSOAPAction("http://tempuri.org/IEurekabankWS/RegistrarRetiro");
        
        String resultado = port.registrarRetiro(cuenta, importe);
        
        try {
            return Integer.parseInt(resultado);
        } catch (NumberFormatException e) {
            return -1;
        }
    }
    
    // 6. Registrar Transferencia
    public int regTransferencia(String cuentaOrigen, String cuentaDestino, double importe) {
        setSOAPAction("http://tempuri.org/IEurekabankWS/RegistrarTransferencia");
        
        String resultado = port.registrarTransferencia(cuentaOrigen, cuentaDestino, importe);
        
        try {
            return Integer.parseInt(resultado);
        } catch (NumberFormatException e) {
            return -1;
        }
    }
}
```

---

## Modelos de Datos

### Clase Movimiento (Unificada)

```java
import com.fasterxml.jackson.annotation.JsonProperty;
import java.time.LocalDateTime;

public class Movimiento {
    @JsonProperty("cuenta")
    private String cuenta;
    
    @JsonProperty("nromov") 
    @JsonProperty("nroMov") // Para compatibilidad con .NET
    private int nromov;
    
    @JsonProperty("fecha")
    private String fecha; // O LocalDateTime si prefieres parsear
    
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
```

### Enumeración de Tipos de Servidor

```java
public enum ServerType {
    REST_JAVA("REST Java", ServerConfig.REST_JAVA_BASE_URL),
    REST_DOTNET("REST .NET", ServerConfig.REST_DOTNET_BASE_URL),
    SOAP_JAVA("SOAP Java", ServerConfig.SOAP_JAVA_WSDL_URL),
    SOAP_DOTNET("SOAP .NET", ServerConfig.SOAP_DOTNET_WSDL_URL);
    
    private final String displayName;
    private final String url;
    
    ServerType(String displayName, String url) {
        this.displayName = displayName;
        this.url = url;
    }
    
    public String getDisplayName() { return displayName; }
    public String getUrl() { return url; }
    
    public boolean isSOAP() {
        return this == SOAP_JAVA || this == SOAP_DOTNET;
    }
    
    public boolean isREST() {
        return this == REST_JAVA || this == REST_DOTNET;
    }
}
```

---

## Interfaz Gráfica - Replicar Apariencia Web

### Dependencias Adicionales para UI

Agregar a `pom.xml`:

```xml
<dependencies>
    <!-- FlatLaf - Look and Feel moderno -->
    <dependency>
        <groupId>com.formdev</groupId>
        <artifactId>flatlaf</artifactId>
        <version>3.2.5</version>
    </dependency>
    
    <!-- MigLayout - Layout manager flexible -->
    <dependency>
        <groupId>com.miglayout</groupId>
        <artifactId>miglayout-swing</artifactId>
        <version>11.3</version>
    </dependency>
    
    <!-- JGoodies Forms - Para formularios elegantes -->
    <dependency>
        <groupId>com.jgoodies</groupId>
        <artifactId>jgoodies-forms</artifactId>
        <version>1.9.0</version>
    </dependency>
</dependencies>
```

### Configuración del Look and Feel

```java
import com.formdev.flatlaf.FlatLightLaf;
import com.formdev.flatlaf.themes.FlatMacLightLaf;

public class UIConfiguration {
    public static void setupLookAndFeel() {
        try {
            // Usar FlatLaf para apariencia moderna similar a la web
            FlatLightLaf.setup();
            
            // Configurar colores personalizados para EurekaBank
            UIManager.put("Button.arc", 8); // Botones redondeados
            UIManager.put("Component.arc", 8); // Componentes redondeados
            UIManager.put("TextComponent.arc", 8); // Campos de texto redondeados
            
            // Colores del tema EurekaBank (azul)
            UIManager.put("Button.background", new Color(59, 130, 246)); // blue-500
            UIManager.put("Button.foreground", Color.WHITE);
            UIManager.put("Button.hoverBackground", new Color(37, 99, 235)); // blue-600
            UIManager.put("Button.pressedBackground", new Color(29, 78, 216)); // blue-700
            
            // Panel de fondo gradiente
            UIManager.put("Panel.background", new Color(239, 246, 255)); // blue-50
            
            // Campos de entrada
            UIManager.put("TextField.background", Color.WHITE);
            UIManager.put("TextField.border", BorderFactory.createLineBorder(new Color(209, 213, 219))); // gray-300
            UIManager.put("TextField.focusedBorder", BorderFactory.createLineBorder(new Color(59, 130, 246))); // blue-500
            
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
```

### Pantalla de Selección de Servidor

```java
import javax.swing.*;
import net.miginfocom.swing.MigLayout;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class ServerSelectionPanel extends JPanel {
    private ServerType selectedServerType;
    private ActionListener onServerSelected;
    
    public ServerSelectionPanel(ActionListener onServerSelected) {
        this.onServerSelected = onServerSelected;
        initializeComponents();
        setupLayout();
        applyWebStyling();
    }
    
    private void initializeComponents() {
        setLayout(new MigLayout("fill, insets 20", "[center]", "[center]"));
        
        // Panel principal con sombra y bordes redondeados
        JPanel mainCard = new JPanel(new MigLayout("fill, insets 40", "[center]", "[]20[]30[]20[]"));
        mainCard.setBackground(Color.WHITE);
        mainCard.setBorder(createCardBorder());
        
        // Logo/Título
        JLabel titleLabel = new JLabel("EurekaBank");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 32));
        titleLabel.setForeground(new Color(30, 58, 138)); // blue-900
        titleLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        JLabel subtitleLabel = new JLabel("Seleccione el tipo de servidor");
        subtitleLabel.setFont(new Font("Segoe UI", Font.PLAIN, 16));
        subtitleLabel.setForeground(new Color(107, 114, 128)); // gray-500
        subtitleLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        // Panel de botones de servidor
        JPanel serverButtonsPanel = new JPanel(new MigLayout("fill", "[]10[]", "[]10[]"));
        serverButtonsPanel.setBackground(Color.WHITE);
        
        // Crear botones para cada tipo de servidor
        for (ServerType serverType : ServerType.values()) {
            JButton serverButton = createServerButton(serverType);
            serverButtonsPanel.add(serverButton, "growx, width 200");
            
            if (serverType == ServerType.REST_JAVA || serverType == ServerType.SOAP_JAVA) {
                serverButtonsPanel.add("wrap"); // Nueva fila después de los primeros dos
            }
        }
        
        // Agregar componentes al card principal
        mainCard.add(titleLabel, "wrap");
        mainCard.add(subtitleLabel, "wrap");
        mainCard.add(serverButtonsPanel, "wrap");
        
        add(mainCard, "width 500, height 400");
    }
    
    private JButton createServerButton(ServerType serverType) {
        JButton button = new JButton();
        button.setLayout(new MigLayout("fill, insets 15", "[center]", "[]5[]"));
        
        // Icono y texto del servidor
        String iconText = getServerIcon(serverType);
        String description = getServerDescription(serverType);
        
        JLabel iconLabel = new JLabel(iconText);
        iconLabel.setFont(new Font("Segoe UI", Font.BOLD, 20));
        iconLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        JLabel nameLabel = new JLabel(serverType.getDisplayName());
        nameLabel.setFont(new Font("Segoe UI", Font.BOLD, 14));
        nameLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        JLabel descLabel = new JLabel("<html><center>" + description + "</center></html>");
        descLabel.setFont(new Font("Segoe UI", Font.PLAIN, 11));
        descLabel.setForeground(new Color(107, 114, 128));
        descLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        button.add(iconLabel, "wrap");
        button.add(nameLabel, "wrap");
        button.add(descLabel, "wrap");
        
        // Styling del botón
        button.setBackground(Color.WHITE);
        button.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
            BorderFactory.createEmptyBorder(10, 10, 10, 10)
        ));
        button.setFocusPainted(false);
        button.setCursor(new Cursor(Cursor.HAND_CURSOR));
        
        // Efectos hover
        button.addMouseListener(new java.awt.event.MouseAdapter() {
            public void mouseEntered(java.awt.event.MouseEvent evt) {
                button.setBackground(new Color(239, 246, 255)); // blue-50
                button.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(59, 130, 246), 2, true),
                    BorderFactory.createEmptyBorder(9, 9, 9, 9)
                ));
            }
            
            public void mouseExited(java.awt.event.MouseEvent evt) {
                button.setBackground(Color.WHITE);
                button.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
                    BorderFactory.createEmptyBorder(10, 10, 10, 10)
                ));
            }
        });
        
        button.addActionListener(e -> {
            selectedServerType = serverType;
            onServerSelected.actionPerformed(new ActionEvent(this, 0, serverType.name()));
        });
        
        return button;
    }
    
    private String getServerIcon(ServerType serverType) {
        switch (serverType) {
            case REST_JAVA: return "☕";
            case REST_DOTNET: return "🔷";
            case SOAP_JAVA: return "📡";
            case SOAP_DOTNET: return "🌐";
            default: return "🖥️";
        }
    }
    
    private String getServerDescription(ServerType serverType) {
        switch (serverType) {
            case REST_JAVA: return "API RESTful Java<br>JAX-RS + JSON";
            case REST_DOTNET: return "API RESTful .NET<br>ASP.NET Core";
            case SOAP_JAVA: return "Web Service Java<br>JAX-WS + XML";
            case SOAP_DOTNET: return "Web Service .NET<br>WCF + XML";
            default: return "Servidor";
        }
    }
    
    private void setupLayout() {
        // El layout ya está configurado en initializeComponents
    }
    
    private void applyWebStyling() {
        setBackground(new Color(243, 244, 246)); // gray-100 (fondo general)
    }
    
    private Border createCardBorder() {
        return BorderFactory.createCompoundBorder(
            // Sombra simulada con borde gris claro
            BorderFactory.createCompoundBorder(
                BorderFactory.createMatteBorder(0, 0, 3, 3, new Color(0, 0, 0, 20)),
                BorderFactory.createMatteBorder(0, 0, 1, 1, new Color(0, 0, 0, 40))
            ),
            // Borde redondeado blanco
            BorderFactory.createLineBorder(Color.WHITE, 1, true)
        );
    }
    
    public ServerType getSelectedServerType() {
        return selectedServerType;
    }
}
```

### Pantalla de Login

```java
public class LoginPanel extends JPanel {
    private ServerType serverType;
    private JTextField usernameField;
    private JPasswordField passwordField;
    private JButton loginButton;
    private JButton backButton;
    private JLabel errorLabel;
    private ActionListener onLoginSuccess;
    private ActionListener onBack;
    
    public LoginPanel(ServerType serverType, ActionListener onLoginSuccess, ActionListener onBack) {
        this.serverType = serverType;
        this.onLoginSuccess = onLoginSuccess;
        this.onBack = onBack;
        initializeComponents();
    }
    
    private void initializeComponents() {
        setLayout(new MigLayout("fill, insets 20", "[center]", "[center]"));
        setBackground(new Color(243, 244, 246)); // gray-100
        
        // Panel principal (card)
        JPanel loginCard = new JPanel(new MigLayout("fill, insets 40", "[grow,fill]", "[]20[]30[]10[]10[]20[]10[]20[]"));
        loginCard.setBackground(Color.WHITE);
        loginCard.setBorder(createCardBorder());
        loginCard.setMaximumSize(new Dimension(400, 500));
        
        // Header
        JLabel titleLabel = new JLabel("EurekaBank");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 28));
        titleLabel.setForeground(new Color(30, 58, 138)); // blue-900
        titleLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        JLabel subtitleLabel = new JLabel("Ingrese sus credenciales para acceder");
        subtitleLabel.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        subtitleLabel.setForeground(new Color(107, 114, 128)); // gray-500
        subtitleLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        // Server info panel
        JPanel serverInfoPanel = createServerInfoPanel();
        
        // Username field
        JLabel usernameLabel = new JLabel("Usuario");
        usernameLabel.setFont(new Font("Segoe UI", Font.MEDIUM, 12));
        usernameField = createStyledTextField("Ingrese su usuario");
        
        // Password field
        JLabel passwordLabel = new JLabel("Contraseña");
        passwordLabel.setFont(new Font("Segoe UI", Font.MEDIUM, 12));
        passwordField = createStyledPasswordField("Ingrese su contraseña");
        
        // Error label
        errorLabel = new JLabel();
        errorLabel.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        errorLabel.setForeground(new Color(239, 68, 68)); // red-500
        errorLabel.setHorizontalAlignment(SwingConstants.CENTER);
        errorLabel.setVisible(false);
        
        // Buttons
        loginButton = createStyledButton("Iniciar Sesión", true);
        loginButton.addActionListener(this::handleLogin);
        
        backButton = createStyledButton("Cambiar Servidor", false);
        backButton.addActionListener(onBack);
        
        // Add components to card
        loginCard.add(titleLabel, "wrap");
        loginCard.add(subtitleLabel, "wrap");
        loginCard.add(serverInfoPanel, "wrap");
        loginCard.add(usernameLabel, "wrap");
        loginCard.add(usernameField, "height 40, wrap");
        loginCard.add(passwordLabel, "wrap");
        loginCard.add(passwordField, "height 40, wrap");
        loginCard.add(errorLabel, "wrap");
        loginCard.add(loginButton, "height 40, wrap");
        loginCard.add(backButton, "height 35, wrap");
        
        add(loginCard, "width 400, height 550");
    }
    
    private JPanel createServerInfoPanel() {
        JPanel panel = new JPanel(new MigLayout("fill, insets 12", "[]", "[]"));
        panel.setBackground(new Color(239, 246, 255)); // blue-50
        panel.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(219, 234, 254), 1, true),
            BorderFactory.createEmptyBorder(0, 0, 0, 0)
        ));
        
        JLabel serverLabel = new JLabel(serverType.getDisplayName());
        serverLabel.setFont(new Font("Segoe UI", Font.BOLD, 12));
        serverLabel.setForeground(new Color(30, 58, 138)); // blue-900
        serverLabel.setHorizontalAlignment(SwingConstants.CENTER);
        
        panel.add(serverLabel, "center");
        return panel;
    }
    
    private JTextField createStyledTextField(String placeholder) {
        JTextField field = new JTextField();
        field.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        field.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
            BorderFactory.createEmptyBorder(8, 12, 8, 12)
        ));
        
        // Placeholder effect
        field.setForeground(new Color(156, 163, 175)); // gray-400
        field.setText(placeholder);
        
        field.addFocusListener(new java.awt.event.FocusAdapter() {
            public void focusGained(java.awt.event.FocusEvent evt) {
                if (field.getText().equals(placeholder)) {
                    field.setText("");
                    field.setForeground(Color.BLACK);
                }
                field.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(59, 130, 246), 2, true),
                    BorderFactory.createEmptyBorder(7, 11, 7, 11)
                ));
            }
            
            public void focusLost(java.awt.event.FocusEvent evt) {
                if (field.getText().isEmpty()) {
                    field.setForeground(new Color(156, 163, 175));
                    field.setText(placeholder);
                }
                field.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
                    BorderFactory.createEmptyBorder(8, 12, 8, 12)
                ));
            }
        });
        
        return field;
    }
    
    private JPasswordField createStyledPasswordField(String placeholder) {
        JPasswordField field = new JPasswordField();
        field.setFont(new Font("Segoe UI", Font.PLAIN, 14));
        field.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
            BorderFactory.createEmptyBorder(8, 12, 8, 12)
        ));
        
        // Focus effects similar to text field
        field.addFocusListener(new java.awt.event.FocusAdapter() {
            public void focusGained(java.awt.event.FocusEvent evt) {
                field.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(59, 130, 246), 2, true),
                    BorderFactory.createEmptyBorder(7, 11, 7, 11)
                ));
            }
            
            public void focusLost(java.awt.event.FocusEvent evt) {
                field.setBorder(BorderFactory.createCompoundBorder(
                    BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
                    BorderFactory.createEmptyBorder(8, 12, 8, 12)
                ));
            }
        });
        
        return field;
    }
    
    private JButton createStyledButton(String text, boolean isPrimary) {
        JButton button = new JButton(text);
        button.setFont(new Font("Segoe UI", Font.BOLD, 14));
        button.setFocusPainted(false);
        button.setCursor(new Cursor(Cursor.HAND_CURSOR));
        
        if (isPrimary) {
            button.setBackground(new Color(59, 130, 246)); // blue-500
            button.setForeground(Color.WHITE);
            button.setBorder(BorderFactory.createEmptyBorder(10, 20, 10, 20));
            
            button.addMouseListener(new java.awt.event.MouseAdapter() {
                public void mouseEntered(java.awt.event.MouseEvent evt) {
                    button.setBackground(new Color(37, 99, 235)); // blue-600
                }
                
                public void mouseExited(java.awt.event.MouseEvent evt) {
                    button.setBackground(new Color(59, 130, 246)); // blue-500
                }
            });
        } else {
            button.setBackground(Color.WHITE);
            button.setForeground(new Color(107, 114, 128)); // gray-500
            button.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(new Color(209, 213, 219), 1, true),
                BorderFactory.createEmptyBorder(9, 19, 9, 19)
            ));
            
            button.addMouseListener(new java.awt.event.MouseAdapter() {
                public void mouseEntered(java.awt.event.MouseEvent evt) {
                    button.setBackground(new Color(249, 250, 251)); // gray-50
                }
                
                public void mouseExited(java.awt.event.MouseEvent evt) {
                    button.setBackground(Color.WHITE);
                }
            });
        }
        
        return button;
    }
    
    private void handleLogin(ActionEvent e) {
        String username = usernameField.getText();
        String password = new String(passwordField.getPassword());
        
        // Validar campos no vacíos y no placeholders
        if (username.isEmpty() || username.equals("Ingrese su usuario") || 
            password.isEmpty()) {
            showError("Por favor complete todos los campos");
            return;
        }
        
        // Cambiar botón a estado de carga
        loginButton.setText("Iniciando sesión...");
        loginButton.setEnabled(false);
        errorLabel.setVisible(false);
        
        // Ejecutar login en hilo separado para no bloquear UI
        SwingWorker<Boolean, Void> worker = new SwingWorker<Boolean, Void>() {
            @Override
            protected Boolean doInBackground() throws Exception {
                EurekaBankClient client = EurekaBankClientFactory.createClient(serverType);
                return client.login(username, password);
            }
            
            @Override
            protected void done() {
                try {
                    boolean loginSuccess = get();
                    
                    if (loginSuccess) {
                        // Login exitoso
                        ActionEvent successEvent = new ActionEvent(LoginPanel.this, 0, username);
                        onLoginSuccess.actionPerformed(successEvent);
                    } else {
                        showError("Credenciales inválidas. Por favor, intente nuevamente.");
                    }
                } catch (Exception ex) {
                    showError("Error de conexión: " + ex.getMessage());
                }
                
                // Restaurar botón
                loginButton.setText("Iniciar Sesión");
                loginButton.setEnabled(true);
            }
        };
        
        worker.execute();
    }
    
    private void showError(String message) {
        errorLabel.setText("<html><center>" + message + "</center></html>");
        errorLabel.setVisible(true);
        
        // Animar error (opcional)
        Timer timer = new Timer(3000, e -> errorLabel.setVisible(false));
        timer.setRepeats(false);
        timer.start();
    }
    
    private Border createCardBorder() {
        return BorderFactory.createCompoundBorder(
            BorderFactory.createCompoundBorder(
                BorderFactory.createMatteBorder(0, 0, 3, 3, new Color(0, 0, 0, 20)),
                BorderFactory.createMatteBorder(0, 0, 1, 1, new Color(0, 0, 0, 40))
            ),
            BorderFactory.createLineBorder(Color.WHITE, 1, true)
        );
    }
}
```

### Dashboard Principal

```java
public class DashboardPanel extends JPanel {
    private ServerType serverType;
    private String currentUser;
    private EurekaBankClient client;
    private JTabbedPane tabbedPane;
    
    public DashboardPanel(ServerType serverType, String currentUser) {
        this.serverType = serverType;
        this.currentUser = currentUser;
        this.client = EurekaBankClientFactory.createClient(serverType);
        initializeComponents();
    }
    
    private void initializeComponents() {
        setLayout(new MigLayout("fill, insets 0", "[grow,fill]", "[60][grow,fill]"));
        setBackground(new Color(243, 244, 246)); // gray-100
        
        // Header
        JPanel headerPanel = createHeaderPanel();
        
        // Tabbed pane con las funcionalidades
        tabbedPane = createStyledTabbedPane();
        
        add(headerPanel, "dock north, height 60");
        add(tabbedPane, "grow");
    }
    
    private JPanel createHeaderPanel() {
        JPanel header = new JPanel(new MigLayout("fill, insets 15", "[]push[]", "[center]"));
        header.setBackground(Color.WHITE);
        header.setBorder(BorderFactory.createMatteBorder(0, 0, 1, 0, new Color(229, 231, 235))); // gray-200
        
        // Logo y título
        JLabel titleLabel = new JLabel("EurekaBank Dashboard");
        titleLabel.setFont(new Font("Segoe UI", Font.BOLD, 18));
        titleLabel.setForeground(new Color(30, 58, 138)); // blue-900
        
        // Info del usuario y servidor
        JPanel userInfoPanel = new JPanel(new MigLayout("insets 0", "[]5[]5[]", "[center]"));
        userInfoPanel.setBackground(Color.WHITE);
        
        JLabel userLabel = new JLabel("Usuario: " + currentUser);
        userLabel.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        userLabel.setForeground(new Color(107, 114, 128)); // gray-500
        
        JLabel separatorLabel = new JLabel("|");
        separatorLabel.setForeground(new Color(209, 213, 219)); // gray-300
        
        JLabel serverLabel = new JLabel(serverType.getDisplayName());
        serverLabel.setFont(new Font("Segoe UI", Font.BOLD, 12));
        serverLabel.setForeground(new Color(59, 130, 246)); // blue-500
        
        userInfoPanel.add(userLabel);
        userInfoPanel.add(separatorLabel);
        userInfoPanel.add(serverLabel);
        
        header.add(titleLabel);
        header.add(userInfoPanel);
        
        return header;
    }
    
    private JTabbedPane createStyledTabbedPane() {
        JTabbedPane tabs = new JTabbedPane();
        tabs.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        tabs.setBackground(Color.WHITE);
        
        // Pestaña de Movimientos
        tabs.addTab("📋 Movimientos", createMovimientosPanel());
        
        // Pestaña de Depósitos
        tabs.addTab("💰 Depósitos", createDepositoPanel());
        
        // Pestaña de Retiros
        tabs.addTab("💸 Retiros", createRetiroPanel());
        
        // Pestaña de Transferencias
        tabs.addTab("🔄 Transferencias", createTransferenciaPanel());
        
        return tabs;
    }
    
    private JPanel createMovimientosPanel() {
        JPanel panel = new JPanel(new MigLayout("fill, insets 20", "[grow,fill]", "[]20[grow,fill]"));
        panel.setBackground(new Color(243, 244, 246)); // gray-100
        
        // Card de búsqueda
        JPanel searchCard = new JPanel(new MigLayout("fill, insets 20", "[]10[grow,fill]10[]", "[center]"));
        searchCard.setBackground(Color.WHITE);
        searchCard.setBorder(createCardBorder());
        
        JLabel searchLabel = new JLabel("Número de cuenta:");
        JTextField cuentaField = createStyledTextField("Ej: 00100001");
        JButton searchButton = createStyledButton("Consultar", true);
        
        searchCard.add(searchLabel);
        searchCard.add(cuentaField);
        searchCard.add(searchButton);
        
        // Table para mostrar movimientos
        String[] columns = {"Mov#", "Fecha", "Tipo", "Acción", "Importe"};
        DefaultTableModel tableModel = new DefaultTableModel(columns, 0) {
            @Override
            public boolean isCellEditable(int row, int column) {
                return false;
            }
        };
        
        JTable movimientosTable = new JTable(tableModel);
        styleTable(movimientosTable);
        
        JScrollPane scrollPane = new JScrollPane(movimientosTable);
        scrollPane.setBorder(createCardBorder());
        scrollPane.getViewport().setBackground(Color.WHITE);
        
        // Action listener para búsqueda
        ActionListener searchAction = e -> {
            String cuenta = cuentaField.getText().trim();
            if (!cuenta.isEmpty() && !cuenta.equals("Ej: 00100001")) {
                loadMovimientos(cuenta, tableModel);
            }
        };
        
        searchButton.addActionListener(searchAction);
        cuentaField.addActionListener(searchAction);
        
        panel.add(searchCard, "wrap");
        panel.add(scrollPane, "grow");
        
        return panel;
    }
    
    // Métodos similares para createDepositoPanel(), createRetiroPanel(), createTransferenciaPanel()
    // ... (implementación similar con formularios estilizados)
    
    private void styleTable(JTable table) {
        table.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        table.setRowHeight(35);
        table.setGridColor(new Color(229, 231, 235)); // gray-200
        table.setSelectionBackground(new Color(239, 246, 255)); // blue-50
        table.setSelectionForeground(new Color(30, 58, 138)); // blue-900
        
        // Header styling
        table.getTableHeader().setFont(new Font("Segoe UI", Font.BOLD, 12));
        table.getTableHeader().setBackground(new Color(249, 250, 251)); // gray-50
        table.getTableHeader().setForeground(new Color(55, 65, 81)); // gray-700
        table.getTableHeader().setBorder(BorderFactory.createMatteBorder(0, 0, 1, 0, new Color(229, 231, 235)));
    }
    
    private void loadMovimientos(String cuenta, DefaultTableModel tableModel) {
        // Limpiar tabla
        tableModel.setRowCount(0);
        
        // Cargar en hilo separado
        SwingWorker<List<Movimiento>, Void> worker = new SwingWorker<List<Movimiento>, Void>() {
            @Override
            protected List<Movimiento> doInBackground() throws Exception {
                return client.traerMovimientos(cuenta);
            }
            
            @Override
            protected void done() {
                try {
                    List<Movimiento> movimientos = get();
                    
                    for (Movimiento mov : movimientos) {
                        Object[] row = {
                            mov.getNromov(),
                            mov.getFecha(),
                            mov.getTipo(),
                            mov.getAccion(),
                            String.format("$%.2f", mov.getImporte())
                        };
                        tableModel.addRow(row);
                    }
                    
                    if (movimientos.isEmpty()) {
                        Object[] emptyRow = {"", "", "No se encontraron movimientos", "", ""};
                        tableModel.addRow(emptyRow);
                    }
                    
                } catch (Exception ex) {
                    Object[] errorRow = {"", "", "Error: " + ex.getMessage(), "", ""};
                    tableModel.addRow(errorRow);
                }
            }
        };
        
        worker.execute();
    }
    
    // Métodos helper reutilizados de LoginPanel
    private JTextField createStyledTextField(String placeholder) {
        // ... (mismo código que en LoginPanel)
    }
    
    private JButton createStyledButton(String text, boolean isPrimary) {
        // ... (mismo código que en LoginPanel)
    }
    
    private Border createCardBorder() {
        // ... (mismo código que en LoginPanel)
    }
}
```

### Ventana Principal

```java
public class EurekaBankMainWindow extends JFrame {
    private CardLayout cardLayout;
    private JPanel mainPanel;
    
    // Estados de la aplicación
    private ServerType currentServerType;
    private String currentUser;
    
    public EurekaBankMainWindow() {
        initializeWindow();
        showServerSelection();
    }
    
    private void initializeWindow() {
        setTitle("EurekaBank - Cliente Java");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setExtendedState(JFrame.MAXIMIZED_BOTH); // Pantalla completa como la web
        setMinimumSize(new Dimension(1024, 768));
        
        // Configurar layout principal
        cardLayout = new CardLayout();
        mainPanel = new JPanel(cardLayout);
        
        add(mainPanel);
        
        // Centrar ventana
        setLocationRelativeTo(null);
        
        // Configurar icono de la aplicación
        try {
            // Aquí puedes agregar un icono personalizado
            // setIconImage(ImageIO.read(getClass().getResource("/icon.png")));
        } catch (Exception e) {
            // Icono por defecto
        }
    }
    
    private void showServerSelection() {
        ServerSelectionPanel serverPanel = new ServerSelectionPanel(e -> {
            String serverTypeName = e.getActionCommand();
            currentServerType = ServerType.valueOf(serverTypeName);
            showLogin();
        });
        
        mainPanel.add(serverPanel, "SERVER_SELECTION");
        cardLayout.show(mainPanel, "SERVER_SELECTION");
    }
    
    private void showLogin() {
        LoginPanel loginPanel = new LoginPanel(
            currentServerType,
            e -> {
                currentUser = e.getActionCommand(); // El username viene en el ActionCommand
                showDashboard();
            },
            e -> showServerSelection() // Volver a selección de servidor
        );
        
        mainPanel.add(loginPanel, "LOGIN");
        cardLayout.show(mainPanel, "LOGIN");
    }
    
    private void showDashboard() {
        DashboardPanel dashboardPanel = new DashboardPanel(currentServerType, currentUser);
        
        mainPanel.add(dashboardPanel, "DASHBOARD");
        cardLayout.show(mainPanel, "DASHBOARD");
    }
    
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            // Configurar Look and Feel antes de crear la UI
            UIConfiguration.setupLookAndFeel();
            
            try {
                new EurekaBankMainWindow().setVisible(true);
            } catch (Exception e) {
                e.printStackTrace();
                JOptionPane.showMessageDialog(null, 
                    "Error iniciando la aplicación: " + e.getMessage(),
                    "Error", 
                    JOptionPane.ERROR_MESSAGE);
            }
        });
    }
}
```

### Consejos Adicionales para Replicar la Apariencia Web

#### 1. **Colores Consistentes**
```java
public class EurekaBankColors {
    // Paleta de colores basada en Tailwind CSS (como tu web)
    public static final Color BLUE_50 = new Color(239, 246, 255);
    public static final Color BLUE_500 = new Color(59, 130, 246);
    public static final Color BLUE_600 = new Color(37, 99, 235);
    public static final Color BLUE_900 = new Color(30, 58, 138);
    public static final Color GRAY_50 = new Color(249, 250, 251);
    public static final Color GRAY_100 = new Color(243, 244, 246);
    public static final Color GRAY_200 = new Color(229, 231, 235);
    public static final Color GRAY_300 = new Color(209, 213, 219);
    public static final Color GRAY_500 = new Color(107, 114, 128);
    public static final Color RED_500 = new Color(239, 68, 68);
}
```

#### 2. **Animaciones Simples**
```java
public class UIAnimations {
    public static void fadeIn(JComponent component) {
        Timer timer = new Timer(50, null);
        float[] alpha = {0.0f};
        
        timer.addActionListener(e -> {
            alpha[0] += 0.1f;
            if (alpha[0] >= 1.0f) {
                alpha[0] = 1.0f;
                timer.stop();
            }
            component.setBackground(new Color(
                component.getBackground().getRed(),
                component.getBackground().getGreen(),
                component.getBackground().getBlue(),
                (int)(255 * alpha[0])
            ));
            component.repaint();
        });
        
        timer.start();
    }
}
```

#### 3. **Responsive Design**
```java
public class ResponsiveLayout {
    public static void makeResponsive(JFrame frame) {
        frame.addComponentListener(new ComponentAdapter() {
            @Override
            public void componentResized(ComponentEvent e) {
                int width = frame.getWidth();
                
                // Ajustar componentes según el ancho
                if (width < 768) {
                    // Móvil
                    adjustForMobile(frame);
                } else if (width < 1024) {
                    // Tablet
                    adjustForTablet(frame);
                } else {
                    // Desktop
                    adjustForDesktop(frame);
                }
            }
        });
    }
}
```

---

## Ejemplo de Implementación Completa

### Cliente Unificado con Factory Pattern

```java
public interface EurekaBankClient {
    String health() throws Exception;
    boolean login(String username, String password) throws Exception;
    List<Movimiento> traerMovimientos(String cuenta) throws Exception;
    int regDeposito(String cuenta, double importe) throws Exception;
    int regRetiro(String cuenta, double importe) throws Exception;
    int regTransferencia(String cuentaOrigen, String cuentaDestino, double importe) throws Exception;
}

public class EurekaBankClientFactory {
    public static EurekaBankClient createClient(ServerType serverType) {
        switch (serverType) {
            case REST_JAVA:
                return new RestJavaClientImpl();
            case REST_DOTNET:
                return new RestDotNetClientImpl();
            case SOAP_JAVA:
                return new SoapJavaClientImpl();
            case SOAP_DOTNET:
                return new SoapDotNetClientImpl();
            default:
                throw new IllegalArgumentException("Tipo de servidor no soportado: " + serverType);
        }
    }
}

// Clase principal para pruebas
public class EurekaBankTester {
    public static void main(String[] args) {
        // Probar todos los servidores
        for (ServerType serverType : ServerType.values()) {
            System.out.println("\n=== Probando " + serverType.getDisplayName() + " ===");
            
            try {
                EurekaBankClient client = EurekaBankClientFactory.createClient(serverType);
                
                // 1. Health check
                String health = client.health();
                System.out.println("Health: " + health);
                
                // 2. Login
                boolean loginOk = client.login("MONSTER", "MONSTER9");
                System.out.println("Login: " + loginOk);
                
                if (loginOk) {
                    // 3. Obtener movimientos
                    List<Movimiento> movimientos = client.traerMovimientos("00100001");
                    System.out.println("Movimientos encontrados: " + movimientos.size());
                    
                    // 4. Registrar depósito
                    int estadoDeposito = client.regDeposito("00100001", 100.0);
                    System.out.println("Depósito registrado - Estado: " + estadoDeposito);
                    
                    // 5. Registrar retiro
                    int estadoRetiro = client.regRetiro("00100001", 50.0);
                    System.out.println("Retiro registrado - Estado: " + estadoRetiro);
                    
                    // 6. Registrar transferencia
                    int estadoTransferencia = client.regTransferencia("00100001", "00100002", 25.0);
                    System.out.println("Transferencia registrada - Estado: " + estadoTransferencia);
                }
                
            } catch (Exception e) {
                System.err.println("Error con " + serverType.getDisplayName() + ": " + e.getMessage());
                e.printStackTrace();
            }
        }
    }
}
```

---

## Puntos Importantes para Implementación Java

### 1. **Manejo de Certificados SSL**
```java
// Deshabilitar verificación SSL para desarrollo (solo desarrollo!)
System.setProperty("com.sun.net.ssl.checkRevocation", "false");
System.setProperty("sun.security.ssl.allowUnsafeRenegotiation", "true");
```

### 2. **Timeout Configuration**
```java
// Para clientes HTTP
RequestConfig config = RequestConfig.custom()
    .setConnectTimeout(5000)
    .setSocketTimeout(10000)
    .build();
    
HttpGet request = new HttpGet(url);
request.setConfig(config);
```

### 3. **Manejo de Errores**
```java
public class EurekaBankException extends Exception {
    private final ServerType serverType;
    private final String operation;
    
    public EurekaBankException(ServerType serverType, String operation, String message, Throwable cause) {
        super(String.format("[%s] Error en operación '%s': %s", 
                           serverType.getDisplayName(), operation, message), cause);
        this.serverType = serverType;
        this.operation = operation;
    }
    
    // Getters...
}
```

### 4. **Pool de Conexiones**
```java
// Para mejor rendimiento con múltiples peticiones
PoolingHttpClientConnectionManager connectionManager = new PoolingHttpClientConnectionManager();
connectionManager.setMaxTotal(20);
connectionManager.setDefaultMaxPerRoute(10);

CloseableHttpClient httpClient = HttpClients.custom()
    .setConnectionManager(connectionManager)
    .build();
```

### 5. **Logging**
```java
import java.util.logging.Logger;
import java.util.logging.Level;

public class EurekaBankLogger {
    private static final Logger logger = Logger.getLogger(EurekaBankLogger.class.getName());
    
    public static void logRequest(ServerType serverType, String operation, Object params) {
        logger.info(String.format("[%s] Ejecutando %s con parámetros: %s", 
                                 serverType.getDisplayName(), operation, params));
    }
    
    public static void logResponse(ServerType serverType, String operation, Object response) {
        logger.info(String.format("[%s] Respuesta de %s: %s", 
                                 serverType.getDisplayName(), operation, response));
    }
    
    public static void logError(ServerType serverType, String operation, Exception e) {
        logger.log(Level.SEVERE, String.format("[%s] Error en %s", 
                                              serverType.getDisplayName(), operation), e);
    }
}
```

---

## Resumen de Diferencias Clave

| Aspecto | REST Java | REST .NET | SOAP Java | SOAP .NET |
|---------|-----------|-----------|-----------|-----------|
| **Puerto** | 8080 | 5000 | 8080 | 57199 |
| **Parámetros** | Query params | JSON body | Objetos Java | Objetos Java + Headers |
| **Respuestas** | Directas | Wrapper estándar | Objetos tipados | Strings que requieren conversión |
| **Headers** | Content-Type básico | Content-Type básico | Automáticos | SOAPAction requerido |
| **Nombres métodos** | minúsculas | PascalCase | minúsculas | PascalCase |
| **Movimientos** | `traerMovimientos` | `ObtenerPorCuenta` | `traerMovimientos` | `ObtenerPorCuenta` |

---

Esta documentación te proporciona todo lo necesario para implementar un cliente Java de escritorio completo que se conecte a todos los servidores EurekaBank. Cada implementación maneja las particularidades específicas de cada tipo de servidor y protocolo.