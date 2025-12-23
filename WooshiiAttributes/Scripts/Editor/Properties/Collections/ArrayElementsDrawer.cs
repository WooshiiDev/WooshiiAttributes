using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Adds add/remove controls to array elemennts.
    /// </summary>
    [CustomPropertyDrawer (typeof (ArrayElementsAttribute))]
    public class ArrayElementsDrawer : WooshiiPropertyDrawer
    {
        private const float BUTTON_SIZE = 19F;
   
        private int _index = 0;
        private SerializedProperty _arrayProperty;

        private bool _hasBeenValidated = false;
        private bool _isValid = false;

        private Texture2D AddTex => EditorGUIUtility.Load("icons/d_winbtn_graph_max_h.png") as Texture2D;
        private Texture2D RemoveTex => EditorGUIUtility.Load("icons/d_winbtn_graph_min_h.png") as Texture2D;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_hasBeenValidated)
            {
                //Get array path
                string path = property.propertyPath;
                path = path.Substring (0, path.LastIndexOf ('.'));

                //Update the property to the array
                _arrayProperty = property.serializedObject.FindProperty (path);

                _isValid = _arrayProperty.isArray;

                if (!_isValid)
                {
                    Debug.LogWarning ("Array Elements useless as property is not an array!");
                }

                _hasBeenValidated = true;
            }

            if (!_isValid)
            {
                base.OnGUI (position, property, label);
                return;
            }

            //Find current index
            _index = GetElementIndex (property);

            //Draw property and buttons
            EditorGUI.BeginChangeCheck ();
            {
                position.width -= BUTTON_SIZE * 2;

                EditorGUI.PropertyField (position, property, true);

                DrawButtonLabel (position, RemoveTex, "", () => _arrayProperty.DeleteArrayElementAtIndex (_index));
                position.x += BUTTON_SIZE;
                DrawButtonLabel (position, AddTex, "", () => _arrayProperty.InsertArrayElementAtIndex (_index + 1));
            }
            if (EditorGUI.EndChangeCheck ())
            {
                property.serializedObject.ApplyModifiedProperties ();
                property.serializedObject.Update ();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                return EditorGUI.GetPropertyHeight (property, label, true);
            }
            else
            {
                return base.GetPropertyHeight (property, label);
            }
        }

        private bool DrawButtonLabel(Rect rect, Texture2D texture, string label, Action action)
        {
            rect.x += rect.width;
            rect.width = BUTTON_SIZE;

            if (GUI.Button (rect, texture, EditorStyles.centeredGreyMiniLabel))
            {
                action?.Invoke ();
                return true;
            }

            return false;
        }

        private int GetElementIndex(SerializedProperty property)
        {
            for (int i = 0; i < _arrayProperty.arraySize; i++)
            {
                if (_arrayProperty.GetArrayElementAtIndex (i).displayName == property.displayName)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}