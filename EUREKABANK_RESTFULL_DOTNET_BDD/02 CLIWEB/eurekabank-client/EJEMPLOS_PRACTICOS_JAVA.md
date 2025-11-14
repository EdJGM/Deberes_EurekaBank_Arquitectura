# Ejemplos Prácticos - Cliente Java EurekaBank

Este documento complementa la documentación principal con ejemplos prácticos y casos de uso reales.

## Ejemplos de Respuestas Reales

### REST Java - Respuestas Típicas

```json
// Health Check
"Servicio Eureka REST activo y funcionando correctamente"

// Login exitoso
true

// Login fallido  
false

// Movimientos
[
  {
    "cuenta": "00100001",
    "nromov": 1,
    "fecha": "2024-01-15T10:30:00.000+00:00",
    "tipo": "001",
    "accion": "Depósito en efectivo", 
    "importe": 1000.50
  },
  {
    "cuenta": "00100001",
    "nromov": 2,
    "fecha": "2024-01-16T14:20:00.000+00:00",
    "tipo": "002",
    "accion": "Retiro en cajero",
    "importe": 500.00
  }
]

// Depósito exitoso (HTTP 200)
"Depósito registrado con éxito."

// Depósito fallido (HTTP 500)
"Error al registrar el depósito."
```

### REST .NET - Respuestas Típicas

```json
// Health Check
{
  "success": true,
  "message": "Servicio activo y funcionando correctamente",
  "data": {
    "status": "healthy",
    "service": "Eurekabank REST API",
    "timestamp": "2024-01-15T10:30:00.000Z"
  }
}

// Login exitoso
{
  "success": true,
  "message": "Autenticación exitosa",
  "data": {
    "authenticated": true,
    "username": "admin"
  }
}

// Login fallido
{
  "success": false,
  "message": "Credenciales inválidas",
  "data": null
}

// Movimientos
{
  "success": true,
  "message": "Se encontraron 2 movimientos",
  "data": [
    {
      "cuenta": "00100001",
      "nroMov": 1,
      "fecha": "2024-01-15T10:30:00",
      "tipo": "001",
      "accion": "Depósito en efectivo",
      "importe": 1000.50
    }
  ]
}

// Operación exitosa
{
  "success": true,
  "message": "Depósito registrado exitosamente",
  "data": {
    "operacion": "deposito",
    "cuenta": "00100001",
    "importe": 1500.75
  }
}

// Operación fallida
{
  "success": false,
  "message": "El importe debe ser mayor a cero",
  "data": null
}
```

### SOAP Java - XML de Respuestas

```xml
<!-- Health Check -->
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:healthResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <status>Servicio Eurekabank SOAP activo y funcionando correctamente</status>
      </ns2:healthResponse>
   </soap:Body>
</soap:Envelope>

<!-- Login -->
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:loginResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <return>true</return>
      </ns2:loginResponse>
   </soap:Body>
</soap:Envelope>

<!-- Movimientos -->
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:traerMovimientosResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <movimiento>
            <cuenta>00100001</cuenta>
            <nromov>1</nromov>
            <fecha>2024-01-15T10:30:00</fecha>
            <tipo>001</tipo>
            <accion>Depósito en efectivo</accion>
            <importe>1000.50</importe>
         </movimiento>
      </ns2:traerMovimientosResponse>
   </soap:Body>
</soap:Envelope>

<!-- Operaciones (depósito/retiro/transferencia) -->
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regDepositoResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>1</estado>
      </ns2:regDepositoResponse>
   </soap:Body>
</soap:Envelope>
```

### SOAP .NET - XML de Respuestas

```xml
<!-- Health Check -->
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <HealthResponse xmlns="http://tempuri.org/">
         <HealthResult>Servicio Eurekabank SOAP activo y funcionando correctamente</HealthResult>
      </HealthResponse>
   </s:Body>
</s:Envelope>

<!-- Login -->
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <LoginResponse xmlns="http://tempuri.org/">
         <LoginResult>true</LoginResult>
      </LoginResponse>
   </s:Body>
</s:Envelope>

<!-- Movimientos -->
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <ObtenerPorCuentaResponse xmlns="http://tempuri.org/">
         <ObtenerPorCuentaResult xmlns:a="http://schemas.datacontract.org/2004/07/Eurekabank_Soap_Dotnet.ec.edu.monster.modelo">
            <a:movimiento>
               <a:Cuenta>00100001</a:Cuenta>
               <a:NroMov>1</a:NroMov>
               <a:Fecha>2008-01-06T00:00:00</a:Fecha>
               <a:Tipo>Apertura de cuenta</a:Tipo>
               <a:Accion>INGRESO</a:Accion>
               <a:Importe>2800</a:Importe>
            </a:movimiento>
         </ObtenerPorCuentaResult>
      </ObtenerPorCuentaResponse>
   </s:Body>
</s:Envelope>

<!-- Operaciones -->
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarDepositoResponse xmlns="http://tempuri.org/">
         <RegistrarDepositoResult>1</RegistrarDepositoResult>
      </RegistrarDepositoResponse>
   </s:Body>
</s:Envelope>
```

