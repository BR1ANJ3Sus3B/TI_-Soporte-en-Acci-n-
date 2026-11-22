# SSO de NovaTech Solutions

El proveedor único de identidad es Keycloak. MongoDB contiene únicamente datos
educativos indexados por `sub` de Keycloak; no contiene contraseñas ni secretos
de inicio de sesión.

## Tema

`compose.yaml` monta `themes/novatech` de solo lectura. Tras iniciar Keycloak,
un administrador del realm debe seleccionar **Realm settings > Themes > Login
theme > novatech**. El tema usa las rutas oficiales de Keycloak para login,
registro y recuperación; ninguna pantalla Flutter captura contraseñas.

## Clientes del mismo realm

Crear clientes públicos separados, todos con Standard Flow y PKCE S256. Mantener
Implicit Flow y Direct Access Grants desactivados.

| Plataforma | Client ID sugerido | Redirección permitida |
| --- | --- | --- |
| Flutter Android/iOS | `ti-soporte-flutter` | `com.tisoporte.accion:/oauthredirect` |
| PWA | `ti-soporte-pwa` | `https://<dominio-pwa>/*` |
| Unity | `ti-soporte-unity` | URI privada que implemente Unity |

No hay clientes PWA o Unity creados en esta rama: no publicar valores de ejemplo
ni habilitar comodines o localhost en producción. Los tres clientes comparten la
cookie de sesión SSO del navegador cuando plataforma y navegador lo permiten.

## Registro y protección de cuentas

En el realm habilitar User registration, Login with email, Verify email y Forgot
password. Definir `jugador` como rol default y comprobar que `administrador` no
sea default. Configurar la política de contraseñas, Brute Force Detection y OTP
según la política de NovaTech. SMTP y Google no se habilitan aquí porque el
repositorio no contiene credenciales autorizadas. Para Google, crear el cliente
OAuth en Google Cloud, registrar la redirect URI que muestra Keycloak para el
provider y cargar client ID/secret únicamente en Keycloak.

## Producción

Usar HTTPS, un hostname público estable, una base de datos soportada por
Keycloak y secretos inyectados por el entorno de despliegue. No usar `start-dev`,
`localhost` ni `KEYCLOAK_ALLOW_HTTP=true` en distribución pública. Compilar
Flutter con los `--dart-define` públicos (issuer HTTPS, URL API y client ID),
nunca con secretos de administrador, SMTP, Google o MongoDB.
