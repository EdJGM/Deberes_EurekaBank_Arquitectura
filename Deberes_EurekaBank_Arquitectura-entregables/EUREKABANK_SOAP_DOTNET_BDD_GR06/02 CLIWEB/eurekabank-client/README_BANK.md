# EurekaBank - Sistema Bancario Web Universal

Sistema web bancario desarrollado con Next.js que consume **4 tipos de servidores diferentes**: SOAP y RESTful en Java y .NET.

## 🚀 Características

- 🔀 **Selección flexible de servidor** - Elija entre 4 servidores diferentes al inicio
- ☁️ **Soporte SOAP** - Consume servicios SOAP Java (JAX-WS) y .NET (WCF)
- 🌐 **Soporte RESTful** - Consume APIs REST Java (JAX-RS) y .NET Core
- ✅ **Autenticación de usuarios** - Login seguro con validación
- 💰 **Gestión de depósitos** - Realizar depósitos en cuentas
- 💸 **Gestión de retiros** - Realizar retiros con validación de saldo
- 🔄 **Transferencias bancarias** - Transferir fondos entre cuentas
- 📊 **Consulta de movimientos** - Ver historial de transacciones
- 🎨 **Interfaz moderna** - UI responsive con Tailwind CSS
- 🔌 **Multi-protocolo** - Una interfaz para todos los servidores

## 📋 Requisitos Previos

- Node.js 20.x o superior
- NPM o Yarn
- **Al menos uno** de los siguientes servidores ejecutándose:

### Servidores SOAP
  - **☕ Java SOAP Server**: `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl`
  - **🔷 .NET SOAP Server**: `http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl`

### Servidores RESTful
  - **☕ Java RESTful Server**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka`
  - **🔷 .NET RESTful Server**: `http://localhost:5000/api`

## 🔧 Instalación

1. **Clonar el repositorio** (si aplica) o navegar al directorio del proyecto:
   ```bash
   cd eurekabank-client
   ```

2. **Instalar dependencias**:
   ```bash
   npm install
   ```

3. **Verificar configuración de los servicios**:
   - Asegúrate de que al menos un servidor esté ejecutándose
   - Las URLs están configuradas en: `src/lib/servers.ts`
   
   **SOAP:**
   - **Java**: `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl`
   - **.NET**: `http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl`
   
   **RESTful:**
   - **Java**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka`
   - **.NET**: `http://localhost:5000/api`

## 🏃 Ejecutar la Aplicación

### Modo Desarrollo
```bash
npm run dev
```
La aplicación estará disponible en: `http://localhost:3000`

### Modo Producción
```bash
npm run build
npm start
```

## 🎯 Uso de la Aplicación

### 1. Selección de Servidor
- Al iniciar la aplicación, se mostrará una pantalla de selección
- Elija entre **4 opciones**:
  
  **Servidores SOAP (XML):**
  - **☕ Java SOAP Server** - JAX-WS (Jakarta Web Services)
  - **🔷 .NET SOAP Server** - WCF (Windows Communication Foundation)
  
  **Servidores RESTful (JSON):**
  - **☕ Java RESTful Server** - JAX-RS (Jakarta RESTful Web Services)
  - **🔷 .NET RESTful Server** - ASP.NET Core Web API

- Todos los servidores ofrecen las mismas funcionalidades
- Puede cambiar de servidor en cualquier momento cerrando sesión

### 2. Login
- Ingrese sus credenciales de usuario
- El sistema validará contra el servicio SOAP seleccionado
- Usuarios de prueba (según configuración del servidor):
  - Usuario: `admin`
  - Contraseña: `pass123` (ejemplo)

### 3. Consultar Movimientos
1. Seleccione la pestaña "Movimientos"
2. Ingrese el número de cuenta (ej: `123456`)
3. Haga clic en "Buscar"
4. Visualice el historial de transacciones con:
   - Número de movimiento
   - Fecha y hora
   - Tipo de transacción
   - Acción (Crédito/Débito)
   - Importe

### 4. Realizar Depósito
1. Seleccione la pestaña "Depósito"
2. Ingrese:
   - Número de cuenta
   - Importe a depositar
3. Haga clic en "Realizar Depósito"
4. Recibirá confirmación del éxito de la operación

### 5. Realizar Retiro
1. Seleccione la pestaña "Retiro"
2. Ingrese:
   - Número de cuenta
   - Importe a retirar
3. Haga clic en "Realizar Retiro"
4. El sistema validará saldo suficiente

### 6. Realizar Transferencia
1. Seleccione la pestaña "Transferencia"
2. Ingrese:
   - Cuenta origen
   - Cuenta destino
   - Importe a transferir
3. Haga clic en "Realizar Transferencia"
4. La operación se ejecuta de forma atómica (todo o nada)

## 📁 Estructura del Proyecto

```
eurekabank-client/
├── src/
│   ├── app/
│   │   ├── api/
│   │   │   └── soap/
│   │   │       └── route.ts          # API Route para SOAP (Java y .NET)
│   │   ├── globals.css               # Estilos globales
│   │   ├── layout.tsx                # Layout principal
│   │   └── page.tsx                  # Página principal
│   ├── components/
│   │   ├── ui/                       # Componentes UI reutilizables
│   │   │   ├── button.tsx
│   │   │   ├── card.tsx
│   │   │   ├── input.tsx
│   │   │   ├── label.tsx
│   │   │   └── tabs.tsx
│   │   ├── BankDashboard.tsx         # Dashboard principal
│   │   ├── DepositForm.tsx           # Formulario de depósitos
│   │   ├── LoginForm.tsx             # Formulario de login
│   │   ├── MovementsView.tsx         # Vista de movimientos
│   │   ├── ServerSelection.tsx       # Selección de servidor
│   │   ├── TransferForm.tsx          # Formulario de transferencias
│   │   └── WithdrawForm.tsx          # Formulario de retiros
│   └── lib/
│       ├── api.ts                    # Cliente API para SOAP
│       ├── servers.ts                # Configuración de servidores
│       └── utils.ts                  # Utilidades
├── package.json
└── README.md
```

