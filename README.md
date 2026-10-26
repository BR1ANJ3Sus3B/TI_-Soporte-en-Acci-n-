# TI: Soporte en Accion

Videojuego educativo 2D sobre los desafios de un profesional de Tecnologias de la Informacion.

El proyecto usa Unity 6000.6.3f1 y esta en `videojuego/`. Abre esa carpeta desde Unity Hub y carga `Assets/Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity`.

El primer nivel es una oficina simetrica de NovaTech: cinco oficinas superiores, cuatro inferiores, cuatro laterales, un nucleo tecnico y un pasillo central. Usa los tiles existentes, 48x32 celdas, puertas, ventanas y un `TilemapCollider2D` para las paredes. Player aparece en el pasillo inferior; usa WASD o flechas para caminar y Shift para correr.

Para regenerar unicamente este mapa desde Unity usa `Tools > TI Soporte > Construir mapa NovaTech`. La herramienta limpia y vuelve a construir sus Tilemaps sin duplicar objetos ni modificar otras escenas.

El prefab `Assets/Scripts/Player.prefab` utiliza `Assets/Art/Characters/Player.controller`. `nivel1` es la primera escena de la compilacion; `SampleScene` sigue disponible para pruebas del personaje.

`Mapa-Principal.prefab` es una paleta historica, no una escena. El mapa usa los tiles validos de `Nivel01_Empresa`. Las copias previas estan en `videojuego/Logs/MapRepairBackup/` (carpeta local ignorada por Git).

Validacion estatica: `python tools/validate_empresa_map.py` (Python y PyYAML). Estas comprobaciones no sustituyen una prueba en Unity Play.

`videojuego/app_flutter/` contiene la pantalla inicial de la app complementaria.

Consulta [REVISION_PROYECTO.md](REVISION_PROYECTO.md) para los hallazgos y limites de la revision local.
