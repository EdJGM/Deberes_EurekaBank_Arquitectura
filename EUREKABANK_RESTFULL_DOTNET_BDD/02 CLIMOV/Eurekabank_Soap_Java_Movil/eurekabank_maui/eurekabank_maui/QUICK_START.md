# 🚀 Guía de Inicio Rápido - Eurekabank Mobile

## ⚡ Pasos Rápidos (5 minutos)

### 1. Verificar Prerrequisitos

```bash
# Verificar .NET SDK
dotnet --version
# Debe mostrar: 8.0.x

# Verificar workloads MAUI
dotnet workload list
# Debe incluir: maui, android, ios, maccatalyst, maui-windows
```

### 2. Iniciar un Servidor Backend

Inicia al menos uno de los servidores Eurekabank:

**Opción A: REST .NET (Recomendado para inicio rápido)**
```bash
cd EUREKABANK_RESTFULL_DOTNET_BDD/01 SERVIDOR/Eurekabank_Restfull_Dotnet
dotnet run
```
Servidor disponible en: `http://localhost:5111`

**Opción B: SOAP Java**
- Inicia GlassFish Server
- Deploy el WAR de `Eurobank_Soap_Java`
- Disponible en: `http://localhost:8080/Eurobank_Soap_Java`

### 3. Copiar el Proyecto a tu Workspace

```bash
# Copiar todos los archivos del cliente móvil
cp -r eurekabank_maui "C:/Users/TU_USUARIO/source/repos/Eurekabank_Maui"
```

### 4. Abrir y Ejecutar

**Opción A: Visual Studio 2022**
```
1. Abrir Visual Studio 2022
2. File → Open → Project/Solution
3. Seleccionar: Eurekabank_Maui/Eurekabank_Maui.csproj
4. Seleccionar plataforma: "Windows Machine" o "Android Emulator"
5. Presionar F5 (Run)
```

**Opción B: Línea de Comandos**
```bash
cd Eurekabank_Maui

# Para Windows
dotnet build -t:Run -f net8.0-windows10.0.19041.0

# Para Android
dotnet build -t:Run -f net8.0-android
```

### 5. Usar la Aplicación

1. **Seleccionar Servidor**
   - Elige "REST .NET" (si iniciaste ese servidor)
   - Presiona "Verificar Conexión" para confirmar

2. **Login**
   - Usuario: `internet`
   - Contraseña: `internet`
   - Presiona "Iniciar Sesión"

3. **Consultar Movimientos**
   - Ingresa cuenta: `00100001`
   - Presiona "Consultar"
   - ¡Listo! Verás el historial de movimientos

## 🎯 Estructura del Código (Para Desarrolladores)

### Agregar Nueva Funcionalidad

**1. Agregar método en la interfaz (`IEurekabankService.cs`):**
```csharp
Task<double> ObtenerSaldoAsync(string cuenta);
```

**2. Implementar en cada servicio:**
```csharp
// RestDotNetService.cs
public async Task<double> ObtenerSaldoAsync(string cuenta)
{
    var response = await _httpClient.GetAsync($"/saldo/{cuenta}");
    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadFromJsonAsync<SaldoResponse>();
        return result?.Saldo ?? 0;
    }
    return 0;
}
```

**3. Agregar en ViewModel:**
```csharp
// MainViewModel.cs
private double _saldo;
public double Saldo
{
    get => _saldo;
    set => SetProperty(ref _saldo, value);
}

public ICommand ConsultarSaldoCommand { get; }

private async Task ConsultarSaldoAsync()
{
    Saldo = await _service.ObtenerSaldoAsync(Cuenta);
}
```

**4. Agregar en la Vista:**
```xml
<!-- MainPage.xaml -->
<Label Text="{Binding Saldo, StringFormat='Saldo: S/. {0:N2}'}" />
<Button Text="Ver Saldo" Command="{Binding ConsultarSaldoCommand}" />
```

## 🔧 Configuraciones Comunes

### Cambiar URL del Servidor

**Archivo:** `Models/ServidorConfig.cs`

```csharp
new ServidorConfig
{
    Tipo = TipoServidor.RestDotNet,
    Nombre = "REST .NET",
    Url = "http://192.168.1.100:5111/api/eureka",  // ← Cambiar aquí
    // ...
}
```

### Usar IP Real en Lugar de Localhost

Para Android/iOS necesitas la IP real de tu máquina:

**Windows - Encontrar tu IP:**
```bash
ipconfig
# Buscar "IPv4 Address" de tu adaptador de red activo
# Ejemplo: 192.168.1.100
```

**Actualizar URLs:**
```csharp
Url = "http://192.168.1.100:5111/api/eureka"
```

### Agregar Permisos en Android

**Archivo:** `Platforms/Android/AndroidManifest.xml`

```xml
<manifest>
    <uses-permission android:name="android.permission.INTERNET" />
    <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
    
    <application android:usesCleartextTraffic="true">
        <!-- ... -->
    </application>
</manifest>
```

## 🐛 Solución de Problemas Comunes

### Error: "No se puede conectar al servidor"

**Solución para Android Emulator:**
```csharp
// Cambiar localhost por 10.0.2.2
Url = "http://10.0.2.2:5111/api/eureka"
```

**Solución para dispositivo físico:**
```csharp
// Usar IP de tu máquina
Url = "http://192.168.1.100:5111/api/eureka"
```

### Error: Workload MAUI no instalado

```bash
dotnet workload install maui
dotnet workload install android  # Para Android
dotnet workload install ios      # Para iOS (solo Mac)
```

### Error al Compilar para Android

```bash
# Limpiar y reconstruir
dotnet clean
dotnet build -f net8.0-android
```

## 📱 Testing en Diferentes Plataformas

### Windows
```bash
dotnet build -t:Run -f net8.0-windows10.0.19041.0
```

### Android Emulator
1. Abrir Android Device Manager en Visual Studio
2. Crear/Iniciar emulador
3. Ejecutar app desde Visual Studio

### iOS Simulator (solo Mac)
```bash
dotnet build -t:Run -f net8.0-ios
```

## 🎨 Personalización Rápida

### Cambiar Colores

**Archivo:** `App.xaml`

```xml
<Color x:Key="Primary">#FF6B35</Color>  <!-- Naranja -->
<Color x:Key="Secondary">#004E89</Color> <!-- Azul -->
```

### Cambiar Título

**Archivo:** `Eurekabank_Maui.csproj`

```xml
<ApplicationTitle>Mi Banco Móvil</ApplicationTitle>
```

## 📚 Recursos Adicionales

- [Documentación .NET MAUI](https://docs.microsoft.com/dotnet/maui)
- [Tutoriales MAUI](https://dotnet.microsoft.com/learn/maui)
- [MVVM Pattern](https://docs.microsoft.com/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)

## ✅ Checklist de Verificación

Antes de hacer commit o entregar:

- [ ] Proyecto compila sin errores
- [ ] App se ejecuta en al menos una plataforma
- [ ] Login funciona con los 4 servidores
- [ ] Todas las operaciones bancarias funcionan
- [ ] UI se ve correctamente en diferentes tamaños
- [ ] Manejo de errores funciona
- [ ] Código comentado y documentado
- [ ] README actualizado

---

## 🎓 Siguientes Pasos

1. ✅ Completar esta guía
2. 📚 Leer el README.md completo
3. 🧪 Probar con los 4 servidores
4. 🎨 Personalizar la UI
5. ⚙️ Agregar nuevas funcionalidades
6. 🚀 Desplegar en tienda de aplicaciones (opcional)

**¡Listo para empezar! 🎉**
