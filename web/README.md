# Aplicación web progresiva (React + Vite)

Aplicación web progresiva de "TI: Soporte en Acción". Incluye el panel de administración y el seguimiento del progreso. Consume los datos a través de la API; no se conecta directamente a MongoDB.

## Estado actual

Componente **pendiente**. En este repositorio solo se creó la estructura prevista de carpetas. Todavía no existe un proyecto React/Vite configurado (`package.json`, `vite.config`, etc.) ni código de la aplicación. No se implementan aún pantallas ni autenticación.

## Organización prevista

```
web/
├── public/
│   └── icons/      # Iconos de la PWA
├── src/
│   ├── assets/         # Recursos estáticos
│   ├── components/     # Componentes reutilizables
│   ├── layouts/        # Plantillas de diseño
│   ├── routes/         # Rutas de la aplicación
│   ├── services/       # Consumo de la API
│   ├── styles/         # Estilos globales
│   └── features/
│       ├── auth/       # Autenticación
│       ├── home/       # Inicio
│       ├── missions/   # Misiones
│       ├── guides/     # Guías
│       ├── progress/   # Progreso
│       ├── profile/    # Perfil
│       └── admin/      # Administración
└── README.md
```

## Ejecución

No se documentan comandos de ejecución porque el proyecto React/Vite todavía no está configurado. Cuando exista la aplicación, se añadirán aquí los comandos verificados.
