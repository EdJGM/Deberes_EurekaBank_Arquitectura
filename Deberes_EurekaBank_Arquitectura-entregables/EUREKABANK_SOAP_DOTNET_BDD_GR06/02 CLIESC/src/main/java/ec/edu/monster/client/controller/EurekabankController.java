package ec.edu.monster.client.controller;

import ec.edu.monster.client.model.BankingOperation;
import ec.edu.monster.client.model.ServerType;
import ec.edu.monster.client.service.EurekabankService;
import ec.edu.monster.client.service.ServiceResponse;

import java.util.Map;
import java.util.Observable;

/**
 * Controlador principal de EUREKABANK - Arquitectura MVC
 * Maneja la lógica de negocio y coordina entre Model y View
 */
public class EurekabankController extends Observable {

    private final EurekabankService eurekabankService;
    private ServerType currentServerType;
    private BankingOperation currentOperation;

    public EurekabankController() {
        this.eurekabankService = new EurekabankService();
        this.currentServerType = ServerType.REST_JAVA; // Servidor por defecto
        this.currentOperation = BankingOperation.HEALTH; // Operación por defecto
    }

    /**
     * Cambiar el tipo de servidor seleccionado
     */
    public void setServerType(ServerType serverType) {
        this.currentServerType = serverType;
        setChanged();
        notifyObservers("SERVER_CHANGED");
    }

    /**
     * Cambiar la operación bancaria seleccionada
     */
    public void setBankingOperation(BankingOperation operation) {
        this.currentOperation = operation;
        setChanged();
        notifyObservers("OPERATION_CHANGED");
    }

    /**
     * Ejecutar la operación bancaria con los parámetros proporcionados
     */
    public ServiceResponse executeOperation(Map<String, String> parameters) {
        System.out.println("=== INICIO executeOperation ===");
        System.out.println("Parámetros recibidos: " + parameters);
        System.out.println("Servidor actual: " + currentServerType);
        System.out.println("Operación actual: " + currentOperation);

        try {
            // Si no hay operación específica en los parámetros, usar la actual
            if (!parameters.containsKey("operation")) {
                parameters.put("operation", currentOperation.getCode());
                System.out.println("Agregada operación por defecto: " + currentOperation.getCode());
            }

            // Para el caso especial de login, establecer la operación correcta
            if (parameters.containsKey("username") && parameters.containsKey("password") &&
                !parameters.containsKey("operation")) {
                parameters.put("operation", "login");
                System.out.println("Operación establecida como login");
            }

            System.out.println("Parámetros finales: " + parameters);

            // Notificar que se está ejecutando la operación
            setChanged();
            notifyObservers("OPERATION_EXECUTING");

            System.out.println("Llamando a eurekabankService.callService...");
            System.out.println("ServerType ID: " + currentServerType.getId());

            // Ejecutar la operación usando el servicio
            ServiceResponse response = eurekabankService.callService(
                currentServerType.getId(),
                parameters
            );

            System.out.println("Respuesta recibida: " + response);
            System.out.println("¿Es exitosa? " + response.isSuccessful());

            // Notificar que la operación se completó
            setChanged();
            if (response.isSuccessful()) {
                notifyObservers("OPERATION_COMPLETED");
                System.out.println("Operación completada exitosamente");
            } else {
                notifyObservers("OPERATION_ERROR");
                System.out.println("Error en la operación");
            }

            System.out.println("=== FIN executeOperation ===");
            return response;

        } catch (Exception e) {
            System.err.println("=== ERROR EN executeOperation ===");
            System.err.println("Exception: " + e.getClass().getSimpleName());
            System.err.println("Message: " + e.getMessage());
            e.printStackTrace();

            ServiceResponse errorResponse = new ServiceResponse(
                500,
                "Error interno: " + e.getMessage(),
                false
            );

            setChanged();
            notifyObservers("OPERATION_ERROR");

            System.out.println("=== FIN executeOperation (ERROR) ===");
            return errorResponse;
        }
    }

    /**
     * Validar parámetros de entrada para la operación actual
     */
    public boolean validateParameters(Map<String, String> parameters) {
        // Para login, validar username y password
        if (parameters.containsKey("username") && parameters.containsKey("password")) {
            String username = parameters.get("username");
            String password = parameters.get("password");
            return username != null && !username.trim().isEmpty() &&
                   password != null && !password.trim().isEmpty();
        }

        // Para operaciones bancarias, validar según el tipo de operación
        switch (currentOperation) {
            case MOVIMIENTOS:
                return parameters.containsKey("cuenta") &&
                       !parameters.get("cuenta").trim().isEmpty();

            case DEPOSITO:
            case RETIRO:
                return parameters.containsKey("cuenta") &&
                       parameters.containsKey("importe") &&
                       !parameters.get("cuenta").trim().isEmpty() &&
                       !parameters.get("importe").trim().isEmpty();

            case TRANSFERENCIA:
                return parameters.containsKey("cuentaOrigen") &&
                       parameters.containsKey("cuentaDestino") &&
                       parameters.containsKey("importe") &&
                       !parameters.get("cuentaOrigen").trim().isEmpty() &&
                       !parameters.get("cuentaDestino").trim().isEmpty() &&
                       !parameters.get("importe").trim().isEmpty();

            default:
                return true;
        }
    }

    /**
     * Ejecutar operación de login
     */
    public ServiceResponse login(String username, String password, ServerType serverType) {
        setServerType(serverType);

        Map<String, String> parameters = new java.util.HashMap<>();
        parameters.put("username", username);
        parameters.put("password", password);
        parameters.put("operation", "login");

        return executeOperation(parameters);
    }

    // Getters
    public ServerType getCurrentServerType() {
        return currentServerType;
    }

    public BankingOperation getCurrentOperation() {
        return currentOperation;
    }

    public EurekabankService getEurekabankService() {
        return eurekabankService;
    }

    /**
     * Get available server types for UI components
     */
    public ServerType[] getAvailableServerTypes() {
        return ServerType.values();
    }

    /**
     * Get available banking operations for UI components
     */
    public BankingOperation[] getAvailableBankingOperations() {
        return BankingOperation.values();
    }

    /**
     * Get current server information for display
     */
    public String getCurrentServerInfo() {
        return currentServerType.getDisplayName() + " (" + currentServerType.getDescription() + ")";
    }
}
