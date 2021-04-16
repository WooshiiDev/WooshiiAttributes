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

        public override void OnGUI(Rect _rect)
        {
            float indent = GetIndentLength (_rect);

            _rect.Set (
                _rect.x + indent, _rect.y,
                _rect.width - indent, GetBoxHeight () - HEIGHT_PADDING * 0.5f);

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

            EditorGUI.HelpBox (_rect, Target.text, messageType);
        }

        public static float GetIndentLength(Rect _sourceRect)
        {
            Rect indentRect = EditorGUI.IndentedRect (_sourceRect);
            float indentLength = indentRect.x - _sourceRect.x;

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
            float minHeight = singleLine * 2f;

            // Icon, Scrollbar, Indent
            if (Target.messageType != CommentAttribute.MessageType.NONE)
            {
                width -= 68;
            }

            //Need a little extra for correct sizing of InfoBox
            float actualHeight = EditorStyles.helpBox.CalcHeight (new GUIContent (Target.text), width);
            return Mathf.Max (minHeight, actualHeight);
        }
    }
}