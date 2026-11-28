# TI: Soporte en Acción · PWA

PWA educativa en React, TypeScript y Vite para NovaTech Solutions. Permite explorar contenido público como invitado, conservar actividad local y, cuando se proveen las variables autorizadas, iniciar sesión con Keycloak mediante Authorization Code + PKCE S256.

## Desarrollo

```powershell
npm install
npm run dev
npm run typecheck
npm run lint
npm test
npm run build
```

## Servicios externos

Copiar `.env.example` a `.env.local` y completar sólo los valores provisionados por infraestructura. No hay endpoints verificables versionados en este repositorio; por ello todas las rutas REST son configurables por ambiente y las vistas privadas no fabrican datos si faltan.

La aplicación no persiste contraseñas ni tokens en `localStorage`; el token de acceso de Keycloak se mantiene en `sessionStorage` durante la pestaña. El cliente Keycloak debe ser público, tener redirección web registrada (`https://<host>/`) y permitir PKCE S256.

## Offline

Vite PWA genera el manifiesto y service worker. Precarga el shell de la aplicación y aplica `NetworkFirst` únicamente a respuestas de API públicas bajo `/api/`; no cachea las rutas privadas `/me`. El avance de invitado se guarda en IndexedDB, con `localStorage` como respaldo de compatibilidad, y se identifica visualmente como local; no se sube ni genera XP/recompensas.

## Contratos pendientes de confirmar

Para habilitar datos reales, la API debe publicar y autorizar las rutas configuradas como `VITE_API_*_PATH`: misiones, niveles, logros, guías, perfil propio, progreso propio y estado de monitoreo. Los contratos deben validar Bearer tokens del realm de Keycloak y resolver conflictos/sincronización del progreso en servidor.
