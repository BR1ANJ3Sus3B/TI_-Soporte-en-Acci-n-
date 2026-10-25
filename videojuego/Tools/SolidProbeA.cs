using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class SolidProbeA
{
    public static string Execute()
    {
        var sb = new StringBuilder();
        Tile solid = AssetDatabase.LoadAssetAtPath<Tile>("Assets/Art/Characters/Scenes/Nivel01_Empresa/Tiles/NovaTech_Solido.asset");
        if (solid == null) { sb.AppendLine("no existe"); return sb.ToString(); }
        sb.AppendLine("colliderType=" + solid.colliderType);
        sb.AppendLine("sprite=" + (solid.sprite != null ? solid.sprite.name : "null"));
        if (solid.sprite != null)
        {
            sb.AppendLine("rect=" + solid.sprite.rect + " rectW=" + solid.sprite.rect.width + " rectH=" + solid.sprite.rect.height);
            sb.AppendLine("ppu=" + solid.sprite.pixelsPerUnit);
        }
        Matrix4x4 m = solid.transform;
        sb.AppendLine("m00=" + m.m00 + " m11=" + m.m11 + " m03=" + m.m03 + " m13=" + m.m13);
        return sb.ToString();
    }
}
