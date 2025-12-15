import { NextRequest, NextResponse } from 'next/server';
import * as soap from 'soap';
import { SERVERS, ServerType } from '@/lib/servers';

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const { operation, params, serverType = 'soap-java' } = body;

    console.log('🚀 SOAP Request iniciado:', {
      operation,
      params,
      serverType,
      timestamp: new Date().toISOString()
    });

    const serverConfig = SERVERS[serverType as ServerType];
    if (!serverConfig || serverConfig.protocol !== 'soap') {
      console.error('❌ Error: Tipo de servidor SOAP no válido:', serverType);
      return NextResponse.json(
        { error: 'Tipo de servidor SOAP no válido' },
        { status: 400 }
      );
    }

    console.log('🔗 Configuración del servidor:', {
      name: serverConfig.name,
      url: serverConfig.url,
      type: serverConfig.type
    });

    console.log('⏳ Creando cliente SOAP para URL:', serverConfig.url);
    const client = await soap.createClientAsync(serverConfig.url);
    console.log('✅ Cliente SOAP creado exitosamente');
    
    // Log de métodos disponibles en el cliente
    console.log('📋 Métodos disponibles en el cliente SOAP:', Object.keys(client));
    console.log('📝 Descripción del servicio:', client.describe());

   
    // Configurar headers específicos para .NET WCF si es necesario
    if (serverType === 'soap-dotnet') {
      console.log('🔧 Configurando headers específicos para .NET WCF');
      // WCF requiere headers específicos para algunas operaciones
      client.addHttpHeader('Content-Type', 'text/xml; charset=utf-8');
      console.log('✅ Header Content-Type agregado: text/xml; charset=utf-8');
    }

    let result;

    // Java SOAP operations
    if (serverType === 'soap-java') {
      switch (operation) {
        case 'health':
          result = await client.healthAsync();
          break;
        case 'traerMovimientos':
          result = await client.traerMovimientosAsync({ cuenta: params.cuenta });
          break;
        case 'regDeposito':
          result = await client.regDepositoAsync({
            cuenta: params.cuenta,
            importe: params.importe
          });
          break;
        case 'regRetiro':
          result = await client.regRetiroAsync({
            cuenta: params.cuenta,
            importe: params.importe
          });
          break;
        case 'regTransferencia':
          result = await client.regTransferenciaAsync({
            cuentaOrigen: params.cuentaOrigen,
            cuentaDestino: params.cuentaDestino,
            importe: params.importe
          });
          break;
        case 'login':
          result = await client.loginAsync({
            username: params.username,
            password: params.password
          });
          break;
        default:
          return NextResponse.json(
            { error: 'Operación no válida' },
            { status: 400 }
          );
      }
    }
    // .NET SOAP operations
    else if (serverType === 'soap-dotnet') {
      switch (operation) {
        case 'health':
          console.log('🏥 Ejecutando operación Health para .NET');
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/Health"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición HealthAsync...');
          result = await client.HealthAsync();
          console.log('📥 Respuesta recibida de HealthAsync:', result);
          break;
        case 'traerMovimientos':
          console.log('📋 Ejecutando operación ObtenerPorCuenta para .NET');
          console.log('📝 Parámetros:', { cuenta: params.cuenta });
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/ObtenerPorCuenta"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición ObtenerPorCuentaAsync...');
          result = await client.ObtenerPorCuentaAsync({ cuenta: params.cuenta });
          console.log('📥 Respuesta recibida de ObtenerPorCuentaAsync:', result);
          break;
        case 'regDeposito':
          console.log('💰 Ejecutando operación RegistrarDeposito para .NET');
          console.log('📝 Parámetros:', { cuenta: params.cuenta, importe: params.importe });
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/RegistrarDeposito"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición RegistrarDepositoAsync...');
          result = await client.RegistrarDepositoAsync({
            cuenta: params.cuenta,
            importe: params.importe
          });
          console.log('📥 Respuesta recibida de RegistrarDepositoAsync:', result);
          break;
        case 'regRetiro':
          console.log('💸 Ejecutando operación RegistrarRetiro para .NET');
          console.log('📝 Parámetros:', { cuenta: params.cuenta, importe: params.importe });
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/RegistrarRetiro"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición RegistrarRetiroAsync...');
          result = await client.RegistrarRetiroAsync({
            cuenta: params.cuenta,
            importe: params.importe
          });
          console.log('📥 Respuesta recibida de RegistrarRetiroAsync:', result);
          break;
        case 'regTransferencia':
          console.log('🔄 Ejecutando operación RegistrarTransferencia para .NET');
          console.log('📝 Parámetros:', { 
            cuentaOrigen: params.cuentaOrigen, 
            cuentaDestino: params.cuentaDestino, 
            importe: params.importe 
          });
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/RegistrarTransferencia"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición RegistrarTransferenciaAsync...');
          result = await client.RegistrarTransferenciaAsync({
            cuentaOrigen: params.cuentaOrigen,
            cuentaDestino: params.cuentaDestino,
            importe: params.importe
          });
          console.log('📥 Respuesta recibida de RegistrarTransferenciaAsync:', result);
          break;
        case 'login':
          console.log('🔐 Ejecutando operación Login para .NET');
          console.log('📝 Parámetros:', { 
            username: params.username, 
            password: '***' // No mostrar la contraseña en logs
          });
          // Para .NET WCF, establecer SOAPAction específicamente para Login
          if (serverType === 'soap-dotnet') {
            const soapAction = '"http://tempuri.org/IEurekabankWS/Login"';
            client.addHttpHeader('SOAPAction', soapAction);
            console.log('✅ SOAPAction agregado:', soapAction);
          }
          console.log('📤 Enviando petición LoginAsync...');
          result = await client.LoginAsync({
            username: params.username,
            password: params.password
          });
          console.log('📥 Respuesta recibida de LoginAsync:', result);
          // Log para debugging detallado
          console.log('🔍 SOAP Request XML enviado:', client.lastRequest);
          console.log('🔍 SOAP Response XML recibido:', client.lastResponse);
          break;
        default:
          return NextResponse.json(
            { error: 'Operación no válida' },
            { status: 400 }
          );
      }
    }

    console.log('✅ Operación SOAP completada exitosamente');
    console.log('📋 Resultado final:', result);
    console.log('📦 Datos originales del servidor:', result[0]);
    
    // Normalizar respuesta para .NET SOAP para que sea consistente con otros servidores
    let normalizedData = result[0];
    
    if (serverType === 'soap-dotnet') {
      // Para login: convertir LoginResult a return para consistencia
      if (operation === 'login' && normalizedData && 'LoginResult' in normalizedData) {
        normalizedData = { return: normalizedData.LoginResult };
        console.log('🔄 Respuesta normalizada de .NET Login:', normalizedData);
      }
      
      // Para health: convertir HealthResult a status para consistencia  
      if (operation === 'health' && normalizedData && 'HealthResult' in normalizedData) {
        normalizedData = { status: normalizedData.HealthResult };
        console.log('🔄 Respuesta normalizada de .NET Health:', normalizedData);
      }
      
      // Para movimientos: extraer el array y normalizar campos
      if (operation === 'traerMovimientos' && normalizedData && 'ObtenerPorCuentaResult' in normalizedData) {
        // Extraer el array de movimientos desde la estructura anidada
        const movimientosRaw = normalizedData.ObtenerPorCuentaResult?.movimiento || [];
        
        // Normalizar cada movimiento: convertir campos con mayúsculas a minúsculas
        const movimientosNormalizados = movimientosRaw.map((mov: any) => ({
          cuenta: mov.Cuenta || mov.cuenta,
          nromov: mov.NroMov || mov.nromov,
          fecha: mov.Fecha || mov.fecha,
          tipo: mov.Tipo || mov.tipo,
          accion: mov.Accion || mov.accion,
          importe: mov.Importe || mov.importe
        }));
        
        normalizedData = { movimiento: movimientosNormalizados };
        console.log('🔄 Respuesta normalizada de .NET Movimientos:', normalizedData);
        console.log('📊 Número de movimientos encontrados:', movimientosNormalizados.length);
        console.log('🔍 Primer movimiento normalizado:', movimientosNormalizados[0]);
      }
      
      // Para depósitos: convertir RegistrarDepositoResult string a estado número
      if (operation === 'regDeposito' && normalizedData && 'RegistrarDepositoResult' in normalizedData) {
        const estadoNumerico = parseInt(normalizedData.RegistrarDepositoResult) || -1;
        normalizedData = { estado: estadoNumerico };
        console.log('🔄 Respuesta normalizada de .NET Depósito:', normalizedData);
      }
      
      // Para retiros: convertir RegistrarRetiroResult string a estado número
      if (operation === 'regRetiro' && normalizedData && 'RegistrarRetiroResult' in normalizedData) {
        const estadoNumerico = parseInt(normalizedData.RegistrarRetiroResult) || -1;
        normalizedData = { estado: estadoNumerico };
        console.log('🔄 Respuesta normalizada de .NET Retiro:', normalizedData);
      }
      
      // Para transferencias: convertir RegistrarTransferenciaResult string a estado número
      if (operation === 'regTransferencia' && normalizedData && 'RegistrarTransferenciaResult' in normalizedData) {
        const estadoNumerico = parseInt(normalizedData.RegistrarTransferenciaResult) || -1;
        normalizedData = { estado: estadoNumerico };
        console.log('🔄 Respuesta normalizada de .NET Transferencia:', normalizedData);
      }
    }
    
    console.log('📦 Datos finales que se envían al cliente:', normalizedData);
    
    return NextResponse.json({ success: true, data: normalizedData });
  } catch (error: any) {
    console.error('❌ ERROR EN SOAP - Detalles completos:', {
      message: error.message,
      stack: error.stack,
      code: error.code,
      errno: error.errno,
      syscall: error.syscall,
      address: error.address,
      port: error.port,
      response: error.response,
      body: error.body,
      statusCode: error.statusCode
    });
    
    // Si hay información de la última petición/respuesta
    if (error.lastRequest) {
      console.error('🔍 Última petición SOAP enviada:', error.lastRequest);
    }
    if (error.lastResponse) {
      console.error('🔍 Última respuesta SOAP recibida:', error.lastResponse);
    }
    
    // Errores específicos de red
    if (error.code === 'ECONNREFUSED') {
      console.error('🚫 ERROR DE CONEXIÓN: El servidor rechazó la conexión');
      console.error('🔧 Verificar que el servidor SOAP .NET esté ejecutándose en la URL configurada');
    } else if (error.code === 'ENOTFOUND') {
      console.error('🌐 ERROR DNS: No se pudo resolver la dirección del servidor');
      console.error('🔧 Verificar la IP/hostname en la configuración');
    } else if (error.code === 'ETIMEDOUT') {
      console.error('⏰ ERROR TIMEOUT: La conexión se agotó');
      console.error('🔧 El servidor puede estar sobrecargado o no responder');
    }
    
    return NextResponse.json(
      { error: error.message || 'Error al conectar con el servicio SOAP' },
      { status: 500 }
    );
  }
}
