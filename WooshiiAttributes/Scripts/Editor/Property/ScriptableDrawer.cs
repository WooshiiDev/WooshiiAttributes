using Boo.Lang;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draws a clamped Vector2.
    /// </summary>
    [CustomPropertyDrawer(typeof(ExposedScriptableAttribute), true)]
    public class ScriptableDrawer : WooshiiPropertyDrawer
    {
        private ScriptableObject currentScriptable;
        private SerializedObject currentObject;
        private List<SerializedProperty> properties = new List<SerializedProperty>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (currentScriptable == null)
            {
                base.OnGUI(position, property, label);
                return;
            }

            EditorGUI.BeginChangeCheck();
            base.OnGUI(GetHeaderRect(position), property, label);
            property.isExpanded = EditorGUI.Foldout(GetHeaderRect(position), property.isExpanded, GUIContent.none, true);
            if (property.isExpanded)
            {
                Rect background = position;
                background.y += SingleLineHeight;
                background.height -= SingleLineHeight;
                DrawBackground(background);
                
                position.y += SingleLineHeight + StandardSpacing * 2;

                EditorGUI.indentLevel++;
                foreach (SerializedProperty child in properties)
                {
                    position.height = EditorGUI.GetPropertyHeight(child, child.isExpanded);
                    EditorGUI.PropertyField(position, child, new GUIContent(child.displayName));
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.indentLevel--;
            }
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
        {
            if (!(_property.objectReferenceValue is ScriptableObject scriptable))
            {
                currentScriptable = null;
                return base.GetPropertyHeight(_property, _label);
            }

            currentScriptable = scriptable;
            currentObject = new SerializedObject(currentScriptable);

            if (!_property.isExpanded)
            {
                return base.GetPropertyHeight(_property, _label);
            }

            float height = 0;

            properties.Clear();
            foreach (SerializedProperty child in SerializedUtility.GetAllVisibleProperties(currentObject))
            {
                height += base.GetPropertyHeight(child, _label);
                properties.Add(child);
            }

            return base.GetPropertyHeight(_property, _label) + height;
        }

        private Rect GetHeaderRect(Rect rect)
        {
            rect.height = this.SingleLineHeight;
            return rect;
        }
    }
}