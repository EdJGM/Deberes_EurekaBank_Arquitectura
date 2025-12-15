'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { DepositForm } from './DepositForm';
import { WithdrawForm } from './WithdrawForm';
import { TransferForm } from './TransferForm';
import { MovementsView } from './MovementsView';
import { LogOut, Server } from 'lucide-react';
import { ServerType, SERVERS } from '@/lib/servers';

interface BankDashboardProps {
  username: string;
  serverType: ServerType;
  onLogout: () => void;
}

export function BankDashboard({ username, serverType, onLogout }: BankDashboardProps) {
  const [activeTab, setActiveTab] = useState('movements');

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-blue-100 p-4">
      <div className="max-w-6xl mx-auto">
        <div className="mb-6 flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
          <div>
            <h1 className="text-3xl font-bold text-blue-900">EurekaBank</h1>
            <p className="text-gray-600">Bienvenido, {username}</p>
            <div className="flex items-center gap-2 mt-1 text-sm text-gray-500">
              <Server className="h-4 w-4" />
              <span>{SERVERS[serverType].name}</span>
            </div>
          </div>
          <Button variant="outline" onClick={onLogout}>
            <LogOut className="mr-2 h-4 w-4" />
            Cerrar Sesión
          </Button>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Panel de Control Bancario</CardTitle>
            <CardDescription>
              Administre sus cuentas y realice transacciones
            </CardDescription>
          </CardHeader>
          <CardContent>
            <Tabs value={activeTab} onValueChange={setActiveTab}>
              <TabsList className="grid w-full grid-cols-4">
                <TabsTrigger value="movements">Movimientos</TabsTrigger>
                <TabsTrigger value="deposit">Depósito</TabsTrigger>
                <TabsTrigger value="withdraw">Retiro</TabsTrigger>
                <TabsTrigger value="transfer">Transferencia</TabsTrigger>
              </TabsList>

              <TabsContent value="movements" className="space-y-4">
                <MovementsView serverType={serverType} />
              </TabsContent>

              <TabsContent value="deposit" className="space-y-4">
                <DepositForm serverType={serverType} />
              </TabsContent>

              <TabsContent value="withdraw" className="space-y-4">
                <WithdrawForm serverType={serverType} />
              </TabsContent>

              <TabsContent value="transfer" className="space-y-4">
                <TransferForm serverType={serverType} />
              </TabsContent>
            </Tabs>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
