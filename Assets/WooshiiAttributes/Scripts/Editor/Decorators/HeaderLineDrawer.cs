using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {
    [CustomPropertyDrawer (typeof (HeaderLineAttribute))]
    public class HeaderLineDrawer : WooshiiDecoratorDrawer
        {
        private HeaderLineAttribute header => attribute as HeaderLineAttribute;
        private GUIStyle style = new GUIStyle (EditorStyles.boldLabel);

        public override void OnGUI(Rect rect)
            {
            //Draw label
            if (!string.IsNullOrWhiteSpace(header.text))
                {
                EditorGUI.LabelField (rect, header.text.ToUpper (), style);

                //Move to new line and set following line height
                rect.y += SingleLine + 1;
                rect.height = 1;
                }
            else
                {
                rect.y += SingleLine / 2f + 1;
                rect.height = 1;
                }

            Color c = Color.gray;
            if (EditorGUIUtility.isProSkin)
                c = style.normal.textColor;

            //Draw spacer
            GUIExtension.CreateLineSpacer (EditorGUI.IndentedRect (rect), c, rect.height);
            }

        //How tall the GUI is for this decorator
        public override float GetHeight()
            {
            return SingleLine * 1.25f;
            }

        }
    }

