using System.Text.Json.Serialization;

namespace Eurekabank_Restfull_Dotnet.Models
{
    // ============ ENTIDADES ============
    public class Movimiento
    {
        [JsonPropertyName("cuenta")]
        public string Cuenta { get; set; }

        [JsonPropertyName("nroMov")]
        public int NroMov { get; set; }

        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("accion")]
        public string Accion { get; set; }

        [JsonPropertyName("importe")]
        public double Importe { get; set; }
    }

    // ============ REQUEST DTOs ============

    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class DepositoRequest
    {
        [JsonPropertyName("cuenta")]
        public string Cuenta { get; set; }

        [JsonPropertyName("importe")]
        public double Importe { get; set; }
    }

    public class RetiroRequest
    {
        [JsonPropertyName("cuenta")]
        public string Cuenta { get; set; }

        [JsonPropertyName("importe")]
        public double Importe { get; set; }
    }

    public class TransferenciaRequest
    {
        [JsonPropertyName("cuentaOrigen")]
        public string CuentaOrigen { get; set; }

        [JsonPropertyName("cuentaDestino")]
        public string CuentaDestino { get; set; }

        [JsonPropertyName("importe")]
        public double Importe { get; set; }
    }

    // ============ RESPONSE DTOs ============

    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        public ApiResponse()
        {
            Success = true;
        }

        public ApiResponse(T data, string message = "Operación exitosa")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Error(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T)
            };
        }
    }

    public class LoginResponse
    {
        [JsonPropertyName("authenticated")]
        public bool Authenticated { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
