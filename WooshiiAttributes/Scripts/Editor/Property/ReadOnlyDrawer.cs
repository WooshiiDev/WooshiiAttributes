using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draw a control as readonly.
    /// </summary>
    [CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
    public class ReadOnlyDrawer : WooshiiPropertyDrawer
    {
        private ReadOnlyAttribute _target;
        private bool _shown;

        // - Methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _target = attribute as ReadOnlyAttribute;

            switch (_target._displayMode)
            {
                case DisplayMode.EDITOR:
                    _shown = !EditorApplication.isPlaying;
                    break;

                case DisplayMode.PLAYING:
                    _shown = EditorApplication.isPlaying;
                    break;

                case DisplayMode.BOTH:
                    _shown = true;
                    break;

                default:
                    break;
            }

            if (_shown)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField (position, property, label, true);
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_shown)
                return EditorGUI.GetPropertyHeight(property, label, property.isExpanded) + EditorGUIUtility.standardVerticalSpacing;
            else
                return 0;
        }
    }
}