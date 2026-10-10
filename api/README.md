# API (Node.js + Express + Mongoose)

API de "TI: Soporte en Acción". Es el único punto de acceso a MongoDB: los tres clientes (Unity, Flutter y React) consumen los datos a través de esta API y nunca se conectan directamente a la base de datos.

## Estado actual

Componente **pendiente**. En este repositorio solo se creó la estructura prevista de carpetas y el archivo de variables de entorno de ejemplo. Todavía no existe un proyecto Node.js configurado (`package.json`) ni código de la API. No se implementan aún autenticación ni endpoints.

## Organización prevista

```
api/
├── src/
│   ├── config/         # Configuración, incluida la conexión a MongoDB (database.js)
│   ├── models/         # Esquemas de Mongoose
│   ├── controllers/    # Controladores de las rutas
│   ├── routes/         # Definición de rutas
│   ├── services/       # Lógica de negocio
│   ├── middlewares/    # Middlewares (autenticación, errores)
│   └── validators/     # Validación de datos de entrada
├── tests/              # Pruebas
├── .env.example        # Variables de entorno de ejemplo
└── README.md
```

## Conexión a la base de datos

- La conexión a MongoDB se implementará en `api/src/config/database.js`.
- Los esquemas de Mongoose pertenecerán a `api/src/models/`.
- Ver la documentación de las colecciones previstas en [`database/README.md`](../database/README.md).

## Variables de entorno

Copiar `.env.example` a `.env` y ajustar los valores locales. El archivo `.env` real está excluido por `.gitignore` y nunca debe subirse al repositorio.

## Ejecución

No se documentan comandos de ejecución porque el proyecto Node.js todavía no está configurado. Cuando exista la API, se añadirán aquí los comandos verificados.
