using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class FoldoutGroupDrawer : GlobalDrawer<FoldoutGroupAttribute>
    {
        private bool shown;
        private static GUIStyle helpStyle;

        public FoldoutGroupDrawer(SerializedObject parent, SerializedProperty property) : base (parent, property)
        {
        }

        protected override void OnGUI_Internal()
        {
            if (helpStyle == null)
            {
                helpStyle = new GUIStyle (EditorStyles.boldLabel);
            }

            string name = Attributes[0].Name;
            if (shown = EditorGUILayout.Foldout (shown, name, true))
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