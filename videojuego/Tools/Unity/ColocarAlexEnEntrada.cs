using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Coloca a Alex en el vestíbulo de la entrada sur del edificio.</summary>
public static class ColocarAlexEnEntrada
{
    // Vestíbulo de la entrada inferior: celdas (0,-9) y (0,-10), con las puertas en
    // y=-8 (interior) e y=-11 (calle). El sprite mide 1.28 y se dibuja centrado en el
    // transform, así que y=-9 deja los pies en -9.64 y la cabeza en -8.36: entero
    // dentro del vestíbulo.
    private static readonly Vector3 Entrada = new Vector3(0.5f, -9.0f, 0f);

    public static string Execute()
    {
        var sb = new StringBuilder();

        var jugador = GameObject.Find("Player");
        if (jugador == null) return "ERROR: no hay Player en la escena";

        jugador.transform.position = Entrada;
        sb.AppendLine("Player = " + jugador.transform.position);

        // La cámara arranca ya encuadrada sobre el jugador (MapCameraFrame lo hace en
        // Awake, pero así la escena guardada también se ve bien sin entrar en Play).
        var camara = GameObject.FindWithTag("MainCamera");
        if (camara != null)
        {
            camara.transform.position = new Vector3(Entrada.x, Entrada.y, camara.transform.position.z);
            sb.AppendLine("Camera = " + camara.transform.position);
        }

        var escena = SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(escena);
        EditorSceneManager.SaveScene(escena, escena.path);
        sb.AppendLine("guardada = " + escena.path);

        return sb.ToString();
    }
}
