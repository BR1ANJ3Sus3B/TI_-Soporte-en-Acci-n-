# Integración Flutter/API — estado verificable

## Implementado

- Flutter usa `API_BASE_URL` de compilación y manda access tokens Bearer para
  perfil, misiones, detalle, áreas, progreso, especialidades y logros.
- El cliente gestiona timeout, desconexión, JSON inválido, 401, 403 y reintento.
  El modo demo solo se habilita explícitamente con `DEMO_MODE=true`.
- La API Express/Mongoose implementa dichos endpoints, guías, niveles, intentos,
  revisión administrativa, idempotencia y `/metrics`.
- JWT valida RS256, issuer, audience, expiración y subject con JWKS de Keycloak.
- MongoDB usa un volumen Docker nuevo `ti-soporte-api_api-mongo-data` conectado
  por una red interna; Mongo no publica puerto ni recibe conexiones desde Flutter.
- Los intentos enviados por jugador quedan `pending_verification`. No generan XP
  hasta la revisión de un administrador; el valor se toma de la misión guardada.
- Prometheus scrape jobs y dashboard JSON se añadieron en `api/`, sin crear otro
  Grafana/Prometheus ni modificar su otra rama.

## Bloqueos reales de integración

- Discovery confirmó el realm `TI Soporte en Acción`, issuer
  `http://localhost:8080/realms/TI%20Soporte%20en%20Acci%C3%B3n`.
- El cliente móvil `ti-soporte-flutter` no existe aún en ese realm. El endpoint
  OIDC respondió `Client not found` para ese Client ID y callback.
- Falta un Audience mapper en Keycloak y definir el `KEYCLOAK_AUDIENCE` real del
  access token. Mientras tanto las rutas protegidas de la API devuelven
  `503 keycloak_not_configured`; la salud de proceso/Mongo sí está disponible.
- El `KC_ADMIN_PASSWORD` añadido al `.env` local ignorado por Git no es la clave
  del admin del volumen Keycloak ya inicializado. Keycloak lo rechazó como
  `invalid_grant`; el volumen se conservó, sin resetear usuarios.
- El catálogo Mongo se mantiene vacío hasta cargar misiones, niveles, guías y
  logros confirmados. No se sembraron datos mock como contenido real.
- Unity no tiene cliente API en la rama revisada. El protocolo de resultados
  espera revisión porque no existe atestación confiable del cliente de juego.
- La PWA tiene sus lecturas previstas en `/profile`, `/missions` y
  `/progress/specialties`, pero su sesión web no está integrada con esta API.
- Los jobs de API/Mongo/Keycloak están en `api/prometheus-scrape.yaml` y el panel
  en `api/grafana-dashboard.json`; deben integrarse al Prometheus/Grafana existente
  desde su rama cuando se autorice integrar cambios entre ramas.
- No se detectó teléfono/emulador Android y este entorno es Windows; no se probó
  el navegador real ni se compiló iOS con Xcode.

## Fuente de verdad y seguridad

El identificador de dueño de los registros es el `sub` del token. El servidor no
acepta subject/roles enviados por Flutter, no almacena passwords y no deja que el
cliente otorgue XP. La API no calcula `nextLevelXp` mientras la regla de niveles
no esté definida; Flutter lo muestra como no disponible en lugar de inventar una
meta. El mapa Flutter representa categorías/áreas, no niveles.
