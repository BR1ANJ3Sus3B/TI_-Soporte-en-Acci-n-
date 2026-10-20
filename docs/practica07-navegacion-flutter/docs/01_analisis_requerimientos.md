# 01 · Análisis del Proyecto Integrador

**Proyecto:** TI: Soporte en Acción  
**Empresa ficticia:** NovaTech Solutions  
**Plataforma del alcance de esta entrega:** **aplicación móvil Flutter para Android/iOS**.  
**Naturaleza:** aplicación móvil complementaria de un videojuego educativo sobre soporte técnico. Las misiones interactivas 2D se ejecutan en **Unity para PC**; el móvil consulta misiones, guías y progresos, y dispone de módulos de consulta/gestión según el rol.

> **Alcance y trazabilidad:** los códigos RF, RNF, HU y RB de este documento son una **propuesta de identificación para esta práctica**. Deben cotejarse con la versión aprobada de los requerimientos del Proyecto Integrador. No se afirma que estén ya implementados o aprobados.

## Problema y solución

Los estudiantes de tecnologías de la información suelen dominar conceptos teóricos, pero necesitan practicar el diagnóstico de incidentes de hardware, redes, configuración, acceso y seguridad. TI: Soporte en Acción combina misiones con contexto de NovaTech Solutions, recompensas por aprendizaje, consulta de guías y seguimiento de avance. La app Flutter acompaña esa experiencia con navegación rápida y recursos académicos.

## Actores

- **Visitante / público:** ve presentación, inicio de sesión, registro, recuperación y datos del software.
- **Estudiante / jugador:** consulta misiones, guías, progreso y perfil propio.
- **Docente / evaluador:** consulta progreso de estudiantes asignados y resultados educativos.
- **Administrador:** gestiona usuarios y sus roles dentro de los permisos concedidos.
- **Editor de contenido:** registra y actualiza borradores de misiones educativas.
- **Representante de institución educativa:** consulta indicadores agregados autorizados.

## Requerimientos funcionales propuestos

| Código | Requerimiento | Pantallas |
|---|---|---|
| RF-01 | Registro con campos obligatorios y validación de duplicados | P10 |
| RF-02 | Iniciar sesión, mantener sesión válida y cerrar sesión | P02, P03, P09, P16 |
| RF-03 | Validar credenciales y mostrar errores no reveladores | P03 |
| RF-04 | Solicitar recuperación y validar código | P04, P06 |
| RF-05 | Bloquear acceso tras intentos fallidos y permitir desbloqueo verificado | P03, P05, P06 |
| RF-06 | Consultar información del software y empresa | P07, P08 |
| RF-07 | Mostrar Home y accesos según rol | P09 |
| RF-08 | Consultar misiones, requisitos, niveles y objetivos | P11, P12 |
| RF-09 | Mostrar dónde ejecutar la misión en Unity para PC | P12 |
| RF-10 | Consultar misiones completadas y experiencia | P11, P15 |
| RF-11 | Consultar y leer guías de apoyo | P13, P14 |
| RF-12 | Actualizar visualización del progreso del jugador | P15 |
| RF-13 | Consultar/editar datos personales permitidos | P16 |
| RF-14 | Consultar seguimiento académico del grupo asignado | P17 |
| RF-15 | Administrar usuarios y estados según privilegios | P18 |
| RF-16 | Controlar el acceso a pantallas, acciones y datos según rol | P09, P17–P20 |
| RF-17 | Crear/actualizar borradores de misiones autorizadas | P19 |
| RF-18 | Consultar estadísticas educativas consolidadas | P20 |

## Requerimientos no funcionales propuestos

