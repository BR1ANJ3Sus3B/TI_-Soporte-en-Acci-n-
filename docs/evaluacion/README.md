# Evaluación 360° Adaptada — TI: Soporte en Acción

Esta carpeta contiene el sistema de evaluación para el equipo de dos integrantes. Se trata de una evaluación 360° adaptada: autoevaluación y evaluación mutua; no es anónima ni usa múltiples fuentes independientes.

## Entregables

- `Evaluacion_360_TI_Soporte_en_Accion.xlsx`: cuestionarios, validaciones, fórmulas, resultados, gráficas y plan de mejora.
- `Informe_Evaluacion_360_TI_Soporte_en_Accion.docx`: informe editable.
- `Informe_Evaluacion_360_TI_Soporte_en_Accion.pdf`: versión de consulta.
- `generar_evaluacion_360.py`: generador reproducible.

## Uso

1. Instale dependencias: `py -3 -m pip install -r requirements.txt`
2. Ejecute: `py -3 generar_evaluacion_360.py`
3. Para capturar en HTML y actualizar Excel automáticamente, ejecute: `py -3 servidor_evaluacion.py`
4. Abra `http://localhost:8765` en el navegador. Cada cambio se guarda automáticamente en `respuestas_evaluacion.json` y actualiza `Evaluacion_360_TI_Soporte_en_Accion.xlsx`.
5. También puede responder directamente en Excel: las celdas de respuesta se encuentran desbloqueadas; las fórmulas están protegidas sin contraseña.

La escala válida es `1`, `2`, `3`, `4`, `5` o `N/A`. Las respuestas `N/A` y las celdas vacías no entran en los promedios. Si no existen calificaciones numéricas, los promedios permanecen vacíos.

No se generan resultados ficticios: las secciones de resultados, conclusiones y plan de mejora parten vacías o marcadas como pendientes.

El servidor es local: no publica ni envía respuestas a Internet. Para detenerlo, presione `Ctrl+C` en la terminal donde se ejecuta.
