// Manifiesto público autocontenido: 20 pares seleccionados; no copia ni altera los originales.
const imageManifest = [
 ['P01','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_08 p.m-1.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_45 a.m-1.png'],
 ['P02','../imagenes/skeches/01-publico-objetivo.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_46 a.m-2.png'],
 ['P03','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_11 p.m-3.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_46 a.m-3.png'],
 ['P04','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_13 p.m-4.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_47 a.m-4.png'],
 ['P05','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_14 p.m-5.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_48 a.m-5.png'],
 ['P06','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_16 p.m-6.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_48 a.m-6.png'],
 ['P07','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_18 p.m-7.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_49 a.m-7.png'],
 ['P08','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_19 p.m-8.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_50 a.m-8.png'],
 ['P09','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_22 p.m-9.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_51 a.m-9.png'],
 ['P10','../imagenes/skeches/Imagen de ChatGPT 3 nov 2026, 07_47_23 p.m-10.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_02_51 a.m-10.png'],
 ['P11','../imagenes/skeches/11_dashboard_inicio.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_16 a.m-1.png'],['P12','../imagenes/skeches/12_mapa_novatech.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_17 a.m-2.png'],['P13','../imagenes/skeches/13_detalle_mision.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_19 a.m-3.png'],['P14','../imagenes/skeches/14_juego_mision.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_20 a.m-4.png'],['P15','../imagenes/skeches/15_dialogo_historia.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_21 a.m-5.png'],['P16','../imagenes/skeches/16_inventario.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_23 a.m-6.png'],['P17','../imagenes/skeches/17_progreso.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_24 a.m-7.png'],['P18','../imagenes/skeches/18_logros_insignias.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_25 a.m-8.png'],['P19','../imagenes/skeches/19_configuracion.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_27 a.m-9.png'],['P20','../imagenes/skeches/20_perfil_usuario.png','../imagenes/wirefremes/Imagen de ChatGPT 4 nov 2026, 08_14_28 a.m-10.png']
].map(([id,sketch,wireframe])=>({id,sketch,wireframe}));
// Los 15 sketches por rol no tienen wireframe tecnico localizado.
imageManifest.push(...roleScreens.map(({id,sketch,wireframe})=>({id,sketch,wireframe})));
