# TI: Soporte en Acción — navegación de interfaces

Prototipo HTML estático para explorar la navegación y las **38 comparaciones visuales reales** del proyecto. No modifica Unity, Flutter, React/Vite ni servicios.

## Recursos integrados

- `../imagenes/20_Pantallas_generales/`: 20 PNG, `P01_comparacion.png` a `P20_comparacion.png`.
- `../imagenes/18_Pantallas_por_rol/`: 18 PNG distribuidos en `Jugador`, `Administrador`, `Docente`, `Evaluador`, `Editor_contenido` y `Soporte_tecnico`.
- No se generaron, movieron, recortaron ni duplicaron imágenes. Cada PNG ya contiene la comparación completa de sketch y wireframe.

## Uso y publicación

Abra `index.html` desde un servidor estático o publique el directorio `docs/` con GitHub Pages y visite `/practica_navegacion/`. Las rutas son relativas y no contienen rutas absolutas de Windows.

## Alcance documentado

La aplicación incluye galería por módulo y por rol, visor completo, diagrama general y seis vistas por rol, simulador local de recorrido, matriz de permisos y enlaces a fichas de cada pantalla. Los controles de permisos son demostrativos: la autorización definitiva corresponde a la API (RF-05 y RNF-03).

Las funciones de Docente (RF-24/RF-25) pertenecen a segunda etapa. Las vistas de Evaluador y Soporte técnico se señalan con requisito específico pendiente donde el repositorio no define uno; no se inventaron identificadores.
