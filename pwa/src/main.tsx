import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { registerSW } from 'virtual:pwa-register';
import { App } from './App';
import { AuthProvider } from './auth';
import { ErrorBoundary } from './error-boundary';
import './styles.css';

registerSW({ onNeedRefresh() { if (confirm('Hay una actualización disponible. ¿Actualizar ahora?')) window.location.reload(); } });
createRoot(document.getElementById('root')!).render(<StrictMode><ErrorBoundary><BrowserRouter><AuthProvider><App /></AuthProvider></BrowserRouter></ErrorBoundary></StrictMode>);
