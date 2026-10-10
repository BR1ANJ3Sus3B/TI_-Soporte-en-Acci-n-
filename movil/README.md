# Aplicación móvil (Flutter + Dart)

Aplicación móvil de "TI: Soporte en Acción". Da acceso a las misiones, guías, progreso y perfil del jugador desde un dispositivo móvil. Consume los datos a través de la API; no se conecta directamente a MongoDB.

## Estado actual

Componente **pendiente**. En este repositorio solo se creó la estructura prevista de carpetas. Todavía no existe un proyecto Flutter configurado (`pubspec.yaml`, `android/`, `ios/`, etc.) ni código de la aplicación. No se implementan aún pantallas ni autenticación.

## Organización prevista

```
movil/
├── assets/
│   ├── images/     # Imágenes
│   ├── icons/      # Iconos
│   └── fonts/      # Tipografías
├── lib/
│   ├── core/
│   │   ├── config/     # Configuración (API base, entornos)
│   │   ├── router/     # Rutas y navegación
│   │   ├── theme/      # Tema y estilos
│   │   ├── network/    # Cliente HTTP e interceptores
│   │   └── widgets/    # Widgets compartidos
│   └── features/
│       ├── auth/       # Autenticación
│       ├── home/       # Pantalla de inicio
│       ├── missions/   # Misiones
│       ├── guides/     # Guías
│       ├── progress/   # Progreso
│       └── profile/    # Perfil
└── test/           # Pruebas
```

## Ejecución

No se documentan comandos de ejecución porque el proyecto Flutter todavía no está configurado. Cuando exista la aplicación, se añadirán aquí los comandos verificados.
