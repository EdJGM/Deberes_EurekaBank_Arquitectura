using Eurekabank_Restfull_Dotnet.Models;

namespace Eurekabank_Restfull_Dotnet.Services.Interfaces
{
    /// <summary>
    /// Interface para servicios de Sucursal
    /// </summary>
    public interface ISucursalService
    {
        /// <summary>
        /// Obtener todas las sucursales
        /// </summary>
        /// <returns>Lista de sucursales</returns>
        Task<List<Sucursal>> ListarSucursalesAsync();

        /// <summary>
        /// Obtener sucursal por código
        /// </summary>
        /// <param name="codigo">Código de la sucursal</param>
        /// <returns>Sucursal encontrada o null</returns>
        Task<Sucursal?> ObtenerSucursalAsync(string codigo);

        /// <summary>
        /// Crear nueva sucursal
        /// </summary>
        /// <param name="sucursal">Datos de la sucursal</param>
        /// <returns>True si se creó correctamente</returns>
        Task<bool> CrearSucursalAsync(SucursalRequest sucursal);

        /// <summary>
        /// Actualizar sucursal existente
        /// </summary>
        /// <param name="sucursal">Datos actualizados de la sucursal</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> ActualizarSucursalAsync(SucursalRequest sucursal);

        /// <summary>
        /// Eliminar sucursal (cambiar estado a INACTIVO)
        /// </summary>
        /// <param name="codigo">Código de la sucursal</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> EliminarSucursalAsync(string codigo);

        /// <summary>
        /// Calcular distancia entre dos sucursales
        /// </summary>
        /// <param name="codigoSucursal1">Código de la primera sucursal</param>
        /// <param name="codigoSucursal2">Código de la segunda sucursal</param>
        /// <returns>Información de distancia</returns>
        Task<DistanciaResponse?> CalcularDistanciaEntreSucursalesAsync(string codigoSucursal1, string codigoSucursal2);

        /// <summary>
        /// Calcular distancia desde posición actual a una sucursal
        /// </summary>
        /// <param name="request">Datos de posición y sucursal destino</param>
        /// <returns>Información de distancia</returns>
        Task<DistanciaResponse?> CalcularDistanciaASucursalAsync(DistanciaRequest request);

        /// <summary>
        /// Encontrar sucursal más cercana a una posición
        /// </summary>
        /// <param name="latitud">Latitud de la posición actual</param>
        /// <param name="longitud">Longitud de la posición actual</param>
        /// <returns>Sucursal más cercana</returns>
        Task<SucursalConDistancia?> EncontrarSucursalMasCercanaAsync(double latitud, double longitud);

        /// <summary>
        /// Obtener sucursales ordenadas por distancia desde una posición
        /// </summary>
        /// <param name="latitud">Latitud de la posición actual</param>
        /// <param name="longitud">Longitud de la posición actual</param>
        /// <param name="limite">Número máximo de sucursales a retornar</param>
        /// <returns>Lista de sucursales con distancias</returns>
        Task<List<SucursalConDistancia>> ObtenerSucursalesConDistanciasAsync(double latitud, double longitud, int limite = 10);

        /// <summary>
        /// Obtener sucursales por ciudad
        /// </summary>
        /// <param name="ciudad">Nombre de la ciudad</param>
        /// <returns>Lista de sucursales de la ciudad</returns>
        Task<List<Sucursal>> ObtenerSucursalesPorCiudadAsync(string ciudad);
    }
}