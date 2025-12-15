# Documentación Técnica - Cliente Universal EurekaBank

## Arquitectura del Sistema

### Descripción General

El cliente web de EurekaBank es una aplicación **agnóstica de protocolo** que puede consumir servicios bancarios desde 4 tipos diferentes de servidores:

1. **SOAP Java** (JAX-WS)
2. **SOAP .NET** (WCF)
3. **RESTful Java** (JAX-RS)
4. **RESTful .NET** (ASP.NET Core)

### Flujo de Funcionamiento

```
Usuario → Selección de Servidor → Login → Dashboard → Operaciones Bancarias
                ↓
    [soap-java, soap-dotnet, rest-java, rest-dotnet]
                ↓
    API Route correspondiente (/api/soap o /api/rest)
                ↓
    Cliente SOAP (node-soap) o Cliente HTTP (Fetch)
                ↓
    Servidor Backend seleccionado
```

## Estructura de Código

### 1. Configuración de Servidores (`src/lib/servers.ts`)

Define los 4 servidores disponibles con su configuración:

```typescript
export type ServerType = 'soap-java' | 'soap-dotnet' | 'rest-java' | 'rest-dotnet';
export type Protocol = 'soap' | 'rest';

export const SERVERS: Record<ServerType, ServerConfig> = {
  'soap-java': { ... },
  'soap-dotnet': { ... },
  'rest-java': { ... },
  'rest-dotnet': { ... }
};
```

### 2. API Routes

#### `/api/soap/route.ts` - Handler para SOAP

Maneja las peticiones SOAP usando el cliente `node-soap`:

```typescript
// Detecta el tipo de servidor SOAP
if (serverType === 'soap-java') {
  // Usa nomenclatura Java: health, login, traerMovimientos, etc.
} else if (serverType === 'soap-dotnet') {
  // Usa nomenclatura .NET: Health, Login, ObtenerPorCuenta, etc.
}
```

#### `/api/rest/route.ts` - Handler para RESTful

Maneja las peticiones REST usando Fetch API nativo:

```typescript
// Detecta el tipo de servidor REST
if (serverType === 'rest-java') {
  // Usa query parameters
  fetch(`${baseUrl}/deposito?cuenta=${cuenta}&importe=${importe}`)
} else if (serverType === 'rest-dotnet') {
  // Usa request body JSON
  fetch(`${baseUrl}/Movimientos/deposito`, {
    body: JSON.stringify({ cuenta, importe })
  })
}
```

### 3. Cliente API (`src/lib/api.ts`)

Proporciona una interfaz unificada para todas las operaciones:

```typescript
export const createApiClient = (serverType: ServerType) => ({
  health: () => callApiOperation('health', {}, serverType),
  login: (username, password) => callApiOperation('login', { username, password }, serverType),
  traerMovimientos: (cuenta) => callApiOperation('traerMovimientos', { cuenta }, serverType),
  regDeposito: (cuenta, importe) => callApiOperation('regDeposito', { cuenta, importe }, serverType),
  regRetiro: (cuenta, importe) => callApiOperation('regRetiro', { cuenta, importe }, serverType),
  regTransferencia: (origen, destino, importe) => callApiOperation('regTransferencia', { cuentaOrigen: origen, cuentaDestino: destino, importe }, serverType)
});
```

La función `callApiOperation` automáticamente:
1. Detecta si es SOAP o REST según el `serverType`
2. Enruta a `/api/soap` o `/api/rest`
3. Retorna una respuesta normalizada

## Mapeo de Operaciones

### Health Check

| Servidor | Operación | Respuesta |
|----------|-----------|-----------|
| SOAP Java | `health()` | `<status>Servicio activo...</status>` |
| SOAP .NET | `Health()` | `<HealthResult>Servicio activo...</HealthResult>` |
| REST Java | `GET /health` | `"Servicio activo..."` (string) |
| REST .NET | `GET /Health` | `{ success: true, data: {...} }` (JSON) |

