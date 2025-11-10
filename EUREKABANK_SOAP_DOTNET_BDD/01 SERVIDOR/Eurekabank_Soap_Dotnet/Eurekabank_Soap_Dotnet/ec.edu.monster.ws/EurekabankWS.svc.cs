using Eurekabank_Soap_Dotnet.ec.edu.monster.modelo;
using Eurekabank_Soap_Dotnet.ec.edu.monster.servicio;
using System;
using System.Collections.Generic;

namespace Eurekabank_Soap_Dotnet.ec.edu.monster.ws
{
    public class EurekabankWS : IEurekabankWS
    {
        public List<Movimiento> ObtenerPorCuenta(string cuenta)
        {
            return EurekaService.ListarPorCuenta(cuenta);
        }

        public string RegistrarDeposito(string cuenta, double importe)
        {
            EurekaService.RegistrarDeposito(cuenta, importe, "0001");
            return "1";
        }

        public string RegistrarRetiro(string cuenta, double importe)
        {
            EurekaService.RegistrarRetiro(cuenta, importe, "0001");
            return "1";
        }

        public string RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe)
        {
            EurekaService.RegistrarTransferencia(cuentaOrigen, cuentaDestino, importe, "0001");
            return "1";
        }

        public bool Login(string username, string password)
        {
            return LoginService.Login(username, password);
        }

        public string Health()
        {
            return "Servicio Eurekabank SOAP activo y funcionando correctamente";
        }
    }
}
