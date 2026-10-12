/* ==========================================================================
   TI: Soporte en Acción — Business Model Canvas Interactivo
   Práctica 04 · js/canvas.js
   Canvas de 9 componentes con modales, tooltips, conexiones y animaciones.
   Sin dependencias externas: HTML5 + CSS3 + JavaScript.
   ========================================================================== */
"use strict";

/* ------------------------------------------------------------------ *
 *  DATOS DEL CANVAS — 9 componentes
 * ------------------------------------------------------------------ */
const CANVAS_DATA = [
  {
    id: "segmentos",
    index: "01",
    area: "segments",
    title: "Segmentos de clientes / usuarios",
    short: "Jóvenes y estudiantes que quieren acercarse a las TI jugando.",
    icon: { type: "img", src: "./img/estudiante.png", alt: "Estudiante, usuario objetivo del videojuego" },
    chips: [
      { text: "Jóvenes de 15 a 30 años", tip: "Público principal interesado en videojuegos, tecnología y retos." },
      { text: "Estudiantes de TI e informática", tip: "Estudiantes que desean practicar soporte, redes, hardware y programación." },
      { text: "Instituciones educativas", tip: "Escuelas, universidades y centros de capacitación interesados en el material." },
      { text: "Sin conocimientos técnicos previos", tip: "No se requieren conocimientos avanzados para comenzar a jugar." },
      { text: "Roles: Jugador y Administrador", tip: "El rol Docente o evaluador se contempla como ampliación opcional." }
    ],
    person: { img: "./img/estudiante.png", label: "Persona: jugador / estudiante" },
    detail: {
      desc: "TI: Soporte en Acción está dirigido a jóvenes interesados en los videojuegos, la tecnología y la resolución de problemas, que desean acercarse al mundo de las Tecnologías de la Información sin requerir conocimientos técnicos avanzados.",
      metrics: [
        { value: "15–30 años", label: "Rango de edad" },
        { value: "Sin requisitos", label: "Conocimientos previos" },
        { value: "3", label: "Aplicaciones integradas" }
      ],
      sections: [
        {
          heading: "Perfil del público objetivo",
          list: [
            "Edad: jóvenes de entre 15 y 30 años.",
            "Intereses: videojuegos, tecnología y resolución de problemas.",
            "Perfil: principalmente estudiantes y personas que desean conocer más sobre las TI.",
            "Conocimientos previos: no se requieren conocimientos técnicos avanzados.",
            "Áreas relacionadas: soporte técnico, programación, redes, bases de datos y ciberseguridad."
          ]
        },
        {
          heading: "Roles del sistema",
          list: [
            "Jugador: juega en Unity; consulta misiones, guías y su progreso; edita datos básicos de su perfil.",
            "Administrador: gestiona misiones, guías y cuentas; publica contenido desde la PWA.",
            "Docente o evaluador (ampliación opcional): consulta el progreso de grupos autorizados."
          ]
        },
        {
          heading: "Público adicional",
          list: [
            "Escuelas, universidades y centros de capacitación que busquen material de apoyo para clases de informática y soporte técnico."
          ]
        }
      ],
      gallery: [
        { img: "./img/estudiante.png", caption: "Estudiante / jugador" },
        { img: "./img/elena.png", caption: "Docente / evaluadora" }
      ],
      tags: ["15–30 años", "Estudiantes", "Jugador", "Administrador", "Docente (futuro)"]
    }
  },
  {
    id: "propuesta-valor",
    index: "02",
    area: "value",
    title: "Propuesta de valor",
    short: "Videojuego educativo 2D pixel art que enseña TI resolviendo casos reales.",
    icon: { type: "img", src: "./img/propuesta-valor.png", alt: "Icono de propuesta de valor" },
    chips: [
      { text: "Videojuego educativo 2D pixel art", tip: "Combina entretenimiento y aprendizaje en una experiencia interactiva." },
      { text: "Aprender TI resolviendo casos reales", tip: "Soporte, redes, hardware, programación, bases de datos y ciberseguridad." },
      { text: "Historia con Alex y el Saboteador", tip: "Aventura y simulación ambientada en NovaTech Solutions." },
      { text: "Ecosistema Unity + Flutter + PWA", tip: "El progreso se comparte entre el videojuego, la app móvil y la web." },
      { text: "Premium, sin publicidad", tip: "Compra única; sin microtransacciones obligatorias." }
    ],
    person: { img: "./img/alex.png", label: "Protagonista: Alex" },
    detail: {
      desc: "Un videojuego de aventura y simulación que combina el entretenimiento y el aprendizaje: cada problema representa una nueva misión y cada solución permite adquirir experiencia y avanzar en la historia.",
      metrics: [
        { value: "2D pixel art", label: "Estilo visual" },
        { value: "3 productos", label: "Experiencia integrada" },
        { value: "0", label: "Publicidad y microtransacciones" }
      ],
      sections: [
        {
          heading: "Qué ofrece",
          list: [
            "Aprender soporte técnico, redes, hardware, programación, bases de datos y ciberseguridad jugando.",
            "Historia de aventura y simulación con Alex y el Saboteador en NovaTech Solutions.",
            "Retos y minijuegos que desarrollan observación, análisis y toma de decisiones.",
            "Ecosistema integrado: videojuego en Unity, app móvil en Flutter y PWA en React.",
            "Modelo Premium sin publicidad ni pagos obligatorios durante la partida."
          ]
        },
        {
          heading: "Valor diferencial",
          list: [
            "Cada misión resuelta otorga experiencia y desbloquea nuevos desafíos.",
            "Progresión con niveles de dificultad creciente.",
            "No requiere conocimientos técnicos avanzados para comenzar."
          ]
        },
        {
          heading: "Ciclo de juego",
          list: [
            "Recibir un reporte → investigar el problema → analizar la situación → encontrar la solución → obtener experiencia → avanzar al siguiente desafío."
          ]
        }
      ],
      gallery: [
        { img: "./img/alex.png", caption: "Alex, protagonista" },
        { img: "./img/saboteador.png", caption: "El Saboteador, antagonista" }
      ],
      tags: ["Educativo", "2D pixel art", "Aventura", "Simulación", "Premium"]
    }
  },
  {
    id: "canales",
    index: "03",
    area: "channels",
    title: "Canales",
    short: "Cómo se distribuye y llega el producto a los usuarios.",
    icon: { type: "img", src: "./img/canales.png", alt: "Icono de canales" },
    chips: [
      { text: "Tiendas digitales (PC)", tip: "Distribución principal del videojuego completo." },
      { text: "App móvil Flutter (Android)", tip: "Aplicación complementaria de acompañamiento." },
      { text: "PWA instalable (Chrome/Edge)", tip: "Panel del jugador y de administración desde el navegador." },
      { text: "GitHub Pages", tip: "Publica el Canvas y la documentación del proyecto." },
      { text: "Licencias institucionales", tip: "Convenios con escuelas y universidades." }
    ],
    person: { img: "./img/martin.png", label: "Canal interno: Departamento de TI" },
    detail: {
      desc: "Los canales son los puntos de contacto por los que el videojuego y sus aplicaciones complementarias llegan a los usuarios y a las instituciones.",
      metrics: [
        { value: "PC", label: "Plataforma inicial" },
        { value: "Android", label: "Móvil (Flutter)" },
        { value: "Chrome / Edge", label: "PWA (React)" }
      ],
      sections: [
        {
          heading: "Distribución del videojuego",
          list: ["Tiendas digitales para PC, con demo gratuita de las primeras misiones."]
        },
        {
          heading: "Aplicaciones complementarias",
          list: [
            "App móvil en Flutter, con Android como prioridad.",
            "PWA en React instalable desde Chrome y Edge."
          ]
        },
        {
          heading: "Documentación y comunidad",
          list: [
            "GitHub Pages publica el Canvas interactivo y la documentación del proyecto.",
            "Convenios y licencias educativas con escuelas y universidades."
          ]
        }
      ],
      gallery: [
        { img: "./img/canales.png", caption: "Canal digital" },
        { img: "./img/elena.png", caption: "Canal institucional" }
      ],
      tags: ["Tiendas digitales", "Flutter", "PWA", "GitHub Pages", "Instituciones"]
    }
  },
  {
    id: "relaciones",
    index: "04",
    area: "relationships",
    title: "Relación con los usuarios",
    short: "Acompañamiento, retroalimentación y comunidad alrededor del juego.",
    icon: { type: "img", src: "./img/relaciones.png", alt: "Icono de relación con los usuarios" },
    chips: [
      { text: "Acompañamiento en móvil y PWA", tip: "Misiones, guías y progreso al alcance del jugador." },
      { text: "Retroalimentación en el juego", tip: "El juego evalúa acciones y muestra aciertos y errores." },
      { text: "Guías educativas", tip: "Apoyo sobre soporte, hardware y redes; guías públicas sin conexión." },
      { text: "Cuenta única compartida", tip: "Un mismo perfil en Unity, Flutter y React." },
      { text: "Actualizaciones y expansiones", tip: "Nuevas misiones del Saboteador y nuevas áreas." }
    ],
    person: { img: "./img/sofia.png", label: "Usuario: Sofía (primera misión)" },
    detail: {
      desc: "La relación con los usuarios se sostiene mediante el acompañamiento diario en la app móvil y la PWA, la retroalimentación dentro del videojuego y los materiales educativos.",
      metrics: [
        { value: "1 cuenta", label: "Compartida en 3 apps" },
        { value: "+100 XP", label: "Recompensa nivel 1" },
        { value: "Sin conexión", label: "Guías públicas (PWA)" }
      ],
      sections: [
        {
          heading: "Acompañamiento",
          list: [
            "La app móvil y la PWA muestran misiones, guías y progreso del jugador.",
            "Una misma cuenta permite consultar el avance desde cualquier aplicación."
          ]
        },
        {
          heading: "Retroalimentación",
          list: [
            "El videojuego evalúa las acciones del jugador y muestra aciertos y errores.",
            "Al completar misiones se otorgan experiencia (XP) y reputación."
          ]
        },
        {
          heading: "Soporte educativo y comunidad",
          list: [
            "Guías de soporte técnico, hardware y redes consultables.",
            "Las guías públicas se abren sin conexión en la PWA.",
            "Actualizaciones, expansiones y nuevas misiones del Saboteador."
          ]
        }
      ],
      gallery: [
        { img: "./img/sofia.png", caption: "Sofía" },
        { img: "./img/richar.png", caption: "Richar" }
      ],
      tags: ["Acompañamiento", "Retroalimentación", "Guías", "Cuenta única"]
    }
  },
  {
    id: "ingresos",
    index: "05",
    area: "revenue",
    title: "Fuentes de ingresos / sostenibilidad",
    short: "Modelo Premium de compra única y licencias educativas.",
    icon: {
      type: "svg",
      markup:
        '<svg viewBox="0 0 32 32" role="img" aria-hidden="true">' +
        '<rect x="3" y="18" width="6" height="11" fill="#15803d"/>' +
        '<rect x="12" y="12" width="6" height="17" fill="#22c55e"/>' +
        '<rect x="21" y="5" width="6" height="24" fill="#4ade80"/>' +
        '<path d="M4 13 L13 5 L20 9 L29 1" fill="none" stroke="#f59e0b" stroke-width="2"/>' +
        "</svg>"
    },
    chips: [
      { text: "Venta única Premium", tip: "Precio inicial tentativo: $99–$119 MXN." },
      { text: "Licencias educativas", tip: "Instituciones que compran varias licencias para sus estudiantes." },
      { text: "Expansiones / DLC opcionales", tip: "Contenido adicional de ciberseguridad, servidores, redes y hardware." },
      { text: "Demo gratuita", tip: "Incluye las primeras misiones." },
      { text: "MVP académico sin cobros", tip: "La primera entrega no implementa pagos ni licencias comerciales." }
    ],
    person: { img: "./img/marco.png", label: "Valor de desarrollo: Marco" },
    detail: {
      desc: "Se propone un modelo Premium de compra única: el jugador compra el videojuego una vez y accede a la historia, las misiones y los contenidos educativos sin publicidad ni pagos obligatorios.",
      metrics: [
        { value: "$99–$119 MXN", label: "Precio tentativo" },
        { value: "Compra única", label: "Modelo Premium" },
        { value: "Gratis", label: "Demo inicial" }
      ],
      sections: [
        {
          heading: "Ingresos previstos",
          list: [
            "Venta única del videojuego completo (modelo Premium) en PC.",
            "Versión educativa con licencias para escuelas, universidades y centros de capacitación.",
            "Expansiones o DLC opcionales."
          ]
        },
        {
          heading: "Contenido adicional",
          list: [
            "Ciberseguridad avanzada, administración de servidores y redes empresariales.",
            "Reparación avanzada de hardware, nuevas misiones del Saboteador y nuevas áreas de NovaTech Solutions."
          ]
        },
        {
          heading: "Sostenibilidad académica",
          list: [
            "El MVP académico no implementará cobros, licencias comerciales ni pagos en las aplicaciones complementarias."
          ]
        }
      ],
      gallery: [
        { img: "./img/marco.png", caption: "Desarrollo" },
        { img: "./img/costos.png", caption: "Costos" }
      ],
      tags: ["Premium", "$99–$119 MXN", "Licencias", "DLC", "Demo gratis"]
    }
  },
  {
    id: "recursos",
    index: "06",
    area: "resources",
    title: "Recursos clave",
    short: "Tecnologías, contenido y personas que hacen posible el proyecto.",
    icon: { type: "img", src: "./img/recursos.png", alt: "Icono de recursos clave" },
    chips: [
      { text: "Unity + assets pixel art", tip: "Motor y arte 2D del videojuego." },
      { text: "Flutter y React / PWA", tip: "Aplicaciones complementarias." },
      { text: "API REST + MongoDB + Keycloak", tip: "Servicios compartidos, datos y autenticación." },
      { text: "Docker + Grafana", tip: "Despliegue y monitoreo." },
      { text: "Git, GitHub y GitHub Pages", tip: "Control de versiones y publicación." }
    ],
    person: { img: "./img/richar.png", label: "Infraestructura: Richar" },
    detail: {
      desc: "Los recursos clave son los activos tecnológicos, de contenido y humanos necesarios para construir y mantener todo el ecosistema.",
      metrics: [
        { value: "11", label: "Tecnologías base" },
        { value: "1", label: "Responsable único" },
        { value: "MongoDB", label: "Base de datos" }
      ],
      sections: [
        {
          heading: "Tecnológicos",
          list: [
            "Unity y C# para el videojuego.",
            "Flutter y Dart para la app móvil.",
            "React y PWA para la aplicación web.",
            "API REST (Node.js, Express y Mongoose como propuesta).",
            "MongoDB como base de datos.",
            "Keycloak para autenticación.",
            "Docker y Grafana para despliegue y monitoreo.",
            "Git, GitHub y GitHub Pages."
          ]
        },
        {
          heading: "De contenido",
          list: [
            "Assets pixel art, hojas de sprite y storyboards.",
            "Diseño de misiones, niveles e historia."
          ]
        },
        {
          heading: "Humanos",
          list: ["Responsable único del proyecto: Brian Jesús Mendoza Márquez."]
        }
      ],
      gallery: [
        { img: "./img/recursos.png", caption: "Servidores y datos" },
        { img: "./img/richar.png", caption: "Infraestructura de red" }
      ],
      tags: ["Unity", "Flutter", "React/PWA", "API REST", "MongoDB", "Keycloak", "Docker", "Grafana", "Git"]
    }
  },
  {
    id: "actividades",
    index: "07",
    area: "activities",
    title: "Actividades clave",
    short: "Desarrollo, diseño, calidad y publicación del proyecto.",
    icon: { type: "img", src: "./img/actividades.png", alt: "Icono de actividades clave" },
    chips: [
      { text: "Desarrollo del videojuego en Unity", tip: "Escenas, diálogos, movimiento y misiones." },
      { text: "Desarrollo móvil y web", tip: "Pantallas en Flutter y PWA en React." },
      { text: "Diseño de niveles e historia", tip: "Definir requisitos, alcance e historia." },
      { text: "API, seguridad y datos", tip: "Implementar servicios, autorización y almacenamiento." },
      { text: "Pruebas, documentación y publicación", tip: "Verificar, documentar y publicar en GitHub Pages." }
    ],
    person: { img: "./img/marco.png", label: "Desarrollo de software: Marco" },
    detail: {
      desc: "Estas son las actividades esenciales para crear, mantener y publicar el videojuego y sus aplicaciones complementarias.",
      metrics: [
        { value: "6", label: "Etapas del plan" },
        { value: "3", label: "Clientes a integrar" },
        { value: "RF-01 a RF-21", label: "Alcance inicial" }
      ],
      sections: [
        {
          heading: "Desarrollo",
          list: [
            "Programar escenas, interacciones, diálogos y retos en Unity.",
            "Construir las pantallas de la app Flutter y de la PWA en React.",
            "Implementar la API REST, la autorización y el almacenamiento en MongoDB."
          ]
        },
        {
          heading: "Diseño",
          list: [
            "Definir requisitos, alcance, roles, historia, misiones y pantallas.",
            "Diseñar niveles y minijuegos con dificultad creciente."
          ]
        },
        {
          heading: "Calidad y entrega",
          list: [
            "Probar el flujo completo de las tres aplicaciones.",
            "Documentar y preparar evidencias y presentación.",
            "Publicar la documentación en GitHub Pages."
          ]
        }
      ],
      gallery: [
        { img: "./img/marco.png", caption: "Programación" },
        { img: "./img/martin.png", caption: "Supervisión de TI" }
      ],
      tags: ["Unity", "Flutter", "React/PWA", "API", "Pruebas", "Documentación"]
    }
  },
  {
    id: "socios",
    index: "08",
    area: "partners",
    title: "Socios clave",
    short: "Aliados para distribución, infraestructura y educación.",
    icon: { type: "img", src: "./img/socios.png", alt: "Icono de socios clave" },
    chips: [
      { text: "Instituciones educativas", tip: "Escuelas y universidades mediante convenios y licencias." },
      { text: "Tiendas digitales", tip: "Distribución y venta del videojuego en PC." },
      { text: "Alojamiento y nube", tip: "Infraestructura para la API, MongoDB y el monitoreo." },
      { text: "Comunidad de jugadores", tip: "Retroalimentación sobre misiones y contenido." },
      { text: "Herramientas de código abierto", tip: "Unity, Flutter, React, Keycloak, MongoDB, Docker, Grafana, Git y GitHub." }
    ],
    person: { img: "./img/martin.png", label: "NovaTech Solutions" },
    detail: {
      desc: "Los socios clave aportan distribución, infraestructura, respaldo educativo y tecnología para sostener el proyecto.",
      metrics: [
        { value: "Instituciones", label: "Respaldo educativo" },
        { value: "Nube", label: "Infraestructura" },
        { value: "Open source", label: "Base tecnológica" }
      ],
      sections: [
        {
          heading: "Educación",
          list: ["Instituciones educativas para licencias y convenios de uso en clase."]
        },
        {
          heading: "Distribución",
          list: ["Tiendas digitales para la venta del videojuego en PC."]
        },
        {
          heading: "Infraestructura",
          list: ["Proveedor de alojamiento y servicios en la nube para la API, MongoDB y el monitoreo con Grafana."]
        },
        {
          heading: "Tecnología y comunidad",
          list: [
            "Herramientas de código abierto: Unity, Flutter, React, Keycloak, MongoDB, Docker, Grafana, Git y GitHub.",
            "Comunidad de jugadores y estudiantes que aporta retroalimentación."
          ]
        }
      ],
      gallery: [
        { img: "./img/martin.png", caption: "NovaTech Solutions" },
        { img: "./img/socios.png", caption: "Alianzas" }
      ],
      tags: ["Instituciones", "Tiendas digitales", "Hosting", "Comunidad", "Open source"]
    }
  },
  {
    id: "costos",
    index: "09",
    area: "costs",
    title: "Estructura de costos",
    short: "Desarrollo, infraestructura, contenido y mantenimiento.",
    icon: { type: "img", src: "./img/costos.png", alt: "Icono de estructura de costos" },
    chips: [
      { text: "Tiempo de desarrollo (responsable único)", tip: "Análisis, diseño, programación, pruebas y documentación." },
      { text: "Licencias y herramientas", tip: "Herramientas de desarrollo y licencias necesarias." },
      { text: "Hosting, API y MongoDB", tip: "Infraestructura para los servicios compartidos." },
      { text: "Monitoreo con Grafana", tip: "Supervisión del funcionamiento de los servicios." },
      { text: "Diseño, sonido y publicación", tip: "Assets gráficos, sonido y publicación en tiendas." }
    ],
    person: { img: "./img/richar.png", label: "Operación: infraestructura" },
    detail: {
      desc: "Los costos se concentran en el tiempo de desarrollo individual y en la infraestructura para los servicios compartidos y la publicación.",
      metrics: [
        { value: "1", label: "Responsable de desarrollo" },
        { value: "Sin publicidad", label: "Costo de marketing" },
        { value: "MVP sin cobros", label: "Costo de pasarelas de pago" }
      ],
      sections: [
        {
          heading: "Desarrollo",
          list: ["Tiempo del responsable único en análisis, diseño, programación, pruebas y documentación."]
        },
        {
          heading: "Infraestructura",
          list: [
            "Hosting de la API REST y de la base de datos MongoDB.",
            "Monitoreo con Grafana.",
            "Registro de dominio y publicación."
          ]
        },
        {
          heading: "Contenido y herramientas",
          list: [
            "Assets gráficos y sonido.",
            "Licencias y herramientas de desarrollo."
          ]
        },
        {
          heading: "Operación",
          list: ["Mantenimiento, actualizaciones y publicación en tiendas digitales."]
        }
      ],
      gallery: [
        { img: "./img/costos.png", caption: "Costos" },
        { img: "./img/richar.png", caption: "Infraestructura" }
      ],
      tags: ["Desarrollo", "Hosting", "MongoDB", "Grafana", "Mantenimiento"]
    }
  }
];

