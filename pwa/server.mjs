/* global process */
import { createServer } from 'node:http';
import { readFile } from 'node:fs/promises';
import { extname, join, normalize } from 'node:path';
import { render } from './dist/server/entry-server.js';

const port = Number(process.env.PORT || 4173);
const publicRoutes = new Map([
  ['/recursos', { title: 'Recursos educativos | TI: Soporte en Acción', description: 'Recursos educativos de NovaTech Solutions.' }],
  ['/ayuda', { title: 'Ayuda | TI: Soporte en Acción', description: 'Ayuda para TI: Soporte en Acción.' }],
  ['/equipo', { title: 'Equipo NovaTech | TI: Soporte en Acción', description: 'Conoce al equipo NovaTech.' }],
]);
const mime = { '.js': 'text/javascript', '.css': 'text/css', '.svg': 'image/svg+xml', '.json': 'application/json', '.webmanifest': 'application/manifest+json', '.ico': 'image/x-icon' };
createServer(async (request, response) => {
  const url = new URL(request.url || '/', `http://${request.headers.host}`); const path = decodeURIComponent(url.pathname);
  const file = normalize(join('dist', path === '/' ? 'index.html' : path));
  if (path.startsWith('/assets/') || path.startsWith('/icons/') || path === '/sw.js' || path === '/manifest.webmanifest') {
    try { const body = await readFile(file); response.writeHead(200, { 'Content-Type': mime[extname(file)] || 'application/octet-stream', 'Cache-Control': 'public, max-age=31536000, immutable' }); response.end(body); } catch { response.writeHead(404).end(); } return;
  }
  if (!publicRoutes.has(path)) { response.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8', 'Cache-Control': 'no-store' }); response.end(await readFile('dist/index.html')); return; }
  const meta = publicRoutes.get(path); let html = await readFile('dist/index.html', 'utf8');
  html = html.replace('<title>TI: Soporte en Acción</title>', `<title>${meta.title}</title><meta name="description" content="${meta.description}" />`);
  html = html.replace('<div id="root"></div>', `<div id="root">${render(path)}</div>`);
  response.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8', 'Cache-Control': 'public, max-age=0, must-revalidate' }); response.end(html);
}).listen(port, () => console.log(`SSR en http://localhost:${port}`));
