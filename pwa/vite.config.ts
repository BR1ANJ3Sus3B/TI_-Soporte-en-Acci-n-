import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { VitePWA } from 'vite-plugin-pwa';

export default defineConfig({
  plugins: [react(), VitePWA({
    registerType: 'autoUpdate',
    includeAssets: ['icons/icon.svg', 'icons/maskable-icon.svg'],
    manifest: {
      name: 'TI: Soporte en Acción', short_name: 'Soporte TI', description: 'Plataforma educativa de NovaTech Solutions.',
      theme_color: '#0B1120', background_color: '#0B1120', display: 'standalone', start_url: '/', lang: 'es',
      icons: [{ src: '/icons/icon.svg', sizes: 'any', type: 'image/svg+xml', purpose: 'any' }, { src: '/icons/maskable-icon.svg', sizes: 'any', type: 'image/svg+xml', purpose: 'maskable' }]
    },
    workbox: { navigateFallback: '/index.html', globPatterns: ['**/*.{js,css,html,svg,png,ico}'] }
  })]
});
