using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    interface IGroupDrawer
    {
        SerializedObject SerializedObject { get; }
        Type AttributeType { get; }
        List<SerializedProperty> Properties { get; }

        void OnGUI();
        void RegisterProperty(SerializedProperty _property);
    }

}
