'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { SERVERS, ServerType } from '@/lib/servers';
import { Server, Check, Code2, Cloud } from 'lucide-react';

interface ServerSelectionProps {
  onServerSelect: (serverType: ServerType) => void;
}

export function ServerSelection({ onServerSelect }: ServerSelectionProps) {
  const [selectedServer, setSelectedServer] = useState<ServerType | null>(null);

  const handleSelect = (type: ServerType) => {
    setSelectedServer(type);
  };

  const handleContinue = () => {
    if (selectedServer) {
      onServerSelect(selectedServer);
    }
  };

  const getServerIcon = (protocol: string) => {
    return protocol === 'soap' ? <Cloud className="h-5 w-5" /> : <Code2 className="h-5 w-5" />;
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-blue-100 p-4">
      <Card className="w-full max-w-4xl">
        <CardHeader className="space-y-1">
          <div className="flex items-center justify-center mb-4">
            <Server className="h-12 w-12 text-blue-600" />
          </div>
          <CardTitle className="text-2xl font-bold text-center text-blue-900">
            EurekaBank - Selección de Servidor
          </CardTitle>
          <CardDescription className="text-center">
            Seleccione el servidor y protocolo que desea utilizar
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {Object.entries(SERVERS).map(([key, server]) => (
              <button
                key={key}
                onClick={() => handleSelect(key as ServerType)}
                className={`relative p-6 rounded-lg border-2 transition-all text-left ${
                  selectedServer === key
                    ? 'border-blue-600 bg-blue-50 shadow-lg'
                    : 'border-gray-200 bg-white hover:border-blue-300 hover:shadow-md'
                }`}
              >
                {selectedServer === key && (
                  <div className="absolute top-3 right-3">
                    <Check className="h-6 w-6 text-blue-600" />
                  </div>
                )}
                
                <div className="space-y-3">
                  <div className="flex items-center gap-2">
                    {getServerIcon(server.protocol)}
                    <h3 className="text-lg font-semibold text-gray-900">
                      {server.name}
                    </h3>
                  </div>
                  
                  <div className="flex items-center gap-2">
                    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                      server.protocol === 'soap' 
                        ? 'bg-purple-100 text-purple-800' 
                        : 'bg-green-100 text-green-800'
                    }`}>
                      {server.protocol.toUpperCase()}
                    </span>
                    <span className="text-xs text-gray-500">
                      {server.type.includes('java') ? '☕ Java' : '🔷 .NET'}
                    </span>
                  </div>
                  
                  <p className="text-sm text-gray-600">
                    {server.description}
                  </p>
                  
                  <div className="pt-2">
                    <p className="text-xs text-gray-500 font-mono break-all">
                      {server.url}
                    </p>
                  </div>
                </div>
              </button>
            ))}
          </div>

          {selectedServer && (
            <div className="pt-4 space-y-3">
              <div className="bg-blue-50 border border-blue-200 rounded-md p-4">
                <p className="text-sm text-blue-900">
                  <strong>Servidor seleccionado:</strong> {SERVERS[selectedServer].name}
                </p>
                <p className="text-xs text-blue-700 mt-1">
                  <strong>Protocolo:</strong> {SERVERS[selectedServer].protocol.toUpperCase()}
                </p>
                <p className="text-xs text-blue-700 mt-1">
                  Asegúrese de que el servidor esté ejecutándose antes de continuar.
                </p>
              </div>
              
              <Button 
                onClick={handleContinue} 
                className="w-full"
                size="lg"
              >
                Continuar al Sistema Bancario
              </Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
