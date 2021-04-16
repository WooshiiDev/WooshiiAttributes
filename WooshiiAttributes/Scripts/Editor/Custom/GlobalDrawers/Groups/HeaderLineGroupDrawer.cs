using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class HeaderLineGroupDrawer : GlobalDrawer<HeaderLineGroupAttributeAttribute>
    {
        private static GUIStyle HeaderLineStyle;

        public HeaderLineGroupDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            if (HeaderLineStyle == null)
            {
                HeaderLineStyle = new GUIStyle (EditorStyles.boldLabel);
            }

            string name = Attributes[0].Name;
            EditorGUILayout.LabelField (name.ToUpper (), HeaderLineStyle);

            Color color = HeaderLineStyle.normal.textColor;
            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += rect.height - 1;

            WooshiiGUI.CreateLineSpacer (rect, color, 1);

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