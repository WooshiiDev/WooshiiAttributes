using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (IntClampAttribute))]
    public class IntClampDrawer : WooshiiPropertyDrawer
    {
        private IntClampAttribute Target => attribute as IntClampAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (Target.ShowClamp)
            {
                _label.text += $" [{Target.Min} - {Target.Max}]";
            }

            EditorGUI.PropertyField (_position, _property, _label);
            _property.intValue = Mathf.Clamp (_property.intValue, Target.Min, Target.Max);
        }
    }
}
