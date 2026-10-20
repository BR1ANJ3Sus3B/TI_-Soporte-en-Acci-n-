(function () {
  "use strict";

  /* ============================================================
     DATOS DEL CANVAS — TI: SOPORTE EN ACCIÓN
     ============================================================ */

  var HERO_BADGES = [
    "Videojuego educativo",
    "2D pixel art",
    "Simulación de soporte TI",
    "Misiones y puzzles",
    "XP · niveles · logros",
    "Entorno seguro",
    "Problemas reales de TI",
  ];

  var FLOW_STEPS = [
    { label: "Misión", tip: "El jugador recibe una misión contextualizada en el área de TI." },
    { label: "Investigación", tip: "Busca y recopila pistas en el entorno del videojuego." },
    { label: "Análisis", tip: "Revisa correos, reportes e indicadores relacionados con el incidente." },
    { label: "Diagnóstico", tip: "Identifica la causa del problema y decide el siguiente paso." },
    { label: "Solución", tip: "Realiza las acciones de soporte y verifica la resolución." },
    { label: "Evaluación", tip: "Analiza el resultado de la intervención realizada.", gain: true },
    { label: "Retroalimentación", tip: "Comentarios y guía sobre el desempeño del jugador.", gain: true },
    { label: "XP", tip: "El jugador obtiene puntos de experiencia por la misión.", win: true },
    { label: "Logro", tip: "Progresión en nivel, logros e insignias.", win: true },
  ];

  var CHARACTERS = [
    {
      id: "alex",
      img: "./img/personajes/alex.png",
      name: "Alex",
      role: "Protagonista · Técnico junior",
      desc: "Jugador controlable. Resuelve incidentes de soporte, redes, programación, servidores y ciberseguridad.",
    },
    {
      id: "martin",
      img: "./img/personajes/martin.png",
      name: "Martín",
      role: "Mentor · Soporte senior",
      desc: "Guía la formación de Alex y canaliza los casos del servicio de soporte de NovaTech Solutions.",
    },
    {
      id: "richar",
      img: "./img/personajes/richar.png",
      name: "Richar",
      role: "Redes y servidores",
      desc: "Serio y concentrado, con humor sarcástico. Administra la infraestructura y los servidores.",
    },
    {
      id: "marco",
      img: "./img/personajes/marco.png",
      name: "Marco",
      role: "Desarrollo de software",
      desc: "Actor secundario dentro del área de TI; apoyo en casos de soporte y programación.",
    },
    {
      id: "sofia",
      img: "./img/personajes/sofia.png",
      name: "Sofía",
      role: "Empleada de NovaTech Solutions",
      desc: "Personaje secundario que aporta contexto y apoyo en el área de TI de la empresa.",
    },
    {
      id: "saboteador",
      img: "./img/personajes/saboteador.png",
      name: "Saboteador",
      role: "Antagonista · Misterioso",
      desc: "Causa incidentes deliberadamente y es el motor del conflicto. Su identidad se desconoce.",
      antagonist: true,
    },
  ];

  var GALLERY = [
    { src: "./img/galeria/alex-llegada.png", caption: "Alex llega a NovaTech Solutions" },
    { src: "./img/galeria/sala-servidores.png", caption: "Sala de servidores" },
    { src: "./img/galeria/oficina-soporte.png", caption: "Oficina de soporte" },
    { src: "./img/galeria/alex-computadora.png", caption: "Puesto de trabajo de Alex" },
    { src: "./img/galeria/alex-martin.png", caption: "Alex hablando con Martín" },
    { src: "./img/galeria/richar-servidores.png", caption: "Richar en los servidores" },
    { src: "./img/galeria/marco-programando.png", caption: "Marco programando" },
    { src: "./img/galeria/sofia-novatech.png", caption: "Sofía en NovaTech Solutions" },
    { src: "./img/galeria/problema-red.png", caption: "Problema de red detectado" },
    { src: "./img/galeria/error-programacion.png", caption: "Error de programación" },
    { src: "./img/galeria/mision-seguridad.png", caption: "Misión de ciberseguridad" },
    { src: "./img/galeria/saboteador.png", caption: "Aparición del Saboteador" },
    { src: "./img/galeria/logro-desbloqueado.png", caption: "Logro desbloqueado" },
  ];

  var VARIANT_CLASS = {
    propuesta_valor: "",
    ingresos: "is-green",
    costos: "is-alt",
  };

  var COMPONENTS = [
    {
      id: "socios",
      area: "socios",
      index: "Componente 1 — Socios clave",
      title: "Socios clave",
      short: "Aliados académicos y tecnológicos que respaldan el proyecto.",
      icon: "./img/socios/universidad.png",
      cover: "./img/socios/equipo.png",
      chips: [
        { t: "Institución educativa", tip: "Avala el proyecto y el Proyecto Integrador." },
        { t: "Docentes / asesores", tip: "Guían el desarrollo y evalúan el avance." },
        { t: "Equipo Proyecto Integrador", tip: "Responsable del análisis, diseño y programación." },
        { t: "GitHub + Unity/Flutter", tip: "Plataformas y ecosistemas de desarrollo." },
        { t: "Tecnologías open source", tip: "Herramientas gratuitas y de código abierto." },
      ],
      tags: ["Universidad", "Docentes", "Proyecto Integrador", "GitHub", "Open source"],
      gallery: [
        "./img/socios/universidad.png",
        "./img/socios/docente.png",
        "./img/socios/equipo.png",
        "./img/socios/github.png",
        "./img/socios/open-source.png",
      ],
      metrics: [
        { v: "3", l: "tipos de socios" },
        { v: "1", l: "institución educativa" },
        { v: "Open source", l: "enfoque tecnológico" },
      ],
      sections: [
        {
          h: "Institucional",
          body: "La institución educativa y los docentes/asesores avalan el proyecto dentro del Proyecto Integrador, sin convenios comerciales.",
        },
        {
          h: "Equipo",
          body: "Brian Jesús Mendoza Márquez es responsable del análisis, diseño y programación, apoyado por el equipo del Proyecto Integrador.",
        },
        {
          h: "Tecnológico",
          list: [
            "GitHub: control de versiones y publicación.",
            "Ecosistema Unity: videojuego principal.",
            "Ecosistema Flutter: aplicación móvil complementaria.",
            "Tecnologías open source y planes educativos.",
          ],
        },
      ],
      note: "No se contemplan tiendas digitales ni convenios comerciales inventados: los socios son académicos y tecnológicos.",
    },
    {
      id: "actividades",
      area: "actividades",
      index: "Componente 2 — Actividades clave",
      title: "Actividades clave",
      short: "Desarrollo del videojuego, sus aplicaciones y su calidad.",
      icon: "./img/actividades/mision.png",
      cover: "./img/actividades/programacion.png",
      chips: [
        { t: "Gameplay (Unity/C#)", tip: "Desarrollo principal del videojuego." },
        { t: "Apps móvil + PWA", tip: "Flutter y React/PWA." },
        { t: "API y servicios", tip: "Lógica compartida del proyecto." },
        { t: "Diseño de misiones", tip: "Misiones, niveles, diálogos y puzzles." },
        { t: "Pruebas y docs", tip: "Calidad, documentación y mantenimiento." },
      ],
      tags: ["Gameplay", "Apps", "API", "Diseño", "Calidad"],
      gallery: [
        "./img/actividades/mision.png",
        "./img/actividades/investigacion.png",
        "./img/actividades/programacion.png",
        "./img/actividades/redes.png",
        "./img/actividades/servidor.png",
        "./img/actividades/xp.png",
        "./img/actividades/logro.png",
      ],
      metrics: [
        { v: "Unity", l: "videojuego principal" },
        { v: "Flutter", l: "app móvil" },
        { v: "React/PWA", l: "panel web" },
      ],
      sections: [
        {
          h: "Desarrollo",
          body: "Programación del videojuego en Unity (C#), aplicación móvil en Flutter, panel web responsivo en React/PWA y la API de servicios.",
        },
        {
          h: "Diseño",
          body: "Diseño de personajes, escenarios, mapas, diálogos y puzzles para cada misión del soporte TI.",
        },
        {
          h: "Calidad",
          list: [
            "Pruebas funcionales y de integración.",
            "Monitoreo y mantenimiento de servicios.",
            "Documentación técnica y manuales.",
          ],
        },
        {
          h: "Flujo de misión",
          body: "El jugador aprende siguiendo un ciclo completo por misión:",
          flow: true,
        },
      ],
    },
    {
      id: "recursos",
      area: "recursos",
      index: "Componente 3 — Recursos clave",
      title: "Recursos clave",
      short: "Tecnologías, activos del videojuego y equipo humano.",
      icon: "./img/recursos/servidor.png",
      cover: "./img/recursos/servidor.png",
      chips: [
        { t: "Unity · Flutter · React/PWA · API", tip: "Entornos y servicios principales." },
        { t: "Keycloak · MongoDB", tip: "Autenticación y base de datos." },
        { t: "Docker · Grafana", tip: "Infraestructura y monitoreo." },
        { t: "Git · GitHub · Pages", tip: "Versionado y publicación." },
        { t: "Sprites · mapas · sonidos", tip: "Assets propios del videojuego." },
      ],
      tags: ["Tecnologías", "Server", "Assets", "Equipo"],
      gallery: [
        "./img/recursos/servidor.png",
        "./img/recursos/base-datos.png",
        "./img/recursos/mapa.png",
        "./img/recursos/sprites.png",
        "./img/recursos/equipo.png",
      ],
      metrics: [
        { v: "4", l: "entornos de juego" },
        { v: "API", l: "servicios compartidos" },
        { v: "Assets", l: "contenido propio" },
      ],
      sections: [
        {
          h: "Tecnologías",
          list: [
            "Videojuego: Unity (C#).",
            "App móvil: Flutter (Dart).",
            "Panel web: React + PWA.",
            "Servicios: API, Keycloak, MongoDB.",
            "Infraestructura: Docker, Grafana.",
            "Versionado: Git, GitHub, GitHub Pages.",
          ],
        },
        {
          h: "Recursos del videojuego",
          body: "Sprites, personajes, mapas, animaciones, diálogos, misiones, puzzles, sonidos e interfaces en estilo 2D pixel art.",
        },
        {
          h: "Recursos humanos",
          body: "Analista, diseñador, programador (Brian Jesús Mendoza Márquez), además de los docentes y el equipo del Proyecto Integrador.",
        },
      ],
    },
    {
      id: "propuesta-valor",
      area: "propuesta",
      index: "Componente 4 — Propuesta de valor",
      title: "Propuesta de valor",
      short:
        "Aprender TI jugando: el jugador encarna a Alex y resuelve problemas reales de soporte en un entorno seguro y gamificado.",
      icon: "./img/personajes/alex.png",
      cover: "./img/propuesta/alex-propuesta-valor.png",
      valueQuote:
        "Aprender y practicar habilidades de Tecnologías de la Información mediante un videojuego educativo que simula problemas reales de soporte y servicio técnico en un entorno seguro y gamificado.",
      chips: [
        { t: "Aprendizaje práctico" },
        { t: "Simulación" },
        { t: "Gamificación" },
        { t: "Entorno seguro" },
        { t: "Problemas reales" },
        { t: "Retroalimentación" },
        { t: "XP" },
        { t: "Niveles" },
        { t: "Logros" },
        { t: "Preparación profesional" },
      ],
      tags: ["Aprender jugando", "Simulación TI", "Sin riesgos"],
      gallery: [
        "./img/propuesta/alex-propuesta-valor.png",
        "./img/galeria/alex-computadora.png",
        "./img/galeria/problema-red.png",
      ],
      metrics: [
        { v: "9", l: "áreas de TI" },
        { v: "XP + logros", l: "feedback continuo" },
        { v: "Seguro", l: "entorno simulado" },
      ],
      sections: [
        {
          h: "Propuesta",
          body: "El jugador afronta incidentes inspirados en problemas reales de un servicio de soporte TI, obteniendo retroalimentación y progresión.",
        },
        {
          h: "El jugador (Alex)",
          list: [
            "Investigar, analizar y diagnosticar incidentes.",
            "Hablar con personajes y buscar pistas.",
            "Decidir el siguiente paso y aplicarlo.",
            "Recibir retroalimentación y desbloquear XP, niveles, logros e insignias.",
          ],
        },
        {
          h: "Áreas que se practican",
          body: "Soporte, redes, programación, bases de datos, servidores, infraestructura y ciberseguridad.",
        },
      ],
    },
    {
      id: "relacion",
      area: "relacion",
      index: "Componente 5 — Relación con el usuario",
      title: "Relación con el usuario",
      short: "El juego mantiene al estudiante motivado con progresión y logros.",
      icon: "./img/relacion/logro-desbloqueado.png",
      cover: "./img/relacion/logro-desbloqueado.png",
      chips: [
        { t: "Misiones y progreso", tip: "Avance por misiones completadas." },
        { t: "XP · niveles · logros", tip: "Progresión del jugador." },
        { t: "Insignias y estadísticas", tip: "Reconocimiento del desempeño." },
        { t: "Notificaciones", tip: "Alertas de logros y eventos." },
        { t: "Retroalimentación", tip: "Guia posterior a cada misión." },
      ],
      tags: ["Progreso", "XP", "Logros", "Feedback"],
      gallery: [
        "./img/relacion/logro-desbloqueado.png",
        "./img/relacion/barra-xp.png",
        "./img/relacion/insignias.png",
        "./img/relacion/progreso.png",
        "./img/relacion/notificacion.png",
      ],
      metrics: [
        { v: "XP", l: "por misión" },
        { v: "Insignias", l: "reconocimiento" },
        { v: "Historial", l: "de progreso" },
      ],
      sections: [
        {
          h: "Producción automotivada",
          body: "La relación se construye dentro del juego: cada misión aporta experiencia, niveles, insignias y un historial de progreso visible.",
        },
        {
          h: "Feedback",
          body: "Notificaciones, retroalimentación y estadísticas mantienen al estudiante informado de su avance y lo motivan a continuar.",
        },
        {
          h: "Flujo de interacción",
          body: "Jugador → misión → retroalimentación → XP → nivel → logro → notificación.",
        },
      ],
    },
    {
      id: "canales",
      area: "canales",
      index: "Componente 6 — Canales",
      title: "Canales",
      short: "Diversos entornos para jugar, administrar y documentar.",
      icon: "./img/canales/unity-pc.png",
      cover: "./img/canales/unity-pc.png",
      chips: [
        { t: "Unity · PC", tip: "Videojuego principal para escritorio." },
        { t: "Flutter · móvil", tip: "App complementaria." },
        { t: "React/PWA · web", tip: "Panel de administración." },
        { t: "GitHub Pages", tip: "Documentación y este Canvas." },
      ],
      tags: ["PC", "Móvil", "Web", "Docs"],
      gallery: [
        "./img/canales/unity-pc.png",
        "./img/canales/flutter-movil.png",
        "./img/canales/react-pwa.png",
        "./img/canales/github-pages.png",
      ],
      metrics: [
        { v: "4", l: "canales" },
        { v: "1", l: "videojuego principal" },
        { v: "3", l: "entornos de acceso" },
      ],
      sections: [
        {
          h: "Videojuego principal",
          body: "Unity (PC) es el canal central donde se desarrollan las misiones del soporte TI.",
        },
        {
          h: "Aplicaciones complementarias",
          list: [
            "Flutter (móvil): acceso complementario según disponibilidad.",
            "React/PWA (web): panel de administración y seguimiento.",
            "GitHub Pages: publicación de la documentación y del Canvas.",
          ],
        },
        {
          h: "Estrategia",
          body: "Un mismo proyecto integrador se publica en varios entornos con identidad visual consistente.",
        },
      ],
    },
    {
      id: "segmentos",
      area: "segmentos",
      index: "Componente 7 — Segmentos de mercado",
      title: "Segmentos de mercado",
      short: "Estudiantes de TI como jugadores, con roles de apoyo educativo.",
      icon: "./img/usuarios/jugador-estudiante.png",
      cover: "./img/usuarios/jugador-estudiante.png",
      chips: [
        { t: "Jugador / Estudiante", tip: "Perfil principal del jugador." },
        { t: "Docente / Evaluador", tip: "Usa el juego como apoyo." },
        { t: "Administrador", tip: "Gestiona acceso y configuraciones." },
        { t: "Editor de contenido", tip: "Crea y actualiza contenido." },
        { t: "Institución educativa", tip: "Implementa el proyecto." },
      ],
      tags: ["Jugador", "Docente", "Institución"],
      gallery: [
        "./img/usuarios/jugador-estudiante.png",
        "./img/usuarios/docente.png",
        "./img/usuarios/administrador.png",
        "./img/usuarios/editor.png",
        "./img/usuarios/institucion.png",
      ],
      metrics: [
        { v: "5", l: "segmentos" },
        { v: "1", l: "jugador principal" },
        { v: "2", l: "roles de apoyo" },
      ],
      sections: [
        {
          h: "Perfiles identificados",
          list: [
            "Jugador / Estudiante: interactúa con el juego y completa misiones.",
            "Docente / Evaluador: usa el videojuego como herramienta de apoyo.",
            "Administrador: gestiona usuarios, acceso y configuración.",
            "Editor de contenido: crea y mantiene misiones y materiales.",
            "Institución educativa: implementa y difunde el proyecto.",
          ],
        },
        {
          h: "Aclaración",
          body: "No se define un rango de edad específico ni características poblacionales no establecidas oficialmente por el proyecto.",
        },
      ],
    },
    {
      id: "costos",
      area: "costos",
      index: "Componente 8 — Estructura de costes",
      title: "Estructura de costes",
      short: "Costos de desarrollo, operación e infraestructura del proyecto.",
      icon: "./img/costos/servidor.png",
      cover: "./img/costos/desarrollo.png",
      chips: [
        { t: "Desarrollo", tip: "Tiempo y herramientas de desarrollo." },
        { t: "Videojuego", tip: "Diseño, arte y contenido." },
        { t: "Infraestructura", tip: "Servidores y servicios en la nube." },
        { t: "Operación", tip: "Publicación y monitoreo." },
        { t: "Recursos humanos", tip: "Equipo del Proyecto Integrador." },
      ],
      tags: ["Desarrollo", "Infraestructura", "Operación"],
      gallery: [
        "./img/costos/desarrollo.png",
        "./img/costos/diseno.png",
        "./img/costos/servidor.png",
        "./img/costos/nube.png",
        "./img/costos/mantenimiento.png",
      ],
      metrics: [
        { v: "5", l: "categorías" },
        { v: "Académico", l: "etapa actual" },
        { v: "Open source", l: "prioridad" },
      ],
      sections: [
        {
          h: "Categorías de costo",
          list: [
            "Desarrollo: programación del videojuego y aplicaciones.",
            "Videojuego: diseño, arte pixel art y contenido.",
            "Infraestructura: servidores y servicios en la nube.",
            "Operación: publicación, monitoreo y mantenimiento.",
            "Recursos humanos: equipo y docentes del Proyecto Integrador.",
          ],
        },
        {
          h: "Etapa académica",
          body: "Durante el desarrollo académico se prioriza el uso de herramientas gratuitas, open source o planes educativos.",
        },
      ],
    },
    {
      id: "ingresos",
      area: "ingresos",
      index: "Componente 9 — Ingresos / Sostenibilidad",
      title: "Ingresos / Sostenibilidad",
      short: "Etapa actual académica; posibilidades futuras sin monetización inventada.",
      icon: "./img/sostenibilidad/proyecto-academico.png",
      cover: "./img/sostenibilidad/proyecto-academico.png",
      chips: [
        { t: "Etapa actual: proyecto académico", tip: "Sin monetización directa." },
        { t: "Sostenimiento", tip: "Recursos educativos y de desarrollo." },
        { t: "Futuro: licencias educativas", tip: "Posibilidad a futuro." },
        { t: "Futuro: convenios", tip: "Acuerdos académicos futuros." },
        { t: "Futuro: capacitación", tip: "Formación con el juego." },
      ],
      tags: ["Etapa académica", "Sin monetización", "Posibilidades futuras"],
      gallery: [
        "./img/sostenibilidad/proyecto-academico.png",
        "./img/sostenibilidad/universidad.png",
        "./img/sostenibilidad/licencia.png",
        "./img/sostenibilidad/capacitacion.png",
      ],
      metrics: [
        { v: "Actual", l: "proyecto académico" },
        { v: "Futuro", l: "posibilidades" },
        { v: "0", l: "monetización directa" },
      ],
      sections: [
        {
          h: "Etapa actual — Proyecto académico",
          body: "El videojuego es un proyecto académico en desarrollo con fines educativos. No existe monetización directa ni modelo comercial activo.",
        },
        {
          h: "Posibilidades futuras (no comprometidas)",
          body: "Opciones que el proyecto podría explorar cuando deje de ser una práctica académica:",
          list: [
            "Licencias educativas para instituciones.",
            "Implementación institucional del videojuego.",
            "Capacitación basada en el juego.",
            "Convenios académicos.",
            "Módulos especializados y contenido adicional.",
          ],
        },
        {
          h: "Aclaración",
          body: "No se contemplan ingresos inventados por venta única, contenido de pago, publicidad ni tienda digital en la etapa actual.",
        },
      ],
    },
  ];

  var CONNECTIONS = [
    ["socios", "actividades"],
    ["socios", "recursos"],
    ["actividades", "recursos"],
    ["actividades", "propuesta-valor"],
    ["recursos", "propuesta-valor"],
    ["recursos", "costos"],
    ["propuesta-valor", "segmentos"],
    ["propuesta-valor", "canales"],
    ["propuesta-valor", "relacion"],
    ["relacion", "segmentos"],
    ["canales", "segmentos"],
    ["socios", "ingresos"],
  ];

  var TECH_FLOW = [
    { label: "USUARIO", cls: "is-user" },
    { arrow: true },
    { label: "KEYCLOAK", cls: "is-auth" },
    { arrow: true },
    {
      apps: [
        { label: "UNITY", cls: "is-app" },
        { label: "FLUTTER", cls: "is-app" },
        { label: "REACT/PWA", cls: "is-app" },
      ],
    },
    { arrow: true },
    { label: "API", cls: "is-api" },
    { arrow: true },
    { label: "MONGODB", cls: "is-db" },
  ];

  var TECH_SIDE = [
    { label: "DOCKER", desc: "Infraestructura" },
    { label: "GRAFANA", desc: "Monitoreo" },
    { label: "GIT/GITHUB", desc: "Versionado" },
    { label: "GITHUB PAGES", desc: "Publicación" },
  ];

  var TECH_LIST = [
    { name: "Unity", desc: "Videojuego principal (C#)" },
    { name: "Flutter", desc: "App móvil complementaria (Dart)" },
    { name: "React", desc: "Panel web de administración" },
    { name: "PWA", desc: "Progressive Web App" },
    { name: "API", desc: "Lógica y servicios compartidos" },
    { name: "Keycloak", desc: "Autenticación" },
    { name: "MongoDB", desc: "Base de datos" },
    { name: "Docker", desc: "Infraestructura / despliegue" },
    { name: "Grafana", desc: "Monitoreo" },
    { name: "Git", desc: "Control de versiones" },
    { name: "GitHub", desc: "Repositorios / colaboración" },
    { name: "GitHub Pages", desc: "Publicación del Canvas y docs" },
  ];

  /* ============================================================
     UTILIDADES
     ============================================================ */

  function $(sel, ctx) {
    return (ctx || document).querySelector(sel);
  }

  function byId(id) {
    return document.getElementById(id);
  }

  var componentById = {};
  COMPONENTS.forEach(function (c) {
    componentById[c.id] = c;
  });

  var neighborMap = {};
  CONNECTIONS.forEach(function (edge) {
    (neighborMap[edge[0]] = neighborMap[edge[0]] || []).push(edge[1]);
    (neighborMap[edge[1]] = neighborMap[edge[1]] || []).push(edge[0]);
  });

  var board = {
    tooltip: null,
    modal: null,
    lightbox: null,
    currentFocus: null,
    modalOpenFor: null,
  };

  /* ============================================================
     RENDER: HERO + FLUJO
     ============================================================ */

  function renderHeroBadges() {
    var ul = byId("hero-badges");
    HERO_BADGES.forEach(function (b) {
      var li = document.createElement("li");
      li.textContent = b;
      ul.appendChild(li);
    });
  }

  function renderFlow() {
    var ol = byId("flow-track");
    FLOW_STEPS.forEach(function (step, i) {
      var li = document.createElement("li");
      var div = document.createElement("div");
      div.className = "flow-step" + (step.gain ? " is-gain" : "") + (step.win ? " is-win" : "");
      div.tabIndex = 0;
      div.setAttribute("role", "listitem");
      div.title = step.tip;
      var num = document.createElement("b");
      num.textContent = i + 1;
      var label = document.createElement("span");
      label.textContent = step.label;
      div.appendChild(num);
      div.appendChild(label);
      li.appendChild(div);
      if (i < FLOW_STEPS.length - 1) {
        var arrow = document.createElement("span");
        arrow.className = "flow-arrow";
        arrow.setAttribute("aria-hidden", "true");
        arrow.textContent = "→";
        li.appendChild(arrow);
      }
      attachTooltip(div, step.label, step.tip);
      div.addEventListener("click", function () {
        showTooltipNear(div, step.label, step.tip);
      });
      ol.appendChild(li);
    });
  }

  /* ============================================================
     RENDER: PERSONAJES Y GALERÍA
     ============================================================ */

  function renderCharacters() {
    var ul = byId("characters-grid");
    CHARACTERS.forEach(function (ch) {
      var li = document.createElement("li");
      var card = document.createElement("article");
      card.className = "character-card" + (ch.antagonist ? " is-antagonist" : "");
      card.tabIndex = 0;
      var img = document.createElement("img");
      img.src = ch.img;
      img.alt = "Hoja de sprite de " + ch.name + ", personaje de TI: Soporte en Acción";
      img.loading = "lazy";
      var h3 = document.createElement("h3");
      h3.innerHTML = "<b>" + ch.name + "</b>";
      var role = document.createElement("p");
      role.className = "role";
      role.textContent = ch.role;
      var desc = document.createElement("p");
      desc.textContent = ch.desc;
      card.appendChild(img);
      card.appendChild(h3);
      card.appendChild(role);
      card.appendChild(desc);
      li.appendChild(card);
      ul.appendChild(li);
    });
  }

  function renderGallery(grid) {
    var root = grid || byId("gallery-grid");
    GALLERY.forEach(function (scene) {
      var li = document.createElement("li");
      var fig = document.createElement("figure");
      fig.className = "gallery-item";
      fig.tabIndex = 0;
      var img = document.createElement("img");
      img.src = scene.src;
      img.alt = scene.caption;
      img.loading = "lazy";
      var cap = document.createElement("figcaption");
      cap.textContent = scene.caption;
      fig.appendChild(img);
      fig.appendChild(cap);
      li.appendChild(fig);
      var open = function () {
        openLightbox(scene.src, scene.caption);
      };
      fig.addEventListener("click", open);
      fig.addEventListener("keydown", function (ev) {
        if (ev.key === "Enter" || ev.key === " ") {
          ev.preventDefault();
          open();
        }
      });
      root.appendChild(li);
    });
  }

  /* ============================================================
     RENDER: TECNOLOGÍAS
     ============================================================ */

  function renderTech() {
    var arch = byId("architecture");
    var flow = document.createElement("div");
    flow.className = "arch-flow";
    TECH_FLOW.forEach(function (node) {
      if (node.arrow) {
        var a = document.createElement("span");
        a.className = "arch-arrow";
        a.setAttribute("aria-hidden", "true");
        a.textContent = "→";
        flow.appendChild(a);
        return;
      }
      if (node.apps) {
        var apps = document.createElement("div");
        apps.className = "arch-apps";
        node.apps.forEach(function (app) {
          var n = document.createElement("span");
          n.className = "arch-node " + app.cls;
          n.textContent = app.label;
          apps.appendChild(n);
        });
        flow.appendChild(apps);
        return;
      }
      var n = document.createElement("span");
      n.className = "arch-node " + node.cls;
      n.textContent = node.label;
      flow.appendChild(n);
    });

    var side = document.createElement("div");
    side.className = "arch-side";
    TECH_SIDE.forEach(function (t) {
      var node = document.createElement("span");
      node.className = "arch-node";
      node.title = t.desc;
      node.textContent = t.label;
      side.appendChild(node);
    });

    arch.appendChild(flow);
    arch.appendChild(side);

    var list = byId("tech-list");
    TECH_LIST.forEach(function (t) {
      var li = document.createElement("li");
      var b = document.createElement("b");
      b.textContent = t.name;
      var s = document.createElement("span");
      s.textContent = t.desc;
      li.appendChild(b);
      li.appendChild(s);
      list.appendChild(li);
    });
  }

  /* ============================================================
     RENDER: BLOQUES DEL CANVAS
     ============================================================ */

  function makeBlock(c) {
    var isValue = c.id === "propuesta-valor";
    var art = document.createElement("article");
    art.className =
      "bmc-block blk-" + c.id + (isValue ? " bmc-block--value" : "");
    art.setAttribute("data-component", c.id);
    art.setAttribute("aria-label", c.title + ". Hacer clic abre la ficha detallada.");
    art.tabIndex = 0;

    var cover = document.createElement("figure");
    cover.className = "blk-cover";
    var coverImg = document.createElement("img");
    coverImg.src = c.cover;
    coverImg.alt = "Ilustración de " + c.title;
    coverImg.loading = "lazy";
    cover.appendChild(coverImg);

    var head = document.createElement("header");
    head.className = "blk-head";
    var icon = document.createElement("span");
    icon.className = "blk-icon";
    var iconImg = document.createElement("img");
    iconImg.src = c.icon;
    iconImg.alt = "";
    icon.appendChild(iconImg);

    var box = document.createElement("div");
    box.className = "blk-title-box";
    var idx = document.createElement("span");
    idx.className = "blk-index";
    idx.textContent = c.index;
    var h3 = document.createElement("h3");
    h3.className = "blk-title";
    h3.textContent = c.title;
    box.appendChild(idx);
    box.appendChild(h3);

    var expand = document.createElement("button");
    expand.className = "blk-expand";
    expand.type = "button";
    expand.setAttribute("aria-label", "Abrir ficha de " + c.title);
    expand.textContent = "+";

    head.appendChild(icon);
    head.appendChild(box);
    head.appendChild(expand);

    var short = document.createElement("p");
    short.className = "blk-short";
    if (isValue) {
      short.className = "blk-value-quote";
      short.innerHTML = highlightQuote(c.valueQuote);
    } else {
      short.textContent = c.short;
    }

    var chips = document.createElement("ul");
    chips.className = "blk-chips";
    var visibleChips = c.chips;
    if (!isValue && visibleChips.length > 4) {
      visibleChips = visibleChips.slice(0, 4);
    }
    visibleChips.forEach(function (chip) {
      var li = document.createElement("li");
      li.textContent = chip.t;
      chips.appendChild(li);
      attachTooltip(li, chip.t, chip.tip || c.title);
    });

    var tags = document.createElement("div");
    tags.className = "blk-tags";
    (c.tags || []).forEach(function (t) {
      var i = document.createElement("i");
      i.textContent = t;
      tags.appendChild(i);
    });

    var cta = document.createElement("button");
    cta.type = "button";
    cta.className = "btn-detail" + (VARIANT_CLASS[c.id] ? " " + VARIANT_CLASS[c.id] : "");
    cta.textContent = "Ver detalle";

    art.appendChild(cover);
    art.appendChild(head);
    art.appendChild(short);
    art.appendChild(chips);
    art.appendChild(tags);
    art.appendChild(cta);

    var open = function (ev) {
      ev.preventDefault();
      ev.stopPropagation();
      openModal(c.id);
    };

    art.addEventListener("click", function () {
      openModal(c.id);
    });
    art.addEventListener("keydown", function (ev) {
      if (ev.key === "Enter" || ev.key === " ") {
        ev.preventDefault();
        openModal(c.id);
      }
    });
    expand.addEventListener("click", open);
    cta.addEventListener("click", open);

    art.addEventListener("mouseenter", function () {
      focusComponent(c.id, false);
    });
    art.addEventListener("mouseleave", function () {
      if (board.modalOpenFor !== c.id) clearEmphasis();
    });
    art.addEventListener("focus", function () {
      focusComponent(c.id, false);
    });
    art.addEventListener("blur", function () {
      if (board.modalOpenFor !== c.id) clearEmphasis();
    });

    return art;
  }

  function escapeHtml(str) {
    return String(str)
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;");
  }

  function highlightQuote(text) {
    var escaped = escapeHtml(text);
    return escaped.replace(
      /(Tecnolog[^\s]+ de la Informaci[^\s]+)/g,
      "<strong>$1</strong>"
    );
  }

  function renderCanvas() {
    var grid = byId("bmc");
    COMPONENTS.forEach(function (c) {
      grid.appendChild(makeBlock(c));
    });
  }

  /* ============================================================
     CONEXIONES SVG
     ============================================================ */

  function renderLinks() {
    var svg = byId("bmc-links");
    svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    CONNECTIONS.forEach(function (edge) {
      var path = document.createElementNS("http://www.w3.org/2000/svg", "path");
      path.setAttribute("data-a", edge[0]);
      path.setAttribute("data-b", edge[1]);
      svg.appendChild(path);
    });
    positionLinks();
    window.addEventListener("resize", debounce(positionLinks, 200));
  }

  function positionLinks() {
    var svg = byId("bmc-links");
    var grid = byId("bmc");
    if (!grid) return;
    var gr = grid.getBoundingClientRect();
    svg.setAttribute("viewBox", "0 0 " + gr.width + " " + gr.height);
    svg.querySelectorAll("path").forEach(function (path) {
      var a = componentById[path.getAttribute("data-a")];
      var b = componentById[path.getAttribute("data-b")];
      var elA = grid.querySelector(".blk-" + a.id);
      var elB = grid.querySelector(".blk-" + b.id);
      if (!elA || !elB) return;
      var rectA = elA.getBoundingClientRect();
      var rectB = elB.getBoundingClientRect();
      var x1 = rectA.left - gr.left + rectA.width / 2;
      var y1 = rectA.top - gr.top + rectA.height / 2;
      var x2 = rectB.left - gr.left + rectB.width / 2;
      var y2 = rectB.top - gr.top + rectB.height / 2;
      var dx = Math.max(28, Math.abs(x2 - x1) / 2);
      var d =
        "M " +
        x1.toFixed(1) +
        " " +
        y1.toFixed(1) +
        " C " +
        (x1 + (x2 > x1 ? dx : -dx)).toFixed(1) +
        " " +
        y1.toFixed(1) +
        ", " +
        (x2 + (x2 > x1 ? -dx : dx)).toFixed(1) +
        " " +
        y2.toFixed(1) +
        ", " +
        x2.toFixed(1) +
        " " +
        y2.toFixed(1);
      path.setAttribute("d", d);
      if (rectA.top < gr.top - 6 || rectB.top < gr.top - 6) {
        path.setAttribute("visibility", "hidden");
      } else {
        path.setAttribute("visibility", "visible");
      }
    });
  }

  function debounce(fn, wait) {
    var t;
    return function () {
      clearTimeout(t);
      t = setTimeout(fn, wait);
    };
  }

  /* ============================================================
     ENFOQUE: RESALTAR CONEXIONES / ATENUAR BLOQUES
     ============================================================ */

  function focusComponent(id, sustained) {
    if (board.currentFocus === id && sustained) return;
    board.currentFocus = id;
    clearClass("is-focused", "related", "dim");
    var block = $("#bmc .blk-" + id);
    if (block) block.classList.add("is-focused");
    var neighbors = neighborMap[id] || [];
    COMPONENTS.forEach(function (c) {
      var el = $("#bmc .blk-" + c.id);
      if (!el) return;
      if (c.id === id) return;
      if (neighbors.indexOf(c.id) !== -1) {
        el.classList.add("related");
      } else {
        el.classList.add("dim");
      }
    });
    // Resalta las conexiones incidentes al bloque
    var svg = byId("bmc-links");
    svg.querySelectorAll("path").forEach(function (path) {
      var a = path.getAttribute("data-a");
      var b = path.getAttribute("data-b");
      if (a === id || b === id) {
        path.classList.add("hl");
        path.style.opacity = "1";
      } else {
        path.classList.remove("hl");
        path.style.opacity = "0.12";
      }
    });
  }

  function clearEmphasis() {
    board.currentFocus = null;
    clearClass("is-focused", "related", "dim");
    var svg = byId("bmc-links");
    svg.querySelectorAll("path").forEach(function (path) {
      path.classList.remove("hl");
      path.style.opacity = "";
    });
  }

  function clearClass() {
    var classes = Array.prototype.slice.call(arguments);
    var blocks = document.querySelectorAll(".bmc-block");
    blocks.forEach(function (b) {
      classes.forEach(function (c) {
        b.classList.remove(c);
      });
    });
  }

  /* ============================================================
     MODAL DE DETALLE
     ============================================================ */

  function buildModalBody(c, contextFlows) {
    var frag = document.createDocumentFragment();

    var cover = document.createElement("img");
    cover.className = "modal-cover";
    cover.src = c.cover;
    cover.alt = "Ilustración de " + c.title;
    frag.appendChild(cover);

    if (c.metrics && c.metrics.length) {
      var metricRow = document.createElement("div");
      metricRow.className = "metric-row";
      c.metrics.forEach(function (m) {
        var box = document.createElement("div");
        box.className = "metric";
        var b = document.createElement("b");
        b.textContent = m.v;
        var s = document.createElement("span");
        s.textContent = m.l;
        box.appendChild(b);
        box.appendChild(s);
        metricRow.appendChild(box);
      });
      frag.appendChild(metricRow);
    }

    (c.sections || []).forEach(function (sec) {
      var section = document.createElement("section");
      section.className = "modal-section";
      var h3 = document.createElement("h3");
      h3.textContent = sec.h;
      section.appendChild(h3);
      if (sec.body) {
        var p = document.createElement("p");
        p.textContent = sec.body;
        section.appendChild(p);
      }
      if (sec.flow) {
        section.appendChild(buildFlowStrip());
      }
      if (sec.list) {
        var ul = document.createElement("ul");
        sec.list.forEach(function (item) {
          var li = document.createElement("li");
          li.textContent = item;
          ul.appendChild(li);
        });
        section.appendChild(ul);
      }
      frag.appendChild(section);
    });

    if (c.note) {
      var note = document.createElement("div");
      note.className = "modal-note";
      note.textContent = c.note;
      frag.appendChild(note);
    }

    if (c.gallery && c.gallery.length) {
      var gridTitle = document.createElement("section");
      gridTitle.className = "modal-section";
      var h3g = document.createElement("h3");
      h3g.textContent = "Imagenes";
      gridTitle.appendChild(h3g);
      var gGrid = document.createElement("div");
      gGrid.className = "modal-gallery";
      c.gallery.forEach(function (src) {
        var img = document.createElement("img");
        img.src = src;
        img.alt = "Imagen de " + c.title;
        img.loading = "lazy";
        img.addEventListener("click", function () {
          openLightbox(src, c.title);
        });
        gGrid.appendChild(img);
      });
      gridTitle.appendChild(gGrid);
      frag.appendChild(gridTitle);
    }

    if (c.tags && c.tags.length) {
      var tags = document.createElement("div");
      tags.className = "modal-tags";
      c.tags.forEach(function (t) {
        var span = document.createElement("span");
        span.textContent = t;
        tags.appendChild(span);
      });
      frag.appendChild(tags);
    }

    return frag;
  }

  function buildFlowStrip() {
    var ol = document.createElement("ol");
    ol.className = "flow-track";
    ol.style.justifyContent = "flex-start";
    FLOW_STEPS.forEach(function (step, i) {
      var li = document.createElement("li");
      var div = document.createElement("div");
      div.className = "flow-step" + (step.gain ? " is-gain" : "") + (step.win ? " is-win" : "");
      div.title = step.tip;
      var num = document.createElement("b");
      num.textContent = i + 1;
      var label = document.createElement("span");
      label.textContent = step.label;
      div.appendChild(num);
      div.appendChild(label);
      li.appendChild(div);
      if (i < FLOW_STEPS.length - 1) {
        var arrow = document.createElement("span");
        arrow.className = "flow-arrow";
        arrow.textContent = "→";
        li.appendChild(arrow);
      }
      ol.appendChild(li);
    });
    return ol;
  }

  function openModal(id) {
    var c = componentById[id];
    if (!c) return;
    board.modalOpenFor = id;
    byId("modal-icon").innerHTML = "";
    var img = document.createElement("img");
    img.src = c.icon;
    img.alt = "";
    byId("modal-icon").appendChild(img);
    byId("modal-index").textContent = c.index;
    byId("modal-title").textContent = c.title;
    var body = byId("modal-body");
    body.innerHTML = "";
    body.appendChild(buildModalBody(c));
    var modal = byId("modal");
    modal.classList.add("is-open");
    modal.setAttribute("aria-hidden", "false");
    document.body.classList.add("no-scroll");
    focusComponent(id, true);
    setTimeout(function () {
      var closeBtn = modal.querySelector(".modal-close");
      if (closeBtn) closeBtn.focus();
    }, 60);
  }

  function closeModal() {
    var modal = byId("modal");
    modal.classList.remove("is-open");
    modal.setAttribute("aria-hidden", "true");
    document.body.classList.remove("no-scroll");
    board.modalOpenFor = null;
    clearEmphasis();
  }

  /* ============================================================
     LIGHTBOX
     ============================================================ */

  function openLightbox(src, caption) {
    var lb = byId("lightbox");
    var img = byId("lightbox-img");
    img.src = src;
    img.alt = caption || "";
    byId("lightbox-caption").textContent = caption || "";
    lb.classList.add("is-open");
    lb.setAttribute("aria-hidden", "false");
    document.body.classList.add("no-scroll");
  }

  function closeLightbox() {
    var lb = byId("lightbox");
    lb.classList.remove("is-open");
    lb.setAttribute("aria-hidden", "true");
    if (!byId("modal").classList.contains("is-open")) {
      document.body.classList.remove("no-scroll");
    }
  }

  /* ============================================================
     TOOLTIP
     ============================================================ */

  function attachTooltip(el, title, text) {
    el.addEventListener("mouseenter", function () {
      if (text) showTooltipNear(el, title, text);
    });
    el.addEventListener("mousemove", function (ev) {
      if (board.tooltip && board.tooltip.classList.contains("is-visible")) {
        positionTooltip(ev.clientX, ev.clientY);
      }
    });
    el.addEventListener("mouseleave", hideTooltip);
    el.addEventListener("focus", function () {
      if (text) showTooltipNear(el, title, text);
    });
    el.addEventListener("blur", hideTooltip);
  }

  function showTooltipNear(el, title, text) {
    var tip = board.tooltip;
    tip.innerHTML = "";
    if (title) {
      var b = document.createElement("b");
      b.textContent = title;
      tip.appendChild(b);
    }
    var span = document.createElement("span");
    span.textContent = text;
    tip.appendChild(span);
    tip.classList.add("is-visible");
    var r = el.getBoundingClientRect();
    positionTooltip(r.left + r.width / 2, r.top + window.scrollY - 10, 0);
  }

  function positionTooltip(x, y) {
    var tip = board.tooltip;
    var w = tip.offsetWidth;
    var h = tip.offsetHeight;
    var left = x - w / 2;
    var top = y - h - 12;
    if (left < 8) left = 8;
    if (left + w > window.innerWidth - 8) left = window.innerWidth - w - 8;
    if (top < 8) top = y + 18;
    tip.style.left = left + "px";
    tip.style.top = top + "px";
  }

  function hideTooltip() {
    if (board.tooltip) board.tooltip.classList.remove("is-visible");
  }

  /* ============================================================
     ARRANQUE
     ============================================================ */

  function bindGlobals() {
    board.tooltip = byId("tooltip");
    board.modal = byId("modal");
    board.lightbox = byId("lightbox");

    document.querySelectorAll("[data-close-modal]").forEach(function (el) {
      el.addEventListener("click", closeModal);
    });
    document.querySelectorAll("[data-close-lightbox]").forEach(function (el) {
      el.addEventListener("click", closeLightbox);
    });

    document.addEventListener("keydown", function (ev) {
      if (ev.key === "Escape") {
        if (byId("modal").classList.contains("is-open")) {
          closeModal();
        } else if (byId("lightbox").classList.contains("is-open")) {
          closeLightbox();
        } else {
          clearEmphasis();
        }
      }
    });
  }

  function init() {
    bindGlobals();
    renderHeroBadges();
    renderFlow();
    renderCharacters();
    renderGallery();
    renderTech();
    renderCanvas();
    renderLinks();
    // Recalcular conexiones cuando todas las imágenes cargan
    if (document.readyState === "complete") {
      positionLinks();
    } else {
      window.addEventListener("load", positionLinks);
    }
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
  } else {
    init();
  }
})();