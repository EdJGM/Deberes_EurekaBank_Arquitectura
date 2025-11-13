# 📱 Eurekabank Mobile - Cliente MAUI Unificado

Cliente móvil multiplataforma desarrollado en .NET MAUI que se conecta de forma unificada a los 4 servidores Eurekabank (SOAP .NET, SOAP Java, REST .NET, REST Java).

## 🎯 Características Principales

- ✅ **Arquitectura Unificada**: Un solo cliente para 4 servidores diferentes
- ✅ **Selección Dinámica**: Elige el servidor al iniciar sesión
- ✅ **Patrón Strategy**: Cambio transparente entre implementaciones
- ✅ **UI/UX Moderna**: Interfaz limpia y profesional
- ✅ **MVVM Pattern**: Separación clara de responsabilidades
- ✅ **Multiplataforma**: Android, iOS, Windows, macOS

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────┐
│                   MAUI App                      │
│                                                 │
│  ┌──────────────┐       ┌──────────────┐       │
│  │  LoginPage   │───────│   MainPage   │       │
│  └──────────────┘       └──────────────┘       │
│         │                       │               │
│         ▼                       ▼               │
│  ┌──────────────┐       ┌──────────────┐       │
│  │LoginViewModel│       │MainViewModel │       │
│  └──────────────┘       └──────────────┘       │
│         │                       │               │
│         ▼                       ▼               │
│  ┌─────────────────────────────────────┐       │
│  │   IEurekabankService (Interface)    │       │
│  └─────────────────────────────────────┘       │
│         │                                       │
│    ┌────┴────┬────────┬────────┐              │
│    ▼         ▼        ▼        ▼              │
│  SOAP.NET  SOAP.Java REST.NET REST.Java       │
└─────────────────────────────────────────────────┘
           │         │        │        │
           ▼         ▼        ▼        ▼
     ┌─────────────────────────────────────┐
     │     Servidores Eurekabank           │
     │  (4 implementaciones independientes)│
     └─────────────────────────────────────┘
```

## 📂 Estructura del Proyecto

```
Eurekabank_Maui/
├── Models/
│   ├── Movimiento.cs                  # Modelo de movimiento bancario
│   └── ServidorConfig.cs              # Configuración de servidores
│
├── Services/
│   ├── IEurekabankService.cs          # Interfaz común
│   ├── SoapDotNetService.cs           # Implementación SOAP .NET
│   ├── SoapJavaService.cs             # Implementación SOAP Java
│   ├── RestDotNetService.cs           # Implementación REST .NET
│   ├── RestJavaService.cs             # Implementación REST Java
│   └── EurekabankServiceFactory.cs    # Factory para crear servicios
│
├── ViewModels/
│   ├── BaseViewModel.cs               # ViewModel base
│   ├── LoginViewModel.cs              # ViewModel de login
│   └── MainViewModel.cs               # ViewModel principal
│
├── Views/
│   ├── LoginPage.xaml                 # Vista de login
│   ├── LoginPage.xaml.cs
│   ├── MainPage.xaml                  # Vista principal
│   └── MainPage.xaml.cs
│
├── Helpers/
│   └── SoapHelper.cs                  # Helper para llamadas SOAP
│
├── Converters/
│   └── ValueConverters.cs             # Converters para bindings
│
├── App.xaml                           # Recursos globales
├── App.xaml.cs
├── MauiProgram.cs                     # Configuración de la app
└── Eurekabank_Maui.csproj            # Archivo de proyecto
```

## 🔧 Requisitos Previos

### Software Necesario

1. **.NET 8 SDK**
   ```bash
   dotnet --version  # Debe ser 8.0.x
   ```

2. **Visual Studio 2022** (Windows) o **Visual Studio for Mac**
   - Workload: .NET Multi-platform App UI development

3. **Workloads MAUI**
   ```bash
   dotnet workload install maui
   dotnet workload install android
   dotnet workload install ios
   dotnet workload install maccatalyst
   dotnet workload install maui-windows
   ```

### Servidores Eurekabank

Asegúrate de tener ejecutándose al menos uno de los 4 servidores:

| Servidor | URL por Defecto |
|----------|----------------|
| SOAP .NET | `http://localhost/Eurekabank_Soap_Dotnet/ec.edu.monster.ws/EurekabankWS.svc` |
| SOAP Java | `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS` |
| REST .NET | `http://localhost:5111/api/eureka` |
| REST Java | `http://localhost:8080/Eurobank_Restfull_Java/api/eureka` |

