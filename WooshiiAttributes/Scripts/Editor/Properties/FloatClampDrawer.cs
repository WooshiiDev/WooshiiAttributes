using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (FloatClampAttribute))]
    public class FloatClampDrawer : WooshiiPropertyDrawer
    {
        private FloatClampAttribute Target => attribute as FloatClampAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Target.ShowClamp)
            {
                label.text += $" [{Target.Min} - {Target.Max}]";
            }

            EditorGUI.PropertyField (position, property, label);
            property.floatValue = Mathf.Clamp (property.floatValue, Target.Min, Target.Max);
        }
    }
}
