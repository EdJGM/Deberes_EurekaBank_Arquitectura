@echo off
echo ========================================
echo   EUREKABANK - CLIENTE CONSOLA
echo   Compilacion y Ejecucion
echo ========================================
echo.

REM Verificar si existe dotnet
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK no esta instalado
    echo Por favor, descargue e instale .NET 6.0 SDK desde:
    echo https://dotnet.microsoft.com/download/dotnet/6.0
    pause
    exit /b 1
)

echo [1/3] Restaurando dependencias...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Fallo al restaurar dependencias
    pause
    exit /b 1
)

echo.
echo [2/3] Compilando proyecto...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo ERROR: Fallo la compilacion
    pause
    exit /b 1
)

echo.
echo [3/3] Ejecutando aplicacion...
echo.
dotnet run --configuration Release

pause
