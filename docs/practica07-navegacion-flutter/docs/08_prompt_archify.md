# 08 · Prompt para Archify — TI: Soporte en Acción (solo Flutter)

Copia el siguiente texto en Archify y adjunta las **20 imágenes a lápiz** de `sketches/` y, si es posible, las **20 versiones a color** de `mockups-color/`. El sketch original sigue siendo evidencia obligatoria.

---

## PROMPT PRINCIPAL

Necesito un **DIAGRAMA DE NAVEGACIÓN INTERACTIVO**, completo y responsivo, para mi proyecto integrador **TI: SOPORTE EN ACCIÓN**, de la empresa ficticia **NovaTech Solutions**.

**ALCANCE OBLIGATORIO: DISEÑO EXCLUSIVAMENTE MÓVIL CON FLUTTER (ANDROID/iOS). NO GENERAR INTERFACES DE ESCRITORIO NI APLICACIONES WEB.** El HTML que produzcas servirá únicamente para **documentar y consultar** el prototipo académico desde un navegador; no representa la interfaz final de la aplicación.

El proyecto es un videojuego educativo 2D para aprender soporte técnico. La app móvil Flutter es **complementaria** del videojuego principal (Unity para PC), por lo que puede consultar misiones y guías, pero no debe representar que una misión Unity se ejecuta dentro de Flutter.

### Identidad visual que deberá respetar Archify

- Fondo `#070D1B`, superficie `#10213B`, cian `#00D9F5`, turquesa `#28E6CB`, violeta `#906DFF`, verde `#38D99A` y naranja `#FF9E3B`.
- Estilo tecnológico profesional, inspirado en RPG 2D / videojuego educativo de soporte técnico.
- El diagrama se presenta en fondo oscuro, nodos azules, enlaces cian, condiciones positivas turquesa, secciones por rol y leyenda clara.
- Cada nodo debe ofrecer **dos referencias**: sketch lápiz y propuesta a color.

### Entregable visual

Diseña **20 nodos/pantallas móviles** numerados P01 a P20, cada uno con una miniatura del PNG de su sketch, nombre, rol, descripción corta, enlaces entrantes y salientes y restricciones de acceso. Conservar las imágenes originales como evidencia de **lápiz / blanco y negro / baja fidelidad** y añadir, cuando se seleccione cada nodo, un enlace complementario al diseño a color de esa misma pantalla. El contenedor del diagrama puede usar la identidad del proyecto.

Pantallas obligatorias P01–P09:
- P01 Splash Screen
- P02 Loading Screen
- P03 Iniciar sesión
- P04 Recuperar cuenta
- P05 Cuenta bloqueada
- P06 Código de desbloqueo
- P07 Acerca del software
- P08 Acerca del equipo / NovaTech Solutions
- P09 Inicio (Home)

Pantallas adicionales P10–P20:
- P10 Registro de cuenta
- P11 Misiones
- P12 Detalle de misión
- P13 Guías de apoyo
- P14 Lectura de guía
- P15 Mi progreso
- P16 Mi perfil
- P17 Panel docente
- P18 Gestión de usuarios
- P19 Editor de misiones
- P20 Reportes institucionales

### Roles

- Visitante: acceso y pantallas públicas.
- Estudiante/Jugador: P09, P11–P16.
- Docente/Evaluador: P09, P16 y P17.
- Administrador: P09, P16 y P18.
- Editor de contenido: P09, P16 y P19.
- Institución educativa: P09, P16 y P20.

### Reglas de navegación indispensables

- P01 → P02 → P03 sin sesión o P09 con sesión vigente.
- P03 → P09 si se autentica; permanece P03 con error; P03 → P05 por exceso de intentos.
- P03 → P04 → P06 → P03 para recuperación.
- P05 → P06 → P03 para desbloqueo verificado.
- P03 → P10 → P03 para registro correcto; si hay campos inválidos o correo duplicado, conservar P10.
- P03/P09 → P07 → P08 y rutas de retorno.
- Para estudiante: P09 → P11 → P12 → P13 → P14; P09 → P15; P09 → P16 → P03 para salir.
- Para docente: P09 → P17 → P09.
- Para administrador: P09 → P18 → P09.
- Para editor: P09 → P19 → P09.
- Para institución: P09 → P20 → P09.
- Acceso no autorizado: mostrar aviso y permanecer o volver a P09 sin mostrar datos protegidos.
- Cada transición debe tener flecha direccional, condición y rol.

### Interacciones requeridas

1. Barra de filtro por roles y selector de escenarios de prueba.
2. Al hacer clic en un nodo, abrir su descripción, la imagen del sketch original y una opción para ver el mismo diseño a color.
3. Resaltar ruta del usuario, destino siguiente y transiciones permitidas.
4. Opción de volver y destacar transiciones especiales (error, bloqueo, recuperación, logout).
5. Recorridos de ejemplo para estudiante, docente, administrador, editor e institución.
6. Vista general que permita localizar todos los nodos sin perder las relaciones.
7. HTML estático y autónomo, compatible con GitHub Pages, con rutas **relativas** a `sketches/` y `mockups-color/`.
8. Documentación con identificación, funciones, rol, plataforma, elementos y relación con requerimientos.

Asegúrate de que no haya nodos huérfanos ni enlaces rotos. Conserva los 20 PNG individuales a lápiz y los 20 PNG a color, sin sustituir los primeros por UI final. El sistema de permisos es sólo una **simulación visual** en el prototipo, no seguridad real.

### Archivos de apoyo

Usa los archivos `docs/inventario_pantallas.csv`, `docs/matriz_transiciones.csv`, `diagramas/navegacion_general.mmd`, `docs/04_fichas_20_sketches.md` y `prototipo/datos_navegacion.js` para recuperar el contenido completo, permisos, rutas y asociaciones visuales.

---

## PROMPT DE CORRECCIÓN / VALIDACIÓN

Revisa el diagrama Flutter móvil generado. Confirma 20 pantallas únicas, nueve generales (P01–P09), once adicionales (P10–P20), flechas de retorno, transiciones de credenciales incorrectas, acceso denegado, recuperación y bloqueo. Comprueba que los módulos docente, administrador, editor e institución sólo se alcanzan desde el rol correspondiente. Verifica que al seleccionar cada nodo aparezca su sketch PNG correcto. Proporciona una lista verificable de hallazgos y cambios, sin declarar pruebas que no hayas realizado.
