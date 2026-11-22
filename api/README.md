# API REST — TI: Soporte en Acción

API Node.js 24 + Express + Mongoose. Flutter, Unity y la PWA acceden a MongoDB
solo por esta API. La API valida access tokens OIDC de Keycloak y deriva el usuario
del claim estable `sub`.

## Estado actual y límites

- `GET /api/v1/profile`, catálogo de misiones, niveles, progreso, especialidades,
  logros, áreas y guías publicadas ya tienen rutas implementadas.
- El catálogo no se siembra automáticamente. Missions, levels, guides y
  achievements quedan vacíos hasta que un administrador los cree.
- `POST /api/v1/attempts` guarda un reporte idempotente como
  `pending_verification`; no concede XP. Solo un administrador puede revisarlo y,
  al aprobar un resultado exitoso, la API usa la recompensa guardada en MongoDB.
- Unity no contiene cliente API en la rama revisada; no se puede demostrar todavía
  quién atestigua un resultado de juego. La revisión administrativa evita que un
  access token del jugador sea suficiente para regalarse XP.
- MongoDB de desarrollo en Compose no publica puerto y solo es accesible desde la
  red privada API/exporter. Para producción se requiere Mongo con autenticación,
  TLS y credenciales administradas fuera de Git.

## Keycloak comprobado

El issuer local de discovery fue:

```text
http://localhost:8080/realms/TI%20Soporte%20en%20Acci%C3%B3n
```

El realm `ti-soporte` no existe en esta instancia. `ti-soporte-flutter` aún no
está registrado: la solicitud OIDC devuelve `Client not found`. La API necesita
además un Audience mapper que incluya `KEYCLOAK_AUDIENCE` en los access tokens.
Consulta `../movil/KEYCLOAK.md`; la API no acepta contraseñas ni `JWT_SECRET` local.

## Contrato de API v1

Las rutas `/api/v1/*` requieren `Authorization: Bearer <access_token>`. La API
valida firma RS256, issuer exacto, audiencia y expiración usando JWKS de Keycloak.
Los roles provienen de `realm_access.roles` y `resource_access[KEYCLOAK_CLIENT_ID].roles`.

| Método y ruta | Propósito |
|---|---|
| `GET /api/v1/profile` | Perfil de la cuenta autenticada, progreso confirmado y rol |
| `PATCH /api/v1/profile` | Actualiza exclusivamente `displayName` |
| `GET /api/v1/missions` | Catálogo publicado y estado propio |
| `GET /api/v1/missions/:code` | Detalle, objetivos, requisitos y recompensa del catálogo |
| `GET /api/v1/areas` | Categorías derivadas de misiones publicadas |
| `GET /api/v1/levels` | Niveles y requisitos explícitos configurados |
| `GET /api/v1/progress` | XP y misiones completadas/en curso del `sub` autenticado |
| `GET /api/v1/progress/specialties` | Habilidades del jugador |
| `GET /api/v1/achievements` | Logros configurados y desbloqueos persistidos |
| `GET /api/v1/guides`, `GET /api/v1/guides/:code` | Guías publicadas |
| `POST /api/v1/attempts` | Registra intento como pendiente, sin conceder XP |
| `GET /api/v1/attempts` | Historial de intentos del usuario actual |
| `GET /api/v1/admin/attempts` | Cola de revisión (rol `administrador`) |
| `GET /api/v1/admin/users` | Perfiles conocidos, sin contraseñas ni tokens (rol `administrador`) |
| `GET /api/v1/admin/users` | Perfiles conocidos, sin contraseñas/tokens (rol `administrador`) |
| `POST /api/v1/admin/attempts/:id/review` | Aprueba/rechaza intento (rol `administrador`) |
| `POST /api/v1/admin/missions`, `/guides`, `/levels`, `/achievements` | Alta validada (rol `administrador`) |

Errores uniformes: 400 datos inválidos, 401 token ausente/inválido/vencido, 403
rol insuficiente, 404 recurso no publicado/inexistente, 409 conflicto idempotente,
503 configuración/servicio OIDC incompleto.

La especificación está en [`openapi.yaml`](openapi.yaml). Los contratos se
mantienen compatibles con las lecturas ya previstas por la PWA (`/profile`,
`/missions`, `/missions/:id`, `/progress/specialties`, `/achievements`). Su modo
actual sigue en demo y la sesión web es independiente: no se afirma que la PWA ya
consuma esta API.

## Configuración y arranque

Se usa `PORT=3000`, el puerto base esperado por el cliente PWA existente. En
desarrollo local con Docker:

```powershell
Copy-Item infra/api/.env.example infra/api/.env
# Completa KEYCLOAK_AUDIENCE solo después de añadir el Audience mapper en el cliente.
docker compose -f infra/api/compose.yaml config --quiet
docker compose -f infra/api/compose.yaml up -d --build --wait
docker compose -f infra/api/compose.yaml ps
```

El Compose requiere las redes existentes `keycloak_default` y
`ti-soporte-monitoring_default`; inicia primero esos servicios si las redes no
están creadas. Usa un volumen Mongo nuevo llamado `api-mongo-data`, separado de
los volúmenes previos; no ejecutar `down -v` en otros stacks.

En Android físico conectado por USB, tras iniciar la API y Keycloak:

```powershell
adb reverse tcp:8080 tcp:8080
adb reverse tcp:3000 tcp:3000
```

Así el teléfono puede usar el issuer de discovery `localhost:8080` y
`API_BASE_URL=http://localhost:3000/api/v1` mientras esté conectado por USB.
Para LAN usa una dirección del PC alcanzable desde el teléfono y actualiza issuer,
discovery y API base para que coincidan; en producción, HTTPS.

También puede ejecutarse Node directamente desde `api/` con Node 20 o superior,
`MONGODB_URI` accesible y las variables OIDC configuradas:

```powershell
npm ci
npm test
npm start
```

## Prometheus, MongoDB exporter y Grafana

- `GET /metrics` exporta métricas HTTP con labels acotados (`method`, ruta de
  plantilla y código de estado); no usa usuarios, tokens ni datos personales.
- `infra/api/compose.yaml` agrega el exporter oficial `percona/mongodb_exporter`
  en la red privada.
- `prometheus-scrape.yaml` contiene los jobs que se integran en el Prometheus ya
  existente; este branch no crea otro Prometheus ni Grafana.
- `grafana-dashboard.json` aporta el dashboard de API/MongoDB/Keycloak para
  importarlo en la instancia existente.
- `prometheus-alerts.yaml` define alertas de disponibilidad; los umbrales de latencia/error requieren un SLO de producto antes de fijarse.

Todavía hay que integrar el fragmento de scrape y el dashboard en la rama de
monitoreo; esta rama no modifica otros branches. Las métricas no incluyen datos
personales ni credenciales.

## Pruebas

```powershell
npm ci
npm test
npm run check
```

La suite actual contiene 11 pruebas y usa un JWKS local firmado para probar JWT
(RS256, issuer y audiencia), además de fakes de modelos para probar rutas, permisos
e idempotencia. La prueba
con el realm Keycloak real sigue bloqueada mientras falte el cliente móvil y el
Audience mapper.
