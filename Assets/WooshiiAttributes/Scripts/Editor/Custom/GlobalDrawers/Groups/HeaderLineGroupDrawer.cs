using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class HeaderLineGroupDrawer : GlobalDrawer<HeaderLineGroupAttributeAttribute>
    {
        private static GUIStyle style;

        public HeaderLineGroupDrawer(SerializedObject parent, SerializedProperty property) : base (parent, property)
        {
        }

        protected override void OnGUI_Internal()
        {
            if (style == null)
            {
                style = new GUIStyle (EditorStyles.boldLabel);
            }

            string name = Attributes[0].Name;
            EditorGUILayout.LabelField (name.ToUpper (), style);

            Color color = style.normal.textColor;
            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += rect.height - 1;

            GUIExtension.CreateLineSpacer (rect, color, 1);

            for (int j = 0; j < Attributes.Count; j++)
            {
                if (!(Attributes[j] is HeaderLineGroupAttributeAttribute groupAttribute))
                {
                    continue;
                }

                if (groupAttribute.Name == name)
                {
                    EditorGUILayout.PropertyField (Properties[j]);
                }
            }
        }
    }
}