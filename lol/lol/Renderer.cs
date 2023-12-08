namespace lol
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class Renderer
    {
        public static void DrawString(Vector2 position, string label, bool centered = true)
        {
            GUIContent content = new GUIContent(label);
            Vector2 vector = StringStyle.CalcSize(content);
            GUI.Label(new Rect(centered ? (position - (vector / 2f)) : position, new Vector2(250f, 30f)), label);
        }

        public static void DrawString(Vector2 position, string label, UnityEngine.Color color, bool centered = true)
        {
            Color = color;
            DrawString(position, label, centered);
        }

        public static GUIStyle StringStyle { get; set; }

        public static UnityEngine.Color Color
        {
            get => 
                GUI.color;
            set => 
                GUI.color = value;
        }
    }
}

