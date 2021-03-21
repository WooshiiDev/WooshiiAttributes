using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WooshiiAttributes
{
    public class ReorderableDrawer : ArrayDrawer<ReorderableAttribute>
    {
        // --- Draw Refs ---
        private ReorderableList list;

        public ReorderableDrawer(SerializedObject parent, SerializedProperty property) : base (parent, property)
        {
            if (list == null)
            {
                list = new ReorderableList (SerializedObject, SerializedProperty, true, true, true, true)
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

            list.DoLayoutList ();

            EditorGUILayout.Space ();
        }

        private void DrawHeader(Rect rect)
        {
            string label = $"{SerializedProperty.displayName} ({SerializedProperty.arrayElementType})";

            EditorGUI.LabelField (rect, label);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index > SerializedProperty.arraySize)
            {
                return;
            }

            SerializedProperty property = SerializedProperty.GetArrayElementAtIndex (index);

            if (property == null)
            {
                return;
            }

            rect.x += 12f;
            rect.width = rect.width - 12f;

            EditorGUI.PropertyField (rect, property, true);
        }

        private float GetElementHeight(int index)
        {
            if (index > SerializedProperty.arraySize)
            {
                return 19f;
            }

            SerializedProperty element = SerializedProperty.GetArrayElementAtIndex (index);

            if (element == null)
            {
                return 19f;
            }

            return EditorGUI.GetPropertyHeight (element, true);
        }
    }
}