/* ------------------------------------------------------------------ *
 *  CONEXIONES VISUALES entre componentes (from, to, color)
 * ------------------------------------------------------------------ */
const CONNECTIONS = [
  ["propuesta-valor", "segmentos", "#06b6d4"],
  ["propuesta-valor", "canales", "#2563eb"],
  ["propuesta-valor", "relaciones", "#b91c1c"],
  ["propuesta-valor", "ingresos", "#15803d"],
  ["propuesta-valor", "recursos", "#22d3ee"],
  ["recursos", "actividades", "#a855f7"],
  ["actividades", "socios", "#38bdf8"],
  ["recursos", "costos", "#fb923c"],
  ["socios", "costos", "#fb923c"],
  ["canales", "segmentos", "#2563eb"],
  ["relaciones", "segmentos", "#b91c1c"],
  ["actividades", "canales", "#a855f7"],
  ["socios", "recursos", "#38bdf8"],
  ["ingresos", "socios", "#15803d"]
];

/* ------------------------------------------------------------------ *
 *  PERSONAJES
 * ------------------------------------------------------------------ */
const CHARACTERS = [
  { name: "Alex", role: "Protagonista · recién egresado", img: "./img/alex.png" },
  { name: "Martín", role: "Jefe del Departamento de TI", img: "./img/martin.png" },
  { name: "Richar", role: "Especialista en redes", img: "./img/richar.png" },
  { name: "Sofía", role: "Contabilidad · primera misión", img: "./img/sofia.png" },
  { name: "Marco", role: "Desarrollador de software", img: "./img/marco.png" },
  { name: "Elena", role: "Docente / evaluadora", img: "./img/elena.png" },
  { name: "Estudiante", role: "Jugador del videojuego", img: "./img/estudiante.png" },
  { name: "Saboteador", role: "Antagonista principal", img: "./img/saboteador.png" }
];

