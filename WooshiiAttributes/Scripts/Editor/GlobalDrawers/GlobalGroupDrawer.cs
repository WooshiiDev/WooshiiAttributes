using UnityEngine;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GlobalGroupDrawer : GlobalDrawer<GlobalGroupAttribute>
    {
        private GlobalGroupAttribute Header => Attributes[0] as GlobalGroupAttribute;
        private bool foldout;

        private const float HEADER_HEIGHT = 23F;
        private const float FOOTER_HEIGHT = 3F;

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
                //EditorGUILayout.BeginVertical (EditorStyles.helpBox);
                InspectorGUI.BeginContainer (GetTotalHeight ());
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

                for (int i = 0; i < Attributes.Count; i++)
                {
                    GlobalGroupAttribute groupAttribute = Attributes[i] as GlobalGroupAttribute;

                    if (groupAttribute.Name == name)
                    {
                        EditorGUILayout.PropertyField (Properties[i], new GUIContent(Properties[i].displayName),  Properties[i].isExpanded);
                    }
                }
            }

            if (isContained)
            {
                //EditorGUILayout.EndVertical ();
                InspectorGUI.EndInspectorContainer ();
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
            return GUILayoutUtility.GetLastRect ();
        }

        private float GetTotalHeight()
        {
            if (!foldout)
            {
                return 20f;
            }

            float height = 0;

            for (int i = 0; i < Properties.Count; i++)
            {
                height += EditorGUI.GetPropertyHeight (Properties[i], true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height + HEADER_HEIGHT + FOOTER_HEIGHT;
        }
    }
}