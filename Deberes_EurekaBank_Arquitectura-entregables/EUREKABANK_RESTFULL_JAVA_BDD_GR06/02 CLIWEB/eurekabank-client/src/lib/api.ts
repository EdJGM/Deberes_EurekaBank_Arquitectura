import { ServerType, SERVERS } from './servers';

export interface Movimiento {
  cuenta: string;
  nromov: number;
  fecha: string;
  tipo: string;
  accion: string;
  importe: number;
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
}

async function callApiOperation<T>(
  operation: string, 
  params: any = {}, 
  serverType: ServerType
): Promise<ApiResponse<T>> {
  try {
    const serverConfig = SERVERS[serverType];
    const endpoint = serverConfig.protocol === 'soap' ? '/api/soap' : '/api/rest';
    
    const response = await fetch(endpoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ operation, params, serverType }),
    });

    const result = await response.json();
    return result;
  } catch (error: any) {
    return {
      success: false,
      error: error.message || 'Error de conexión',
    };
  }
}

export const createApiClient = (serverType: ServerType) => ({
  health: () => callApiOperation<{ status: string }>('health', {}, serverType),
  
  traerMovimientos: (cuenta: string) => 
    callApiOperation<{ movimiento: Movimiento[] }>('traerMovimientos', { cuenta }, serverType),
  
  regDeposito: (cuenta: string, importe: number) => 
    callApiOperation<{ estado: number }>('regDeposito', { cuenta, importe }, serverType),
  
  regRetiro: (cuenta: string, importe: number) => 
    callApiOperation<{ estado: number }>('regRetiro', { cuenta, importe }, serverType),
  
  regTransferencia: (cuentaOrigen: string, cuentaDestino: string, importe: number) => 
    callApiOperation<{ estado: number }>('regTransferencia', { cuentaOrigen, cuentaDestino, importe }, serverType),
  
  login: (username: string, password: string) => 
    callApiOperation<{ return: boolean }>('login', { username, password }, serverType),
});