/* ------------------------------------------------------------------ *
 *  TECNOLOGÍAS Y ARQUITECTURA
 * ------------------------------------------------------------------ */
const TECH = [
  "Unity", "Flutter", "React / PWA", "API REST", "Keycloak",
  "MongoDB", "Docker", "Grafana", "Git", "GitHub", "GitHub Pages"
];

const TECH_ARCH = [
  { title: "Videojuego (Unity · C#)", items: ["Escenas, diálogos y retos", "Movimiento de Alex y misiones", "Envía los resultados a la API"] },
  { title: "App móvil (Flutter · Dart)", items: ["Inicio de sesión", "Misiones, guías y progreso", "Android como prioridad"] },
  { title: "PWA (React)", items: ["Panel del jugador", "Administración de contenido", "Manifiesto y service worker"] },
  { title: "Servicios y datos", items: ["API REST", "MongoDB", "Keycloak", "Docker", "Grafana"] }
];

/* ================================================================== *
 *  UTILIDADES
 * ================================================================== */
function el(tag, attrs, children) {
  const node = document.createElement(tag);
  if (attrs) {
    Object.keys(attrs).forEach(function (key) {
      if (key === "class") node.className = attrs[key];
      else if (key === "html") node.innerHTML = attrs[key];
      else if (key === "text") node.textContent = attrs[key];
      else if (key.indexOf("on") === 0 && typeof attrs[key] === "function") {
        node.addEventListener(key.slice(2).toLowerCase(), attrs[key]);
      } else {
        node.setAttribute(key, attrs[key]);
      }
    });
  }
  if (children) {
    (Array.isArray(children) ? children : [children]).forEach(function (child) {
      if (child === null || child === undefined) return;
      node.appendChild(typeof child === "string" ? document.createTextNode(child) : child);
    });
  }
  return node;
}

