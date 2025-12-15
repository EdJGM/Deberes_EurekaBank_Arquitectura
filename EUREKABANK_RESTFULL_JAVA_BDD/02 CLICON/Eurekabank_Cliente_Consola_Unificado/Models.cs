using System;

namespace Eurekabank_Cliente_Consola_Unificado.Models
{
    // Modelo de Movimiento
    public class Movimiento
    {
        public string Cuenta { get; set; }
        public int NroMov { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Accion { get; set; }
        public double Importe { get; set; }

        public override string ToString()
        {
            return $"[{NroMov}] {Fecha:dd/MM/yyyy} - {Tipo} ({Accion}) - S/. {Importe:N2}";
        }
    }

    // Modelo de Usuario
    public class Usuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Enum de tipos de servidor
    public enum TipoServidor
    {
        SOAP_DOTNET = 1,
        SOAP_JAVA = 2,
        REST_DOTNET = 3,
        REST_JAVA = 4
    }

    // Respuesta de operación
    public class OperacionResult
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public object Data { get; set; }
    }

    // DTOs para REST .NET
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DepositoRequest
    {
        public string Cuenta { get; set; }
        public double Importe { get; set; }
    }

    public class RetiroRequest
    {
        public string Cuenta { get; set; }
        public double Importe { get; set; }
    }

    public class TransferenciaRequest
    {
        public string CuentaOrigen { get; set; }
        public string CuentaDestino { get; set; }
        public double Importe { get; set; }
    }
}
