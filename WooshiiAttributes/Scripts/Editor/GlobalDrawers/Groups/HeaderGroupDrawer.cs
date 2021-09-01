using UnityEditor;

namespace WooshiiAttributes
{
    public class HeaderGroupDrawer : GlobalDrawer<HeaderGroupAttribute>
    {
        private HeaderGroupAttribute Header => Attributes[0] as HeaderGroupAttribute;

        public HeaderGroupDrawer(SerializedObject _serializedObject, SerializedProperty _property) : base (_serializedObject, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            EditorGUILayout.LabelField (Header.Name, EditorStyles.boldLabel);

            for (int i = 0; i < Properties.Count; i++)
            {
                SerializedProperty property = Properties[i];
                EditorGUILayout.PropertyField (property);
            }
        }
    }
}