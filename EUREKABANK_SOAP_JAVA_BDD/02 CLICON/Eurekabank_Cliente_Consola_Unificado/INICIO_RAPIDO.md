# ⚡ INICIO RÁPIDO - 5 Minutos

Guía express para ejecutar el cliente en menos de 5 minutos.

---

## ✅ Checklist Pre-requisitos

- [ ] .NET 6.0 SDK instalado ([Descargar aquí](https://dotnet.microsoft.com/download/dotnet/6.0))
- [ ] Al menos un servidor Eurekabank ejecutándose
- [ ] Archivo ZIP descargado y extraído

---

## 🚀 Pasos de Ejecución

### Windows

```cmd
1. Extraer Eurekabank_Cliente_Consola_Unificado.zip
2. Hacer doble clic en: ejecutar.bat
3. ¡Listo! La aplicación se compilará y ejecutará automáticamente
```

### Linux / Mac

```bash
1. Extraer el ZIP
2. Abrir terminal en la carpeta
3. Ejecutar: ./ejecutar.sh
4. ¡Listo!
```

### Visual Studio

```
1. Abrir: Eurekabank_Cliente_Consola_Unificado.csproj
2. Presionar: F5 (o Ctrl+F5)
3. ¡Listo!
```

---

## 🎮 Primer Uso

### Paso 1: Seleccionar Servidor
```
Opciones:
1 = SOAP .NET
2 = SOAP Java
3 = REST .NET  ← Recomendado para empezar
4 = REST Java
```

**Recomendación:** Elige opción 3 (REST .NET) si no estás seguro.

### Paso 2: Login
```
Usuario: cromero
Contraseña: chicho
```

**Otros usuarios disponibles en:** `DATOS_PRUEBA.md`

### Paso 3: Probar Operaciones

**Opción 1 - Consultar Movimientos:**
```
- Selecciona: 1
- Ingresa una cuenta válida de tu BD
- Observa la tabla de movimientos
```

**Opción 2 - Realizar Depósito:**
```
- Selecciona: 2
- Cuenta: [número de cuenta válido]
- Importe: 500.00
- Confirma con: S
- ¡Listo! Depósito registrado
```

---

## ❓ Problemas Comunes

### "dotnet no se reconoce..."
**Solución:** Instala .NET 6.0 SDK desde [aquí](https://dotnet.microsoft.com/download/dotnet/6.0)

### "No se pudo conectar al servidor"
**Solución:** 
1. Verifica que el servidor esté ejecutándose
2. Prueba con otro servidor (opciones 1-4)
3. Revisa el puerto en el servidor

### "Credenciales inválidas"
**Solución:**
1. Usa: cromero / chicho
2. O consulta otros usuarios en `DATOS_PRUEBA.md`
3. Verifica que el usuario esté ACTIVO en la BD

### "Cuenta no encontrada"
**Solución:**
1. Consulta cuentas disponibles en tu base de datos
2. Ejecuta: `SELECT chr_cuencodigo FROM cuenta WHERE vch_cuenestado = 'ACTIVO'`

---

## 📚 Siguiente Nivel

Ya funcionó? Aprende más:

- **Documentación completa:** `README.md`
- **Usuarios y datos:** `DATOS_PRUEBA.md`
- **Ver capturas de pantalla:** `CAPTURAS.md`
- **Resumen ejecutivo:** `RESUMEN.md`

---

## 💡 Tips Rápidos

1. **Números para todo:** Usa el teclado numérico para seleccionar opciones
2. **Confirmaciones:** Siempre confirma operaciones críticas con "S"
3. **Colores importan:** 
   - 🟩 Verde = Todo bien
   - 🟥 Rojo = Algo falló
   - 🟨 Amarillo = Cuidado, confirma
4. **Salir:** Opción 5 cierra sesión, luego "N" para salir

---

## 🎯 Objetivo Logrado

Si llegaste hasta aquí y pudiste:
- ✅ Conectarte a un servidor
- ✅ Hacer login
- ✅ Ver el menú principal

**¡Felicitaciones! 🎉** Estás listo para usar el cliente.

---

**¿Dudas?** Lee `README.md` para información detallada.
**¿Errores?** Revisa los logs y mensajes en rojo.
**¿Sugerencias?** Este es un proyecto educativo, siéntete libre de mejorarlo.

---

**Tiempo estimado:** ⏱️ 3-5 minutos  
**Dificultad:** 🟢 Fácil  
**Requisito:** .NET 6.0 SDK