## Implementación de Clases Helper

### ResponseProcessor - Procesador de Respuestas

```java
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

public class ResponseProcessor {
    private static final ObjectMapper objectMapper = new ObjectMapper();
    
    // Procesar respuesta REST Java
    public static <T> T processRestJavaResponse(String jsonResponse, Class<T> responseType) throws Exception {
        if (responseType == String.class) {
            // Para respuestas simples como health y mensajes
            return (T) jsonResponse.replace("\"", "");
        } else if (responseType == Boolean.class) {
            // Para login
            return (T) Boolean.valueOf(jsonResponse);
        } else {
            // Para objetos complejos como listas de movimientos
            return objectMapper.readValue(jsonResponse, responseType);
        }
    }
    
    // Procesar respuesta REST .NET
    public static <T> T processRestDotNetResponse(String jsonResponse, TypeReference<DotNetResponse<T>> typeRef) throws Exception {
        DotNetResponse<T> response = objectMapper.readValue(jsonResponse, typeRef);
        
        if (!response.success) {
            throw new EurekaBankException(ServerType.REST_DOTNET, "unknown", response.message, null);
        }
        
        return response.data;
    }
    
    // Procesar estado de operaciones bancarias
    public static int processOperationState(String response, boolean isSuccess) {
        return isSuccess ? 1 : -1;
    }
    
    // Normalizar movimientos de .NET SOAP (campos con mayúsculas)
    public static List<Movimiento> normalizeMovimientosDotNet(List<?> movimientosRaw) {
        List<Movimiento> normalized = new ArrayList<>();
        
        for (Object movRaw : movimientosRaw) {
            if (movRaw instanceof Map) {
                Map<String, Object> movMap = (Map<String, Object>) movRaw;
                Movimiento mov = new Movimiento();
                
                // Convertir campos con mayúsculas a minúsculas
                mov.setCuenta(getString(movMap, "Cuenta", "cuenta"));
                mov.setNromov(getInt(movMap, "NroMov", "nromov"));
                mov.setFecha(getString(movMap, "Fecha", "fecha"));
                mov.setTipo(getString(movMap, "Tipo", "tipo"));
                mov.setAccion(getString(movMap, "Accion", "accion"));
                mov.setImporte(getDouble(movMap, "Importe", "importe"));
                
                normalized.add(mov);
            }
        }
        
        return normalized;
    }
    
    // Métodos helper para extraer valores
    private static String getString(Map<String, Object> map, String... keys) {
        for (String key : keys) {
            Object value = map.get(key);
            if (value != null) {
                return value.toString();
            }
        }
        return "";
    }
    
    private static int getInt(Map<String, Object> map, String... keys) {
        for (String key : keys) {
            Object value = map.get(key);
            if (value != null) {
                if (value instanceof Number) {
                    return ((Number) value).intValue();
                }
                try {
                    return Integer.parseInt(value.toString());
                } catch (NumberFormatException e) {
                    // Continuar con la siguiente clave
                }
            }
        }
        return 0;
    }
    
    private static double getDouble(Map<String, Object> map, String... keys) {
        for (String key : keys) {
            Object value = map.get(key);
            if (value != null) {
                if (value instanceof Number) {
                    return ((Number) value).doubleValue();
                }
                try {
                    return Double.parseDouble(value.toString());
                } catch (NumberFormatException e) {
                    // Continuar con la siguiente clave
                }
            }
        }
        return 0.0;
    }
}
```

### ConnectionManager - Gestor de Conexiones