## 🚀 Instalación y Ejecución

### 1. Clonar/Copiar el Proyecto

Copia todos los archivos a tu directorio de trabajo.

### 2. Restaurar Dependencias

```bash
cd Eurekabank_Maui
dotnet restore
```

### 3. Compilar el Proyecto

```bash
# Para Android
dotnet build -t:Run -f net8.0-android

# Para Windows
dotnet build -t:Run -f net8.0-windows10.0.19041.0

# Para iOS (requiere Mac)
dotnet build -t:Run -f net8.0-ios

# Para macOS
dotnet build -t:Run -f net8.0-maccatalyst
```

### 4. Ejecutar desde Visual Studio

1. Abrir `Eurekabank_Maui.sln` en Visual Studio
2. Seleccionar la plataforma de destino (Android Emulator, Windows Machine, etc.)
3. Presionar F5 o clic en "Start"

## 📱 Uso de la Aplicación

### Pantalla de Login

1. **Seleccionar Servidor**: Elige uno de los 4 servidores disponibles
2. **Verificar Conexión** (opcional): Usa el botón para verificar que el servidor esté disponible
3. **Ingresar Credenciales**:
   - Usuario: `internet`
   - Contraseña: `internet`
4. **Iniciar Sesión**

### Pantalla Principal (Operaciones)

#### Consultar Movimientos
- Ingresa el número de cuenta (ej: `00100001`)
- Presiona "Consultar"
- Ver listado de movimientos con colores:
  - 🟢 Verde = INGRESO
  - 🔴 Rojo = SALIDA

#### Realizar Depósito
- Ingresa cuenta e importe
- Presiona "Depósito"
- Confirma la operación

#### Realizar Retiro
- Ingresa cuenta e importe
- Presiona "Retiro"
- Confirma la operación

#### Transferencia
- Ingresa cuenta origen, cuenta destino e importe
- Presiona "Transferir"
- Confirma la operación

## 🔐 Usuarios de Prueba

```
Usuario: internet
Contraseña: internet

Usuario: MONSTER
Contraseña: MONSTER9
```

## 💳 Cuentas de Prueba

```
- 00100001 (Soles)
- 00100002 (Dólares)
- 00200001 (Soles)
- 00200002 (Soles)
- 00200003 (Dólares)
```

## 🎨 Características de UI/UX

- **Material Design**: Interfaz moderna y limpia
- **Colores Diferenciados**: Cada servidor tiene su color identificador
- **Feedback Visual**: Indicadores de carga y mensajes de estado
- **Responsive**: Se adapta a diferentes tamaños de pantalla
- **Validaciones**: Control de campos requeridos
- **Confirmaciones**: Diálogos de confirmación para operaciones críticas

## 🏛️ Patrón de Diseño: Strategy

El proyecto usa el patrón Strategy para permitir cambiar dinámicamente entre diferentes implementaciones de servicio:

```csharp
// Interfaz común
public interface IEurekabankService
{
    Task<bool> LoginAsync(string username, string password);
    Task<List<Movimiento>> ObtenerMovimientosAsync(string cuenta);
    // ... más métodos
}

// Factory para crear el servicio apropiado
var service = EurekabankServiceFactory.Create(tipoServidor);

// El cliente usa la interfaz, no la implementación concreta
await service.LoginAsync(username, password);
```

## 🔍 Características Técnicas

### SOAP Services

Los servicios SOAP usan `SoapHelper` para:
- Construir sobres SOAP manualmente
- Parsear respuestas XML
- Manejar namespaces
- Extraer datos de elementos XML

### REST Services

