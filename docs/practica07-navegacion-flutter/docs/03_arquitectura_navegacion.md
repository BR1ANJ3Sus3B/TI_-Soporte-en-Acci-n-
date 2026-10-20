# 03 · Arquitectura de navegación para Flutter

La práctica documenta la aplicación móvil complementaria **TI: Soporte en Acción**. No incluye interfaces web; el videojuego ejecutable se reserva para Unity en PC.

## Entradas, rutas y retornos

**Inicio general:** P01 Splash → P02 Loading → P03 Login (sin sesión) o P09 Home (sesión vigente).  
**Seguridad:** P03 → P04 Recuperar → P06 Código → P03; P03 → P05 Bloqueada → P06 → P03.  
**Registro:** P03 → P10 Registro → P03, sólo si no hay duplicados y se cumplen los campos obligatorios.  
**Información:** P03/P09 → P07 Acerca → P08 Equipo → P07.  
**Estudiante:** P09 → P11 Misiones → P12 Detalle → P13 Guías → P14 Lectura. P09 → P15 Progreso.  
**Perfil y salida:** P09 → P16 Perfil → P03 tras cerrar sesión.  
**Otros roles:** P09 → P17 Docente / P18 Administrador / P19 Editor / P20 Institución, sólo para el rol indicado.

## Reglas de retorno

- En P12, la flecha Atrás devuelve a P11 sin perder el estado del listado.
- Desde una guía P14 se vuelve a P13; si se llegó desde P12 y el contexto sigue disponible, se puede volver al detalle.
- La barra inferior de estudiante mantiene destinos Inicio, Misiones, Guías, Progreso y Perfil. En otros roles el menú se sustituye por módulos autorizados.
- El cierre de sesión (P16) elimina la posibilidad de volver a contenido protegido mediante Atrás.
- Si se intenta navegar a una pantalla no autorizada, el sistema muestra un mensaje de acceso denegado y redirige a P09.

## Condiciones que no precisan una pantalla adicional

- Error de correo o contraseña: mensaje dentro de P03.
- Datos del registro erróneos o duplicados: validación dentro de P10.
- Código inválido/caducado: mensaje y reenvío controlado dentro de P06.
- Misión bloqueada: estado visual en P11/P12, sin opción de ejecutarla desde el teléfono.
- Fallo de conexión: mensaje de carga/reintento en P02 o en la pantalla actual.
- Acceso denegado: aviso temporal dentro de Home P09.

## Documentos y visualización

- [Diagrama interactivo](../prototipo/index.html) — elegir rol, pulsar pantallas, explorar enlaces y ejecutar escenarios.
- [Diagrama estático SVG](../diagramas/navegacion_estatica.svg).
- [Diagrama editable Mermaid](../diagramas/navegacion_general.mmd).
- [Matriz completa de 51 transiciones](matriz_transiciones.csv) — contiene origen, destino, condición y rol.

## Implementación sugerida más adelante

Flutter puede usar un sistema de rutas declarativas y un guard de autenticación/roles. Los sketch y el HTML son **prototipos**, no pantallas Flutter ni mecanismos de seguridad reales. La autenticación integrada, incluida la política de bloqueo, debe validarse con el servicio real antes de presentar la aplicación como funcional.