function iconNode(icon) {
  const span = el("span", { "aria-hidden": "true" });
  if (icon.type === "svg") span.innerHTML = icon.markup;
  else span.appendChild(el("img", { src: icon.src, alt: icon.alt || "", loading: "lazy" }));
  return span;
}

/* ================================================================== *
 *  RENDER
 * ================================================================== */
const bmc = document.getElementById("bmc");
const bmcLinks = document.getElementById("bmc-links");
const blockEls = {};
const linkEls = [];

function renderCanvas() {
  CANVAS_DATA.forEach(function (comp, i) {
    const cards = comp.chips.map(function (chip) {
      return el("li", { class: "chip", "data-tip": chip.tip, tabindex: "0" }, chip.text);
    });

    const person = comp.person
      ? el("div", { class: "block-person" }, [
          el("img", { src: comp.person.img, alt: "", loading: "lazy" }),
          el("span", { text: comp.person.label })
        ])
      : null;

    const expandBtn = el("button", {
      class: "block-expand",
      type: "button",
      "aria-label": "Abrir ficha: " + comp.title,
      onclick: function (e) { e.stopPropagation(); openModal(comp.id); },
      onfocus: function () { highlightConnections(comp.id, true); },
      onblur: function () { highlightConnections(comp.id, false); },
      text: "+"
    });

    const block = el(
      "article",
      {
        class: "bmc-block area-" + comp.area,
        "data-component": comp.id,
        onclick: function () { openModal(comp.id); },
        onmouseenter: function () { highlightConnections(comp.id, true); },
        onmouseleave: function () { highlightConnections(comp.id, false); }
      },
      [
        el("header", { class: "block-head" }, [
          el("span", { class: "block-icon" }, iconNode(comp.icon)),
          el("div", { class: "block-title-wrap" }, [
            el("span", { class: "block-index", text: comp.index }),
            el("h3", { class: "block-title", text: comp.title })
          ]),
          expandBtn
        ]),
        el("ul", { class: "block-cards" }, cards),
        person
      ]
    );

    block.style.animationDelay = i * 60 + "ms";
    bmc.appendChild(block);
    blockEls[comp.id] = block;
  });
}

