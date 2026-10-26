using UnityEngine;

/// <summary>
/// Datos de una misión. Es un ScriptableObject para poder añadir misiones nuevas sin
/// reescribir el sistema: basta con crear otro asset y engancharlo a su controlador.
/// </summary>
[CreateAssetMenu(fileName = "Mision", menuName = "TI Soporte/Misión")]
public class MissionDefinition : ScriptableObject
{
    [Header("Identidad")]
    public string id = "M01";
    public string numero = "01";
    public string titulo = "EL PRIMER INCIDENTE";

    [Header("Objetivo")]
    [TextArea(2, 4)]
    public string objetivo = "Dirígete a la oficina de soporte y revisa la computadora reportada.";

    [Header("Recompensas")]
    public int recompensaXp = 100;
    public string insigniaId = "primer_diagnostico";
    public string insigniaNombre = "Primer Diagnóstico";

    [Header("Registro")]
    [TextArea(1, 3)]
    public string registro = "Incidente 001 resuelto: cable de alimentación desconectado.";
}
