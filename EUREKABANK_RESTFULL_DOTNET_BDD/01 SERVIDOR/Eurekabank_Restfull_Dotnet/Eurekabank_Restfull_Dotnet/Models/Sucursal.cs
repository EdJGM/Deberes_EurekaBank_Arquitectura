using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Eurekabank_Restfull_Dotnet.Models
{
    /// <summary>
    /// Modelo de datos para Sucursal
    /// </summary>
    public class Sucursal
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [JsonPropertyName("contadorCuentas")]
        public int ContadorCuentas { get; set; }

        [JsonPropertyName("latitud")]
        public double Latitud { get; set; }

        [JsonPropertyName("longitud")]
        public double Longitud { get; set; }

        [JsonPropertyName("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "ACTIVO";

        public override string ToString()
        {
            return $"[{Codigo}] {Nombre} - {Ciudad} ({Estado})";
        }
    }

    /// <summary>
    /// Request para crear/actualizar sucursal
    /// </summary>
    public class SucursalRequest
    {
        [Required]
        [StringLength(3)]
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [JsonPropertyName("ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [JsonPropertyName("contadorCuentas")]
        public int ContadorCuentas { get; set; } = 0;

        [JsonPropertyName("latitud")]
        public double Latitud { get; set; }

        [JsonPropertyName("longitud")]
        public double Longitud { get; set; }

        [StringLength(20)]
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "ACTIVO";
    }

    /// <summary>
    /// Response para cálculo de distancia
    /// </summary>
    public class DistanciaResponse
    {
        [JsonPropertyName("distanciaKm")]
        public double DistanciaKm { get; set; }

        [JsonPropertyName("sucursalOrigen")]
        public string? SucursalOrigen { get; set; }

        [JsonPropertyName("sucursalDestino")]
        public string? SucursalDestino { get; set; }

        [JsonPropertyName("coordenadasOrigen")]
        public Coordenadas? CoordenadasOrigen { get; set; }

        [JsonPropertyName("coordenadasDestino")]
        public Coordenadas? CoordenadasDestino { get; set; }

        [JsonPropertyName("tiempoEstimadoMinutos")]
        public double? TiempoEstimadoMinutos { get; set; }
    }

    /// <summary>
    /// Modelo para coordenadas geográficas
    /// </summary>
    public class Coordenadas
    {
        [JsonPropertyName("latitud")]
        public double Latitud { get; set; }

        [JsonPropertyName("longitud")]
        public double Longitud { get; set; }

        public Coordenadas() { }

        public Coordenadas(double latitud, double longitud)
        {
            Latitud = latitud;
            Longitud = longitud;
        }

        public override string ToString()
        {
            return $"{Latitud:F6}, {Longitud:F6}";
        }
    }

    /// <summary>
    /// Request para cálculo de distancia desde posición actual
    /// </summary>
    public class DistanciaRequest
    {
        [JsonPropertyName("latitudOrigen")]
        public double LatitudOrigen { get; set; }

        [JsonPropertyName("longitudOrigen")]
        public double LongitudOrigen { get; set; }

        [JsonPropertyName("codigoSucursal")]
        public string CodigoSucursal { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response con sucursal y su distancia
    /// </summary>
    public class SucursalConDistancia
    {
        [JsonPropertyName("sucursal")]
        public Sucursal Sucursal { get; set; } = new();

        [JsonPropertyName("distanciaKm")]
        public double DistanciaKm { get; set; }

        [JsonPropertyName("tiempoEstimadoMinutos")]
        public double? TiempoEstimadoMinutos { get; set; }

        public override string ToString()
        {
            return $"{Sucursal.Nombre} - {DistanciaKm:F2} km ({Sucursal.Ciudad})";
        }
    }
}