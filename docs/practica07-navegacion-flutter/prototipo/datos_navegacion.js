/* Datos locales: no se realiza ninguna llamada a servidor. */
window.TI_DATA = {
  "nodes": [
    {
      "id": "P01",
      "title": "Splash Screen",
      "roles": [
        "Público"
      ],
      "description": "Presenta el proyecto y a Alex antes de comprobar la sesión.",
      "elements": [
        "Logotipo del videojuego",
        "Ilustración de Alex",
        "Nombre del producto"
      ],
      "actions": [
        "Iniciar automáticamente"
      ],
      "related": [
        "P02"
      ],
      "requirements": [
        "RNF-04"
      ],
      "story": "HU-01",
      "route": "/splash",
      "image": "../sketches/P01_Splash_Screen.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P02",
      "title": "Loading Screen",
      "roles": [
        "Público"
      ],
      "description": "Muestra el progreso mientras se restauran configuración y sesión.",
      "elements": [
        "Logotipo",
        "Barra de carga",
        "Indicador de actividad"
      ],
      "actions": [
        "Esperar",
        "Reintentar si falla la carga"
      ],
      "related": [
        "P03",
        "P09"
      ],
      "requirements": [
        "RNF-02",
        "RF-02"
      ],
      "story": "HU-01",
      "route": "/cargando",
      "image": "../sketches/P02_Loading_Screen.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P03",
      "title": "Iniciar sesión",
      "roles": [
        "Público"
      ],
      "description": "Solicita credenciales y dirige al usuario autorizado a su inicio.",
      "elements": [
        "Correo",
        "Contraseña",
        "Mostrar/ocultar contraseña",
        "Ingresar",
        "Crear cuenta"
      ],
      "actions": [
        "Iniciar sesión",
        "Crear cuenta",
        "Recuperar acceso",
        "Consultar acerca del sistema"
      ],
      "related": [
        "P04",
        "P05",
        "P07",
        "P09",
        "P10"
      ],
      "requirements": [
        "RF-02",
        "RF-03",
        "RNF-01"
      ],
      "story": "HU-02",
      "route": "/acceso",
      "image": "../sketches/P03_Iniciar_Sesion.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P04",
      "title": "Recuperar cuenta",
      "roles": [
        "Público"
      ],
      "description": "Inicia el proceso de recuperación por correo verificado.",
      "elements": [
        "Campo de correo",
        "Botón de envío",
        "Mensaje neutro de confirmación"
      ],
      "actions": [
        "Solicitar código",
        "Volver al ingreso"
      ],
      "related": [
        "P03",
        "P06"
      ],
      "requirements": [
        "RF-04",
        "RNF-01"
      ],
      "story": "HU-03",
      "route": "/recuperacion",
      "image": "../sketches/P04_Recuperar_Cuenta.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P05",
      "title": "Cuenta bloqueada",
      "roles": [
        "Público"
      ],
      "description": "Informa que se suspendió temporalmente el inicio de sesión después de intentos fallidos.",
      "elements": [
        "Ícono candado",
        "Explicación de bloqueo",
        "Acción de desbloqueo"
      ],
      "actions": [
        "Solicitar desbloqueo",
        "Volver"
      ],
      "related": [
        "P03",
        "P06"
      ],
      "requirements": [
        "RF-05",
        "RNF-01"
      ],
      "story": "HU-04",
      "route": "/bloqueo",
      "image": "../sketches/P05_Cuenta_Bloqueada.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P06",
      "title": "Código de desbloqueo",
      "roles": [
        "Público"
      ],
      "description": "Valida un código de un solo uso enviado por un medio verificado.",
      "elements": [
        "Casillas de código",
        "Confirmar",
        "Reenviar con espera"
      ],
      "actions": [
        "Verificar código",
        "Reenviar código",
        "Regresar"
      ],
      "related": [
        "P03",
        "P04",
        "P05"
      ],
      "requirements": [
        "RF-04",
        "RF-05"
      ],
      "story": "HU-04",
      "route": "/verificar-codigo",
      "image": "../sketches/P06_Codigo_Desbloqueo.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P07",
      "title": "Acerca del software",
      "roles": [
        "Público"
      ],
      "description": "Explica el juego, sus objetivos educativos y plataformas de aprendizaje.",
      "elements": [
        "Logo",
        "Descripción",
        "Módulos",
        "Créditos"
      ],
      "actions": [
        "Ver equipo",
        "Volver"
      ],
      "related": [
        "P03",
        "P08",
        "P09"
      ],
      "requirements": [
        "RF-06"
      ],
      "story": "HU-05",
      "route": "/acerca",
      "image": "../sketches/P07_Acerca_Del_Software.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P08",
      "title": "Acerca del equipo / empresa",
      "roles": [
        "Público"
      ],
      "description": "Presenta NovaTech Solutions, los integrantes y el contexto académico.",
      "elements": [
        "Nombre de empresa",
        "Ilustración del equipo",
        "Resumen",
        "Volver"
      ],
      "actions": [
        "Regresar a acerca"
      ],
      "related": [
        "P07"
      ],
      "requirements": [
        "RF-06"
      ],
      "story": "HU-05",
      "route": "/equipo",
      "image": "../sketches/P08_Acerca_Del_Equipo.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P09",
      "title": "Inicio (Home)",
      "roles": [
        "Autenticado"
      ],
      "description": "Es la entrada después de la autenticación: accesos acordes al rol, misión próxima o módulo de gestión.",
      "elements": [
        "Bienvenida",
        "Estado de sesión",
        "Accesos rápidos por rol",
        "Menú de navegación"
      ],
      "actions": [
        "Abrir módulo permitido",
        "Abrir perfil",
        "Cerrar sesión desde perfil"
      ],
      "related": [
        "P07",
        "P11",
        "P13",
        "P15",
        "P16",
        "P17",
        "P18",
        "P19",
        "P20"
      ],
      "requirements": [
        "RF-07",
        "RF-16",
        "RNF-01"
      ],
      "story": "HU-06",
      "route": "/inicio",
      "image": "../sketches/P09_Inicio_Home.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P10",
      "title": "Registro de cuenta",
      "roles": [
        "Público"
      ],
      "description": "Permite solicitar una cuenta de estudiante y comprueba campos obligatorios y duplicados.",
      "elements": [
        "Nombre",
        "Correo",
        "Contraseña",
        "Confirmación",
        "Registrarme"
      ],
      "actions": [
        "Registrar",
        "Corregir campos",
        "Regresar a login"
      ],
      "related": [
        "P03"
      ],
      "requirements": [
        "RF-01",
        "RNF-01"
      ],
      "story": "HU-07",
      "route": "/registro",
      "image": "../sketches/P10_Registro.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P11",
      "title": "Misiones",
      "roles": [
        "Estudiante/Jugador"
      ],
      "description": "Lista misiones disponibles, completadas y bloqueadas y el XP potencial.",
      "elements": [
        "Tarjetas de misión",
        "Estado",
        "Nivel",
        "XP",
        "Barra inferior"
      ],
      "actions": [
        "Filtrar",
        "Seleccionar misión",
        "Volver a inicio"
      ],
      "related": [
        "P09",
        "P12",
        "P15"
      ],
      "requirements": [
        "RF-08",
        "RF-10"
      ],
      "story": "HU-08",
      "route": "/misiones",
      "image": "../sketches/P11_Misiones.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P12",
      "title": "Detalle de misión",
      "roles": [
        "Estudiante/Jugador"
      ],
      "description": "Expone objetivos, requisitos y recompensas; la ejecución 2D sucede en Unity en PC.",
      "elements": [
        "Título",
        "Objetivos",
        "Recompensa XP",
        "Estado de misión",
        "Acceso a guía"
      ],
      "actions": [
        "Consultar guía",
        "Volver a misiones",
        "Leer indicación de uso en PC"
      ],
      "related": [
        "P11",
        "P13"
      ],
      "requirements": [
        "RF-08",
        "RF-09"
      ],
      "story": "HU-08",
      "route": "/misiones/detalle",
      "image": "../sketches/P12_Detalle_Mision.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P13",
      "title": "Guías de apoyo",
      "roles": [
        "Estudiante/Jugador"
      ],
      "description": "Organiza referencias educativas sobre soporte, hardware, redes y seguridad.",
      "elements": [
        "Categorías",
        "Tarjetas de guías",
        "Navegación inferior"
      ],
      "actions": [
        "Abrir guía",
        "Volver al inicio"
      ],
      "related": [
        "P09",
        "P12",
        "P14"
      ],
      "requirements": [
        "RF-11"
      ],
      "story": "HU-09",
      "route": "/guias",
      "image": "../sketches/P13_Guias.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P14",
      "title": "Lectura de guía",
      "roles": [
        "Estudiante/Jugador"
      ],
      "description": "Muestra una guía concreta con pasos e ilustraciones a lápiz.",
      "elements": [
        "Título",
        "Categoría",
        "Pasos numerados",
        "Imagen de apoyo"
      ],
      "actions": [
        "Leer",
        "Regresar a guías",
        "Volver a misión contextual"
      ],
      "related": [
        "P12",
        "P13"
      ],
      "requirements": [
        "RF-11"
      ],
      "story": "HU-09",
      "route": "/guias/lectura",
      "image": "../sketches/P14_Lectura_Guia.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P15",
      "title": "Mi progreso",
      "roles": [
        "Estudiante/Jugador"
      ],
      "description": "Consulta misiones completadas, experiencia y reputación, sincronizadas con el sistema.",
      "elements": [
        "Porcentaje",
        "Barra de progreso",
        "XP",
        "Historial"
      ],
      "actions": [
        "Actualizar",
        "Abrir misiones",
        "Volver a inicio"
      ],
      "related": [
        "P09",
        "P11",
        "P16"
      ],
      "requirements": [
        "RF-10",
        "RF-12"
      ],
      "story": "HU-10",
      "route": "/progreso",
      "image": "../sketches/P15_Progreso.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P16",
      "title": "Mi perfil",
      "roles": [
        "Autenticado"
      ],
      "description": "Permite consultar/editar datos básicos y finalizar sesión.",
      "elements": [
        "Avatar",
        "Nombre visible",
        "Correo",
        "Guardar cambios",
        "Cerrar sesión"
      ],
      "actions": [
        "Editar nombre",
        "Guardar",
        "Ver progreso solo estudiante",
        "Cerrar sesión"
      ],
      "related": [
        "P03",
        "P09",
        "P15"
      ],
      "requirements": [
        "RF-13",
        "RF-02"
      ],
      "story": "HU-11",
      "route": "/perfil",
      "image": "../sketches/P16_Perfil.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P17",
      "title": "Panel docente",
      "roles": [
        "Docente/Evaluador"
      ],
      "description": "Consulta progreso académico agregado del grupo y evaluaciones de estudiantes asignados.",
      "elements": [
        "Resumen de estudiantes",
        "Barras de avance",
        "Estado de misiones"
      ],
      "actions": [
        "Consultar seguimiento",
        "Regresar al inicio"
      ],
      "related": [
        "P09"
      ],
      "requirements": [
        "RF-14",
        "RF-16"
      ],
      "story": "HU-12",
      "route": "/docente",
      "image": "../sketches/P17_Panel_Docente.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P18",
      "title": "Gestión de usuarios",
      "roles": [
        "Administrador"
      ],
      "description": "Visualiza, filtra y administra cuentas autorizadas sin revelar contraseñas.",
      "elements": [
        "Buscador",
        "Listado",
        "Rol",
        "Estado",
        "Acción de alta"
      ],
      "actions": [
        "Consultar",
        "Cambiar rol si autorizado",
        "Activar o suspender",
        "Regresar"
      ],
      "related": [
        "P09"
      ],
      "requirements": [
        "RF-15",
        "RF-16"
      ],
      "story": "HU-13",
      "route": "/admin/usuarios",
      "image": "../sketches/P18_Gestion_Usuarios.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P19",
      "title": "Editor de misiones",
      "roles": [
        "Editor de contenido"
      ],
      "description": "Permite proponer o editar título, objetivos y recompensa de misiones educativas.",
      "elements": [
        "Campos de misión",
        "Categoría",
        "Nivel",
        "Recompensa",
        "Guardar"
      ],
      "actions": [
        "Editar borrador",
        "Guardar",
        "Volver"
      ],
      "related": [
        "P09"
      ],
      "requirements": [
        "RF-17",
        "RF-16"
      ],
      "story": "HU-14",
      "route": "/editor/misiones",
      "image": "../sketches/P19_Editor_Misiones.png",
      "platform": "Flutter (Android/iOS)"
    },
    {
      "id": "P20",
      "title": "Reportes institucionales",
      "roles": [
        "Institución educativa"
      ],
      "description": "Presenta indicadores agregados y reportes académicos según el ámbito autorizado.",
      "elements": [
        "Selector de periodo",
        "Tarjetas KPI",
        "Gráfica de barras",
        "Acceso a reporte"
      ],
      "actions": [
        "Filtrar",
        "Consultar resumen",
        "Regresar"
      ],
      "related": [
        "P09"
      ],
      "requirements": [
        "RF-18",
        "RF-16"
      ],
      "story": "HU-15",
      "route": "/institucion/reportes",
      "image": "../sketches/P20_Reportes_Institucion.png",
      "platform": "Flutter (Android/iOS)"
    }
  ],
  "edges": [
    {
      "from_id": "P01",
      "to_id": "P02",
      "condition": "Inicio de la aplicación",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P02",
      "to_id": "P03",
      "condition": "Sin sesión, token inválido o cierre previo",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P02",
      "to_id": "P09",
      "condition": "Sesión válida: cargar inicio según rol",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P03",
      "to_id": "P09",
      "condition": "Credenciales válidas, cuenta activa y permisos",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P03",
      "to_id": "P03",
      "condition": "Credenciales incorrectas bajo el umbral",
      "role": "Todos",
      "kind": "error"
    },
    {
      "from_id": "P03",
      "to_id": "P05",
      "condition": "Exceso de intentos fallidos",
      "role": "Todos",
      "kind": "error"
    },
    {
      "from_id": "P03",
      "to_id": "P04",
      "condition": "Selecciona recuperación",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P03",
      "to_id": "P10",
      "condition": "Selecciona crear cuenta",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P03",
      "to_id": "P07",
      "condition": "Selecciona información del software",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P04",
      "to_id": "P06",
      "condition": "Solicitud aceptada: verificar código",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P04",
      "to_id": "P03",
      "condition": "Cancelar y regresar",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P05",
      "to_id": "P06",
      "condition": "Solicitar desbloqueo",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P05",
      "to_id": "P03",
      "condition": "Regresar; el bloqueo se mantiene",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P06",
      "to_id": "P03",
      "condition": "Código correcto y desbloqueo o recuperación confirmados",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P06",
      "to_id": "P06",
      "condition": "Código incorrecto o caducado",
      "role": "Todos",
      "kind": "error"
    },
    {
      "from_id": "P06",
      "to_id": "P04",
      "condition": "Reiniciar recuperación",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P10",
      "to_id": "P03",
      "condition": "Registro validado y sin duplicados",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P10",
      "to_id": "P10",
      "condition": "Campos inválidos o cuenta duplicada",
      "role": "Todos",
      "kind": "error"
    },
    {
      "from_id": "P07",
      "to_id": "P08",
      "condition": "Ver equipo y empresa",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P08",
      "to_id": "P07",
      "condition": "Regresar",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P07",
      "to_id": "P03",
      "condition": "Volver sin autenticación",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P07",
      "to_id": "P09",
      "condition": "Volver con sesión válida",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P07",
      "condition": "Ver información del software",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P11",
      "condition": "Estudiante: consultar misiones",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P13",
      "condition": "Estudiante: abrir guías",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P15",
      "condition": "Estudiante: consultar progreso",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P16",
      "condition": "Abrir perfil",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P11",
      "to_id": "P12",
      "condition": "Misión disponible o consultar misión bloqueada",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P11",
      "to_id": "P09",
      "condition": "Regresar al inicio",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P11",
      "to_id": "P15",
      "condition": "Ir a progreso",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P12",
      "to_id": "P13",
      "condition": "Consultar guía de apoyo",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P12",
      "to_id": "P11",
      "condition": "Regresar a misiones",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P13",
      "to_id": "P14",
      "condition": "Seleccionar guía",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P13",
      "to_id": "P12",
      "condition": "Volver al detalle de la misión de origen",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P13",
      "to_id": "P09",
      "condition": "Regresar al inicio",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P14",
      "to_id": "P13",
      "condition": "Volver al catálogo de guías",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P14",
      "to_id": "P12",
      "condition": "Volver al detalle si se conserva el contexto",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P15",
      "to_id": "P11",
      "condition": "Ir a misiones",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P15",
      "to_id": "P09",
      "condition": "Regresar al inicio",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P15",
      "to_id": "P16",
      "condition": "Abrir perfil",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P16",
      "to_id": "P15",
      "condition": "Solo estudiante: ver mi progreso",
      "role": "Estudiante/Jugador",
      "kind": "normal"
    },
    {
      "from_id": "P16",
      "to_id": "P09",
      "condition": "Volver al inicio",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P16",
      "to_id": "P03",
      "condition": "Cerrar sesión, invalidar sesión local y remota",
      "role": "Todos",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P17",
      "condition": "Docente: seguimiento autorizado",
      "role": "Docente/Evaluador",
      "kind": "normal"
    },
    {
      "from_id": "P17",
      "to_id": "P09",
      "condition": "Regresar a inicio",
      "role": "Docente/Evaluador",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P18",
      "condition": "Administrador: módulo usuarios",
      "role": "Administrador",
      "kind": "normal"
    },
    {
      "from_id": "P18",
      "to_id": "P09",
      "condition": "Regresar a inicio",
      "role": "Administrador",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P19",
      "condition": "Editor: edición de misiones",
      "role": "Editor de contenido",
      "kind": "normal"
    },
    {
      "from_id": "P19",
      "to_id": "P09",
      "condition": "Regresar a inicio",
      "role": "Editor de contenido",
      "kind": "normal"
    },
    {
      "from_id": "P09",
      "to_id": "P20",
      "condition": "Institución: indicadores agregados",
      "role": "Institución educativa",
      "kind": "normal"
    },
    {
      "from_id": "P20",
      "to_id": "P09",
      "condition": "Regresar a inicio",
      "role": "Institución educativa",
      "kind": "normal"
    }
  ],
  "roles": [
    "Estudiante/Jugador",
    "Docente/Evaluador",
    "Administrador",
    "Editor de contenido",
    "Institución educativa"
  ],
  "scenarios": [
    {
      "name": "Acceso satisfactorio",
      "role": "Estudiante/Jugador",
      "path": [
        "P01",
        "P02",
        "P03",
        "P09"
      ],
      "detail": "Inicia la app y se autentica un estudiante."
    },
    {
      "name": "Recuperación de acceso",
      "role": "Estudiante/Jugador",
      "path": [
        "P03",
        "P04",
        "P06",
        "P03",
        "P09"
      ],
      "detail": "Correo registrado, código de verificación y acceso normal."
    },
    {
      "name": "Bloqueo y desbloqueo",
      "role": "Estudiante/Jugador",
      "path": [
        "P03",
        "P05",
        "P06",
        "P03"
      ],
      "detail": "Demasiados intentos; se verifica un código y se permite volver a login."
    },
    {
      "name": "Registro de estudiante",
      "role": "Estudiante/Jugador",
      "path": [
        "P03",
        "P10",
        "P03",
        "P09"
      ],
      "detail": "Nuevo usuario sin correo duplicado."
    },
    {
      "name": "Misión con guía",
      "role": "Estudiante/Jugador",
      "path": [
        "P09",
        "P11",
        "P12",
        "P13",
        "P14",
        "P13",
        "P09"
      ],
      "detail": "Consulta una misión para PC desde Flutter y lee la guía."
    },
    {
      "name": "Seguimiento de progreso",
      "role": "Estudiante/Jugador",
      "path": [
        "P09",
        "P15",
        "P16",
        "P03"
      ],
      "detail": "Revisa XP, abre perfil y cierra sesión."
    },
    {
      "name": "Acceso docente",
      "role": "Docente/Evaluador",
      "path": [
        "P03",
        "P09",
        "P17",
        "P09"
      ],
      "detail": "Ingresa y consulta únicamente seguimiento autorizado."
    },
    {
      "name": "Acceso administrador",
      "role": "Administrador",
      "path": [
        "P03",
        "P09",
        "P18",
        "P09"
      ],
      "detail": "Accede a gestión de usuarios."
    },
    {
      "name": "Acceso editor",
      "role": "Editor de contenido",
      "path": [
        "P03",
        "P09",
        "P19",
        "P09"
      ],
      "detail": "Accede a edición de misiones."
    },
    {
      "name": "Acceso institución",
      "role": "Institución educativa",
      "path": [
        "P03",
        "P09",
        "P20",
        "P09"
      ],
      "detail": "Accede a informes consolidados."
    }
  ]
};
