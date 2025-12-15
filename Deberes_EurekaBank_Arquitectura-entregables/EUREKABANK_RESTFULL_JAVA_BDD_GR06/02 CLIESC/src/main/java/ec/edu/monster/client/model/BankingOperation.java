package ec.edu.monster.client.model;

/**
 * Enumeración de operaciones bancarias disponibles en EUREKABANK
 * Modelo para las diferentes operaciones del sistema bancario
 */
public enum BankingOperation {
    HEALTH("health", "Health Check", "Verificar conectividad del servidor", new String[0]),
    LOGIN("login", "Iniciar Sesión", "Autenticar usuario en el sistema", new String[]{"username", "password"}),
    MOVIMIENTOS("movimientos", "Consultar Movimientos", "Obtener movimientos de una cuenta", new String[]{"cuenta"}),
    DEPOSITO("deposito", "Realizar Depósito", "Depositar dinero en una cuenta", new String[]{"cuenta", "importe"}),
    RETIRO("retiro", "Realizar Retiro", "Retirar dinero de una cuenta", new String[]{"cuenta", "importe"}),
    TRANSFERENCIA("transferencia", "Realizar Transferencia", "Transferir dinero entre cuentas", new String[]{"cuentaOrigen", "cuentaDestino", "importe"});

    private final String code;
    private final String displayName;
    private final String description;
    private final String[] parameters;

    BankingOperation(String code, String displayName, String description, String[] parameters) {
        this.code = code;
        this.displayName = displayName;
        this.description = description;
        this.parameters = parameters.clone();
    }

    public String getCode() { return code; }
    public String getDisplayName() { return displayName; }
    public String getDescription() { return description; }
    public String[] getParameters() { return parameters.clone(); }

    public boolean requiresParameters() {
        return parameters.length > 0;
    }

    public static BankingOperation getByCode(String code) {
        for (BankingOperation operation : values()) {
            if (operation.getCode().equalsIgnoreCase(code)) {
                return operation;
            }
        }
        throw new IllegalArgumentException("Operación no válida: " + code);
    }

    @Override
    public String toString() {
        return displayName;
    }
}
