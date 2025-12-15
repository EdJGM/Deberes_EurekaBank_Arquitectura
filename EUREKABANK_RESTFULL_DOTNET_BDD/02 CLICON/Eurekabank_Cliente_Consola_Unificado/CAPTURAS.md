# 📸 Capturas Visuales - Cliente Consola Eurekabank

Este documento muestra cómo se verá la aplicación durante su ejecución.

---

## 1️⃣ Pantalla de Bienvenida

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

---

## 2️⃣ Verificación de Conexión

```
Ingrese su opción (1-4): 3

🔍 Verificando conexión con el servidor...
✅ Conectado exitosamente a: REST_DOTNET
   Servicio Eureka REST activo y funcionando correctamente
```

---

## 3️⃣ Pantalla de Login

```
════════════════════════════════════════════════════════════
 🔐 INICIO DE SESIÓN - REST_DOTNET
════════════════════════════════════════════════════════════

👤 Usuario: cromero
🔑 Contraseña: ******

🔍 Autenticando...
✅ Login exitoso
   Bienvenido, cromero!
```

---

## 4️⃣ Menú Principal

```
════════════════════════════════════════════════════════════
 💼 MENÚ PRINCIPAL - Usuario: cromero
════════════════════════════════════════════════════════════

  1️⃣  Consultar Movimientos de Cuenta
  2️⃣  Realizar Depósito
  3️⃣  Realizar Retiro
  4️⃣  Realizar Transferencia
  5️⃣  Cerrar Sesión

Seleccione una opción: _
```

---

## 5️⃣ Consultar Movimientos

```
════════════════════════════════════════════════════════════
 📊 CONSULTAR MOVIMIENTOS
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: 00100001

🔍 Consultando movimientos...

✅ Se encontraron 5 movimientos:

┌─────┬────────────┬────────────────────────┬──────────┬────────────┐
│ Nro │   Fecha    │          Tipo          │  Acción  │   Importe  │
├─────┼────────────┼────────────────────────┼──────────┼────────────┤
│   1 │ 15/01/2025 │ Apertura de cuenta     │ INGRESO  │ S/.  500.00 │
│   2 │ 20/01/2025 │ Depósito               │ INGRESO  │ S/. 1000.00 │
│   3 │ 25/01/2025 │ Retiro                 │ SALIDA   │ S/.  200.00 │
│   4 │ 28/01/2025 │ Transferencia          │ SALIDA   │ S/.  150.00 │
│   5 │ 01/02/2025 │ Interés                │ INGRESO  │ S/.   10.50 │
└─────┴────────────┴────────────────────────┴──────────┴────────────┘

Presione cualquier tecla para continuar...
```

---

## 6️⃣ Realizar Depósito

```
════════════════════════════════════════════════════════════
 💰 REALIZAR DEPÓSITO
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: 00100001
Ingrese el importe a depositar: S/. 500.00

⚠️  Va a depositar S/. 500.00 en la cuenta 00100001
¿Confirma la operación? (S/N): S

🔄 Procesando depósito...
✅ Depósito registrado exitosamente

Presione cualquier tecla para continuar...
```

---

## 7️⃣ Realizar Retiro

```
════════════════════════════════════════════════════════════
 💸 REALIZAR RETIRO
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: 00100001
Ingrese el importe a retirar: S/. 200.00

⚠️  Va a retirar S/. 200.00 de la cuenta 00100001
¿Confirma la operación? (S/N): S

🔄 Procesando retiro...
✅ Retiro registrado exitosamente

Presione cualquier tecla para continuar...
```

---

## 8️⃣ Realizar Transferencia

```
════════════════════════════════════════════════════════════
 🔄 REALIZAR TRANSFERENCIA
════════════════════════════════════════════════════════════

Ingrese la cuenta de origen: 00100001
Ingrese la cuenta de destino: 00200002
Ingrese el importe a transferir: S/. 150.00

⚠️  Va a transferir S/. 150.00
   Desde: 00100001
   Hacia: 00200002
¿Confirma la operación? (S/N): S

🔄 Procesando transferencia...
✅ Transferencia registrada exitosamente

Presione cualquier tecla para continuar...
```

---

## 9️⃣ Error de Conexión (Ejemplo)

```
🔍 Verificando conexión con el servidor...
❌ No se pudo conectar al servidor: Connection refused

❌ No se pudo conectar al servidor. Presione cualquier tecla para reintentar...
```

---

## 🔟 Login Fallido (Ejemplo)

```
👤 Usuario: usuario_invalido
🔑 Contraseña: ******

🔍 Autenticando...
❌ Credenciales inválidas

❌ Login fallido. Presione cualquier tecla para reintentar...
```

---

## 1️⃣1️⃣ Cerrar Sesión y Salir

```
Seleccione una opción: 5

👋 Sesión cerrada.

¿Desea conectarse a otro servidor? (S/N): N

👋 Gracias por usar Eurekabank. ¡Hasta pronto!
```

---

## 🎨 Código de Colores

| Color | Significado | Uso |
|-------|-------------|-----|
| 🟦 Cyan | Títulos y encabezados | Separadores visuales |
| 🟩 Verde | Éxito | Operaciones completadas |
| 🟥 Rojo | Error | Fallos y problemas |
| 🟨 Amarillo | Advertencia | Confirmaciones críticas |
| ⬜ Blanco | Información | Texto general |

---

## ⚙️ Características de la UI

✨ **Animaciones de Carga**
```
🔍 Procesando depósito.
🔍 Procesando depósito..
🔍 Procesando depósito...
```

✨ **Formato de Moneda**
```
S/. 1,500.00    (correcto)
S/. 1500.00     (también válido)
```

✨ **Formato de Fecha**
```
15/01/2025  (dd/MM/yyyy)
```

✨ **Validaciones**
- ✅ Números de cuenta válidos
- ✅ Importes positivos
- ✅ Confirmación de operaciones críticas
- ✅ Contraseñas ocultas

---

## 📱 Experiencia de Usuario

### Flujo Típico (60 segundos)
```
0:00  - Inicio de aplicación
0:05  - Selección de servidor REST .NET
0:08  - Verificación exitosa de conexión
0:10  - Login con usuario/contraseña
0:15  - Ingreso al menú principal
0:18  - Selección de "Consultar Movimientos"
0:20  - Ingreso de número de cuenta
0:25  - Visualización de tabla de movimientos
0:30  - Retorno al menú principal
0:33  - Selección de "Realizar Depósito"
0:35  - Ingreso de cuenta e importe
0:38  - Confirmación de operación
0:42  - Procesamiento exitoso
0:45  - Retorno al menú principal
0:48  - Selección de "Cerrar Sesión"
0:50  - Despedida del sistema
0:52  - Opción de reconectar
0:55  - Salida del programa
1:00  - Fin
```

---

## 💡 Tips de Usabilidad

1. **Navegación Rápida:** Usa números para seleccionar opciones
2. **Confirmaciones:** Lee cuidadosamente antes de confirmar con "S"
3. **Errores:** Los mensajes en rojo indican problemas - lee con atención
4. **Esperas:** Los puntos suspensivos (...) indican que se está procesando
5. **Salir:** Opción 5 cierra sesión, luego N sale del programa

---

**Nota:** Las capturas son representaciones textuales. Los colores reales
se verán en la consola cuando ejecutes la aplicación.
