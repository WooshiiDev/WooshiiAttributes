using UnityEditor;

namespace WooshiiAttributes
{
    public class ContainedGroupDrawer : GlobalDrawer<ContainedGroupAttribute>
    {
        public ContainedGroupDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            string name = Attributes[0].Name;
            EditorGUILayout.BeginVertical (EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField (name, EditorStyles.boldLabel);

                for (int j = 0; j < Attributes.Count; j++)
                {
                    ContainedGroupAttribute groupAttribute = Attributes[j] as ContainedGroupAttribute;

                    if (groupAttribute.Name == name)
                    {
                        EditorGUILayout.PropertyField (Properties[j]);
                    }
                }
            }
            EditorGUILayout.EndVertical ();
        }
    }
}