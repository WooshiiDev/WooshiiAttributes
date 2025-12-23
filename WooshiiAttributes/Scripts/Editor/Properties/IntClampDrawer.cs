using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Clamps an numerical control to a given int range. Must be a supported type.
    /// </summary>
    [CustomPropertyDrawer (typeof (IntClampAttribute))]
    public class IntClampDrawer : WooshiiPropertyDrawer
    {
        private IntClampAttribute Target => attribute as IntClampAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Target.ShowClamp)
            {
                label.text += $" [{Target.Min} - {Target.Max}]";
            }

            EditorGUI.PropertyField (position, property, label);
            property.intValue = Mathf.Clamp (property.intValue, Target.Min, Target.Max);
        }
    }
}
