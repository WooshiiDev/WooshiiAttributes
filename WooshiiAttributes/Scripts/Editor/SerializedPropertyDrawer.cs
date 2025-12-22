using UnityEditor;

namespace WooshiiAttributes
{
    public class SerializedPropertyDrawer : GUIDrawerBase<SerializedProperty>
    {
        public SerializedPropertyDrawer(SerializedProperty target) : base(target) { }

        public override void OnGUI()
        {
            EditorGUILayout.PropertyField(target);
        }
    }
}