```java
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.impl.conn.PoolingHttpClientConnectionManager;
import org.apache.http.client.config.RequestConfig;

public class ConnectionManager {
    private static ConnectionManager instance;
    private final CloseableHttpClient httpClient;
    private final RequestConfig requestConfig;
    
    private ConnectionManager() {
        // Configurar pool de conexiones
        PoolingHttpClientConnectionManager connectionManager = new PoolingHttpClientConnectionManager();
        connectionManager.setMaxTotal(20);
        connectionManager.setDefaultMaxPerRoute(10);
        
        // Configurar timeouts
        this.requestConfig = RequestConfig.custom()
            .setConnectTimeout(5000)
            .setSocketTimeout(10000)
            .setConnectionRequestTimeout(3000)
            .build();
        
        // Crear cliente HTTP
        this.httpClient = HttpClients.custom()
            .setConnectionManager(connectionManager)
            .setDefaultRequestConfig(requestConfig)
            .build();
    }
    
    public static synchronized ConnectionManager getInstance() {
        if (instance == null) {
            instance = new ConnectionManager();
        }
        return instance;
    }
    
    public CloseableHttpClient getHttpClient() {
        return httpClient;
    }
    
    public RequestConfig getRequestConfig() {
        return requestConfig;
    }
    
    public void shutdown() {
        try {
            if (httpClient != null) {
                httpClient.close();
            }
        } catch (Exception e) {
            EurekaBankLogger.logError(null, "shutdown", e);
        }
    }
}
```

### SOAPClientHelper - Helper para clientes SOAP

```java
import javax.xml.ws.BindingProvider;
import javax.xml.ws.handler.MessageContext;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SOAPClientHelper {
    
    // Configurar endpoint para cliente SOAP
    public static void configureEndpoint(Object port, String endpointUrl) {
        if (port instanceof BindingProvider) {
            BindingProvider bp = (BindingProvider) port;
            bp.getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, endpointUrl);
            
            // Configurar timeouts
            bp.getRequestContext().put("com.sun.xml.ws.connect.timeout", 5000);
            bp.getRequestContext().put("com.sun.xml.ws.request.timeout", 10000);
        }
    }
    
    // Establecer headers HTTP para SOAP .NET
    public static void setSOAPActionHeader(Object port, String soapAction) {
        if (port instanceof BindingProvider) {
            BindingProvider bp = (BindingProvider) port;
            Map<String, Object> requestContext = bp.getRequestContext();
            
            Map<String, List<String>> headers = new HashMap<>();
            headers.put("Content-Type", Collections.singletonList("text/xml; charset=utf-8"));
            headers.put("SOAPAction", Collections.singletonList("\"" + soapAction + "\""));
            
            requestContext.put(MessageContext.HTTP_REQUEST_HEADERS, headers);
        }
    }
    
    // Obtener información de debug de la última petición SOAP
    public static void logLastSOAPExchange(Object port, String operation) {
        if (port instanceof BindingProvider) {
            BindingProvider bp = (BindingProvider) port;
            Map<String, Object> responseContext = bp.getResponseContext();
            
            // Intentar obtener información de debug
            Object requestHeaders = responseContext.get(MessageContext.HTTP_REQUEST_HEADERS);
            Object responseHeaders = responseContext.get(MessageContext.HTTP_RESPONSE_HEADERS);
            
            EurekaBankLogger.logger.info("SOAP " + operation + " - Request Headers: " + requestHeaders);
            EurekaBankLogger.logger.info("SOAP " + operation + " - Response Headers: " + responseHeaders);
        }
    }
    
    // Mapeo de operaciones a SOAPActions para .NET
    public static String getSOAPAction(String operation) {
        Map<String, String> soapActions = new HashMap<>();
        soapActions.put("login", "http://tempuri.org/IEurekabankWS/Login");
        soapActions.put("health", "http://tempuri.org/IEurekabankWS/Health");
        soapActions.put("traerMovimientos", "http://tempuri.org/IEurekabankWS/ObtenerPorCuenta");
        soapActions.put("regDeposito", "http://tempuri.org/IEurekabankWS/RegistrarDeposito");
        soapActions.put("regRetiro", "http://tempuri.org/IEurekabankWS/RegistrarRetiro");
        soapActions.put("regTransferencia", "http://tempuri.org/IEurekabankWS/RegistrarTransferencia");
        
        return soapActions.getOrDefault(operation, "");
    }
}
```

## Casos de Uso Prácticos

### Ejemplo 1: Aplicación de Consola Completa

