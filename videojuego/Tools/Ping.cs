using UnityEngine;

public static class Ping
{
    public static void Execute()
    {
        Debug.Log("AURA_PING_OK " + Application.unityVersion);
    }
}
