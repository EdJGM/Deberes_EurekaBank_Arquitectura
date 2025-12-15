using Eurekabank_Restfull_Dotnet.Models;
using Eurekabank_Restfull_Dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eurekabank_Restfull_Dotnet.Controllers
{
    // ============ HEALTH CHECK CONTROLLER ============

    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new ApiResponse<object>(
                new
                {
                    status = "healthy",
                    service = "Eurekabank REST API",
                    timestamp = DateTime.Now
                },
                "Servicio activo y funcionando correctamente"
            ));
        }
    }

    // ============ AUTHENTICATION CONTROLLER ============

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// Autenticar usuario
        /// </summary>
        /// <param name="request">Credenciales de usuario</param>
        /// <returns>Resultado de autenticación</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(ApiResponse<object>.Error("Usuario y contraseña son requeridos"));
                }

                bool isAuthenticated = _loginService.Login(request.Username, request.Password);

                if (isAuthenticated)
                {
                    var loginResponse = new LoginResponse
                    {
                        Authenticated = true,
                        Username = request.Username
                    };

                    return Ok(new ApiResponse<LoginResponse>(
                        loginResponse,
                        "Autenticación exitosa"
                    ));
                }
                else
                {
                    return Unauthorized(ApiResponse<object>.Error("Credenciales inválidas"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error en autenticación: {ex.Message}"));
            }
        }
    }

    // ============ MOVIMIENTOS CONTROLLER ============

    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly IEurekaService _eurekaService;

        public MovimientosController(IEurekaService eurekaService)
        {
            _eurekaService = eurekaService;
        }

        /// <summary>
        /// Obtener movimientos por cuenta
        /// </summary>
        /// <param name="cuenta">Código de cuenta</param>
        /// <returns>Lista de movimientos</returns>
        [HttpGet("cuenta/{cuenta}")]
        public IActionResult ObtenerPorCuenta(string cuenta)
        {
            try
            {
                if (string.IsNullOrEmpty(cuenta))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de cuenta es requerido"));
                }

                var movimientos = _eurekaService.ListarPorCuenta(cuenta);

                return Ok(new ApiResponse<List<Movimiento>>(
                    movimientos,
                    $"Se encontraron {movimientos.Count} movimientos"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al obtener movimientos: {ex.Message}"));
            }
        }

        /// <summary>
        /// Registrar depósito
        /// </summary>
        /// <param name="request">Datos del depósito</param>
        /// <returns>Confirmación de operación</returns>
        [HttpPost("deposito")]
        public IActionResult RegistrarDeposito([FromBody] DepositoRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Cuenta))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de cuenta es requerido"));
                }

                if (request.Importe <= 0)
                {
                    return BadRequest(ApiResponse<object>.Error("El importe debe ser mayor a cero"));
                }

                _eurekaService.RegistrarDeposito(request.Cuenta, request.Importe);

                return Ok(new ApiResponse<object>(
                    new { operacion = "deposito", cuenta = request.Cuenta, importe = request.Importe },
                    "Depósito registrado exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al registrar depósito: {ex.Message}"));
            }
        }

        /// <summary>
        /// Registrar retiro
        /// </summary>
        /// <param name="request">Datos del retiro</param>
        /// <returns>Confirmación de operación</returns>
        [HttpPost("retiro")]
        public IActionResult RegistrarRetiro([FromBody] RetiroRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Cuenta))
                {
                    return BadRequest(ApiResponse<object>.Error("El código de cuenta es requerido"));
                }

                if (request.Importe <= 0)
                {
                    return BadRequest(ApiResponse<object>.Error("El importe debe ser mayor a cero"));
                }

                _eurekaService.RegistrarRetiro(request.Cuenta, request.Importe);

                return Ok(new ApiResponse<object>(
                    new { operacion = "retiro", cuenta = request.Cuenta, importe = request.Importe },
                    "Retiro registrado exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al registrar retiro: {ex.Message}"));
            }
        }

        /// <summary>
        /// Registrar transferencia
        /// </summary>
        /// <param name="request">Datos de la transferencia</param>
        /// <returns>Confirmación de operación</returns>
        [HttpPost("transferencia")]
        public IActionResult RegistrarTransferencia([FromBody] TransferenciaRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.CuentaOrigen) || string.IsNullOrEmpty(request.CuentaDestino))
                {
                    return BadRequest(ApiResponse<object>.Error("Las cuentas origen y destino son requeridas"));
                }

                if (request.Importe <= 0)
                {
                    return BadRequest(ApiResponse<object>.Error("El importe debe ser mayor a cero"));
                }

                if (request.CuentaOrigen == request.CuentaDestino)
                {
                    return BadRequest(ApiResponse<object>.Error("La cuenta origen y destino no pueden ser iguales"));
                }

                _eurekaService.RegistrarTransferencia(
                    request.CuentaOrigen,
                    request.CuentaDestino,
                    request.Importe
                );

                return Ok(new ApiResponse<object>(
                    new
                    {
                        operacion = "transferencia",
                        cuentaOrigen = request.CuentaOrigen,
                        cuentaDestino = request.CuentaDestino,
                        importe = request.Importe
                    },
                    "Transferencia registrada exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Error($"Error al registrar transferencia: {ex.Message}"));
            }
        }
    }
}
