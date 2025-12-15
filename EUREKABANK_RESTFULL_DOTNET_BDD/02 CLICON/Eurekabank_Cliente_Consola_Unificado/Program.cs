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
                Console.WriteLine("  5️⃣  Cerrar Sesión");
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
    }
}
