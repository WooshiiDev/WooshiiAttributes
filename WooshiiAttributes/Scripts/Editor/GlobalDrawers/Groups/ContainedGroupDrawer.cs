using UnityEditor;

namespace WooshiiAttributes
{
    public class ContainedGroupDrawer : GlobalDrawer<ContainedGroupAttribute>
    {
        private ContainedGroupAttribute Header => Attributes[0] as ContainedGroupAttribute;

        public ContainedGroupDrawer(SerializedObject _parent, SerializedProperty _property) : base (_parent, _property)
        {
        }

        protected override void OnGUI_Internal()
        {
            string name = Header.Name;
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