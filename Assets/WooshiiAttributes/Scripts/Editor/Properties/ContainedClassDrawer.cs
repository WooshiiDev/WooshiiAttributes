using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
    {

    [CustomPropertyDrawer (typeof (ContainedClassAttribute))]
    public class ContainedClassDrawer : WooshiiPropertyDrawer
        {
        private GUIStyle style = new GUIStyle (EditorStyles.boldLabel);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
            style.alignment = TextAnchor.UpperLeft;

            //============ Draw ============
            GUIStyle boxStyle = new GUIStyle (GUI.skin.window)
                {
                padding = new RectOffset (0, 0, 0, 0),
                };
            
            EditorGUILayout.BeginVertical (boxStyle);
                {
                property.isExpanded = EditorGUILayout.Foldout (property.isExpanded, " " + property.displayName, true);

                if (property.isExpanded)
                    {
                    EditorGUILayout.Space ();
                    DrawChildProperties (property);
                    EditorGUILayout.Space ();

                    }
                }
                
            EditorGUILayout.EndVertical ();
            }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
            return 0;
            }

        private void DrawChildProperties(SerializedProperty property)
            {
            SerializedProperty itr = property.Copy ();
            SerializedProperty current = itr.Copy();

            bool iterateChildrenTemp = true;
            while (itr.NextVisible (iterateChildrenTemp))
                {
                iterateChildrenTemp = false;

                if (itr.hasVisibleChildren)
                    iterateChildrenTemp = itr.isExpanded;

                //Return if end
                if (SerializedProperty.EqualContents (itr, property.GetEndProperty ()))
                    break;

                EditorGUILayout.PropertyField (itr);
                }
            }

        }
    }

