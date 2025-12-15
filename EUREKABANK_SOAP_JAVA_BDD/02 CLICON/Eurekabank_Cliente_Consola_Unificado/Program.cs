using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eurekabank_Cliente_Consola_Unificado.Models;
using Eurekabank_Cliente_Consola_Unificado.Services;

namespace Eurekabank_Cliente_Consola_Unificado
{
    class Program
    {
        private static IEurekabankService? servicioActual;
        private static TipoServidor servidorSeleccionado;
        private static SucursalSoapService? servicioSucursales;
        private static GoogleDirectionsService? serviceDirecciones;
        private static bool autenticado = false;
        private static string usuarioActual = "";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                MostrarBienvenida();
                
                if (!await SeleccionarServidor())
                {
                    Console.WriteLine("\n❌ No se pudo conectar al servidor. Presione cualquier tecla para reintentar...");
                    Console.ReadKey();
                    continue;
                }

                if (!await RealizarLogin())
                {
                    Console.WriteLine("\n❌ Login fallido. Presione cualquier tecla para reintentar...");
                    Console.ReadKey();
                    continue;
                }

                await MenuPrincipal();

                Console.WriteLine("\n¿Desea conectarse a otro servidor? (S/N): ");
                var respuesta = Console.ReadLine()?.ToUpper();
                if (respuesta != "S")
                    break;

                autenticado = false;
            }

