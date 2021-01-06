using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WooshiiAttributes
    {
    public class ReorderableDrawer : ArrayDrawer
        {
        // --- Draw Refs ---
        private SerializedProperty arrayProperty;
        private ReorderableList list;
        private bool isShown;

        public ReorderableDrawer() : base (typeof(ReorderableAttribute))
            {

            }

        protected override void OnGUI_Internal(SerializedProperty property)
            {
            if (!property.isArray)
                {
                base.OnGUI_Internal (property);
                Debug.LogError ("Array Elements useless as property is not an array!");

                return;
                }
      
            arrayProperty = property;
                
            if (list == null)
                {
                list = new ReorderableList (property.serializedObject, property, true, true, true, true)
                    {
                    drawHeaderCallback = DrawHeader,
                    drawElementCallback = DrawElement,
                    elementHeightCallback = GetElementHeight,
                    };
                }

            EditorGUILayout.Space ();

            list.DoLayoutList ();

            EditorGUILayout.Space ();
            }

        private void DrawHeader(Rect rect)
            {
            string label = $"{arrayProperty.displayName} ({arrayProperty.arrayElementType})";
            
            EditorGUI.LabelField (rect, label);
            }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
            if (index > arrayProperty.arraySize)
                return;

            SerializedProperty property = arrayProperty.GetArrayElementAtIndex (index);

            if (property == null)
                return;

            rect.x += 12f;
            rect.width = rect.width - 12f;

            EditorGUI.PropertyField (rect, property, true);
            }

        private float GetElementHeight(int index)
            {
            if (index > arrayProperty.arraySize)
                return 19f;

            var element = arrayProperty.GetArrayElementAtIndex (index);

            if (element == null)
                return 19f;

            return EditorGUI.GetPropertyHeight (element, true);
            }
        }
    }
