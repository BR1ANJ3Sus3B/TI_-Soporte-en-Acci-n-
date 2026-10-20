# 02 · Roles y permisos de navegación (solo móvil Flutter)

Todos los roles pueden ver los datos públicos, autenticarse y consultar su propio perfil una vez iniciada la sesión. La pantalla P09 se adapta al rol mediante accesos condicionales. Las fichas P17–P20 corresponden a **pantallas de teléfono**, no a interfaces de escritorio.

| Rol | Pantallas protegidas propias | Operaciones permitidas | Restricciones |
|---|---|---|---|
| Estudiante / jugador | P09, P11, P12, P13, P14, P15, P16 | Consultar misiones y objetivos, leer guías, consultar XP e historial, editar nombre visible | No administrar cuentas, editar contenidos ni consultar informes de terceros |
| Docente / evaluador | P09, P16, P17 | Consultar avance académico de estudiantes asignados | No modificar cuentas ni misiones; no ver grupos ajenos |
| Administrador | P09, P16, P18 | Consultar, activar/suspender y gestionar roles dentro de su ámbito | No acceder automáticamente a datos académicos confidenciales o contraseñas |
| Editor de contenido | P09, P16, P19 | Crear y modificar borradores de misiones asignadas | No otorgar roles ni consultar reportes institucionales por defecto |
| Institución educativa | P09, P16, P20 | Consultar estadísticas consolidadas de su institución | No modificar datos de estudiantes ni ver información identificable sin autorización |
| Visitante | P01–P08 y P10 según flujo | Ver información, registrarse, ingresar, recuperar acceso | No acceder a P09 ni a ningún módulo privado |

## Comportamiento de la Home por rol

1. **Estudiante:** muestra accesos a Misiones, Guías, Progreso y Perfil (tal como el sketch P09).
2. **Docente:** presenta acceso a Seguimiento (P17) y Perfil, sin enlaces de estudiante.
3. **Administrador:** presenta acceso a Gestión de usuarios (P18) y Perfil.
4. **Editor:** presenta acceso a Editor de misiones (P19) y Perfil.
5. **Institución:** presenta acceso a Reportes institucionales (P20) y Perfil.

La Home P09 es un **patrón base con contenido variable**, no cinco sketches adicionales. El diseño concreto de las variantes de rol debe revisarse cuando se implemente.

## Denegación de acceso

- **Ocultar no equivale a autorizar:** quitar un botón no reemplaza una validación del servidor.
- Si el usuario trata de abrir `/admin/usuarios` sin el rol correspondiente, se presenta un aviso de **acceso denegado** y se devuelve a P09.
- Nunca usar datos personales reales ni credenciales en imágenes, prototipos o repositorio.