Los servicios REST usan:
- `HttpClient` para peticiones HTTP
- `System.Text.Json` para serialización/deserialización
- Métodos async/await
- Manejo de diferentes formatos de respuesta

### MVVM Pattern

- **Models**: Entidades de datos (Movimiento, ServidorConfig)
- **Views**: XAML files (LoginPage, MainPage)
- **ViewModels**: Lógica de presentación (LoginViewModel, MainViewModel)
- **Data Binding**: Sincronización automática View ↔ ViewModel

## ⚙️ Configuración Avanzada

### Cambiar URLs de Servidores

Edita `Models/ServidorConfig.cs`:

```csharp
new ServidorConfig
{
    Tipo = TipoServidor.RestDotNet,
    Nombre = "REST .NET",
    Url = "http://TU_IP:5111/api/eureka",  // Cambiar aquí
    // ...
}
```

### Agregar Nuevo Servidor

1. Crear nueva clase que implemente `IEurekabankService`
2. Agregar nuevo `TipoServidor` en el enum
3. Agregar configuración en `ServidorConfig.ObtenerServidores()`
4. Actualizar el Factory para incluir el nuevo tipo

## 🐛 Troubleshooting

### Error: "No se puede conectar al servidor"

**Causa**: El servidor no está ejecutándose o la URL es incorrecta

**Solución**:
1. Verifica que el servidor esté ejecutándose
2. En Android emulator, usa `10.0.2.2` en lugar de `localhost`
3. Usa la IP real de tu máquina en lugar de `localhost`

### Error de Certificado SSL

**Causa**: Certificados SSL auto-firmados en desarrollo

**Solución**: El código ya incluye bypass de validación SSL para desarrollo:
```csharp
ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
```

### Android: Network Security Config

Si usas Android 9+, necesitas permitir cleartext traffic:

Crear `Resources/xml/network_security_config.xml`:
```xml
<?xml version="1.0" encoding="utf-8"?>
<network-security-config>
    <base-config cleartextTrafficPermitted="true">
        <trust-anchors>
            <certificates src="system" />
        </trust-anchors>
    </base-config>
</network-security-config>
```

Y en `AndroidManifest.xml`:
```xml
<application android:networkSecurityConfig="@xml/network_security_config">
```

## 📊 Testing

### Flujo de Prueba Completo

1. **Login con SOAP .NET**
   - Usuario: `internet` / Contraseña: `internet`
   - Verificar login exitoso

2. **Consultar Movimientos**
   - Cuenta: `00100001`
   - Verificar listado de movimientos

3. **Realizar Depósito**
   - Cuenta: `00100001`
   - Importe: `500`
   - Confirmar operación

4. **Cerrar Sesión y Cambiar de Servidor**
   - Cerrar sesión
   - Seleccionar REST Java
   - Login nuevamente

5. **Verificar Consistencia**
   - Consultar misma cuenta
   - Verificar que el depósito aparezca en el historial

## 🔒 Seguridad

- Contraseñas hasheadas con SHA1 en el servidor
- Comunicación HTTPS recomendada para producción
- No se almacenan credenciales en el dispositivo
- Sesión se cierra al salir de la aplicación

## 📈 Mejoras Futuras

- [ ] Soporte para biometría (huella/Face ID)
- [ ] Cache local de movimientos
- [ ] Modo offline con sincronización
- [ ] Notificaciones push
- [ ] Estadísticas y gráficos
- [ ] Export a PDF/Excel
- [ ] Múltiples idiomas
- [ ] Tema oscuro

## 📝 Licencia

Este proyecto es parte del sistema Eurekabank y está destinado para fines educativos.

## 👥 Contribuciones

Para contribuir:
1. Fork el repositorio
2. Crea una rama para tu feature
3. Commit tus cambios
4. Push a la rama
5. Crea un Pull Request

## 📞 Soporte

Para preguntas o problemas:
- Revisa la sección de Troubleshooting
- Verifica que los servidores estén ejecutándose
- Verifica las URLs de conexión

---

**Desarrollado con ❤️ usando .NET MAUI**
