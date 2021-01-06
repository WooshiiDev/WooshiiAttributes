using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
    {
    public class HeaderGroupDrawer : GlobalDrawer
        {
        private List<string> names;

        public HeaderGroupDrawer() : base (typeof(HeaderGroupAttribute))
            {
            names = new List<string> ();
            }

        public override void Initalize(SerializedObject serializedObject)
            {
            base.Initalize (serializedObject);

            for (int i = 0; i < Attributes.Count; i++)
                {
                HeaderGroupAttribute groupAttribute = Attributes[i] as HeaderGroupAttribute;

                if (!names.Contains (groupAttribute.Name))
                    names.Add (groupAttribute.Name);
                }
            }

        protected override void OnGUI_Internal()
            {
            for (int i = 0; i < names.Count; i ++)
                {
                string name = names[i];
                EditorGUILayout.LabelField (name, EditorStyles.boldLabel);

                for (int j = 0; j < Attributes.Count; j++)
                    {
                    HeaderGroupAttribute groupAttribute = Attributes[j] as HeaderGroupAttribute;

                    if (groupAttribute.Name == name)
                        EditorGUILayout.PropertyField (Properties[j]);
                    }
                }

            }
        }
    }