## 🔌 Operaciones Disponibles por Servidor

La aplicación consume diferentes APIs según el servidor seleccionado:

### 📡 SOAP Java (JAX-WS)
1. **health** - Verifica estado del servicio
2. **login(username, password)** - Autenticación
3. **traerMovimientos(cuenta)** - Consulta movimientos
4. **regDeposito(cuenta, importe)** - Registra depósito
5. **regRetiro(cuenta, importe)** - Registra retiro
6. **regTransferencia(cuentaOrigen, cuentaDestino, importe)** - Transferencia

### 📡 SOAP .NET (WCF)
1. **Health** - Verifica estado del servicio
2. **Login(username, password)** - Autenticación
3. **ObtenerPorCuenta(cuenta)** - Consulta movimientos
4. **RegistrarDeposito(cuenta, importe)** - Registra depósito
5. **RegistrarRetiro(cuenta, importe)** - Registra retiro
6. **RegistrarTransferencia(cuentaOrigen, cuentaDestino, importe)** - Transferencia

### 🌐 RESTful Java (JAX-RS)
- `GET /health` - Health check
- `POST /login` - Autenticación
- `GET /movimientos/{cuenta}` - Consulta movimientos
- `POST /deposito?cuenta={}&importe={}` - Registra depósito
- `POST /retiro?cuenta={}&importe={}` - Registra retiro
- `POST /transferencia?cuentaOrigen={}&cuentaDestino={}&importe={}` - Transferencia

### 🌐 RESTful .NET (ASP.NET Core)
- `GET /Health` - Health check
- `POST /Auth/login` - Autenticación
- `GET /Movimientos/cuenta/{cuenta}` - Consulta movimientos
- `POST /Movimientos/deposito` - Registra depósito (JSON body)
- `POST /Movimientos/retiro` - Registra retiro (JSON body)
- `POST /Movimientos/transferencia` - Transferencia (JSON body)

## 🛠️ Tecnologías Utilizadas

### Frontend
- **Next.js 16** - Framework React con App Router
- **React 19** - Biblioteca de UI
- **TypeScript** - Tipado estático
- **Tailwind CSS** - Framework CSS utility-first
- **Radix UI** - Componentes accesibles
- **Lucide React** - Iconos modernos

### Clientes de Protocolo
- **Soap (node-soap)** - Cliente SOAP para Node.js
- **Fetch API** - Cliente HTTP nativo para RESTful

## ⚠️ Solución de Problemas

### Error de conexión
- Verifique que el servidor seleccionado esté ejecutándose
- Confirme las URLs en `src/lib/servers.ts`
- Para SOAP: Verifique el WSDL en el navegador
- Para REST: Pruebe el endpoint `/health` directamente
- Revise los logs del servidor en la consola
- Intente cambiar a otro servidor disponible

### Error de CORS
- El proxy API Route de Next.js soluciona problemas de CORS
- Si persiste, verifique configuración del servidor SOAP

### Credenciales inválidas
- Verifique que el usuario esté registrado en la base de datos
- Confirme que el estado del usuario sea 'ACTIVO'

## 📝 Notas Importantes

- **Multi-protocolo**: Soporta SOAP (XML) y REST (JSON) sin cambios en la interfaz
- **Multi-servidor**: 4 servidores diferentes con una sola interfaz unificada
- **Adaptación automática**: El cliente se adapta automáticamente al protocolo seleccionado
- Las transacciones de transferencia son atómicas (commit/rollback) en todos los servidores
- Los retiros validan saldo suficiente antes de ejecutarse
- Los movimientos se muestran ordenados por número descendente
- Puede cambiar de servidor cerrando sesión y seleccionando otro en la pantalla inicial

### Diferencias entre Servidores

**SOAP vs RESTful:**
- SOAP usa XML, RESTful usa JSON
- SOAP requiere cliente SOAP (node-soap), RESTful usa Fetch API nativo
- SOAP tiene WSDL para autodescubrimiento, RESTful sigue convenciones REST

**Java vs .NET:**
- Nomenclatura de operaciones diferente (camelCase vs PascalCase)
- RESTful Java usa query parameters, .NET usa request body
- RESTful .NET incluye wrapper de respuesta estándar con `success`, `message`, `data`

## 🔐 Seguridad

- Las contraseñas se hashean con SHA en el backend
- No se almacenan credenciales en el frontend
- Considere implementar JWT o sesiones para autenticación persistente
- En producción, use HTTPS para todas las comunicaciones

## 🤝 Contribución

Para contribuir al proyecto:
1. Fork del repositorio
2. Cree una rama para su feature
3. Commit de cambios
4. Push a la rama
5. Abra un Pull Request

## 📄 Licencia

Este proyecto es parte de un trabajo académico de Arquitectura de Software.

---

**Desarrollado para EurekaBank** 🏦
