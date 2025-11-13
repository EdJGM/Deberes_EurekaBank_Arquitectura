# 🔄 Integración con Proyecto Existente

Esta guía te ayudará a integrar el cliente móvil unificado en tu proyecto existente "Eurekabank_Soap_Java_Movil".

## 📋 Opciones de Integración

Tienes dos opciones para usar este código:

### Opción 1: Reemplazar Proyecto Completo (Recomendado)

Esta es la opción más limpia si estás empezando.

**Pasos:**

1. **Respaldar tu proyecto actual**
   ```bash
   # Renombrar tu proyecto actual
   ren "Eurekabank_Soap_Java_Movil" "Eurekabank_Soap_Java_Movil_BACKUP"
   ```

2. **Copiar el nuevo proyecto**
   ```bash
   # Copiar todos los archivos del nuevo proyecto
   xcopy /E /I eurekabank_maui Eurekabank_Soap_Java_Movil
   ```

3. **Renombrar el proyecto**
   - Renombrar `Eurekabank_Maui.csproj` a `Eurekabank_Soap_Java_Movil.csproj`
   - Actualizar namespace en todos los archivos:
     - Buscar: `namespace Eurekabank_Maui`
     - Reemplazar: `namespace Eurekabank_Soap_Java_Movil`

4. **Abrir en Visual Studio**
   - Abrir Visual Studio 2022
   - File → Open → Project/Solution
   - Seleccionar: `Eurekabank_Soap_Java_Movil/Eurekabank_Soap_Java_Movil.csproj`

### Opción 2: Integrar en Proyecto Existente

Si ya tienes código en tu proyecto que quieres conservar.

**Estructura del Proyecto Actual:**
```
Eurekabank_Soap_Java_Movil/
├── Properties/
├── Platforms/
├── Resources/
├── App.xaml
├── AppShell.xaml
├── MainPage.xaml
├── MauiProgram.cs
└── Eurekabank_Soap_Java_Movil.csproj
```

**Pasos de Integración:**

#### 1. Agregar Carpetas del Nuevo Código

Crear estas carpetas en tu proyecto:
```
Eurekabank_Soap_Java_Movil/
├── Models/           ← NUEVO
├── Services/         ← NUEVO
├── ViewModels/       ← NUEVO
├── Views/            ← NUEVO
├── Helpers/          ← NUEVO
└── Converters/       ← NUEVO
```

#### 2. Copiar Archivos por Categoría

**A. Models (Copiar todos)**
```
Models/
├── Movimiento.cs
└── ServidorConfig.cs
```

**B. Services (Copiar todos)**
```
Services/
├── IEurekabankService.cs
├── SoapDotNetService.cs
├── SoapJavaService.cs
├── RestDotNetService.cs
├── RestJavaService.cs
└── EurekabankServiceFactory.cs
```

**C. ViewModels (Copiar todos)**
```
ViewModels/
├── BaseViewModel.cs
├── LoginViewModel.cs
└── MainViewModel.cs
```

**D. Views (Copiar todos)**
```
Views/
├── LoginPage.xaml
├── LoginPage.xaml.cs
├── MainPage.xaml (puede reemplazar el existente)
└── MainPage.xaml.cs (puede reemplazar el existente)
```

**E. Helpers**
```
Helpers/
└── SoapHelper.cs
```

**F. Converters**
```
Converters/
└── ValueConverters.cs
```

#### 3. Actualizar Archivos Existentes

**A. App.xaml** (Reemplazar contenido)
```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:converters="clr-namespace:Eurekabank_Soap_Java_Movil.Converters"
             x:Class="Eurekabank_Soap_Java_Movil.App">
    <Application.Resources>
        <ResourceDictionary>
            <converters:EqualConverter x:Key="EqualConverter" />
            <converters:GreaterThanZeroConverter x:Key="GreaterThanZeroConverter" />
            <converters:InverseBoolConverter x:Key="InverseBoolConverter" />
            
            <!-- Agregar estilos del nuevo App.xaml aquí -->
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

**B. App.xaml.cs** (Actualizar)
```csharp
using Eurekabank_Soap_Java_Movil.Views;

namespace Eurekabank_Soap_Java_Movil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.FromArgb("#512BD4"),
                BarTextColor = Colors.White
            };
        }
    }
}
```

**C. MauiProgram.cs** (Actualizar)
```csharp
using Microsoft.Extensions.Logging;

