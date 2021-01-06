using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace WooshiiAttributes
    {
    [CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
    public class ReadOnlyDrawer : WooshiiPropertyDrawer
        {
        private ReadOnlyAttribute target;
        private bool canShow;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
            target = attribute as ReadOnlyAttribute;

            switch (target.displayMode)
                {
                case DisplayMode.EDITOR:
                    canShow = !EditorApplication.isPlaying;
                    break;

                case DisplayMode.PLAYING:
                    canShow = EditorApplication.isPlaying;
                    break;

                case DisplayMode.BOTH:
                    canShow = true;
                    break;

                default:
                    break;
                }

            if (canShow)
                {
                //EditorGUI.BeginProperty (position, label, property);

                GUI.enabled = false;
                //EditorGUI.PropertyField (position, property, label, true);
                EditorGUILayout.PropertyField (property, label, true);
                GUI.enabled = true;

                //EditorGUI.EndProperty ();
                }
            }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
            //if (canShow)
            //    return base.GetPropertyHeight (property, label) * 10;
            //else
            //    return 0;

            return 0;
            }
        }
    }

