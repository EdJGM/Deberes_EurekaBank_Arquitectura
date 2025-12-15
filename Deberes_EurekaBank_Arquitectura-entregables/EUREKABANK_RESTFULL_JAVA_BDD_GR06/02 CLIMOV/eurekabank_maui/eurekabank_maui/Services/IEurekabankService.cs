using Eurekabank_Maui.Models;

namespace Eurekabank_Maui.Services
{
    /// <summary>
    /// Interfaz común para todos los servidores Eurekabank.
    /// Permite abstraer la implementación específica (SOAP/REST, .NET/Java)
    /// </summary>
    public interface IEurekabankService
    {
        /// <summary>
        /// Verifica si el servidor está activo y respondiendo
        /// </summary>
        Task<bool> HealthCheckAsync();

        /// <summary>
        /// Autentica un usuario en el sistema
        /// </summary>
        Task<bool> LoginAsync(string username, string password);

        /// <summary>
        /// Obtiene la lista de movimientos de una cuenta
        /// </summary>
        Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta);

        /// <summary>
        /// Registra un depósito en una cuenta
        /// </summary>
        Task<bool> RegistrarDepositoAsync(string cuenta, double importe);

        /// <summary>
        /// Registra un retiro de una cuenta
        /// </summary>
        Task<bool> RegistrarRetiroAsync(string cuenta, double importe);

        /// <summary>
        /// Registra una transferencia entre cuentas
        /// </summary>
        Task<bool> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, double importe);

        /// <summary>
        /// Obtiene la información del servidor actual
        /// </summary>
        ServidorConfig GetServidorInfo();
    }
}
