# Prompt final — Práctica 04 · Business Model Canvas Interactivo

Este documento registra el **prompt final** utilizado para generar y refinar el Business Model Canvas
interactivo de **TI: Soporte en Acción**.

## Contexto

- **Proyecto:** TI: Soporte en Acción.
- **Tipo de producto:** videojuego educativo 2D pixel art para practicar Tecnologías de la Información.
- **Herramienta de representación visual:** Archify.
- **Ubicación obligatoria:** `docs/business-model-canvas/`.
- **Rama de trabajo:** `documentacion`.
- **Publicación:** GitHub Pages, con rutas relativas y sin backend ni Node.js.

## Prompt final

> Diseña y construye un **Business Model Canvas interactivo** del proyecto **TI: Soporte en Acción**
> con exactamente **nueve componentes** en la disposición clásica del lienzo: Segmentos de clientes,
> Propuesta de valor, Canales, Relación con los usuarios, Fuentes de ingresos/sostenibilidad,
> Recursos clave, Actividades clave, Socios clave y Estructura de costos.
>
> El resultado debe ser una página estática construida con **HTML5, CSS3 y JavaScript**, sin
> dependencias de backend, Node.js ni rutas absolutas, lista para **GitHub Pages**. Todas las rutas
> deben ser relativas (`./css/styles.css`, `./js/canvas.js`, `./img/...`).
>
> No quiero únicamente texto. Debe utilizar **imágenes, pixel art, iconos, diagramas, tarjetas,
> animaciones, modales, tooltips y conexiones visuales** entre los componentes. La página debe ser
> **responsive** y accesible (etiquetas, teclado, contraste).
>
> Respeta la identidad del proyecto: videojuego educativo 2D pixel art, personajes (Alex, Martín,
> Richar, Sofía, Marco, Elena, Estudiante y el Saboteador) y las tecnologías **Unity, Flutter,
> React/PWA, API, Keycloak, MongoDB, Docker, Grafana, Git, GitHub y GitHub Pages**. No agregues
> motores ni bases de datos distintos (por ejemplo, no usar Godot ni PostgreSQL).
>
> Organiza la entrega en `docs/business-model-canvas/` con `index.html`, `README.md`,
> `prompt-final.md`, `css/styles.css`, `js/canvas.js`, la carpeta `img/` para personajes e iconos y la
> carpeta `evidencias/` para las capturas `canvas-v1.png` y `canvas-final.png`.

## Criterios aplicados

1. Disposición clásica del lienzo en cinco columnas y dos filas inferiores (costos e ingresos).
2. Cada bloque muestra un icono, su número, título y tarjetas con los elementos principales.
3. Al hacer clic en un bloque se abre una **ficha (modal)** con descripción, métricas, secciones y galería.
4. Al pasar el cursor sobre las tarjetas se muestran **tooltips** con información ampliada.
5. Se dibujan **conexiones visuales** (SVG animado) entre componentes relacionados.
6. Animación de entrada escalonada de los bloques y acentos de color por componente.
7. Diseño **responsive**: 5 columnas en escritorio, 2 en tablet y 1 en móvil.
8. Rutas relativas y recursos estáticos para funcionar en GitHub Pages.

## Verificación

- La rama de trabajo es `documentacion` (documentación del proyecto).
- Todos los archivos están dentro de `docs/business-model-canvas/`.
- El Canvas contiene exactamente nueve componentes e imágenes.
- No se agregó Godot ni PostgreSQL; se conservan Unity, Flutter, React/PWA, Keycloak, MongoDB,
  Docker y Grafana.
