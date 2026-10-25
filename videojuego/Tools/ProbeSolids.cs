using System.Text;
using UnityEngine;

public static class ProbeSolids
{
    public static string Execute()
    {
        var samples = new (string label, Vector2 point, bool expectSolid)[]
        {
            ("cafeteria mesa redonda",  new Vector2(15.5f, -2.5f), true),
            ("cafeteria paso libre",    new Vector2(17.5f, -4.5f), false),
            ("cafeteria encimera",      new Vector2(17.5f, -6.5f), true),
            ("recepcion nucleo",        new Vector2(0.5f, 3.5f),   true),
            ("nucleo paso libre",       new Vector2(0.5f, 4.5f),   false),
            ("nucleo ascensores",       new Vector2(0.5f, 6.5f),   true),
            ("pasillo vertical",        new Vector2(0.5f, -5.5f),  false),
            ("muro nucleo norte",       new Vector2(0.5f, 7.5f),   true),
            ("oficina S2 escritorio",   new Vector2(4.5f, -3.5f),  true),
            ("oficina S2 paso libre",   new Vector2(4.5f, -5.5f),  true),
            ("hall NO escritorio",      new Vector2(-18.5f, 14.5f), true),
            ("hall NO pasillo",         new Vector2(-18.5f, 13.5f), false),
            ("sala este B mesa",        new Vector2(15.5f, 3.5f),  true),
            ("sala este B libre",       new Vector2(17.5f, 2.5f),  false),
        };

        var sb = new StringBuilder();
        int fails = 0;

        foreach (var sample in samples)
        {
            var hits = Physics2D.OverlapPointAll(sample.point);
            bool solid = hits.Length > 0;

            var who = new StringBuilder();
            foreach (var hit in hits)
                who.Append(hit.name).Append(' ');

            bool ok = solid == sample.expectSolid;
            if (!ok) fails++;

            sb.AppendLine(string.Format("{0,-24} ({1,6:0.0},{2,6:0.0}) esperado={3,-8} real={4,-8} {5} [{6}]",
                sample.label, sample.point.x, sample.point.y,
                sample.expectSolid ? "SOLIDO" : "libre",
                solid ? "SOLIDO" : "libre",
                ok ? "OK" : "FALLO", who.ToString().Trim()));
        }

        sb.AppendLine();
        sb.AppendLine(fails == 0 ? "RESULTADO: todos los puntos coinciden" : $"RESULTADO: {fails} discrepancias");
        return sb.ToString();
    }
}
