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

                default:
                    break;
            }

            if (m_canShow)
            {
                //EditorGUI.BeginProperty (position, label, property);

                GUI.enabled = false;
                //EditorGUI.PropertyField (position, property, label, true);
                EditorGUILayout.PropertyField (property, label, true);
                GUI.enabled = true;

                //EditorGUI.EndProperty ();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //if (canShow)
            //    return base.GetPropertyHeight (property, label) * 10;
            //else
            //    return 0;

            return 0;
        }
    }
}