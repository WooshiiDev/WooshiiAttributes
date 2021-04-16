using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    [CustomPropertyDrawer (typeof (ContainedClassAttribute))]
    public class ContainedClassDrawer : WooshiiPropertyDrawer
    {
        private static GUIStyle m_style = new GUIStyle (EditorStyles.boldLabel);

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            m_style.alignment = TextAnchor.UpperLeft;

            //============ Draw ============
            GUIStyle boxStyle = new GUIStyle (GUI.skin.window)
            {
                padding = new RectOffset (0, 0, 0, 0),
            };

            EditorGUILayout.BeginVertical (boxStyle);
            {
                _property.isExpanded = EditorGUILayout.Foldout (_property.isExpanded, " " + _property.displayName, true);

                if (_property.isExpanded)
                {
                    EditorGUILayout.Space ();
                    DrawChildProperties (_property);
                    EditorGUILayout.Space ();
                }
            }

            EditorGUILayout.EndVertical ();
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            return 0;
        }

        private void DrawChildProperties(SerializedProperty _property)
        {
            SerializedProperty itr = _property.Copy ();
            SerializedProperty current = itr.Copy ();

            bool iterateChildrenTemp = true;
            while (itr.NextVisible (iterateChildrenTemp))
            {
                iterateChildrenTemp = false;

                if (itr.hasVisibleChildren)
                {
                    iterateChildrenTemp = itr.isExpanded;
                }

                //Return if end
                if (SerializedProperty.EqualContents (itr, _property.GetEndProperty ()))
                {
                    break;
                }

                EditorGUILayout.PropertyField (itr);
            }
        }
    }
}