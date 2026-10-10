# TI: Soporte en Acción

Proyecto integrador de videojuego educativo en Unity, aplicación móvil en Flutter y PWA en React.

**Autor y responsable único:** Brian Jesús Mendoza Márquez · **Matrícula:** 230308.

**Estado:** documentación de diseño y alcance. Las funciones descritas son requisitos previstos; este documento no acredita que estén implementadas.

**Base de datos confirmada:** MongoDB. Las aplicaciones se comunicarán con ella mediante una API; Node.js, Express y Mongoose constituyen la propuesta de servidor.

Este documento conserva el contenido textual, las tablas y las 21 imágenes del documento Word del proyecto, en su orden original. Las imágenes son referencias de diseño. Al final se incluye la paleta acordada en la conversación como complemento, ya que no está presente en esta copia del Word.

## Índice

- [Propuesta de integración académica](#propuesta-de-integración-académica)
- [Funciones de cada aplicación](#funciones-de-cada-aplicación)
- [Roles de usuario y permisos](#roles-de-usuario-y-permisos)
- [Arquitectura y datos compartidos](#arquitectura-y-datos-compartidos)
- [Alcance inicial y plan individual](#alcance-inicial-y-plan-individual)
- [Requerimientos funcionales](#requerimientos-funcionales)
- [Requerimientos funcionales de progreso y administración](#requerimientos-funcionales-de-progreso-y-administración)
- [Requerimientos no funcionales](#requerimientos-no-funcionales)
- [Requerimientos no funcionales de operación y mantenimiento](#requerimientos-no-funcionales-de-operación-y-mantenimiento)
- [Requerimientos para una segunda etapa](#requerimientos-para-una-segunda-etapa)
- [Diseño del videojuego](#diseño-del-videojuego)
- [Introducción](#introducción)
- [Dinámica general](#dinámica-general)
- [Público objetivo](#público-objetivo)
- [Género](#género)
- [Historia](#historia)
- [Personaje principal](#personaje-principal)
- [Enemigo principal](#enemigo-principal)
- [Personajes secundarios](#personajes-secundarios)
- [Niveles](#niveles)
- [Storyboard](#storyboard)
- [Canvas](#canvas)
- [Plan de monetización](#plan-de-monetización)
- [Organización del trabajo individual](#organización-del-trabajo-individual)
- [Bocetos de la aplicación web progresiva](#bocetos-de-la-aplicación-web-progresiva)
- [Bocetos de la aplicación móvil](#bocetos-de-la-aplicación-móvil)
- [Paleta de colores complementaria](#paleta-de-colores-complementaria)
- [Uso de este paquete](#uso-de-este-paquete)

## Propuesta de integración académica

Desarrollaré TI: Soporte en Acción como un proyecto con tres productos relacionados. El videojuego conservará la historia de Alex y sus misiones en NovaTech Solutions. La aplicación móvil y la PWA acompañarán esa experiencia con información y seguimiento del aprendizaje.

Para mantener un alcance viable, propongo comenzar con el primer nivel, El Primer Día, y dos roles: jugador y administrador. Los siguientes niveles y el rol docente se incorporarán después de validar la integración básica.

| Asignatura | Producto | Entregable inicial |
| --- | --- | --- |
| Creación de Videojuegos | Videojuego 2D en Unity con C# | Movimiento, diálogo con Sofía, diagnóstico de Bloq Mayús, cierre de ticket y recompensa. |
| Desarrollo Móvil Integral | Aplicación complementaria con Flutter y Dart | Inicio de sesión, misiones, guía de soporte y progreso del jugador. |
| Aplicaciones Web Progresivas | PWA con React | Inicio de sesión, progreso, guías y administración de misiones; manifiesto y caché de contenido público. |

### Objetivo general

Desarrollar un videojuego educativo y dos aplicaciones complementarias que permitan resolver incidentes simulados de TI, consultar materiales de apoyo y dar seguimiento al avance del jugador mediante servicios compartidos.

### Objetivos específicos

- Implementar en Unity una misión completa con exploración, interacción y resolución de un ticket.
- Crear en Flutter pantallas conectadas a una API para consultar perfil, misiones y avances.
- Construir en React una PWA con funciones de jugador y administración.
- Compartir usuarios, catálogo de misiones y resultados sin mantener bases de datos independientes por aplicación.
- Definir permisos y verificar que cada usuario acceda únicamente a las funciones y datos autorizados.

## Funciones de cada aplicación

### Videojuego en Unity

Será el lugar donde se juega la historia: el usuario controla a Alex, recorre la oficina, conversa con los personajes y resuelve los incidentes. En el prototipo, completar la misión de Sofía desbloqueará el cierre del ticket y la recompensa definida para el primer nivel. El juego enviará el resultado a la API para que las otras aplicaciones puedan mostrarlo.

### Aplicación móvil en Flutter

Será una aplicación de acompañamiento. El jugador podrá iniciar sesión, consultar las misiones disponibles, revisar sus objetivos, leer una guía breve y visualizar sus resultados. La versión inicial priorizará Android para reducir el esfuerzo de desarrollo y pruebas.

- Pantallas: acceso, inicio, lista de misiones, detalle de misión, guías, progreso y perfil.
- Las guías explicarán conceptos de soporte, hardware y redes presentes en la historia.
- El teléfono permitirá consultar el avance; completar una guía no cerrará automáticamente una misión del videojuego.

### Aplicación web progresiva en React

La PWA ofrecerá al jugador un panel con su avance, misiones y guías. El administrador contará con un panel para editar el catálogo y publicar materiales. El alcance inicial incluirá un manifiesto web y un service worker para disponer de una página de inicio y guías públicas previamente almacenadas cuando falte conexión.

La consulta de resultados actualizados, el inicio de sesión y la administración requerirán conexión. Si se muestra progreso almacenado, se indicará la fecha de la última actualización. La instalación y las capacidades disponibles se comprobarán en los navegadores elegidos para la entrega.

### Ejemplo de uso integrado

El jugador entra en Unity con su cuenta y resuelve el problema de Sofía. El juego registra la misión terminada en la API. Al abrir Flutter o la PWA y actualizar la información, el jugador observa la misión completada y su experiencia acumulada. Antes de una misión posterior, puede consultar una guía desde cualquiera de las aplicaciones complementarias.

## Roles de usuario y permisos

Los roles de usuario son permisos de las personas que utilizan el sistema. Alex, Martín, Richar, Sofía, Marco y el Saboteador son personajes de la historia; no representan cuentas ni permisos administrativos. La responsabilidad individual del desarrollo tampoco limita la cantidad de jugadores que pueden usar el producto.

| Rol | Funciones permitidas | Límites |
| --- | --- | --- |
| Jugador | Jugar en Unity; consultar misiones y guías; ver su progreso; modificar datos básicos de su perfil. | Accede a sus propios resultados. No edita el catálogo, roles, recompensas ni resultados de otras personas. |
| Administrador | Gestionar misiones y guías; publicar contenido; consultar usuarios; activar o desactivar cuentas y administrar roles. | No modifica resultados históricos desde el panel inicial. Las acciones administrativas se registran. |
| Docente o evaluador<br>Ampliación opcional | Asignar misiones a un grupo y consultar el progreso de estudiantes vinculados. | Sólo consulta grupos autorizados. No gestiona administradores ni el contenido global. |

### Distribución por plataforma

| Función | Unity | Flutter | React PWA |
| --- | --- | --- | --- |
| Jugar y resolver incidentes | Jugador | No | No |
| Consultar progreso propio | Jugador | Jugador | Jugador |
| Consultar misiones y guías | Jugador | Jugador | Jugador |
| Editar catálogo y guías | No | No | Administrador |
| Gestionar cuentas y roles | No | No | Administrador |
| Consultar un grupo educativo | No | Futuro | Docente opcional |

Una persona puede tener permisos de jugador y administrador. Para las pruebas conviene utilizar cuentas separadas, de modo que se comprueben los límites de cada rol. El registro público creará únicamente jugadores; los permisos de administrador se asignarán mediante un procedimiento controlado.

### Reglas de autorización

La API comprobará la identidad, el rol y la propiedad del recurso en cada solicitud. Ocultar un botón en Flutter o React no será suficiente. Un jugador no podrá obtener los resultados de otra cuenta cambiando su identificador. Las contraseñas se almacenarán mediante un hash adecuado y las credenciales se excluirán del repositorio.

![Seis roles en acción para soporte TI](<../imagenes/Seis roles en acción para soporte TI.png>)

## Arquitectura y datos compartidos

Propongo una API REST con Node.js y Express y una base de datos MongoDB, como una alternativa de implementación para los servicios compartidos. Unity, Flutter y React se comunicarán con la API mediante HTTPS; la base de datos será accesible únicamente desde el servidor. La elección del alojamiento quedará para la etapa de despliegue.

![TI: Soporte en acción con MongoDB](<../imagenes/TI_ Soporte en acción con MongoDB.png>)

| Componente | Responsabilidad |
| --- | --- |
| Unity y C# | Ejecutar escenas, diálogos y retos; guardar temporalmente el estado local; enviar resultados. |
| Flutter y Dart | Consultar información del jugador, misiones y materiales mediante la API. |
| React y PWA | Mostrar progreso y guías; ofrecer un panel de administración protegido. |
| API REST | Autenticar, autorizar, validar resultados y gestionar los datos comunes. |
| Base de datos | Conservar cuentas, misiones, guías, intentos y progreso. |

### Datos principales

| Entidad | Datos necesarios |
| --- | --- |
| Usuario | Identificador, nombre, correo, hash de contraseña, roles y estado. |
| Misión | Identificador, título, nivel, objetivo, requisitos, versión y recompensa. |
| Intento | Identificador único, jugador, misión, resultado y fecha. |
| Progreso | Jugador, misión, estado y experiencia otorgada. |
| Guía | Título, tema, contenido y estado de publicación. |

### Sincronización de resultados

El servidor comprobará que la misión existe, que sus requisitos se cumplen y que el intento no fue registrado previamente. Calculará la recompensa a partir de la misión y otorgará XP una sola vez. Flutter y React consultarán ese mismo progreso al cargar o actualizar sus pantallas.

Para el MVP, el envío de resultados requerirá conexión. Si falla, Unity conservará el intento pendiente y permitirá reintentar con el mismo identificador, evitando duplicados. El prototipo no incluirá una protección completa contra clientes manipulados; no se utilizarán sus resultados para premios económicos o clasificaciones competitivas.

### Operaciones mínimas de la API

- Registro e inicio de sesión; consulta del perfil de la cuenta autenticada.
- Consulta del catálogo de misiones, sus requisitos y las guías publicadas.
- Registro de un intento y consulta del progreso del jugador autenticado.
- Creación y edición de misiones y guías, restringidas al administrador.
- Consulta administrativa de usuarios y gestión controlada de roles.

## Alcance inicial y plan individual

La primera entrega demostrará una misión completa y su resultado compartido entre las tres aplicaciones. La historia y los materiales de los niveles posteriores se conservarán como diseño del producto, sin exigir que todo el contenido esté programado en el primer prototipo.

| Etapa | Resultado verificable |
| --- | --- |
| 1 Diseño | Pantallas, flujo del primer nivel, datos y permisos definidos. |
| 2 Servicios compartidos | Registro, acceso, catálogo y progreso probados con dos cuentas de jugador. |
| 3 Unity | Misión de Sofía jugable desde el inicio hasta el registro del resultado. |
| 4 Flutter | Consulta de misiones, guía y progreso de la cuenta autenticada. |
| 5 React y PWA | Panel del jugador, administración y guías públicas disponibles sin conexión tras almacenarlas. |
| 6 Integración y entrega | Pruebas, correcciones, documentación y demostración de las tres aplicaciones. |

### Criterios de aceptación

- Una cuenta permite ingresar a las tres aplicaciones y consultar el mismo perfil.
- Resolver la misión de Sofía registra el cierre y suma la recompensa sólo una vez, incluso si el envío se repite.
- Flutter y React muestran el resultado al actualizar el progreso.
- Un jugador no puede acceder a funciones administrativas ni consultar el progreso de otra cuenta.
- El administrador puede editar y publicar una guía desde la PWA.
- Una guía pública previamente almacenada abre sin conexión; las pantallas que necesitan datos actuales informan de la falta de conexión.

### Ampliaciones posteriores

Después del primer prototipo se incorporarán los niveles de hardware y redes, más desafíos de programación y seguridad, logros y el rol docente. Las notificaciones, pagos, chat, multijugador y sincronización automática sin conexión quedarán fuera de la primera entrega.

### Responsable del desarrollo

Brian Jesús Mendoza Márquez será el único responsable del análisis, diseño, programación en Unity, Flutter y React, servicios compartidos, pruebas, documentación y presentación. Los roles de análisis, programación y pruebas representan funciones que realizaré durante el proyecto, sin implicar integrantes adicionales.

## Requerimientos funcionales

Estos requerimientos definen lo que deberá permitir el sistema integrado por Unity, Flutter, la PWA en React y los servicios compartidos. Describen funciones previstas; su cumplimiento se comprobará durante el desarrollo y las pruebas.

### Acceso e interacción en el videojuego

| ID | Requerimiento y comportamiento | Componente |
| --- | --- | --- |
| RF-01 | El sistema debe permitir el registro de usuarios mediante campos obligatorios, sin duplicación de datos. | Flutter, PWA y API |
| RF-02 | Iniciar sesión. Acceder con la misma cuenta al videojuego, la aplicación móvil y la PWA. | Todas las plataformas |
| RF-03 | Cerrar sesión. Cerrar la sesión e impedir el acceso a funciones protegidas hasta una nueva autenticación. | Todas las plataformas |
| RF-04 | Consultar y editar el perfil. Consultar el perfil y modificar el nombre visible del jugador. | Flutter y PWA |
| RF-05 | Controlar permisos por rol. Permitir el acceso a las funciones correspondientes a jugador y administrador. | API y aplicaciones |
| RF-06 | Controlar al personaje. Mover a Alex por las zonas habilitadas del escenario. | Unity |
| RF-07 | Interactuar con personajes y objetos. Conversar con personajes y revisar computadoras u objetos cercanos para obtener información sobre un incidente. | Unity |
| RF-08 | Recibir y consultar misiones. Revisar los objetivos, requisitos y estado de las misiones o tickets. | Unity, Flutter y PWA |
| RF-09 | Resolver incidentes tecnológicos. Investigar un problema y seleccionar o ejecutar acciones para resolverlo. La primera misión consiste en ayudar a Sofía a iniciar sesión. | Unity |
| RF-10 | Evaluar las acciones del jugador. Comprobar las acciones realizadas y mostrar retroalimentación sobre aciertos y errores. | Unity |
| RF-11 | Completar y cerrar misiones. Marcar una misión como completada únicamente cuando se cumplan sus condiciones de resolución. | Unity y API |

## Requerimientos funcionales de progreso y administración

La información de avance se asociará a la cuenta del jugador. Las operaciones administrativas estarán reservadas a usuarios autorizados y serán comprobadas por la API.

| ID | Requerimiento y comportamiento | Componente |
| --- | --- | --- |
| RF-12 | Otorgar experiencia. Asignar la recompensa configurada al completar una misión y evitar otorgarla nuevamente por un envío repetido del mismo resultado. | API |
| RF-13 | Guardar y recuperar el progreso. Almacenar las misiones completadas y la experiencia del jugador; recuperar esa información al volver a ingresar. | Unity, API y base de datos |
| RF-14 | Compartir el progreso entre aplicaciones. Mostrar en Flutter y la PWA los resultados registrados desde Unity cuando se consulte o actualice el progreso. | Todas las plataformas |
| RF-15 | Reintentar el envío de resultados. Conservar en Unity el intento pendiente si falla su envío y permitir reenviarlo sin duplicar la recompensa. | Unity y API |
| RF-16 | Consultar guías educativas. Consultar materiales relacionados con soporte técnico y los temas de las misiones. | Flutter y PWA |
| RF-17 | Consultar guías públicas sin conexión. Abrir las guías públicas previamente almacenadas e informar cuando una función necesite conexión. | PWA |
| RF-18 | Administrar el catálogo de misiones. Permitir al administrador crear, editar, publicar y desactivar misiones del catálogo. Las escenas y mecánicas nuevas requerirán desarrollo en Unity. | PWA y API |
| RF-19 | Administrar guías educativas. Permitir al administrador crear, editar, publicar y retirar materiales educativos. | PWA y API |
| RF-20 | Administrar cuentas. Consultar usuarios, activar o desactivar cuentas y asignar roles autorizados. El registro público sólo creará jugadores. | PWA y API |
| RF-21 | Registrar acciones administrativas. Guardar quién realizó una modificación administrativa, qué acción ejecutó y cuándo ocurrió. | API y base de datos |

## Requerimientos no funcionales

Estos requerimientos establecen condiciones de calidad, seguridad y funcionamiento. Los valores de rendimiento y capacidad son metas propuestas para el prototipo y se validarán con el equipo y el alojamiento elegidos.

| ID | Requerimiento | Condición verificable |
| --- | --- | --- |
| RNF-01 | Protección de contraseñas | Almacenar las contraseñas mediante un algoritmo de hash diseñado para contraseñas, con salt. No aparecerán en texto plano en la base de datos ni en los registros. |
| RNF-02 | Protección de las comunicaciones | Las aplicaciones desplegadas se comunicarán con la API mediante HTTPS. Las credenciales y secretos se mantendrán fuera del repositorio. |
| RNF-03 | Autorización en el servidor | Verificar identidad, rol y propiedad de los datos en cada operación protegida. Las pruebas confirmarán que un jugador no puede consultar resultados ajenos ni ejecutar funciones administrativas. |
| RNF-04 | Tiempo de respuesta | En el entorno de prueba acordado, al menos el 95 % de las consultas de perfil, misiones y progreso responderá en un máximo de 2 segundos, con 20 usuarios concurrentes simulados. |
| RNF-05 | Fluidez del videojuego | El primer nivel mantendrá al menos 30 FPS durante una prueba de 10 minutos en el equipo de referencia, cuyas características se documentarán. |
| RNF-06 | Facilidad de uso | Al menos 4 de 5 usuarios de prueba podrán iniciar sesión y localizar una misión y su progreso en menos de 3 minutos, sin ayuda directa. |
| RNF-07 | Compatibilidad | Probar el prototipo en una PC Windows para Unity, un dispositivo Android para Flutter y Chrome y Edge para la PWA. Documentar las versiones utilizadas; no deberán existir fallos que impidan completar el flujo principal. |

## Requerimientos no funcionales de operación y mantenimiento

Las siguientes condiciones complementan los controles de seguridad y rendimiento, y deberán verificarse antes de la entrega del prototipo.

| ID | Requerimiento | Condición verificable |
| --- | --- | --- |
| RNF-08 | Diseño adaptable | La PWA mantendrá controles y textos utilizables en anchos de pantalla de 360, 768 y 1366 píxeles, sin desplazamiento horizontal en las pantallas principales. |
| RNF-09 | Integridad de los resultados | Reenviar varias veces un mismo intento producirá un único registro válido y una sola recompensa. El registro del resultado y de la recompensa deberá mantenerse consistente ante errores. |
| RNF-10 | Manejo de fallos | Ante una pérdida de conexión, mostrar un mensaje comprensible y una opción de reintento cuando corresponda. No informar que un resultado fue sincronizado si el servidor no lo confirmó. |
| RNF-11 | Accesibilidad | Flutter y React mostrarán etiquetas comprensibles y contraste suficiente en los controles principales. La PWA permitirá navegar por teclado. El videojuego presentará instrucciones y diálogos en texto y no comunicará información esencial únicamente mediante sonido o color. |
| RNF-12 | Mantenibilidad | Separar interfaz, lógica y acceso a datos. Cada componente tendrá instrucciones de instalación, configuración y ejecución, y se gestionará mediante control de versiones. |
| RNF-13 | Respaldo y recuperación | Durante las pruebas con datos persistentes se realizará un respaldo diario. Antes de la entrega se comprobará que un respaldo permite restaurar usuarios, misiones y progreso. |
| RNF-14 | Separación entre clientes y datos | Unity, Flutter y React accederán a los datos mediante la API. Ninguna aplicación cliente incluirá credenciales de conexión directa a la base de datos. |

## Requerimientos para una segunda etapa

Estas funciones se contemplan como ampliaciones posteriores a la integración inicial. Su desarrollo dependerá del avance del proyecto y de los tiempos disponibles.

| ID | Requerimiento funcional futuro |
| --- | --- |
| RF-22 | Incorporar las misiones de hardware, redes, programación y seguridad informática. |
| RF-23 | Desbloquear nuevos niveles cuando el jugador cumpla los requisitos de progreso. |
| RF-24 | Permitir que un docente asigne misiones a sus grupos. |
| RF-25 | Permitir que el docente consulte únicamente los resultados de los estudiantes vinculados a sus grupos. |
| RF-26 | Notificar al jugador cuando se publiquen nuevas misiones o materiales. |

### Verificación de la primera entrega

La primera entrega demostrará el recorrido completo: el jugador crea su cuenta, resuelve la misión de Sofía en Unity, obtiene su recompensa y consulta el mismo progreso desde Flutter y React. Este recorrido permitirá comprobar la integración de las tres materias.

Los requisitos RF-01 a RF-21 corresponden al alcance funcional inicial. RF-22 a RF-26 quedan para una segunda etapa. Los requisitos RNF-01 a RNF-14 se utilizarán como criterios de evaluación del prototipo, con los objetivos numéricos sujetos a validación en el entorno elegido.

## Diseño del videojuego

Las siguientes secciones conservan la historia, personajes, niveles y referencias visuales del videojuego. Las imágenes son materiales conceptuales; las funciones implementadas se comprobarán mediante el prototipo. Algunas hojas conceptuales conservan rótulos de Godot de una versión anterior; el motor definido para este proyecto es Unity.

## Introducción

En la actualidad, la tecnología forma parte fundamental de la vida cotidiana y del funcionamiento de empresas, instituciones educativas y organizaciones. Sin embargo, los jóvenes suelen relacionarse con ella principalmente como usuarios, sin tener la oportunidad de conocer de manera práctica los problemas y situaciones que pueden presentarse dentro de un entorno laboral relacionado con las Tecnologías de la Información.

TI: Soporte en Acción es un videojuego de aventura, simulación y resolución de problemas, diseñado principalmente para jóvenes y estudiantes interesados en el área de la tecnología. En el juego, el jugador asume el papel de Alex, un joven recién egresado de Ingeniería en Desarrollo y Gestión de Software que obtiene su primer empleo en una empresa tecnológica.

A lo largo de la historia, Alex deberá enfrentarse a diferentes situaciones relacionadas con el soporte técnico, las redes, la programación, las bases de datos y la seguridad informática. Para avanzar en el juego, el jugador tendrá que explorar las instalaciones, interactuar con otros personajes, investigar las causas de los problemas, tomar decisiones y completar diversos retos y minijuegos.

El propósito de este proyecto es combinar el entretenimiento y el aprendizaje mediante una experiencia interactiva que permita desarrollar habilidades como la observación, el análisis, la toma de decisiones y la resolución de problemas. Asimismo, busca acercar a los jóvenes al mundo de las Tecnologías de la Información de una manera sencilla, dinámica y entretenida, sin requerir conocimientos técnicos avanzados para comenzar a jugar.

De esta manera, TI: Soporte en Acción propone una experiencia en la que cada problema representa una nueva misión y cada solución permite al jugador adquirir experiencia, avanzar en la historia y conocer progresivamente algunos de los desafíos que pueden presentarse dentro de una empresa tecnológica.

## Dinámica general

La dinámica principal de TI: Soporte en Acción consiste en que el jugador asume el papel de Alex, un joven recién egresado que comienza a trabajar en una empresa tecnológica.

Durante el juego, el jugador deberá realizar las siguientes actividades:

- Recepción de reportes: Recibir solicitudes de los empleados relacionadas con diferentes problemas tecnológicos.
- Investigación: Recopilar información y revisar la situación para encontrar posibles causas.
- Análisis: Identificar el origen del problema y determinar las posibles soluciones.
- Resolución: Aplicar una solución adecuada para resolver el problema presentado.
- Obtención de experiencia: Recibir experiencia por cada situación resuelta correctamente.
- Progresión: Avanzar en la historia y desbloquear nuevos desafíos con un mayor nivel de dificultad.
- Atención de incidentes especiales: Resolver situaciones relacionadas con problemas de seguridad o fallas que puedan afectar a la empresa.

El ciclo principal del juego se desarrolla de la siguiente manera: recibir un reporte, investigar el problema, analizar la situación, encontrar una solución, obtener experiencia y avanzar al siguiente desafío.

## Público objetivo

TI: Soporte en Acción está dirigido a jóvenes interesados en los videojuegos, la tecnología y los retos. El proyecto busca acercar a los jugadores al mundo de las Tecnologías de la Información mediante una experiencia interactiva y fácil de comprender.

El público objetivo se caracteriza por:

- Edad: Jóvenes de entre 15 y 30 años.
- Intereses: Videojuegos, tecnología y resolución de problemas.
- Perfil: Principalmente estudiantes y personas que desean conocer más sobre las Tecnologías de la Información.
- Conocimientos previos: No se requieren conocimientos técnicos avanzados.
- Áreas relacionadas: Soporte técnico, programación, redes, bases de datos y seguridad informática

![Público objetivo](../imagenes/01-publico-objetivo.png)

![Público objetivo](../imagenes/02-publico-objetivo.png)

## Género

### Aventura y simulación tecnológica.

El juego combina elementos de aventura y simulación, permitiendo al jugador explorar una empresa, interactuar con diferentes personajes, recibir misiones y resolver problemas relacionados con el área tecnológica. A lo largo del juego, el jugador deberá investigar situaciones, tomar decisiones y aplicar sus conocimientos para completar los diferentes desafíos.

## Historia

Después de varios años de estudiar Ingeniería en Desarrollo y Gestión de Software, Alex finalmente consigue su primer empleo en una empresa tecnológica.

Aunque acaba de egresar y tiene conocimientos de programación, redes, soporte y bases de datos, pronto descubre que la universidad y el mundo laboral son dos cosas muy diferentes.

Su primer día comienza con una sencilla tarea:

"Ayudar a un empleado que no puede iniciar sesión en su computadora."

Lo que parece un problema sencillo termina siendo el comienzo de una serie de desafíos que pondrán a prueba todo lo que aprendió durante su carrera.

La empresa depende completamente de sus sistemas tecnológicos. Una computadora que no funciona, una red caída, un servidor con problemas o un incidente de seguridad pueden detener el trabajo de decenas de empleados

Alex tendrá que investigar los problemas, hablar con los empleados, encontrar soluciones y aprender de sus errores.

Pero hay algo más...

A medida que Alex gana experiencia, descubre que algunos problemas no son simples fallas técnicas. Alguien está intentando acceder a información que no debería.

Ahora tendrá que demostrar que puede pasar de ser un estudiante recién egresado a un verdadero profesional de TI.

## Personaje principal

### Alex

- Rol: Es el personaje principal de TI: Soporte en Acción.
- Historia: Es un recién egresado de Ingeniería en Desarrollo y Gestión de Software que consigue su primer trabajo en una empresa de tecnología.
- Personalidad: Es curioso, responsable y tiene ganas de aprender, aunque al principio puede sentirse inseguro al enfrentarse a problemas reales.
- Objetivo: Resolver los problemas tecnológicos que aparecen en la empresa y demostrar sus habilidades.
- Qué hace en el juego: Alex investiga los problemas, analiza las situaciones, busca pistas y realiza diferentes tareas para encontrar soluciones.
- Habilidades: Soporte técnico, programación, redes, servidores, bases de datos y ciberseguridad.
- Interacciones: Puede hablar con otros personajes, reparar equipos, programar, trabajar con redes y servidores y atender incidentes de seguridad.
- Evolución: Conforme avanza en las misiones, adquiere experiencia y se enfrenta a problemas cada vez más complicados.
- Diseño: Tiene un estilo pixel art 2D, con cabello oscuro, ropa casual y una mochila. Cuenta con diferentes animaciones para caminar, correr, interactuar, reparar, programar y celebrar.
- Expresiones: Puede mostrarse normal, feliz, preocupado, sorprendido, confundido, pensativo, determinado, concentrado, cansado y celebrando.

### Hoja de Sprite de Alex

![Hoja de Sprite de Alex](../imagenes/03-hoja-de-sprite-de-alex.png)

## Enemigo principal

### Saboteador

- Rol: Es el antagonista principal de TI: Soporte en Acción y representa una amenaza interna dentro de NovaTech Solutions.
- Historia: Es un empleado de NovaTech Solutions que, debido a su conocimiento de la infraestructura tecnológica de la empresa, tiene acceso a diferentes sistemas, equipos y áreas internas. Aprovecha su posición para provocar incidentes tecnológicos y crear problemas que parecen accidentes o errores comunes. Mientras el equipo de TI intenta solucionar los incidentes, él procura mantenerse oculto y evitar que descubran su participación.
- Personalidad: Es discreto, calculador, paciente y observador. Evita llamar la atención y procura comportarse como cualquier otro empleado. Antes de actuar analiza la situación y busca aprovechar las vulnerabilidades existentes. Cuando sospecha que alguien está investigando los incidentes, intenta desviar las sospechas mediante pistas falsas.
- Objetivo: Provocar problemas dentro de la infraestructura tecnológica de NovaTech Solutions sin ser descubierto y dificultar las investigaciones del Departamento de TI.
- Qué hace en el juego: Provoca diferentes incidentes tecnológicos que Alex debe investigar. Algunos problemas inicialmente parecen simples fallas técnicas, pero conforme avanza la historia aparecen patrones que hacen sospechar que alguien está interviniendo deliberadamente.
- Habilidades: Conocimientos de redes, sistemas, servidores, configuraciones de equipos, infraestructura tecnológica y seguridad informática. También sabe identificar procedimientos internos y aprovechar los conocimientos que posee como empleado.
- Interacciones: Puede aparecer ocasionalmente dentro de la empresa como un empleado aparentemente normal. Puede hablar con Alex, Martín y otros trabajadores. También puede aparecer en determinadas escenas realizando actividades que inicialmente parecen normales, pero que posteriormente adquieren importancia para la investigación.

### Hoja de Sprite del Saboteador

![Hoja de Sprite del Saboteador](../imagenes/04-hoja-de-sprite-del-saboteador.png)

## Personajes secundarios

### Martín

- Rol: Es el jefe del Departamento de TI
- Personalidad: Es serio, exigente, analítico y responsable. No suele dar las respuestas directamente, ya que prefiere que Alex investigue y encuentre las soluciones por sí mismo. Aunque puede parecer estricto, realmente busca preparar a Alex para enfrentar problemas reales.
- Objetivo: Mantener funcionando correctamente la infraestructura tecnológica de NovaTech Solutions, proteger los sistemas de la empresa y ayudar a Alex a desarrollar las habilidades necesarias para convertirse en un buen profesional de TI.
- Qué hace en el juego: Asigna misiones a Alex, presenta problemas tecnológicos, proporciona pistas, supervisa las soluciones y evalúa las decisiones del jugador. También interviene directamente cuando ocurre un problema crítico que afecta a la empresa.
- Habilidades: Soporte técnico, administración de redes, servidores, bases de datos, programación, sistemas empresariales, ciberseguridad, diagnóstico de problemas y gestión de incidentes.
- Interacciones: Puede hablar con Alex y otros empleados, asignar tareas, revisar equipos, analizar registros de sistemas, supervisar reparaciones, investigar incidentes de seguridad y proporcionar información importante para resolver determinadas misiones al jugador pensar antes de solucionar un problema.
- Diseño: Martín tiene un estilo pixel art 2D y apariencia de jefe de TI experimentado. Utiliza ropa de oficina, gafete y una tablet o laptop para revisar incidentes y supervisar a Alex.

### Hoja de Sprite de Martín

![Hoja de Sprite de Martín](../imagenes/05-hoja-de-sprite-de-martin.png)

### Richar

- Rol: Especialista en Redes y Administrador de Infraestructura de NovaTech Solutions.
- Personalidad: Es tranquilo, práctico y directo. Prefiere solucionar los problemas con acciones concretas en lugar de perder tiempo con explicaciones innecesarias.
- Habilidades: Redes, direccionamiento IP, configuración de routers y switches, cableado estructurado, mantenimiento de servidores, diagnóstico de conectividad, configuración de dispositivos y seguridad de redes.
- Evolución: Al principio considera a Alex un principiante que necesita supervisión. Conforme Alex demuestra que puede resolver problemas de conectividad y configurar correctamente los equipos, Richar comienza a confiar más en él y le permite trabajar directamente con la infraestructura de red. Más adelante puede pedirle ayuda durante problemas de red que afectan a varios departamentos.
- Diseño: Tiene un estilo pixel art 2D. Es un hombre adulto con apariencia de técnico experimentado. Tiene cabello corto, barba ligera y utiliza ropa de trabajo cómoda, como una camisa o camiseta, pantalón cargo y botas. Lleva un gafete de NovaTech Solutions y una mochila o cinturón con herramientas. Puede llevar una laptop y herramientas para trabajar con cableado de red. Cuenta con diferentes animaciones para caminar, revisar cables, configurar equipos, utilizar una laptop, agacharse para reparar conexiones y analizar dispositivos de red.
- Expresiones: Puede mostrarse tranquilo, concentrado, serio, confundido, sorprendido, preocupado, pensativo, satisfecho, divertido y determinado.

### Hoja de Sprite de Richar

![Hoja de Sprite de Richar](../imagenes/06-hoja-de-sprite-de-richar.jpeg)

### Sofía

- Rol: Empleada del área de Contabilidad de NovaTech Solutions y uno de los primeros personajes que solicita ayuda a Alex.
- Personalidad: Es amable, responsable y organizada. Puede ponerse nerviosa cuando algo no funciona correctamente, especialmente cuando necesita acceder a información importante. Está dispuesta a colaborar con Alex y responder sus preguntas para ayudarlo a encontrar la causa del problema.
- Objetivo: Poder realizar normalmente sus actividades de contabilidad y resolver los problemas tecnológicos que interfieren con su trabajo.
- Qué hace en el juego: Reporta problemas al Departamento de TI, explica lo que estaba haciendo cuando ocurrió el incidente y proporciona información que ayuda a Alex a investigar. También puede participar en misiones relacionadas con computadoras, cuentas de usuario y aplicaciones empresariales.
- Habilidades: Conocimientos de contabilidad, administración de documentos, manejo de sistemas administrativos y herramientas de oficina. Tiene conocimientos básicos de informática, pero no es especialista en soporte técnico.
- Diseño: Tiene un estilo pixel art 2D, con apariencia de empleada de oficina. Puede utilizar ropa formal o semiformal, gafete de NovaTech Solutions y llevar una carpeta, documentos o una tablet. Trabaja principalmente frente a una computadora en el área de Contabilidad. Cuenta con animaciones para caminar, trabajar en la computadora, hablar con Alex, revisar documentos y reaccionar ante problemas.

### Hoja de Sprite de Sofia

![Hoja de Sprite de Sofia](../imagenes/07-hoja-de-sprite-de-sofia.jpeg)

### Marco

- Rol: Desarrollador de software de NovaTech Solutions y especialista en programación.
- Personalidad: Es inteligente, curioso y bastante concentrado cuando trabaja. Puede pasar mucho tiempo frente a su computadora buscando la causa de un error. Tiene un carácter tranquilo, aunque puede frustrarse cuando un problema de programación no tiene una solución evidente. Le gusta explicar conceptos mediante ejemplos prácticos.
- Objetivo: Mantener funcionando correctamente las aplicaciones y sistemas desarrollados por el equipo de software, además de corregir errores que puedan afectar a los usuarios.
- Qué hace en el juego: Solicita ayuda a Alex cuando encuentra problemas técnicos relacionados con aplicaciones, código o sistemas. También puede plantearle pequeños desafíos de programación donde el jugador debe analizar código, encontrar errores y seleccionar la solución correcta.
- Habilidades: Programación, desarrollo de aplicaciones, bases de datos, análisis de errores, lógica de programación, estructuras de datos y mantenimiento de software.
- Diseño: Tiene un estilo pixel art 2D, con apariencia de desarrollador de software. Utiliza ropa casual, gafete de NovaTech Solutions y suele llevar una laptop. Puede tener audífonos alrededor del cuello y trabajar frente a varios monitores con código en pantalla. Cuenta con animaciones para caminar, escribir código, revisar errores, utilizar la computadora, hablar con Alex y celebrar cuando consigue solucionar un problema.

### Hoja de Sprite de Marco

![Hoja de Sprite de Marco](../imagenes/08-hoja-de-sprite-de-marco.jpeg)

## Niveles

El juego está dividido en diferentes niveles. Cada uno presenta una situación en la que Alex deberá investigar el problema, tomar decisiones y encontrar una solución. Conforme avance, los retos serán más complejos y se desbloquearán nuevas áreas y habilidades.

### Primer nivel El Primer Día

- Objetivo: Presentar a Alex, la empresa NovaTech Solutions y las mecánicas principales del juego.
- Situación: Alex comienza su primer día de trabajo en la empresa.
- Problema: Sofía no puede iniciar sesión en su computadora.
- El jugador deberá:
- Mover a Alex.
- Interactuar con objetos y personajes.
- Recibir y revisar tickets.
- Investigar el problema antes de realizar cambios.
- Tomar decisiones.
- Resolver y cerrar el ticket.
- Solución: El problema se debe a que Bloq Mayús está activado.
- Recompensa:
- +100 XP de Soporte al Usuario.
- +15 de Reputación.
- Desbloquea: Taller de reparación.
- Nuevo problema: Al finalizar el nivel, Alex recibe un reporte de que tres estaciones de trabajo de Contabilidad se apagaron después de percibirse un olor a plástico quemado.

### Segundo nivel Problemas de Hardware

- Área: Taller de reparación.
- Situación: Tres computadoras del área de Contabilidad dejaron de funcionar.
- El jugador deberá:
- Revisar visualmente los equipos.
- Revisar cables y conexiones.
- Escuchar posibles sonidos extraños.
- Utilizar un multímetro.
- Identificar el componente defectuoso.
- Sustituir la pieza dañada.
- Problemas encontrados:
- PC04: No enciende.
- PC05: Enciende, pero se apaga después de unos segundos.
- PC06: Funciona, pero presenta olor a quemado.
- Componentes que se pueden identificar:
- Fuente de poder.
- Memoria RAM.
- Disco SSD.
- Ventilador.
- Tarjeta madre.
- Solución: Alex deberá descubrir que las tres fallas están relacionadas con un problema de alimentación eléctrica.

### Tercer nivel Sin Conexión

- Área: Redes.
- Situación: Varios empleados comienzan a reportar que no tienen conexión a Internet.
- Problema: La falla afecta a varias computadoras al mismo tiempo.
- El jugador aprenderá sobre:
- Cable Ethernet.
- Switch.
- Router.
- Dirección IP.
- Gateway.
- DNS.
- ping.
- El jugador deberá:
- Revisar la conexión de los equipos.
- Comprobar los cables.
- Revisar el switch y el router.
- Realizar pruebas de conexión.
- Identificar dónde se encuentra la interrupción.
- Recorrido de la conexión: PC → Switch → Router → Internet
- Objetivo: Encontrar el punto donde se perdió la conexión y solucionar el problema.
- Recompensa: Desbloquea la habilidad Diagnóstico de Redes I.

## Storyboard

### Primer nivel

![Primer nivel](../imagenes/09-primer-nivel.png)

### Segundo nivel

![Segundo nivel](../imagenes/10-segundo-nivel.jpeg)

### Tercer nivel

![Tercer nivel](../imagenes/11-tercer-nivel.png)

La referencia visual de esta sección muestra el taller de hardware. Para el tercer nivel se desarrollará una escena de diagnóstico de redes con Richar, cables, switch y router.

## Canvas

![Canvas](../imagenes/12-canvas.png)

## Plan de monetización

Para TI: Soporte en Acción se propone un modelo Premium de compra única. El jugador compraría el videojuego una sola vez y tendría acceso a la historia principal, las misiones y los contenidos educativos sin publicidad ni pagos obligatorios durante la partida.

La idea es que el jugador pueda avanzar aprendiendo y resolviendo problemas de soporte técnico, redes, hardware, programación y ciberseguridad, sin tener que comprar herramientas, habilidades o pistas con dinero real.

¿De dónde vendrían los ingresos?

La principal fuente de ingresos sería la venta del videojuego completo.

Si el proyecto crece, también podrían desarrollarse expansiones con nuevos contenidos, por ejemplo:

Ciberseguridad avanzada

Administración de servidores

Redes empresariales

Reparación avanzada de hardware

Nuevas misiones del Saboteador

Nuevas áreas dentro de NovaTech Solutions

También podría existir una versión educativa para escuelas o universidades, donde una institución pague por varias licencias para que los estudiantes utilicen el juego como apoyo en clases relacionadas con informática y soporte técnico.

| Aspecto | Propuesta para TI: Soporte en Acción |
| --- | --- |
| Modelo | Compra única / Premium |
| Precio inicial tentativo | $99–$119 MXN |
| Plataforma inicial | PC |
| Distribución | Tiendas digitales |
| Publicidad | No |
| Microtransacciones | No |
| Contenido adicional | Expansiones o DLC opcionales |
| Demo | Gratuita con las primeras misiones |
| Público adicional | Escuelas, universidades y centros de capacitación |

El precio y la venta comercial son propuestas futuras. El MVP académico no implementará cobros, licencias comerciales ni pagos en las aplicaciones complementarias.

## Organización del trabajo individual

Responsable único del proyecto: Brian Jesús Mendoza Márquez, matrícula 230308.

| Función | Actividades a cargo del responsable |
| --- | --- |
| Análisis y diseño | Definir requisitos, alcance, roles, historia, misiones y pantallas. |
| Desarrollo del videojuego | Programar escenas, interacciones y retos en Unity. |
| Desarrollo móvil | Construir la aplicación Flutter y conectar sus pantallas a la API. |
| Desarrollo web | Construir la PWA en React y el panel administrativo. |
| Servicios y datos | Implementar la API, autorización y almacenamiento. |
| Pruebas y documentación | Verificar el funcionamiento y preparar evidencias y presentación. |

El orden de trabajo corresponde al plan individual de etapas incluido al inicio. Las fechas de entrega se ajustarán a los calendarios de las asignaturas.

## Bocetos de la aplicación web progresiva

Propuesta visual para la PWA en React de TI: Soporte en Acción. Cada pantalla se presenta por separado en formato de escritorio. Los datos mostrados son ejemplos de la interfaz.

### Pantalla 1 Inicio de sesión

![Pantalla 1 Inicio de sesión](../imagenes/13-pantalla-1-inicio-de-sesion.png)

Permite acceder a la cuenta con correo y contraseña, recordar la sesión y recuperar el acceso.

### Pantalla 2 Registro

![Pantalla 2 Registro](../imagenes/14-pantalla-2-registro.png)

Solicita los campos obligatorios para crear una cuenta. El nombre de usuario y el correo electrónico deben ser únicos.

### Pantalla 3 Inicio

![Pantalla 3 Inicio](../imagenes/15-pantalla-3-inicio.png)

Resume las misiones completadas, los puntos de experiencia y el rango del jugador. Ofrece acceso a misiones y guías recomendadas.

### Pantalla 4 Misiones

![Pantalla 4 Misiones](../imagenes/16-pantalla-4-misiones.png)

Presenta el catálogo de misiones con búsqueda, filtros por estado y acceso al detalle de cada misión.

### Pantalla 5 Detalle de misión

![Pantalla 5 Detalle de misión](../imagenes/17-pantalla-5-detalle-de-mision.png)

Muestra el objetivo, los pasos, las recompensas y una guía relacionada. La misión se realiza en el videojuego de PC.

### Pantalla 6 Guías

![Pantalla 6 Guías](../imagenes/18-pantalla-6-guias.png)

Organiza las guías por Hardware, Redes y Seguridad e incluye un buscador y acceso a su lectura.

### Pantalla 7 Lectura de guía

![Pantalla 7 Lectura de guía](../imagenes/19-pantalla-7-lectura-de-guia.png)

Presenta una guía de diagnóstico con pasos, ilustración e índice para facilitar la consulta.

### Pantalla 8 Progreso

![Pantalla 8 Progreso](../imagenes/20-pantalla-8-progreso.png)

Muestra el avance de las misiones, la experiencia, la reputación y el historial de resultados.

### Pantalla 9 Perfil

![Pantalla 9 Perfil](../imagenes/21-pantalla-9-perfil.png)

Permite consultar y editar los datos de la cuenta, cambiar la contraseña y cerrar sesión.

## Bocetos de la aplicación móvil

Propuesta visual para la aplicación móvil en Flutter de TI: Soporte en Acción. Las imágenes se presentan en el orden de los archivos proporcionados y con títulos genéricos por orden; los datos que muestran son ejemplos de la interfaz.

### Pantalla móvil 1

![Pantalla móvil 1](../imagenes/movil1.png)

### Pantalla móvil 2

![Pantalla móvil 2](../imagenes/movil2.png)

### Pantalla móvil 3

![Pantalla móvil 3](../imagenes/movil3.png)

### Pantalla móvil 4

![Pantalla móvil 4](../imagenes/movil4.png)

### Pantalla móvil 5

![Pantalla móvil 5](../imagenes/movil5.png)

### Pantalla móvil 6

![Pantalla móvil 6](../imagenes/6.png)

### Pantalla móvil 7

![Pantalla móvil 7](../imagenes/movil7.png)

### Pantalla móvil 8

![Pantalla móvil 8](../imagenes/movil8.png)

### Pantalla móvil 9

![Pantalla móvil 9](../imagenes/movil9.png)

### Pantalla móvil 10

![Pantalla móvil 10](../imagenes/movil10.png)

### Pantalla móvil 11

![Pantalla móvil 11](../imagenes/movil11.png)

### Pantalla móvil 12

![Pantalla móvil 12](../imagenes/movil12.png)

### Pantalla móvil 13

![Pantalla móvil 13](../imagenes/movil13.png)

### Pantalla móvil 14

![Pantalla móvil 14](../imagenes/movil14.png)

### Pantalla móvil 15

![Pantalla móvil 15](../imagenes/movil15.png)

### Pantalla móvil 16

![Pantalla móvil 16](../imagenes/movil16.png)

### Pantalla móvil 17

![Pantalla móvil 17](../imagenes/movil17.png)

### Pantalla móvil 18

![Pantalla móvil 18](../imagenes/movil18.png)

### Pantalla móvil 19

![Pantalla móvil 19](../imagenes/movil19.png)

### Pantalla móvil 20

![Pantalla móvil 20](../imagenes/movil20.png)

### Pantalla móvil 21

![Pantalla móvil 21](../imagenes/movil21.png)

## Paleta de colores complementaria

Propuesta acordada en la conversación para Flutter y React. Los bocetos permanecen a lápiz; esta tabla define el acabado a color.

| Color | HEX | Uso |
| --- | --- | --- |
| Azul noche | `#0F172A` | Barra superior, menú lateral y texto principal sobre fondos claros. |
| Azul principal | `#1D4ED8` | Botones principales con texto blanco. |
| Cian | `#06B6D4` | Acentos e indicadores con texto azul noche. |
| Gris muy claro | `#F1F5F9` | Fondo general. |
| Blanco | `#FFFFFF` | Tarjetas, formularios y áreas de lectura. |
| Gris pizarra | `#475569` | Texto secundario sobre fondos claros. |
| Gris medio | `#64748B` | Bordes de campos e iconos de apoyo. |
| Verde | `#15803D` | Confirmaciones y misiones completadas, con texto blanco. |
| Ámbar | `#F59E0B` | Avisos y estados pendientes, con texto azul noche. |
| Rojo | `#B91C1C` | Errores, con texto blanco. |

Acompañar los estados con texto o iconos; no transmitir su significado únicamente mediante color.

## Uso de este paquete

1. Descomprime el paquete.
2. Coloca `README.md` y `docs/imagenes/` en la raíz del mismo repositorio, conservando las rutas relativas.
3. Si ya tienes un README, integra su información útil antes de sustituirlo.
4. Copia el contenido del prompt de OpenCode desde la carpeta del repositorio.

El archivo [inventario-imagenes.json](../inventario-imagenes.json) relaciona cada imagen con su sección. Las ilustraciones de sprites y storyboards son material conceptual; no son recursos jugables listos para importar sin revisión.

> Nota de integración: este documento es la fuente completa del proyecto y se conserva en `docs/proyecto/README.md`. Las rutas de las imágenes se ajustaron a `../imagenes/` para que resuelvan desde esta ubicación. El archivo `PROMPT_OPENCODE.md` mencionado en el paquete original no forma parte de este repositorio.
