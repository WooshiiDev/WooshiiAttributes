using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (ArrayElementsAttribute))]
    public class ArrayElementsDrawer : WooshiiPropertyDrawer
    {
        private const float BUTTON_SIZE = 19F;

        // --- Draw Refs ---
        private Texture2D AddTex => EditorGUIUtility.Load ("icons/d_winbtn_graph_max_h.png") as Texture2D;
        private Texture2D RemoveTex => EditorGUIUtility.Load ("icons/d_winbtn_graph_min_h.png") as Texture2D;

        private int m_index = 0;
        private SerializedProperty m_arrayProperty;

        private bool m_hasBeenValidated = false;
        private bool m_isValid = false;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (!m_hasBeenValidated)
            {
                //Get array path
                string path = _property.propertyPath;
                path = path.Substring (0, path.LastIndexOf ('.'));

                //Update the property to the array
                m_arrayProperty = _property.serializedObject.FindProperty (path);

                m_isValid = m_arrayProperty.isArray;

                if (!m_isValid)
                {
                    Debug.LogWarning ("Array Elements useless as property is not an array!");
                }

                m_hasBeenValidated = true;
            }

            if (!m_isValid)
            {
                base.OnGUI (_position, _property, _label);
                return;
            }

            //Find current index
            m_index = GetElementIndex (_property);

            //Draw property and buttons
            EditorGUI.BeginChangeCheck ();
            {
                _position.width -= BUTTON_SIZE * 2;

                EditorGUI.PropertyField (_position, _property, true);

                DrawButtonLabel (_position, RemoveTex, "", () => m_arrayProperty.DeleteArrayElementAtIndex (m_index));
                _position.x += BUTTON_SIZE;
                DrawButtonLabel (_position, AddTex, "", () => m_arrayProperty.InsertArrayElementAtIndex (m_index + 1));
            }
            if (EditorGUI.EndChangeCheck ())
            {
                _property.serializedObject.ApplyModifiedProperties ();
                _property.serializedObject.Update ();
            }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            if (_property.isExpanded)
            {
                return EditorGUI.GetPropertyHeight (_property, _label, true);
            }
            else
            {
                return base.GetPropertyHeight (_property, _label);
            }
        }

        private bool DrawButtonLabel(Rect _rect, Texture2D _texture, string _label, Action _action)
        {
            _rect.x += _rect.width;
            _rect.width = BUTTON_SIZE;

            if (GUI.Button (_rect, _texture, EditorStyles.centeredGreyMiniLabel))
            {
                _action?.Invoke ();
                return true;
            }

            return false;
        }

        private int GetElementIndex(SerializedProperty _property)
        {
            for (int i = 0; i < m_arrayProperty.arraySize; i++)
            {
                if (m_arrayProperty.GetArrayElementAtIndex (i).displayName == _property.displayName)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}