using UnityEngine;
using UnityEditor;

public static class GUIExtension
    {
    private readonly static GUIStyle style = new GUIStyle (EditorStyles.boldLabel);

    //public static EditorDrawProfile EditorProfile => AssetDatabase.LoadAssetAtPath ("Assets/Editor/_Settings/Design/Base Style.asset", typeof (EditorDrawProfile)) as EditorDrawProfile;

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

    public static void CreateNewSection(string str)
        {
        EditorGUILayout.BeginVertical ();
            {
            CreateLineSpacer ();
            CreateSubSection (str, style);
            }
        EditorGUILayout.EndVertical ();
        }

    public static void CreateNewSection(string str, System.Action drawCallback)
        {
        EditorGUILayout.BeginVertical ();
            {
            CreateLineSpacer ();

            EditorGUILayout.BeginHorizontal ();
            CreateSubSection (str, style);
            drawCallback?.Invoke ();

            EditorGUILayout.EndHorizontal ();

            }
        EditorGUILayout.EndVertical ();
        }

    public static void CreateSubSection(string str, GUIStyle skin = null)
        {
        EditorGUILayout.LabelField (str, style);
        }

    public static void CreateCenteredSection(string str, int strSize = 16)
        {
        //Draw label
        GUIStyle boldStyle = new GUIStyle (style)
            {
            alignment = TextAnchor.MiddleCenter,
            fontSize = strSize
            };

        EditorGUILayout.LabelField (str.ToUpper(), boldStyle);
        Rect rect = GUILayoutUtility.GetLastRect ();

        //Move to new line and set following line height
        rect.y += rect.height + 1;
        rect.height = 1;

        //Draw spacer
        CreateLineSpacer (rect, Color.white, rect.height);

        GUILayout.Space (6f);
        }
    }

    
