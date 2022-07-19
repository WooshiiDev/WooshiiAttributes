using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
    public class ReadOnlyDrawer : WooshiiPropertyDrawer
    {
        private ReadOnlyAttribute m_target;
        private bool m_canShow;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_target = attribute as ReadOnlyAttribute;

            switch (m_target.m_displayMode)
            {
                case DisplayMode.EDITOR:
                    m_canShow = !EditorApplication.isPlaying;
                    break;

                case DisplayMode.PLAYING:
                    m_canShow = EditorApplication.isPlaying;
                    break;

                case DisplayMode.BOTH:
                    m_canShow = true;
                    break;
            }

            if (m_canShow)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField (position, property, label);
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (m_canShow)
            {
                return base.GetPropertyHeight (property, label);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}