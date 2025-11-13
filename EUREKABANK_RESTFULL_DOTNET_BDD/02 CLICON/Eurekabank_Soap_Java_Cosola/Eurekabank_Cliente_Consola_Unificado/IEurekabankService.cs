using System.Collections.Generic;
using System.Threading.Tasks;
using Eurekabank_Cliente_Consola_Unificado.Models;

namespace Eurekabank_Cliente_Consola_Unificado.Services
{
    /// <summary>
    /// Interfaz común para todos los servicios bancarios
    /// </summary>
    public interface IEurekabankService
    {
        /// <summary>
        /// Verifica el estado del servidor
        /// </summary>
        Task<OperacionResult> Health();

        /// <summary>
        /// Realiza el login del usuario
        /// </summary>
        Task<OperacionResult> Login(string username, string password);

        /// <summary>
        /// Obtiene los movimientos de una cuenta
        /// </summary>
        Task<OperacionResult> ObtenerMovimientos(string cuenta);

        /// <summary>
        /// Registra un depósito
        /// </summary>
        Task<OperacionResult> RegistrarDeposito(string cuenta, double importe);

        /// <summary>
        /// Registra un retiro
        /// </summary>
        Task<OperacionResult> RegistrarRetiro(string cuenta, double importe);

        /// <summary>
        /// Registra una transferencia
        /// </summary>
        Task<OperacionResult> RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe);
    }
}
