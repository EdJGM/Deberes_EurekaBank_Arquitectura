# Cliente de Escritorio Unificado para 4 Servicios Web

Este proyecto contiene un cliente de escritorio (Swing) que se conecta a cuatro servicios web REST configurables mediante el archivo `services.properties`.

## Interfaz Unificada
- Selector de servicio (combo) que muestra nombre y número.
- Panel dinámico de parámetros (se generan según la propiedad `serviceX.params`).
- Botón "Invocar" que construye la URL y envía una petición GET.
- Botón "Limpiar" para reiniciar los campos.
- Área de salida con estado HTTP y cuerpo/JSON formateado.

## Configuración de Servicios
Edite `src/main/resources/services.properties`:
```
service1.name=Usuarios
service1.baseUrl=https://tu-dominio.com/api
service1.endpoint=usuarios
service1.params=usuarioId

service2.name=Productos
service2.baseUrl=https://tu-dominio.com/api
service2.endpoint=productos
service2.params=categoria,limite

service3.name=Pedidos
service3.baseUrl=https://tu-dominio.com/api
service3.endpoint=pedidos
service3.params=pedidoId

service4.name=Reportes
service4.baseUrl=https://tu-dominio.com/api
service4.endpoint=reportes
service4.params=fechaInicio,fechaFin,tipo
```
Si un servicio no requiere parámetros, deje `serviceX.params` vacío o elimínelo.

## Estrategias (lógica por servicio)
- `UserServiceStrategy` (Servicio 1): usa `usuarioId` en el path.
- `ProductServiceStrategy` (Servicio 2): usa `categoria` y `limite` como query params.
- `OrderServiceStrategy` (Servicio 3): usa `pedidoId` en el path.
- `ReportServiceStrategy` (Servicio 4): valida fechas `yyyy-MM-dd` y arma la query con `tipo`.
- `StrategyFactory` elige la estrategia según el servicio seleccionado.

## Ejecutar en Windows (cmd.exe)
```cmd
mvn clean package
mvn exec:java -Dexec.mainClass=ec.edu.monster.client.DesktopClientApp
```
El `exec` plugin ya apunta al main, puede usar también:
```cmd
mvn exec:java
```

## Pruebas
```cmd
mvn test
```

## Extensiones Futuras
- Métodos POST/PUT con cuerpo JSON.
- Autenticación (token Bearer, API key) por servicio.
- Manejo de reintentos y timeouts por servicio.
- Validación de parámetros (tipos, rangos, formato fecha) y mensajes en la UI.

## Requisitos
- Java 21
- Maven 3.9+

## Notas
- Reemplace el dominio de ejemplo por el real antes de probar.
- Si un servicio devuelve algo que no es JSON, se muestra cuerpo crudo.
- Los parámetros vacíos se omiten del query string.
