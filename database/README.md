# Base de datos (MongoDB)

Base de datos de "TI: Soporte en Acción". Nombre previsto: `ti_soporte_en_accion`.

## Acceso a la base de datos

Ningún cliente se conecta directamente a MongoDB. El flujo es:

```
Unity / Flutter / React  →  API (Express + Mongoose)  →  MongoDB
```

- Los modelos Mongoose actuales están en `api/src/models/`.
- La conexión runtime está en `api/src/config/database.js`; el servidor se inicia desde `api/src/server.js`.
- En el Compose API/Mongo local la URI interna es `mongodb://mongo:27017/ti_soporte_en_accion`; Mongo no publica su puerto al host.
- Si instalas MongoDB directamente en el host para desarrollo, la URI documentada es `mongodb://127.0.0.1:27017/ti_soporte_en_accion`.

## Colecciones implementadas / sin seeds

Los modelos y validaciones están en código, pero el catálogo comienza vacío:
no se importan datos de demostración como contenido oficial.

### Identidad y perfiles (`UserProfile`)

Datos de las personas que usan el juego y las aplicaciones.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `subject` | String | Identificador estable `sub` de Keycloak; único |
| `displayName` | String | Nombre visible editable por la API |
| `email` | String | Claim de Keycloak, no identificador de autorización |

Seguridad:

- La contraseña pertenece a Keycloak y **no se almacena en MongoDB**.
- El índice único de identidad es `subject`, no el correo ni el nombre editable.
- Los roles se validan desde el access token en cada petición; el cliente no puede asignárselos editando el perfil.

### `misiones`

Catálogo de misiones del videojuego.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `code` | String | Identificador legible; único |
| `title` | String | |
| `description` | String | |
| `category` | String | |
| `level` | Number | Orden/nivel configurado |
| `xpReward` | Number | La API lo lee de la misión; no lo acepta del cliente al validar recompensa |
| `active` | Boolean | El catálogo público solo devuelve entradas activas |
| `requirements`, `objectives` | Array | Requisitos y objetivos definidos por el administrador |

### `Level`

| Campo | Tipo | Notas |
| --- | --- | --- |
| `code` | String | Identificador único |
| `title`, `description` | String | Contenido del mapa/nivel |
| `order` | Number | Orden explícito |
| `requiredMissionCodes` | Array | Condiciones de desbloqueo declaradas |
| `unlockedByDefault`, `active` | Boolean | Reglas explícitas, no deducidas de datos del cliente |

### `intentos`

Registro de cada intento de resolver una misión.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `subject` | String | `sub` de Keycloak |
| `attemptId` | String | Idempotencia única por usuario |
| `missionCode` | String | Referencia a misión por código |
| `reportedOutcome` | String | Resultado reportado, inicialmente no confiable |
| `status` | String | `pending_verification`, `reviewing`, `approved` o `rejected` |
| `awardedXp` | Number | Calculado por API desde la recompensa de misión aprobada |
| `durationSeconds`, `createdAt`, `reviewedAt` | Number/Date | Metadatos del intento y revisión |

### `progresos`

Estado de avance de cada usuario.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `subject` | String | Identificador estable del usuario; único |
| `completedMissionCodes`, `inProgressMissionCodes` | Array | Estados reconciliados por servidor |
| `awardedAttemptIds` | Array | Evita acreditar dos veces el mismo intento |
| `xp`, `level`, `nextLevelXp` | Number/null | Nivel/meta permanecen null si la regla no está configurada |
| `specialties`, `awardedAchievements` | Array | Habilidades y premios verificados |
| `updatedAt` | Date | Gestionado por Mongoose |

### `guias`

Contenido de apoyo para el jugador.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `titulo` | String | |
| `contenido` | String | |
| `categoria` | String | |
| `orden` | Number | |
| `publicada` | Boolean | |

### `registros_administrativos`

Bitácora de acciones administrativas.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `usuario` | ObjectId | Referencia a `usuarios` |
| `accion` | String | Tipo de acción realizada |
| `entidad` | String | Colección o recurso afectado |
| `detalle` | String | Descripción |
| `fecha` | Date | |

## Organización de esta carpeta

```
database/
├── README.md    # Este documento
├── diagrams/    # Diagramas del modelo de datos
└── seeds/       # Datos de ejemplo para desarrollo
```

Los diagramas y seeds aún no se han creado. La API devuelve catálogos vacíos hasta que un administrador publique datos reales.

## Notas de seguridad

- No se incluyen credenciales reales ni respaldos de datos privados en el repositorio.
- Las credenciales locales se gestionan con variables de entorno (ver `api/.env.example`).
