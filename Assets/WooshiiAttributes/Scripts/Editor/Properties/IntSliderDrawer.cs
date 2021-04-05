using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (IntSliderAttribute))]
    public class IntSliderDrawer : WooshiiPropertyDrawer
    {
        private IntSliderAttribute Target => attribute as IntSliderAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.IntSlider (position, property, Target.Min, Target.Max, label.text);
        }
    }
}