            Console.WriteLine("\n👋 Gracias por usar Eurekabank. ¡Hasta pronto!");
        }

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║        🏦  EUREKABANK - CLIENTE CONSOLA UNIFICADO  🏦      ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║          Sistema de Gestión Bancaria Multiplataforma      ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        static async Task<bool> SeleccionarServidor()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("📡 SELECCIÓN DE SERVIDOR");
            Console.WriteLine("========================");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Seleccione el servidor al que desea conectarse:");
            Console.WriteLine();
            Console.WriteLine("  1️⃣  SOAP .NET     (Puerto 57199)");
            Console.WriteLine("  2️⃣  SOAP Java     (Puerto 8080)");
            Console.WriteLine("  3️⃣  REST .NET     (Puerto 5111)");
            Console.WriteLine("  4️⃣  REST Java     (Puerto 8080)");
            Console.WriteLine();
            Console.Write("Ingrese su opción (1-4): ");

            if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 1 || opcion > 4)
            {
                Console.WriteLine("❌ Opción inválida.");
                return false;
            }

            servidorSeleccionado = (TipoServidor)opcion;

            // Crear instancia del servicio correspondiente
            servicioActual = servidorSeleccionado switch
            {
                TipoServidor.SOAP_DOTNET => new SoapDotNetService(),
                TipoServidor.SOAP_JAVA => new SoapJavaService(),
                TipoServidor.REST_DOTNET => new RestDotNetService(),
                TipoServidor.REST_JAVA => new RestJavaService(),
                _ => null
            };

            if (servidorSeleccionado == TipoServidor.SOAP_JAVA)
            {
                servicioSucursales = new SucursalSoapService();
                serviceDirecciones = new GoogleDirectionsService();
            }

            if (servicioActual == null)
            {
                Console.WriteLine("❌ Error al crear el servicio.");
                return false;
            }

            // Verificar estado del servidor
            Console.WriteLine();
            Console.Write("🔍 Verificando conexión con el servidor");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual.Health();
            
            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Conectado exitosamente a: {servidorSeleccionado}");
                Console.ResetColor();
                Console.WriteLine($"   {resultado.Mensaje}");
                await Task.Delay(1500);
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ No se pudo conectar al servidor: {resultado.Mensaje}");
                Console.ResetColor();
                return false;
            }
        }

        static async Task<bool> RealizarLogin()
        {
            Console.Clear();
            MostrarEncabezado($"🔐 INICIO DE SESIÓN - {servidorSeleccionado}");

            Console.Write("👤 Usuario: ");
            string? username = Console.ReadLine();
            
            Console.Write("🔑 Contraseña: ");
            string password = LeerPasswordOculto();
            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("❌ Usuario o contraseña vacíos.");
                return false;
            }

            Console.WriteLine();
            Console.Write("🔍 Autenticando");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual!.Login(username, password);

            if (resultado.Exito)
            {
                autenticado = true;
                usuarioActual = username;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.WriteLine($"   Bienvenido, {username}!");
                Console.ResetColor();
                await Task.Delay(1500);
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
                return false;
            }
        }

        static async Task MenuPrincipal()
        {
            while (autenticado)
            {
                Console.Clear();
                MostrarEncabezado($"💼 MENÚ PRINCIPAL - Usuario: {usuarioActual}");

                Console.WriteLine("  1️⃣  Consultar Movimientos de Cuenta");
                Console.WriteLine("  2️⃣  Realizar Depósito");
                Console.WriteLine("  3️⃣  Realizar Retiro");
                Console.WriteLine("  4️⃣  Realizar Transferencia");
                Console.WriteLine("  5️⃣  Sucursales");
                Console.WriteLine("  6️⃣  Cerrar Sesión");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    Console.WriteLine("❌ Opción inválida.");
                    await Task.Delay(1500);
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        await ConsultarMovimientos();
                        break;
                    case 2:
                        await RealizarDeposito();
                        break;
                    case 3:
                        await RealizarRetiro();
                        break;
                    case 4:
                        await RealizarTransferencia();
                        break;
                    case 5:
                            await MenuSucursales();
                        break;
                    case 6:
                        autenticado = false;
                        Console.WriteLine("\n👋 Sesión cerrada.");
                        await Task.Delay(1500);
                        break;
                    default:
                        Console.WriteLine("❌ Opción inválida.");
                        await Task.Delay(1500);
                        break;
                }
            }
        }

        static async Task ConsultarMovimientos()
        {
            Console.Clear();
            MostrarEncabezado("📊 CONSULTAR MOVIMIENTOS");

            Console.Write("Ingrese el número de cuenta: ");
            string? cuenta = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cuenta))
            {
                Console.WriteLine("❌ Número de cuenta inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔍 Consultando movimientos");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual!.ObtenerMovimientos(cuenta);

            if (resultado.Exito && resultado.Data is List<Movimiento> movimientos)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Se encontraron {movimientos.Count} movimientos:");
                Console.ResetColor();
                Console.WriteLine();

                if (movimientos.Count > 0)
                {
                    Console.WriteLine("┌─────┬────────────┬────────────────────────┬──────────┬────────────┐");
                    Console.WriteLine("│ Nro │   Fecha    │          Tipo          │  Acción  │   Importe  │");
                    Console.WriteLine("├─────┼────────────┼────────────────────────┼──────────┼────────────┤");
                    
                    foreach (var mov in movimientos)
                    {
                        Console.WriteLine($"│ {mov.NroMov,3} │ {mov.Fecha:dd/MM/yyyy} │ {mov.Tipo,-22} │ {mov.Accion,-8} │ S/. {mov.Importe,7:N2} │");
                    }
                    
                    Console.WriteLine("└─────┴────────────┴────────────────────────┴──────────┴────────────┘");
                }
                else
                {
                    Console.WriteLine("ℹ️  No hay movimientos registrados para esta cuenta.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task RealizarDeposito()
        {
            Console.Clear();
            MostrarEncabezado("💰 REALIZAR DEPÓSITO");

            Console.Write("Ingrese el número de cuenta: ");
            string? cuenta = Console.ReadLine();

            Console.Write("Ingrese el importe a depositar: S/. ");
            if (!double.TryParse(Console.ReadLine(), out double importe) || importe <= 0)
            {
                Console.WriteLine("❌ Importe inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  Va a depositar S/. {importe:N2} en la cuenta {cuenta}");
            Console.ResetColor();
            Console.Write("¿Confirma la operación? (S/N): ");
            
            if (Console.ReadLine()?.ToUpper() != "S")
            {
                Console.WriteLine("❌ Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔄 Procesando depósito");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual!.RegistrarDeposito(cuenta!, importe);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task RealizarRetiro()
        {
            Console.Clear();
            MostrarEncabezado("💸 REALIZAR RETIRO");

            Console.Write("Ingrese el número de cuenta: ");
            string? cuenta = Console.ReadLine();

            Console.Write("Ingrese el importe a retirar: S/. ");
            if (!double.TryParse(Console.ReadLine(), out double importe) || importe <= 0)
            {
                Console.WriteLine("❌ Importe inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  Va a retirar S/. {importe:N2} de la cuenta {cuenta}");
            Console.ResetColor();
            Console.Write("¿Confirma la operación? (S/N): ");
            
            if (Console.ReadLine()?.ToUpper() != "S")
            {
                Console.WriteLine("❌ Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔄 Procesando retiro");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual!.RegistrarRetiro(cuenta!, importe);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task RealizarTransferencia()
        {
            Console.Clear();
            MostrarEncabezado("🔄 REALIZAR TRANSFERENCIA");

            Console.Write("Ingrese la cuenta de origen: ");
            string? cuentaOrigen = Console.ReadLine();

            Console.Write("Ingrese la cuenta de destino: ");
            string? cuentaDestino = Console.ReadLine();

            Console.Write("Ingrese el importe a transferir: S/. ");
            if (!double.TryParse(Console.ReadLine(), out double importe) || importe <= 0)
            {
                Console.WriteLine("❌ Importe inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  Va a transferir S/. {importe:N2}");
            Console.WriteLine($"   Desde: {cuentaOrigen}");
            Console.WriteLine($"   Hacia: {cuentaDestino}");
            Console.ResetColor();
            Console.Write("¿Confirma la operación? (S/N): ");
            
            if (Console.ReadLine()?.ToUpper() != "S")
            {
                Console.WriteLine("❌ Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔄 Procesando transferencia");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioActual!.RegistrarTransferencia(cuentaOrigen!, cuentaDestino!, importe);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.WriteLine($" {titulo}");
            Console.WriteLine("════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine();
        }

        static string LeerPasswordOculto()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            return password;
        }

        static async Task MenuSucursales()
        {
            while (true)
            {
                Console.Clear();
                MostrarEncabezado("🏢 GESTIÓN DE SUCURSALES");

                Console.WriteLine("  1️⃣  Listar Todas las Sucursales");
                Console.WriteLine("  2️⃣  Buscar Sucursal por Código");
                Console.WriteLine("  3️⃣  Crear Nueva Sucursal");
                Console.WriteLine("  4️⃣  Actualizar Sucursal");
                Console.WriteLine("  5️⃣  Eliminar Sucursal");
                Console.WriteLine("  6️⃣  Calcular Distancia Entre Sucursales");
                Console.WriteLine("  7️⃣  Encontrar Sucursal Más Cercana");
                Console.WriteLine("  8️⃣  🗺️ Obtener Direcciones a Sucursal");
                Console.WriteLine("  9️⃣  Volver al Menú Principal");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    Console.WriteLine("❌ Opción inválida.");
                    await Task.Delay(1500);
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        await ListarSucursales();
                        break;
                    case 2:
                        await BuscarSucursal();
                        break;
                    case 3:
                        await CrearSucursal();
                        break;
                    case 4:
                        await ActualizarSucursal();
                        break;
                    case 5:
                        await EliminarSucursal();
                        break;
                    case 6:
                        await CalcularDistanciaEntreSucursales();
                        break;
                    case 7:
                        await EncontrarSucursalMasCercana();
                        break;
                    case 8:
                        await ObtenerDireccionesASucursal();
                        break;
                    case 9:
                        return; // Volver al menú principal
                    default:
                        Console.WriteLine("❌ Opción inválida.");
                        await Task.Delay(1500);
                        break;
                }
            }
        }

        // 5. Métodos para las funcionalidades de Sucursales
        static async Task ListarSucursales()
        {
            Console.Clear();
            MostrarEncabezado("📋 LISTAR SUCURSALES");

            Console.WriteLine();
            Console.Write("🔍 Consultando sucursales");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales!.ListarSucursales();

            if (resultado.Exito && resultado.Sucursales != null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Se encontraron {resultado.Sucursales.Count} sucursales:");
                Console.ResetColor();
                Console.WriteLine();

                if (resultado.Sucursales.Count > 0)
                {
                    Console.WriteLine("┌─────┬──────────────────────┬──────────────┬─────────────────────────┬───────────┬──────────┐");
                    Console.WriteLine("│ Cód │       Nombre         │    Ciudad    │        Dirección        │  Teléfono │  Estado  │");
                    Console.WriteLine("├─────┼──────────────────────┼──────────────┼─────────────────────────┼───────────┼──────────┤");

                    foreach (var sucursal in resultado.Sucursales)
                    {
                        Console.WriteLine($"│ {sucursal.Codigo,-3} │ {sucursal.Nombre,-20} │ {sucursal.Ciudad,-12} │ {sucursal.Direccion,-23} │ {sucursal.Telefono,-9} │ {sucursal.Estado,-8} │");
                    }

                    Console.WriteLine("└─────┴──────────────────────┴──────────────┴─────────────────────────┴───────────┴──────────┘");
                }
                else
                {
                    Console.WriteLine("ℹ️  No hay sucursales registradas.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task BuscarSucursal()
        {
            Console.Clear();
            MostrarEncabezado("🔍 BUSCAR SUCURSAL");

            Console.Write("Ingrese el código de la sucursal: ");
            string? codigo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("❌ Código de sucursal inválido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔍 Buscando sucursal");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales!.ObtenerSucursal(codigo);

            if (resultado.Exito && resultado.Sucursal != null)
            {
                var s = resultado.Sucursal;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Sucursal encontrada:");
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine($"📋 Código:      {s.Codigo}");
                Console.WriteLine($"🏢 Nombre:      {s.Nombre}");
                Console.WriteLine($"🌆 Ciudad:      {s.Ciudad}");
                Console.WriteLine($"📍 Dirección:   {s.Direccion}");
                Console.WriteLine($"📞 Teléfono:    {s.Telefono}");
                Console.WriteLine($"📧 Email:       {s.Email}");
                Console.WriteLine($"📊 Estado:      {s.Estado}");
                Console.WriteLine($"🗺️  Coordenadas: {s.Latitud:F6}, {s.Longitud:F6}");
                Console.WriteLine($"🏦 Cuentas:     {s.ContadorCuentas}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task CrearSucursal()
        {
            Console.Clear();
            MostrarEncabezado("➕ CREAR NUEVA SUCURSAL");

            try
            {
                Console.WriteLine("Ingrese los datos de la nueva sucursal:");
                Console.WriteLine();

                Console.Write("📋 Código (3 caracteres): ");
                string codigo = Console.ReadLine() ?? "";

                Console.Write("🏢 Nombre: ");
                string nombre = Console.ReadLine() ?? "";

                Console.Write("🌆 Ciudad: ");
                string ciudad = Console.ReadLine() ?? "";

                Console.Write("📍 Dirección: ");
                string direccion = Console.ReadLine() ?? "";

                Console.Write("📞 Teléfono: ");
                string telefono = Console.ReadLine() ?? "";

                Console.Write("📧 Email: ");
                string email = Console.ReadLine() ?? "";

                Console.Write("🗺️  Latitud: ");
                if (!double.TryParse(Console.ReadLine(), out double latitud))
                {
                    Console.WriteLine("❌ Latitud inválida.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("🗺️  Longitud: ");
                if (!double.TryParse(Console.ReadLine(), out double longitud))
                {
                    Console.WriteLine("❌ Longitud inválida.");
                    Console.ReadKey();
                    return;
                }

                var nuevaSucursal = new Sucursal
                {
                    Codigo = codigo,
                    Nombre = nombre,
                    Ciudad = ciudad,
                    Direccion = direccion,
                    Telefono = telefono,
                    Email = email,
                    Latitud = latitud,
                    Longitud = longitud,
                    ContadorCuentas = 0,
                    Estado = "ACTIVO"
                };

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠️  Va a crear la sucursal '{nombre}' con código '{codigo}'");
                Console.ResetColor();
                Console.Write("¿Confirma la operación? (S/N): ");

                if (Console.ReadLine()?.ToUpper() != "S")
                {
                    Console.WriteLine("❌ Operación cancelada.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine();
                Console.Write("🔄 Creando sucursal");
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(".");
                    await Task.Delay(300);
                }
                Console.WriteLine();

                var resultado = await servicioSucursales!.CrearSucursal(nuevaSucursal);

                if (resultado.Exito)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ {resultado.Mensaje}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ {resultado.Mensaje}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task ActualizarSucursal()
        {
            Console.Clear();
            MostrarEncabezado("📝 ACTUALIZAR SUCURSAL");

            Console.Write("Ingrese el código de la sucursal a actualizar: ");
            string? codigo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("❌ Código de sucursal inválido.");
                Console.ReadKey();
                return;
            }

            // Primero buscar la sucursal
            var resultadoBusqueda = await servicioSucursales!.ObtenerSucursal(codigo);

            if (!resultadoBusqueda.Exito || resultadoBusqueda.Sucursal == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Sucursal no encontrada: {resultadoBusqueda.Mensaje}");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            var sucursal = resultadoBusqueda.Sucursal;
            Console.WriteLine();
            Console.WriteLine("Sucursal encontrada. Presione ENTER para mantener el valor actual:");
            Console.WriteLine();

            Console.Write($"🏢 Nombre [{sucursal.Nombre}]: ");
            string nombre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nombre)) sucursal.Nombre = nombre;

            Console.Write($"🌆 Ciudad [{sucursal.Ciudad}]: ");
            string ciudad = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ciudad)) sucursal.Ciudad = ciudad;

            Console.Write($"📍 Dirección [{sucursal.Direccion}]: ");
            string direccion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(direccion)) sucursal.Direccion = direccion;

            Console.Write($"📞 Teléfono [{sucursal.Telefono}]: ");
            string telefono = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(telefono)) sucursal.Telefono = telefono;

            Console.Write($"📧 Email [{sucursal.Email}]: ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) sucursal.Email = email;

            Console.Write($"📊 Estado [{sucursal.Estado}]: ");
            string estado = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(estado)) sucursal.Estado = estado;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  Va a actualizar la sucursal '{sucursal.Nombre}'");
            Console.ResetColor();
            Console.Write("¿Confirma la operación? (S/N): ");

            if (Console.ReadLine()?.ToUpper() != "S")
            {
                Console.WriteLine("❌ Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔄 Actualizando sucursal");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales.ActualizarSucursal(sucursal);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task EliminarSucursal()
        {
            Console.Clear();
            MostrarEncabezado("🗑️ ELIMINAR SUCURSAL");

            Console.Write("Ingrese el código de la sucursal a eliminar: ");
            string? codigo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("❌ Código de sucursal inválido.");
                Console.ReadKey();
                return;
            }

            // Primero buscar la sucursal para mostrar información
            var resultadoBusqueda = await servicioSucursales!.ObtenerSucursal(codigo);

            if (!resultadoBusqueda.Exito || resultadoBusqueda.Sucursal == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Sucursal no encontrada: {resultadoBusqueda.Mensaje}");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            var sucursal = resultadoBusqueda.Sucursal;

            Console.WriteLine();
            Console.WriteLine("Sucursal a eliminar:");
            Console.WriteLine($"🏢 {sucursal.Codigo} - {sucursal.Nombre} ({sucursal.Ciudad})");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠️  ATENCIÓN: Va a eliminar (desactivar) la sucursal '{sucursal.Nombre}'");
            Console.WriteLine("     Esta operación cambiará el estado a INACTIVO.");
            Console.ResetColor();
            Console.Write("¿Está seguro? (S/N): ");

            if (Console.ReadLine()?.ToUpper() != "S")
            {
                Console.WriteLine("❌ Operación cancelada.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🔄 Eliminando sucursal");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales.EliminarSucursal(codigo);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task CalcularDistanciaEntreSucursales()
        {
            Console.Clear();
            MostrarEncabezado("📏 CALCULAR DISTANCIA ENTRE SUCURSALES");

            Console.Write("Ingrese el código de la primera sucursal: ");
            string? sucursal1 = Console.ReadLine();

            Console.Write("Ingrese el código de la segunda sucursal: ");
            string? sucursal2 = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(sucursal1) || string.IsNullOrWhiteSpace(sucursal2))
            {
                Console.WriteLine("❌ Códigos de sucursal inválidos.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("📏 Calculando distancia");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales!.CalcularDistanciaEntreSucursales(sucursal1, sucursal2);

            if (resultado.Exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.WriteLine();
                Console.WriteLine($"📍 Desde: {resultado.SucursalOrigen}");
                Console.WriteLine($"📍 Hacia: {resultado.SucursalDestino}");
                Console.WriteLine($"📏 Distancia: {resultado.Distancia:F2} km");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static async Task EncontrarSucursalMasCercana()
        {
            Console.Clear();
            MostrarEncabezado("🎯 ENCONTRAR SUCURSAL MÁS CERCANA");

            Console.WriteLine("Ingrese su ubicación actual:");
            Console.Write("🗺️  Latitud: ");
            if (!double.TryParse(Console.ReadLine(), out double latitud))
            {
                Console.WriteLine("❌ Latitud inválida.");
                Console.ReadKey();
                return;
            }

            Console.Write("🗺️  Longitud: ");
            if (!double.TryParse(Console.ReadLine(), out double longitud))
            {
                Console.WriteLine("❌ Longitud inválida.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("🎯 Buscando sucursal más cercana");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultado = await servicioSucursales!.EncontrarSucursalMasCercana(latitud, longitud);

            if (resultado.Exito && resultado.Sucursal != null)
            {
                var s = resultado.Sucursal;

                // Calcular distancia para mostrar
                var distanciaResult = await servicioSucursales.CalcularDistanciaASucursal(s.Codigo, latitud, longitud);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ {resultado.Mensaje}");
                Console.WriteLine();
                Console.WriteLine("🏢 Sucursal más cercana:");
                Console.WriteLine($"   📋 Código:    {s.Codigo}");
                Console.WriteLine($"   🏢 Nombre:    {s.Nombre}");
                Console.WriteLine($"   🌆 Ciudad:    {s.Ciudad}");
                Console.WriteLine($"   📍 Dirección: {s.Direccion}");
                Console.WriteLine($"   📞 Teléfono:  {s.Telefono}");

                if (distanciaResult.Exito)
                {
                    Console.WriteLine($"   📏 Distancia: {distanciaResult.Distancia:F2} km");
                }

                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ {resultado.Mensaje}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // MÉTODO: Obtener direcciones paso a paso
        static async Task ObtenerDireccionesASucursal()
        {
            Console.Clear();
            MostrarEncabezado("🗺️ DIRECCIONES PASO A PASO");

            // Solicitar ubicación actual
            Console.WriteLine("📍 Ingrese su ubicación actual:");
            Console.Write("🗺️  Latitud: ");
            string latInput = Console.ReadLine()?.Replace(",", ".") ?? "";
            if (!double.TryParse(latInput, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double origenLat))
            {
                Console.WriteLine("❌ Latitud inválida.");
                Console.ReadKey();
                return;
            }

            Console.Write("🗺️  Longitud: ");
            string lngInput = Console.ReadLine()?.Replace(",", ".") ?? "";
            if (!double.TryParse(lngInput, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double origenLng))
            {
                Console.WriteLine("❌ Longitud inválida.");
                Console.ReadKey();
                return;
            }

            // Solicitar sucursal destino
            Console.WriteLine();
            Console.Write("🏢 Ingrese el código de la sucursal destino: ");
            string? codigoSucursal = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigoSucursal))
            {
                Console.WriteLine("❌ Código de sucursal inválido.");
                Console.ReadKey();
                return;
            }

            // Obtener datos de la sucursal
            Console.WriteLine();
            Console.Write("🔍 Buscando sucursal");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(300);
            }
            Console.WriteLine();

            var resultadoSucursal = await servicioSucursales!.ObtenerSucursal(codigoSucursal);

            if (!resultadoSucursal.Exito || resultadoSucursal.Sucursal == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Sucursal no encontrada: {resultadoSucursal.Mensaje}");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            var sucursal = resultadoSucursal.Sucursal;

            // Solicitar modo de transporte
            Console.WriteLine();
            Console.WriteLine("🚗 Seleccione modo de transporte:");
            Console.WriteLine("  1️⃣  Conduciendo (driving)");
            Console.WriteLine("  2️⃣  Caminando (walking)");
            Console.WriteLine("  3️⃣  Transporte público (transit)");
            Console.WriteLine("  4️⃣  Bicicleta (bicycling)");
            Console.Write("Seleccione una opción (1-4): ");

            string modo = "driving";
            if (int.TryParse(Console.ReadLine(), out int modoOpcion))
            {
                modo = modoOpcion switch
                {
                    1 => "driving",
                    2 => "walking",
                    3 => "transit",
                    4 => "bicycling",
                    _ => "driving"
                };
            }

            string modoTexto = modo switch
            {
                "driving" => "🚗 Conduciendo",
                "walking" => "🚶‍♂️ Caminando",
                "transit" => "🚌 Transporte público",
                "bicycling" => "🚴‍♂️ En bicicleta",
                _ => "🚗 Conduciendo"
            };

            // Obtener direcciones
            Console.WriteLine();
            Console.Write("🗺️ Calculando ruta");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                await Task.Delay(500);
            }
            Console.WriteLine();

            var ruta = await serviceDirecciones!.ObtenerDirecciones(
                origenLat, origenLng,
                sucursal.Latitud, sucursal.Longitud,
                modo
            );

            // Mostrar información de la ruta
            Console.Clear();
            MostrarEncabezado($"🗺️ RUTA A {sucursal.Nombre}");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("📋 INFORMACIÓN DE LA RUTA");
            Console.WriteLine("═══════════════════════════");
            Console.ResetColor();

            Console.WriteLine($"🏢 Destino:     {sucursal.Nombre} - {sucursal.Ciudad}");
            Console.WriteLine($"📍 Dirección:   {sucursal.Direccion}");
            Console.WriteLine($"📏 Distancia:   {ruta.DistanciaTotal}");
            Console.WriteLine($"⏱️ Tiempo:      {ruta.TiempoTotal}");
            Console.WriteLine($"🚗 Transporte:  {modoTexto}");

            if (!GoogleDirectionsService.IsApiKeyConfigured())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ Usando cálculos básicos (sin Google Directions API)");
                Console.WriteLine("   Para direcciones detalladas, configure Google API Key");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("🧭 DIRECCIONES PASO A PASO");
            Console.WriteLine("══════════════════════════");
            Console.ResetColor();

            foreach (var paso in ruta.Pasos)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{paso.Maniobra} {paso.Numero}. ");
                Console.ResetColor();
                Console.WriteLine($"{paso.Instruccion}");

                if (!string.IsNullOrEmpty(paso.Distancia) && !string.IsNullOrEmpty(paso.Tiempo))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"     📏 {paso.Distancia} • ⏱️ {paso.Tiempo}");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ ¡Has llegado a {sucursal.Nombre}!");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("🔗 Enlaces útiles:");
            Console.WriteLine($"📱 Google Maps: https://maps.google.com/maps?daddr={sucursal.Latitud},{sucursal.Longitud}");
            Console.WriteLine($"📞 Teléfono: {sucursal.Telefono}");

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // MÉTODOM: mostrar direcciones con colores mejorados
        static void MostrarPasoConIcono(PasoRuta paso)
        {
            // Color para el número de paso
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{paso.Maniobra} {paso.Numero}. ");
            Console.ResetColor();

            // Texto de la instrucción
            Console.WriteLine($"{paso.Instruccion}");

            // Información adicional en gris
            if (!string.IsNullOrEmpty(paso.Distancia))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     📏 {paso.Distancia} • ⏱️ {paso.Tiempo}");
                Console.ResetColor();
            }
        }
    }
}
