# 07 · Elaboración, herramientas, decisiones y bitácora

## Herramientas y estado

| Herramienta | Función en la práctica | Estado de este paquete |
|---|---|---|
| Bocetos PNG | Representar 20 pantallas móviles de baja fidelidad | Archivos incluidos |
| Flutter | Plataforma objetivo de la app complementaria Android/iOS | **Diseño**, no desarrollo |
| HTML/CSS/JavaScript | Prototipo local de navegación y selección de sketches | Archivo incluido y apto para inspección local |
| Mermaid / Graphviz | Diagrama general del flujo con condiciones | Archivos fuente / SVG incluidos |
| Archify | Herramienta requerida por la asignatura para reconstruir el diagrama interactivo | **Pendiente de ejecutar en Archify**; se adjunta prompt |
| Git / GitHub | Versionamiento y entrega | Instrucciones incluidas; no se ha publicado desde esta carpeta |

## Decisiones aplicadas

1. Toda pantalla se representa como un **teléfono**, incluso los módulos de docente, administrador, editor e institución.
2. P01–P09 satisfacen las nueve pantallas generales. P10 es registro y P11–P20 son funcionalidades específicas; hay 11 adicionales.
3. Las pantallas se denominan P01–P20 y cada una tiene una ruta conceptual y un PNG independiente.
4. La pestaña inferior se mantiene para el estudiante; otros roles pueden tener menú contextual diferente.
5. Para no superar 20 bocetos, **P06** se reutiliza como verificación de código de recuperación/desbloqueo, según el flujo.
6. La **Home P09** admite variantes de contenido por rol; el sketch de referencia muestra al estudiante.
7. El videojuego principal se conserva en Unity PC; las misiones del teléfono sirven para consulta y seguimiento.
8. Los estados de error se representan dentro de su pantalla (validación de registro, credenciales incorrectas, código expirado, acceso denegado), evitando pantallas artificiales.

## Observaciones de calidad

- Hay sketches móviles de origen provistos por el estudiante y bocetos complementarios generados para completar el inventario.
- Algunas ilustraciones muestran nombres, puntajes y recompensas de **ejemplo**; no representan datos reales.
- La pantalla P17 muestra progreso; no otorga automáticamente privilegios para ver grupos no asignados.
- El HTML es un **simulador de rutas**, no comprueba credenciales ni conecta con bases de datos.
- Las transiciones de la matriz son rutas de diseño; la API y el proveedor de identidad deben ratificar la seguridad.

## Bitácora para completar tras trabajar con Archify

| Fecha | Acción / prompt | Resultado | Corrección | Evidencia |
|---|---|---|---|---|
| Por registrar | Importar estructura y PNG desde este paquete | Pendiente | Pendiente | E03 |
| Por registrar | Probar recuperación y bloqueo | Pendiente | Pendiente | E04 |
| Por registrar | Probar flujo misiones y guías | Pendiente | Pendiente | E05 |
| Por registrar | Probar filtrado por roles | Pendiente | Pendiente | E06 |
| Por registrar | Revisar con el equipo y documentar ajustes | Pendiente | Pendiente | E07 |

Los prompts sugeridos se encuentran en [`08_prompt_archify.md`](08_prompt_archify.md). Editar esta bitácora con datos reales en cuanto se valide el trabajo.


## Nueva iteración de diseño visual · 20 octubre 2026

Se decidió aplicar a los diseños móviles la identidad de TI: Soporte en Acción: azul oscuro, cian, turquesa, violeta, verde y naranja. Los **20 sketches a lápiz no se modificaron** para conservar su calidad como evidencia de baja fidelidad. Se crearon **20 mockups a color adicionales** en `mockups-color/`, se agregó navegación demostrativa en HTML y se actualizó el mapa de navegación para consultar ambas variantes por pantalla. Esta iteración **no valida** flujos con backend ni constituye ejecución dentro de Archify; esas evidencias siguen pendientes. Véase `docs/10_identidad_visual_flutter.md`.