namespace Eurekabank_Soap_Java_Movil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Configurar HttpClient
            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var httpClient = new HttpClient(new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };
                return httpClient;
            });

            return builder.Build();
        }
    }
}
```

#### 4. Actualizar Namespaces

**Buscar y reemplazar en todos los archivos nuevos:**
- **Buscar:** `namespace Eurekabank_Maui`
- **Reemplazar:** `namespace Eurekabank_Soap_Java_Movil`

**También en los using statements:**
- **Buscar:** `using Eurekabank_Maui`
- **Reemplazar:** `using Eurekabank_Soap_Java_Movil`

#### 5. Actualizar .csproj

Asegúrate de que tu archivo `.csproj` tenga esta estructura:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFrameworks>net8.0-android;net8.0-ios;net8.0-maccatalyst</TargetFrameworks>
        <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net8.0-windows10.0.19041.0</TargetFrameworks>
        <OutputType>Exe</OutputType>
        <RootNamespace>Eurekabank_Soap_Java_Movil</RootNamespace>
        <UseMaui>true</UseMaui>
        <SingleProject>true</SingleProject>
        <ImplicitUsings>enable</ImplicitUsings>
        <ApplicationTitle>Eurekabank Móvil</ApplicationTitle>
        <ApplicationId>com.eurekabank.mobile</ApplicationId>
        <!-- ... resto de configuración ... -->
    </PropertyGroup>
</Project>
```

#### 6. Eliminar Archivos No Necesarios

Si tienes estos archivos del proyecto anterior, puedes eliminarlos:
- `AppShell.xaml` (no se usa en la nueva arquitectura)
- Cualquier `MainPage.xaml` anterior si no tiene código que quieras conservar

## ✅ Verificación Post-Integración

### 1. Compilar el Proyecto

```bash
cd Eurekabank_Soap_Java_Movil
dotnet clean
dotnet restore
dotnet build
```

**Verificar que no hay errores de compilación.**

### 2. Verificar Estructura

Tu proyecto final debe verse así:

```
Eurekabank_Soap_Java_Movil/
├── Converters/
│   └── ValueConverters.cs
├── Helpers/
│   └── SoapHelper.cs
├── Models/
│   ├── Movimiento.cs
│   └── ServidorConfig.cs
├── Services/
│   ├── IEurekabankService.cs
│   ├── SoapDotNetService.cs
│   ├── SoapJavaService.cs
│   ├── RestDotNetService.cs
│   ├── RestJavaService.cs
│   └── EurekabankServiceFactory.cs
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── LoginViewModel.cs
│   └── MainViewModel.cs
├── Views/
│   ├── LoginPage.xaml
│   ├── LoginPage.xaml.cs
│   ├── MainPage.xaml
│   └── MainPage.xaml.cs
├── Platforms/
├── Properties/
├── Resources/
├── App.xaml
├── App.xaml.cs
├── MauiProgram.cs
├── GlobalUsings.cs
└── Eurekabank_Soap_Java_Movil.csproj
```

### 3. Ejecutar la Aplicación

```bash
# Para Windows
dotnet build -t:Run -f net8.0-windows10.0.19041.0

# O desde Visual Studio:
# 1. Abrir Eurekabank_Soap_Java_Movil.csproj
# 2. Seleccionar "Windows Machine"
# 3. Presionar F5
```

### 4. Probar Funcionalidad

- [ ] Login con diferentes servidores
- [ ] Consulta de movimientos
- [ ] Depósito
- [ ] Retiro
- [ ] Transferencia
- [ ] Cerrar sesión

## 🐛 Solución de Problemas

### Error: "Type not found"

**Causa:** Namespaces incorrectos

**Solución:**
1. Verificar que todos los archivos usen `namespace Eurekabank_Soap_Java_Movil`
2. Limpiar y reconstruir: `dotnet clean && dotnet build`

### Error: "Could not load file or assembly"

**Causa:** Dependencias no restauradas

**Solución:**
```bash
dotnet restore --force
dotnet build
```

### Error en compilación de XAML

**Causa:** Converters no registrados

**Solución:**
Verificar que `App.xaml` incluya:
```xml
xmlns:converters="clr-namespace:Eurekabank_Soap_Java_Movil.Converters"
<converters:EqualConverter x:Key="EqualConverter" />
```

## 📚 Archivos de Referencia

Los siguientes archivos contienen ejemplos completos:
- `README.md` - Documentación completa
- `QUICK_START.md` - Guía de inicio rápido
- `setup.ps1` - Script de configuración automática

## ✨ Características Nuevas vs Proyecto Anterior

| Característica | Proyecto Anterior | Proyecto Nuevo |
|----------------|-------------------|----------------|
| Servidores Soportados | 1 (SOAP Java) | 4 (SOAP .NET/Java, REST .NET/Java) |
| Arquitectura | Acoplada | Desacoplada (Strategy Pattern) |
| UI/UX | Básica | Moderna con Material Design |
| MVVM | Parcial | Completo |
| Manejo de Errores | Básico | Completo con validaciones |
| Testing | Manual | Preparado para Unit Tests |

## 🎓 Próximos Pasos

Después de la integración:

1. ✅ Verificar que todo funciona
2. 📝 Revisar y entender el código nuevo
3. 🎨 Personalizar colores/estilos si es necesario
4. 🧪 Probar con los 4 servidores
5. 📱 Desplegar en dispositivo real
6. 📚 Leer documentación completa en README.md

---

**¿Necesitas ayuda?**
- Revisa QUICK_START.md para comandos rápidos
- Revisa README.md para documentación completa
- Consulta la sección de Troubleshooting en README.md
