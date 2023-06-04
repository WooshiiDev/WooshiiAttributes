using System;
using System.Reflection;
using UnityEditor;

namespace WooshiiAttributes
{
    /// <summary>
    /// Representation of an exposed field.
    /// </summary>
    public class GUIProperty
    {
        /// <summary>
        /// The original SerializedProperty that this GUIProperty is connected to.
        /// </summary>
        public readonly SerializedProperty SerializedValue;

        /// <summary>
        /// The FieldInfo for this property.
        /// </summary>
        public readonly FieldInfo Field;

        /// <summary>
        /// Create a new property instance, providing the <see cref="SerializedProperty"/> it represents.
        /// </summary>
        /// <param name="property">The SerializedProperty this object represents.</param>
        public GUIProperty(SerializedProperty property)
        {
            SerializedValue = property;
            Field = GetField();
        }

        /// <summary>
        /// Get the field from the serialized property for this instance.
        /// </summary>
        /// <returns>Returns the field for the serialized property.</returns>
        private FieldInfo GetField()
        {
            Type parent = SerializedValue.serializedObject.targetObject.GetType();
            return ReflectionUtility.GetField(parent, SerializedValue.name);
        }
    }
}