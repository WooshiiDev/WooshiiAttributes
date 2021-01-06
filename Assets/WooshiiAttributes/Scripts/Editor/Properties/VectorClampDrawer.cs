using System;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {
    [CustomPropertyDrawer (typeof (Vector2ClampAttribute))]
    public class Vector2ClampDrawer : WooshiiPropertyDrawer
        {
        private Vector2ClampAttribute Target => attribute as Vector2ClampAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
            label.text = label.text + $" [{Target.min}-{Target.max}]";

            EditorGUI.BeginChangeCheck ();

            EditorGUI.PropertyField (position, property, label, true);

            if (EditorGUI.EndChangeCheck())
                {
                for (int i = 0; i < 2; i ++)
                    Target.value[i] = Mathf.Clamp (property.vector2Value[i], Target.min, Target.max);

                property.vector2Value = Target.value;
                }
            }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
            return EditorGUI.GetPropertyHeight (property);
            }
        }

    [CustomPropertyDrawer (typeof (Vector3ClampAttribute))]
    public class Vector3ClampDrawer : WooshiiPropertyDrawer
        {
        private Vector3ClampAttribute Target => attribute as Vector3ClampAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
            label.text = label.text + $" [{Target.min}-{Target.max}]";

            EditorGUI.BeginChangeCheck ();

            EditorGUI.PropertyField (position, property, label, true);

            if (EditorGUI.EndChangeCheck ())
                {
                for (int i = 0; i < 3; i ++)
                    Target.value[i] = Mathf.Clamp (property.vector3Value[i], Target.min, Target.max);

                property.vector3Value = Target.value;
                }
            }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
            return EditorGUI.GetPropertyHeight (property);
            }
        }
    }