/* ---------- Conexiones visuales (SVG) ---------- */
function clearLinks() {
  bmcLinks.innerHTML = "";
  linkEls.length = 0;
}

function drawConnections() {
  clearLinks();
  if (window.innerWidth <= 1024) return;

  const rect = bmc.getBoundingClientRect();
  const w = Math.round(rect.width);
  const h = Math.round(rect.height);
  if (w === 0 || h === 0) return;
  bmcLinks.setAttribute("viewBox", "0 0 " + w + " " + h);

  CONNECTIONS.forEach(function (conn) {
    const from = blockEls[conn[0]];
    const to = blockEls[conn[1]];
    if (!from || !to) return;

    const a = center(from, rect);
    const b = center(to, rect);
    const dx = b.x - a.x;
    const dy = b.y - a.y;
    const len = Math.hypot(dx, dy) || 1;
    const off = Math.min(46, len * 0.16);
    const cx = (a.x + b.x) / 2 + (-dy / len) * off;
    const cy = (a.y + b.y) / 2 + (dx / len) * off;

    const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
    path.setAttribute("d", "M " + a.x + " " + a.y + " Q " + cx + " " + cy + " " + b.x + " " + b.y);
    path.setAttribute("stroke", conn[2]);
    path.setAttribute("data-from", conn[0]);
    path.setAttribute("data-to", conn[1]);
    bmcLinks.appendChild(path);
    linkEls.push({ node: path, from: conn[0], to: conn[1] });
  });
}

