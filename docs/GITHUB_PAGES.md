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
- [Navegación de sketches por rol](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/wireframes/navegacion.html)
- [20 wireframes](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/wireframes/)
- [Descarga ZIP de los 20 wireframes](https://br1anj3sus3b.github.io/TI_-Soporte-en-Acci-n-/practica07-navegacion-flutter/wireframes/wireframes-20.zip)

## Comprobaciones antes de publicar

Los enlaces del sitio son relativos para funcionar bajo el nombre del repositorio. Se incluyen los PNG, los scripts de navegación y el ZIP; no hay dependencias de un servidor de aplicaciones. Publica desde `docs/documentacion` y `/docs`, donde ya existe `.nojekyll`.

Verificación de la entrega: `python docs/practica07-navegacion-flutter/scripts/verificar_entrega.py`. La comprobación de navegación en Edge se ejecuta con `node docs/practica07-navegacion-flutter/scripts/verificar_roles.cjs --navegacion`.

## Vista local

Desde la raíz del repositorio:

```powershell
python -m http.server 8000 --directory docs
```

Abre `http://localhost:8000`. No se requiere compilar Flutter ni iniciar la API para consultar estos entregables.

GitHub Pages publica el contenido de `docs/`; los documentos Markdown también pueden consultarse directamente en el repositorio.
