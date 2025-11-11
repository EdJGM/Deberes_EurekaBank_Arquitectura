# Documentación de API - EurekaBank

Esta documentación describe cómo realizar peticiones y las respuestas esperadas para cada uno de los servidores de EurekaBank.

## Tabla de Contenido

1. [SOAP Java](#soap-java)
2. [SOAP .NET](#soap-dotnet)
3. [RESTful Java](#restful-java)
4. [RESTful .NET](#restful-dotnet)

---

## SOAP Java

### Información General

- **Servidor**: EurekabankWS
- **Tecnología**: Java Web Services (JAX-WS)
- **Formato**: XML/SOAP
- **URL Base**: `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS`
- **WSDL**: `http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl`

### Endpoints

#### 1. Health Check

**Operación**: `health`

**Descripción**: Verifica el estado del servicio.

**Request (SOAP Envelope)**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:health/>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:healthResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <status>Servicio Eurekabank SOAP activo y funcionando correctamente</status>
      </ns2:healthResponse>
   </soap:Body>
</soap:Envelope>
```

#### 2. Login

**Operación**: `login`

**Descripción**: Autentica un usuario en el sistema.

**Request**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:login>
         <username>admin</username>
         <password>admin123</password>
      </ws:login>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response (Exitoso)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:loginResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <return>true</return>
      </ns2:loginResponse>
   </soap:Body>
</soap:Envelope>
```

**Response (Fallido)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:loginResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <return>false</return>
      </ns2:loginResponse>
   </soap:Body>
</soap:Envelope>
```

#### 3. Traer Movimientos

**Operación**: `traerMovimientos`

**Descripción**: Obtiene la lista de movimientos de una cuenta.

**Request**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:traerMovimientos>
         <cuenta>00100001</cuenta>
      </ws:traerMovimientos>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:traerMovimientosResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <movimiento>
            <cuenta>00100001</cuenta>
            <nromov>1</nromov>
            <fecha>2024-01-15T10:30:00</fecha>
            <tipo>001</tipo>
            <accion>Depósito en efectivo</accion>
            <importe>1000.50</importe>
         </movimiento>
         <movimiento>
            <cuenta>00100001</cuenta>
            <nromov>2</nromov>
            <fecha>2024-01-16T14:20:00</fecha>
            <tipo>002</tipo>
            <accion>Retiro en cajero</accion>
            <importe>500.00</importe>
         </movimiento>
      </ns2:traerMovimientosResponse>
   </soap:Body>
</soap:Envelope>
```

#### 4. Registrar Depósito

**Operación**: `regDeposito`

**Descripción**: Registra un depósito en una cuenta.

**Request**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:regDeposito>
         <cuenta>00100001</cuenta>
         <importe>1500.75</importe>
      </ws:regDeposito>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response (Exitoso)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regDepositoResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>1</estado>
      </ns2:regDepositoResponse>
   </soap:Body>
</soap:Envelope>
```

**Response (Error)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regDepositoResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>-1</estado>
      </ns2:regDepositoResponse>
   </soap:Body>
</soap:Envelope>
```

#### 5. Registrar Retiro

**Operación**: `regRetiro`

**Descripción**: Registra un retiro de una cuenta.

**Request**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:regRetiro>
         <cuenta>00100001</cuenta>
         <importe>300.00</importe>
      </ws:regRetiro>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response (Exitoso)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regRetiroResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>1</estado>
      </ns2:regRetiroResponse>
   </soap:Body>
</soap:Envelope>
```

**Response (Error)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regRetiroResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>-1</estado>
      </ns2:regRetiroResponse>
   </soap:Body>
</soap:Envelope>
```

#### 6. Registrar Transferencia

**Operación**: `regTransferencia`

**Descripción**: Registra una transferencia entre dos cuentas.

**Request**:
```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:regTransferencia>
         <cuentaOrigen>00100001</cuentaOrigen>
         <cuentaDestino>00100002</cuentaDestino>
         <importe>750.00</importe>
      </ws:regTransferencia>
   </soapenv:Body>
</soapenv:Envelope>
```

**Response (Exitoso)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regTransferenciaResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>1</estado>
      </ns2:regTransferenciaResponse>
   </soap:Body>
</soap:Envelope>
```

**Response (Error)**:
```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <ns2:regTransferenciaResponse xmlns:ns2="http://ws.monster.edu.ec/">
         <estado>-1</estado>
      </ns2:regTransferenciaResponse>
   </soap:Body>
</soap:Envelope>
```

---

## SOAP .NET

### Información General

- **Servidor**: EurekabankWS
- **Tecnología**: WCF (Windows Communication Foundation)
- **Formato**: XML/SOAP
- **URL Base**: `http://localhost:port/ec.edu.monster.ws/EurekabankWS.svc`
- **WSDL**: `http://localhost:port/ec.edu.monster.ws/EurekabankWS.svc?wsdl`

### Endpoints

#### 1. Health

**Operación**: `Health`

**Descripción**: Verifica el estado del servicio.

**Request (SOAP Envelope)**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <Health xmlns="http://tempuri.org/"/>
   </s:Body>
</s:Envelope>
```

**Response**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <HealthResponse xmlns="http://tempuri.org/">
         <HealthResult>Servicio Eurekabank SOAP activo y funcionando correctamente</HealthResult>
      </HealthResponse>
   </s:Body>
</s:Envelope>
```

#### 2. Login

**Operación**: `Login`

**Descripción**: Autentica un usuario en el sistema.

**Request**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <Login xmlns="http://tempuri.org/">
         <username>admin</username>
         <password>admin123</password>
      </Login>
   </s:Body>
</s:Envelope>
```

**Response (Exitoso)**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <LoginResponse xmlns="http://tempuri.org/">
         <LoginResult>true</LoginResult>
      </LoginResponse>
   </s:Body>
</s:Envelope>
```

**Response (Fallido)**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <LoginResponse xmlns="http://tempuri.org/">
         <LoginResult>false</LoginResult>
      </LoginResponse>
   </s:Body>
</s:Envelope>
```

#### 3. Obtener Por Cuenta

**Operación**: `ObtenerPorCuenta`

**Descripción**: Obtiene la lista de movimientos de una cuenta.

**Request**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <ObtenerPorCuenta xmlns="http://tempuri.org/">
         <cuenta>00100001</cuenta>
      </ObtenerPorCuenta>
   </s:Body>
</s:Envelope>
```

**Response**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <ObtenerPorCuentaResponse xmlns="http://tempuri.org/">
         <ObtenerPorCuentaResult xmlns:a="http://schemas.datacontract.org/2004/07/Eurekabank_Soap_Dotnet.ec.edu.monster.modelo">
            <a:movimiento>
               <a:Cuenta>00100001</a:Cuenta>
               <a:NroMov>1</a:NroMov>
               <a:Fecha>2024-01-15T10:30:00</a:Fecha>
               <a:Tipo>001</a:Tipo>
               <a:Accion>Depósito en efectivo</a:Accion>
               <a:Importe>1000.50</a:Importe>
            </a:movimiento>
            <a:movimiento>
               <a:Cuenta>00100001</a:Cuenta>
               <a:NroMov>2</a:NroMov>
               <a:Fecha>2024-01-16T14:20:00</a:Fecha>
               <a:Tipo>002</a:Tipo>
               <a:Accion>Retiro en cajero</a:Accion>
               <a:Importe>500.00</a:Importe>
            </a:movimiento>
         </ObtenerPorCuentaResult>
      </ObtenerPorCuentaResponse>
   </s:Body>
</s:Envelope>
```

#### 4. Registrar Depósito

**Operación**: `RegistrarDeposito`

**Descripción**: Registra un depósito en una cuenta.

**Request**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarDeposito xmlns="http://tempuri.org/">
         <cuenta>00100001</cuenta>
         <importe>1500.75</importe>
      </RegistrarDeposito>
   </s:Body>
</s:Envelope>
```

**Response**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarDepositoResponse xmlns="http://tempuri.org/">
         <RegistrarDepositoResult>1</RegistrarDepositoResult>
      </RegistrarDepositoResponse>
   </s:Body>
</s:Envelope>
```

#### 5. Registrar Retiro

**Operación**: `RegistrarRetiro`

**Descripción**: Registra un retiro de una cuenta.

**Request**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarRetiro xmlns="http://tempuri.org/">
         <cuenta>00100001</cuenta>
         <importe>300.00</importe>
      </RegistrarRetiro>
   </s:Body>
</s:Envelope>
```

**Response**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarRetiroResponse xmlns="http://tempuri.org/">
         <RegistrarRetiroResult>1</RegistrarRetiroResult>
      </RegistrarRetiroResponse>
   </s:Body>
</s:Envelope>
```

#### 6. Registrar Transferencia

**Operación**: `RegistrarTransferencia`

**Descripción**: Registra una transferencia entre dos cuentas.

**Request**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarTransferencia xmlns="http://tempuri.org/">
         <cuentaOrigen>00100001</cuentaOrigen>
         <cuentaDestino>00100002</cuentaDestino>
         <importe>750.00</importe>
      </RegistrarTransferencia>
   </s:Body>
</s:Envelope>
```

**Response**:
```xml
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <RegistrarTransferenciaResponse xmlns="http://tempuri.org/">
         <RegistrarTransferenciaResult>1</RegistrarTransferenciaResult>
      </RegistrarTransferenciaResponse>
   </s:Body>
</s:Envelope>
```

---

## RESTful Java

### Información General

- **Tecnología**: JAX-RS (Jakarta RESTful Web Services)
- **Formato**: JSON
- **URL Base**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka`
- **Content-Type**: `application/json`
- **Accept**: `application/json`

### Endpoints

#### 1. Health Check

**Método**: `GET`  
**Endpoint**: `/health`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/health`

**Request**:
```http
GET /api/eureka/health HTTP/1.1
Host: localhost:8080
Accept: application/json
```

**Response** (200 OK):
```json
"Servicio Eureka REST activo y funcionando correctamente"
```

#### 2. Login

**Método**: `POST`  
**Endpoint**: `/login`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/login`

**Request Headers**:
```http
POST /api/eureka/login HTTP/1.1
Host: localhost:8080
Content-Type: application/json
Accept: application/json
```

**Request Body**:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK)**:
```json
true
```

**Response (401 Unauthorized)**:
```json
false
```

#### 3. Traer Movimientos

**Método**: `GET`  
**Endpoint**: `/movimientos/{cuenta}`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/movimientos/00100001`

**Request**:
```http
GET /api/eureka/movimientos/00100001 HTTP/1.1
Host: localhost:8080
Accept: application/json
```

**Response** (200 OK):
```json
[
  {
    "cuenta": "00100001",
    "nromov": 1,
    "fecha": "2024-01-15T10:30:00.000+00:00",
    "tipo": "001",
    "accion": "Depósito en efectivo",
    "importe": 1000.50
  },
  {
    "cuenta": "00100001",
    "nromov": 2,
    "fecha": "2024-01-16T14:20:00.000+00:00",
    "tipo": "002",
    "accion": "Retiro en cajero",
    "importe": 500.00
  }
]
```

**Response** (500 Internal Server Error):
```json
"Error al obtener movimientos."
```

#### 4. Registrar Depósito

**Método**: `POST`  
**Endpoint**: `/deposito?cuenta={cuenta}&importe={importe}`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/deposito?cuenta=00100001&importe=1500.75`

**Request**:
```http
POST /api/eureka/deposito?cuenta=00100001&importe=1500.75 HTTP/1.1
Host: localhost:8080
Content-Type: application/json
Accept: application/json
```

**Response** (200 OK):
```json
"Depósito registrado con éxito."
```

**Response** (500 Internal Server Error):
```json
"Error al registrar el depósito."
```

#### 5. Registrar Retiro

**Método**: `POST`  
**Endpoint**: `/retiro?cuenta={cuenta}&importe={importe}`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/retiro?cuenta=00100001&importe=300.00`

**Request**:
```http
POST /api/eureka/retiro?cuenta=00100001&importe=300.00 HTTP/1.1
Host: localhost:8080
Content-Type: application/json
Accept: application/json
```

**Response** (200 OK):
```json
"Retiro registrado con éxito."
```

**Response** (500 Internal Server Error):
```json
"Error al registrar el retiro."
```

#### 6. Registrar Transferencia

**Método**: `POST`  
**Endpoint**: `/transferencia?cuentaOrigen={origen}&cuentaDestino={destino}&importe={importe}`  
**URL Completa**: `http://localhost:8080/Eurobank_Restfull_Java/api/eureka/transferencia?cuentaOrigen=00100001&cuentaDestino=00100002&importe=750.00`

**Request**:
```http
POST /api/eureka/transferencia?cuentaOrigen=00100001&cuentaDestino=00100002&importe=750.00 HTTP/1.1
Host: localhost:8080
Content-Type: application/json
Accept: application/json
```

**Response** (200 OK):
```json
"Transferencia registrada con éxito."
```

**Response** (500 Internal Server Error):
```json
"Error al registrar la transferencia."
```

---

## RESTful .NET

### Información General

- **Tecnología**: ASP.NET Core Web API
- **Formato**: JSON
- **URL Base**: `http://localhost:5000/api` o `https://localhost:5001/api`
- **Content-Type**: `application/json`
- **Accept**: `application/json`
- **Swagger UI**: `http://localhost:5000/swagger` (en modo desarrollo)

### Estructura de Respuestas

Todas las respuestas de la API .NET siguen una estructura estándar:

```json
{
  "success": true,
  "message": "Mensaje descriptivo",
  "data": { }
}
```

### Endpoints

#### 1. Health Check

**Método**: `GET`  
**Endpoint**: `/Health`  
**URL Completa**: `http://localhost:5000/api/Health`

**Request**:
```http
GET /api/Health HTTP/1.1
Host: localhost:5000
Accept: application/json
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Servicio activo y funcionando correctamente",
  "data": {
    "status": "healthy",
    "service": "Eurekabank REST API",
    "timestamp": "2024-01-15T10:30:00.000Z"
  }
}
```

#### 2. Login

**Método**: `POST`  
**Endpoint**: `/Auth/login`  
**URL Completa**: `http://localhost:5000/api/Auth/login`

**Request Headers**:
```http
POST /api/Auth/login HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json
```

**Request Body**:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "message": "Autenticación exitosa",
  "data": {
    "authenticated": true,
    "username": "admin"
  }
}
```

**Response (401 Unauthorized)**:
```json
{
  "success": false,
  "message": "Credenciales inválidas",
  "data": null
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "message": "Usuario y contraseña son requeridos",
  "data": null
}
```

#### 3. Obtener Movimientos por Cuenta

**Método**: `GET`  
**Endpoint**: `/Movimientos/cuenta/{cuenta}`  
**URL Completa**: `http://localhost:5000/api/Movimientos/cuenta/00100001`

**Request**:
```http
GET /api/Movimientos/cuenta/00100001 HTTP/1.1
Host: localhost:5000
Accept: application/json
```

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Se encontraron 2 movimientos",
  "data": [
    {
      "cuenta": "00100001",
      "nroMov": 1,
      "fecha": "2024-01-15T10:30:00",
      "tipo": "001",
      "accion": "Depósito en efectivo",
      "importe": 1000.50
    },
    {
      "cuenta": "00100001",
      "nroMov": 2,
      "fecha": "2024-01-16T14:20:00",
      "tipo": "002",
      "accion": "Retiro en cajero",
      "importe": 500.00
    }
  ]
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "message": "El código de cuenta es requerido",
  "data": null
}
```

**Response (500 Internal Server Error)**:
```json
{
  "success": false,
  "message": "Error al obtener movimientos: [detalle del error]",
  "data": null
}
```

#### 4. Registrar Depósito

**Método**: `POST`  
**Endpoint**: `/Movimientos/deposito`  
**URL Completa**: `http://localhost:5000/api/Movimientos/deposito`

**Request Headers**:
```http
POST /api/Movimientos/deposito HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json
```

**Request Body**:
```json
{
  "cuenta": "00100001",
  "importe": 1500.75
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "message": "Depósito registrado exitosamente",
  "data": {
    "operacion": "deposito",
    "cuenta": "00100001",
    "importe": 1500.75
  }
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "message": "El importe debe ser mayor a cero",
  "data": null
}
```

**Response (500 Internal Server Error)**:
```json
{
  "success": false,
  "message": "Error al registrar depósito: [detalle del error]",
  "data": null
}
```

#### 5. Registrar Retiro

**Método**: `POST`  
**Endpoint**: `/Movimientos/retiro`  
**URL Completa**: `http://localhost:5000/api/Movimientos/retiro`

**Request Headers**:
```http
POST /api/Movimientos/retiro HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json
```

**Request Body**:
```json
{
  "cuenta": "00100001",
  "importe": 300.00
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "message": "Retiro registrado exitosamente",
  "data": {
    "operacion": "retiro",
    "cuenta": "00100001",
    "importe": 300.00
  }
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "message": "El código de cuenta es requerido",
  "data": null
}
```

**Response (500 Internal Server Error)**:
```json
{
  "success": false,
  "message": "Error al registrar retiro: [detalle del error]",
  "data": null
}
```

#### 6. Registrar Transferencia

**Método**: `POST`  
**Endpoint**: `/Movimientos/transferencia`  
**URL Completa**: `http://localhost:5000/api/Movimientos/transferencia`

**Request Headers**:
```http
POST /api/Movimientos/transferencia HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json
```

**Request Body**:
```json
{
  "cuentaOrigen": "00100001",
  "cuentaDestino": "00100002",
  "importe": 750.00
}
```

**Response (200 OK)**:
```json
{
  "success": true,
  "message": "Transferencia registrada exitosamente",
  "data": {
    "operacion": "transferencia",
    "cuentaOrigen": "00100001",
    "cuentaDestino": "00100002",
    "importe": 750.00
  }
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "message": "La cuenta origen y destino no pueden ser iguales",
  "data": null
}
```

**Response (500 Internal Server Error)**:
```json
{
  "success": false,
  "message": "Error al registrar transferencia: [detalle del error]",
  "data": null
}
```

---

## Modelos de Datos

### Movimiento

Representa una transacción bancaria.

**Campos**:
- `cuenta` (string): Código de la cuenta
- `nromov` / `nroMov` (int): Número del movimiento
- `fecha` (DateTime): Fecha y hora de la transacción
- `tipo` (string): Código del tipo de movimiento
- `accion` (string): Descripción de la acción realizada
- `importe` (double): Monto de la transacción

**Ejemplo JSON**:
```json
{
  "cuenta": "00100001",
  "nroMov": 1,
  "fecha": "2024-01-15T10:30:00",
  "tipo": "001",
  "accion": "Depósito en efectivo",
  "importe": 1000.50
}
```

### Usuario (RESTful Java)

Credenciales de autenticación.

**Campos**:
- `username` (string): Nombre de usuario
- `password` (string): Contraseña

**Ejemplo JSON**:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

---

## Códigos de Estado HTTP

### Servicios RESTful

- **200 OK**: Operación exitosa
- **400 Bad Request**: Datos de entrada inválidos
- **401 Unauthorized**: Credenciales incorrectas
- **500 Internal Server Error**: Error en el servidor

### Servicios SOAP

Los servicios SOAP generalmente devuelven **200 OK** para todas las respuestas exitosas. Los errores se indican dentro del contenido de la respuesta:
- Estado `1` o `true`: Operación exitosa
- Estado `-1` o `false`: Operación fallida

---

## Ejemplos de Uso con cURL

### RESTful Java - Login

```bash
curl -X POST http://localhost:8080/Eurobank_Restfull_Java/api/eureka/login \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

### RESTful .NET - Obtener Movimientos

```bash
curl -X GET http://localhost:5000/api/Movimientos/cuenta/00100001 \
  -H "Accept: application/json"
```

### RESTful Java - Registrar Depósito

```bash
curl -X POST "http://localhost:8080/Eurobank_Restfull_Java/api/eureka/deposito?cuenta=00100001&importe=1500.75" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json"
```

### RESTful .NET - Registrar Transferencia

```bash
curl -X POST http://localhost:5000/api/Movimientos/transferencia \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "cuentaOrigen": "00100001",
    "cuentaDestino": "00100002",
    "importe": 750.00
  }'
```

---

## Ejemplos de Uso con SOAP UI o Cliente SOAP

### SOAP Java - Login

```xml
POST http://localhost:8080/Eurobank_Soap_Java/EurekabankWS

Content-Type: text/xml; charset=utf-8
SOAPAction: ""

<?xml version="1.0" encoding="utf-8"?>
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" 
                  xmlns:ws="http://ws.monster.edu.ec/">
   <soapenv:Header/>
   <soapenv:Body>
      <ws:login>
         <username>admin</username>
         <password>admin123</password>
      </ws:login>
   </soapenv:Body>
</soapenv:Envelope>
```

### SOAP .NET - Obtener Movimientos

```xml
POST http://localhost:port/ec.edu.monster.ws/EurekabankWS.svc

Content-Type: text/xml; charset=utf-8
SOAPAction: "http://tempuri.org/IEurekabankWS/ObtenerPorCuenta"

<?xml version="1.0" encoding="utf-8"?>
<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <ObtenerPorCuenta xmlns="http://tempuri.org/">
         <cuenta>00100001</cuenta>
      </ObtenerPorCuenta>
   </s:Body>
</s:Envelope>
```

---

## Notas Importantes

1. **Puertos**: Los puertos mencionados (8080 para Java, 5000/5001 para .NET) son valores por defecto y pueden variar según la configuración.

2. **Base de Datos**: Todos los servicios requieren conexión a la base de datos EurekaBank para funcionar correctamente.

3. **Autenticación**: Actualmente los servicios no implementan tokens de autenticación persistentes. Cada operación es independiente.

4. **CORS**: El servicio RESTful .NET tiene CORS habilitado para permitir peticiones desde cualquier origen.

5. **Swagger**: El servicio RESTful .NET incluye documentación Swagger interactiva disponible en modo desarrollo.

6. **Códigos de Empleado**: Las operaciones de depósito, retiro y transferencia utilizan el código de empleado `"0001"` de forma predeterminada.

---

## Diferencias entre Implementaciones

### SOAP vs RESTful

- **SOAP**: Utiliza XML para mensajes, requiere sobre SOAP Envelope, protocolo más pesado pero más formal
- **RESTful**: Utiliza JSON, más ligero, más fácil de consumir desde aplicaciones web y móviles

### Java vs .NET

- **Java**: 
  - RESTful usa query parameters para depósitos, retiros y transferencias
  - Respuestas más simples (strings o valores directos)
  - SOAP usa estado numérico (1 o -1)

- **.NET**: 
  - RESTful usa request body (JSON) para todas las operaciones POST
  - Respuestas estructuradas con ApiResponse wrapper
  - Validaciones más detalladas con mensajes de error específicos
  - Incluye Swagger para documentación interactiva
  - SOAP devuelve string "1" en lugar de int

---

## Soporte

Para más información o soporte técnico, consulte la documentación del código fuente en cada uno de los directorios del servidor.
