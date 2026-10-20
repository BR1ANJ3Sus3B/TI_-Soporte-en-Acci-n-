# Docker — TI: Soporte en Acción

Esta carpeta reúne el trabajo de contenedores del proyecto. Su desarrollo se realiza en la rama `feature/docker`, con el objetivo de preparar un entorno reproducible para los servicios compartidos del videojuego, la aplicación móvil y la PWA.

## Estado actual

La configuración está pendiente de implementación. Esta carpeta contiene únicamente este README; todavía no incluye archivos Dockerfile, configuración de Docker Compose ni servicios listos para ejecutar.

## Alcance previsto

| Componente | Trabajo previsto |
| --- | --- |
| API Node.js + Express | Preparar su imagen cuando exista una aplicación ejecutable en `api/`. |
| MongoDB | Definir el servicio de base de datos y la persistencia de sus datos. |
| Docker Compose | Coordinar los servicios, sus redes, variables de entorno y volúmenes. |
| Prometheus y Grafana | Incorporar el monitoreo cuando existan servicios instrumentados. |

Los clientes Unity, Flutter y React consumirán la API; no se conectarán directamente a MongoDB. El código de cada componente permanecerá en su carpeta del monorepositorio.

## Organización prevista

Los siguientes archivos y directorios se crearán conforme se implementen los servicios:

```text
Docker/
├── README.md             # Propósito y estado del trabajo
├── compose.yaml          # Definición de servicios del entorno
├── .env.example          # Variables de ejemplo, sin credenciales reales
├── api/
│   └── Dockerfile        # Construcción de la imagen de la API
└── monitoring/
    ├── prometheus/       # Configuración de métricas
    └── grafana/          # Configuración de paneles
```

## Configuración y datos

- Documentar las variables necesarias en `.env.example` y mantener los valores locales fuera del control de versiones.
- Definir volúmenes para los datos persistentes de MongoDB.
- Documentar los puertos y las conexiones entre servicios al implementar Compose.
- Mantener separados los datos de desarrollo y los de pruebas.

## Puesta en marcha

Los comandos de construcción, inicio y apagado se añadirán cuando exista una configuración verificada. Antes de dar el entorno por terminado, se comprobarán el arranque de los servicios, la conexión de la API con MongoDB y la persistencia de los datos al reiniciar los contenedores.

## Documentación relacionada

- [Descripción del proyecto](../README.md)
- [API REST](../api/README.md)
- [Base de datos MongoDB](../database/README.md)
- [Documentación DevOps](../docs/devops/README.md)
- [Entornos y requerimientos](../docs/devops/02_Entornos_Requerimientos.md)
- [Parámetros de configuración](../docs/devops/05_Parametros_Configuracion.md)
