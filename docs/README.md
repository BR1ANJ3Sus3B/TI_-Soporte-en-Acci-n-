# Documentación

Documentación del proyecto "TI: Soporte en Acción".



## Sitio publicado y prácticas interactivas

La documentación HTML está publicada en **GitHub Pages**. Estos enlaces permiten abrir los entregables directamente en el navegador:

| Entregable | Enlace público |
| --- | --- |
| Portada de documentación | [Abrir el sitio](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/) |
| Práctica 04: Business Model Canvas | [Explorar el Canvas actualizado](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica04/business-model-canvas/) |
| Análisis de ingresos del proyecto | [Consultar fuentes de ingresos, costos y punto de equilibrio](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica04/business-model-canvas/#analisis-ingresos) |
| Práctica 07: galería de pantallas móviles | [Ver diseños a color y sketches](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/) |
| Prototipo visual móvil | [Recorrer las pantallas](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/mockups-color/) |
| Diagrama de navegación interactivo | [Explorar roles y recorridos](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/prototipo/) |
| Galería de bocetos a lápiz | [Ver los sketches originales](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/galeria_sketches_lapiz.html) |
| Canvas de la carpeta original | [Consultar la versión original](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/business-model-canvas/) |

Los prototipos son entregables de diseño; no acreditan funciones de producción. El análisis de ingresos contiene propuestas y cifras hipotéticas por validar. El sitio se actualiza al subir cambios a `docs/` en la rama `docs/documentacion`.
## Documentación general

El contenido completo del proyecto (objetivos, roles, arquitectura, requerimientos RF-01 a RF-26 y RNF-01 a RNF-14, diseño del videojuego, personajes, niveles, storyboard, Canvas, monetización y bocetos de la PWA y de la aplicación móvil) se encuentra en:

- [Documento completo del proyecto](proyecto/README.md)
- [Cobertura documental](proyecto/COBERTURA_DOCUMENTAL.md)
- [Inventario de imágenes](inventario-imagenes.json)
- [Documentación DevOps](devops/README.md)

Las imágenes del proyecto están en [`docs/imagenes/`](imagenes/).

## Organización

```
docs/
├── README.md               # Este documento
├── proyecto/               # Visión general y alcance del proyecto
├── requerimientos/         # Requerimientos funcionales y no funcionales
├── historias-de-usuario/   # Historias de usuario
├── arquitectura/           # Arquitectura del sistema
├── diseno/
│   ├── bocetos-movil/      # Bocetos de la aplicación móvil
│   ├── bocetos-web/        # Bocetos de la aplicación web
│   ├── paleta-de-colores/  # Paleta de colores
│   └── personajes/         # Diseño de personajes
├── pruebas/                # Planes y resultados de pruebas
├── manuales/               # Manuales de usuario y de desarrollo
├── devops/                 # Documentación DevOps (CI/CD, entornos, SLA, métricas y configuración)
│   └── diagramas/          # Diagramas de arquitectura, pipeline y monitoreo
└── imagenes/               # Imágenes referenciadas por el documento del proyecto
```

## Secciones y estado

| Sección | Estado | Contenido |
| --- | --- | --- |
| `proyecto/` | Completa | Documento íntegro del proyecto y cobertura documental. |
| `devops/` | Completa | [Documentación DevOps](devops/README.md): justificación CI/CD, entornos, SLA, métricas y parámetros de configuración, con diagramas (`devops/diagramas/`). |
| `requerimientos/` | Pendiente | RF-01 a RF-26 y RNF-01 a RNF-14 ya redactados en `proyecto/README.md`; se desglosarán aquí si se requiere. |
| `historias-de-usuario/` | Pendiente | Historias de usuario. |
| `arquitectura/` | Pendiente | Componentes, diagramas y decisiones técnicas. |
| `diseno/bocetos-movil/` | Pendiente | Bocetos de la aplicación móvil (actualmente en `proyecto/README.md` e `imagenes/`). |
| `diseno/bocetos-web/` | Pendiente | Bocetos de la PWA (actualmente en `proyecto/README.md` e `imagenes/`). |
| `diseno/paleta-de-colores/` | Pendiente | Paleta acordada para Flutter y React (listada en `proyecto/README.md`). |
| `diseno/personajes/` | Pendiente | Diseño de personajes (hojas de sprite en `imagenes/`). |
| `pruebas/` | Pendiente | Estrategia y evidencia de pruebas. |
| `manuales/` | Pendiente | Guías de uso y de desarrollo. |

## Estado actual

La organización inicial, el contenido completo del proyecto y las imágenes ya están integrados. La sección `devops/` reúne la documentación de DevOps; su contenido describe propuestas de CI/CD, Docker y monitoreo (Prometheus y Grafana), que aún no están implementados en el repositorio. Las secciones marcadas como "Pendiente" conservan su carpeta con un archivo `.gitkeep` a la espera de contenido; no se duplica información que ya está en `proyecto/README.md`.