```java
import java.util.Scanner;
import java.util.List;

public class EurekaBankConsoleApp {
    private static final Scanner scanner = new Scanner(System.in);
    private EurekaBankClient client;
    private ServerType currentServerType;
    private String currentUser;
    
    public static void main(String[] args) {
        EurekaBankConsoleApp app = new EurekaBankConsoleApp();
        app.run();
    }
    
    public void run() {
        System.out.println("=== EurekaBank Cliente Java ===");
        
        while (true) {
            try {
                if (client == null) {
                    selectServer();
                }
                
                if (currentUser == null) {
                    login();
                }
                
                showMainMenu();
                
            } catch (Exception e) {
                System.err.println("Error: " + e.getMessage());
                e.printStackTrace();
            }
        }
    }
    
    private void selectServer() {
        System.out.println("\nSeleccione el tipo de servidor:");
        ServerType[] servers = ServerType.values();
        
        for (int i = 0; i < servers.length; i++) {
            System.out.printf("%d. %s\n", i + 1, servers[i].getDisplayName());
        }
        
        System.out.print("Opción: ");
        int option = scanner.nextInt() - 1;
        scanner.nextLine(); // Consumir newline
        
        if (option >= 0 && option < servers.length) {
            currentServerType = servers[option];
            client = EurekaBankClientFactory.createClient(currentServerType);
            
            System.out.println("Conectado a: " + currentServerType.getDisplayName());
            
            // Probar conectividad
            try {
                String health = client.health();
                System.out.println("Estado del servidor: " + health);
            } catch (Exception e) {
                System.err.println("Error conectando al servidor: " + e.getMessage());
                client = null;
            }
        } else {
            System.out.println("Opción inválida.");
        }
    }
    
    private void login() {
        System.out.print("\nUsuario: ");
        String username = scanner.nextLine();
        
        System.out.print("Contraseña: ");
        String password = scanner.nextLine();
        
        try {
            boolean loginSuccess = client.login(username, password);
            
            if (loginSuccess) {
                currentUser = username;
                System.out.println("Login exitoso. Bienvenido " + username + "!");
            } else {
                System.out.println("Credenciales inválidas.");
            }
        } catch (Exception e) {
            System.err.println("Error en login: " + e.getMessage());
        }
    }
    
    private void showMainMenu() {
        System.out.println("\n=== Menú Principal ===");
        System.out.println("1. Ver movimientos");
        System.out.println("2. Registrar depósito");
        System.out.println("3. Registrar retiro");
        System.out.println("4. Registrar transferencia");
        System.out.println("5. Cambiar servidor");
        System.out.println("6. Cerrar sesión");
        System.out.println("7. Salir");
        
        System.out.print("Opción: ");
        int option = scanner.nextInt();
        scanner.nextLine(); // Consumir newline
        
        switch (option) {
            case 1:
                verMovimientos();
                break;
            case 2:
                registrarDeposito();
                break;
            case 3:
                registrarRetiro();
                break;
            case 4:
                registrarTransferencia();
                break;
            case 5:
                client = null;
                currentServerType = null;
                currentUser = null;
                break;
            case 6:
                currentUser = null;
                break;
            case 7:
                System.out.println("Hasta luego!");
                System.exit(0);
                break;
            default:
                System.out.println("Opción inválida.");
        }
    }
    
    private void verMovimientos() {
        System.out.print("Ingrese número de cuenta: ");
        String cuenta = scanner.nextLine();
        
        try {
            List<Movimiento> movimientos = client.traerMovimientos(cuenta);
            
            if (movimientos.isEmpty()) {
                System.out.println("No se encontraron movimientos para la cuenta " + cuenta);
            } else {
                System.out.println("\nMovimientos de la cuenta " + cuenta + ":");
                System.out.println("=============================================");
                
                for (Movimiento mov : movimientos) {
                    System.out.printf("Mov %d | %s | %s | %s | $%.2f\n",
                                    mov.getNromov(),
                                    mov.getFecha(),
                                    mov.getTipo(),
                                    mov.getAccion(),
                                    mov.getImporte());
                }
            }
        } catch (Exception e) {
            System.err.println("Error obteniendo movimientos: " + e.getMessage());
        }
    }
    
    private void registrarDeposito() {
        System.out.print("Cuenta: ");
        String cuenta = scanner.nextLine();
        
        System.out.print("Importe: ");
        double importe = scanner.nextDouble();
        scanner.nextLine();
        
        try {
            int estado = client.regDeposito(cuenta, importe);
            
            if (estado == 1) {
                System.out.println("Depósito registrado exitosamente.");
            } else {
                System.out.println("Error registrando el depósito.");
            }
        } catch (Exception e) {
            System.err.println("Error en depósito: " + e.getMessage());
        }
    }
    
    private void registrarRetiro() {
        System.out.print("Cuenta: ");
        String cuenta = scanner.nextLine();
        
        System.out.print("Importe: ");
        double importe = scanner.nextDouble();
        scanner.nextLine();
        
        try {
            int estado = client.regRetiro(cuenta, importe);
            
            if (estado == 1) {
                System.out.println("Retiro registrado exitosamente.");
            } else {
                System.out.println("Error registrando el retiro.");
            }
        } catch (Exception e) {
            System.err.println("Error en retiro: " + e.getMessage());
        }
    }
    
    private void registrarTransferencia() {
        System.out.print("Cuenta origen: ");
        String cuentaOrigen = scanner.nextLine();
        
        System.out.print("Cuenta destino: ");
        String cuentaDestino = scanner.nextLine();
        
        System.out.print("Importe: ");
        double importe = scanner.nextDouble();
        scanner.nextLine();
        
        try {
            int estado = client.regTransferencia(cuentaOrigen, cuentaDestino, importe);
            
            if (estado == 1) {
                System.out.println("Transferencia registrada exitosamente.");
            } else {
                System.out.println("Error registrando la transferencia.");
            }
        } catch (Exception e) {
            System.err.println("Error en transferencia: " + e.getMessage());
        }
    }
}
```

