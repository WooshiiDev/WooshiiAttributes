using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draws a slider for a float range.
    /// </summary>
    [CustomPropertyDrawer (typeof (FloatSliderAttribute))]
    public class FloatSliderDrawer : WooshiiPropertyDrawer
    {
        private FloatSliderAttribute Target => attribute as FloatSliderAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.Slider (position, property, Target.Min, Target.Max, label.text);
        }
    }
}
