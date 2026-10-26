using System;

/// <summary>Una línea de diálogo: quién habla y qué dice.</summary>
[Serializable]
public struct DialogueLine
{
    public string hablante;
    public string texto;

    public DialogueLine(string hablante, string texto)
    {
        this.hablante = hablante;
        this.texto = texto;
    }
}
