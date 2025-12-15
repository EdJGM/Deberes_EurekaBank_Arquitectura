# ❓ FAQ - Preguntas Frecuentes

Respuestas a las preguntas más comunes sobre el Cliente Consola Eurekabank.

---

## 🚀 INSTALACIÓN Y EJECUCIÓN

### ¿Qué necesito para ejecutar el cliente?
- .NET 6.0 SDK o superior
- Windows, Linux o macOS
- Al menos un servidor Eurekabank ejecutándose
- Conexión a la base de datos del servidor

### ¿Cómo instalo .NET 6.0?
1. Ve a: https://dotnet.microsoft.com/download/dotnet/6.0
2. Descarga el SDK (no solo el Runtime)
3. Instala y reinicia la terminal
4. Verifica con: `dotnet --version`

### ¿Cómo ejecuto el cliente?
**Opción más fácil:**
- Windows: Doble clic en `ejecutar.bat`
- Linux/Mac: `./ejecutar.sh`

**Opción avanzada:**
```bash
dotnet restore
dotnet build
dotnet run
```

### ¿Puedo ejecutarlo sin Visual Studio?
¡Sí! Solo necesitas el .NET SDK. Puedes usar:
- Scripts incluidos (ejecutar.bat / ejecutar.sh)
- Visual Studio Code
- Línea de comandos

---

## 🔌 CONEXIÓN A SERVIDORES

### ¿Qué servidor debo elegir?
Depende de cuál tengas ejecutándose:
- **SOAP .NET** (Opción 1): Si usas IIS con .NET Framework
- **SOAP Java** (Opción 2): Si usas Payara/GlassFish
- **REST .NET** (Opción 3): Si usas ASP.NET Core ⭐ Recomendado
- **REST Java** (Opción 4): Si usas Jakarta EE REST

### ¿Cómo sé si el servidor está ejecutándose?
Intenta acceder a:
- SOAP .NET: http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl
- REST .NET: http://localhost:5111/swagger
- SOAP Java: http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl
- REST Java: http://localhost:8080/Eurobank_Restfull_Java/api/eureka/health

### ¿Puedo cambiar los puertos o URLs?
Sí, edita el archivo `appsettings.json`:
```json
{
  "ServidoresConfig": {
    "RestDotNet": {
      "BaseUrl": "http://TU_SERVIDOR:PUERTO/api"
    }
  }
}
```

### ¿Qué significa "Connection refused"?
El servidor no está ejecutándose o el puerto está mal. Verifica:
1. El servidor está iniciado
2. El puerto es correcto
3. El firewall no bloquea la conexión

---

## 🔐 AUTENTICACIÓN Y USUARIOS

### ¿Qué usuario puedo usar para probar?
Usuarios de prueba disponibles:
- `cromero` / `chicho`
- `lcastro` / `flaca`
- `MONSTER` / `MONSTER9`

Ver lista completa en: `DATOS_PRUEBA.md`

### ¿Por qué dice "Credenciales inválidas"?
Posibles causas:
1. Usuario o contraseña incorrectos
2. Usuario con estado ANULADO
3. Usuario no existe en la base de datos
4. Error en el servidor de autenticación

### ¿Cómo agrego un nuevo usuario?
Ejecuta este SQL en tu base de datos:
```sql
-- SQL Server / MySQL
INSERT INTO empleado (chr_emplcodigo, vch_emplpaterno, vch_emplmaterno, vch_emplnombre, vch_emplciudad, vch_empldireccion)
VALUES ('0015', 'Apellido', 'Apellido2', 'Nombre', 'Ciudad', 'Dirección');

INSERT INTO usuario (chr_emplcodigo, vch_emplusuario, vch_emplclave, vch_emplestado)
VALUES ('0015', 'miusuario', 'mipassword', 'ACTIVO');
```

### ¿Las contraseñas se guardan seguras?
En este proyecto educativo, las contraseñas están en texto plano. En producción deberías usar:
- Hashing (bcrypt, Argon2)
- Salting
- HTTPS
- Tokens JWT

---

