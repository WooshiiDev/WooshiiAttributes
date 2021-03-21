using System;
using UnityEditor;

namespace WooshiiAttributes
{
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