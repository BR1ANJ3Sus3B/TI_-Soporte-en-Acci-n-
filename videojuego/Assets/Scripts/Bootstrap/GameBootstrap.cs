using UnityEngine;

/// <summary>
/// Punto de entrada del gameplay. Crea los servicios (progreso, misiones, diálogos e
/// interfaz) y arranca la misión 01 con las referencias de la escena. Al estar todo
/// cableado desde aquí, la escena no depende de asignaciones manuales en el inspector.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("Misión 01")]
    [SerializeField] private MissionDefinition mision01;

    [Header("Referencias del mundo")]
    [SerializeField] private BrokenComputer computadora;
    [SerializeField] private PowerCable cable;
    [SerializeField] private NpcInteractable martin;
    [SerializeField] private SecretRoomDoor paredFalsa;

    void Awake()
    {
        Asegurar<ProgressService>("Progreso");
        Asegurar<MissionService>("Misiones");
        Asegurar<DialogueRunner>("Dialogos");
        Asegurar<GameUI>("Interfaz");

        var mision = gameObject.GetComponent<Mission01ElPrimerIncidente>();
        if (mision == null) mision = gameObject.AddComponent<Mission01ElPrimerIncidente>();
        mision.Configurar(mision01, computadora, cable, martin, paredFalsa);
    }

    private T Asegurar<T>(string nombre) where T : Component
    {
        var existente = FindFirstObjectByType<T>();
        if (existente != null) return existente;

        var go = new GameObject(nombre);
        go.transform.SetParent(transform, false);
        return go.AddComponent<T>();
    }
}
