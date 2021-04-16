using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WooshiiAttributes
{
    public class ReorderableDrawer : ArrayDrawer<ReorderableAttribute>
    {
        // --- Draw Refs ---
        private ReorderableList m_list;

        public ReorderableDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property)
        {
            if (m_list == null)
            {
                m_list = new ReorderableList (SerializedObject, SerializedProperty, true, true, true, true)
                {
                    drawHeaderCallback = DrawHeader,
                    drawElementCallback = DrawElement,
                    elementHeightCallback = GetElementHeight,
                };
            }
        }

        protected override void OnGUI_Internal()
        {
            EditorGUILayout.Space ();

            m_list.DoLayoutList ();

            EditorGUILayout.Space ();
        }

        private void DrawHeader(Rect _rect)
        {
            string label = $"{SerializedProperty.displayName} ({SerializedProperty.arrayElementType})";

            EditorGUI.LabelField (_rect, label);
        }

        private void DrawElement(Rect _rect, int _index, bool _isActive, bool _isFocused)
        {
            if (_index > SerializedProperty.arraySize)
            {
                return;
            }

            SerializedProperty property = SerializedProperty.GetArrayElementAtIndex (_index);

            if (property == null)
            {
                return;
            }

            _rect.x += 12f;
            _rect.width = _rect.width - 12f;

            EditorGUI.PropertyField (_rect, property, true);
        }

        private float GetElementHeight(int _index)
        {
            if (_index > SerializedProperty.arraySize)
            {
                return 19f;
            }

            SerializedProperty element = SerializedProperty.GetArrayElementAtIndex (_index);

            if (element == null)
            {
                return 19f;
            }

            return EditorGUI.GetPropertyHeight (element, true);
        }
    }
}