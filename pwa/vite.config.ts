import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { VitePWA } from 'vite-plugin-pwa';

export default defineConfig({
  plugins: [react(), VitePWA({
    // Los SVG se mantienen como assets propios; evita la transformación HTML
    // automática de iconos al generar el manifiesto.
    pwaAssets: { disabled: true },
    registerType: 'prompt',
    includeAssets: ['icons/icon.svg', 'icons/maskable-icon.svg'],
    manifest: {
      name: 'TI: Soporte en Acción', short_name: 'TI Soporte',
      description: 'Plataforma educativa de soporte TI de NovaTech Solutions.',
      start_url: '/', display: 'standalone', theme_color: '#06132A', background_color: '#06132A', lang: 'es',
      icons: [
        { src: '/icons/icon.svg', sizes: 'any', type: 'image/svg+xml', purpose: 'any' },
        { src: '/icons/maskable-icon.svg', sizes: 'any', type: 'image/svg+xml', purpose: 'maskable' }
      ]
    },
    workbox: {
      navigateFallback: '/index.html',
      globPatterns: ['**/*.{js,css,html,svg,png,ico,woff2}'],
      runtimeCaching: [{
        urlPattern: ({ url }) => url.pathname.startsWith('/api/') && !url.pathname.includes('/me'),
        handler: 'NetworkFirst', options: { cacheName: 'public-education', networkTimeoutSeconds: 4, expiration: { maxEntries: 40, maxAgeSeconds: 60 * 60 * 24 * 7 } }
      }]
    }
  })]
});