function center(node, containerRect) {
  const r = node.getBoundingClientRect();
  return {
    x: r.left - containerRect.left + r.width / 2,
    y: r.top - containerRect.top + r.height / 2
  };
}

function highlightConnections(id, on) {
  linkEls.forEach(function (link) {
    if (link.from === id || link.to === id) {
      link.node.classList.toggle("hl", on);
    }
  });
}

/* ---------- Animación de entrada ---------- */
function observeBlocks() {
  if (!("IntersectionObserver" in window)) {
    Object.keys(blockEls).forEach(function (id) { blockEls[id].classList.add("is-visible"); });
    return;
  }
  const io = new IntersectionObserver(
    function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          entry.target.classList.add("is-visible");
          io.unobserve(entry.target);
        }
      });
    },
    { threshold: 0.12 }
  );
  Object.keys(blockEls).forEach(function (id) { io.observe(blockEls[id]); });
}

/* ================================================================== *
 *  MODAL
 * ================================================================== */
const modal = document.getElementById("modal");
const modalIcon = document.getElementById("modal-icon");
const modalIndex = document.getElementById("modal-index");
const modalTitle = document.getElementById("modal-title");
const modalBody = document.getElementById("modal-body");
let lastFocused = null;

function openModal(id) {
  const comp = CANVAS_DATA.find(function (c) { return c.id === id; });
  if (!comp) return;

  lastFocused = document.activeElement;
  modalIcon.innerHTML = "";
  modalIcon.appendChild(iconNode(comp.icon));
  modalIndex.textContent = "COMPONENTE " + comp.index + " / 09";
  modalTitle.textContent = comp.title;

  const body = [];
  body.push(el("p", { class: "modal-desc", text: comp.detail.desc }));

  if (comp.detail.metrics) {
    const grid = el("div", { class: "modal-metrics" });
    comp.detail.metrics.forEach(function (m) {
      grid.appendChild(el("div", { class: "metric" }, [
        el("strong", { text: m.value }),
        el("span", { text: m.label })
      ]));
    });
    body.push(grid);
  }

  comp.detail.sections.forEach(function (section) {
    body.push(el("h3", { text: section.heading }));
    body.push(
      el("ul", { class: "modal-list" }, section.list.map(function (item) {
        return el("li", { text: item });
      }))
    );
  });

  if (comp.detail.gallery && comp.detail.gallery.length) {
    body.push(el("h3", { text: "Imágenes relacionadas" }));
    const gallery = el("div", { class: "modal-gallery" });
    comp.detail.gallery.forEach(function (g) {
      gallery.appendChild(el("figure", null, [
        el("img", { src: g.img, alt: g.caption, loading: "lazy" }),
        el("figcaption", { text: g.caption })
      ]));
    });
    body.push(gallery);
  }

  if (comp.detail.tags && comp.detail.tags.length) {
    const tags = el("div", { class: "modal-tags" });
    comp.detail.tags.forEach(function (t) { tags.appendChild(el("span", { text: t })); });
    body.push(tags);
  }

  modalBody.innerHTML = "";
  body.forEach(function (n) { modalBody.appendChild(n); });

  modal.classList.add("is-open");
  modal.setAttribute("aria-hidden", "false");
  document.body.style.overflow = "hidden";
  const closeBtn = modal.querySelector(".modal-close");
  if (closeBtn) closeBtn.focus();
}

