using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public class HeaderGroupDrawer : GlobalDrawer<HeaderGroupAttribute>
    {
        public HeaderGroupDrawer(SerializedObject serializedObject, SerializedProperty property) : base (serializedObject, property)
        {

        }

        protected override void OnGUI_Internal()
        {
            EditorGUILayout.LabelField (Attributes[0].Name, EditorStyles.boldLabel);

            for (int i = 0; i < Properties.Count; i++)
            {
                var property = Properties[i];
                var attribute = Attributes[i];

                EditorGUILayout.PropertyField (property);

            }
        }
    }
}
