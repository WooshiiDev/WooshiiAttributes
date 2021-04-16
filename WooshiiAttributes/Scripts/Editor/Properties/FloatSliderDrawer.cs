using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (FloatSliderAttribute))]
    public class FloatSliderDrawer : WooshiiPropertyDrawer
    {
        private FloatSliderAttribute Target => attribute as FloatSliderAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.Slider (_position, _property, Target.Min, Target.Max, _label.text);
        }
    }
}
