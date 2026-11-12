using System;
using UnityEngine;

/// <summary>Asocia los nombres del diálogo con los retratos originales de los personajes.</summary>
public class DialoguePortraitRegistry : MonoBehaviour
{
    [Serializable]
    public struct Entry
    {
        public string nombre;
        public Sprite retrato;
    }

    public static DialoguePortraitRegistry Instancia { get; private set; }
    [SerializeField] private Entry[] entradas;

    void Awake() => Instancia = this;
    void OnDestroy() { if (Instancia == this) Instancia = null; }

    public void Configurar(Sprite alex, Sprite martin, Sprite richard, Sprite marco, Sprite sofia)
    {
        entradas = new[]
        {
            new Entry { nombre = "Alex", retrato = alex },
            new Entry { nombre = "Martín", retrato = martin },
            new Entry { nombre = "Martin", retrato = martin },
            new Entry { nombre = "Richard", retrato = richard },
            new Entry { nombre = "Marco", retrato = marco },
            new Entry { nombre = "Sofía", retrato = sofia },
            new Entry { nombre = "Sofia", retrato = sofia }
        };
    }

    public static Sprite Obtener(string nombre)
    {
        if (Instancia == null || string.IsNullOrEmpty(nombre) || Instancia.entradas == null) return null;
        foreach (var entrada in Instancia.entradas)
            if (string.Equals(entrada.nombre, nombre, StringComparison.OrdinalIgnoreCase)) return entrada.retrato;
        return null;
    }
}
