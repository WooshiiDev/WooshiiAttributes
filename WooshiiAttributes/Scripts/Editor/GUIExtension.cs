using System;
using UnityEditor;
using UnityEngine;

public static class WooshiiGUI
{
    private readonly static GUIStyle style = new GUIStyle (EditorStyles.boldLabel);

    //public static EditorDrawProfile EditorProfile => AssetDatabase.LoadAssetAtPath ("Assets/Editor/_Settings/Design/Base Style.asset", typeof (EditorDrawProfile)) as EditorDrawProfile;

    public static void CreateLineSpacer()
    {
        EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
    }

    public static void CreateLineSpacer(Color _color)
    {
        Color oldColour = GUI.color;

        GUI.color = _color;
        EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
        GUI.color = oldColour;
    }

    public static void CreateLineSpacer(Rect _rect, Color _color, float _height = 2)
    {
        _rect.height = _height;

        Color oldColour = GUI.color;

        GUI.color = _color;
        EditorGUI.DrawRect (_rect, _color);
        GUI.color = oldColour;
    }

    public static void CreateNewSection(string _title)
    {
        EditorGUILayout.BeginVertical ();
        {
            CreateLineSpacer ();
            CreateSubSection (_title, style);
        }
        EditorGUILayout.EndVertical ();
    }

    public static void CreateNewSection(string _title, Action _onDrawCallback)
    {
        EditorGUILayout.BeginVertical ();
        {
            CreateLineSpacer ();

            EditorGUILayout.BeginHorizontal ();
            CreateSubSection (_title, style);
            _onDrawCallback?.Invoke ();

            EditorGUILayout.EndHorizontal ();
        }
        EditorGUILayout.EndVertical ();
    }

    public static void CreateSubSection(string _title, GUIStyle _skin = null)
    {
        EditorGUILayout.LabelField (_title, style);
    }

    public static void CreateCenteredSection(string _title, int _fontSize = 16)
    {
        //Draw label
        GUIStyle boldStyle = new GUIStyle (style)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = _fontSize
        };

        EditorGUILayout.LabelField (_title.ToUpper (), boldStyle);
        Rect rect = GUILayoutUtility.GetLastRect ();

        //Move to new line and set following line height
        rect.y += rect.height + 1;
        rect.height = 1;

        //Draw spacer
        CreateLineSpacer (rect, Color.white, rect.height);

        GUILayout.Space (6f);
    }
}