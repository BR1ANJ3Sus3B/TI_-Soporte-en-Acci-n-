# 06 · Plan de validación y evidencias

**Estado:** casos definidos para ejecutar. No se afirma que Archify, autenticación, Keycloak, API ni Flutter estén probados en un entorno real.

## Casos de prueba

| ID | Escenario | Recorrido de referencia | Resultado esperado | Rol | Estado |
|---|---|---|---|---|---|
| T01 | Arranque, sin sesión | P01 → P02 → P03 | Login visible, sin contenido privado | Todos | Pendiente |
| T02 | Arranque con sesión válida | P01 → P02 → P09 | Home acorde al rol actual | Todos | Pendiente |
| T03 | Login correcto estudiante | P03 → P09 | Home de estudiante | Estudiante | Pendiente |
| T04 | Login incorrecto | P03 → P03 | Error neutro, sin conceder acceso | Todos | Pendiente |
| T05 | Fallos consecutivos | P03 → P05 | Bloqueo al llegar al umbral configurable | Todos | Pendiente |
| T06 | Solicitar recuperación | P03 → P04 → P06 | Código solicitado, sin filtrar existencia de cuenta | Todos | Pendiente |
| T07 | Código correcto | P06 → P03 | Se habilita volver a ingresar según flujo | Todos | Pendiente |
| T08 | Código inválido | P06 → P06 | Mensaje de error sin abrir el área protegida | Todos | Pendiente |
| T09 | Cuenta bloqueada | P05 → P06 → P03 | Desbloqueo requiere validación | Todos | Pendiente |
| T10 | Registro válido | P03 → P10 → P03 | No duplica correo | Estudiante | Pendiente |
| T11 | Registro inválido | P10 → P10 | Muestra correcciones pendientes | Estudiante | Pendiente |
| T12 | Acerca del proyecto | P03 → P07 → P08 → P07 | Informaciones accesibles y regreso disponible | Todos | Pendiente |
| T13 | Misión disponible | P09 → P11 → P12 | Detalle con objetivos y XP | Estudiante | Pendiente |
| T14 | Misión bloqueada | P11 → P12 | Se muestran requisitos; no se habilita inicio en Unity desde móvil | Estudiante | Pendiente |
| T15 | Guía vinculada | P12 → P13 → P14 → P13 | Se conserva retorno al catálogo | Estudiante | Pendiente |
| T16 | Mi progreso | P09 → P15 | Indicadores propios y opciones de retorno | Estudiante | Pendiente |
| T17 | Perfil y cierre de sesión | P09 → P16 → P03 | Sesión cerrada; Atrás no reabre contenido | Todos | Pendiente |
| T18 | Docente autorizado | P03 → P09 → P17 | Sólo seguimiento del grupo autorizado | Docente | Pendiente |
| T19 | Administrador autorizado | P03 → P09 → P18 | Sólo acciones administrativas autorizadas | Administrador | Pendiente |
| T20 | Editor autorizado | P03 → P09 → P19 | Guarda sólo borradores de sus misiones | Editor | Pendiente |
| T21 | Institución autorizada | P03 → P09 → P20 | Sólo datos consolidados autorizados | Institución | Pendiente |
| T22 | Estudiante intenta /admin/usuarios | P09 → aviso en P09 | Denegado: sin acceso a P18 | Estudiante | Pendiente |
| T23 | Docente intenta /editor/misiones | P09 → aviso en P09 | Denegado: sin acceso a P19 | Docente | Pendiente |
| T24 | Institución intenta perfil ajeno | P20 → aviso en P09 | Denegado: no muestra identificables sin autorización | Institución | Pendiente |
| T25 | Sin conexión durante carga | P02 → P02 | Informar y permitir reintentar | Todos | Pendiente |
| T26 | Navegación inferior estudiante | P09 ↔ P11/P13/P15/P16 | El menú presenta sólo destinos propios | Estudiante | Pendiente |

## Procedimiento de comprobación

1. Abrir `prototipo/index.html` en navegador y elegir un rol.
2. Seleccionar el escenario en la caja de recorridos para resaltar nodos.
3. Seleccionar un nodo y comprobar sketch, rol, requisito y transiciones salientes.
4. Confirmar visualmente que un rol no puede recorrer módulos propios de otro.
5. Realizar las pruebas con Archify y capturar evidencias de cada flujo crítico.
6. Repetir las pruebas de autenticación y autorización contra la implementación real cuando exista.

## Registro de evidencias pendientes

| Captura o video | Qué demostrar | Archivo propuesto |
|---|---|---|
| E01 | Pantallas P01–P10 en catálogo | `evidencias/E01_generales.png` |
| E02 | Pantallas P11–P20 por rol | `evidencias/E02_funcionales.png` |
| E03 | Diagrama interactivo abierto con Archify | `evidencias/E03_archify_diagrama.png` |
| E04 | Ruta login + recuperación + bloqueo | `evidencias/E04_seguridad.png` |
| E05 | Ruta del estudiante y guía | `evidencias/E05_misiones_guias.png` |
| E06 | Filtro por roles + intento de acceso denegado | `evidencias/E06_roles_acceso.png` |
| E07 | Pull request y revisión / merge en GitHub | `evidencias/E07_github_pr.png` |

**Importante:** las evidencias E01–E07 son nombres sugeridos, no archivos capturados. No afirmar que la entrega final está validada hasta completar estas pruebas.