### Login

| Servidor | Operación | Request | Respuesta |
|----------|-----------|---------|-----------|
| SOAP Java | `login(username, password)` | XML | `<return>true/false</return>` |
| SOAP .NET | `Login(username, password)` | XML | `<LoginResult>true/false</LoginResult>` |
| REST Java | `POST /login` | JSON body | `true/false` |
| REST .NET | `POST /Auth/login` | JSON body | `{ success: true, data: { authenticated: true } }` |

### Movimientos

| Servidor | Operación | Request | Respuesta |
|----------|-----------|---------|-----------|
| SOAP Java | `traerMovimientos(cuenta)` | XML | Array de `<movimiento>` |
| SOAP .NET | `ObtenerPorCuenta(cuenta)` | XML | Array de `<a:movimiento>` |
| REST Java | `GET /movimientos/{cuenta}` | URL param | JSON Array |
| REST .NET | `GET /Movimientos/cuenta/{cuenta}` | URL param | `{ data: [...] }` |

### Depósito

| Servidor | Operación | Request |
|----------|-----------|---------|
| SOAP Java | `regDeposito(cuenta, importe)` | XML params |
| SOAP .NET | `RegistrarDeposito(cuenta, importe)` | XML params |
| REST Java | `POST /deposito?cuenta={}&importe={}` | Query params |
| REST .NET | `POST /Movimientos/deposito` | JSON body |

### Retiro

| Servidor | Operación | Request |
|----------|-----------|---------|
| SOAP Java | `regRetiro(cuenta, importe)` | XML params |
| SOAP .NET | `RegistrarRetiro(cuenta, importe)` | XML params |
| REST Java | `POST /retiro?cuenta={}&importe={}` | Query params |
| REST .NET | `POST /Movimientos/retiro` | JSON body |

### Transferencia

| Servidor | Operación | Request |
|----------|-----------|---------|
| SOAP Java | `regTransferencia(origen, destino, importe)` | XML params |
| SOAP .NET | `RegistrarTransferencia(origen, destino, importe)` | XML params |
| REST Java | `POST /transferencia?cuentaOrigen={}&cuentaDestino={}&importe={}` | Query params |
| REST .NET | `POST /Movimientos/transferencia` | JSON body |

## Normalización de Respuestas

El cliente normaliza todas las respuestas a un formato común:

```typescript
interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
}
```

### Transformaciones Aplicadas

#### Estado de Operaciones
- **SOAP Java**: `<estado>1</estado>` → `{ estado: 1 }`
- **SOAP .NET**: `<Result>1</Result>` → `{ estado: 1 }`
- **REST Java**: `"Éxito"` → `{ estado: 1 }`
- **REST .NET**: `{ success: true }` → `{ estado: 1 }`

#### Movimientos
Todos los formatos se normalizan a:
```typescript
{
  cuenta: string,
  nromov: number,
  fecha: string,
  tipo: string,
  accion: string,
  importe: number
}
```

## Componentes React

### 1. ServerSelection
Pantalla inicial que muestra los 4 servidores disponibles con:
- Iconos distintivos (Cloud para SOAP, Code para REST)
- Badges de protocolo (SOAP/REST)
- Indicador de tecnología (Java/.NET)
- URL del servidor

### 2. LoginForm
Formulario de autenticación que:
- Muestra el servidor seleccionado
- Permite regresar a la selección
- Valida credenciales contra el servidor elegido

### 3. BankDashboard
Panel principal que:
- Muestra el servidor activo en el header
- Contiene 4 pestañas: Movimientos, Depósito, Retiro, Transferencia
- Pasa el `serverType` a todos los componentes hijos

### 4. Formularios (Deposit/Withdraw/Transfer)
Cada formulario:
- Recibe `serverType` como prop
- Crea cliente API específico: `createApiClient(serverType)`
- Maneja respuestas normalizadas

