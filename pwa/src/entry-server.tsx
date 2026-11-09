import { renderToString } from 'react-dom/server';
import { MemoryRouter } from 'react-router-dom';
import App from './App';
import { AuthProvider } from './auth/AuthContext';
import './styles/global.css';
import './styles/tokens.css';
import './styles/typography.css';
import './styles/components.css';

export function render(url: string): string {
  return renderToString(<AuthProvider><MemoryRouter initialEntries={[url]}><App /></MemoryRouter></AuthProvider>);
}
