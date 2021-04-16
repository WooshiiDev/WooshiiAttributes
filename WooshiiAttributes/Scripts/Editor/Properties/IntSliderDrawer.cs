using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (IntSliderAttribute))]
    public class IntSliderDrawer : WooshiiPropertyDrawer
    {
        private IntSliderAttribute Target => attribute as IntSliderAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.IntSlider (_position, _property, Target.Min, Target.Max, _label.text);
        }
    }
}
