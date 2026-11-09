import { StrictMode } from 'react'; import { createRoot, hydrateRoot } from 'react-dom/client'; import { BrowserRouter } from 'react-router-dom'; import { registerSW } from 'virtual:pwa-register'; import App from './App'; import { AuthProvider } from './auth/AuthContext'; import './styles/global.css'; import './styles/tokens.css'; import './styles/typography.css'; import './styles/components.css';
registerSW({ immediate: true });
const app = <StrictMode><AuthProvider><BrowserRouter><App /></BrowserRouter></AuthProvider></StrictMode>;
const root = document.getElementById('root')!;
if (root.hasChildNodes()) hydrateRoot(root, app); else createRoot(root).render(app);