| Código | Característica | Criterio verificable |
|---|---|---|
| RNF-01 | Seguridad y privacidad | Requerir sesión, usar transporte seguro, no revelar contraseñas y aplicar permisos también en la API |
| RNF-02 | Rendimiento | Mostrar retroalimentación de carga y estados de error si la conexión tarda |
| RNF-03 | Accesibilidad | Uso de etiquetas claras, navegación de retorno y tamaño táctil suficiente |
| RNF-04 | Consistencia | Mantener estructura visual y patrones de navegación coherentes entre pantallas móviles |
| RNF-05 | Compatibilidad | Ajustarse a tamaños habituales de Android/iOS mediante diseño adaptable Flutter |
| RNF-06 | Disponibilidad/errores | Explicar fallas de conexión y permitir reintentar sin perder el flujo |
| RNF-07 | Mantenibilidad | Rutas y permisos centralizados, componentes reutilizables, pruebas sobre flujos críticos |

## Historias de usuario propuestas

| ID | Historia |
|---|---|
| HU-01 | Como visitante, quiero ver la apertura y carga para saber que el sistema está iniciando. |
| HU-02 | Como usuario, quiero ingresar de forma segura para acceder a mis módulos. |
| HU-03 | Como usuario, quiero recuperar mi acceso si olvido mi contraseña. |
| HU-04 | Como usuario, quiero conocer un bloqueo y verificar mi identidad para recuperar el acceso. |
| HU-05 | Como visitante, quiero conocer el proyecto y a NovaTech Solutions. |
| HU-06 | Como usuario autenticado, quiero ver accesos de acuerdo con mi rol. |
| HU-07 | Como estudiante nuevo, quiero registrarme sin duplicar mi cuenta. |
| HU-08 | Como jugador, quiero explorar misiones y sus objetivos desde el móvil. |
| HU-09 | Como jugador, quiero leer guías técnicas para prepararme antes de una misión. |
| HU-10 | Como jugador, quiero revisar experiencia y misiones completadas. |
| HU-11 | Como usuario, quiero consultar y actualizar mi perfil o cerrar sesión. |
| HU-12 | Como docente, quiero revisar el progreso del grupo que evalúo. |
| HU-13 | Como administrador, quiero administrar cuentas de forma autorizada. |
| HU-14 | Como editor, quiero redactar y guardar misiones educativas. |
| HU-15 | Como institución, quiero consultar indicadores generales autorizados. |

## Reglas de negocio y seguridad propuestas

| Regla | Definición | Efecto de navegación |
|---|---|---|
| RB-01 | El registro de estudiantes exige datos obligatorios, contraseñas coincidentes y correo único. | P10 solo continúa a P03 después de una validación exitosa. |
| RB-02 | El inicio de sesión exige credenciales válidas y cuenta habilitada. | P03 → P09; error mantiene P03 sin exponer si la cuenta existe. |
| RB-03 | **Umbral ilustrativo: 5 fallos** consecutivos, configurable en el proveedor de identidad. | P03 → P05 cuando se alcanza el límite. |
| RB-04 | El código de recuperación/desbloqueo debe expirar, ser de un solo uso y tener límite de reintentos. | P06 → P03 si se valida; de otro modo se mantiene y explica el error. |
| RB-05 | El menú muestra solo módulos del rol y la API comprueba permisos reales. | P09 → P11/P13/P15 o P17–P20 según rol. |
| RB-06 | Los resultados académicos individuales sólo son visibles para roles autorizados; instituciones reciben datos agregados según permisos. | P17/P20 sin lectura transversal indebida. |
| RB-07 | La app móvil permite consultar misiones, pero el videojuego jugable reside en Unity PC. | P12 indica que la misión se ejecuta en PC; no finge ejecución en Flutter. |
| RB-08 | Al salir se invalida o cierra la sesión y se elimina el acceso a rutas protegidas. | P16 → P03; volver atrás no debe mostrar contenido privado. |
| RB-09 | Un acceso por URL/ruta no autorizado devuelve acceso denegado y conserva al usuario en un área permitida. | Ruta restringida → aviso + P09, sin exponer datos. |

**Nota técnica:** RB-03 no constituye una configuración confirmada; el umbral real deberá definirse y comprobarse con el sistema de autenticación antes de implementarlo. El boceto P06 representa el mismo control visual de código para recuperación o desbloqueo, cuyo comportamiento depende del flujo de entrada.
