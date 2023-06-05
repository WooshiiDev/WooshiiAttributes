using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    // -- New --

    [RegisterDrawer]
    public class GroupGUIDrawer : GUIDrawer<GroupAttribute>
    {
        private const float HEADER_HEIGHT = 22f;
        private static GUIStyle GroupStyle;

        private bool m_needsIndent;

        public GroupGUIDrawer(GroupAttribute attribute) : base(attribute) { }

        public override void OnGUI()
        {
            if (GroupStyle == null)
            {
                GroupStyle = EditorStyles.boldLabel;
            }

            bool isTitleGrouped = Attribute.TitleGrouped;
            string titleName = (Attribute.TitleUpper) ? Attribute.GroupName.ToUpper() : Attribute.GroupName;

            if (!isTitleGrouped)
            {
                DrawHeader(titleName);
                InspectorGUI.BeginContainer(GetTotalHeight());
            }
            else
            {
                InspectorGUI.BeginContainer(GetTotalHeight());
                DrawHeader(titleName);
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < properties.Count; i++)
            {
                SerializedProperty property = properties[i].SerializedValue;
                EditorGUILayout.PropertyField(property, new GUIContent(property.displayName), property.isExpanded);
            }
            EditorGUI.indentLevel--;

            InspectorGUI.EndInspectorContainer();

            // Make sure there's decent spacing

            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing * 2f);
        }

        private void DrawHeader(string _name)
        {
            EditorGUILayout.LabelField(_name, GroupStyle);

            if (Attribute.TitleUnderlined)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                rect.y += EditorGUIUtility.singleLineHeight - 2f;

                WooshiiGUI.CreateLineSpacer(rect, EditorStyles.boldLabel.normal.textColor, 1f);
            }
        }

        private float GetTotalHeight()
        {
            float height = 0;

            for (int i = 0; i < properties.Count; i++)
            {
                height += EditorGUI.GetPropertyHeight(properties[i].SerializedValue, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height + HEADER_HEIGHT;
        }
    }
}
