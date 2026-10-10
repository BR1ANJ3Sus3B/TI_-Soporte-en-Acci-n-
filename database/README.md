# Base de datos (MongoDB)

Base de datos de "TI: Soporte en Acción". Nombre previsto: `ti_soporte_en_accion`.

## Acceso a la base de datos

Ningún cliente se conecta directamente a MongoDB. El flujo es:

```
Unity / Flutter / React  →  API (Express + Mongoose)  →  MongoDB
```

- Los esquemas de Mongoose pertenecerán a `api/src/models/`.
- La conexión a la base de datos pertenecerá a `api/src/config/database.js` (cuando la API se implemente).
- La URI local de desarrollo es `mongodb://127.0.0.1:27017/ti_soporte_en_accion`.

## Colecciones previstas

### `usuarios`

Datos de las personas que usan el juego y las aplicaciones.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `nombreUsuario` | String | **Índice único** |
| `correo` | String | **Índice único** |
| `contrasenaHash` | String | Hash de la contraseña, nunca texto plano |
| `rol` | String | Por ejemplo: `jugador` o `administrador` |
| `fechaCreacion` | Date | |

Seguridad:

- `nombreUsuario` y `correo` deben tener índices únicos.
- Las contraseñas se almacenan mediante hash (por ejemplo, bcrypt/argon2), nunca en texto plano.
- El hash se realiza en la API; la base de datos solo guarda el resultado.

### `misiones`

Catálogo de misiones del videojuego.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `codigo` | String | Identificador legible |
| `titulo` | String | |
| `descripcion` | String | |
| `nivel` | Number | |
| `recompensa` | Number | |
| `activa` | Boolean | |

### `intentos`

Registro de cada intento de resolver una misión.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `usuario` | ObjectId | Referencia a `usuarios` |
| `mision` | ObjectId | Referencia a `misiones` |
| `resultado` | String | Por ejemplo: `exito` o `fallo` |
| `puntaje` | Number | |
| `duracionSegundos` | Number | |
| `fecha` | Date | |

### `progresos`

Estado de avance de cada usuario.

| Campo | Tipo | Notas |
| --- | --- | --- |
| `usuario` | ObjectId | Referencia a `usuarios` |
| `misionesCompletadas` | Array | Referencias a `misiones` |
| `nivelActual` | Number | |
| `puntajeTotal` | Number | |
| `ultimaActualizacion` | Date | |

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

Los diagramas y los datos de ejemplo aún no se han creado.

## Notas de seguridad

- No se incluyen credenciales reales ni respaldos de datos privados en el repositorio.
- Las credenciales locales se gestionan con variables de entorno (ver `api/.env.example`).
