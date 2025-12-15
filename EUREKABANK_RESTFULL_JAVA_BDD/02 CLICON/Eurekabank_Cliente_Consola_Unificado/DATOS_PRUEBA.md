# 📋 Datos de Prueba - Eurekabank

## 👥 Usuarios Disponibles

| Usuario    | Contraseña  | Estado   | Nombre Completo      |
|------------|-------------|----------|----------------------|
| cromero    | chicho      | ACTIVO   | Carlos A. Romero     |
| lcastro    | flaca       | ACTIVO   | Lidia Castro         |
| aramos     | china       | ACTIVO   | Angelica Ramos       |
| cvalencia  | angel       | ACTIVO   | Claudia Valencia     |
| rcruz      | cerebro     | ACTIVO   | Ricardo Cruz         |
| lpachas    | gato        | ACTIVO   | Luis A. Pachas       |
| htello     | machupichu  | ACTIVO   | Hugo V. Tello        |
| pcarrasco  | tinajones   | ACTIVO   | Pedro H. Carrasco    |
| MONSTER    | MONSTER9    | ACTIVO   | Usuario Admin        |
| creyes     | linda       | ANULADO  | Claudia Reyes        |
| ediaz      | princesa    | ANULADO  | Edith Diaz           |

⚠️ **Nota:** Solo usuarios con estado ACTIVO pueden iniciar sesión.

## 🏦 Sucursales

| Código | Nombre      | Ciudad   | Dirección                    |
|--------|-------------|----------|------------------------------|
| 001    | Sipan       | Chiclayo | Av. Balta 1456               |
| 002    | Chan Chan   | Trujillo | Jr. Independencia 456        |
| 003    | Los Olivos  | Lima     | Av. Central 1234             |
| 004    | Pardo       | Lima     | Av. Pardo 345 - Miraflores   |
| 005    | Misti       | Arequipa | Bolivar 546                  |
| 006    | Machupicchu | Cusco    | Calle El Sol 534             |
| 007    | Grau        | Piura    | Av. Grau 1528                |

## 💰 Tipos de Cuenta

- **Ahorro:** Cuenta de ahorros personal
- **Corriente:** Cuenta corriente empresarial

## 💵 Monedas

| Código | Descripción |
|--------|-------------|
| 01     | Soles (S/.) |
| 02     | Dólares ($) |

## 🔄 Tipos de Movimiento

| Código | Descripción            | Acción  |
|--------|------------------------|---------|
| 001    | Apertura de cuenta     | INGRESO |
| 002    | Cancelar cuenta        | SALIDA  |
| 003    | Depósito               | INGRESO |
| 004    | Retiro                 | SALIDA  |
| 005    | Interés                | INGRESO |
| 006    | Mantenimiento          | SALIDA  |
| 007    | ITF (Impuesto)         | SALIDA  |
| 008    | Transferencia (ingreso)| INGRESO |
| 009    | Transferencia (salida) | SALIDA  |
| 010    | Cargo por movimiento   | SALIDA  |

## 📊 Ejemplos de Cuentas

Las cuentas específicas dependen de los datos cargados en tu base de datos.
Típicamente el formato es:

- **Cuenta Ahorro:** 001-XXXXX (sucursal 001)
- **Cuenta Corriente:** 002-XXXXX (sucursal 002)

Para ver cuentas disponibles, ejecuta en la BD:

```sql
-- SQL Server / MySQL
SELECT chr_cuencodigo AS Cuenta, 
       vch_cuentipo AS Tipo,
       dec_cuensaldo AS Saldo
FROM cuenta
WHERE vch_cuenestado = 'ACTIVO';
```

## 🧪 Escenarios de Prueba

### 1. Consulta de Movimientos
```
Usuario: cromero
Password: chicho
Opción: 1 - Consultar Movimientos
Cuenta: [ingrese una cuenta válida]
```

### 2. Depósito Simple
```
Usuario: lcastro
Password: flaca
Opción: 2 - Realizar Depósito
Cuenta: [cuenta válida]
Importe: 500.00
```

### 3. Retiro
```
Usuario: aramos
Password: china
Opción: 3 - Realizar Retiro
Cuenta: [cuenta con saldo suficiente]
Importe: 200.00
```

### 4. Transferencia
```
Usuario: rcruz
Password: cerebro
Opción: 4 - Realizar Transferencia
Cuenta Origen: [cuenta con saldo]
Cuenta Destino: [otra cuenta válida]
Importe: 150.00
```

## 🔧 Configuración de Servidores

### Servidor SOAP .NET
- URL: http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc
- WSDL: http://localhost:57199/ec.edu.monster.ws/EurekabankWS.svc?wsdl
- BD: SQL Server (eurekabank_soap_dotnet)

### Servidor SOAP Java
- URL: http://localhost:8080/Eurobank_Soap_Java/EurekabankWS
- WSDL: http://localhost:8080/Eurobank_Soap_Java/EurekabankWS?wsdl
- BD: MySQL (eurekabank_soap_java)

### Servidor REST .NET
- URL: http://localhost:5111/api
- Swagger: http://localhost:5111/swagger
- BD: SQL Server (eurekabank_rest_dotnet)

### Servidor REST Java
- URL: http://localhost:8080/Eurobank_Restfull_Java/api
- Context: /api/eureka
- BD: MySQL (eurekabank_rest_java)

## ⚠️ Solución de Problemas Comunes

### "No se pudieron obtener los movimientos"
- ✅ Verifica que la cuenta exista en la BD
- ✅ Asegúrate de usar el formato correcto de cuenta

### "Saldo insuficiente" (al retirar)
- ✅ Consulta primero el saldo de la cuenta
- ✅ Verifica que haya fondos suficientes

### "Cuenta inválida" (al transferir)
- ✅ Ambas cuentas deben existir y estar activas
- ✅ No puedes transferir a la misma cuenta

### "Error de conexión"
- ✅ Verifica que el servidor esté ejecutándose
- ✅ Comprueba que el puerto sea correcto
- ✅ Revisa el firewall

## 📞 Soporte

Para problemas con:
- **Base de datos:** Verifica los scripts en `03 BDD/`
- **Servidores:** Revisa los logs en cada proyecto servidor
- **Cliente:** Consulta el README.md principal
