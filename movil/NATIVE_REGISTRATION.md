# Registro nativo Flutter → API → Keycloak

## Flujo implementado

`Crear una cuenta gratis` abre `/register` dentro de Flutter. Se utiliza el logo
`assets/icons/Soporte en Acción Pixelado.png` y los componentes/tokens Stitch del
proyecto. No hay WebView ni navegador durante la captura de datos.

La pantalla envía exclusivamente `name`, `email`, `password` a
`POST /api/v1/auth/register`. La confirmación de contraseña solo se valida en el
dispositivo. La contraseña no se escribe en almacenamiento local, MongoDB o logs;
permanece transitoriamente en memoria y se envía a Keycloak. Los controladores se
limpian tras aceptación y se liberan al salir de la pantalla.

Validaciones: nombre 2–100 caracteres, correo normalizado y máximo 254 caracteres,
contraseña 12–128 con mayúscula, minúscula, número y símbolo. Keycloak aplica además
su política de contraseñas. Campos adicionales, incluidos roles, se rechazan.

La API obtiene un token de servicio con client_credentials (no Password Grant),
comprueba SMTP y el rol no compuesto `jugador`, crea el usuario inicialmente
deshabilitado con `emailVerified=false` y `VERIFY_EMAIL`, elimina grupos/roles por
defecto de ESA CUENTA NUEVA y asigna únicamente jugador. Solo entonces la habilita
y pide a Keycloak `send-verify-email`. No modifica cuentas existentes.

Un fallo de preparación deja la cuenta nueva deshabilitada y devuelve 503; requiere
revisión del operador, no se borra el usuario. Si falla solo el correo, permanece
VERIFY_EMAIL pendiente y se puede solicitar un nuevo envío. Las acciones requeridas
de Keycloak bloquean la finalización de login mientras el correo no esté verificado.

El resultado público 202 es una **confirmación neutral de solicitud**, no una
afirmación de que un correo se envió o una cuenta nueva existe. Los duplicados
reciben la misma respuesta sin alterar la cuenta existente. No se anuncia éxito
si Keycloak rechaza la creación, el servicio falla o el correo nuevo falla.

`POST /api/v1/auth/resend-verification` acepta solo email. Responde de forma neutral
si no hay usuario elegible; si SMTP falla devuelve 503. Enlaces expirados se
reemplazan solicitando otro correo. La UI exige esperar un minuto.

Después se ofrece Iniciar sesión: navegador seguro, Authorization Code + PKCE S256,
callback ya existente. Nunca se convierte la contraseña de registro en tokens.

## Configuración requerida en Keycloak 26.4

El servicio activo no tiene credenciales de registro configuradas. Un administrador
debe crear un cliente confidencial dedicado en `ti-soporte` con service accounts
habilitadas, sin Standard Flow ni Direct Access Grants. No usar el cliente público
`ti-soporte-flutter`, no dar `realm-admin` ni permisos para administrar clientes.

Permisos requeridos por las llamadas implementadas:

- Consulta de realm/SMTP y del rol jugador (view-realm).
- Consulta, creación y actualización de usuarios; envío de verificación.
- Gestión de grupos y asignaciones de roles de los usuarios nuevos.

Configurar permisos administrativos granulares cuando estén disponibles,
restringiendo la asignación al rol jugador. Los roles heredados `manage-users` y
`view-realm` son amplios: no otorgar privilegios adicionales. Validar los permisos
del cliente en el entorno antes de habilitar registro público; la app no contiene
credenciales ni acceso a endpoints administrativos.

Configurar SMTP real y remitente en Realm settings → Email. Debe poder entregar a
una bandeja controlada por el equipo. Sin SMTP la API falla antes de crear usuarios.
No se ha comprobado SMTP real: falta la credencial de servicio para leer el realm.

Añadir al archivo **ignorado** `infra/api/.env` los valores reales, usando la
plantilla `infra/api/.env.example`:

- KEYCLOAK_ADMIN_BASE_URL (base de Keycloak, sin `/realms/...`).
- KEYCLOAK_REGISTRATION_REALM=ti-soporte.
- KEYCLOAK_REGISTRATION_CLIENT_ID y KEYCLOAK_REGISTRATION_CLIENT_SECRET.
- REGISTRATION_ALLOW_HTTP=true únicamente para desarrollo local con ADB y red
  Docker privada. En producción false, Keycloak HTTPS y API detrás de TLS.
- TRUSTED_PROXY solo IP/CIDR del proxy TLS real; no confiar en cualquier origen.

No pasar ninguna de estas credenciales mediante dart-define. TLS valida los
certificados; no hay bypass. El endpoint rechaza HTTP salvo opción local explícita.

## Límites

Registro y reenvío comparten: 10 solicitudes/IP/15 minutos, 100 globales/15 minutos,
una por email/minuto. Las claves de email se hashean; mapa acotado a 10000 entradas.
Son límites por proceso en memoria. Para varios contenedores o exposición pública
persistente, aplicar cuota compartida en el gateway (incluidos reinicios) y control
anti-bot. No se ha instalado un sistema CAPTCHA ni un almacén de cuota distribuido.

## MongoDB

El registro no escribe en MongoDB. `GET /api/v1/profile`, tras autenticación, usa
upsert por `subject` de Keycloak para el perfil educativo; la identidad tiene índice
único. Repetir la consulta no crea otro perfil. El esquema no tiene contraseña.

## Ejecución

Desde `C:\Users\jesus\AppData\Local\Temp\opencode\ti-flutter-keycloak`:

```powershell
docker compose -f infra/api/compose.yaml up -d --build --no-deps api
.\scripts\check-services.ps1
.\scripts\run-flutter.ps1
```

La compilación Android usa `movil/keycloak.local.json`. El registro requiere API
disponible; el invitado sigue sin depender de Keycloak ni del registro.

## Verificación

Tests de backend usan transporte Keycloak simulado explícito para validar orden
de creación, rol exclusivo, SMTP ausente, fallo de correo, duplicados, datos y cuotas.
Tests Flutter verifican navegación nativa, payload sin token/rol, validaciones,
aceptación y error de API. No sustituyen la creación de un usuario real.

Bloqueos actuales: credencial de servicio ausente, SMTP sin verificar y Android
desconectado. No se creó una cuenta real ni se envió correo. La prueba completa
requiere una bandeja de prueba autorizada; no usar cuentas de terceros.

La imagen de API fue reconstruida y el contenedor actualizado con estos endpoints.
Prueba real por HTTP local: ambos endpoints devuelven 400 `https_required` porque
la excepción de desarrollo no está habilitada. El servicio ejecutado dentro del
contenedor devuelve 503 `registration_unavailable` por falta de credenciales.
`/health/ready` sigue confirmando MongoDB conectado y Keycloak configurado para JWT.
Para pruebas USB locales hay que habilitar explícitamente REGISTRATION_ALLOW_HTTP
en el .env del backend; no confundirlo con KEYCLOAK_ALLOW_HTTP del APK.

APK compilado con el JSON local real y build ID `native-registration-20261122` en
`movil/build/app/outputs/flutter-apk/app-debug.apk`. Sin Android conectado no se
instaló ni se capturó una pantalla real del teléfono.