function closeModal() {
  modal.classList.remove("is-open");
  modal.setAttribute("aria-hidden", "true");
  document.body.style.overflow = "";
  if (lastFocused && typeof lastFocused.focus === "function") lastFocused.focus();
}

modal.addEventListener("click", function (e) {
  if (e.target.hasAttribute && e.target.hasAttribute("data-close-modal")) closeModal();
});

document.addEventListener("keydown", function (e) {
  if (e.key === "Escape" && modal.classList.contains("is-open")) closeModal();
  if (e.key === "Tab" && modal.classList.contains("is-open")) {
    const focusables = modal.querySelectorAll("button, [href], img, [tabindex]:not([tabindex='-1'])");
    if (!focusables.length) return;
    const first = focusables[0];
    const last = focusables[focusables.length - 1];
    if (e.shiftKey && document.activeElement === first) {
      e.preventDefault();
      last.focus();
    } else if (!e.shiftKey && document.activeElement === last) {
      e.preventDefault();
      first.focus();
    }
  }
});

/* ================================================================== *
 *  TOOLTIP
 * ================================================================== */
const tooltip = document.getElementById("tooltip");

function showTooltip(target, text) {
  tooltip.textContent = text;
  tooltip.classList.add("is-visible");
  tooltip.setAttribute("aria-hidden", "false");
  positionTooltip(target);
}

