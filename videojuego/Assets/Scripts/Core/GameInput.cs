using UnityEngine;

/// <summary>
/// Bloqueo global de la entrada del jugador. Cualquier ventana modal (diálogo,
/// diagnóstico, aviso o cartel de misión) lo mantiene activo mientras está abierta,
/// de modo que Alex no se desplaza mientras el jugador lee o elige una opción.
/// </summary>
public static class GameInput
{
    public static bool Bloqueado { get; private set; }

    public static void Bloquear()
    {
        Bloqueado = true;
    }

    public static void Liberar()
    {
        Bloqueado = false;
    }
}
