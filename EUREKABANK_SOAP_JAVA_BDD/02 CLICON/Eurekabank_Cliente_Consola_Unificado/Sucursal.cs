using System;
using System.Collections.Generic;

namespace Eurekabank_Cliente_Consola_Unificado.Models
{
    // Modelo de Sucursal
    public class Sucursal
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public int ContadorCuentas { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        public override string ToString()
        {
            return $"[{Codigo}] {Nombre} - {Ciudad} ({Estado})";
        }
    }

    // Respuesta para cálculo de distancia
    public class DistanciaResponse
    {
        public double Distancia { get; set; }
        public string SucursalOrigen { get; set; }
        public string SucursalDestino { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

    // Modelo para sucursal con distancia calculada
    public class SucursalConDistancia
    {
        public Sucursal Sucursal { get; set; }
        public double DistanciaKm { get; set; }
        public double? TiempoMinutos { get; set; }

        public override string ToString()
        {
            return $"{Sucursal.Nombre} - {DistanciaKm:F2} km ({Sucursal.Ciudad})";
        }
    }

    // Request para crear/actualizar sucursal
    public class SucursalRequest
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public int ContadorCuentas { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; } = "ACTIVO";
    }

    // Response para operaciones de sucursal
    public class SucursalOperacionResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public Sucursal Sucursal { get; set; }
        public List<Sucursal> Sucursales { get; set; }
    }

    // Coordenadas geográficas
    public class Coordenadas
    {
        public double Latitud { get; set; }
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

    // Enum para tipos de búsqueda de sucursales
    public enum TipoBusquedaSucursal
    {
        Todas,
        PorCodigo,
        PorCiudad,
        Activas,
        MasCercanas
    }
}