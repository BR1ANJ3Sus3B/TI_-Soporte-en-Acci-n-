# Práctica — Navegación de interfaces

## Introducción, objetivo y contexto

Esta práctica documenta el recorrido de 20 interfaces de **TI: Soporte en Acción**, videojuego educativo situado en NovaTech Solutions. Su objetivo es permitir revisar bocetos, wireframes, roles, permisos y rutas sin modificar Unity, Flutter, React/Vite ni servicios. El visor estático usa HTML, CSS y JavaScript y puede publicarse desde `docs/` en GitHub Pages.

## Tecnologías y requerimientos

El proyecto contempla Unity, Flutter, React/Vite PWA, MongoDB, Keycloak, Docker/Docker Compose, Grafana y GitHub Pages. Las referencias utilizadas proceden de `docs/proyecto/README.md`: RF-01 a RF-21 y RNF-08/RNF-11 cuando aplican. Las relaciones marcadas como pendientes no inventan requisitos.

## Inventario y correspondencia

Los originales permanecen en `../imagenes/skeches/` y `../imagenes/wirefremes/`. El manifiesto web contiene 20 pares seleccionados mediante inspección visual y secuencia. Se detectaron **21 sketches y 20 wireframes**: el archivo `Imagen de ChatGPT 3 nov 2026, 07_47_11 p.m-3.png` es una variante de Login P03 no seleccionada, pues `6.png` se usa como su correspondiente principal. No se borró, movió ni renombró ningún original.

## Roles y permisos

| Rol | Módulo | Operación | Pantalla | Permiso | Restricción |
|---|---|---|---|---|---|
| Público | Autenticación | iniciar/registrar/recuperar | P03–P07 | Permitido | registro crea jugador |
| Jugador / Estudiante | Misiones y gamificación | consultar y jugar | P10–P20 | Permitido | sólo resultados propios |
| Docente / Evaluador | Progreso | consultar | P17 | Propuesto | sólo resultados autorizados; interfaz específica pendiente |
| Editor de contenido | Contenido | administrar | — | Pendiente | no hay pantalla entre los 20 recursos |
| Administrador | Cuentas/misiones | gestionar | — | Pendiente | RF-20; interfaz propia pendiente |
| Institución educativa | Resultados | consultar | — | Pendiente | interfaz institucional pendiente |

## Reglas de navegación

P01→P02 ocurre automáticamente. P03 dirige a registro, recuperación, bloqueo o Home con credenciales válidas. P06→P07 requiere solicitud y P07→P03 requiere código válido. P10 exige autenticación; P14 y P15 pertenecen a Unity. El cierre de sesión retorna a P03. Facebook, Google e Instagram aparecen en P03 sólo como opciones propuestas, no como proveedores configurados.

## Fichas: sketches y wireframes

Cada ficha se consulta en la página y contiene el enlace real, descripción, plataforma, rol, elementos, acciones, conexiones y requisito asociado.

| ID | Pantalla | Sketch / Wireframe | Plataforma | Rol | Siguiente |
|---|---|---|---|---|---|
| P01 | Splash Screen | originales | Flutter/PWA | Público | P02 |
| P02 | Loading Screen | originales | Flutter/PWA | Público | P03 |
| P03 | Login Screen | originales | Flutter/PWA | Público | P04/P05/P06/P10 |
| P04 | Registro | originales | Flutter/PWA | Público | P03 |
| P05 | Recovery Account | originales | Flutter/PWA | Público | P03 |
| P06 | Lock Account | originales | Flutter/PWA | Público | P07/P03 |
| P07 | Unlock Account Code | originales | Flutter/PWA | Público | P03 |
| P08 | About Software | originales | Flutter/PWA | Público | P03/P10 |
| P09 | About Team / Enterprise | originales | Flutter/PWA | Público | P03/P10 |
| P10 | Home | originales | Flutter/PWA | Jugador | módulos |
| P11 | Dashboard | originales | Flutter/PWA | Jugador | P12/P16/P17 |
| P12 | Mapa de NovaTech | originales | Flutter/PWA | Jugador | P13 |
| P13 | Detalle de misión | originales | Flutter/PWA | Jugador | P14 |
| P14 | Juego / Misión | originales | Unity | Jugador | P15/P17 |
| P15 | Diálogo / Historia | originales | Unity | Jugador | P14/P17 |
| P16 | Inventario | originales | Unity/Flutter/PWA | Jugador | P10/P11 |
| P17 | Progreso | originales | Unity/Flutter/PWA | Jugador/Docente | P18 |
| P18 | Logros | originales | Unity/Flutter/PWA | Jugador | P10 |
| P19 | Configuración | originales | Flutter/PWA | Jugador | P10 |
| P20 | Perfil | originales | Flutter/PWA | Jugador | P19 |

## Ampliacion por roles

Se integraron 15 sketches nuevos: ADM01–ADM03, DOC01–DOC03, EDI01–EDI03, EVA01–EVA03 y SOP01–SOP03. Cada uno tiene ruta real, rol, restricciones, acciones y conexiones en `js/roles-data.js`; no tienen wireframe localizado, por lo que el visor declara su estado pendiente. Consulte `documentacion/pantallas-por-rol.md` y `documentacion/roles-permisos.md`.

## Diagrama, simulación y Archify

La fuente Mermaid está en `diagramas/navegacion-general.mmd` y los seis recorridos por rol están en `diagramas/roles/`. El sitio incluye filtros, selección de nodos, explorador de roles, visor alternable/comparativo, ampliación, enlaces a imagen completa, anterior/siguiente y simulador de recorridos con datos de prueba. Los insumos de Archify están en `diagramas/archify/`; no se ejecutó Archify.

## Validaciones ejecutadas

- Conteo real: 21 sketches, 20 wireframes; 20 pares incorporados y una variante P03 registrada.
- Dimensiones leídas: sketches P11–P20 1000×1600, restantes 1024×1536; wireframes 1086×1448 o 1024×1536.
- Se verificó por inspección visual la correspondencia de los sketches de P01–P10 y por nombres P11–P20.
- Pendiente: publicar en GitHub Pages, prueba manual en navegadores y ejecución futura de Archify.

## Uso local

Abra `index.html` mediante un servidor estático (por ejemplo, el servidor de su editor); las rutas relativas a `../imagenes/` se resuelven desde `docs/practica_navegacion/`. Para GitHub Pages, publique `docs/` y abra `/practica_navegacion/`.

## Conclusión

La práctica preserva los recursos originales y deja un prototipo navegable, trazable a requisitos existentes y preparado para revisión académica.
