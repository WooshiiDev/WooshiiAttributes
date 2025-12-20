using System;
using UnityEditor;

namespace WooshiiAttributes
{    
    public abstract class GUIDrawerBase
    {
        public virtual void Initialise() { }
        public abstract void OnGUI();
    }

    public abstract class GUIDrawerBase<T> : GUIDrawerBase
    {
        /// <summary>
        /// The target object to draw.
        /// </summary>
        protected T target;

        public GUIDrawerBase(T target)
        {
            this.target = target;
        }
    }

    public class SerializedPropertyDrawer : GUIDrawerBase<SerializedProperty>
    {
        public SerializedPropertyDrawer(SerializedProperty target) : base(target) { }

        public override void OnGUI()
        {
            EditorGUILayout.PropertyField(target);
        }
    }

    public class GUIDrawerAttribute : Attribute
    {
        /// <summary>
        /// The target GUIElement.
        /// </summary>
        public Type Element { get; }

        public GUIDrawerAttribute(Type element)
        {
            Element = element;
        }
    }

    public class GUIElementAttribute : Attribute
    {

    }

    public interface ICustomPropertyDrawer
    {
        Type AttributeType { get; }
        SerializedObject SerializedObject { get; }
        SerializedProperty SerializedProperty { get; }

        void OnGUI();
    }

    public interface ICustomPropertyDrawer<T> : ICustomPropertyDrawer
    {
        T Attribute { get; }
    }
}