## 💼 OPERACIONES BANCARIAS

### ¿Qué número de cuenta debo usar?
Depende de tu base de datos. Consulta cuentas activas:
```sql
SELECT chr_cuencodigo, vch_cuentipo, dec_cuensaldo 
FROM cuenta 
WHERE vch_cuenestado = 'ACTIVO';
```

### ¿Por qué no veo movimientos?
Posibles razones:
1. La cuenta no existe
2. La cuenta no tiene movimientos registrados
3. Error en la consulta del servidor

### ¿Puedo hacer depósitos negativos?
No, el cliente valida que el importe sea positivo.

### ¿Qué pasa si retiro más de mi saldo?
Depende de la lógica del servidor. Normalmente debería rechazarse.

### ¿Puedo transferir a la misma cuenta?
No, el servidor debería validar que origen ≠ destino.

### ¿Los cambios persisten en la base de datos?
Sí, todas las operaciones se registran en la BD del servidor.

---

## 🐛 ERRORES COMUNES

### "dotnet no se reconoce como comando"
**Causa:** .NET SDK no instalado o no en PATH  
**Solución:** Instala .NET 6.0 SDK y reinicia la terminal

### "No se pudo cargar el archivo o ensamblado..."
**Causa:** Paquetes NuGet no restaurados  
**Solución:** Ejecuta `dotnet restore`

### "Error al compilar el proyecto"
**Causa:** Error en el código o dependencias faltantes  
**Solución:**
1. `dotnet clean`
2. `dotnet restore`
3. `dotnet build`

### "Timeout al conectar"
**Causa:** Servidor lento o no responde  
**Solución:**
1. Verifica que el servidor esté activo
2. Aumenta timeout en el código (línea `_httpClient.Timeout`)
3. Revisa logs del servidor

### "Error de serialización JSON"
**Causa:** Formato de respuesta inesperado  
**Solución:**
1. Verifica que el servidor esté retornando JSON válido
2. Usa Postman para ver la respuesta del servidor
3. Ajusta los modelos si es necesario

### "No se puede parsear la respuesta SOAP"
**Causa:** Formato XML inválido o namespace incorrecto  
**Solución:**
1. Verifica el WSDL del servidor
2. Comprueba los namespaces en SoapServices.cs
3. Usa herramientas como SoapUI para probar

---

## 🔧 PERSONALIZACIÓN

### ¿Cómo cambio los colores de la UI?
Edita en `Program.cs`:
```csharp
Console.ForegroundColor = ConsoleColor.Green;  // Cambia el color
```

Colores disponibles:
- Black, Blue, Cyan, DarkBlue, DarkCyan, DarkGray
- DarkGreen, DarkMagenta, DarkRed, DarkYellow
- Gray, Green, Magenta, Red, White, Yellow

### ¿Cómo agrego una nueva operación?
1. Agrega método a `IEurekabankService.cs`
2. Implementa en cada servicio (Rest/Soap)
3. Agrega opción al menú en `Program.cs`
4. Crea método UI para la operación

### ¿Cómo conecto a un quinto servidor?
1. Crea nueva clase que implemente `IEurekabankService`
2. Agrega al enum `TipoServidor`
3. Actualiza `SeleccionarServidor()` en Program.cs
4. Configura URL en `appsettings.json`

### ¿Puedo agregar logging?
Sí, puedes usar:
- `Microsoft.Extensions.Logging`
- `Serilog`
- `NLog`

Ejemplo:
```csharp
_logger.LogInformation("Operación ejecutada: {Operacion}", "Deposito");
```

---

## 📊 DATOS Y BASE DE DATOS

### ¿Puedo usar mi propia base de datos?
Sí, solo asegúrate de:
1. Usar el mismo esquema de tablas
2. Cargar datos de prueba
3. Configurar el connection string en el servidor

### ¿Cómo cargo datos de prueba?
Ejecuta los scripts en cada proyecto servidor:
- `03 BDD/1_crear_bd.sql` (crear tablas)
- `03 BDD/2_cargar_datos.sql` (insertar datos)

