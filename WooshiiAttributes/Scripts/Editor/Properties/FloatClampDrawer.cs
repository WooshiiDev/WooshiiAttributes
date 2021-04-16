using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (FloatClampAttribute))]
    public class FloatClampDrawer : WooshiiPropertyDrawer
    {
        private FloatClampAttribute Target => attribute as FloatClampAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (Target.ShowClamp)
            {
                _label.text += $" [{Target.Min} - {Target.Max}]";
            }

            EditorGUI.PropertyField (_position, _property, _label);
            _property.floatValue = Mathf.Clamp (_property.floatValue, Target.Min, Target.Max);
        }
    }
}
