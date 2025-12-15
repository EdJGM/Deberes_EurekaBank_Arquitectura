'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { createApiClient } from '@/lib/api';
import { ServerType } from '@/lib/servers';
import { DollarSign } from 'lucide-react';

interface DepositFormProps {
  serverType: ServerType;
}

export function DepositForm({ serverType }: DepositFormProps) {
  const [cuenta, setCuenta] = useState('');
  const [importe, setImporte] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  const api = createApiClient(serverType);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage(null);

    const result = await api.regDeposito(cuenta, parseFloat(importe));

    if (result.success && result.data?.estado === 1) {
      setMessage({ type: 'success', text: `Depósito de $${importe} realizado exitosamente en la cuenta ${cuenta}` });
      setCuenta('');
      setImporte('');
    } else {
      setMessage({ type: 'error', text: result.error || 'Error al realizar el depósito. Verifique los datos.' });
    }

    setLoading(false);
  };

  return (
    <div className="space-y-4">
      <div className="flex items-center gap-2 text-blue-900">
        <DollarSign className="h-5 w-5" />
        <h3 className="text-lg font-semibold">Realizar Depósito</h3>
      </div>
      
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="space-y-2">
          <Label htmlFor="deposit-cuenta">Número de Cuenta</Label>
          <Input
            id="deposit-cuenta"
            type="text"
            placeholder="Ej: 123456"
            value={cuenta}
            onChange={(e) => setCuenta(e.target.value)}
            required
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="deposit-importe">Importe</Label>
          <Input
            id="deposit-importe"
            type="number"
            step="0.01"
            placeholder="0.00"
            value={importe}
            onChange={(e) => setImporte(e.target.value)}
            required
            min="0.01"
          />
        </div>

        {message && (
          <div className={`text-sm p-3 rounded-md ${
            message.type === 'success' 
              ? 'text-green-700 bg-green-50 border border-green-200' 
              : 'text-red-700 bg-red-50 border border-red-200'
          }`}>
            {message.text}
          </div>
        )}

        <Button type="submit" className="w-full" disabled={loading}>
          {loading ? 'Procesando...' : 'Realizar Depósito'}
        </Button>
      </form>
    </div>
  );
}
