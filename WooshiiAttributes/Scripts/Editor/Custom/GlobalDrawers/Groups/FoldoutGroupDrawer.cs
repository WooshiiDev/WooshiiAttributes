using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class FoldoutGroupDrawer : GlobalDrawer<FoldoutGroupAttribute>
    {
        private static GUIStyle HelpStyle;

        private bool m_shown;

        public FoldoutGroupDrawer(SerializedObject parent, SerializedProperty property) : base (parent, property)
        {
        }

        protected override void OnGUI_Internal()
        {
            if (HelpStyle == null)
            {
                HelpStyle = new GUIStyle (EditorStyles.boldLabel);
            }

            string name = Attributes[0].Name;
            if (m_shown = EditorGUILayout.Foldout (m_shown, name, true))
            {
                EditorGUI.indentLevel++;
                for (int j = 0; j < Attributes.Count; j++)
                {
                    FoldoutGroupAttribute groupAttribute = Attributes[j] as FoldoutGroupAttribute;

                    if (groupAttribute.Name == name)
                    {
                        EditorGUILayout.PropertyField (Properties[j]);
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}