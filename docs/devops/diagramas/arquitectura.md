# Diagrama de arquitectura — Unity + Flutter + API REST

Este documento presenta la arquitectura del sistema **TI: Soporte en Acción**. Los tres clientes (Unity, Flutter y React) acceden a los datos exclusivamente a través de la API REST; ninguno se conecta directamente a MongoDB (RNF-14).

> Los componentes marcados como *(propuesto)* o *(pendiente)* aún no están implementados en el repositorio.

## 1. Arquitectura general

```mermaid
flowchart TB
    subgraph Clientes["Clientes"]
        U["Videojuego Unity + C#<br/>(implementado parcial)"]
        F["App movil Flutter + Dart<br/>(pendiente)"]
        W["PWA React + Vite<br/>(pendiente)"]
    end

    subgraph Servicios["Servicios compartidos"]
        API["API REST<br/>Node.js + Express<br/>(pendiente)"]
        DB[("MongoDB<br/>ti_soporte_en_accion<br/>(pendiente)")]
    end

    subgraph Infra["Infraestructura DevOps (propuesta)"]
        CI["GitHub Actions"]
        DOCK["Docker + Docker Compose"]
        MON["Prometheus + Grafana"]
    end

    U -->|"HTTPS / JSON"| API
    F -->|"HTTPS / JSON"| API
    W -->|"HTTPS / JSON"| API
    API -->|"Mongoose"| DB
    CI -.->|"build y pruebas"| U
    CI -.->|"build y pruebas"| F
    CI -.->|"build y pruebas"| W
    CI -.->|"build y pruebas"| API
    DOCK -.->|"contenedores"| API
    DOCK -.->|"contenedor"| DB
    MON -.->|"metricas"| API
    MON -.->|"metricas"| DOCK
```

## 2. Interacción por casos de uso principales

```mermaid
sequenceDiagram
    autonumber
    actor J as Jugador
    participant U as Unity
    participant API as API REST
    participant DB as MongoDB
    participant F as Flutter
    participant W as PWA React

    J->>U: Inicia sesion y juega la mision
    U->>API: POST /intentos (resultado de la mision)
    API->>DB: Verifica mision y evita duplicados
    API->>DB: Registra intento y otorga XP una sola vez
    API-->>U: Confirmacion del resultado
    J->>F: Abre la app movil
    F->>API: GET /progreso (progreso del jugador)
    API->>DB: Consulta progreso
    API-->>F: Progreso actualizado
    J->>W: Abre la PWA
    W->>API: GET /progreso
    API-->>W: Progreso actualizado
```

## 3. Responsabilidad de cada componente

| Componente | Responsabilidad | Estado |
| --- | --- | --- |
| Unity + C# | Escenas, diálogos y retos; envía resultados a la API | Implementado parcial |
| Flutter + Dart | Consulta de misiones, guías y progreso vía API | Pendiente |
| React + PWA | Progreso, guías y panel de administración | Pendiente |
| API REST | Autenticación, autorización y datos comunes | Pendiente |
| MongoDB | Persistencia de cuentas, misiones, intentos y progreso | Pendiente |

## Notas

- La comunicación cliente–servidor será por **HTTPS** (RNF-02).
- La autorización se valida en el servidor por rol y propiedad del recurso (RNF-03).
- El diagrama de monitoreo se detalla en [`monitoreo.md`](monitoreo.md).
- El pipeline CI/CD y los ambientes se detallan en [`pipeline_cicd.md`](pipeline_cicd.md).
