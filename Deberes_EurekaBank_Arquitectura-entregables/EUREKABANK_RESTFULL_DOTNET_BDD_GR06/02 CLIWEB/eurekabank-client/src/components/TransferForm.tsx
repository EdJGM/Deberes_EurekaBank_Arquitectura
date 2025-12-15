'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { createApiClient } from '@/lib/api';
import { ServerType } from '@/lib/servers';
import { ArrowRightLeft } from 'lucide-react';

interface TransferFormProps {
  serverType: ServerType;
}

export function TransferForm({ serverType }: TransferFormProps) {
  const [cuentaOrigen, setCuentaOrigen] = useState('');
  const [cuentaDestino, setCuentaDestino] = useState('');
  const [importe, setImporte] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  const api = createApiClient(serverType);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage(null);

    const result = await api.regTransferencia(cuentaOrigen, cuentaDestino, parseFloat(importe));

    if (result.success && result.data?.estado === 1) {
      setMessage({ 
        type: 'success', 
        text: `Transferencia de $${importe} realizada exitosamente de ${cuentaOrigen} a ${cuentaDestino}` 
      });
      setCuentaOrigen('');
      setCuentaDestino('');
      setImporte('');
    } else {
      setMessage({ 
        type: 'error', 
        text: result.error || 'Error al realizar la transferencia. Verifique saldo y cuentas activas.' 
      });
    }

    setLoading(false);
  };

  return (
    <div className="space-y-4">
      <div className="flex items-center gap-2 text-blue-900">
        <ArrowRightLeft className="h-5 w-5" />
        <h3 className="text-lg font-semibold">Realizar Transferencia</h3>
      </div>
      
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="space-y-2">
          <Label htmlFor="transfer-origen">Cuenta Origen</Label>
          <Input
            id="transfer-origen"
            type="text"
            placeholder="Ej: 123456"
            value={cuentaOrigen}
            onChange={(e) => setCuentaOrigen(e.target.value)}
            required
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="transfer-destino">Cuenta Destino</Label>
          <Input
            id="transfer-destino"
            type="text"
            placeholder="Ej: 789012"
            value={cuentaDestino}
            onChange={(e) => setCuentaDestino(e.target.value)}
            required
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="transfer-importe">Importe</Label>
          <Input
            id="transfer-importe"
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
          {loading ? 'Procesando...' : 'Realizar Transferencia'}
        </Button>
      </form>
    </div>
  );
}
