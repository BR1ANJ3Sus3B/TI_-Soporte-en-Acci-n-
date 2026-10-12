# Prompt final — Práctica 04 · Business Model Canvas Interactivo

Este documento registra el **prompt final** utilizado para generar y refinar el Business Model Canvas
interactivo de **TI: Soporte en Acción** (versión visual mejorada).

## Contexto

- **Proyecto:** TI: Soporte en Acción.
- **Tipo de producto:** videojuego educativo 2D pixel art para practicar Tecnologías de la Información.
- **Herramienta de representación visual:** Archify.
- **Ubicación obligatoria:** `docs/business-model-canvas/`.
- **Rama de trabajo:** `documentacion`.
- **Publicación:** GitHub Pages, con rutas relativas y sin backend ni Node.js.

## Prompt final

> Mejora el **Business Model Canvas interactivo** del proyecto **TI: Soporte en Acción** dentro de
> `docs/business-model-canvas/`, en la rama `documentacion`, manteniendo únicamente esa carpeta.
> Debe verse como un **dashboard de videojuego RPG pixel art**, más visual y profesional, y debe ser
> reconocible como un **Business Model Canvas tradicional de nueve componentes**.
>
> El reparto de personajes se limita a la lista oficial: Alex, Martín, Richar, Marco, Sofía y el
> Saboteador. **No incluyas ningún otro personaje** que no pertenezca a esa lista.
>
> No inventes ingresos comerciales actuales: **no** usar "venta única", contenido de pago,
> publicidad ni tienda digital como fuentes de ingreso. El proyecto está en **etapa académica**, sin
> monetización directa; solo pueden describirse **posibilidades futuras** (licencias educativas,
> implementación institucional, capacitación, convenios académicos, módulos especializados y
> contenido adicional).
>
> **Socios clave** sin tiendas digitales ni convenios comerciales inventados: institución educativa,
> docentes/asesores, equipo del Proyecto Integrador, GitHub, ecosistemas Unity/Flutter y tecnologías
> open source.
>
> **Segmentos** sin edad específica ni "sin conocimientos previos": Jugador/Estudiante,
> Docente/Evaluador, Administrador, Editor de contenido e Institución educativa.
>
> No agregues motores ni bases de datos distintos: **no Godot, no PostgreSQL**. Conserva Unity,
> Flutter, React/PWA, API, Keycloak, MongoDB, Docker, Grafana, Git, GitHub y GitHub Pages.
>
> La **Propuesta de valor** debe ser el bloque central más destacado, con la imagen de Alex y la
> cita "Aprender y practicar habilidades de Tecnologías de la Información mediante un videojuego
> educativo que simula problemas reales...". Incluye un **flujo de misión** (Misión → Investigación
> → Análisis → Diagnóstico → Solución → Evaluación → Retroalimentación → XP → Logro), una
> **galería "Dentro de NovaTech Solutions"** con escenas del juego y una **mini-arquitectura**
> (Usuario → Keycloak → Unity/Flutter/React-PWA → API → MongoDB, con Docker, Grafana, Git/GitHub y
> GitHub Pages).
>
> Interactividad: hover, imágenes, resumen, etiquetas, botón **"Ver detalle"** y modal/panel lateral.
> Al seleccionar un bloque: resaltarlo, resaltar sus conexiones, atenuar los bloques no relacionados
> y mostrar información ampliada. Las imágenes de la galería se abren en un lightbox.
>
> Diseño oscuro con paleta azul oscuro/negro azulado, cyan, turquesa, violeta, verde y naranja (el
> rojo solo para alertas), tipografía tipo terminal y estética pixel art. Reduce el texto visible a
> **4–5 elementos resumidos por bloque**; el resto va al modal. Debe ser **responsive** y accesible.
>
> Organiza las imágenes en `img/` con subcarpetas: `personajes`, `usuarios`, `propuesta`, `canales`,
> `relacion`, `actividades`, `recursos`, `socios`, `costos`, `sostenibilidad` y `galeria`.
>
> Entrega dentro de `docs/business-model-canvas/` con `index.html`, `README.md`, `prompt-final.md`,
> `css/styles.css`, `js/canvas.js`, la carpeta `img/` y la carpeta `evidencias/` (`canvas-v1.png` y
> `canvas-final.png`).

## Criterios aplicados

1. Disposición clásica del lienzo en cinco columnas y fila inferior de costos e ingresos.
2. Cada bloque muestra una imagen, icono, número, título, 4–5 tarjetas resumidas y botón "Ver detalle".
3. Al hacer clic en un bloque se abre una **ficha (modal)** con descripción, métricas, secciones,
   notas, galería y etiquetas.
4. Al pasar el cursor sobre las tarjetas se muestran **tooltips** con información ampliada.
5. Se dibujan **conexiones visuales** (SVG) entre componentes relacionados; el hover resalta las
   conexiones y atenúa los bloques no relacionados.
6. La **propuesta de valor** es el bloque central más grande y destacado.
7. Diseño **responsive**: 5 columnas en escritorio, 2 en tablet y 1 en móvil.
8. Rutas relativas y recursos estáticos para funcionar en GitHub Pages.

## Verificación

- La rama de trabajo es `documentacion`.
- Todos los archivos están dentro de `docs/business-model-canvas/`.
- El Canvas contiene exactamente nueve componentes e imágenes.
- No aparece ningún personaje fuera de la lista oficial (Alex, Martín, Richar, Marco, Sofía y el
  Saboteador).
- No se agregó Godot ni PostgreSQL; se conservan Unity, Flutter, React/PWA, Keycloak, MongoDB,
  Docker y Grafana.
- Los ingresos describen una etapa académica sin monetización directa; solo se listan posibilidades
  futuras.
