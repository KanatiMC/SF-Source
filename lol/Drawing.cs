using System;
using System.Reflection;
using UnityEngine;

public static class Drawing
{
    private static Texture2D aaLineTex = null;
    private static Texture2D lineTex = null;
    private static Material blitMaterial = null;
    private static Material blendMaterial = null;
    private static Rect lineRect = new Rect(0f, 0f, 1f, 1f);

    private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
    {
        float num = 1f - t;
        return (Vector2) ((((((num * num) * num) * s) + ((((3f * num) * num) * t) * st)) + ((((3f * num) * t) * t) * et)) + (((t * t) * t) * e));
    }

    public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
    {
        Vector2 pointA = CubeBezier(start, startTangent, end, endTangent, 0f);
        for (int i = 1; i < (segments + 1); i++)
        {
            Vector2 pointB = CubeBezier(start, startTangent, end, endTangent, ((float) i) / ((float) segments));
            DrawLine(pointA, pointB, color, width, antiAlias);
            pointA = pointB;
        }
    }

    public static void DrawCircle(Vector2 center, int radius, Color color, float width, int segmentsPerQuarter)
    {
        DrawCircle(center, radius, color, width, false, segmentsPerQuarter);
    }

    public static void DrawCircle(Vector2 center, int radius, Color color, float width, bool antiAlias, int segmentsPerQuarter)
    {
        float num = ((float) radius) / 2f;
        Vector2 start = new Vector2(center.x, center.y - radius);
        Vector2 endTangent = new Vector2(center.x - num, center.y - radius);
        Vector2 startTangent = new Vector2(center.x + num, center.y - radius);
        Vector2 end = new Vector2(center.x + radius, center.y);
        Vector2 vector5 = new Vector2(center.x + radius, center.y - num);
        Vector2 vector6 = new Vector2(center.x + radius, center.y + num);
        Vector2 vector7 = new Vector2(center.x, center.y + radius);
        Vector2 vector8 = new Vector2(center.x - num, center.y + radius);
        Vector2 vector9 = new Vector2(center.x + num, center.y + radius);
        Vector2 vector10 = new Vector2(center.x - radius, center.y);
        Vector2 vector11 = new Vector2(center.x - radius, center.y - num);
        Vector2 vector12 = new Vector2(center.x - radius, center.y + num);
        DrawBezierLine(start, startTangent, end, vector5, color, width, antiAlias, segmentsPerQuarter);
        DrawBezierLine(end, vector6, vector7, vector9, color, width, antiAlias, segmentsPerQuarter);
        DrawBezierLine(vector7, vector8, vector10, vector12, color, width, antiAlias, segmentsPerQuarter);
        DrawBezierLine(vector10, vector11, start, endTangent, color, width, antiAlias, segmentsPerQuarter);
    }

    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
    {
        float num = pointB.x - pointA.x;
        float num2 = pointB.y - pointA.y;
        float num3 = Mathf.Sqrt((num * num) + (num2 * num2));
        if (num3 >= 0.001f)
        {
            Texture2D lineTex;
            if (!antiAlias)
            {
                lineTex = Drawing.lineTex;
                Material blitMaterial = Drawing.blitMaterial;
            }
            else
            {
                width *= 3f;
                lineTex = aaLineTex;
                Material blendMaterial = Drawing.blendMaterial;
            }
            float num4 = (width * num2) / num3;
            float num5 = (width * num) / num3;
            Matrix4x4 identity = Matrix4x4.identity;
            identity.m00 = num;
            identity.m01 = -num4;
            identity.m03 = pointA.x + (0.5f * num4);
            identity.m10 = num2;
            identity.m11 = num5;
            identity.m13 = pointA.y - (0.5f * num5);
            GL.PushMatrix();
            GL.MultMatrix(identity);
            GUI.color = color;
            GUI.DrawTexture(lineRect, lineTex);
            GL.PopMatrix();
        }
    }

    public static void Initialize()
    {
        if (lineTex == null)
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            lineTex.SetPixel(0, 1, Color.magenta);
            lineTex.Apply();
        }
        if (aaLineTex == null)
        {
            aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, true);
            aaLineTex.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
            aaLineTex.SetPixel(0, 1, Color.magenta);
            aaLineTex.SetPixel(0, 2, new Color(1f, 1f, 1f, 0f));
            aaLineTex.Apply();
        }
        blitMaterial = (Material) typeof(GUI).GetMethod("get_blitMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        blendMaterial = (Material) typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
    }
}

