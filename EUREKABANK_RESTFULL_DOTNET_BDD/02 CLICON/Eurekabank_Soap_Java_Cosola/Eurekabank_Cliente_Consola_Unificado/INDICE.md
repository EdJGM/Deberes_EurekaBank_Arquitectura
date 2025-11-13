# 📑 ÍNDICE DE ARCHIVOS - Cliente Consola Eurekabank

Guía rápida de todos los archivos incluidos en el proyecto.

---

## 🚀 ARCHIVOS DE EJECUCIÓN

### ejecutar.bat
- **Tipo:** Script de Windows
- **Uso:** Doble clic para compilar y ejecutar
- **Comandos:** dotnet restore + build + run

### ejecutar.sh
- **Tipo:** Script de Linux/Mac
- **Uso:** `./ejecutar.sh` en terminal
- **Permisos:** Ejecutable (chmod +x)

---

## 💻 CÓDIGO FUENTE (C#)

### Program.cs (512 líneas)
- **Propósito:** Programa principal
- **Contenido:**
  - Menú de bienvenida
  - Selección de servidor
  - Sistema de login
  - Menú de operaciones
  - UI en consola

### Models.cs (77 líneas)
- **Propósito:** Modelos de datos
- **Contenido:**
  - Clase Movimiento
  - Clase OperacionResult
  - DTOs para REST
  - Enum TipoServidor

### IEurekabankService.cs (42 líneas)
- **Propósito:** Interfaz común
- **Contenido:**
  - Contrato para todos los servicios
  - 6 métodos principales
  - Async/await signatures

### RestServices.cs (325 líneas)
- **Propósito:** Clientes REST
- **Contenido:**
  - RestDotNetService (165 líneas)
  - RestJavaService (160 líneas)
  - Implementa IEurekabankService

### SoapServices.cs (428 líneas)
- **Propósito:** Clientes SOAP
- **Contenido:**
  - SoapDotNetService (214 líneas)
  - SoapJavaService (214 líneas)
  - Construcción de mensajes XML

---

## ⚙️ ARCHIVOS DE CONFIGURACIÓN

### Eurekabank_Cliente_Consola_Unificado.csproj
- **Tipo:** Proyecto .NET
- **Framework:** .NET 6.0
- **Paquetes:**
  - Newtonsoft.Json 13.0.3
  - System.ServiceModel.Http 6.0.0
  - System.ServiceModel.Primitives 6.0.0

### appsettings.json (29 líneas)
- **Tipo:** Configuración JSON
- **Contenido:**
  - URLs de servidores
  - Timeouts
  - Configuración de app

---

## 📖 DOCUMENTACIÓN

### README.md (247 líneas)
- **Propósito:** Manual completo
- **Secciones:**
  - Características
  - Requisitos
  - Instalación
  - Uso detallado
  - Estructura
  - Configuración
  - Usuarios de prueba
  - Solución de problemas
  - Notas técnicas

### INICIO_RAPIDO.md (151 líneas)
- **Propósito:** Guía de 5 minutos
- **Secciones:**
  - Checklist rápido
  - Pasos de ejecución
  - Primer uso
  - Problemas comunes
  - Tips rápidos

### RESUMEN.md (267 líneas)
- **Propósito:** Resumen ejecutivo
- **Secciones:**
  - Entregable completo
  - Estructura
  - Servidores soportados
  - Características principales
  - Tecnologías
  - Patrones de diseño
  - Flujo de trabajo

### DATOS_PRUEBA.md (163 líneas)
- **Propósito:** Datos para testing
- **Secciones:**
  - Usuarios disponibles
  - Sucursales
  - Tipos de cuenta
  - Monedas
  - Tipos de movimiento
  - Ejemplos de cuentas
  - Escenarios de prueba
  - Configuración de servidores

### CAPTURAS.md (291 líneas)
- **Propósito:** Visualización de UI
- **Secciones:**
  - 11 capturas de pantalla (ASCII art)
  - Código de colores
  - Características de UI
  - Experiencia de usuario
  - Tips de usabilidad

### COMPILACION.md (33 líneas)
- **Propósito:** Guía de compilación
- **Secciones:**
  - Visual Studio 2022
  - Visual Studio Code
  - Línea de comandos
  - Notas importantes

### INDICE.md (este archivo)
- **Propósito:** Índice de todos los archivos
- **Contenido:** Lo que estás leyendo ahora

### NAVEGACION.md
- **Propósito:** Guía maestra de navegación
- **Secciones:**
  - Orden recomendado de lectura
  - Guías por perfil (desarrollador, usuario, profesor)
  - Búsqueda rápida
  - Estructura de archivos
  - Consejos de navegación

### DIAGRAMAS.md
- **Propósito:** Arquitectura visual del sistema
- **Secciones:**
  - Arquitectura general en ASCII art
  - Flujo de datos completo
  - Diagrama de clases
  - Diagrama de secuencia
  - Componentes y responsabilidades
  - Patrones de diseño aplicados
  - Stack tecnológico

