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
        private const float HEADER_HEIGHT = 23F;
        private static GUIStyle GroupStyle;

        private bool m_needsIndent;

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
               // DrawHeader (titleName);
            }

            //EditorGUILayout.BeginVertical (EditorStyles.helpBox);
            InspectorGUI.BeginContainer (GetTotalHeight ());

            if (isTitleGrouped)
            {
               DrawHeader (titleName);
            }

            for (int i = 0; i < m_properties.Count; i++)
            {
                SerializedProperty property = m_properties[i];
                EditorGUILayout.PropertyField (property, property.isExpanded);
            }

            InspectorGUI.EndInspectorContainer ();

            GUILayout.Space (3f);
        }

        private void DrawHeader(string _name)
        {
            EditorGUILayout.LabelField (_name, GroupStyle);

            if (attribute.TitleUnderlined)
            {
                Color color = new Color(0.4f, 0.4f, 0.4f);
                Rect rect = GUILayoutUtility.GetLastRect ();

                rect.x -= 4f;
                rect.width += 8f;
                rect.y += rect.height + 2f;

                WooshiiGUI.CreateLineSpacer (rect, color, 1);
            }

            GUILayout.Space (3f);
        }

        private float GetTotalHeight()
        {
            float height = 0;

            for (int i = 0; i < m_properties.Count; i++)
            {
                height += EditorGUI.GetPropertyHeight (m_properties[i], true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height + EditorGUIUtility.standardVerticalSpacing + HEADER_HEIGHT;
        }
    }
}