### ¿Funciona con PostgreSQL?
No directamente. Necesitarías:
1. Modificar los servidores
2. Ajustar los scripts SQL
3. Cambiar drivers de conexión

---

## 🔒 SEGURIDAD

### ¿Es seguro para producción?
NO. Este es un proyecto educativo. Para producción necesitas:
- ✅ HTTPS obligatorio
- ✅ Tokens JWT
- ✅ Hashing de contraseñas
- ✅ Rate limiting
- ✅ Validación de entrada
- ✅ Logging de auditoría
- ✅ Encriptación de datos sensibles

### ¿Por qué las contraseñas se envían en texto plano?
Es una simplificación educativa. En producción usa:
- Hashing (bcrypt, Argon2, PBKDF2)
- HTTPS para transmisión
- Nunca logs de contraseñas

### ¿Dónde se almacena la sesión?
En memoria (variable `autenticado` y `usuarioActual`). Se pierde al cerrar.

---

## 🧪 TESTING

### ¿Cómo pruebo sin servidor?
Puedes:
1. Crear mocks de `IEurekabankService`
2. Usar herramientas como Moq
3. Crear un servidor de prueba local

### ¿Hay pruebas unitarias incluidas?
No en esta versión. Puedes agregar:
- xUnit o NUnit
- Moq para mocking
- FluentAssertions

### ¿Cómo pruebo cada servidor por separado?
1. Ejecuta un servidor a la vez
2. Selecciona ese servidor en el cliente
3. Prueba todas las operaciones

---

## 📦 DISTRIBUCIÓN

### ¿Cómo compilo para Windows?
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

### ¿Cómo compilo para Linux?
```bash
dotnet publish -c Release -r linux-x64 --self-contained
```

### ¿Cómo creo un ejecutable único?
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

### ¿Cuál es el tamaño del ejecutable?
- Sin self-contained: ~200 KB
- Con self-contained: ~60-80 MB

---

## 🎓 APRENDIZAJE

### ¿Qué conceptos aprendo con este proyecto?
- ✅ Servicios web (REST + SOAP)
- ✅ Arquitectura SOA
- ✅ Patrones de diseño
- ✅ Programación asíncrona
- ✅ Serialización (JSON/XML)
- ✅ Interfaces y abstracción
- ✅ Clean Code

### ¿Dónde aprendo más sobre REST?
- https://restfulapi.net/
- https://www.restapitutorial.com/
- Documentación oficial de ASP.NET Core

### ¿Dónde aprendo más sobre SOAP?
- https://www.w3.org/TR/soap/
- https://www.tutorialspoint.com/soap/
- Documentación de WCF

---

## 📞 SOPORTE

### ¿Dónde encuentro más información?
- **README.md** - Documentación completa
- **INICIO_RAPIDO.md** - Guía de 5 minutos
- **DATOS_PRUEBA.md** - Usuarios y cuentas
- **CAPTURAS.md** - Screenshots de la UI
- **DIAGRAMAS.md** - Arquitectura visual

### ¿Cómo reporto un bug?
Este es un proyecto educativo. Puedes:
1. Revisar el código fuente
2. Modificarlo según necesites
3. Aprender del error

### ¿Puedo contribuir mejoras?
¡Sí! Es un proyecto educativo abierto. Mejoras sugeridas:
- Agregar logs
- Implementar caché
- Mejorar manejo de errores
- Agregar pruebas unitarias
- Crear interfaz gráfica

---

## 🎯 CASOS DE USO

### ¿Para qué sirve este proyecto?
- 🎓 Aprendizaje de arquitectura SOA
- 🧪 Testing de servidores bancarios
- 📊 Demostración de tecnología
- 🔧 Prototipo rápido
- 📚 Proyecto académico

### ¿Puedo usarlo en mi tesis?
Sí, es perfecto para demostrar:
- Consumo de servicios web
- Arquitectura multicapa
- Integración de sistemas
- Patrones de diseño

---

**Última actualización:** 2025  
**Versión:** 1.0.0  
**Proyecto:** Cliente Consola Eurekabank
