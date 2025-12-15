export type ServerType = 'soap-java' | 'soap-dotnet' | 'rest-java' | 'rest-dotnet';
export type Protocol = 'soap' | 'rest';

export interface ServerConfig {
  type: ServerType;
  protocol: Protocol;
  name: string;
  url: string;
  description: string;
}

const SERVER_IP = process.env.SERVER_IP || '10.40.15.139';
const SOAP_JAVA_PORT = process.env.SOAP_JAVA_PORT || '8080';
const SOAP_DOTNET_PORT = process.env.SOAP_DOTNET_PORT || '57199';
const REST_JAVA_PORT = process.env.REST_JAVA_PORT || '8080';
const REST_DOTNET_PORT = process.env.REST_DOTNET_PORT || '5111';
export const SERVERS: Record<ServerType, ServerConfig> = {
  'soap-java': {
    type: 'soap-java',
    protocol: 'soap',
    name: 'Java SOAP Server',
    url: `http://${SERVER_IP}:${SOAP_JAVA_PORT}/Eurobank_Soap_Java/EurekabankWS?wsdl`,
    description: 'Servidor SOAP implementado en Java con JAX-WS'
  },
  'soap-dotnet': {
    type: 'soap-dotnet',
    protocol: 'soap',
    name: '.NET SOAP Server',
    url: `http://${SERVER_IP}:${SOAP_DOTNET_PORT}/ec.edu.monster.ws/EurekabankWS.svc?wsdl`,
    description: 'Servidor SOAP implementado en .NET con WCF'
  },
  'rest-java': {
    type: 'rest-java',
    protocol: 'rest',
    name: 'Java RESTful Server',
    url: `http://${SERVER_IP}:${REST_JAVA_PORT}/Eurobank_Restfull_Java/api/eureka`,
    description: 'API RESTful implementada en Java con JAX-RS'
  },
  'rest-dotnet': {
    type: 'rest-dotnet',
    protocol: 'rest',
    name: '.NET RESTful Server',
    url: `http://${SERVER_IP}:${REST_DOTNET_PORT}/api`,
    description: 'API RESTful implementada en .NET Core'
  }
};
