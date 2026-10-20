# Publicación en GitHub Pages

El sitio estático se encuentra en `docs/`. La portada `index.html` enlaza las prácticas 04 y 07, conservando sus recursos y rutas relativas. `.nojekyll` permite servir los archivos sin procesarlos con Jekyll.

## Activación

Después de subir los archivos a GitHub:

1. Abre **Settings → Pages** del repositorio.
2. En **Build and deployment → Source**, selecciona **Deploy from a branch**.
3. Selecciona la rama que contiene estos archivos (`docs/documentacion` para esta entrega) y la carpeta **/docs**.
4. Guarda y espera a que termine el despliegue en **Actions**.

Cuando los cambios se integren mediante `develop` a `main`, puedes cambiar la rama de publicación a `main`, conservando `/docs`.

## Direcciones previstas

Estos enlaces estarán disponibles cuando GitHub Pages termine el despliegue:

- [Portada](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/)
- [Práctica 04: Canvas](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica04/)
- [Práctica 07: galería](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/)
- [Prototipo visual móvil](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/mockups-color/)
- [Diagrama interactivo](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/prototipo/)

## Vista local

Desde la raíz del repositorio:

```powershell
python -m http.server 8000 --directory docs
```

Abre `http://localhost:8000`. No se requiere compilar Flutter ni iniciar la API para consultar estos entregables.

GitHub Pages publica el contenido de `docs/`; los documentos Markdown también pueden consultarse directamente en el repositorio.
