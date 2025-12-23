using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    [GUIDrawer(typeof(BeginGroupAttribute))]
    public class BeginGroupDrawer : GroupDrawer<BeginGroupAttribute>
    {
        private static GUIStyle s_groupStyle;

        public BeginGroupDrawer(BeginGroupAttribute attribute, SerializedObject serializedObject) : base (attribute, serializedObject)
        {

        }

        public override void OnGUI()
        {
            if (s_groupStyle == null)
            {
                s_groupStyle = EditorStyles.boldLabel;
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
            for (int i = 0; i < _drawers.Count; i++)
            {
                _drawers[i].OnGUI();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical ();
        }

        private void DrawHeader(string name)
        {
            EditorGUILayout.LabelField (name, s_groupStyle);

            if (attribute.TitleUnderlined)
            {
                Color color = s_groupStyle.normal.textColor;
                Rect rect = GUILayoutUtility.GetLastRect ();
                rect.y += rect.height - 1;

                WooshiiGUI.CreateLineSpacer (rect, color, 1);
            }
        }
    }
}