using Eurekabank_Soap_Dotnet.ec.edu.monster.modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Eurekabank_Soap_Dotnet.ec.edu.monster.ws
{
    [ServiceContract]
    public interface IEurekabankWS
    {
        [OperationContract]
        List<Movimiento> ObtenerPorCuenta(string cuenta);

        [OperationContract]
        string RegistrarDeposito(string cuenta, double importe);

        [OperationContract]
        string RegistrarRetiro(string cuenta, double importe);

        [OperationContract]
        string RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe);

        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        string Health();
    }
}
