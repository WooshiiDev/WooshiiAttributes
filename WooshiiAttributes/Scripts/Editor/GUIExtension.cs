using System;
using UnityEditor;
using UnityEngine;

public static class WooshiiGUI
{
    private readonly static GUIStyle s_style = new GUIStyle (EditorStyles.boldLabel);

    public static void CreateLineSpacer()
    {
        EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
    }

    public static void CreateLineSpacer(Color color)
    {
        Color oldColour = GUI.color;

        GUI.color = color;
        EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
        GUI.color = oldColour;
    }

    public static void CreateLineSpacer(Rect rect, Color color, float height = 2)
    {
        rect.height = height;

        Color oldColour = GUI.color;

        GUI.color = color;
        EditorGUI.DrawRect (rect, color);
        GUI.color = oldColour;
    }

    public static void CreateNewSection(string title)
    {
        EditorGUILayout.BeginVertical ();
        {
            CreateLineSpacer ();
            CreateSubSection (title);
        }
        EditorGUILayout.EndVertical ();
    }

    public static void CreateNewSection(string title, Action onDrawCallback)
    {
        EditorGUILayout.BeginVertical ();
        {
            CreateLineSpacer ();

            EditorGUILayout.BeginHorizontal ();
            CreateSubSection (title);
            onDrawCallback?.Invoke ();

            EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();
    }

    public static void CreateSubSection(string title)
    {
        EditorGUILayout.LabelField (title, s_style);
    }

    public static void CreateCenteredSection(string title, int fontSize = 16)
    {
        //Draw label
        GUIStyle boldStyle = new GUIStyle (s_style)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = fontSize
        };

        EditorGUILayout.LabelField (title.ToUpper (), boldStyle);
        Rect rect = GUILayoutUtility.GetLastRect ();

        //Move to new line and set following line height
        rect.y += rect.height + 1;
        rect.height = 1;

        //Draw spacer
        CreateLineSpacer (rect, Color.white, rect.height);

        GUILayout.Space (6f);
    }
}