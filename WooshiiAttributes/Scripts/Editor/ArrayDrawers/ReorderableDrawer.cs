using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WooshiiAttributes
{
    [RegisterDrawer]
    public class ReorderableDrawer : GUIDrawer<ReorderableAttribute>
    {
        // --- Draw Refs ---
        private ReorderableList m_list;

        private SerializedProperty Property => properties[0].SerializedValue;

        public ReorderableDrawer(ReorderableAttribute attribute) : base(attribute) { }

        public override void OnGUI()
        {
            if (m_list == null)
            {
                GUIProperty property = properties[0];
                SerializedProperty prop = property.SerializedValue;

                m_list = new ReorderableList(prop.serializedObject, prop, true, true, true, true)
                {
                    drawHeaderCallback = DrawHeader,
                    drawElementCallback = DrawElement,
                    elementHeightCallback = GetElementHeight,
                };
            }

            EditorGUILayout.Space();

            m_list.DoLayoutList();

            EditorGUILayout.Space();
        }

        private void DrawHeader(Rect _rect)
        {
            string label = $"{Property.displayName} ({Property.arrayElementType})";

            EditorGUI.LabelField (_rect, label);
        }

        private void DrawElement(Rect _rect, int _index, bool _isActive, bool _isFocused)
        {
            if (_index > Property.arraySize)
            {
                return;
            }

            SerializedProperty property = Property.GetArrayElementAtIndex (_index);

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
            if (_index > Property.arraySize)
            {
                return 19f;
            }

            SerializedProperty element = Property.GetArrayElementAtIndex (_index);

            if (element == null)
            {
                return 19f;
            }

            return EditorGUI.GetPropertyHeight (element, true);
        }
    }
}