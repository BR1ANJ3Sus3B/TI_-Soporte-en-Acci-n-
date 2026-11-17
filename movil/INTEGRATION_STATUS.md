# Integración Flutter — estado y dependencias

## Implementado en esta rama

- Keycloak móvil por navegador del sistema con Authorization Code + PKCE.
- Tokens de la sesión cifrados por Android Keystore / iOS Keychain, renovación y
  restauración en el siguiente inicio cuando el refresh token sigue vigente.
- Se leen nombre, correo y roles del ID token para presentación. La app no usa
  esos claims para autorizar operaciones del servidor.
- Misiones, objetivos, niveles, XP, habilidades, áreas, logros y actividad son
  constantes de demostración bajo `lib/data/mock_data.dart`. Se señalan como tales
  en las vistas.
- El mapa actual es un mapa conceptual de áreas de oficina, no un mapa de niveles.
- El botón de inicio de una misión explica que debe ejecutarse en Unity. No marca
  el resultado como completado ni desbloquea logros/recompensas.

## Bloqueo de API confirmado

`../api/README.md` indica que la API es pendiente. En `../api/src/` las carpetas
de rutas, modelos y servicios solo contienen `.gitkeep`; no hay `package.json`,
servidor, endpoints ni contrato JSON. Por eso Flutter no configura una URL base,
no inventa rutas ni simula una conexión HTTP exitosa. MongoDB tampoco es accesible
desde la app: el camino esperado es `Flutter → API → MongoDB`.

El documento `../docs/proyecto/README.md` enumera operaciones conceptuales
(perfil, catálogo, guías, intentos y progreso), pero no define paths, métodos,
esquemas de request/response, códigos de error, paginación, versionado ni
audiencia. Antes de conectar la app, el backend debe publicar esos contratos y
validar firma, issuer, audiencia, expiración, roles, propiedad del recurso e
idempotencia de resultados.

## Dependencias para completar el ecosistema

| Área | Evidencia actual | Para completarla se requiere |
|---|---|---|
| Perfil | Nombre/correo desde Keycloak; estadísticas demo | Endpoint de perfil autenticado y política de edición |
| Misiones | Catálogo demo y filtros por estado/área | Catálogo real, reglas de desbloqueo y estados del backend |
| Guías | No hay pantalla ni modelos en la app móvil | Endpoints y contrato de guías/contenido |
| Progreso | XP, habilidades y misiones demo | Fuente de verdad, historial, esquema de progreso y perfil |
| Logros | Estados demo; no se generan al iniciar misión | Reglas server-side y fechas de desbloqueo verificables |
| Unity | Proyecto separado; no hay cliente API funcional en Unity | Identidad compartida, envío idempotente de intentos y contrato de resultados |
| PWA | Cliente separado; sin sesión web de Keycloak | Decidir SSO web/API; la sesión Flutter no se comparte directamente con el navegador |
| Offline | No hay caché ni cola de sincronización | Contratos/versionado, políticas de conflicto y fuente de verdad antes de cachear |
| Roles | Claims mostrados como información | Nombres definitivos y permisos aplicados por la API en cada operación |

El inicio de sesión móvil aún requiere probarse con el issuer/realm real y el
cliente público `ti-soporte-flutter`; la expiración, el refresh token y el retorno
del navegador se verifican actualmente con proveedor simulado. La compilación de