### Ejemplo 2: Pruebas Unitarias

```java
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.BeforeEach;
import static org.junit.jupiter.api.Assertions.*;

public class EurekaBankClientTest {
    
    @Test
    public void testAllServersHealth() {
        for (ServerType serverType : ServerType.values()) {
            try {
                EurekaBankClient client = EurekaBankClientFactory.createClient(serverType);
                String health = client.health();
                
                assertNotNull(health);
                assertFalse(health.trim().isEmpty());
                
                System.out.println(serverType.getDisplayName() + " - Health: " + health);
                
            } catch (Exception e) {
                fail("Health check failed for " + serverType.getDisplayName() + ": " + e.getMessage());
            }
        }
    }
    
    @Test
    public void testLoginAllServers() {
        String validUsername = "MONSTER";
        String validPassword = "MONSTER9";
        String invalidPassword = "invalid";
        
        for (ServerType serverType : ServerType.values()) {
            try {
                EurekaBankClient client = EurekaBankClientFactory.createClient(serverType);
                
                // Test valid login
                boolean validLogin = client.login(validUsername, validPassword);
                assertTrue(validLogin, "Valid login should succeed for " + serverType.getDisplayName());
                
                // Test invalid login
                boolean invalidLogin = client.login(validUsername, invalidPassword);
                assertFalse(invalidLogin, "Invalid login should fail for " + serverType.getDisplayName());
                
            } catch (Exception e) {
                fail("Login test failed for " + serverType.getDisplayName() + ": " + e.getMessage());
            }
        }
    }
    
    @Test
    public void testMovimientosAllServers() {
        String testAccount = "00100001";
        
        for (ServerType serverType : ServerType.values()) {
            try {
                EurekaBankClient client = EurekaBankClientFactory.createClient(serverType);
                
                // Login first
                assertTrue(client.login("MONSTER", "MONSTER9"));
                
                // Get movements
                List<Movimiento> movimientos = client.traerMovimientos(testAccount);
                assertNotNull(movimientos);
                
                System.out.println(serverType.getDisplayName() + " - Movimientos: " + movimientos.size());
                
                // Validate movement structure if any exist
                if (!movimientos.isEmpty()) {
                    Movimiento first = movimientos.get(0);
                    assertNotNull(first.getCuenta());
                    assertTrue(first.getNromov() > 0);
                    assertNotNull(first.getFecha());
                    
                    System.out.println("First movement: " + first);
                }
                
            } catch (Exception e) {
                fail("Movements test failed for " + serverType.getDisplayName() + ": " + e.getMessage());
            }
        }
    }
}
```

## Configuración de Proyecto Maven Completo

### pom.xml

