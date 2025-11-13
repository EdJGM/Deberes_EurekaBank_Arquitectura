# 📚 TUTORIAL PASO A PASO - Cliente Consola Eurekabank

Guía completa desde cero hasta ejecutar todas las operaciones bancarias.

---

## 🎯 OBJETIVO DEL TUTORIAL

Al finalizar este tutorial serás capaz de:
- ✅ Instalar y configurar el proyecto
- ✅ Conectarte a cualquiera de los 4 servidores
- ✅ Realizar login exitoso
- ✅ Ejecutar todas las operaciones bancarias
- ✅ Solucionar problemas básicos

**Tiempo estimado:** 20 minutos

---

## 📋 PARTE 1: PREPARACIÓN (5 minutos)

### Paso 1.1: Verificar Requisitos

**Windows:**
```cmd
# Abre CMD o PowerShell y ejecuta:
dotnet --version
```

**Linux/Mac:**
```bash
# Abre Terminal y ejecuta:
dotnet --version
```

**Resultado esperado:** 
```
6.0.xxx o superior
```

**Si no tienes .NET:**
1. Ve a: https://dotnet.microsoft.com/download/dotnet/6.0
2. Descarga el **SDK** (no solo Runtime)
3. Instala y reinicia la terminal
4. Verifica nuevamente

---

### Paso 1.2: Descargar el Proyecto

1. Descarga: `Eurekabank_Cliente_Consola_Unificado.zip`
2. Extrae en una carpeta (ej: `C:\Proyectos\Eurekabank`)
3. Verifica que veas estos archivos:
   ```
   ✓ Program.cs
   ✓ ejecutar.bat (o ejecutar.sh)
   ✓ README.md
   ✓ appsettings.json
   ```

---

### Paso 1.3: Verificar Servidor

Antes de continuar, asegúrate de tener al menos UN servidor ejecutándose.

**Prueba rápida:**

**REST .NET (Recomendado):**
```
Abre navegador: http://localhost:5111/swagger
¿Se abrió Swagger UI? → ✅ Servidor activo
```

**SOAP .NET:**
```
Abre navegador: http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl
¿Apareció XML? → ✅ Servidor activo
```

**Si no tienes servidor activo:**
- Inicia primero uno de los servidores del proyecto Eurekabank
- Lee la documentación de ese servidor
- Luego vuelve a este tutorial

---

## 🚀 PARTE 2: PRIMERA EJECUCIÓN (3 minutos)

### Paso 2.1: Ejecutar el Cliente

**Opción A - Doble Clic (Más fácil):**

**Windows:**
```
1. Doble clic en: ejecutar.bat
2. Espera a que compile (15-30 segundos)
3. La aplicación se abrirá automáticamente
```

**Linux/Mac:**
```bash
1. Abre terminal en la carpeta del proyecto
2. Ejecuta: ./ejecutar.sh
3. Espera la compilación
```

**Opción B - Manual:**
```bash
# Abre terminal en la carpeta del proyecto
dotnet restore
dotnet build
dotnet run
```

---

### Paso 2.2: Pantalla de Bienvenida

Deberías ver algo como:

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

**¿No ves esta pantalla?**
- Verifica que tengas .NET 6.0 instalado
- Revisa si hay errores en la compilación
- Lee la sección de troubleshooting

---

### Paso 2.3: Seleccionar Servidor

**Para este tutorial usaremos REST .NET (opción 3):**

```
Ingrese su opción (1-4): 3
```

**Presiona Enter**

Verás:
```
🔍 Verificando conexión con el servidor...
✅ Conectado exitosamente a: REST_DOTNET
   Servicio Eureka REST activo y funcionando correctamente
```

**Si ves error:**
```
❌ No se pudo conectar al servidor: Connection refused
```

**Solución:**
1. Verifica que el servidor REST .NET esté ejecutándose
2. Confirma el puerto (5111)
3. Intenta con otro servidor (opciones 1, 2 o 4)

---

## 🔐 PARTE 3: AUTENTICACIÓN (2 minutos)

### Paso 3.1: Pantalla de Login

Después de conectarte exitosamente, verás:

```
════════════════════════════════════════════════════════════
 🔐 INICIO DE SESIÓN - REST_DOTNET
════════════════════════════════════════════════════════════

👤 Usuario: _
```

---

### Paso 3.2: Ingresar Credenciales

**Usa estas credenciales de prueba:**

```
👤 Usuario: cromero
🔑 Contraseña: chicho
```

**Notas:**
- La contraseña se ocultará con asteriscos (*****)
- Escribe exactamente como se muestra (minúsculas)
- Presiona Enter después de cada campo

---

### Paso 3.3: Autenticación Exitosa

Verás:

```
🔍 Autenticando...
✅ Login exitoso
   Bienvenido, cromero!
```

**Si ves error:**
```
❌ Credenciales inválidas
```

**Causas comunes:**
1. Usuario o contraseña incorrectos
2. Usuario con estado ANULADO
3. Usuario no existe en la BD

**Otros usuarios disponibles:**
- lcastro / flaca
- aramos / china
- MONSTER / MONSTER9

