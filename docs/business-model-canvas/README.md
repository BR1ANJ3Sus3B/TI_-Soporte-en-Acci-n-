# Práctica 04 — Business Model Canvas Interactivo

Business Model Canvas tradicional de nueve componentes para el proyecto **TI: Soporte en Acción**,
presentado como una página web estática, visual e interactiva compatible con **GitHub Pages**.

## Proyecto

**TI: Soporte en Acción** es un videojuego educativo **2D pixel art** enfocado en la práctica de
conocimientos de **Tecnologías de la Información**. El jugador interpreta a **Alex**, un técnico
junior que resuelve incidentes reales de soporte, redes, programación, servidores y ciberseguridad
en **NovaTech Solutions**, con el **Saboteador** como antagonista principal.

- **Autor y responsable único:** Brian Jesús Mendoza Márquez · Matrícula 230308

## Objetivo

Diseñar un **Business Model Canvas tradicional de nueve componentes** con una presentación más
visual y profesional, tipo *dashboard* de videojuego RPG pixel art. El resultado se publica como un
**Canvas interactivo** con imágenes, pixel art, iconos, tarjetas, modales, tooltips, galería de
escenas y conexiones visuales entre los nueve bloques.

## Los nueve componentes

1. Socios clave
2. Actividades clave
3. Recursos clave
4. Propuesta de valor
5. Relación con el usuario
6. Canales
7. Segmentos de mercado
8. Estructura de costes
9. Ingresos / sostenibilidad

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

### Nota sobre ingresos

El proyecto se encuentra en su **etapa académica**: es un proyecto educativo **en desarrollo y sin
monetización directa**. Las líneas de sostenibilidad (licencias educativas, implementación
institucional, capacitación, convenios académicos, módulos especializados y contenido adicional) se
plantean únicamente como **posibilidades futuras**, no como ingresos activos.

## Canvas interactivo

- [Abrir el Business Model Canvas interactivo](./index.html)

El Canvas se genera únicamente con HTML5, CSS3, JavaScript y recursos estáticos. No requiere Node.js,
servidor local ni backend, y utiliza rutas relativas para funcionar directamente en GitHub Pages.

### Interactividad

- **Hover sobre un bloque:** resalta sus conexiones y atenúa los bloques no relacionados.
- **Clic en un bloque o en "Ver detalle":** abre una ficha completa en un modal.
- **Tooltips:** al pasar el cursor sobre las tarjetas de cada componente.
- **Galería con lightbox:** las escenas del juego se abren en pantalla completa.

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
├── img/
│   ├── personajes/     # Sprites de los personajes
│   ├── usuarios/       # Iconos de segmentos
│   ├── propuesta/      # Imágenes de la propuesta de valor
│   ├── canales/        # Iconos de canales
│   ├── relacion/       # Iconos de relación con el usuario
│   ├── actividades/    # Iconos de actividades clave
│   ├── recursos/       # Iconos de recursos clave
│   ├── socios/         # Iconos de socios clave
│   ├── costos/         # Iconos de la estructura de costes
│   ├── sostenibilidad/ # Iconos de ingresos / sostenibilidad
│   └── galeria/        # Escenas "Dentro de NovaTech Solutions"
└── evidencias/         # Capturas del proceso
```

## Evidencias

- [Primera versión del lienzo (canvas-v1.png)](./evidencias/canvas-v1.png)
- [Versión final del lienzo (canvas-final.png)](./evidencias/canvas-final.png)

## Personajes del proyecto

Alex (protagonista · técnico junior), Martín (mentor · soporte senior), Richar (redes y servidores),
Marco (desarrollo de software), Sofía (empleada de NovaTech Solutions) y el Saboteador (antagonista).

## Rama de trabajo

`documentacion`

Todo el contenido de esta práctica permanece en la rama de documentación hasta su revisión e
integración autorizada. No se realizan fusiones automáticas hacia `main`.

## Uso en local

Basta con abrir `index.html` en un navegador moderno o publicar la carpeta mediante GitHub Pages.
No se requieren pasos de compilación.
