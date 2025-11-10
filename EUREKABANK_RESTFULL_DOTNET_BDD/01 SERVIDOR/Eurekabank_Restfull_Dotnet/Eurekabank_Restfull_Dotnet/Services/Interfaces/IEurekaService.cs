using Eurekabank_Restfull_Dotnet.Models;

namespace Eurekabank_Restfull_Dotnet.Services.Interfaces
{
    public interface IEurekaService
    {
        List<Movimiento> ListarPorCuenta(string cuenta);
        void RegistrarDeposito(string cuenta, double importe, string codEmp = "0001");
        void RegistrarRetiro(string cuenta, double importe, string codEmp = "0001");
        void RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe, string codEmp = "0001");
    }
}
