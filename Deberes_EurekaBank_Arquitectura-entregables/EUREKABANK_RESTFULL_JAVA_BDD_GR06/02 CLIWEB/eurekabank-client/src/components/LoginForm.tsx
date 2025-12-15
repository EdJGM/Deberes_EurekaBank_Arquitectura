'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { createApiClient } from '@/lib/api';
import { ServerType, SERVERS } from '@/lib/servers';

interface LoginFormProps {
  serverType: ServerType;
  onLoginSuccess: (username: string) => void;
  onBackToServerSelection: () => void;
}

export function LoginForm({ serverType, onLoginSuccess, onBackToServerSelection }: LoginFormProps) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const api = createApiClient(serverType);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    const result = await api.login(username, password);

    if (result.success && result.data?.return) {
      onLoginSuccess(username);
    } else {
      setError('Credenciales inválidas. Por favor, intente nuevamente.');
    }

    setLoading(false);
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-blue-100 p-4">
      <Card className="w-full max-w-md">
        <CardHeader className="space-y-1">
          <CardTitle className="text-2xl font-bold text-center text-blue-900">
            EurekaBank
          </CardTitle>
          <CardDescription className="text-center">
            Ingrese sus credenciales para acceder
          </CardDescription>
          <div className="pt-2">
            <div className="bg-blue-50 border border-blue-200 rounded-md p-3 text-center">
              <p className="text-sm text-blue-900 font-medium">
                {SERVERS[serverType].name}
              </p>
            </div>
          </div>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="username">Usuario</Label>
              <Input
                id="username"
                type="text"
                placeholder="Ingrese su usuario"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="password">Contraseña</Label>
              <Input
                id="password"
                type="password"
                placeholder="Ingrese su contraseña"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            {error && (
              <div className="text-sm text-red-600 bg-red-50 p-3 rounded-md border border-red-200">
                {error}
              </div>
            )}
            <Button type="submit" className="w-full" disabled={loading}>
              {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
            </Button>
            <Button 
              type="button" 
              variant="outline" 
              className="w-full" 
              onClick={onBackToServerSelection}
            >
              Cambiar Servidor
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
