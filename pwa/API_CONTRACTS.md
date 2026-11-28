# Contratos de integración requeridos

No se encontró una API REST implementada ni contratos versionados en las ramas inspeccionadas. La PWA no presupone que estas rutas existan: se configuran por ambiente mediante `VITE_API_*_PATH`.

| Recurso | Método | Autorización | Respuesta mínima esperada |
| --- | --- | --- | --- |
| Misiones | `GET` | pública o Bearer | lista con `id`, `title`, `description`, `status`, `level` |
| Niveles | `GET` | pública o Bearer | lista de nivel y misiones asociadas |
| Guías | `GET` | pública o Bearer | contenido educativo publicado |
| Logros | `GET` | pública o Bearer | catálogo; `unlocked` y fecha sólo para el usuario autenticado |
| Perfil propio | `GET` | Bearer | identidad, nivel, XP y preferencias del sujeto del token |
| Progreso propio | `GET` / `POST` | Bearer | estado verificado, versión/conflicto y actividad educativa |
| Monitoreo | `GET` | Bearer + rol | indicadores permitidos por la API, nunca credenciales de observabilidad |

La API debe validar `issuer`, `audience`, firma y expiración de los tokens de Keycloak. Las operaciones de recompensa, XP y sincronización deben verificarse en el servidor y responder conflictos explícitos; la PWA no envía XP arbitrario ni promueve el progreso local de invitado a oficial.
