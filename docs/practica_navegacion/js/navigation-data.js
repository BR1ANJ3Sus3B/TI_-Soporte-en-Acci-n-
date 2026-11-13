/* Inventario de navegación. Rutas relativas publicables desde esta carpeta. */
const IMG = '../imagenes/';
const sketch = {
  P01:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_08 p.m-1.png', P02:'skeches/01-publico-objetivo.png',
  P03:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_11 p.m-3.png', P04:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_13 p.m-4.png',
  P05:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_14 p.m-5.png', P06:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_16 p.m-6.png',
  P07:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_18 p.m-7.png', P08:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_19 p.m-8.png', P09:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_22 p.m-9.png', P10:'skeches/Imagen de ChatGPT 3 nov 2026, 07_47_23 p.m-10.png',
  P11:'skeches/11_dashboard_inicio.png',P12:'skeches/12_mapa_novatech.png',P13:'skeches/13_detalle_mision.png',P14:'skeches/14_juego_mision.png',P15:'skeches/15_dialogo_historia.png',P16:'skeches/16_inventario.png',P17:'skeches/17_progreso.png',P18:'skeches/18_logros_insignias.png',P19:'skeches/19_configuracion.png',P20:'skeches/20_perfil_usuario.png'
};
const wireNames = ['Imagen de ChatGPT 4 nov 2026, 08_02_45 a.m-1.png','Imagen de ChatGPT 4 nov 2026, 08_02_46 a.m-2.png','Imagen de ChatGPT 4 nov 2026, 08_02_46 a.m-3.png','Imagen de ChatGPT 4 nov 2026, 08_02_47 a.m-4.png','Imagen de ChatGPT 4 nov 2026, 08_02_48 a.m-5.png','Imagen de ChatGPT 4 nov 2026, 08_02_48 a.m-6.png','Imagen de ChatGPT 4 nov 2026, 08_02_49 a.m-7.png','Imagen de ChatGPT 4 nov 2026, 08_02_50 a.m-8.png','Imagen de ChatGPT 4 nov 2026, 08_02_51 a.m-9.png','Imagen de ChatGPT 4 nov 2026, 08_02_51 a.m-10.png','Imagen de ChatGPT 4 nov 2026, 08_14_16 a.m-1.png','Imagen de ChatGPT 4 nov 2026, 08_14_17 a.m-2.png','Imagen de ChatGPT 4 nov 2026, 08_14_19 a.m-3.png','Imagen de ChatGPT 4 nov 2026, 08_14_20 a.m-4.png','Imagen de ChatGPT 4 nov 2026, 08_14_21 a.m-5.png','Imagen de ChatGPT 4 nov 2026, 08_14_23 a.m-6.png','Imagen de ChatGPT 4 nov 2026, 08_14_24 a.m-7.png','Imagen de ChatGPT 4 nov 2026, 08_14_25 a.m-8.png','Imagen de ChatGPT 4 nov 2026, 08_14_27 a.m-9.png','Imagen de ChatGPT 4 nov 2026, 08_14_28 a.m-10.png'];
const rows = [
 ['P01','Splash Screen','Inicialización','Presentación inicial del software',['Flutter','PWA'],['Público'],['Inicialización automática'],['P02'],'RF-01'],
 ['P02','Loading Screen','Inicialización','Carga de recursos y comprobación de sesión.',['Flutter','PWA'],['Público'],['Cargar recursos'],['P03'],'RF-01'],
 ['P03','Login Screen','Autenticación','Permite iniciar sesión y presenta opciones sociales propuestas.',['Flutter','PWA'],['Público'],['Iniciar sesión','Recuperar contraseña','Registrarse','Facebook (propuesto)','Google (propuesto)','Instagram (propuesto)'],['P04','P05','P06','P10'],'RF-02, RF-05'],
 ['P04','Registro','Autenticación','Creación de una cuenta de jugador.',['Flutter','PWA'],['Público'],['Registrarme'],['P03'],'RF-03, RF-20'],
 ['P05','Recovery Account','Seguridad','Solicita un enlace de recuperación.',['Flutter','PWA'],['Público'],['Enviar enlace'],['P03'],'RF-04'],
 ['P06','Lock Account','Seguridad','Comunica bloqueo por intentos fallidos.',['Flutter','PWA'],['Público'],['Solicitar desbloqueo'],['P07','P03'],'RF-04, RF-05'],
 ['P07','Unlock Account Code','Seguridad','Valida el código de desbloqueo.',['Flutter','PWA'],['Público'],['Desbloquear'],['P03'],'RF-04'],
 ['P08','About Software','Información general','Describe el propósito de TI: Soporte en Acción.',['Flutter','PWA'],['Público'],['Volver'],['P03','P10'],'Pendiente de relación específica'],
 ['P09','About Team / Enterprise','Información general','Presenta el equipo y la empresa ficticia NovaTech Solutions.',['Flutter','PWA'],['Público'],['Volver'],['P03','P10'],'Pendiente de relación específica'],
 ['P10','Home','Inicio','Punto de entrada a módulos del jugador.',['Flutter','PWA'],['Jugador / Estudiante'],['Abrir módulos','Cerrar sesión'],['P11','P12','P16','P17','P20','P08','P09','P03'],'RF-05'],
 ['P11','Dashboard','Inicio','Resumen de información, accesos y métricas personales.',['Flutter','PWA'],['Jugador / Estudiante'],['Consultar resumen'],['P10','P12','P16','P17'],'RF-12'],
 ['P12','Mapa de NovaTech','Misiones','Consulta de zonas y misiones disponibles.',['Flutter','PWA'],['Jugador / Estudiante'],['Seleccionar misión'],['P10','P13'],'RF-06'],
 ['P13','Detalle de misión','Misiones','Muestra objetivos y recompensa de la misión.',['Flutter','PWA'],['Jugador / Estudiante'],['Comenzar misión'],['P12','P14'],'RF-06, RF-07'],
 ['P14','Juego / Misión','Misiones','Representación de la misión jugable.',['Unity'],['Jugador / Estudiante'],['Jugar','Pausar'],['P13','P15','P17'],'RF-07'],
 ['P15','Diálogo / Historia','Misiones','Diálogos e instrucciones narrativas de la misión.',['Unity'],['Jugador / Estudiante'],['Continuar'],['P14','P17'],'RF-08'],
 ['P16','Inventario / Herramientas','Gamificación','Consulta de herramientas disponibles.',['Unity','Flutter','PWA'],['Jugador / Estudiante'],['Consultar herramientas'],['P10','P11'],'RF-13'],
 ['P17','Progreso del jugador','Gamificación','Muestra experiencia, nivel y estadísticas.',['Unity','Flutter','PWA'],['Jugador / Estudiante','Docente / Evaluador'],['Consultar progreso'],['P10','P11','P18'],'RF-14'],
 ['P18','Logros / Insignias','Gamificación','Consulta de reconocimientos obtenidos.',['Unity','Flutter','PWA'],['Jugador / Estudiante'],['Consultar logros'],['P17','P10'],'RF-15'],
 ['P19','Configuración','Configuración','Preferencias del sistema.',['Flutter','PWA'],['Jugador / Estudiante'],['Guardar preferencias'],['P20','P10'],'RF-11'],
 ['P20','Perfil de usuario','Perfil','Información básica del usuario y accesos de cuenta.',['Flutter','PWA'],['Jugador / Estudiante'],['Editar perfil','Abrir configuración'],['P10','P19'],'RF-10']
];
const screens = rows.map((r,i)=>({id:r[0],nombre:r[1],modulo:r[2],descripcion:r[3],plataforma:r[4],roles:r[5],acciones:r[6],conexiones:r[7],requerimientos:r[8],sketch:IMG+sketch[r[0]],wireframe:IMG+'wirefremes/'+wireNames[i],elementos:['Encabezado o identidad','Contenido principal','Acciones de navegación'],anterior:rows.filter(x=>x[7].includes(r[0])).map(x=>x[0])}));
const roles = ['Público','Jugador / Estudiante','Docente / Evaluador','Editor de contenido','Administrador','Institución educativa'];