```xml
<?xml version="1.0" encoding="UTF-8"?>
<project xmlns="http://maven.apache.org/POM/4.0.0"
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 
         http://maven.apache.org/xsd/maven-4.0.0.xsd">
    <modelVersion>4.0.0</modelVersion>
    
    <groupId>ec.monster.edu</groupId>
    <artifactId>eurekabank-client-java</artifactId>
    <version>1.0.0</version>
    <packaging>jar</packaging>
    
    <name>EurekaBank Java Client</name>
    <description>Cliente Java de escritorio para EurekaBank</description>
    
    <properties>
        <maven.compiler.source>11</maven.compiler.source>
        <maven.compiler.target>11</maven.compiler.target>
        <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
    </properties>
    
    <dependencies>
        <!-- Jackson para JSON -->
        <dependency>
            <groupId>com.fasterxml.jackson.core</groupId>
            <artifactId>jackson-databind</artifactId>
            <version>2.15.2</version>
        </dependency>
        
        <!-- Apache HTTP Client -->
        <dependency>
            <groupId>org.apache.httpcomponents</groupId>
            <artifactId>httpclient</artifactId>
            <version>4.5.14</version>
        </dependency>
        
        <!-- SOAP/Web Services -->
        <dependency>
            <groupId>javax.xml.ws</groupId>
            <artifactId>jaxws-api</artifactId>
            <version>2.3.1</version>
        </dependency>
        
        <dependency>
            <groupId>com.sun.xml.ws</groupId>
            <artifactId>jaxws-rt</artifactId>
            <version>2.3.5</version>
        </dependency>
        
        <!-- Testing -->
        <dependency>
            <groupId>org.junit.jupiter</groupId>
            <artifactId>junit-jupiter</artifactId>
            <version>5.9.2</version>
            <scope>test</scope>
        </dependency>
        
        <!-- Logging -->
        <dependency>
            <groupId>org.slf4j</groupId>
            <artifactId>slf4j-api</artifactId>
            <version>2.0.7</version>
        </dependency>
        
        <dependency>
            <groupId>ch.qos.logback</groupId>
            <artifactId>logback-classic</artifactId>
            <version>1.4.8</version>
        </dependency>
    </dependencies>
    
    <build>
        <plugins>
            <!-- Compilación -->
            <plugin>
                <groupId>org.apache.maven.plugins</groupId>
                <artifactId>maven-compiler-plugin</artifactId>
                <version>3.11.0</version>
                <configuration>
                    <source>11</source>
                    <target>11</target>
                </configuration>
            </plugin>
            
            <!-- Tests -->
            <plugin>
                <groupId>org.apache.maven.plugins</groupId>
                <artifactId>maven-surefire-plugin</artifactId>
                <version>3.1.2</version>
            </plugin>
            
            <!-- JAR ejecutable -->
            <plugin>
                <groupId>org.apache.maven.plugins</groupId>
                <artifactId>maven-shade-plugin</artifactId>
                <version>3.4.1</version>
                <executions>
                    <execution>
                        <phase>package</phase>
                        <goals>
                            <goal>shade</goal>
                        </goals>
                        <configuration>
                            <transformers>
                                <transformer implementation="org.apache.maven.plugins.shade.resource.ManifestResourceTransformer">
                                    <mainClass>ec.monster.edu.eurekabank.EurekaBankConsoleApp</mainClass>
                                </transformer>
                            </transformers>
                        </configuration>
                    </execution>
                </executions>
            </plugin>
            
            <!-- Generación de clientes SOAP -->
            <plugin>
                <groupId>com.sun.xml.ws</groupId>
                <artifactId>jaxws-maven-plugin</artifactId>
                <version>2.3.2</version>
                <executions>
                    <!-- Cliente SOAP Java -->
                    <execution>
                        <id>soap-java-client</id>
                        <goals>
                            <goal>wsimport</goal>
                        </goals>
                        <configuration>
                            <wsdlUrls>
                                <wsdlUrl>http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl</wsdlUrl>
                            </wsdlUrls>
                            <packageName>ec.monster.edu.soap.java</packageName>
                            <sourceDestDir>${project.build.directory}/generated-sources/soap-java</sourceDestDir>
                        </configuration>
                    </execution>
                    
                    <!-- Cliente SOAP .NET -->
                    <execution>
                        <id>soap-dotnet-client</id>
                        <goals>
                            <goal>wsimport</goal>
                        </goals>
                        <configuration>
                            <wsdlUrls>
                                <wsdlUrl>http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl</wsdlUrl>
                            </wsdlUrls>
                            <packageName>ec.monster.edu.soap.dotnet</packageName>
                            <sourceDestDir>${project.build.directory}/generated-sources/soap-dotnet</sourceDestDir>
                        </configuration>
                    </execution>
                </executions>
            </plugin>
        </plugins>
    </build>
</project>
```

Esta documentación práctica complementa la documentación principal y te proporciona ejemplos concretos y código listo para usar en tu cliente Java de escritorio.