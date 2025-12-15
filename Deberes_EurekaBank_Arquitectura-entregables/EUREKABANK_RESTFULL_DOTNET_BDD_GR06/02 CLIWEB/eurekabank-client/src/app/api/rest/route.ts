import { NextRequest, NextResponse } from 'next/server';

interface LoginRequest {
  username: string;
  password: string;
}

interface TransactionRequest {
  cuenta?: string;
  cuentaOrigen?: string;
  cuentaDestino?: string;
  importe?: number;
}

// Deshabilitar verificación SSL para entorno de desarrollo
// NOTA: Solo para desarrollo/pruebas con certificados autofirmados
if (process.env.NODE_ENV !== 'production') {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

const SOAP_JAVA_PORT = process.env.SOAP_JAVA_PORT || '8080';
const SOAP_DOTNET_PORT = process.env.SOAP_DOTNET_PORT || '57199';
const REST_JAVA_PORT = process.env.REST_JAVA_PORT || '8080';
const REST_DOTNET_PORT = process.env.REST_DOTNET_PORT || '5111';

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const { operation, params, serverType } = body;

    let response;

    // RESTful Java
    if (serverType === 'rest-java') {
      const serverIp = process.env.SERVER_IP || '10.40.15.139';
      const baseUrl = `http://${serverIp}:${REST_JAVA_PORT}/Eurobank_Restfull_Java/api/eureka`;
      
      switch (operation) {
        case 'health':
          response = await fetch(`${baseUrl}/health`);
          const healthText = await response.text();
          return NextResponse.json({ success: true, data: { status: healthText } });

        case 'login':
          response = await fetch(`${baseUrl}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              username: params.username,
              password: params.password
            })
          });
          const loginResult = await response.json();
          return NextResponse.json({ success: true, data: { return: loginResult } });

        case 'traerMovimientos':
          response = await fetch(`${baseUrl}/movimientos/${params.cuenta}`);
          const movimientos = await response.json();
          return NextResponse.json({ success: true, data: { movimiento: movimientos } });

        case 'regDeposito':
          response = await fetch(
            `${baseUrl}/deposito?cuenta=${params.cuenta}&importe=${params.importe}`,
            { method: 'POST', headers: { 'Content-Type': 'application/json' } }
          );
          const depositoMsg = await response.text();
          return NextResponse.json({ 
            success: true, 
            data: { estado: response.ok ? 1 : -1, message: depositoMsg } 
          });

        case 'regRetiro':
          response = await fetch(
            `${baseUrl}/retiro?cuenta=${params.cuenta}&importe=${params.importe}`,
            { method: 'POST', headers: { 'Content-Type': 'application/json' } }
          );
          const retiroMsg = await response.text();
          return NextResponse.json({ 
            success: true, 
            data: { estado: response.ok ? 1 : -1, message: retiroMsg } 
          });

        case 'regTransferencia':
          response = await fetch(
            `${baseUrl}/transferencia?cuentaOrigen=${params.cuentaOrigen}&cuentaDestino=${params.cuentaDestino}&importe=${params.importe}`,
            { method: 'POST', headers: { 'Content-Type': 'application/json' } }
          );
          const transferenciaMsg = await response.text();
          return NextResponse.json({ 
            success: true, 
            data: { estado: response.ok ? 1 : -1, message: transferenciaMsg } 
          });
      }
    }

    // RESTful .NET
    if (serverType === 'rest-dotnet') {
      const serverIp = process.env.SERVER_IP || '10.40.15.139';
      const baseUrl = `http://${serverIp}:${REST_DOTNET_PORT}/api`;
      
      switch (operation) {
        case 'health':
          response = await fetch(`${baseUrl}/Health`);
          const healthData = await response.json();
          return NextResponse.json({ success: true, data: healthData });

        case 'login':
          response = await fetch(`${baseUrl}/Auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              username: params.username,
              password: params.password
            })
          });
          const loginData = await response.json();
          return NextResponse.json({ 
            success: true, 
            data: { return: loginData.data?.authenticated || false } 
          });

        case 'traerMovimientos':
          response = await fetch(`${baseUrl}/Movimientos/cuenta/${params.cuenta}`);
          const movData = await response.json();
          return NextResponse.json({ 
            success: true, 
            data: { movimiento: movData.data || [] } 
          });

        case 'regDeposito':
          response = await fetch(`${baseUrl}/Movimientos/deposito`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              cuenta: params.cuenta,
              importe: params.importe
            })
          });
          const depositoData = await response.json();
          return NextResponse.json({ 
            success: true, 
            data: { 
              estado: depositoData.success ? 1 : -1,
              message: depositoData.message 
            } 
          });

        case 'regRetiro':
          response = await fetch(`${baseUrl}/Movimientos/retiro`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              cuenta: params.cuenta,
              importe: params.importe
            })
          });
          const retiroData = await response.json();
          return NextResponse.json({ 
            success: true, 
            data: { 
              estado: retiroData.success ? 1 : -1,
              message: retiroData.message 
            } 
          });

        case 'regTransferencia':
          response = await fetch(`${baseUrl}/Movimientos/transferencia`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              cuentaOrigen: params.cuentaOrigen,
              cuentaDestino: params.cuentaDestino,
              importe: params.importe
            })
          });
          const transferData = await response.json();
          return NextResponse.json({ 
            success: true, 
            data: { 
              estado: transferData.success ? 1 : -1,
              message: transferData.message 
            } 
          });
      }
    }

    return NextResponse.json(
      { error: 'Operación no válida' },
      { status: 400 }
    );

  } catch (error: any) {
    console.error('Error en REST API:', error);
    return NextResponse.json(
      { error: error.message || 'Error al conectar con el servicio REST' },
      { status: 500 }
    );
  }
}
