# Roles, permisos y flujos

| Rol | Operaciones autorizadas | Pantallas | No autorizado |
|---|---|---|---|
| Jugador / Estudiante | jugar, consultar progreso e inventario, editar perfil | P10–P20 | usuarios, roles, resultados de terceros |
| Administrador | gestionar cuentas y permisos autorizados | ADM01–ADM03 | alterar resultados historicos |
| Docente | consultar grupos y asignar mision | DOC01–DOC03 | roles y parametros globales |
| Evaluador | evaluar evidencias y consultar resultados | EVA01–EVA03 | datos no autorizados |
| Editor de contenido | editar materiales y recursos | EDI01–EDI03 | cuentas y permisos |
| Soporte tecnico | atender tickets y consultar ayuda | SOP01–SOP03 | XP, resultados y privilegios |

Todos los recorridos parten de P03 tras autenticacion, contemplan P05–P07 para recuperacion/bloqueo y retornan a P03 al cerrar sesion. Los diagramas independientes estan en `../diagramas/roles/`.
