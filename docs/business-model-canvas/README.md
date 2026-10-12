# Práctica 04 — Business Model Canvas Interactivo

Business Model Canvas tradicional de nueve componentes para el proyecto **TI: Soporte en Acción**,
presentado como una página web estática, visual e interactiva compatible con **GitHub Pages**.

## Proyecto

**TI: Soporte en Acción** es un videojuego educativo **2D pixel art** enfocado en la práctica de
conocimientos de **Tecnologías de la Información**. El jugador interpreta a **Alex**, un recién
egresado que resuelve incidentes de soporte técnico en **NovaTech Solutions**, con la presencia del
**Saboteador** como antagonista principal.

- **Autor y responsable único:** Brian Jesús Mendoza Márquez · Matrícula 230308

## Objetivo

Se desarrolló un **Business Model Canvas tradicional de nueve componentes** utilizando la metodología
de lienzo y apoyándose en **Archify** como herramienta de representación visual. El resultado se
publica como un **Canvas interactivo** con imágenes, pixel art, iconos, tarjetas, animaciones,
modales, tooltips y conexiones visuales entre los nueve bloques.

## Los nueve componentes

1. Segmentos de clientes / usuarios
2. Propuesta de valor
3. Canales
4. Relación con los usuarios
5. Fuentes de ingresos / sostenibilidad
6. Recursos clave
7. Actividades clave
8. Socios clave
9. Estructura de costos

## Tecnologías

- Unity
- Flutter
- React / PWA
- API
- Keycloak
- MongoDB
- Docker
- Grafana
- Git
- GitHub
- GitHub Pages

> La documentación describe requisitos previstos y diseño del producto. No acredita que todas las
> funciones estén implementadas.

## Canvas interactivo

- [Abrir el Business Model Canvas interactivo](./index.html)

El Canvas se genera únicamente con HTML5, CSS3, JavaScript y recursos estáticos. No requiere Node.js,
servidor local ni backend, y utiliza rutas relativas para funcionar directamente en GitHub Pages.

## Estructura de la carpeta

```
docs/business-model-canvas/
├── index.html          # Canvas interactivo (página principal)
├── README.md           # Este documento
├── prompt-final.md     # Prompt final utilizado
├── css/
│   └── styles.css      # Estilos (pixel art, responsive)
├── js/
│   └── canvas.js       # Datos, modales, tooltips y conexiones
├── img/                # Personajes e iconos de los componentes
└── evidencias/         # Capturas del proceso
```

## Evidencias

- [Primera versión del lienzo (canvas-v1.png)](./evidencias/canvas-v1.png)
- [Versión final del lienzo (canvas-final.png)](./evidencias/canvas-final.png)

## Personajes del proyecto

Alex (protagonista), Martín (jefe de TI), Richar (redes), Sofía (contabilidad), Marco (desarrollo),
Elena (docente/evaluadora), Estudiante (jugador) y el Saboteador (antagonista).

## Rama de trabajo

`documentacion`

Todo el contenido de esta práctica permanece en la rama de documentación hasta su revisión e
integración autorizada. No se realizan fusiones automáticas hacia `main`.

## Uso en local

Basta con abrir `index.html` en un navegador moderno o publicar la carpeta mediante GitHub Pages.
No se requieren pasos de compilación.
