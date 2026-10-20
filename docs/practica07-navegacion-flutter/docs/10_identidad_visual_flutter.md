# 10 · Identidad visual del proyecto aplicada a Flutter

**Proyecto:** TI: Soporte en Acción · **Empresa ficticia:** NovaTech Solutions.  
**Alcance:** diseño de **interfaces móviles Flutter (Android/iOS)**, no PWA ni aplicación web de usuario final.

## Concepto

Se propone una interfaz de **videojuego educativo / simulador de soporte técnico**: fondos oscuros, tarjetas de incidentes, contornos cian, indicadores de desempeño y llamados a la acción diferenciados. La inspiración procede de interfaces RPG 2D y paneles de tecnología; se evita copiar directamente la interfaz de otras aplicaciones.

## Paleta de color

| Uso | Color | HEX |
|---|---|---|
| Fondo principal | Negro azulado | `#070D1B` |
| Fondo secundario | Azul noche | `#091326` |
| Tarjetas | Azul oscuro | `#10213B` |
| Tarjetas elevadas | Azul profundo | `#142A47` |
| Acción principal y navegación activa | Cian | `#00D9F5` |
| Retroalimentación, enlaces positivos | Turquesa | `#28E6CB` |
| Niveles y elementos especiales | Violeta | `#906DFF` |
| Éxito, misión resuelta, verificación | Verde | `#38D99A` |
| XP, recompensa y misión destacada | Naranja | `#FF9E3B` |
| Error, bloqueo y seguridad | Rojo coral | `#FF5F78` |
| Texto principal | Blanco frío | `#F2FBFF` |
| Texto secundario | Azul grisáceo | `#A1B7CD` |

## Reglas de diseño y navegación

1. Todas las pantallas diseñadas muestran el contexto de **TI: Soporte en Acción**, con la empresa NovaTech Solutions en pantallas informativas.
2. Se mantiene jerarquía consistente: barra superior, contenido, tarjetas y barra de navegación inferior solamente después de autenticación.
3. Se usa **cian** para botones principales, navegación activa y resaltado de controles; **naranja** para recompensas XP y acceso a la siguiente misión.
4. **Verde** comunica operación completada; **rojo coral** comunica bloqueos; **violeta** identifica rangos, niveles y secciones especiales.
5. El menú inferior cambia de acuerdo con el rol; la autorización mostrada en los prototipos es **visual y simulada**, no reemplaza la autorización de la API / Keycloak.
6. Se incluyen alternativas visuales para un ancho móvil de alrededor de 396 px; la adaptación real a distintos teléfonos debe comprobarse durante la implementación Flutter.
7. El estilo no implica ejecutar las misiones Unity dentro de Flutter: la app móvil consulta misiones, seguimiento, guías y resultados.

## Dos tipos de evidencia, sin confundirlas

- **`sketches/`**: 20 sketches originales **en blanco y negro, apariencia de lápiz**, que cumplen el requisito de baja fidelidad.
- **`mockups-color/`**: 20 reinterpretaciones visuales **de mayor fidelidad**, a color, PNG independientes (aprox. 792×1692), creadas para mostrar cómo se vería cada pantalla con la paleta oficial.
- **`mockups-color/index.html`**: demostración navegable en HTML de diseño; no es la aplicación Flutter definitiva.
- **`prototipo/index.html`**: mapa interactivo de transiciones y roles, ahora con identidad visual NovaTech y acceso a ambas versiones.
- **`flutter_referencia/lib/theme/app_theme.dart`**: clase Dart referencial con colores y ThemeData para uso posterior en un proyecto Flutter.

## Alcance de la entrega

Estas imágenes representan una **propuesta de diseño**, no pantallas Flutter ejecutándose, no validan autenticación real ni permisos de producción. Los esquemas de pantalla y escenarios académicos mantienen pendiente la ejecución y evidencias en Archify, revisión real de recorridos y subida a GitHub.
