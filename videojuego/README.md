# Videojuego (Unity + C#)

Videojuego educativo 2D de "TI: Soporte en Acción". Simula, mediante misiones y diálogos con NPC, los retos que enfrenta un profesional de Tecnologías de la Información en el entorno laboral.

## Propósito

- Representar situaciones reales de soporte de TI de forma interactiva.
- Servir como componente principal de simulación del proyecto.
- Compartir progreso y resultados con la API para que la app móvil y la web los muestren.

## Organización

Este es un proyecto Unity real (no una estructura vacía). Contiene:

```
videojuego/
├── Assets/
│   ├── Animacion/      # Animaciones existentes del personaje
│   ├── Art/            # Arte, personajes y escenas existentes
│   ├── Scripts/        # Scripts en C# (contiene PlayerMovement.cs)
│   ├── Settings/       # Configuración de render (URP 2D)
│   ├── Welcome/        # Contenido de la plantilla 2D de Unity
│   ├── Audio/          # (previsto)
│   ├── Materials/      # (previsto)
│   ├── Prefabs/        # (previsto)
│   ├── Scenes/         # (previsto)
│   ├── Sprites/        # (previsto)
│   └── UI/             # (previsto)
├── Packages/           # manifest.json y packages-lock.json
├── ProjectSettings/    # Configuración del proyecto Unity
└── README.md
```

Las carpetas previstas se crean vacías para organizar el trabajo futuro. Las carpetas existentes (`Animacion`, `Art`, `Scripts`, `Settings`, `Welcome`) se conservan con su contenido y sus archivos `.meta`.

Subcarpetas previstas dentro de `Assets/Scripts/` para organizar los scripts:

- `Player/`: movimiento y control del jugador.
- `NPC/`: comportamiento de personajes no jugables.
- `Missions/`: lógica de misiones.
- `UI/`: interfaz del juego.
- `Services/`: servicios (por ejemplo, comunicación con la API).

## Estado actual

- Proyecto Unity 6 (`6000.6.3f1`) con Universal Render Pipeline 2D.
- Existe un script `Assets/Scripts/PlayerMovement.cs` y recursos de personajes y animaciones.
- Aún no se implementan las misiones, los diálogos ni la integración con la API.

## Ejecución

El proyecto se abre con Unity Hub usando la versión de editor indicada en `ProjectSettings/ProjectVersion.txt` (`6000.6.3f1`). No se documentan comandos de línea de comandos porque el proyecto se gestiona desde el editor de Unity.

## Nota de control de versiones

El proyecto de Unity era antes un repositorio Git anidado (`My project (2)`). Se integró en este monorepositorio dentro de `videojuego/`; su carpeta `.git` original se conservó como respaldo fuera del repositorio. Los archivos `.meta` de Unity se mantienen junto a sus recursos.

Los archivos generados por Unity (`Library`, `Temp`, `Logs`, `UserSettings`, entre otros) están excluidos mediante `.gitignore`.