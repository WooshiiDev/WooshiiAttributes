using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
    {
    public class ContainedGroupDrawer : GlobalDrawer
        {
        private List<string> names;

        public ContainedGroupDrawer() : base (typeof (ContainedGroupAttribute))
            {
            names = new List<string> ();
            }

        public override void Initalize(SerializedObject serializedObject)
            {
            base.Initalize (serializedObject);

            for (int i = 0; i < Attributes.Count; i++)
                {
                ContainedGroupAttribute groupAttribute = Attributes[i] as ContainedGroupAttribute;

                if (!names.Contains (groupAttribute.Name))
                    names.Add (groupAttribute.Name);
                }
            }

        protected override void OnGUI_Internal()
            {
            for (int i = 0; i < names.Count; i++)
                {
                string name = names[i];
                EditorGUILayout.BeginVertical (EditorStyles.helpBox);
                    {
                    EditorGUILayout.LabelField (name, EditorStyles.boldLabel);

                    for (int j = 0; j < Attributes.Count; j++)
                        {
                        ContainedGroupAttribute groupAttribute = Attributes[j] as ContainedGroupAttribute;

                        if (groupAttribute.Name == name)
                            EditorGUILayout.PropertyField (Properties[j]);
                        }
                    }
                EditorGUILayout.EndVertical ();
                }

            }
        }
    }
