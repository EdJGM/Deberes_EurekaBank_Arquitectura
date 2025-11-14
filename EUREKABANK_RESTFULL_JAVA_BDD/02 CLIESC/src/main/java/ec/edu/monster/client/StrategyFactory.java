package ec.edu.monster.client;

/**
 * Fábrica que crea las estrategias apropiadas para cada servicio de EUREKABANK
 */
public class StrategyFactory {

    public ServiceStrategy getStrategy(int serviceNumber) {
        switch (serviceNumber) {
            case 1:
                return new UserServiceStrategy();
            case 2:
                return new ProductServiceStrategy();
            case 3:
                return new OrderServiceStrategy();
            case 4:
                return new ReportServiceStrategy();
            default:
                return new DefaultServiceStrategy();
        }
    }
}
