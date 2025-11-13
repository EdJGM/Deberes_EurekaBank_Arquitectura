#!/bin/bash

echo "========================================"
echo "  EUREKABANK - CLIENTE CONSOLA"
echo "  Compilación y Ejecución"
echo "========================================"
echo ""

# Verificar si existe dotnet
if ! command -v dotnet &> /dev/null
then
    echo "❌ ERROR: .NET SDK no está instalado"
    echo "Por favor, descargue e instale .NET 6.0 SDK desde:"
    echo "https://dotnet.microsoft.com/download/dotnet/6.0"
    exit 1
fi

echo "[1/3] Restaurando dependencias..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "❌ ERROR: Falló al restaurar dependencias"
    exit 1
fi

echo ""
echo "[2/3] Compilando proyecto..."
dotnet build --configuration Release
if [ $? -ne 0 ]; then
    echo "❌ ERROR: Falló la compilación"
    exit 1
fi

echo ""
echo "[3/3] Ejecutando aplicación..."
echo ""
dotnet run --configuration Release

echo ""
echo "Presione Enter para salir..."
read
