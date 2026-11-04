# Validación registrada

| Comprobación | Resultado |
|---|---|
| Carpetas nuevas localizadas | Correcto: `docs/imagenes/20_Pantallas_generales/` y `docs/imagenes/18_Pantallas_por_rol/` |
| Conteo de comparaciones | Correcto: 20 generales + 18 por rol = 38 |
| Subcarpetas por rol | Correcto: seis, con tres PNG cada una |
| IDs del manifiesto | Correcto: P01–P20, JUG01–JUG03, ADM01–ADM03, DOC01–DOC03, EVA01–EVA03, EDI01–EDI03 y SOP01–SOP03 |
| Archivos repetidos | No se detectaron hashes repetidos entre las imágenes inspeccionadas |
| Rutas para GitHub Pages | Rutas relativas desde `docs/practica_navegacion/`; sin rutas de Windows |
| Recorte de comparaciones | El visor aplica `object-fit: contain`; no divide ni usa `cover` |
| Visor | Implementado: ampliar, reducir, ajustar, pantalla completa, descargar, anterior, siguiente y cerrar |
| Filtros, diagramas y simulador | Implementados en JavaScript local; requieren prueba manual final en navegador |
| Restricciones | Implementadas como simulación local; la autorización real queda en API según RF-05/RNF-03 |

Pendiente: prueba manual en navegadores y dispositivos objetivo, y publicación en GitHub Pages. Archify no se ejecutó; se conservan los insumos y el procedimiento manual en `diagramas/archify/`.
