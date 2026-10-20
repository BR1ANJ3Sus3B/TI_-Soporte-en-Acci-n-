/* =====================================================================
   TI: Soporte en Acción — Business Model Canvas Interactivo
   app.js — datos del proyecto + motor de interacción (sin dependencias)
   ===================================================================== */
(function () {
  "use strict";

  /* ----------------------------- ICONOS ----------------------------- */
  var ICONS = {
    alert: '<path d="M12 3 2.6 20h18.8L12 3Z"/><path d="M12 9.5v4.3"/><path d="M12 16.6v.1"/>',
    users: '<circle cx="9" cy="8" r="3.2"/><path d="M3 20c0-3.4 2.7-6 6-6s6 2.6 6 6"/><path d="M16 5.6a3 3 0 0 1 0 5.6"/><path d="M17.5 14.4c2 .8 3.5 2.7 3.5 5.6"/>',
    star: '<path d="m12 3 2.6 5.6 6 .7-4.4 4.2 1.2 5.9-5.4-3-5.4 3 1.2-5.9L3.4 9.3l6-.7L12 3Z"/>',
    layers: '<path d="m12 3 9 5-9 5-9-5 9-5Z"/><path d="m3 13 9 5 9-5"/>',
    cogs: '<circle cx="12" cy="12" r="3.1"/><path d="M12 2v2.6M12 19.4V22M4.2 4.2l1.9 1.9M17.9 17.9l1.9 1.9M2 12h2.6M19.4 12H22M4.2 19.8l1.9-1.9M17.9 6.1l1.9-1.9"/>',
    server: '<rect x="3" y="4" width="18" height="7" rx="2"/><rect x="3" y="13" width="18" height="7" rx="2"/><path d="M7 7.5h.01M7 16.5h.01"/>',
    trend: '<path d="M3 17 9 11l4 4 8-8"/><path d="M14 4h7v7"/>',
    gamepad: '<rect x="2" y="7" width="20" height="11" rx="5"/><path d="M7 11v3M5.5 12.5h3"/><circle cx="16" cy="11" r="1"/><circle cx="18" cy="13" r="1"/>',
    smartphone: '<rect x="6" y="2" width="12" height="20" rx="3"/><path d="M11 18.5h2"/>',
    atom: '<circle cx="12" cy="12" r="1.8"/><ellipse cx="12" cy="12" rx="10" ry="4.2"/><ellipse cx="12" cy="12" rx="10" ry="4.2" transform="rotate(60 12 12)"/><ellipse cx="12" cy="12" rx="10" ry="4.2" transform="rotate(120 12 12)"/>',
    globe: '<circle cx="12" cy="12" r="9"/><path d="M3 12h18"/><path d="M12 3c3 3.3 3 14.7 0 18M12 3c-3 3.3-3 14.7 0 18"/>',
    network: '<circle cx="6" cy="6" r="2.4"/><circle cx="18" cy="6" r="2.4"/><circle cx="12" cy="18" r="2.4"/><path d="M8.3 7.2 10.7 15.8M15.7 7.2 13.3 15.8M8.5 6h7"/>',
    key: '<circle cx="8" cy="15" r="4"/><path d="M11 12 20 3M17 6l2 2M14.5 8.5l2 2"/>',
    database: '<ellipse cx="12" cy="6" rx="8" ry="3"/><path d="M4 6v12c0 1.7 3.6 3 8 3s8-1.3 8-3V6"/><path d="M4 12c0 1.7 3.6 3 8 3s8-1.3 8-3"/>',
    box: '<path d="M12 2 3 7v10l9 5 9-5V7l-9-5Z"/><path d="M3 7l9 5 9-5M12 12v10"/>',
    chart: '<path d="M3 20h18"/><rect x="5" y="10" width="3" height="7"/><rect x="10.5" y="6" width="3" height="11"/><rect x="16" y="12" width="3" height="5"/>',
    git: '<circle cx="6" cy="5" r="2.4"/><circle cx="6" cy="19" r="2.4"/><circle cx="18" cy="9" r="2.4"/><path d="M6 7.4v9.2M18 11.4c0 3.2-3.2 4.2-6.4 4.7"/>',
    github: '<path d="M9.5 19.5c-4 1.3-4-2.2-5.5-2.7m11 5.2v-3.3c0-1 .1-1.4-.5-2 2.8-.3 4.5-1.5 4.5-5.1a3.9 3.9 0 0 0-1-2.7 3.6 3.6 0 0 0-.1-2.7s-1.1-.3-3.5 1.3a9.4 9.4 0 0 0-5 0C7.5 4.9 6.4 5.2 6.4 5.2a3.6 3.6 0 0 0-.1 2.7A3.9 3.9 0 0 0 5.3 10.6c0 3.6 1.7 4.8 4.5 5.1-.5.6-.6 1.2-.5 2v3.3"/>',
    pages: '<path d="M4 5a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V5Z"/><path d="M8 7.5h8M8 11.5h8M8 15.5h4"/>',
    login: '<path d="M14 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"/><path d="M10 12H3M6 8l-3 4 3 4"/>',
    target: '<circle cx="12" cy="12" r="9"/><circle cx="12" cy="12" r="4.5"/><circle cx="12" cy="12" r="1"/>',
    search: '<circle cx="11" cy="11" r="7"/><path d="m20 20-3.6-3.6"/>',
    check: '<path d="m4.5 12.5 5 5L20 6.5"/>',
    message: '<path d="M4 5h16a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1H9l-5 4V6a1 1 0 0 1 1-1Z"/><path d="M8 9h8M8 12.5h5"/>',
    trophy: '<path d="M7 4h10v4a5 5 0 0 1-10 0V4Z"/><path d="M7 5H4v2a4 4 0 0 0 4 4M17 5h3v2a4 4 0 0 1-4 4"/><path d="M12 13v4M8.5 21h7M10 17.5h4"/>',
    shield: '<path d="M12 3 5 6v5c0 4.5 3 8.3 7 10 4-1.7 7-5.5 7-10V6l-7-3Z"/><path d="m9 12 2 2 4-4"/>',
    user: '<circle cx="12" cy="8" r="3.5"/><path d="M5 20c0-3.5 3.1-6 7-6s7 2.5 7 6"/>',
    clipboard: '<rect x="6" y="4" width="12" height="17" rx="2"/><path d="M9 4a3 3 0 0 1 6 0"/><path d="M9 11h6M9 15h4"/>',
    edit: '<path d="M4 20h4L20 8l-4-4L4 16v4Z"/><path d="M14 6l4 4"/>',
    bug: '<rect x="7" y="7" width="10" height="12" rx="5"/><path d="M12 4v3M7 10H3M21 10h-4M7 15H4M20 15h-3M8 8 6 5M16 8l2-3M8 17l-2 3M16 17l2 3"/>',
    lock: '<rect x="5" y="10" width="14" height="10" rx="2"/><path d="M8 10V7a4 4 0 0 1 8 0v3"/>',
    eye: '<path d="M2 12s3.5-6 10-6 10 6 10 6-3.5 6-10 6S2 12 2 12Z"/><circle cx="12" cy="12" r="3"/>',
    bulb: '<path d="M9.5 18h5M10.5 21h3"/><path d="M12 3a6 6 0 0 0-3.6 10.8c.6.5.6 1.2.6 1.9V16h6v-.3c0-.7 0-1.4.6-1.9A6 6 0 0 0 12 3Z"/>',
    badge: '<circle cx="12" cy="14" r="6"/><path d="m9 3 3 5 3-5M12 11.5l.9 1.8 2 .3-1.4 1.4.3 2-1.8-1-1.8 1 .3-2-1.4-1.4 2-.3.9-1.8Z"/>',
    refresh: '<path d="M20.5 11A8.5 8.5 0 1 0 19 16"/><path d="M20.5 4.5V11h-6"/>',
    code: '<path d="m9 8-4 4 4 4M15 8l4 4-4 4"/>',
    wifi: '<path d="M3 9a14 14 0 0 1 18 0M6 12.6a9 9 0 0 1 12 0M9.5 16.1a4 4 0 0 1 5 0"/><circle cx="12" cy="19.5" r="1"/>',
    puzzle: '<path d="M10 4a2 2 0 1 1 4 0v1h3a1 1 0 0 1 1 1v3h1a2 2 0 1 1 0 4h-1v3a1 1 0 0 1-1 1h-3v-1a2 2 0 1 0-4 0v1H7a1 1 0 0 1-1-1v-3H5a2 2 0 1 1 0-4h1V6a1 1 0 0 1 1-1h3V4Z"/>',
    dot: '<circle cx="12" cy="12" r="4"/>'
  };

  function icon(name, cls) {
    var body = ICONS[name] || ICONS.dot;
    return '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" ' +
      'stroke-linecap="round" stroke-linejoin="round"' + (cls ? ' class="' + cls + '"' : '') +
      ' aria-hidden="true">' + body + '</svg>';
  }
  function esc(s) {
    return String(s).replace(/[&<>"']/g, function (c) {
      return { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c];
    });
  }

  /* ----------------------------- DATOS ----------------------------- */
  var HERO_FACTS = [
    { icon: "gamepad", t: "Videojuego educativo 2D", s: "Estilo pixel art, basado en misiones." },
    { icon: "user", t: "Protagonista: Alex", s: "Técnico junior recién egresado en NovaTech Solutions." },
    { icon: "shield", t: "Antagonista: el Saboteador", s: "Provoca errores intencionales dentro de la empresa." },
    { icon: "layers", t: "Tres aplicaciones", s: "Unity, Flutter y React PWA sobre una capa API." }
  ];
  var HERO_STRIP = ["Unity", "Flutter", "React / PWA", "API", "Keycloak", "MongoDB", "Docker", "Grafana", "Git", "GitHub", "GitHub Pages"];

  var BLOCKS = [
    {
      id: "problema", n: 1, color: "#fb7185", icon: "alert",
      title: "Problema / Necesidad",
      tagline: "Brecha entre lo aprendido en el aula y los escenarios reales de una empresa.",
      summary: "Los estudiantes de TI adquieren teoría, pero tienen pocas oportunidades de enfrentarse a escenarios similares a los de un entorno laboral real.",
      tags: ["Poca experiencia práctica", "Teoría vs. práctica", "Falta de simuladores", "Practicar incidentes reales", "Falta de gamificación", "Diagnóstico de problemas"],
      note: "Existe una diferencia entre lo aprendido académicamente y la experiencia requerida en un entorno profesional.",
      sections: [
        { title: "Necesidad central", list: [
          { t: "Poca experiencia práctica" },
          { t: "Diferencia entre teoría y práctica" },
          { t: "Falta de simuladores especializados en TI" },
          { t: "Dificultad para practicar incidentes reales" },
          { t: "Falta de gamificación" },
          { t: "Dificultad para diagnosticar problemas" }
        ] },
        { title: "Causas", list: [
          { t: "Poca experiencia práctica" },
          { t: "Dificultad para relacionar teoría y práctica" },
          { t: "Métodos educativos tradicionales" },
          { t: "Falta de simuladores especializados en TI" },
          { t: "Poca utilización de videojuegos educativos" },
          { t: "Falta de experiencias gamificadas" },
          { t: "Dificultad para practicar problemas de infraestructura real" },
          { t: "Riesgos de practicar sobre servidores o redes reales" },
          { t: "Poca experiencia diagnosticando problemas" }
        ] },
        { title: "Consecuencias", list: [
          { t: "Dificultad para diagnosticar incidentes" },
          { t: "Inseguridad en las primeras experiencias laborales" },
          { t: "Problemas para tomar decisiones técnicas" },
          { t: "Dificultad para identificar la causa real de un error" },
          { t: "Poca experiencia solucionando incidentes" },
          { t: "Dificultad para priorizar problemas" },
          { t: "Falta de experiencia tratando con usuarios" },
          { t: "Dificultad para relacionar varias áreas de TI en un mismo problema" }
        ] }
      ]
    },
    {
      id: "usuarios", n: 2, color: "#38bdf8", icon: "users",
      title: "Usuarios o beneficiarios",
      tagline: "Cuatro roles: estudiante, docente, administrador y editor de contenido.",
      summary: "El estudiante es el usuario principal; docentes, administradores y editores usan la plataforma para enseñar, gestionar y crear contenido.",
      tags: ["Jugador / Estudiante", "Docente / Evaluador", "Administrador", "Editor de contenido"],
      note: "La autenticación y el control de acceso de todos los roles se gestionan con Keycloak.",
      sections: [
        { title: "Roles de la plataforma", list: [
          { t: "Jugador / Estudiante", s: "Usuario principal. Juega misiones, explora escenarios, habla con personajes, investiga incidentes, toma decisiones, resuelve problemas, gana experiencia, sube de nivel, desbloquea logros e insignias y consulta su progreso y estadísticas. Usa el videojuego Unity y la aplicación móvil Flutter." },
          { t: "Docente / Evaluador", s: "Herramienta complementaria de enseñanza. Consulta progreso, desempeño, misiones completadas, resultados, estadísticas, áreas de dificultad y logros obtenidos. Usa la React PWA." },
          { t: "Administrador", s: "Administra usuarios, gestiona información, supervisa el funcionamiento, consulta métricas, revisa actividad y gestiona configuraciones. Usa la React PWA." },
          { t: "Editor de contenido", s: "Crea y modifica misiones, gestiona preguntas y retos, modifica contenido, actualiza información educativa y publica contenido nuevo. Usa la React PWA." }
        ] },
        { title: "Acceso", list: [
          { t: "Autenticación con Keycloak", s: "Todos los roles inician sesión, obtienen su rol y acceden a la aplicación que les corresponde." }
        ] }
      ]
    },
    {
      id: "propuesta", n: 3, color: "#fbbf24", icon: "star",
      title: "Propuesta de valor",
      tagline: "Aprender TI practicando en un entorno seguro, interactivo y gamificado.",
      summary: "Un videojuego educativo que simula problemas reales de soporte, redes, programación, servidores y ciberseguridad.",
      quote: "Aprender y practicar habilidades de Tecnologías de la Información mediante un videojuego educativo que simula problemas reales de soporte, redes, programación, servidores y ciberseguridad dentro de un entorno seguro, interactivo y gamificado.",
      tags: ["Entorno seguro", "Interactivo", "Gamificado", "Problemas reales", "Varias áreas de TI", "Videojuego educativo"],
      sections: [
        { title: "Propuesta de valor", quote: true },
        { title: "Atributos clave", list: [
          { t: "Entorno seguro", s: "Se practica sin afectar infraestructura real." },
          { t: "Interactivo", s: "El estudiante decide y actúa durante las misiones." },
          { t: "Gamificado", s: "XP, niveles, logros, insignias y estadísticas." },
          { t: "Problemas reales", s: "Situaciones similares a las de una empresa." },
          { t: "Varias áreas de TI", s: "Soporte, redes, programación, servidores y ciberseguridad." },
          { t: "Videojuego educativo", s: "Aprender y entretenerse al mismo tiempo." }
        ] }
      ]
    },
    {
      id: "solucion", n: 4, color: "#a78bfa", icon: "layers",
      title: "Solución propuesta",
      tagline: "Un videojuego, una app móvil y una PWA web conectadas por una API.",
      summary: "Videojuego Unity, aplicación móvil Flutter y React PWA, con sistemas de misiones, logros, progreso, niveles, XP, administración y estadísticas.",
      tags: ["Videojuego Unity", "App Flutter", "React PWA", "Sistema de misiones", "Logros", "Progreso y niveles", "XP", "Administración", "Estadísticas"],
      note: "Estos componentes se conectan con la arquitectura tecnológica del proyecto (ver sección Arquitectura).",
      sections: [
        { title: "Aplicaciones", list: [
          { t: "Videojuego Unity (2D pixel art)", s: "Mapas, escenarios, personajes, movimiento, animaciones, NPC, diálogos, misiones, puzzles, interacción, incidentes, narrativa, exploración, investigación, sistema de XP, niveles y logros." },
          { t: "Aplicación Flutter (móvil)", s: "Iniciar sesión, perfil, progreso, experiencia, niveles, logros, insignias, estadísticas y notificaciones." },
          { t: "React PWA (web)", s: "Administración, gestión de usuarios, gestión de contenido, estadísticas, seguimiento, paneles de información, consulta de actividad y gestión de misiones. Se adapta a distintos tamaños de pantalla." }
        ] },
        { title: "Sistemas del proyecto", list: [
          { t: "Sistema de misiones" },
          { t: "Sistema de logros" },
          { t: "Progreso" },
          { t: "Niveles" },
          { t: "XP" },
          { t: "Administración" },
          { t: "Estadísticas" }
        ] }
      ]
    },
    {
      id: "actividades", n: 5, color: "#34d399", icon: "cogs",
      title: "Actividades y procesos clave",
      tagline: "Autenticarse, investigar, resolver, evaluar y progresar.",
      summary: "Autenticación, selección de misión, investigación, resolución, evaluación, retroalimentación, XP, progreso, logros, administración y creación de contenido.",
      tags: ["Autenticación", "Selección de misión", "Investigación", "Resolución", "Evaluación", "Retroalimentación", "XP y progreso", "Logros", "Administración", "Crear contenido"],
      note: "Los recorridos completos de cada proceso se detallan en la sección Procesos.",
      sections: [
        { title: "Procesos clave", list: [
          { t: "Autenticación" },
          { t: "Selección de misión" },
          { t: "Investigación" },
          { t: "Resolución" },
          { t: "Evaluación" },
          { t: "Retroalimentación" },
          { t: "Asignación de XP" },
          { t: "Progreso" },
          { t: "Logros" },
          { t: "Administración" },
          { t: "Creación de contenido" }
        ] },
        { title: "Flujos de trabajo", list: [
          { t: "Flujo del jugador", s: "De iniciar sesión a desbloquear un logro y actualizar estadísticas." },
          { t: "Flujo de administración", s: "Gestión de usuarios, métricas, actividad e información." },
          { t: "Flujo de contenido", s: "Crear, modificar, guardar y publicar misiones." },
          { t: "Flujo de autenticación", s: "Usuario → Keycloak → aplicación correspondiente." }
        ] }
      ]
    },
    {
      id: "recursos", n: 6, color: "#60a5fa", icon: "server",
      title: "Recursos y tecnologías clave",
      tagline: "Las herramientas que sostienen el proyecto, de la app a los datos.",
      summary: "Unity, Flutter, React, PWA, API, Keycloak, MongoDB, Docker, Grafana, Git, GitHub y GitHub Pages.",
      tags: ["Unity", "Flutter", "React", "PWA", "API", "Keycloak", "MongoDB", "Docker", "Grafana", "Git", "GitHub", "GitHub Pages"],
      note: "Cada tecnología cumple un rol específico dentro de la solución.",
      sections: [
        { title: "Tecnologías clave", list: [
          { t: "Unity", s: "Motor principal del videojuego 2D pixel art: mapas, personajes, misiones, XP y logros." },
          { t: "Flutter", s: "Aplicación móvil complementaria: perfil, progreso, logros y notificaciones." },
          { t: "React", s: "Desarrollo de la aplicación web." },
          { t: "PWA", s: "Aplicación web progresiva, adaptable a distintos tamaños de pantalla, enfocada en administración y consulta." },
          { t: "API", s: "Capa de comunicación entre las aplicaciones, los servicios y los datos." },
          { t: "Keycloak", s: "Autenticación y autorización: inicio de sesión, usuarios, roles, sesiones, permisos y control de acceso." },
          { t: "MongoDB", s: "Base de datos principal: usuarios, perfiles, progreso, misiones, resultados, logros, estadísticas y actividad." },
          { t: "Docker", s: "Contenerización de los servicios e infraestructura del proyecto." },
          { t: "Grafana", s: "Monitoreo, métricas, observabilidad y visualización de información técnica." },
          { t: "Git", s: "Control de versiones." },
          { t: "GitHub", s: "Repositorio, ramas, colaboración, documentación, versionamiento y publicación." },
          { t: "GitHub Pages", s: "Publicación de la documentación web y de este Business Model Canvas." }
        ] }
      ]
    },
    {
      id: "resultados", n: 7, color: "#2dd4bf", icon: "trend",
      title: "Resultados, beneficios e impacto",
      tagline: "Cerrar la brecha entre el aula y el primer empleo en TI.",
      summary: "Aprendizaje práctico, experiencia simulada, habilidades técnicas, diagnóstico, toma de decisiones y preparación para el entorno laboral.",
      tags: ["Aprendizaje práctico", "Experiencia simulada", "Habilidades técnicas", "Pensamiento lógico", "Diagnóstico", "Toma de decisiones", "Motivación", "Seguimiento", "Preparación laboral"],
      impact: "El proyecto busca reducir la brecha entre los conocimientos académicos de un estudiante de TI y las situaciones que encontrará durante sus primeras experiencias profesionales, transformando conocimientos teóricos en experiencias prácticas mediante un entorno educativo, seguro y gamificado.",
      sections: [
        { title: "Resultados esperados", list: [
          { t: "Aprendizaje práctico" },
          { t: "Mayor interacción del estudiante" },
          { t: "Aplicación de conocimientos" },
          { t: "Desarrollo de habilidades técnicas" },
          { t: "Desarrollo del análisis" },
          { t: "Seguimiento del progreso" },
          { t: "Aprendizaje mediante errores" },
          { t: "Experiencia simulada" }
        ] },
        { title: "Beneficios", list: [
          { t: "Practicar sin afectar infraestructura real" },
          { t: "Aprender dentro de escenarios controlados" },
          { t: "Mejorar habilidades de diagnóstico" },
          { t: "Mejorar la toma de decisiones" },
          { t: "Practicar distintas áreas de TI" },
          { t: "Recibir retroalimentación" },
          { t: "Mantener la motivación mediante gamificación" },
          { t: "Acceso mediante diferentes plataformas" },
          { t: "Seguimiento del progreso" }
        ] },
        { title: "Impacto", list: [
          { t: "Pensamiento lógico" },
          { t: "Diagnóstico" },
          { t: "Toma de decisiones" },
          { t: "Motivación" },
          { t: "Preparación para el entorno laboral" }
        ] }
      ]
    }
  ];

  var MACRO = [
    { from: "problema", to: "propuesta", verb: "genera" },
    { from: "propuesta", to: "solucion", verb: "se implementa mediante" },
    { from: "solucion", to: "recursos", verb: "necesita" },
    { from: "recursos", to: "actividades", verb: "permiten realizar" },
    { from: "actividades", to: "usuarios", verb: "son utilizados por" },
    { from: "usuarios", to: "resultados", verb: "generando" }
  ];

  var ARCH_NODES = {
    usuarios: { label: "Jugador / Usuarios", color: "#8ab4ff" },
    keycloak: { label: "Keycloak", color: "#f472b6" },
    unity: { label: "Unity", color: "#a78bfa" },
    flutter: { label: "Flutter", color: "#a78bfa" },
    react: { label: "React PWA", color: "#a78bfa" },
    api: { label: "API", color: "#34d399" },
    mongodb: { label: "MongoDB", color: "#22c55e" }
  };

  var ARCH_EDGES = [
    { from: "usuarios", to: "keycloak", label: "inicia sesión" },
    { from: "keycloak", to: "unity", label: "acceso autorizado" },
    { from: "keycloak", to: "flutter", label: "" },
    { from: "keycloak", to: "react", label: "" },
    { from: "unity", to: "api", label: "consume" },
    { from: "flutter", to: "api", label: "" },
    { from: "react", to: "api", label: "" },
    { from: "api", to: "mongodb", label: "consulta y guarda" }
  ];

  var ROLE_ROUTES = [
    { role: "Jugador / Estudiante", target: "Unity (videojuego) y Flutter (app móvil)", color: "#38bdf8" },
    { role: "Docente / Evaluador", target: "React PWA", color: "#60a5fa" },
    { role: "Administrador", target: "React PWA", color: "#a78bfa" },
    { role: "Editor de contenido", target: "React PWA", color: "#34d399" },
    { role: "Todos los roles", target: "Keycloak (autenticación y autorización)", color: "#f472b6" }
  ];

  var SUPPORT_CARDS = [
    { icon: "box", color: "#2563eb", name: "Docker", role: "Conteneriza los servicios del proyecto como parte de la infraestructura.", arrow: "Docker → infraestructura de servicios" },
    { icon: "chart", color: "#f59e0b", name: "Grafana", role: "Monitoreo, métricas, observabilidad y comportamiento de los servicios.", arrow: "Grafana → monitoreo" },
    { icon: "git", color: "#f97316", name: "Git / GitHub", role: "Control de versiones, repositorio, ramas, colaboración, documentación y versionamiento.", arrow: "Git / GitHub → desarrollo y colaboración" },
    { icon: "pages", color: "#60a5fa", name: "GitHub Pages", role: "Publicación de la documentación web del proyecto, incluido este canvas.", arrow: "GitHub Pages → publicación" }
  ];

  var BLOCK_ARCH = {
    usuarios: ["usuarios", "keycloak", "unity", "flutter", "react"],
    propuesta: ["unity", "flutter", "react"],
    solucion: ["unity", "flutter", "react", "api"],
    actividades: ["keycloak", "api", "mongodb"],
    recursos: ["unity", "flutter", "react", "api", "keycloak", "mongodb"],
    problema: [],
    resultados: []
  };

  var FLOWS = [
    {
      id: "jugador", color: "#38bdf8", icon: "gamepad",
      title: "Flujo del jugador", sub: "De iniciar sesión a desbloquear un logro",
      steps: ["Jugador", "Iniciar sesión", "Acceder al sistema", "Seleccionar misión", "Analizar problema", "Investigar", "Hablar con personajes", "Resolver incidente", "Sistema evalúa solución", "Mostrar retroalimentación", "Asignar XP", "Actualizar nivel", "Comprobar logro", "Desbloquear insignia o logro", "Mostrar notificación", "Actualizar estadísticas"]
    },
    {
      id: "admin", color: "#a78bfa", icon: "clipboard",
      title: "Flujo de administración", sub: "Gestión general de la plataforma",
      steps: ["Administrador", "Gestionar usuarios", "Consultar métricas", "Revisar actividad", "Administrar información"]
    },
    {
      id: "contenido", color: "#34d399", icon: "edit",
      title: "Flujo de contenido", sub: "Del editor a los jugadores",
      steps: ["Editor", "Crear misión", "Modificar misión", "Agregar contenido educativo", "Guardar", "Publicar", "Misión disponible para jugadores"]
    },
    {
      id: "auth", color: "#f472b6", icon: "key",
      title: "Flujo de autenticación", sub: "Usuario → Keycloak → aplicación",
      steps: ["Usuario", "Keycloak", "Validar credenciales", "Identificar rol", "Permitir acceso", "Aplicación correspondiente"]
    }
  ];

  var GAME_ELEMENTS = [
    { icon: "star", name: "XP", desc: "Experiencia ganada al resolver misiones." },
    { icon: "trend", name: "Niveles", desc: "Sube de nivel a medida que avanza." },
    { icon: "target", name: "Misiones", desc: "Objetivos con incidentes por resolver." },
    { icon: "puzzle", name: "Retos", desc: "Problemas que exigen análisis y decisión." },
    { icon: "badge", name: "Insignias", desc: "Recompensas por logros alcanzados." },
    { icon: "trophy", name: "Logros", desc: "Se desbloquean y se notifican al jugador." },
    { icon: "chart", name: "Progreso", desc: "Seguimiento del avance del estudiante." },
    { icon: "clipboard", name: "Estadísticas", desc: "Resultados y áreas de dificultad." }
  ];

  var ACHIEVEMENTS = [
    { icon: "check", name: "Primer incidente resuelto", desc: "Completaste tu primera misión de soporte en NovaTech Solutions." },
    { icon: "search", name: "Modo investigador", desc: "Analizaste pistas y diagnosticaste la causa real de un problema." },
    { icon: "wifi", name: "Red restaurada", desc: "Resolviste un incidente de redes o infraestructura." },
    { icon: "shield", name: "Guardián de la información", desc: "Identificaste un riesgo de seguridad o un acceso no autorizado." }
  ];

  var CHARACTERS = [
    { name: "Alex", role: "Protagonista", color: "#38bdf8", cls: "is-hero", text: "Técnico junior recién egresado que comienza a trabajar en NovaTech Solutions. El jugador lo controla y, durante la historia, desarrolla conocimientos y experiencia." },
    { name: "Martín", role: "Mentor", color: "#60a5fa", text: "Especialista experimentado del área de TI. Ayuda a introducir problemas, explicar procesos y orientar al jugador." },
    { name: "Richar", role: "Redes e infraestructura", color: "#a78bfa", tags: ["Redes", "Servidores", "Infraestructura", "Seguridad", "Información"], text: "Especialista en redes e infraestructura, con una personalidad gruñona y sarcástica." },
    { name: "Marco", role: "Desarrollo de software", color: "#34d399", tags: ["Código", "Lógica", "Errores", "Desarrollo"], text: "Ayuda al jugador en problemas relacionados con código, lógica, errores de programación y desarrollo de software." },
    { name: "Sofía", role: "Personaje secundario", color: "#fbbf24", text: "Participa en misiones, diálogos e incidentes dentro de NovaTech Solutions." },
    { name: "Elena", role: "Personaje secundario", color: "#f472b6", text: "Participa en distintas situaciones dentro de la empresa." },
    { name: "Saboteador", role: "Antagonista principal", color: "#fb7185", cls: "is-villain", tags: ["Desconecta cables", "Modifica configuraciones", "Altera archivos", "Bloquea equipos", "Deja pistas falsas", "Busca acceder a información"], text: "Empleado que conoce la empresa, los sistemas, las redes, las configuraciones y los equipos. Provoca problemas intencionalmente, sin intentar ser descubierto." }
  ];

  /* ----------------------------- ESTADO ----------------------------- */
  var state = { selectedBlock: null, selectedArch: null, search: "" };
  var byId = {};
  BLOCKS.forEach(function (b) { byId[b.id] = b; });

  /* ----------------------------- RENDER ----------------------------- */
  function hydrateIcons(root) {
    (root || document).querySelectorAll("[data-ico]").forEach(function (el) {
      el.innerHTML = icon(el.getAttribute("data-ico"));
    });
  }

  function renderHero() {
    var facts = document.getElementById("heroFacts");
    facts.innerHTML = HERO_FACTS.map(function (f) {
      return '<li><span class="fact-ico">' + icon(f.icon) + "</span><span><b>" + esc(f.t) +
        "</b><small>" + esc(f.s) + "</small></span></li>";
    }).join("");
    var strip = document.getElementById("heroStrip");
    strip.innerHTML = HERO_STRIP.map(function (t) {
      return '<span class="chip-tech"><i></i>' + esc(t) + "</span>";
    }).join("");
  }

  function renderRibbon() {
    var ol = document.getElementById("modelFlow");
    var chain = ["problema", "propuesta", "solucion", "recursos", "actividades", "usuarios", "resultados"];
    var html = "";
    chain.forEach(function (id, i) {
      var b = byId[id];
      html += '<li><button class="ribbon-node" type="button" data-block="' + id + '" style="--c:' + b.color +
        '" title="' + esc(b.tagline) + '"><span class="dot"></span><span class="num">' + b.n +
        "</span>" + esc(b.title) + "</button></li>";
      if (i < chain.length - 1) {
        var rel = MACRO.filter(function (r) { return r.from === id; })[0];
        html += '<li class="ribbon-link" data-rel="' + i + '"><span class="verb">' + esc(rel ? rel.verb : "→") +
          '</span><span class="line"></span></li>';
      }
    });
    ol.innerHTML = html;
  }

  function renderCanvas() {
    var grid = document.getElementById("canvasGrid");
    grid.innerHTML = BLOCKS.map(function (b) {
      var tags = b.tags.slice(0, 5).map(function (t) { return '<span class="tag">' + esc(t) + "</span>"; }).join("");
      if (b.tags.length > 5) tags += '<span class="tag more">+' + (b.tags.length - 5) + " más</span>";
      return '<article class="block-card" data-block="' + b.id + '" tabindex="0" role="button" aria-label="' +
        esc(b.title) + '" style="--c:' + b.color + '">' +
        '<div class="card-top"><span class="card-ico">' + icon(b.icon) + "</span>" +
        '<span class="card-num">' + String(b.n).padStart(2, "0") + "</span></div>" +
        '<h3 class="card-title">' + esc(b.title) + "</h3>" +
        '<p class="card-summary">' + esc(b.summary) + "</p>" +
        '<div class="card-tags">' + tags + "</div>" +
        '<div class="card-open">Ver detalle <span class="arw">→</span></div>' +
        "</article>";
    }).join("");
  }

  function renderArchSupport() {
    var el = document.getElementById("archSupport");
    var routes = ROLE_ROUTES.map(function (r) {
      return '<div class="support-card" style="--c:' + r.color + '">' +
        '<div class="support-head"><span class="support-ico" style="color:' + r.color + '">' + icon("user") + "</span>" +
        "<h3>" + esc(r.role) + '</h3></div><p>Usa</p>' +
        '<span class="support-arrow">→ ' + esc(r.target) + "</span></div>";
    }).join("");
    var tech = SUPPORT_CARDS.map(function (c) {
      return '<div class="support-card" style="--c:' + c.color + '">' +
        '<div class="support-head"><span class="support-ico" style="color:' + c.color + '">' + icon(c.icon) + "</span>" +
        "<h3>" + esc(c.name) + '</h3></div><p>' + esc(c.role) + "</p>" +
        '<span class="support-arrow">' + esc(c.arrow) + "</span></div>";
    }).join("");
    el.innerHTML = routes + tech;
  }

  function renderFlows() {
    var el = document.getElementById("flows");
    el.innerHTML = FLOWS.map(function (f) {
      var steps = f.steps.map(function (s, i) {
        var cls = i === 0 ? " is-start" : (i === f.steps.length - 1 ? " is-end" : "");
        var step = '<span class="fstep' + cls + '"><span class="n">' + String(i + 1).padStart(2, "0") + "</span>" + esc(s) + "</span>";
        return step + (i < f.steps.length - 1 ? '<span class="farrow">→</span>' : "");
      }).join("");
      return '<article class="flow-card" data-flow="' + f.id + '" style="--c:' + f.color + '">' +
        '<div class="flow-card-head" data-flow-head="' + f.id + '"><span class="flow-ico">' + icon(f.icon) + "</span>" +
        "<div><h3>" + esc(f.title) + '</h3><p class="flow-sub">' + esc(f.sub) + "</p></div></div>" +
        '<div class="flow-steps">' + steps + "</div></article>";
    }).join("");
  }

  function renderGame() {
    document.getElementById("gameElements").innerHTML = GAME_ELEMENTS.map(function (g) {
      return '<div class="game-el"><span class="ge-ico">' + icon(g.icon) + "</span><b>" + esc(g.name) +
        "</b><small>" + esc(g.desc) + "</small></div>";
    }).join("");
  }

  function renderRoster() {
    document.getElementById("roster").innerHTML = CHARACTERS.map(function (c) {
      var tags = (c.tags || []).map(function (t) { return '<span class="tag">' + esc(t) + "</span>"; }).join("");
      return '<article class="char ' + (c.cls || "") + '" style="--c:' + c.color + '">' +
        '<div class="char-ava">' + esc(c.name.charAt(0)) + "</div>" +
        "<h3>" + esc(c.name) + '</h3><p class="char-role">' + esc(c.role) + "</p>" +
        "<p>" + esc(c.text) + "</p>" +
        (tags ? '<div class="char-tags">' + tags + "</div>" : "") +
        "</article>";
    }).join("");
  }

  /* ----------------------------- SVG MOTOR ----------------------------- */
  var SVG_NS = "http://www.w3.org/2000/svg";
  function svgDefs() {
    return '<defs>' +
      '<marker id="arrowC" viewBox="0 0 10 10" refX="8.5" refY="5" markerWidth="6.5" markerHeight="6.5" orient="auto-start-reverse"><path d="M0 0 10 5 0 10 z" fill="#5c7191"/></marker>' +
      '<marker id="arrowHot" viewBox="0 0 10 10" refX="8.5" refY="5" markerWidth="7" markerHeight="7" orient="auto-start-reverse"><path d="M0 0 10 5 0 10 z" fill="#f4f9ff"/></marker>' +
      "</defs>";
  }
  function relRect(el, cr) {
    var r = el.getBoundingClientRect();
    return { x: r.left - cr.left, y: r.top - cr.top, w: r.width, h: r.height };
  }
  function edgeSide(rect, side) {
    if (side === "left") return { x: rect.x, y: rect.y + rect.h / 2 };
    if (side === "right") return { x: rect.x + rect.w, y: rect.y + rect.h / 2 };
    if (side === "top") return { x: rect.x + rect.w / 2, y: rect.y };
    return { x: rect.x + rect.w / 2, y: rect.y + rect.h };
  }
  function connection(aEl, bEl, cr) {
    var ra = relRect(aEl, cr), rb = relRect(bEl, cr);
    var ac = { x: ra.x + ra.w / 2, y: ra.y + ra.h / 2 };
    var bc = { x: rb.x + rb.w / 2, y: rb.y + rb.h / 2 };
    var dx = bc.x - ac.x, dy = bc.y - ac.y;
    var sa, ea;
    if (Math.abs(dx) >= Math.abs(dy) * 1.15) { sa = dx > 0 ? "right" : "left"; ea = dx > 0 ? "left" : "right"; }
    else { sa = dy > 0 ? "bottom" : "top"; ea = dy > 0 ? "top" : "bottom"; }
    var s = edgeSide(ra, sa), e = edgeSide(rb, ea);
    var k = Math.max(26, Math.min(120, (Math.abs(e.x - s.x) + Math.abs(e.y - s.y)) / 2.1));
    var c1 = { x: s.x, y: s.y }, c2 = { x: e.x, y: e.y };
    if (sa === "right") c1.x += k; else if (sa === "left") c1.x -= k;
    else if (sa === "bottom") c1.y += k; else c1.y -= k;
    if (ea === "left") c2.x -= k; else if (ea === "right") c2.x += k;
    else if (ea === "top") c2.y -= k; else c2.y += k;
    var mid = { x: (s.x + 3 * c1.x + 3 * c2.x + e.x) / 8, y: (s.y + 3 * c1.y + 3 * c2.y + e.y) / 8 };
    return { s: s, e: e, c1: c1, c2: c2, mid: mid };
  }
  function pathD(c) {
    return "M" + c.s.x + " " + c.s.y + " C" + c.c1.x + " " + c.c1.y + " " + c.c2.x + " " + c.c2.y + " " + c.e.x + " " + c.e.y;
  }
  function labelGroup(mid, text, relIndex) {
    if (!text) return "";
    var w = text.length * 6.1 + 14;
    var x = mid.x - w / 2, y = mid.y - 11;
    return '<g class="conn-label-group" data-rel="' + relIndex + '">' +
      '<rect class="conn-label-bg" x="' + x + '" y="' + y + '" width="' + w + '" height="21" rx="7"></rect>' +
      '<text class="conn-label" x="' + mid.x + '" y="' + (mid.y + 3) + '" text-anchor="middle">' + esc(text) + "</text></g>";
  }

  function drawCanvasLinks() {
    var shell = document.getElementById("canvasShell");
    var svg = document.getElementById("canvasSvg");
    if (!shell || !svg) return;
    if (window.getComputedStyle(svg).display === "none") { svg.innerHTML = ""; return; }
    var cr = shell.getBoundingClientRect();
    svg.setAttribute("viewBox", "0 0 " + cr.width + " " + cr.height);
    svg.setAttribute("width", cr.width);
    svg.setAttribute("height", cr.height);
    var html = svgDefs();
    MACRO.forEach(function (rel, i) {
      var a = document.querySelector('.block-card[data-block="' + rel.from + '"]');
      var b = document.querySelector('.block-card[data-block="' + rel.to + '"]');
      if (!a || !b) return;
      var c = connection(a, b, cr);
      html += '<path class="conn" data-rel="' + i + '" d="' + pathD(c) + '" marker-end="url(#arrowC)"></path>';
      html += labelGroup(c.mid, rel.verb, i);
    });
    svg.innerHTML = html;
  }

  function drawArchLinks() {
    var shell = document.getElementById("archShell");
    var svg = document.getElementById("archSvg");
    if (!shell || !svg) return;
    var cr = shell.getBoundingClientRect();
    svg.setAttribute("viewBox", "0 0 " + cr.width + " " + cr.height);
    svg.setAttribute("width", cr.width);
    svg.setAttribute("height", cr.height);
    var html = svgDefs();
    ARCH_EDGES.forEach(function (rel, i) {
      var a = document.querySelector('.arch-node[data-node="' + rel.from + '"]');
      var b = document.querySelector('.arch-node[data-node="' + rel.to + '"]');
      if (!a || !b) return;
      var c = connection(a, b, cr);
      html += '<path class="conn" data-arch="' + i + '" d="' + pathD(c) + '" marker-end="url(#arrowC)"></path>';
      html += '<g class="conn-label-group" data-archlabel="' + i + '">' +
        (rel.label ? (function () {
          var w = rel.label.length * 6.1 + 14, x = c.mid.x - w / 2, y = c.mid.y - 11;
          return '<rect class="conn-label-bg" x="' + x + '" y="' + y + '" width="' + w + '" height="21" rx="7"></rect>' +
            '<text class="conn-label" x="' + c.mid.x + '" y="' + (c.mid.y + 3) + '" text-anchor="middle">' + esc(rel.label) + "</text>";
        })() : "") + "</g>";
    });
    svg.innerHTML = html;
  }

  /* ----------------------------- SELECCIÓN ----------------------------- */
  function neighborsOf(id) {
    var set = {};
    MACRO.forEach(function (r) {
      if (r.from === id) set[r.to] = true;
      if (r.to === id) set[r.from] = true;
    });
    return set;
  }

  function setArchHighlight(nodeIds) {
    var nodes = document.querySelectorAll(".arch-node");
    nodes.forEach(function (n) {
      var id = n.getAttribute("data-node");
      var on = !nodeIds || nodeIds.indexOf(id) !== -1;
      n.classList.remove("is-active");
      n.classList.toggle("is-related", !!nodeIds && on);
      n.classList.toggle("is-dim", !!nodeIds && !on);
      if (nodeIds && state.selectedArch === id) n.classList.add("is-active");
    });
    var hot = {};
    if (state.selectedArch) {
      ARCH_EDGES.forEach(function (e, i) { if (e.from === state.selectedArch || e.to === state.selectedArch) hot[i] = true; });
    } else if (nodeIds) {
      ARCH_EDGES.forEach(function (e, i) {
        if (nodeIds.indexOf(e.from) !== -1 && nodeIds.indexOf(e.to) !== -1) hot[i] = true;
      });
    }
    document.querySelectorAll('.arch-svg .conn').forEach(function (p) {
      var i = p.getAttribute("data-arch");
      var isHot = !!hot[i];
      var dim = nodeIds && !isHot;
      p.classList.toggle("is-hot", isHot);
      p.classList.toggle("is-dim", !!dim);
      if (isHot) { p.setAttribute("marker-end", "url(#arrowHot)"); p.style.setProperty("--hc", "#eaf2ff"); }
      else { p.setAttribute("marker-end", "url(#arrowC)"); }
    });
    document.querySelectorAll('.arch-svg .conn-label-group').forEach(function (g) {
      var i = g.getAttribute("data-archlabel");
      var isHot = !!hot[i];
      g.classList.toggle("is-dim", !!(nodeIds && !isHot));
      g.style.opacity = isHot ? 1 : "";
    });
  }

  function applyBlockHighlight(id) {
    var related = id ? neighborsOf(id) : {};
    document.querySelectorAll(".block-card").forEach(function (card) {
      var cid = card.getAttribute("data-block");
      card.classList.toggle("is-active", cid === id);
      card.classList.toggle("is-related", !!id && !!related[cid]);
      card.classList.toggle("is-dim", !!id && cid !== id && !related[cid]);
    });
    document.querySelectorAll(".ribbon-node").forEach(function (n) {
      var nid = n.getAttribute("data-block");
      n.classList.toggle("is-active", nid === id);
      n.classList.toggle("ribbon-dim", !!id && nid !== id && !related[nid]);
    });
    document.querySelectorAll('.canvas-svg .conn').forEach(function (p) {
      var i = +p.getAttribute("data-rel");
      var rel = MACRO[i];
      var isHot = !!id && (rel.from === id || rel.to === id);
      var dim = !!id && !isHot;
      p.classList.toggle("is-hot", isHot);
      p.classList.toggle("is-dim", dim);
      if (isHot) { p.setAttribute("marker-end", "url(#arrowHot)"); p.style.setProperty("--hc", byId[id].color); }
      else { p.setAttribute("marker-end", "url(#arrowC)"); }
    });
    document.querySelectorAll('.canvas-svg .conn-label-group').forEach(function (g) {
      var i = +g.getAttribute("data-rel");
      var rel = MACRO[i];
      var isHot = !!id && (rel.from === id || rel.to === id);
      g.classList.toggle("is-dim", !!id && !isHot);
      g.style.opacity = isHot ? 1 : "";
    });
    // Cross-highlight de la arquitectura
    if (state.selectedArch === null) {
      var arch = id ? BLOCK_ARCH[id] : null;
      setArchHighlight(arch && arch.length ? arch : null);
    }
  }

  function setHash(h) {
    var cur = location.hash.replace(/^#/, "");
    if (cur === (h || "")) return;
    try { history.replaceState(null, "", location.pathname + location.search + (h ? "#" + h : "")); }
    catch (e) { location.hash = h || ""; }
  }

  function selectBlock(id, openModal) {
    state.selectedBlock = id;
    state.selectedArch = null;
    applyBlockHighlight(id);
    if (openModal !== false && id) openModalFor(id);
    if (!id) closeModal(false);
    setHash(id || "");
  }

  function clearSelection() {
    state.selectedBlock = null;
    state.selectedArch = null;
    applyBlockHighlight(null);
    setArchHighlight(null);
    var cur = location.hash.replace(/^#/, "");
    if (byId[cur] || cur.indexOf("arch-") === 0) setHash("");
  }

  function selectArchNode(id) {
    state.selectedArch = id;
    var set = [id];
    ARCH_EDGES.forEach(function (e) { if (e.from === id) set.push(e.to); if (e.to === id) set.push(e.from); });
    state.selectedBlock = null;
    applyBlockHighlight(null);
    setArchHighlight(set);
    document.querySelectorAll(".arch-node").forEach(function (n) {
      n.classList.toggle("is-active", n.getAttribute("data-node") === id);
    });
    setHash("arch-" + id);
  }

  function hashTarget(h) {
    h = decodeURIComponent(h || "");
    if (byId[h]) return { type: "block", id: h };
    if (h.indexOf("arch-") === 0 && ARCH_NODES[h.slice(5)]) return { type: "arch", id: h.slice(5) };
    return null;
  }

  function applyHash(scroll) {
    var t = hashTarget(location.hash.replace(/^#/, ""));
    if (!t) { clearSelection(); return; }
    if (t.type === "block") {
      if (state.selectedBlock !== t.id) selectBlock(t.id);
    } else {
      if (state.selectedArch !== t.id) selectArchNode(t.id);
      if (scroll) {
        var sec = document.getElementById("arquitectura");
        if (sec && sec.scrollIntoView) sec.scrollIntoView({ behavior: "smooth", block: "start" });
      }
    }
  }

  function copyLink(el) {
    var url = location.href;
    function done() {
      var s = el.querySelector("span");
      if (!s || el.getAttribute("data-busy") === "1") return;
      el.setAttribute("data-busy", "1");
      var old = s.textContent;
      s.textContent = "Enlace copiado";
      el.classList.add("is-copied");
      setTimeout(function () { s.textContent = old; el.classList.remove("is-copied"); el.removeAttribute("data-busy"); }, 1600);
    }
    function fallback() {
      try {
        var ta = document.createElement("textarea");
        ta.value = url; ta.setAttribute("readonly", "");
        ta.style.position = "absolute"; ta.style.left = "-9999px";
        document.body.appendChild(ta); ta.select();
        document.execCommand("copy");
        document.body.removeChild(ta); done();
      } catch (e) { /* sin portapapeles disponible */ }
    }
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(url).then(done).catch(fallback);
    } else { fallback(); }
  }

  function resetAll() {
    clearSelection();
    state.search = "";
    var s = document.getElementById("canvasSearch");
    if (s) s.value = "";
    filterCanvas("");
    closeModal(false);
    document.querySelectorAll(".flow-card").forEach(function (c) { c.classList.remove("is-active", "is-dim"); });
  }

  /* ----------------------------- BUSCADOR ----------------------------- */
  function haystack(b) {
    var parts = [b.title, b.summary, b.tagline, b.note || "", (b.tags || []).join(" ")];
    (b.sections || []).forEach(function (s) {
      parts.push(s.title);
      (s.list || []).forEach(function (it) { parts.push(it.t || "", it.s || ""); });
    });
    return parts.join(" ").toLowerCase();
  }
  function filterCanvas(q) {
    state.search = q.trim().toLowerCase();
    document.querySelectorAll(".block-card").forEach(function (card) {
      var b = byId[card.getAttribute("data-block")];
      var hit = !state.search || haystack(b).indexOf(state.search) !== -1;
      card.classList.toggle("is-dim", !hit);
    });
  }

  /* ----------------------------- MODAL ----------------------------- */
  function buildModal(block) {
    var head = '<div class="mb-head" style="--c:' + block.color + '">' +
      '<div class="mb-eyebrow"><span class="mb-ico">' + icon(block.icon) + "</span>" +
      '<span class="mb-num">Componente ' + String(block.n).padStart(2, "0") + "</span>" +
      '<button class="mb-link" type="button" data-copy-link aria-label="Copiar enlace a este componente">' +
      icon("clipboard") + "<span>Copiar enlace</span></button></div>" +
      "<h2 id=\"modalTitle\">" + esc(block.title) + "</h2>" +
      '<p class="mb-tagline">' + esc(block.tagline) + "</p></div>";

    var body = '<div class="mb-body">';
    if (block.quote) body += '<p class="mb-quote">“' + esc(block.quote) + "”</p><div style=\"height:16px\"></div>";
    (block.sections || []).forEach(function (sec) {
      body += '<section class="mb-section"><h3>' + esc(sec.title) + "</h3>";
      if (sec.quote) {
        body += '<p class="mb-quote">“' + esc(block.quote) + "”</p>";
      } else {
        body += '<ul class="mb-list">' + (sec.list || []).map(function (it) {
          return "<li><span>" + esc(it.t) + (it.s ? "<small>" + esc(it.s) + "</small>" : "") + "</span></li>";
        }).join("") + "</ul>";
      }
      body += "</section>";
    });
    if (block.note) body += '<p class="mb-note">' + esc(block.note) + "</p>";

    // Relaciones
    var rel = MACRO.filter(function (r) { return r.from === block.id || r.to === block.id; });
    if (rel.length) {
      body += '<section class="mb-section" style="margin-top:20px"><h3>Relaciones del modelo</h3><div class="mb-rel">';
      rel.forEach(function (r) {
        var other = r.from === block.id ? r.to : r.from;
        var ob = byId[other];
        body += '<button class="rel-chip" type="button" data-goto="' + other + '" style="--rc:' + ob.color + '">' +
          '<span class="dot"></span>' + esc(ob.title) +
          ' <span class="verb">' + esc(r.verb) + "</span></button>";
      });
      body += "</div></section>";
    }
    var arch = BLOCK_ARCH[block.id];
    if (arch && arch.length) {
      body += '<p class="mb-note" style="margin-top:16px">Componentes relacionados en la arquitectura: ' +
        esc(arch.map(function (a) { return ARCH_NODES[a].label; }).join(" · ")) + "</p>";
    }
    body += "</div>";
    return head + body;
  }

  var lastFocus = null;
  function openModalFor(id) {
    var block = byId[id];
    if (!block) return;
    lastFocus = document.activeElement;
    var modal = document.getElementById("detailModal");
    document.getElementById("modalBody").innerHTML = buildModal(block);
    modal.hidden = false;
    document.body.style.overflow = "hidden";
    var closeBtn = modal.querySelector(".modal-close");
    if (closeBtn) closeBtn.focus();
  }
  function closeModal(restore) {
    var modal = document.getElementById("detailModal");
    if (modal.hidden) return;
    modal.hidden = true;
    document.body.style.overflow = "";
    if (restore !== false && lastFocus && lastFocus.focus) lastFocus.focus();
  }

  /* ----------------------------- TOAST ----------------------------- */
  var achIndex = -1;
  function showAchievement() {
    var wrap = document.getElementById("toastWrap");
    achIndex = (achIndex + 1) % ACHIEVEMENTS.length;
    var a = ACHIEVEMENTS[achIndex];
    var el = document.createElement("div");
    el.className = "achievement";
    el.innerHTML = '<span class="ach-ico">' + icon(a.icon) + "</span>" +
      '<div><p class="ach-kicker">Logro desbloqueado</p><p class="ach-name">' + esc(a.name) + "</p>" +
      '<p class="ach-desc">' + esc(a.desc) + "</p>" +
      '<div class="ach-progress"><i></i></div></div>';
    wrap.appendChild(el);
    setTimeout(function () {
      el.classList.add("out");
      setTimeout(function () { if (el.parentNode) el.parentNode.removeChild(el); }, 420);
    }, 4200);
  }

  /* ----------------------------- TOOLTIP ----------------------------- */
  var tip = document.getElementById("hoverTip");
  function showTip(el, color, title, body) {
    tip.style.setProperty("--c", color);
    tip.innerHTML = '<p class="tt-title"><span class="dot"></span>' + esc(title) + "</p>" +
      '<p class="tt-body">' + esc(body) + "</p>";
    tip.classList.add("show");
  }
  function moveTip(x, y) {
    var w = tip.offsetWidth, h = tip.offsetHeight;
    var nx = x + 16, ny = y + 16;
    if (nx + w > window.innerWidth - 12) nx = x - w - 16;
    if (ny + h > window.innerHeight - 12) ny = y - h - 16;
    tip.style.left = Math.max(8, nx) + "px";
    tip.style.top = Math.max(8, ny) + "px";
  }
  function hideTip() { tip.classList.remove("show"); }

  /* ----------------------------- EVENTOS ----------------------------- */
  function bindEvents() {
    document.addEventListener("click", function (e) {
      var card = e.target.closest && e.target.closest(".block-card");
      if (card) { selectBlock(card.getAttribute("data-block")); return; }
      var rib = e.target.closest && e.target.closest(".ribbon-node");
      if (rib) { selectBlock(rib.getAttribute("data-block")); return; }
      var copy = e.target.closest && e.target.closest("[data-copy-link]");
      if (copy) { copyLink(copy); return; }
      var goto = e.target.closest && e.target.closest("[data-goto]");
      if (goto) { selectBlock(goto.getAttribute("data-goto")); return; }
      if (e.target.closest && (e.target.closest("[data-close]"))) { closeModal(); clearSelection(); return; }
    });

    // teclado en tarjetas y nodos
    document.addEventListener("keydown", function (e) {
      if (e.key === "Escape") { closeModal(); clearSelection(); }
      var t = e.target;
      if ((e.key === "Enter" || e.key === " ") && t && t.classList && (t.classList.contains("block-card") || t.classList.contains("arch-node"))) {
        e.preventDefault();
        if (t.classList.contains("block-card")) selectBlock(t.getAttribute("data-block"));
        else selectArchNode(t.getAttribute("data-node"));
      }
    });

    // tooltip en tarjetas
    document.addEventListener("mouseover", function (e) {
      var card = e.target.closest && e.target.closest(".block-card");
      if (card) { var b = byId[card.getAttribute("data-block")]; showTip(card, b.color, b.title, b.tagline); }
    });
    document.addEventListener("mousemove", function (e) {
      if (tip.classList.contains("show") && e.target.closest && e.target.closest(".block-card")) moveTip(e.clientX, e.clientY);
    });
    document.addEventListener("mouseout", function (e) {
      var card = e.target.closest && e.target.closest(".block-card");
      if (card && !(e.relatedTarget && card.contains(e.relatedTarget))) hideTip();
    });

    // arquitectura
    document.querySelectorAll(".arch-node").forEach(function (n) {
      n.addEventListener("click", function () { state.selectedArch = null; selectArchNode(n.getAttribute("data-node")); });
      n.addEventListener("mouseenter", function () {
        var id = n.getAttribute("data-node");
        showTip(n, ARCH_NODES[id].color, ARCH_NODES[id].label, "Selecciona para resaltar sus conexiones.");
      });
      n.addEventListener("mousemove", function (e) { moveTip(e.clientX, e.clientY); });
      n.addEventListener("mouseleave", hideTip);
    });

    // flujos
    document.querySelectorAll("[data-flow-head]").forEach(function (h) {
      h.addEventListener("click", function () {
        var card = h.closest(".flow-card");
        var active = card.classList.contains("is-active");
        document.querySelectorAll(".flow-card").forEach(function (c) { c.classList.remove("is-active", "is-dim"); });
        if (!active) {
          card.classList.add("is-active");
          document.querySelectorAll(".flow-card").forEach(function (c) { if (c !== card) c.classList.add("is-dim"); });
        }
      });
    });

    document.getElementById("btnReset").addEventListener("click", resetAll);
    document.getElementById("btnAchievement").addEventListener("click", showAchievement);
    document.getElementById("canvasSearch").addEventListener("input", function (e) { filterCanvas(e.target.value); });
    window.addEventListener("hashchange", function () { applyHash(false); });

    // redibujado de curvas
    var raf = null;
    function redraw() {
      if (raf) cancelAnimationFrame(raf);
      raf = requestAnimationFrame(function () { drawCanvasLinks(); drawArchLinks(); });
    }
    window.addEventListener("resize", redraw);
    window.addEventListener("scroll", function () {
      if (window.getComputedStyle(document.getElementById("canvasSvg")).display === "none") return;
      redraw();
    }, { passive: true });
    if (window.ResizeObserver) {
      var ro = new ResizeObserver(redraw);
      ro.observe(document.getElementById("canvasShell"));
      ro.observe(document.getElementById("archShell"));
    }
    // redibujo tras cargar fuentes
    if (document.fonts && document.fonts.ready) document.fonts.ready.then(redraw);
    return redraw;
  }

  /* ----------------------------- INIT ----------------------------- */
  function init() {
    document.querySelectorAll(".btn-ico[data-ico]").forEach(function (el) {
      el.innerHTML = icon(el.getAttribute("data-ico"));
    });
    renderHero();
    renderRibbon();
    renderCanvas();
    renderArchSupport();
    renderFlows();
    renderGame();
    renderRoster();
    hydrateIcons(document);
    var redraw = bindEvents();
    // posicionar curvas cuando el layout esté listo
    requestAnimationFrame(function () { redraw(); setTimeout(redraw, 120); });
    window.addEventListener("load", function () { setTimeout(redraw, 60); });
    applyHash(true);
  }

  if (document.readyState === "loading") document.addEventListener("DOMContentLoaded", init);
  else init();
})();
