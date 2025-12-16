using Eurekabank_Restfull_Dotnet.Models;
using Eurekabank_Restfull_Dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eurekabank_Restfull_Dotnet.Controllers
{
    /// <summary>
    /// Controlador REST para operaciones CRUD de Sucursales
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        /// <summary>
        /// Verificar estado del servicio de sucursales
        /// </summary>
        /// <returns>Estado del servicio</returns>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new ApiResponse<object>(
                new
                {
                    status = "healthy",
                    service = "Sucursales REST API",
                    timestamp = DateTime.Now,
                    version = "1.0"
                },
                "Servicio de Sucursales activo y funcionando correctamente"
            ));
        }

        /// <summary>
        /// Obtener todas las sucursales
        /// </summary>
        /// <returns>Lista de sucursales</returns>
        [HttpGet]
        public async Task<IActionResult> ListarSucursales()
        {
            try
            {
                var sucursales = await _sucursalService.ListarSucursalesAsync();

                return Ok(new ApiResponse<List<Sucursal>>(
                    sucursales,
                    $"Se encontraron {sucursales.Count} sucursales"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al obtener sucursales: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener sucursal por código
        /// </summary>
        /// <param name="codigo">Código de la sucursal</param>
        /// <returns>Sucursal encontrada</returns>
        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObtenerSucursal(string codigo)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de sucursal es requerido"));
                }

                var sucursal = await _sucursalService.ObtenerSucursalAsync(codigo);

                if (sucursal == null)
                {
                    return NotFound(ApiResponse<object>.Error($"Sucursal con código {codigo} no encontrada"));
                }

                return Ok(new ApiResponse<Sucursal>(
                    sucursal,
                    "Sucursal encontrada exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al obtener sucursal: {ex.Message}"));
            }
        }

        /// <summary>
        /// Crear nueva sucursal
        /// </summary>
        /// <param name="request">Datos de la sucursal a crear</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost]
        public async Task<IActionResult> CrearSucursal([FromBody] SucursalRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.Error("Datos de entrada inválidos"));
                }

                // Verificar si la sucursal ya existe
                var sucursalExistente = await _sucursalService.ObtenerSucursalAsync(request.Codigo);
                if (sucursalExistente != null)
                {
                    return Conflict(ApiResponse<object>.Error($"Ya existe una sucursal con el código {request.Codigo}"));
                }

                var resultado = await _sucursalService.CrearSucursalAsync(request);

                if (resultado)
                {
                    var sucursalCreada = await _sucursalService.ObtenerSucursalAsync(request.Codigo);
                    return CreatedAtAction(
                        nameof(ObtenerSucursal),
                        new { codigo = request.Codigo },
                        new ApiResponse<Sucursal>(
                            sucursalCreada!,
                            "Sucursal creada exitosamente"
                        )
                    );
                }
                else
                {
                    return BadRequest(ApiResponse<object>.Error("No se pudo crear la sucursal"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al crear sucursal: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar sucursal existente
        /// </summary>
        /// <param name="codigo">Código de la sucursal a actualizar</param>
        /// <param name="request">Datos actualizados de la sucursal</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{codigo}")]
        public async Task<IActionResult> ActualizarSucursal(string codigo, [FromBody] SucursalRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de sucursal es requerido"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.Error("Datos de entrada inválidos"));
                }

                if (codigo != request.Codigo)
                {
                    return BadRequest(ApiResponse<object>.Error("El código en la URL no coincide con el código en el cuerpo de la petición"));
                }

                // Verificar si la sucursal existe
                var sucursalExistente = await _sucursalService.ObtenerSucursalAsync(codigo);
                if (sucursalExistente == null)
                {
                    return NotFound(ApiResponse<object>.Error($"Sucursal con código {codigo} no encontrada"));
                }

                var resultado = await _sucursalService.ActualizarSucursalAsync(request);

                if (resultado)
                {
                    var sucursalActualizada = await _sucursalService.ObtenerSucursalAsync(codigo);
                    return Ok(new ApiResponse<Sucursal>(
                        sucursalActualizada!,
                        "Sucursal actualizada exitosamente"
                    ));
                }
                else
                {
                    return BadRequest(ApiResponse<object>.Error("No se pudo actualizar la sucursal"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al actualizar sucursal: {ex.Message}"));
            }
        }

        /// <summary>
        /// Eliminar sucursal (cambiar estado a INACTIVO)
        /// </summary>
        /// <param name="codigo">Código de la sucursal a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> EliminarSucursal(string codigo)
        {
            try
            {
                if (string.IsNullOrEmpty(codigo))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de sucursal es requerido"));
                }

                // Verificar si la sucursal existe
                var sucursalExistente = await _sucursalService.ObtenerSucursalAsync(codigo);
                if (sucursalExistente == null)
                {
                    return NotFound(ApiResponse<object>.Error($"Sucursal con código {codigo} no encontrada"));
                }

                var resultado = await _sucursalService.EliminarSucursalAsync(codigo);

                if (resultado)
                {
                    return Ok(new ApiResponse<object>(
                        new { codigo = codigo, estado = "INACTIVO" },
                        "Sucursal eliminada (desactivada) exitosamente"
                    ));
                }
                else
                {
                    return BadRequest(ApiResponse<object>.Error("No se pudo eliminar la sucursal"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al eliminar sucursal: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calcular distancia entre dos sucursales
        /// </summary>
        /// <param name="codigoOrigen">Código de la sucursal origen</param>
        /// <param name="codigoDestino">Código de la sucursal destino</param>
        /// <returns>Información de distancia</returns>
        [HttpGet("distancia/{codigoOrigen}/{codigoDestino}")]
        public async Task<IActionResult> CalcularDistanciaEntreSucursales(string codigoOrigen, string codigoDestino)
        {
            try
            {
                if (string.IsNullOrEmpty(codigoOrigen) || string.IsNullOrEmpty(codigoDestino))
                {
                    return BadRequest(ApiResponse<object>.Error("Los códigos de sucursal origen y destino son requeridos"));
                }

                if (codigoOrigen == codigoDestino)
                {
                    return BadRequest(ApiResponse<object>.Error("Los códigos de sucursal origen y destino no pueden ser iguales"));
                }

                var resultado = await _sucursalService.CalcularDistanciaEntreSucursalesAsync(codigoOrigen, codigoDestino);

                if (resultado == null)
                {
                    return NotFound(ApiResponse<object>.Error("Una o ambas sucursales no fueron encontradas"));
                }

                return Ok(new ApiResponse<DistanciaResponse>(
                    resultado,
                    $"Distancia calculada: {resultado.DistanciaKm} km"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al calcular distancia: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calcular distancia desde posición actual a una sucursal
        /// </summary>
        /// <param name="request">Datos de posición origen y código de sucursal destino</param>
        /// <returns>Información de distancia</returns>
        [HttpPost("distancia-a-sucursal")]
        public async Task<IActionResult> CalcularDistanciaASucursal([FromBody] DistanciaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.Error("Datos de entrada inválidos"));
                }

                if (string.IsNullOrEmpty(request.CodigoSucursal))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de sucursal es requerido"));
                }

                var resultado = await _sucursalService.CalcularDistanciaASucursalAsync(request);

                if (resultado == null)
                {
                    return NotFound(ApiResponse<object>.Error("Sucursal no encontrada"));
                }

                return Ok(new ApiResponse<DistanciaResponse>(
                    resultado,
                    $"Distancia calculada: {resultado.DistanciaKm} km"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al calcular distancia: {ex.Message}"));
            }
        }

        /// <summary>
        /// Encontrar sucursal más cercana a una posición
        /// </summary>
        /// <param name="latitud">Latitud de la posición actual</param>
        /// <param name="longitud">Longitud de la posición actual</param>
        /// <returns>Sucursal más cercana</returns>
        [HttpGet("mas-cercana")]
        public async Task<IActionResult> EncontrarSucursalMasCercana([FromQuery] double latitud, [FromQuery] double longitud)
        {
            try
            {
                if (latitud == 0 && longitud == 0)
                {
                    return BadRequest(ApiResponse<object>.Error("Las coordenadas de latitud y longitud son requeridas"));
                }

                var resultado = await _sucursalService.EncontrarSucursalMasCercanaAsync(latitud, longitud);

                if (resultado == null)
                {
                    return NotFound(ApiResponse<object>.Error("No se encontraron sucursales activas con coordenadas válidas"));
                }

                return Ok(new ApiResponse<SucursalConDistancia>(
                    resultado,
                    $"Sucursal más cercana: {resultado.Sucursal.Nombre} a {resultado.DistanciaKm} km"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al buscar sucursal más cercana: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener sucursales ordenadas por distancia desde una posición
        /// </summary>
        /// <param name="latitud">Latitud de la posición actual</param>
        /// <param name="longitud">Longitud de la posición actual</param>
        /// <param name="limite">Número máximo de sucursales a retornar</param>
        /// <returns>Lista de sucursales con distancias</returns>
        [HttpGet("con-distancias")]
        public async Task<IActionResult> ObtenerSucursalesConDistancias([FromQuery] double latitud, [FromQuery] double longitud, [FromQuery] int limite = 10)
        {
            try
            {
                if (latitud == 0 && longitud == 0)
                {
                    return BadRequest(ApiResponse<object>.Error("Las coordenadas de latitud y longitud son requeridas"));
                }

                if (limite <= 0 || limite > 50)
                {
                    return BadRequest(ApiResponse<object>.Error("El límite debe estar entre 1 y 50"));
                }

                var resultado = await _sucursalService.ObtenerSucursalesConDistanciasAsync(latitud, longitud, limite);

                return Ok(new ApiResponse<List<SucursalConDistancia>>(
                    resultado,
                    $"Se encontraron {resultado.Count} sucursales ordenadas por distancia"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al obtener sucursales con distancias: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtener sucursales por ciudad
        /// </summary>
        /// <param name="ciudad">Nombre de la ciudad</param>
        /// <returns>Lista de sucursales de la ciudad</returns>
        [HttpGet("ciudad/{ciudad}")]
        public async Task<IActionResult> ObtenerSucursalesPorCiudad(string ciudad)
        {
            try
            {
                if (string.IsNullOrEmpty(ciudad))
                {
                    return BadRequest(ApiResponse<object>.Error("El nombre de la ciudad es requerido"));
                }

                var sucursales = await _sucursalService.ObtenerSucursalesPorCiudadAsync(ciudad);

                return Ok(new ApiResponse<List<Sucursal>>(
                    sucursales,
                    $"Se encontraron {sucursales.Count} sucursales en {ciudad}"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al obtener sucursales por ciudad: {ex.Message}"));
            }
        }
    }
}