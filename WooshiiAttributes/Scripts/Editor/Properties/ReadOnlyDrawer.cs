using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
    public class ReadOnlyDrawer : WooshiiPropertyDrawer
    {
        private ReadOnlyAttribute m_target;
        private bool m_shown;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_target = attribute as ReadOnlyAttribute;

            switch (m_target.m_displayMode)
            {
                case DisplayMode.EDITOR:
                    m_shown = !EditorApplication.isPlaying;
                    break;

                case DisplayMode.PLAYING:
                    m_shown = EditorApplication.isPlaying;
                    break;

                case DisplayMode.BOTH:
                    m_shown = true;
                    break;

                default:
                    break;
            }

            if (m_shown)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField (position, property, label, true);
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_shown)
                return EditorGUI.GetPropertyHeight(property, label, property.isExpanded) + EditorGUIUtility.standardVerticalSpacing;
            else
                return 0;
        }
    }
}