function positionTooltip(target) {
  const r = target.getBoundingClientRect();
  const tw = tooltip.offsetWidth;
  const th = tooltip.offsetHeight;
  let left = r.left;
  let top = r.bottom + 10;
  if (left + tw > window.innerWidth - 10) left = window.innerWidth - tw - 10;
  if (left < 10) left = 10;
  if (top + th > window.innerHeight - 10) top = r.top - th - 10;
  tooltip.style.left = left + "px";
  tooltip.style.top = top + "px";
}

function hideTooltip() {
  tooltip.classList.remove("is-visible");
  tooltip.setAttribute("aria-hidden", "true");
}

document.addEventListener("mouseover", function (e) {
  const t = e.target.closest && e.target.closest("[data-tip]");
  if (t) showTooltip(t, t.getAttribute("data-tip"));
});
document.addEventListener("mouseout", function (e) {
  const t = e.target.closest && e.target.closest("[data-tip]");
  if (t) hideTooltip();
});
document.addEventListener("focusin", function (e) {
  const t = e.target.closest && e.target.closest("[data-tip]");
  if (t) showTooltip(t, t.getAttribute("data-tip"));
});
document.addEventListener("focusout", hideTooltip);
window.addEventListener("scroll", hideTooltip, true);

/* ================================================================== *
 *  SECCIONES AUXILIARES
 * ================================================================== */
function renderBadges() {
  const list = document.getElementById("tech-badges");
  TECH.forEach(function (name) { list.appendChild(el("li", { text: name })); });
}

function renderCharacters() {
  const grid = document.getElementById("characters-grid");
  CHARACTERS.forEach(function (c) {
    grid.appendChild(
      el("li", { class: "character-card" }, [
        el("div", { class: "char-img" }, el("img", { src: c.img, alt: c.name + ", " + c.role, loading: "lazy" })),
        el("h3", { text: c.name }),
        el("p", { text: c.role })
      ])
    );
  });
}

function renderArchitecture() {
  const wrap = document.getElementById("tech-architecture");
  TECH_ARCH.forEach(function (node) {
    wrap.appendChild(
      el("article", { class: "tech-node" }, [
        el("h3", { text: node.title }),
        el("ul", null, node.items.map(function (it) { return el("li", { text: it }); }))
      ])
    );
  });
}

/* ================================================================== *
 *  INICIALIZACIÓN
 * ================================================================== */
function init() {
  renderBadges();
  renderCanvas();
  renderCharacters();
  renderArchitecture();
  observeBlocks();
  drawConnections();

  window.addEventListener("load", drawConnections);

  let resizeTimer = null;
  window.addEventListener("resize", function () {
    window.clearTimeout(resizeTimer);
    resizeTimer = window.setTimeout(drawConnections, 150);
  });
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", init);
} else {
  init();
}
