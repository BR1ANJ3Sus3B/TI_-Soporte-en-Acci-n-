# 09 · Instrucciones GitHub, versionamiento y entrega

## 1. Carpeta y rama

Coloca la carpeta `TI_Soporte_Movil_20_Sketches_Separados` en el repositorio de tu Proyecto Integrador. El nombre de carpeta recomendado dentro del repositorio es `practica07-navegacion-flutter/` (ajustar el número de práctica si la asignatura utiliza otro).

Repositorio de referencia conocido del proyecto:
`https://github.com/BR1ANJ3Sus3B/Practicas_Integradora_Brian-230308`

> El ZIP no ha sido subido a GitHub automáticamente. La rama `Practica07-SketchesMovil` es una **sugerencia**, no una rama creada.

## 2. Comandos sugeridos (PowerShell / terminal)

```bash
cd ruta/a/Practicas_Integradora_Brian-230308
git switch main
git pull origin main
git switch -c Practica07-SketchesMovil
# Copiar la carpeta de la práctica aquí, con nombre practica07-navegacion-flutter/
git add practica07-navegacion-flutter/
git commit -m "docs: agrega 20 sketches Flutter y navegacion por roles"
git push -u origin Practica07-SketchesMovil
```

Crear un **Pull Request** hacia `main`, verificar que el equipo revisó el contenido e integrar sólo mediante el procedimiento que solicite el docente o el equipo.

## 3. Visualización

- Galería PNG: `practica07-navegacion-flutter/index.html`.
- Diagrama navegable: `practica07-navegacion-flutter/prototipo/index.html`.
- Fuentes del diagrama: `diagramas/navegacion_general.mmd` y `diagramas/navegacion_estatica.svg`.
- Manual completo: `docs/README_15_ACTIVIDADES.md` y fichas de 20 pantallas.
- Archify: importar el prompt `docs/08_prompt_archify.md`, documentar cambios y capturas.

## 4. GitHub Pages (opcional para compartir)

Si el repositorio usa GitHub Pages desde `main` o `docs/`, asegúrate de ubicar los archivos en la ruta publicada, o elegir la fuente de publicación compatible. El HTML de esta práctica usa rutas relativas y no requiere backend.

## 5. Checklist antes de entregar

- [ ] Confirmar que hay **exactamente 20 PNG únicos** P01–P20.
- [ ] Corregir errores de ortografía y consistencia de los sketches.
- [ ] Revisar que P01–P09 corresponden a las obligatorias.
- [ ] Confirmar 11 adicionales P10–P20.
- [ ] Probar galería HTML local.
- [ ] Probar diagrama interactivo y filtros por rol.
- [ ] Recrear y comprobar el flujo en **Archify**.
- [ ] Tomar capturas de los recorridos y guardar evidencias.
- [ ] Ejecutar plan de 26 pruebas, registrar resultado real.
- [ ] Validar enlaces del README en el repositorio remoto.
- [ ] Verificar permisos y rutas; cerrar casos sin evidencia.
- [ ] Confirmar commit, push, PR y revisión antes de integrar a `main`.
- [ ] Entregar el enlace de la rama / PR / GitHub Pages según lo solicite la materia.
