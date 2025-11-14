'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { createApiClient, Movimiento } from '@/lib/api';
import { ServerType } from '@/lib/servers';
import { Receipt, Search } from 'lucide-react';

interface MovementsViewProps {
  serverType: ServerType;
}

export function MovementsView({ serverType }: MovementsViewProps) {
  const [cuenta, setCuenta] = useState('');
  const [movimientos, setMovimientos] = useState<Movimiento[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const api = createApiClient(serverType);

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setMovimientos([]);

    const result = await api.traerMovimientos(cuenta);

    if (result.success && result.data?.movimiento) {
      const movs = Array.isArray(result.data.movimiento) 
        ? result.data.movimiento 
        : [result.data.movimiento];
      setMovimientos(movs);
      
      if (movs.length === 0) {
        setError('No se encontraron movimientos para esta cuenta.');
      }
    } else {
      setError(result.error || 'Error al consultar movimientos. Verifique el número de cuenta.');
    }

    setLoading(false);
  };

  const formatDate = (dateString: string) => {
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch {
      return dateString;
    }
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  return (
    <div className="space-y-4">
      <div className="flex items-center gap-2 text-blue-900">
        <Receipt className="h-5 w-5" />
        <h3 className="text-lg font-semibold">Consultar Movimientos</h3>
      </div>

      <form onSubmit={handleSearch} className="space-y-4">
        <div className="flex gap-2">
          <div className="flex-1 space-y-2">
            <Label htmlFor="movements-cuenta">Número de Cuenta</Label>
            <Input
              id="movements-cuenta"
              type="text"
              placeholder="Ej: 123456"
              value={cuenta}
              onChange={(e) => setCuenta(e.target.value)}
              required
            />
          </div>
          <div className="flex items-end">
            <Button type="submit" disabled={loading}>
              <Search className="mr-2 h-4 w-4" />
              {loading ? 'Buscando...' : 'Buscar'}
            </Button>
          </div>
        </div>
      </form>

      {error && (
        <div className="text-sm text-amber-700 bg-amber-50 p-3 rounded-md border border-amber-200">
          {error}
        </div>
      )}

      {movimientos.length > 0 && (
        <div className="space-y-3">
          <h4 className="font-medium text-gray-900">
            Movimientos de la cuenta {cuenta} ({movimientos.length} registros)
          </h4>
          <div className="border rounded-lg overflow-hidden">
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-blue-50 border-b">
                  <tr>
                    <th className="px-4 py-3 text-left text-xs font-medium text-blue-900 uppercase tracking-wider">
                      #
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-blue-900 uppercase tracking-wider">
                      Fecha
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-blue-900 uppercase tracking-wider">
                      Tipo
                    </th>
                    <th className="px-4 py-3 text-left text-xs font-medium text-blue-900 uppercase tracking-wider">
                      Acción
                    </th>
                    <th className="px-4 py-3 text-right text-xs font-medium text-blue-900 uppercase tracking-wider">
                      Importe
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {movimientos.map((mov, index) => (
                    <tr key={index} className="hover:bg-gray-50">
                      <td className="px-4 py-3 text-sm text-gray-900">
                        {mov.nromov}
                      </td>
                      <td className="px-4 py-3 text-sm text-gray-500">
                        {formatDate(mov.fecha)}
                      </td>
                      <td className="px-4 py-3 text-sm text-gray-900">
                        {mov.tipo}
                      </td>
                      <td className="px-4 py-3 text-sm">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                          mov.accion === 'Crédito' || mov.accion === 'CREDITO'
                            ? 'bg-green-100 text-green-800'
                            : 'bg-red-100 text-red-800'
                        }`}>
                          {mov.accion}
                        </span>
                      </td>
                      <td className={`px-4 py-3 text-sm text-right font-medium ${
                        mov.accion === 'Crédito' || mov.accion === 'CREDITO'
                          ? 'text-green-600'
                          : 'text-red-600'
                      }`}>
                        {formatCurrency(mov.importe)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
