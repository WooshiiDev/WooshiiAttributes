using UnityEditor;

namespace WooshiiAttributes
{
    /// <summary>
    /// Draws a <see cref="SerializedProperty"/>.
    /// </summary>
    public class SerializedPropertyDrawer : GUIDrawerBase<SerializedProperty>
    {
        public SerializedPropertyDrawer(SerializedProperty target) : base(target) { }

        public override void OnGUI()
        {
            EditorGUILayout.PropertyField(_data);
        }
    }
}