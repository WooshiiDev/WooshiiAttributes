using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {
    public class HeaderLineGroupDrawer : GlobalDrawer
        {
        public List<string> names;
        private GUIStyle style;

        public HeaderLineGroupDrawer() : base (typeof (HeaderLineGroupAttribute))
            {
            names = new List<string> ();
            }

        public override void Initalize(SerializedObject serializedObject)
            {
            base.Initalize (serializedObject);

            for (int i = 0; i < Attributes.Count; i++)
                {
                if (!(Attributes[i] is HeaderLineGroupAttribute groupAttribute))
                    continue;

                if (!names.Contains (groupAttribute.Name))
                    names.Add (groupAttribute.Name);
                }
            }

        protected override void OnGUI_Internal()
            {
            if (style == null)
                style = new GUIStyle (EditorStyles.boldLabel);

            for (int i = 0; i < names.Count; i++)
                {
                string name = names[i];
                EditorGUILayout.LabelField (name.ToUpper(), style);

                Color color = style.normal.textColor;
                Rect rect = GUILayoutUtility.GetLastRect ();
                rect.y += rect.height - 1;

                GUIExtension.CreateLineSpacer (rect, color, 1);

                for (int j = 0; j < Attributes.Count; j++)
                    {
                    if (!(Attributes[j] is HeaderLineGroupAttribute groupAttribute))
                        continue;

                    if (groupAttribute.Name == name)
                        EditorGUILayout.PropertyField (Properties[j]);
                    }
                }
            }
        }
    }
