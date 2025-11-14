# 🏦 Eurekabank - Cliente Consola Unificado

Cliente de consola multiplataforma en .NET 6 para conectarse a todos los servidores del sistema bancario Eurekabank.

## 📋 Características

- **Conexión a 4 servidores diferentes:**
  - ✅ SOAP .NET (Puerto 57199)
  - ✅ SOAP Java (Puerto 8080)
  - ✅ REST .NET (Puerto 5111)
  - ✅ REST Java (Puerto 8080)

- **Funcionalidades bancarias:**
  - 🔐 Autenticación de usuarios
  - 📊 Consulta de movimientos
  - 💰 Registro de depósitos
  - 💸 Registro de retiros
  - 🔄 Registro de transferencias

## 🚀 Requisitos Previos

- .NET 6.0 SDK o superior
- Visual Studio 2022 o Visual Studio Code
- Al menos uno de los servidores Eurekabank ejecutándose

## 📦 Instalación

### Opción 1: Visual Studio

1. Abrir el archivo `Eurekabank_Cliente_Consola_Unificado.csproj` en Visual Studio
2. Restaurar paquetes NuGet (automático)
3. Compilar el proyecto (F6)
4. Ejecutar (F5)

### Opción 2: Línea de Comandos

```bash
# Clonar o descargar el proyecto
cd Eurekabank_Cliente_Consola_Unificado

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

## 🎮 Uso del Cliente

### 1. Pantalla de Bienvenida

Al iniciar, verás el logo de Eurekabank y podrás seleccionar el servidor:

```
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║        🏦  EUREKABANK - CLIENTE CONSOLA UNIFICADO  🏦      ║
║                                                            ║
║          Sistema de Gestión Bancaria Multiplataforma      ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝

📡 SELECCIÓN DE SERVIDOR
========================

Seleccione el servidor al que desea conectarse:

  1️⃣  SOAP .NET     (Puerto 57199)
  2️⃣  SOAP Java     (Puerto 8080)
  3️⃣  REST .NET     (Puerto 5111)
  4️⃣  REST Java     (Puerto 8080)

Ingrese su opción (1-4): _
```

### 2. Autenticación

Después de seleccionar el servidor, ingresa tus credenciales:

```
🔐 INICIO DE SESIÓN - REST_DOTNET
════════════════════════════════════════════════════════════

👤 Usuario: cromero
🔑 Contraseña: ******

🔍 Autenticando...
✅ Login exitoso
   Bienvenido, cromero!
```

### 3. Menú Principal

Una vez autenticado, accede a las operaciones bancarias:

```
💼 MENÚ PRINCIPAL - Usuario: cromero
════════════════════════════════════════════════════════════

  1️⃣  Consultar Movimientos de Cuenta
  2️⃣  Realizar Depósito
  3️⃣  Realizar Retiro
  4️⃣  Realizar Transferencia
  5️⃣  Cerrar Sesión

Seleccione una opción: _
```

## 🗂️ Estructura del Proyecto

```
Eurekabank_Cliente_Consola_Unificado/
│
├── Eurekabank_Cliente_Consola_Unificado.csproj  # Archivo de proyecto
├── Program.cs                                    # Programa principal y menús
├── Models.cs                                     # Modelos de datos
├── IEurekabankService.cs                        # Interfaz común
├── RestServices.cs                              # Clientes REST (.NET y Java)
├── SoapServices.cs                              # Clientes SOAP (.NET y Java)
└── README.md                                     # Este archivo
```

## 🔧 Configuración de URLs

Por defecto, el cliente usa estas URLs:

| Servidor | URL |
|----------|-----|
| SOAP .NET | `http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc` |
| SOAP Java | `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS` |
| REST .NET | `http://localhost:5111/api` |
| REST Java | `http://localhost:8080/Eurobank_Restfull_Java/api` |

Para cambiar las URLs, modifica los constructores en:
- `RestServices.cs` → `RestDotNetService` y `RestJavaService`
- `SoapServices.cs` → `SoapDotNetService` y `SoapJavaService`

## 👥 Usuarios de Prueba

Según la base de datos del proyecto, puedes usar:

| Usuario | Contraseña | Estado |
|---------|------------|--------|
| cromero | chicho | ACTIVO |
| lcastro | flaca | ACTIVO |
| aramos | china | ACTIVO |
| cvalencia | angel | ACTIVO |
| rcruz | cerebro | ACTIVO |
| lpachas | gato | ACTIVO |
| htello | machupichu | ACTIVO |
| pcarrasco | tinajones | ACTIVO |
| MONSTER | MONSTER9 | ACTIVO |

## 📊 Cuentas de Prueba

El sistema tiene cuentas pre-cargadas en las sucursales "Sipan" (Chiclayo) y "Chan Chan" (Trujillo). Consulta los scripts SQL en la carpeta `03 BDD` de cada servidor.

## 🐛 Solución de Problemas

### Error: "No se pudo conectar al servidor"

**Causa:** El servidor seleccionado no está ejecutándose.

**Solución:**
1. Verifica que el servidor esté iniciado
2. Comprueba que el puerto esté correcto
3. Revisa el firewall

### Error: "Credenciales inválidas"

**Causa:** Usuario o contraseña incorrectos.

**Solución:**
1. Verifica que el usuario exista en la base de datos
2. Asegúrate de que el estado sea 'ACTIVO'
3. Comprueba que la contraseña sea correcta

### Error: "Timeout"

**Causa:** El servidor demora en responder.

**Solución:**
1. Aumenta el timeout en el código (línea `_httpClient.Timeout`)
2. Verifica la conexión de red
3. Revisa el rendimiento del servidor

## 🧪 Pruebas

### Prueba Básica

1. Selecciona servidor REST .NET
2. Login con `cromero` / `chicho`
3. Consulta movimientos de cuenta `00100001` (si existe)
4. Realiza un depósito de S/. 100.00
5. Consulta nuevamente los movimientos

### Prueba de Transferencia

1. Identifica dos cuentas válidas en la BD
2. Realiza una transferencia entre ellas
3. Verifica los movimientos en ambas cuentas

## 📝 Notas Técnicas

### Manejo de Protocolos

- **REST:** Usa `HttpClient` con JSON (Newtonsoft.Json)
- **SOAP:** Construye mensajes SOAP manualmente con XML

### Patrón de Diseño

- **Strategy Pattern:** Implementa `IEurekabankService` para cada servidor
- **Dependency Injection:** Inyecta el servicio según la selección del usuario
- **Async/Await:** Todas las operaciones son asíncronas

### Seguridad

- ⚠️ Este es un cliente de demostración
- Las contraseñas se ocultan con asteriscos en consola
- En producción, usa HTTPS y tokens JWT

## 🤝 Contribuciones

Este es un proyecto educativo. Sugerencias de mejora:

1. Agregar cifrado de comunicaciones
2. Implementar caché de sesión
3. Agregar logs de auditoría
4. Mejorar el parseo XML de respuestas SOAP
5. Agregar pruebas unitarias

## 📄 Licencia

Proyecto educativo - Eurekabank Sistema Bancario

## 👨‍💻 Autor

Desarrollado para el proyecto Eurekabank - Arquitectura de Software

---

**Versión:** 1.0.0  
**Fecha:** 2025  
**Framework:** .NET 6.0
