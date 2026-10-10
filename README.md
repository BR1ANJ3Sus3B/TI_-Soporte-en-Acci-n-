# TI: Soporte en Acción

Proyecto integrador de videojuego educativo en Unity, aplicación móvil en Flutter y PWA en React, con servicios compartidos.

**Autor y responsable único:** Brian Jesús Mendoza Márquez · **Matrícula:** 230308.

**Documentación completa:** [docs/proyecto/README.md](docs/proyecto/README.md)

## Descripción

TI: Soporte en Acción es un videojuego de aventura y simulación en el que el jugador interpreta a Alex, un recién egresado que resuelve incidentes de soporte técnico en NovaTech Solutions. El proyecto integra tres productos relacionados con servicios y datos compartidos:

- **Videojuego** en Unity y C# (historia, misiones e interacción).
- **Aplicación móvil** en Flutter y Dart (acompañamiento y consulta de progreso).
- **PWA** en React para el jugador y la administración.
- **API REST** con autenticación y datos comunes.
- **MongoDB** como base de datos.

## Estado del proyecto

Documentación de diseño y alcance. Las funciones descritas son requisitos previstos; este repositorio no acredita que estén implementadas. El alcance inicial comprende el primer nivel, El Primer Día, y dos roles: jugador y administrador.

## Tecnologías por componente

| Componente | Carpeta | Tecnología |
| --- | --- | --- |
| Videojuego | `videojuego/` | Unity (C#) |
| Aplicación móvil | `movil/` | Flutter (Dart) |
| PWA | `web/` | React |
| API | `api/` | Node.js, Express, Mongoose (propuesta) |
| Base de datos | `database/` | MongoDB |
| Documentación | `docs/` | Markdown |

## Estructura de carpetas

```
TI_-Soporte-en-Acci-n-/
├── README.md
├── .gitignore
├── videojuego/    # Proyecto Unity (Assets, Packages, ProjectSettings)
├── movil/         # Aplicación móvil Flutter
├── web/           # PWA con React
├── api/           # API Express + Mongoose
├── database/      # Documentación, diagramas y seeds de MongoDB
└── docs/          # Documentación del proyecto
```

Cada componente conserva su README con el detalle de su propósito, organización y estado.

## Documentación

- [Documento completo del proyecto](docs/proyecto/README.md)
- [Índice de documentación](docs/README.md)
- [Cobertura documental](docs/proyecto/COBERTURA_DOCUMENTAL.md)
- [Inventario de imágenes](docs/inventario-imagenes.json)
- [Base de datos MongoDB](database/README.md)
- [Videojuego](videojuego/README.md)
- [Aplicación móvil](movil/README.md)
- [Aplicación web](web/README.md)
- [API](api/README.md)

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
| `feature/react-pwa` | PWA con React |
| `feature/react-administracion` | Módulo de administración web |
| `feature/api-autenticacion` | Autenticación y tokens en la API |
| `feature/mongodb-modelos` | Modelos y esquemas de MongoDB |
| `docs/documentacion` | Documentación del proyecto |

Para correcciones futuras se usa la convención `fix/descripcion-del-error`.

## Flujo de trabajo con Git

```
rama de trabajo → develop → main
```

- `main` solo recibe versiones estables integradas desde `develop`.
- `develop` integra el resultado de las ramas de trabajo.
- Las ramas de trabajo se crean desde `develop`, no desde `main`.
- Las correcciones se crean como `fix/descripcion-del-error` y también se integran en `develop`.
