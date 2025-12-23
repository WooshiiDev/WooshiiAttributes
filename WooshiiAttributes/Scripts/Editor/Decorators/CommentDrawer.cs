using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    // Originally based off:
    //https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/NaughtyAttributes/Scripts/Editor/DecoratorDrawers/InfoBoxDecoratorDrawer.cs#L28
    [CustomPropertyDrawer (typeof (CommentAttribute))]
    internal class CommentDrawer : WooshiiDecoratorDrawer
    {
        private const float HEIGHT_PADDING = 4f;

        private CommentAttribute Target => attribute as CommentAttribute;

        public override void OnGUI(Rect rect)
        {
            float indent = GetIndentLength (rect);

            rect.Set (
                rect.x + indent, rect.y,
                rect.width - indent, GetBoxHeight () - HEIGHT_PADDING * 0.5f);

            MessageType messageType = MessageType.None;

            switch (Target.messageType)
            {
                case CommentAttribute.MessageType.WARNING:
                    messageType = MessageType.Warning;
                    break;

                case CommentAttribute.MessageType.INFO:
                    messageType = MessageType.Info;
                    break;

                case CommentAttribute.MessageType.ERROR:
                    messageType = MessageType.Error;
                    break;
            }

            EditorGUI.HelpBox (rect, Target.Text, messageType);
        }

        public static float GetIndentLength(Rect sourceRect)
        {
            Rect indentRect = EditorGUI.IndentedRect (sourceRect);
            float indentLength = indentRect.x - sourceRect.x;

            return indentLength;
        }

        //How tall the GUI is for this decorator
        public override float GetHeight()
        {
            return GetBoxHeight () + HEIGHT_PADDING;
        }

        private float GetBoxHeight()
        {
            float width = EditorGUIUtility.currentViewWidth;
            float minHeight = _singleLine * 2f;

            // Icon, Scrollbar, Indent
            if (Target.messageType != CommentAttribute.MessageType.NONE)
            {
                width -= 68;
            }

            //Need a little extra for correct sizing of InfoBox
            float actualHeight = EditorStyles.helpBox.CalcHeight (new GUIContent (Target.Text), width);
            return Mathf.Max (minHeight, actualHeight);
        }
    }
}