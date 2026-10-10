# Cobertura documental

Este documento verifica que el contenido del paquete de documentación de "TI: Soporte en Acción" fue incorporado, y registra qué se conservó, qué se adaptó y qué queda pendiente.

## Fuente

- Documento de origen: `README.md` proporcionado por el autor (47,799 bytes), transcripción del documento Word del proyecto.
- Imágenes de origen: `docs/imagenes/` del paquete. Las 21 imágenes se recuperaron del documento Word `TI_Soporte_En_Accion_Proyecto_Integrador_Brian (1).docx` (21 elementos en `word/media`) porque la carpeta `docs/imagenes/` no estaba presente en el equipo.
- Destino del documento completo: [`docs/proyecto/README.md`](README.md).
- Un cambio de rutas: enlaces de imágenes de `docs/imagenes/...` a `../imagenes/...`, para que resuelvan desde `docs/proyecto/`.

## Secciones incorporadas

| Sección de origen | Destino | Estado |
| --- | --- | --- |
| Propuesta de integración académica (tabla de asignaturas, objetivos) | `proyecto/README.md` | Incorporada |
| Funciones de cada aplicación y ejemplo de uso integrado | `proyecto/README.md` | Incorporada |
| Roles de usuario y permisos | `proyecto/README.md` | Incorporada |
| Arquitectura y datos compartidos | `proyecto/README.md` | Incorporada |
| Alcance inicial y plan individual (etapas, criterios de aceptación, ampliaciones) | `proyecto/README.md` | Incorporada |
| Requerimientos funcionales (RF-01 a RF-11) | `proyecto/README.md` | Incorporada (RF-01 literal) |
| Requerimientos de progreso y administración (RF-12 a RF-21) | `proyecto/README.md` | Incorporada |
| Requerimientos no funcionales (RNF-01 a RNF-07) | `proyecto/README.md` | Incorporada |
| Requerimientos de operación y mantenimiento (RNF-08 a RNF-14) | `proyecto/README.md` | Incorporada |
| Requerimientos para una segunda etapa (RF-22 a RF-26) | `proyecto/README.md` | Incorporada, marcada como futura |
| Diseño del videojuego (introducción, dinámica, público, género) | `proyecto/README.md` | Incorporada |
| Historia, personaje principal, enemigo y secundarios | `proyecto/README.md` | Incorporada |
| Niveles, storyboard y Canvas | `proyecto/README.md` | Incorporada |
| Plan de monetización | `proyecto/README.md` | Incorporada |
| Organización del trabajo individual | `proyecto/README.md` | Incorporada |
| Bocetos de la PWA (9 pantallas) | `proyecto/README.md` | Incorporada |
| Paleta de colores | `proyecto/README.md` | Incorporada (complemento de la conversación) |
| Uso de este paquete | `proyecto/README.md` | Incorporada y adaptada |

## Requerimientos

| Grupo | IDs | Cantidad | Estado |
| --- | --- | --- | --- |
| Funcionales del alcance inicial | RF-01 a RF-21 | 21 | Incorporados |
| Funcionales de segunda etapa | RF-22 a RF-26 | 5 | Incorporados, marcados como futuros |
| No funcionales | RNF-01 a RNF-14 | 14 | Incorporados |

- RF-01 se conserva de forma literal: «El sistema debe permitir el registro de usuarios mediante campos obligatorios, sin duplicación de datos.»
- Los valores numéricos de RNF-04, RNF-05 y RNF-06 se mantuvieron sin cambios y se presentan como metas a validar, no como resultados de prueba.
- Los requisitos se distinguen entre funciones previstas (alcance inicial), propuestas y ampliaciones futuras.

## Imágenes

Las 21 imágenes se incorporaron en [`docs/imagenes/`](../imagenes/) y se enlazan desde `proyecto/README.md`. El detalle está en [`inventario-imagenes.json`](../inventario-imagenes.json).

| Imágenes | Sección | Estado |
| --- | --- | --- |
| 01-02 | Público objetivo | Enlazadas |
| 03-08 | Personajes (Alex, Saboteador, Martín, Richar, Sofía, Marco) | Enlazadas |
| 09-11 | Storyboard de niveles | Enlazadas |
| 12 | Canvas | Enlazada |
| 13-21 | Bocetos de la PWA (9 pantallas) | Enlazadas |
| movil1-21 y 6.png | Bocetos de la aplicación móvil (21 pantallas, incluidas por orden) | Enlazadas |

Notas de las imágenes:

- Son material conceptual; no son recursos jugables listos para importar sin revisión.
- Las hojas de sprite conservan rótulos de Godot de una versión anterior; el motor definido para el proyecto es Unity.
- La imagen del segundo nivel corresponde al taller de hardware en tercera persona/tercer nivel de la descripción del taller; se anotó esa aclaración en la sección de storyboard, sin reinterpretar la ilustración como recurso de producción.
- Los bocetos móviles se incorporaron tal como se entregaron. `movil14` a `movil21` son idénticas a las pantallas de la PWA (`13-pantalla-1` a `21-pantalla-9`), `movil1` es idéntica a `movil2` y `6.png` es idéntica a `movil7`; se conservan por indicación del autor. Falta el archivo original `movil6.png`; su lugar lo ocupa `6.png`.

## Aclaraciones mantenidas

- No se atribuye autoría a Jennifer en ninguna parte del documento.
- Se distingue entre requisitos, propuestas, ampliaciones futuras y funciones implementadas.
- No se inventaron configuraciones de Unity ni de Flutter; solo se documenta el alcance y las tecnologías.
- El precio de monetización se marca explícitamente como propuesta futura, no implementada.

## Pendientes

Las carpetas `requerimientos/`, `historias-de-usuario/`, `arquitectura/`, `diseno/`, `pruebas/` y `manuales/` permanecen vacías (con `.gitkeep`). Su contenido detallado se derivará de `proyecto/README.md` conforme avance el proyecto; no se duplica aquí para evitar divergencias.

El archivo `PROMPT_OPENCODE.md` mencionado en el paquete original no forma parte de este repositorio.