(Ver `DATOS_PRUEBA.md` para lista completa)

---

## 💼 PARTE 4: MENÚ PRINCIPAL (2 minutos)

### Paso 4.1: Explorar el Menú

Tras login exitoso verás:

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

### Paso 4.2: Entender las Opciones

| Opción | Función | Ejemplo |
|--------|---------|---------|
| 1 | Ver historial de transacciones | Lista movimientos de una cuenta |
| 2 | Agregar dinero a cuenta | Depositar S/. 500 |
| 3 | Retirar dinero de cuenta | Retirar S/. 200 |
| 4 | Mover dinero entre cuentas | Transferir S/. 150 |
| 5 | Salir del sistema | Cerrar sesión |

---

## 📊 PARTE 5: CONSULTAR MOVIMIENTOS (3 minutos)

### Paso 5.1: Seleccionar Opción

```
Seleccione una opción: 1
```

**Presiona Enter**

---

### Paso 5.2: Ingresar Cuenta

```
════════════════════════════════════════════════════════════
 📊 CONSULTAR MOVIMIENTOS
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: _
```

**Para este tutorial, necesitas una cuenta válida de tu BD.**

**¿No conoces ninguna cuenta?**

Ejecuta este SQL en tu base de datos:
```sql
SELECT TOP 5 chr_cuencodigo, vch_cuentipo, dec_cuensaldo 
FROM cuenta 
WHERE vch_cuenestado = 'ACTIVO'
ORDER BY chr_cuencodigo;
```

**Ejemplo de cuenta:** `00100001`

```
Ingrese el número de cuenta: 00100001
```

---

### Paso 5.3: Ver Resultados

```
🔍 Consultando movimientos...

✅ Se encontraron 5 movimientos:

┌─────┬────────────┬────────────────────────┬──────────┬────────────┐
│ Nro │   Fecha    │          Tipo          │  Acción  │   Importe  │
├─────┼────────────┼────────────────────────┼──────────┼────────────┤
│   1 │ 15/01/2025 │ Apertura de cuenta     │ INGRESO  │ S/.  500.00│
│   2 │ 20/01/2025 │ Depósito               │ INGRESO  │ S/. 1000.00│
│   3 │ 25/01/2025 │ Retiro                 │ SALIDA   │ S/.  200.00│
│   4 │ 28/01/2025 │ Transferencia          │ SALIDA   │ S/.  150.00│
│   5 │ 01/02/2025 │ Interés                │ INGRESO  │ S/.   10.50│
└─────┴────────────┴────────────────────────┴──────────┴────────────┘

Presione cualquier tecla para continuar...
```

**Análisis de la tabla:**
- **Nro:** Número secuencial del movimiento
- **Fecha:** Cuándo ocurrió
- **Tipo:** Qué tipo de operación
- **Acción:** INGRESO (suma) o SALIDA (resta)
- **Importe:** Cantidad en soles

---

### Paso 5.4: Volver al Menú

Presiona **cualquier tecla** para regresar al menú principal.

---

## 💰 PARTE 6: REALIZAR DEPÓSITO (3 minutos)

### Paso 6.1: Seleccionar Opción

En el menú principal:
```
Seleccione una opción: 2
```

---

### Paso 6.2: Ingresar Datos

```
════════════════════════════════════════════════════════════
 💰 REALIZAR DEPÓSITO
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: 00100001
Ingrese el importe a depositar: S/. 500.00
```

**Notas:**
- Cuenta: Usa la misma del paso anterior
- Importe: Ingresa solo el número (500.00)
- No incluyas el símbolo S/.

---

### Paso 6.3: Confirmar Operación

```
⚠️  Va a depositar S/. 500.00 en la cuenta 00100001
¿Confirma la operación? (S/N): _
```

**¡IMPORTANTE!** Lee cuidadosamente los datos.

- Si está todo correcto: **S** (mayúscula o minúscula)
- Si quieres cancelar: **N**

```
¿Confirma la operación? (S/N): S
```

---

### Paso 6.4: Procesamiento

```
🔄 Procesando depósito...
✅ Depósito registrado exitosamente

Presione cualquier tecla para continuar...
```

**¡Felicidades!** Has realizado tu primer depósito.

---

### Paso 6.5: Verificar (Opcional)

Para confirmar que funcionó:
1. Vuelve a Consultar Movimientos (Opción 1)
2. Ingresa la misma cuenta
3. Deberías ver el depósito de S/. 500.00 como último movimiento

---

## 💸 PARTE 7: REALIZAR RETIRO (3 minutos)

### Paso 7.1: Seleccionar Opción

```
Seleccione una opción: 3
```

---

### Paso 7.2: Ingresar Datos

```
════════════════════════════════════════════════════════════
 💸 REALIZAR RETIRO
════════════════════════════════════════════════════════════

Ingrese el número de cuenta: 00100001
Ingrese el importe a retirar: S/. 200.00
```

**⚠️ IMPORTANTE:** Asegúrate de que la cuenta tenga saldo suficiente.

---

### Paso 7.3: Confirmar y Procesar

