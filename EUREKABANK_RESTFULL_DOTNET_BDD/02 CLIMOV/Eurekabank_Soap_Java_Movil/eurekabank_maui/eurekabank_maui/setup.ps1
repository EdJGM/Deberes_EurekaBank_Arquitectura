# ========================================
# Script de Configuración Inicial
# Eurekabank Mobile - Cliente MAUI
# ========================================

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   Eurekabank Mobile - Configuración   ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar .NET SDK
Write-Host "[1/5] Verificando .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ .NET SDK instalado: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "  ✗ .NET SDK no encontrado" -ForegroundColor Red
    Write-Host "  Descargue desde: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

# 2. Verificar Workloads MAUI
Write-Host "[2/5] Verificando workloads MAUI..." -ForegroundColor Yellow
$workloads = dotnet workload list 2>$null
if ($workloads -match "maui") {
    Write-Host "  ✓ Workload MAUI instalado" -ForegroundColor Green
} else {
    Write-Host "  ✗ Workload MAUI no encontrado" -ForegroundColor Red
    Write-Host "  Instalando MAUI workload..." -ForegroundColor Yellow
    dotnet workload install maui
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ MAUI instalado correctamente" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Error al instalar MAUI" -ForegroundColor Red
        exit 1
    }
}

# 3. Verificar estructura del proyecto
Write-Host "[3/5] Verificando estructura del proyecto..." -ForegroundColor Yellow
$requiredFiles = @(
    "Eurekabank_Maui.csproj",
    "App.xaml",
    "MauiProgram.cs"
)

$allFilesExist = $true
foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "  ✓ $file" -ForegroundColor Green
    } else {
        Write-Host "  ✗ $file no encontrado" -ForegroundColor Red
        $allFilesExist = $false
    }
}

if (-not $allFilesExist) {
    Write-Host "  Faltan archivos del proyecto" -ForegroundColor Red
    exit 1
}

# 4. Restaurar dependencias
Write-Host "[4/5] Restaurando dependencias..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Dependencias restauradas" -ForegroundColor Green
} else {
    Write-Host "  ✗ Error al restaurar dependencias" -ForegroundColor Red
    exit 1
}

# 5. Compilar proyecto
Write-Host "[5/5] Compilando proyecto..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Proyecto compilado exitosamente" -ForegroundColor Green
} else {
    Write-Host "  ✗ Error al compilar proyecto" -ForegroundColor Red
    exit 1
}

# Resumen
Write-Host ""
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║     ¡Configuración Completada! ✓       ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "Próximos pasos:" -ForegroundColor Cyan
Write-Host "  1. Iniciar un servidor Eurekabank" -ForegroundColor White
Write-Host "  2. Abrir el proyecto en Visual Studio 2022" -ForegroundColor White
Write-Host "  3. Seleccionar plataforma (Windows/Android)" -ForegroundColor White
Write-Host "  4. Presionar F5 para ejecutar" -ForegroundColor White
Write-Host ""
Write-Host "Usuarios de prueba:" -ForegroundColor Cyan
Write-Host "  • internet / internet" -ForegroundColor White
Write-Host "  • MONSTER / MONSTER9" -ForegroundColor White
Write-Host ""
Write-Host "Cuentas de prueba: 00100001, 00200001" -ForegroundColor White
Write-Host ""
Write-Host "Para más información, lee: README.md" -ForegroundColor Yellow
Write-Host ""
