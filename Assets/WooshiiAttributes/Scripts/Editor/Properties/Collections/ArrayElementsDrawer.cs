using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (ArrayElementsAttribute))]
    public class ArrayElementsDrawer : WooshiiPropertyDrawer
    {
        // --- Draw Refs ---
        private Texture2D AddTex => EditorGUIUtility.Load ("icons/d_winbtn_graph_max_h.png") as Texture2D;
        private Texture2D RemoveTex => EditorGUIUtility.Load ("icons/d_winbtn_graph_min_h.png") as Texture2D;

        private int index = 0;
        private SerializedProperty arrayProperty;

        private const float BUTTON_SIZE = 19F;

        private bool hasBeenValidated = false;
        private bool isValid = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!hasBeenValidated)
            {
                //Get array path
                string path = property.propertyPath;
                path = path.Substring (0, path.LastIndexOf ('.'));

                //Update the property to the array
                arrayProperty = property.serializedObject.FindProperty (path);

                isValid = arrayProperty.isArray;

                if (!isValid)
                {
                    Debug.LogWarning ("Array Elements useless as property is not an array!");
                }

                hasBeenValidated = true;
            }

            if (!isValid)
            {
                base.OnGUI (position, property, label);
                return;
            }

            //Find current index
            index = GetElementIndex (property);

            //Draw property and buttons
            EditorGUI.BeginChangeCheck ();
            {
                position.width -= BUTTON_SIZE * 2;

                EditorGUI.PropertyField (position, property, true);

                DrawButtonLabel (position, RemoveTex, "", () => arrayProperty.DeleteArrayElementAtIndex (index));
                position.x += BUTTON_SIZE;
                DrawButtonLabel (position, AddTex, "", () => arrayProperty.InsertArrayElementAtIndex (index + 1));
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

        private bool DrawButtonLabel(Rect rect, Texture2D texture, string label, System.Action action)
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
            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                if (arrayProperty.GetArrayElementAtIndex (i).displayName == property.displayName)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}