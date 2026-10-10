# TI: Soporte en Acción

Videojuego educativo 2D que representa de forma interactiva los desafíos que enfrenta un profesional de Tecnologías de la Información en el mundo laboral. El proyecto está en desarrollo y se compone de un videojuego, una aplicación móvil, una aplicación web progresiva, una API y una base de datos.

## Tecnologías por componente

| Componente | Carpeta | Tecnologías |
| --- | --- | --- |
| Videojuego | `videojuego/` | Unity 6 (6000.6.3f1), C# |
| Aplicación móvil | `movil/` | Flutter, Dart |
| Aplicación web progresiva | `web/` | React, Vite |
| API | `api/` | Node.js, Express, Mongoose |
| Base de datos | `database/` | MongoDB |
| Documentación | `docs/` | Markdown |

## Estructura de carpetas

```
TI-Soporte-en-Accion/
├── README.md
├── .gitignore
├── videojuego/    # Proyecto Unity (Assets, Packages, ProjectSettings)
├── movil/         # Aplicación móvil Flutter
├── web/           # PWA con React y Vite
├── api/           # API Express + Mongoose
├── database/      # Documentación, diagramas y seeds de MongoDB
└── docs/          # Documentación del proyecto
```

Cada componente conserva su README con el detalle de su propósito, organización y estado.

## Responsabilidad de cada componente

- **Unity (`videojuego/`)**: juego 2D con misiones, diálogos con NPC y escenarios que simulan retos de soporte de TI. Es el componente principal de simulación.
- **Flutter (`movil/`)**: acceso móvil a misiones, guías, progreso y perfil del jugador.
- **React (`web/`)**: panel de administración y seguimiento de progreso a través de la PWA.
- **API (`api/`)**: expone los datos de MongoDB a los tres clientes (Unity, Flutter y React). Los clientes no se conectan a la base de datos directamente.
- **MongoDB (`database/`)**: almacena usuarios, misiones, intentos, progresos, guías y registros administrativos.

## Ramas

Ramas permanentes:

| Rama | Propósito |
| --- | --- |
| `main` | Versión estable y publicable |
| `develop` | Integración del trabajo en curso |

Ramas de trabajo:

| Rama | Propósito |
| --- | --- |
| `feature/estructura-inicial` | Organización inicial del monorepositorio |
| `feature/unity-movimiento` | Movimiento y controles del jugador en Unity |
| `feature/unity-misiones` | Misiones y NPC en Unity |
| `feature/flutter-autenticacion` | Autenticación en la app móvil |
| `feature/flutter-pantallas` | Pantallas de la app móvil |
| `feature/react-pwa` | PWA con React y Vite |
| `feature/react-administracion` | Módulo de administración web |
| `feature/api-autenticacion` | Autenticación y tokens en la API |
| `feature/mongodb-modelos` | Modelos y esquemas de MongoDB |
| `docs/documentacion` | Documentación del proyecto |

Para correcciones futuras se usa la convención `fix/descripcion-del-error`.

## Flujo de trabajo con Git

Cada cambio se desarrolla en una rama de trabajo y se integra siguiendo este flujo:

```
rama de trabajo → develop → main
```

- `main` solo recibe versiones estables integradas desde `develop`.
- `develop` integra el resultado de las ramas de trabajo.
- Las ramas de trabajo se crean desde `develop`, no desde `main`.
- Las correcciones se crean como `fix/descripcion-del-error` y también se integran en `develop`.

## Documentación

- [Documentación general](docs/README.md)
- [Base de datos MongoDb](database/README.md)
- [Videojuego](videojuego/README.md)
- [Aplicación móvil](movil/README.md)
- [Aplicación web](web/README.md)
- [API](api/README.md)