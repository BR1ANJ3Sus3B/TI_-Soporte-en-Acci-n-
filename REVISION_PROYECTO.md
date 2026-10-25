# Revision local - 24 de octubre de 2026

## Reconstruccion del mapa NovaTech

- `nivel1.unity` conserva los recursos originales y contiene un edificio de 48x32 celdas: cinco oficinas superiores, cuatro inferiores, cuatro habitaciones laterales, pasillo central y nucleo tecnico.
- Se separaron `Tilemap_Floor`, `Tilemap_Walls`, `Tilemap_Doors`, `Tilemap_Windows` y `Tilemap_Decoracion`. El suelo cubre 1536 celdas, con 45 puertas, 25 ventanas y paredes interiores/exteriores.
- Las paredes usan `TilemapCollider2D` estatico. Puertas y ventanas son capas visuales sin collider para conservar los accesos transitables.
- Player aparece en el pasillo inferior, en `(0, -12)`, con sus animaciones. La luz global y `MapCameraFrame` encuadran el mapa completo.
- `nivel1` es la primera escena de Build Settings; `SampleScene` se conserva.
- `OfficeMapBuilder.cs` permite regenerar el mapa desde `Tools > TI Soporte > Construir mapa NovaTech`, limpiando sus Tilemaps antes de colocar los tiles.
- Pasaron cinco pruebas de `tools/validate_empresa_map.py`: distribucion, referencias, colisiones, aparicion del jugador y configuracion de inicio.
- Los scripts del escenario y `MapCameraFrame` compilan contra las bibliotecas instaladas de Unity con Roslyn. Esto no sustituye una prueba manual en Play.

## Limites conocidos

1. `Mapa-Principal.prefab` conserva referencias antiguas sin destino local. No se usa para el nivel; el mapa usa tiles validos de `Nivel01_Empresa`.
2. Git muestra `videojuego/` como no seguido y la referencia antigua `My project (2)` como eliminada. No se creo ningun commit.
3. `Renderer2D.asset` conserva siete GUID sin destino encontrado; conviene comprobar la iluminacion 2D en el editor.
4. Caminar arriba, izquierda y derecha reutiliza clips de correr a velocidad 0.6 porque no hay clips independientes conectados.
5. La validacion incluida es estatica; conviene pulsar Play en Unity para revisar visualmente el encuadre y recorrer todas las entradas.

## Comprobaciones

- `Player.prefab` tiene `PlayerMovement`, `Rigidbody2D`, collider y `Player.controller`.
- El movimiento diagonal se normaliza y la fisica se actualiza en `FixedUpdate`.
- No se encontraron assets sin `.meta` en la exploracion.
- `flutter analyze --no-pub` no reporta problemas en la app complementaria.

Para probarlo, abre `nivel1.unity`, pulsa Play y recorre con WASD o flechas.
