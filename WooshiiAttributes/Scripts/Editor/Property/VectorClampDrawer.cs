using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draws a clamped Vector2.
    /// </summary>
    [CustomPropertyDrawer (typeof (Vector2ClampAttribute))]
    public class Vector2ClampDrawer : WooshiiPropertyDrawer
    {
        private Vector2ClampAttribute Target => attribute as Vector2ClampAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            _label.text = _label.text + $" [{Target.Min}-{Target.Max}]";

            EditorGUI.BeginChangeCheck ();

            EditorGUI.PropertyField (_position, _property, _label, true);

            if (EditorGUI.EndChangeCheck ())
            {
                for (int i = 0; i < 2; i++)
                {
                    Target.Value[i] = Mathf.Clamp (_property.vector2Value[i], Target.Min, Target.Max);
                }

                _property.vector2Value = Target.Value;
            }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight (_property);
        }
    }

    /// <summary>
    /// Draws a clamped Vector3.
    /// </summary>
    [CustomPropertyDrawer (typeof (Vector3ClampAttribute))]
    public class Vector3ClampDrawer : WooshiiPropertyDrawer
    {
        private Vector3ClampAttribute Target => attribute as Vector3ClampAttribute;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            _label.text = _label.text + $" [{Target.Min}-{Target.Max}]";

            EditorGUI.BeginChangeCheck ();

            EditorGUI.PropertyField (_position, _property, _label, true);

            if (EditorGUI.EndChangeCheck ())
            {
                for (int i = 0; i < 3; i++)
                {
                    Target.value[i] = Mathf.Clamp (_property.vector3Value[i], Target.Min, Target.Max);
                }

                _property.vector3Value = Target.value;
            }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return EditorGUI.GetPropertyHeight (_property);
        }
    }
}