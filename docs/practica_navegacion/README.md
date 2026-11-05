# TI: Soporte en Acción — visor interactivo UX/UI

Visor estático de documentación para las 38 pantallas del proyecto. Es compatible con GitHub Pages y no modifica Unity, Flutter, React/Vite ni servicios de la aplicación.

## Recursos verificados

El manifest `js/image-manifest.js` centraliza las rutas relativas a los recursos originales:

- `../imagenes/Sketches/`: 38 sketches (`{ID}_sketch.png`).
- `../imagenes/Wireframes/`: 38 wireframes (`{ID}_wireframe.png`).
- `../imagenes/Comparaciones/`: 38 comparaciones (`{ID}_comparacion.png`).

Los 114 archivos se consultan sin recortarse ni transformarse. Si un recurso falta al publicar, el visor indica su ruta y no utiliza sustitutos.

## Funciones

- Navegador lateral con búsqueda, filtro por módulo, secciones plegables y las 38 pantallas.
- Visor central para Sketch, Wireframe, Comparación y lado a lado; zoom, arrastre, ajuste, tamaño original, descarga, pantalla completa y navegación anterior/siguiente.
- Ficha conectada por pantalla con roles, plataforma, acciones, requisitos, reglas y conexiones navegables.
- Diagrama general y filtrado por rol; sus nodos abren la pantalla seleccionada.
- Simulador local de recorrido autorizado por rol, con regreso, reinicio y demostración de acceso denegado.

## Publicación

Sirve el directorio `docs/` y visita `practica_navegacion/`. Todas las rutas son relativas, por lo que funcionan bajo el prefijo del repositorio de GitHub Pages. Para probarlo localmente: `python -m http.server 8000 --directory docs`.

La autorización del simulador es demostrativa; la validación real continúa siendo responsabilidad de la API.

### Enlace de GitHub Pages

Cuando GitHub Pages esté habilitado para la rama de publicación y la carpeta `/docs`, el visor estará disponible en [GitHub Pages](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica_navegacion/).
