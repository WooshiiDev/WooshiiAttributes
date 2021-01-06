using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {
    public class FoldoutGroupDrawer : GlobalDrawer
        {
        private bool shown;
        private List<string> names;

        private GUIStyle helpStyle;

        public FoldoutGroupDrawer() : base (typeof (FoldoutGroupAttribute))
            {
            names = new List<string> ();
            }

        public override void Initalize(SerializedObject serializedObject)
            {
            base.Initalize (serializedObject);

            for (int i = 0; i < Attributes.Count; i++)
                {
                FoldoutGroupAttribute groupAttribute = Attributes[i] as FoldoutGroupAttribute;

                if (!names.Contains (groupAttribute.Name))
                    names.Add (groupAttribute.Name);
                }
            }

        protected override void OnGUI_Internal()
            {
            if (helpStyle == null)
                {
                helpStyle = new GUIStyle (EditorStyles.boldLabel)
                    {
                    fontStyle = FontStyle.Bold
                    };
                }

            for (int i = 0; i < names.Count; i++)
                {
                string name = names[i];
          
                if (shown = EditorGUILayout.Foldout (shown, name, true))
                    {
                    EditorGUI.indentLevel++;
                    for (int j = 0; j < Attributes.Count; j++)
                        {
                        FoldoutGroupAttribute groupAttribute = Attributes[j] as FoldoutGroupAttribute;

                        if (groupAttribute.Name == name)
                            EditorGUILayout.PropertyField (Properties[j]);
                        }
                    EditorGUI.indentLevel--;
                    }
                }
            }
        }
    }
