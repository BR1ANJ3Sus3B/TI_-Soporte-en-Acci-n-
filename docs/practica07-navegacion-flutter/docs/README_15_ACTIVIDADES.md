# Guía de cumplimiento · Actividades 1 a 15

Esta práctica fue estructurada **solo para la app móvil Flutter** del Proyecto Integrador TI: Soporte en Acción. **No incluye pantallas PWA ni propuestas de diseño para React/Vite.**

## Correspondencia entre actividad y evidencia

| Nº | Actividad | Contenido agregado | Archivo | Estado |
|---|---|---|---|---|
| 1 | Analizar requerimientos | Análisis de actores, RF/RNF, historias, reglas y distinción móvil/Unity PC. | [01_analisis_requerimientos.md](01_analisis_requerimientos.md) | Incluido |
| 2 | Identificar roles y permisos | Matriz de los cinco roles, visitantes y restricciones por módulo. | [02_roles_y_permisos.md](02_roles_y_permisos.md) | Incluido |
| 3 | Definir navegación | Entradas, retornos y Home variable por rol. | [03_arquitectura_navegacion.md](03_arquitectura_navegacion.md) | Incluido |
| 4 | Elaborar nueve sketches generales | P01–P09 en PNG con estilo de lápiz. | [../sketches/](../sketches/) | Incluido |
| 5 | Diseñar once sketches adicionales | P10–P20: registro, funcionalidades del estudiante y módulos por rol. | [../sketches/](../sketches/) | Incluido |
| 6 | Representar componentes visuales | Campos, tarjetas, navegación, botones y estados en los PNG y fichas. | [04_fichas_20_sketches.md](04_fichas_20_sketches.md) | Incluido |
| 7 | Documentar los veinte | Ficha por ID con descripción, plataforma, rol, componentes, acciones, rutas y requisitos. | [04_fichas_20_sketches.md](04_fichas_20_sketches.md) | Incluido |
| 8 | Definir transiciones y condiciones | Matriz con 51 transiciones, condiciones de error, código, bloqueo, login y salida. | [05_matriz_transiciones.md](05_matriz_transiciones.md) | Incluido |
| 9 | Dibujar diagrama general | Fuentes Mermaid y SVG exportado con veinte nodos. | [../diagramas/navegacion_estatica.svg](../diagramas/navegacion_estatica.svg) | Incluido |
| 10 | Construir con Archify | Prompt íntegro para reproducir, explorar y validar el diagrama desde Archify. | [08_prompt_archify.md](08_prompt_archify.md) | **Pendiente en Archify** |
| 11 | Vincular sketches | Prototipo HTML: cada nodo enlaza a su PNG individual. | [../prototipo/index.html](../prototipo/index.html) | Incluido |
| 12 | Validar recorridos | 26 pruebas con estado pendiente; selector de escenarios para ensayo local. | [06_plan_validacion.md](06_plan_validacion.md) | **Pendiente de pruebas reales** |
| 13 | Documentar proceso | Herramientas, decisiones, observaciones y plantilla de bitácora. | [07_proceso_decisiones.md](07_proceso_decisiones.md) | Incluido |
| 14 | Integrar en GitHub | Estructura y comandos de rama, commit, pull request y enlaces relativos. | [09_guia_github_entrega.md](09_guia_github_entrega.md) | **Pendiente de subir** |
| 15 | Revisar y entregar | Lista de revisión, pruebas y evidencias no simuladas. | [09_guia_github_entrega.md](09_guia_github_entrega.md) | **Pendiente de pruebas reales** |

## Distribución de pantallas

**9 generales (P01–P09)**: Splash, Loading, Login, Recuperar cuenta, Cuenta bloqueada, Código, Acerca del software, Acerca del equipo, Home.

**11 adicionales (P10–P20)**: Registro, Misiones, Detalle de misión, Guías, Lectura de guía, Progreso, Perfil, Panel docente, Gestión de usuarios, Editor de misiones, Reportes institucionales.

## Límites explícitos

- El prototipo visual no sustituye una aplicación Flutter terminada.
- El HTML navegable local no es un producto creado por Archify; se proporciona como **referencia funcional** para reproducirlo con Archify.
- Las pruebas contra servicios, la ejecución en Archify y la entrega a GitHub requieren evidencia humana; no se declaran realizadas.
- El acceso real del usuario y la autorización están sujetos a reglas backend; en el HTML se simulan los recorridos.

## Inicio rápido

1. Abrir [`../index.html`](../index.html) para ver los 20 sketches.
2. Abrir [`../prototipo/index.html`](../prototipo/index.html) y seleccionar rol/escenario.
3. Consultar [`04_fichas_20_sketches.md`](04_fichas_20_sketches.md) para cada interfaz.
4. Copiar el prompt de [`08_prompt_archify.md`](08_prompt_archify.md) y adjuntar el material a Archify.
5. Completar las pruebas de [`06_plan_validacion.md`](06_plan_validacion.md) y entregar mediante GitHub.
