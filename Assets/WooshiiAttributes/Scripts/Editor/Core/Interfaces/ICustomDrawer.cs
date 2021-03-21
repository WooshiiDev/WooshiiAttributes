using System;
using UnityEditor;

namespace WooshiiAttributes
{
    public interface ICustomDrawer
    {
        Type AttributeType { get; }
        SerializedObject SerializedObject { get; }
        SerializedProperty SerializedProperty { get; }

        void OnGUI();
    }

    public interface ICustomDrawer<T> : ICustomDrawer
    {
        T Attribute { get; }
    }
}