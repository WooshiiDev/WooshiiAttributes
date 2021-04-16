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
        private static GUIStyle GroupStyle;

        public BeginGroupDrawer(BeginGroupAttribute _attribute, SerializedObject _serializedObject) : base (_attribute, _serializedObject)
        {

        }

        public override void OnGUI()
        {
            if (GroupStyle == null)
            {
                GroupStyle = EditorStyles.boldLabel;
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
            for (int i = 0; i < m_properties.Count; i++)
            {
                SerializedProperty property = m_properties[i];
                EditorGUILayout.PropertyField (property, true);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical ();
        }

        private void DrawHeader(string _name)
        {
            EditorGUILayout.LabelField (_name, GroupStyle);

            if (attribute.TitleUnderlined)
            {
                Color color = GroupStyle.normal.textColor;
                Rect rect = GUILayoutUtility.GetLastRect ();
                rect.y += rect.height - 1;

                WooshiiGUI.CreateLineSpacer (rect, color, 1);
            }
        }
    }
}