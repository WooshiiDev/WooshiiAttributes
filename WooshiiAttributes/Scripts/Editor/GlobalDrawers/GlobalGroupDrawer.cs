using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GlobalGroupDrawer : GlobalDrawer<GlobalGroupAttribute>
    {
        private GlobalGroupAttribute Header => Attributes[0] as GlobalGroupAttribute;
        private bool foldout;

        public GlobalGroupDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            string name = Header.Name;

            bool isContained = Header.Contained;
            bool hasFoldout = Header.Foldout;

            GUIContent label = new GUIContent(Header.Uppercase ? Header.Name.ToUpper () : Header.Name);

            // Container

            if (isContained)
            {
                EditorGUILayout.BeginVertical (EditorStyles.helpBox);
            }

            // Label

            EditorGUILayout.LabelField (label, EditorStyles.boldLabel);

            // Foldout

            if (hasFoldout)
            {
                foldout = EditorGUI.Foldout (GetFoldoutRect(), foldout, "", true);
            }

            // Attribute Draw

            if (hasFoldout == foldout)
            {
                if (Header.Underline)
                {
                    Color textColor = (isContained) ? new Color (0.4f, 0.4f, 0.4f) : EditorStyles.boldLabel.normal.textColor;

                    WooshiiGUI.CreateLineSpacer (GetUnderlineRect (isContained), textColor, 1);
                    GUILayout.Space (3f);
                }

                if (isContained)
                {
                    EditorGUI.indentLevel++;
                }

                for (int j = 0; j < Attributes.Count; j++)
                {
                    GlobalGroupAttribute groupAttribute = Attributes[j] as GlobalGroupAttribute;

                    if (groupAttribute.Name == name)
                    {
                        EditorGUILayout.PropertyField (Properties[j]);
                    }
                }

                if (isContained)
                {
                    EditorGUI.indentLevel--;
                }
            }

            if (hasFoldout)
            {
                EditorGUILayout.EndFoldoutHeaderGroup ();
            }

            if (isContained)
            {
                EditorGUILayout.EndVertical ();
            }
        }

        private Rect GetUnderlineRect(bool isContained)
        {
            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += rect.height - 1;

            if (isContained)
            {
                rect.x -= 4f;
                rect.width += 8f;
                rect.y += 3f;
            }

            return rect;
        }

        private Rect GetFoldoutRect()
        {
            Rect rect = GUILayoutUtility.GetLastRect ();

            rect.width += rect.x - 16f;
            rect.x = 16;

            return rect;
        }
    }
}