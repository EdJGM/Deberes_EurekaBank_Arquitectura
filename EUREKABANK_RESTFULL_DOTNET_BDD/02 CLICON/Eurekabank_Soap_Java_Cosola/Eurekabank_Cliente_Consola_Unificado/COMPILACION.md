# 🔨 Guía de Compilación

## Visual Studio 2022

1. Abrir `Eurekabank_Cliente_Consola_Unificado.csproj`
2. Menú → Build → Build Solution (F6)
3. Menú → Debug → Start Without Debugging (Ctrl+F5)

## Visual Studio Code

1. Abrir carpeta del proyecto
2. Terminal → dotnet restore
3. Terminal → dotnet build
4. Terminal → dotnet run

## Línea de Comandos

### Windows
```cmd
ejecutar.bat
```

### Linux/Mac
```bash
chmod +x ejecutar.sh
./ejecutar.sh
```

## Notas

- Requiere .NET 6.0 SDK o superior
- El proyecto restaura automáticamente los paquetes NuGet
- Paquetes necesarios: Newtonsoft.Json, System.ServiceModel