```
⚠️  Va a retirar S/. 200.00 de la cuenta 00100001
¿Confirma la operación? (S/N): S

🔄 Procesando retiro...
✅ Retiro registrado exitosamente

Presione cualquier tecla para continuar...
```

---

## 🔄 PARTE 8: REALIZAR TRANSFERENCIA (3 minutos)

### Paso 8.1: Preparación

Para esta operación necesitas **DOS cuentas válidas**:
- Una cuenta ORIGEN (con saldo)
- Una cuenta DESTINO (puede estar vacía)

**Consulta cuentas disponibles:**
```sql
SELECT chr_cuencodigo, dec_cuensaldo 
FROM cuenta 
WHERE vch_cuenestado = 'ACTIVO' 
LIMIT 2;
```

---

### Paso 8.2: Seleccionar Opción

```
Seleccione una opción: 4
```

---

### Paso 8.3: Ingresar Datos

```
════════════════════════════════════════════════════════════
 🔄 REALIZAR TRANSFERENCIA
════════════════════════════════════════════════════════════

Ingrese la cuenta de origen: 00100001
Ingrese la cuenta de destino: 00200002
Ingrese el importe a transferir: S/. 150.00
```

---

### Paso 8.4: Confirmar y Procesar

```
⚠️  Va a transferir S/. 150.00
   Desde: 00100001
   Hacia: 00200002
¿Confirma la operación? (S/N): S

🔄 Procesando transferencia...
✅ Transferencia registrada exitosamente

Presione cualquier tecla para continuar...
```

---

### Paso 8.5: Verificar (Opcional)

Consulta movimientos de ambas cuentas:
- **Cuenta origen:** Verás una SALIDA de S/. 150.00
- **Cuenta destino:** Verás un INGRESO de S/. 150.00

---

## 🚪 PARTE 9: CERRAR SESIÓN Y SALIR (1 minuto)

### Paso 9.1: Cerrar Sesión

```
Seleccione una opción: 5

👋 Sesión cerrada.

¿Desea conectarse a otro servidor? (S/N): _
```

---

### Paso 9.2: Conectar a Otro Servidor

**Opción A - Probar otro servidor:**
```
¿Desea conectarse a otro servidor? (S/N): S
```
Volverás a la pantalla de selección de servidor.

**Opción B - Salir completamente:**
```
¿Desea conectarse a otro servidor? (S/N): N

👋 Gracias por usar Eurekabank. ¡Hasta pronto!
```

---

## ✅ RESUMEN DE LO APRENDIDO

Has completado exitosamente:

- ✅ Instalación y configuración
- ✅ Conexión a servidor REST .NET
- ✅ Autenticación con usuario/contraseña
- ✅ Consulta de movimientos
- ✅ Realización de depósito
- ✅ Realización de retiro
- ✅ Realización de transferencia
- ✅ Cierre de sesión

---

## 🎯 PRÓXIMOS PASOS

### Nivel Intermedio:
1. Prueba conectarte a los 4 servidores diferentes
2. Realiza operaciones con diferentes usuarios
3. Experimenta con cuentas en diferentes monedas
4. Prueba escenarios de error (cuentas inválidas, saldo insuficiente)

### Nivel Avanzado:
1. Modifica el código para agregar nuevas funcionalidades
2. Cambia las URLs en `appsettings.json`
3. Personaliza los colores y mensajes
4. Agrega logging de operaciones
5. Crea tu propia interfaz gráfica

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### "No encuentro una cuenta válida"
```sql
-- Ejecuta esto en tu BD para ver cuentas:
SELECT * FROM cuenta WHERE vch_cuenestado = 'ACTIVO';

-- Si no hay cuentas, crea una:
INSERT INTO cuenta (chr_cuencodigo, chr_cliecodigo, chr_monecodigo, 
                    dtt_cuenfechaCreacion, dec_cuensaldo, vch_cuentipo, vch_cuenestado)
VALUES ('00100001', '00001', '01', GETDATE(), 1000.00, 'AHORRO', 'ACTIVO');
```

### "El depósito no aparece en movimientos"
- Espera 2-3 segundos
- Vuelve a consultar
- Verifica en la BD directamente:
```sql
SELECT * FROM movimiento WHERE chr_cuencodigo = '00100001' 
ORDER BY int_movinumero DESC;
```

### "Error al transferir"
Causas comunes:
- Cuentas no existen
- Cuenta origen sin saldo
- Origen y destino son iguales

---

## 📚 RECURSOS ADICIONALES

| Documento | Para qué sirve |
|-----------|----------------|
| README.md | Documentación completa |
| FAQ.md | Preguntas frecuentes |
| DATOS_PRUEBA.md | Usuarios y cuentas |
| CAPTURAS.md | Ver cómo se ve la UI |
| DIAGRAMAS.md | Arquitectura del sistema |

---

**¡Felicitaciones por completar el tutorial!** 🎉

Ahora eres capaz de usar el Cliente Consola Eurekabank de manera profesional.

---

**Tiempo total:** 20 minutos  
**Dificultad:** ⭐⭐☆☆☆ (Fácil)  
**Nivel requerido:** Principiante
