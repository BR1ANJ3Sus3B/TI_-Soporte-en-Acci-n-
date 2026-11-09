# Sistema de diseño · TI: Soporte en Acción

Esta PWA traslada la guía visual de Figma a React. No modifica ni sincroniza el archivo Figma.

## Identidad y tokens

El imagotipo provisional `TI` representa soporte técnico y conexiones. `BrandLogo`, `BrandSymbol` y `AppIcon` admiten uso compacto y se muestran sobre fondo oscuro; los SVG instalables conservan la misma paleta. Los sprites oficiales, cuando estén disponibles, sustituyen únicamente los placeholders pixel art.

| Token | HEX | RGB | Uso |
|---|---|---|---|
| Fondo | `#101C34` | 16, 28, 52 | `--color-background` |
| Superficie | `#0B1324` | 11, 19, 36 | `--color-surface` |
| Elevada | `#1F2937` | 31, 41, 55 | `--color-surface-elevated` |
| Acción | `#4AE4EC` | 74, 228, 236 | `--color-primary-action` |
| Acento | `#9A7CFF` | 154, 124, 255 | `--color-accent` |
| Éxito | `#41DEA7` | 65, 222, 167 | `--color-success` |
| Advertencia | `#FFB45B` | 255, 180, 91 | `--color-warning` |
| Error | `#EF4444` | 239, 68, 68 | `--color-error` |
| Texto | `#F3F7FF` | 243, 247, 255 | `--color-text-primary` |
| Texto secundario | `#A9B7CC` | 169, 183, 204 | `--color-text-secondary` |
| Borde | `#34445F` | 52, 68, 95 | `--color-border` |

Los aliases heredados (`--surface`, `--accent`, etc.) se mantienen para migración incremental, pero resuelven a los tokens Figma.

## Tipografía, iconos y espaciado

Space Grotesk se usa en títulos; Inter en contenido, formularios y navegación; Press Start 2P solo en insignias (`.pixel-badge`, `AchievementBadge`). La escala es Display 36, H1 32, H2 24, H3 20, cuerpo 18/16/14, etiqueta 14 y caption 12 px. Lucide React usa 24 px normal, 20 px compacto y 32 px destacado. La escala de espacio es 4, 8, 12, 16, 20, 24 y 32 px.

## Componentes

- Botones cyan con texto oscuro, foco visible y estado deshabilitado.
- `Card`, `ProgressBar`, `StatusBadge`, `AlertMessage`, `EmptyState`, `LoadingState`, `ErrorState`, `AchievementBadge` y `ConnectionStatus` expresan estado con icono, texto y color.
- `Input` incorpora etiqueta visible, obligatorio, foco, error y `aria-describedby`; los flujos que requieren servidor se presentan como demostración o solo lectura.
- Las tarjetas, filtros y encabezados se adaptan a una columna; en móvil hay navegación inferior para Inicio, Misiones, Logros y Perfil, y menú para el resto.

## Accesibilidad y pixel art

El texto principal, controles y estados se usan sobre superficies oscuras con contraste AA; los elementos interactivos tienen `:focus-visible`, etiquetas y semántica. Se respeta `prefers-reduced-motion`. `.pixel-art` usa `image-rendering: pixelated`, solo en avatares, sprites y narrativa, nunca como sustitución de iconos funcionales.

## Frames de Figma → React

| Frame | Implementación |
|---|---|
| Dashboard estudiante | `DashboardPage`, `WelcomeBanner`, `SummaryCards`, `NextMission` |
| Catálogo de misiones | `MissionsPage`, filtros y `MissionDetailPage` |
| Progreso | `ProgressPage`, `SkillProgress` |
| Logros | `AchievementsPage`, `AchievementBadge` |
| Perfil | `ProfilePage`, `BrandLogo`, `StatusBadge` |
| Paneles institucionales | Rutas `docente/*` y `admin/*`, `RolePanel` |

## Integraciones

Los datos están aislados en `src/data/demoData.ts`; los servicios tipados (`src/services`) leen `VITE_API_BASE_URL` y `VITE_DEMO_MODE`. La API REST debe validar JWT y roles en servidor cuando se conecte Keycloak. La PWA usa `vite-plugin-pwa`, manifiesto, iconos SVG, actualización automática y caché de recursos estáticos; no guarda tokens privados en caché.
