using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draw an underlined header.
    /// </summary>
    [CustomPropertyDrawer (typeof (HeaderLineAttribute))]
    public class HeaderLineDrawer : WooshiiDecoratorDrawer
    {
        // - Properties

        private GUIStyle _style = new GUIStyle (EditorStyles.boldLabel);
        private HeaderLineAttribute Target => attribute as HeaderLineAttribute;

        // - Methods

        public override void OnGUI(Rect rect)
        {
            //Draw label
            if (!string.IsNullOrWhiteSpace (Target.Text))
            {
                EditorGUI.LabelField (rect, Target.Text.ToUpper (), _style);

                //Move to new line and set following line height
                rect.y += _singleLine + 1;
                rect.height = 1;
            }
            else
            {
                rect.y += _singleLine / 2f + 1;
                rect.height = 1;
            }

            Color c = Color.gray;
            if (EditorGUIUtility.isProSkin)
            {
                c = _style.normal.textColor;
            }

            WooshiiGUI.CreateLineSpacer (EditorGUI.IndentedRect (rect), c, rect.height);
        }

        public override float GetHeight()
        {
            return _singleLine * 1.25f;
        }
    }
}