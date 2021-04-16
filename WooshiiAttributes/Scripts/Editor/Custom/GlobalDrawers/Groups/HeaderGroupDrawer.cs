using UnityEditor;

namespace WooshiiAttributes
{
    public class HeaderGroupDrawer : GlobalDrawer<HeaderGroupAttribute>
    {
        public HeaderGroupDrawer(SerializedObject _serializedObject, SerializedProperty _property) : base (_serializedObject, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            EditorGUILayout.LabelField (Attributes[0].Name, EditorStyles.boldLabel);

            for (int i = 0; i < Properties.Count; i++)
            {
                SerializedProperty property = Properties[i];
                HeaderGroupAttribute attribute = Attributes[i];

                EditorGUILayout.PropertyField (property);
            }
        }
    }
}