### FAQ.md
- **Propósito:** Preguntas y respuestas frecuentes
- **Secciones:**
  - Instalación y ejecución (6 preguntas)
  - Conexión a servidores (5 preguntas)
  - Autenticación (5 preguntas)
  - Operaciones bancarias (6 preguntas)
  - Errores comunes (6 preguntas)
  - Personalización (4 preguntas)
  - Datos y BD (3 preguntas)
  - Seguridad (3 preguntas)
  - Testing (3 preguntas)
  - Distribución (3 preguntas)
  - Aprendizaje (2 preguntas)
  - Soporte (3 preguntas)

### TUTORIAL.md
- **Propósito:** Tutorial completo paso a paso
- **Secciones:**
  - Preparación (5 min)
  - Primera ejecución (3 min)
  - Autenticación (2 min)
  - Menú principal (2 min)
  - Consultar movimientos (3 min)
  - Realizar depósito (3 min)
  - Realizar retiro (3 min)
  - Realizar transferencia (3 min)
  - Cerrar sesión (1 min)
  - Resumen y próximos pasos

---

## 📊 ESTADÍSTICAS DEL PROYECTO (ACTUALIZADO)

```
Total de archivos:     20
Archivos de código:    5 archivos C# + 1 .csproj
Archivos de script:    2 (.bat + .sh)
Archivos de config:    2 (.json + .csproj)
Archivos de docs:      11 archivos .md

Líneas de código C#:   1,384 líneas
Líneas de docs:        ~5,500 líneas
Total general:         ~7,000 líneas

Páginas de docs:       ~80 páginas equivalentes
Tiempo lectura:        2-3 horas (completa)
```

---

## 🎯 ¿QUÉ ARCHIVO LEER PRIMERO?

### Si eres desarrollador:
1. **INICIO_RAPIDO.md** - Para empezar en 5 minutos
2. **Program.cs** - Ver el código principal
3. **README.md** - Documentación completa
4. **RESUMEN.md** - Entender la arquitectura

### Si eres usuario:
1. **INICIO_RAPIDO.md** - Guía express
2. **DATOS_PRUEBA.md** - Usuarios y cuentas
3. **CAPTURAS.md** - Ver cómo se ve
4. **README.md** - Si tienes problemas

### Si eres profesor/evaluador:
1. **RESUMEN.md** - Visión general completa
2. **Program.cs** - Revisar implementación
3. **README.md** - Documentación técnica
4. **DATOS_PRUEBA.md** - Casos de prueba

---

## 🔍 BUSCAR INFORMACIÓN ESPECÍFICA

| Necesitas | Lee este archivo |
|-----------|------------------|
| Compilar el proyecto | COMPILACION.md |
| Ejecutar rápidamente | INICIO_RAPIDO.md |
| Usuarios de prueba | DATOS_PRUEBA.md |
| Ver screenshots | CAPTURAS.md |
| Entender arquitectura | RESUMEN.md |
| Documentación completa | README.md |
| Cambiar URLs | appsettings.json |
| Ver código REST | RestServices.cs |
| Ver código SOAP | SoapServices.cs |
| Interfaz común | IEurekabankService.cs |
| Modelos de datos | Models.cs |
| Lógica principal | Program.cs |

---

## 📦 ARCHIVOS EN EL ZIP

```
Eurekabank_Cliente_Consola_Unificado.zip (24 KB)
│
└── Eurekabank_Cliente_Consola_Unificado/
    │
    ├── 📄 Program.cs
    ├── 📄 Models.cs
    ├── 📄 IEurekabankService.cs
    ├── 📄 RestServices.cs
    ├── 📄 SoapServices.cs
    │
    ├── ⚙️ Eurekabank_Cliente_Consola_Unificado.csproj
    ├── ⚙️ appsettings.json
    │
    ├── 🚀 ejecutar.bat
    ├── 🚀 ejecutar.sh
    │
    ├── 📖 README.md
    ├── 📖 INICIO_RAPIDO.md
    ├── 📖 RESUMEN.md
    ├── 📖 DATOS_PRUEBA.md
    ├── 📖 CAPTURAS.md
    ├── 📖 COMPILACION.md
    └── 📖 INDICE.md (este archivo)
```

---

## 💡 CONSEJOS

1. **Empieza por INICIO_RAPIDO.md** - Es la forma más rápida de ver el proyecto funcionando

2. **Usa ejecutar.bat o ejecutar.sh** - No necesitas abrir Visual Studio

3. **Lee DATOS_PRUEBA.md** - Tiene todos los usuarios y contraseñas

4. **Consulta CAPTURAS.md** - Para ver cómo se ve antes de ejecutar

5. **README.md es tu amigo** - Tiene respuestas a casi todo

---

## 🎓 PROPÓSITO EDUCATIVO

Este proyecto demuestra:
- ✅ Arquitectura de servicios web (SOA)
- ✅ Consumo de REST y SOAP
- ✅ Patrones de diseño (Strategy, Adapter, DTO)
- ✅ Programación asíncrona (async/await)
- ✅ Interfaces y abstracción
- ✅ UI en consola profesional
- ✅ Documentación exhaustiva

---

**Proyecto:** Cliente Consola Unificado Eurekabank  
**Versión:** 1.0.0  
**Archivos:** 15  
**Líneas:** 2,565  
**Tamaño:** 24 KB (comprimido)
