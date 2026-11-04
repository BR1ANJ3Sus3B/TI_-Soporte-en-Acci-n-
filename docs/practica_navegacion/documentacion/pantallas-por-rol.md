# Pantallas especificas por rol

Se localizaron 15 sketches adicionales, tres por carpeta. No se localizaron wireframes tecnicos para estas pantallas; el prototipo muestra el estado pendiente sin reutilizar recursos de otras pantallas.

| Rol | IDs | Pantallas | Restriccion principal |
|---|---|---|---|
| Administrador | ADM01–ADM03 | Panel, usuarios, configuracion | No modifica resultados historicos |
| Docente | DOC01–DOC03 | Panel, asignar mision, avance | No cambia roles ni configuracion general |
| Editor de contenido | EDI01–EDI03 | Contenido, editar mision, recursos | No administra cuentas ni roles |
| Evaluador | EVA01–EVA03 | Panel, evaluar, resultados | Solo informacion academica autorizada |
| Soporte tecnico | SOP01–SOP03 | Centro, ticket, conocimiento | No cambia XP, resultados ni privilegios |

| ID | Descripcion / accion principal | Anterior y siguiente | Sketch | Wireframe |
|---|---|---|---|---|
| ADM01 | Panel: consultar usuarios, roles y misiones | P03; ADM02/ADM03 | `skeches/Administrador/01_Panel_administracion.png` | Pendiente |
| ADM02 | Gestion de usuarios: buscar y editar autorizado | ADM01; ADM03 | `skeches/Administrador/02_Gestion_usuarios.png` | Pendiente |
| ADM03 | Configuracion: consultar parametros y permisos | ADM01; ADM01 | `skeches/Administrador/03_Configuracion_general.png` | Pendiente |
| DOC01 | Panel: consultar grupos y estudiantes | P03; DOC02/DOC03 | `skeches/Docente/01_Panel_docente.png` | Pendiente |
| DOC02 | Asignar una mision al grupo autorizado | DOC01; DOC01/DOC03 | `skeches/Docente/02_Asignar_mision.png` | Pendiente |
| DOC03 | Consultar avance del grupo | DOC01; DOC01/DOC02 | `skeches/Docente/03_Avance_grupo.png` | Pendiente |
| EDI01 | Consultar materiales educativos | P03; EDI02/EDI03 | `skeches/Editor_contenido/01_Contenido_educativo.png` | Pendiente |
| EDI02 | Editar contenido y guardar cambios | EDI01; EDI01/EDI03 | `skeches/Editor_contenido/02_Editar_mision.png` | Pendiente |
| EDI03 | Organizar materiales y recursos | EDI01; EDI01 | `skeches/Editor_contenido/03_Gestion_recursos.png` | Pendiente |
| EVA01 | Consultar evaluaciones pendientes | P03; EVA02/EVA03 | `skeches/Evaluador/01_Panel_evaluacion.png` | Pendiente |
| EVA02 | Revisar evidencia y registrar evaluacion | EVA01; EVA01/EVA03 | `skeches/Evaluador/02_Evaluar_mision.png` | Pendiente |
| EVA03 | Consultar resultados autorizados | EVA01; EVA01/EVA02 | `skeches/Evaluador/03_Resultados.png` | Pendiente |
| SOP01 | Consultar tickets | P03; SOP02/SOP03 | `skeches/Soporte_tecnico/01_Centro_soporte.png` | Pendiente |
| SOP02 | Actualizar seguimiento de incidente | SOP01; SOP01/SOP03 | `skeches/Soporte_tecnico/02_Detalle_ticket.png` | Pendiente |
| SOP03 | Buscar soluciones documentadas | SOP01; SOP01/SOP02 | `skeches/Soporte_tecnico/03_Base_conocimiento.png` | Pendiente |

Las condiciones para las 15 filas son sesion autenticada y el rol indicado en la primera tabla. Sus elementos principales son encabezado, contenido del modulo y acciones autorizadas; los requisitos y restricciones se conservan individualmente en `../js/roles-data.js`.

## ADM01–ADM03 — Administrador

ADM01 muestra usuarios, roles, misiones y estadisticas; conduce a ADM02 y ADM03. ADM02 consulta y edita cuentas autorizadas. ADM03 consulta parametros, permisos y seguridad. Requieren rol Administrador y se relacionan con RF-05/RF-20.

## DOC01–DOC03 — Docente

DOC01 consulta grupos, estudiantes, misiones y reportes; conduce a DOC02 y DOC03. DOC02 asigna una mision al grupo autorizado. DOC03 consulta el avance del grupo. Requieren rol Docente y usan RF-06/RF-14.

## EDI01–EDI03 — Editor de contenido

EDI01 consulta misiones, guias y recursos; conduce a EDI02 y EDI03. EDI02 edita una mision autorizada. EDI03 organiza materiales. Requieren rol Editor de contenido y se asocian a RF-16.

## EVA01–EVA03 — Evaluador

EVA01 lista evaluaciones; conduce a EVA02 y EVA03. EVA02 revisa evidencia y registra retroalimentacion. EVA03 consulta resultados autorizados. Requieren rol Evaluador y se relacionan con RF-14.

## SOP01–SOP03 — Soporte tecnico

SOP01 consulta tickets; conduce a SOP02 y SOP03. SOP02 revisa y actualiza seguimiento. SOP03 consulta soluciones. Requisito especifico pendiente de formalizacion; se conservan las restricciones del rol.
