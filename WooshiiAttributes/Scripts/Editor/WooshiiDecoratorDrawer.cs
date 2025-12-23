using UnityEditor;

namespace WooshiiAttributes
{
    /// <summary>
    /// Base class for custom decorator drawers.
    /// </summary>
    public class WooshiiDecoratorDrawer : DecoratorDrawer
    {
        //Cached
        protected float _singleLine = EditorGUIUtility.singleLineHeight;
    }
}