### 5. MovementsView
Vista de movimientos que:
- Consulta movimientos según el servidor
- Formatea fechas y moneda
- Muestra tabla responsiva con resultados

## Manejo de Errores

### Niveles de Error

1. **Error de Conexión**
   ```typescript
   catch (error) {
     return { error: 'Error al conectar con el servicio' }
   }
   ```

2. **Error de Protocolo**
   ```typescript
   if (!serverConfig || serverConfig.protocol !== 'soap') {
     return { error: 'Tipo de servidor no válido' }
   }
   ```

3. **Error de Operación**
   ```typescript
   if (result.data?.estado === -1) {
     setMessage({ type: 'error', text: 'Error en la operación' })
   }
   ```

## Pruebas

### Escenarios de Prueba

1. **Conexión a cada servidor**
   - Health check exitoso
   - WSDL/API accesible

2. **Autenticación**
   - Login exitoso
   - Login fallido
   - Validación de campos

3. **Operaciones bancarias**
   - Depósito exitoso
   - Retiro con saldo suficiente
   - Retiro con saldo insuficiente
   - Transferencia entre cuentas
   - Consulta de movimientos

4. **Cambio de servidor**
   - Logout y selección de nuevo servidor
   - Operaciones con diferentes servidores

### Comandos de Prueba

```bash
# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev

# Construir para producción
npm run build

# Ejecutar producción
npm start
```

## Consideraciones de Seguridad

1. **CORS**: Configurado en el servidor
2. **Autenticación**: Por petición (sin tokens persistentes)
3. **Validación**: En cliente y servidor
4. **HTTPS**: Recomendado en producción
5. **Variables de entorno**: URLs en `servers.ts` (considerar .env)

## Escalabilidad

### Agregar Nuevo Servidor

1. Definir en `servers.ts`:
```typescript
'soap-python': {
  type: 'soap-python',
  protocol: 'soap',
  name: 'Python SOAP Server',
  url: 'http://localhost:9000/soap?wsdl',
  description: 'Servidor SOAP en Python'
}
```

2. Agregar lógica en `/api/soap/route.ts` o `/api/rest/route.ts`

3. Mapear operaciones según nomenclatura del nuevo servidor

No se requieren cambios en componentes React - la arquitectura es completamente extensible.

## Performance

### Optimizaciones Aplicadas

1. **Next.js App Router**: Server Components por defecto
2. **API Routes**: Servidor como proxy (evita CORS)
3. **Fetch con Cache**: Para health checks
4. **React Suspense**: Para estados de carga
5. **Tailwind CSS**: CSS optimizado en build

### Métricas Objetivo

- **Time to Interactive**: < 3s
- **First Contentful Paint**: < 1.5s
- **API Response Time**: < 500ms (depende del backend)

## Troubleshooting

### Problemas Comunes

1. **"Error al conectar con el servicio SOAP"**
   - Verificar que el servidor SOAP esté ejecutándose
   - Comprobar URL del WSDL en navegador
   - Revisar CORS en servidor

2. **"Error al conectar con el servicio REST"**
   - Verificar que la API REST esté ejecutándose
   - Probar endpoint con curl o Postman
   - Revisar puerto correcto

3. **"Operación no válida"**
   - Verificar que el `serverType` sea correcto
   - Comprobar mapeo de operaciones

4. **Respuestas vacías**
   - Revisar normalización de respuestas
   - Verificar formato de datos del servidor

## Conclusión

Este cliente universal demuestra:
- **Arquitectura flexible**: Soporta múltiples protocolos
- **Código reutilizable**: Una interfaz para 4 servidores
- **Separación de responsabilidades**: API routes como capa de abstracción
- **Mantenibilidad**: Fácil agregar nuevos servidores
- **User Experience**: Interfaz consistente independiente del backend

---

**Desarrollado para EurekaBank** - Sistema Bancario Multi-Protocolo 🏦
