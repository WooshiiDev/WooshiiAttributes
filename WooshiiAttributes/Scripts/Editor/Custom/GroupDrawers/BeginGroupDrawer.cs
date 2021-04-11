using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class BeginGroupDrawer : GroupDrawer<BeginGroupAttribute>
    {
        private static GUIStyle style;

        public BeginGroupDrawer(BeginGroupAttribute attribute, SerializedObject serializedObject) : base (attribute, serializedObject)
        {

        }

        public override void OnGUI()
        {
            if (style == null)
            {
                style = EditorStyles.boldLabel;
            }

            bool isTitleGrouped = attribute.TitleGrouped;
            string titleName = (attribute.TitleUpper) ? attribute.GroupName.ToUpper() : attribute.GroupName;

            if (!isTitleGrouped)
            {
                DrawHeader (titleName);
            }

            EditorGUILayout.BeginVertical (EditorStyles.helpBox);

            if (isTitleGrouped)
            {
                DrawHeader (titleName);
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < properties.Count; i++)
            {
                SerializedProperty property = properties[i];
                EditorGUILayout.PropertyField (property, true);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical ();
        }

        private void DrawHeader(string name)
        {
            EditorGUILayout.LabelField (name, style);

            if (attribute.TitleUnderlined)
            {
                Color color = style.normal.textColor;
                Rect rect = GUILayoutUtility.GetLastRect ();
                rect.y += rect.height - 1;

                GUIExtension.CreateLineSpacer (rect, color, 1);
            }
        }
    }
}