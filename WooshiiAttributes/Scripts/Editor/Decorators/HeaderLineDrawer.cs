using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (HeaderLineAttribute))]
    public class HeaderLineDrawer : WooshiiDecoratorDrawer
    {
        private GUIStyle _style = new GUIStyle (EditorStyles.boldLabel);
        private HeaderLineAttribute Target => attribute as HeaderLineAttribute;

        public override void OnGUI(Rect rect)
        {
            //Draw label
            if (!string.IsNullOrWhiteSpace (Target.Text))
            {
                EditorGUI.LabelField (rect, Target.Text.ToUpper (), _style);

                //Move to new line and set following line height
                rect.y += singleLine + 1;
                rect.height = 1;
            }
            else
            {
                rect.y += singleLine / 2f + 1;
                rect.height = 1;
            }

            Color c = Color.gray;
            if (EditorGUIUtility.isProSkin)
            {
                c = _style.normal.textColor;
            }

            //Draw spacer
            WooshiiGUI.CreateLineSpacer (EditorGUI.IndentedRect (rect), c, rect.height);
        }

        //How tall the GUI is for this decorator
        public override float GetHeight()
        {
            return singleLine * 1.25f;
        }
    }
}