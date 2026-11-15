# Dashboard Gantt basado en Git

Este directorio documenta y visualiza el historial real de **TI: Soporte en Acción**. No modifica el videojuego, sus proyectos ni su historial.

## Abrir el dashboard

Desde la raíz del repositorio, actualiza primero el conjunto de datos y sirve la carpeta por HTTP:

```powershell
./docs/gantt-git/scripts/actualizar-gantt.ps1
python -m http.server 8080 --directory docs/gantt-git
```

Después abre `http://localhost:8080`. Servirlo por HTTP es la forma recomendada para cargar y recargar los JSON separados.

El dashboard también incluye respaldos generados `datos/gantt-git.js` y `datos/equipo-proyecto.js`. Por eso puede abrirse directamente con `file://` cuando no sea posible usar un servidor; HTTP sigue siendo la forma recomendada para que el botón **Recargar datos** consulte los JSON actualizados.

## Estructura

| Ruta | Responsabilidad |
| --- | --- |
| `scripts/actualizar-gantt.ps1` | Consulta Git en modo lectura y regenera los datos. |
| `datos/gantt-git.json` | Dataset procesado: commits, ramas, archivos, actividades y agrupaciones. |
| `datos/equipo-proyecto.json` | Integrantes oficiales, vínculos Git verificados y asignaciones explícitas. |
| `index.html`, `styles/`, `scripts/gantt.js` | Dashboard Gantt interactivo. |
| `archify-flujo-gantt.workflow.json` | Especificación Archify del flujo de evidencia. |
| `archify-flujo-gantt.html` | Visualización Archify generada a partir de la especificación. |

## Qué representa cada elemento

Cada actividad visual es un **commit Git real**. Su posición corresponde exactamente a `authoredAt`; se dibuja como un marcador de ancho mínimo solamente para que se pueda seleccionar. Por ello:

- No expresa duración: el propio detalle declara que es un punto temporal.
- No expresa porcentaje, estado ni avance: esos datos no existen en el historial Git y no se estiman.
- Módulo y fase son clasificaciones derivadas. El módulo se obtiene de las rutas modificadas y la fase del prefijo/mensaje del commit (`feat`, `fix`, `docs`, `chore`, etc.).
- Autores, ramas, hashes, fechas, asunto y archivos modificados proceden de Git. Cuando existe un remoto web compatible, el hash conserva su enlace a `/commit/<hash>`.

La vista actual agrupa commits en actividades derivadas solo cuando las rutas o mensajes aportan evidencia: configuración, animaciones y recursos Unity, sistemas de misiones, escenarios y niveles, personajes y movimiento, interfaces, aplicación móvil, PWA/autenticación, monitoreo, infraestructura y documentación/diseño. Una actividad con varios días muestra un **periodo observado** desde su primer hasta su último commit; una actividad de un solo día se muestra como **hito**. Ninguna barra representa duración planificada ni avance.

El alcance es `git log --branches --remotes`: incluye commits alcanzables desde ramas locales y remotas, y excluye stashes y cambios sin confirmar. Las ramas de un commit se calculan al regenerar el dataset; un commit histórico puede no pertenecer a una punta de rama actual y mostrará “Sin rama actual”.

## Equipo y responsabilidad

`datos/equipo-proyecto.json` declara como integrantes oficiales únicamente a Brian Jesus Mendoza Marquez (autor principal) e Idai Vargas Galindo (colaboradora). La identidad Git se considera vinculada solo cuando un par exacto de `name` y `email` aparece en `verifiedGitIdentities`; una coincidencia de nombre aislada nunca asigna contribuciones.

La responsabilidad de una actividad no se infiere del autor del commit. Para declararla, agrega de forma explícita su identificador a `activityAssignments`:

```json
{
  "activityAssignments": {
    "commit-HASH_COMPLETO": "brian-mendoza",
    "commit-OTRO_HASH": "idai-vargas",
    "commit-HASH_COMPARTIDO": "both"
  }
}
```

Los valores permitidos son `brian-mendoza`, `idai-vargas`, `both` y `unassigned`. Sin una entrada explícita se muestra “Sin responsable asignado”.

## Funciones disponibles

- Zoom temporal por días, semanas o meses.
- Filtros por desarrollador, rama, módulo y rango de fechas.
- Selección de actividades con detalle del commit, hash, enlace, autor, fecha, ramas y lista de archivos modificados.
- Resumen de commits, módulos, autores y ramas visibles.
- Recarga de `datos/gantt-git.json` después de ejecutar el extractor.
- Exportación del Gantt visible a PNG y uso de **Imprimir / PDF** para la exportación PDF del navegador.
- Mapa complementario Archify para auditar el flujo Git → extractor → datos → Gantt.

## Actualización ante nuevos commits

1. Incorpora o descarga los commits normalmente con Git.
2. Ejecuta `./docs/gantt-git/scripts/actualizar-gantt.ps1` desde la raíz.
3. Pulsa **Recargar datos** en el dashboard.

El script solo usa comandos de consulta (`git log`, `git branch`, `git for-each-ref`, `git config`, `git rev-parse`). Su única escritura es el archivo procesado `datos/gantt-git.json`.

## Limitaciones transparentes

- Git no contiene duración planificada ni porcentaje de avance; el dashboard no los fabrica.
- La clasificación no reescribe mensajes ni cambia hashes: facilita el filtro visual, pero no sustituye una taxonomía de proyecto formal.
- La exportación PNG toma el Gantt renderizado. La exportación PDF usa el diálogo nativo para mantener un PDF fiel al navegador.
