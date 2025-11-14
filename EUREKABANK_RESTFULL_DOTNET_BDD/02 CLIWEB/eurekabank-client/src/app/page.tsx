'use client';

import { useState } from 'react';
import { ServerSelection } from '@/components/ServerSelection';
import { LoginForm } from '@/components/LoginForm';
import { BankDashboard } from '@/components/BankDashboard';
import { ServerType } from '@/lib/servers';

type AppState = 'server-selection' | 'login' | 'dashboard';

export default function Home() {
  const [appState, setAppState] = useState<AppState>('server-selection');
  const [selectedServer, setSelectedServer] = useState<ServerType>('soap-java');
  const [username, setUsername] = useState('');

  const handleServerSelect = (serverType: ServerType) => {
    setSelectedServer(serverType);
    setAppState('login');
  };

  const handleLoginSuccess = (user: string) => {
    setUsername(user);
    setAppState('dashboard');
  };

  const handleLogout = () => {
    setAppState('server-selection');
    setUsername('');
  };

  const handleBackToServerSelection = () => {
    setAppState('server-selection');
  };

  return (
    <>
      {appState === 'server-selection' && (
        <ServerSelection onServerSelect={handleServerSelect} />
      )}
      {appState === 'login' && (
        <LoginForm 
          serverType={selectedServer}
          onLoginSuccess={handleLoginSuccess}
          onBackToServerSelection={handleBackToServerSelection}
        />
      )}
      {appState === 'dashboard' && (
        <BankDashboard 
          username={username} 
          serverType={selectedServer}
          onLogout={handleLogout} 
        />
      )}
    </>
  );
}
