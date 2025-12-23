using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace WooshiiAttributes
{
    /// <summary>
    /// Utility methods to help collect serialized data.
    /// </summary>
    public static class SerializedUtility
    {
        /// <summary>
        /// Property values to ignore when collecting properties.
        /// </summary>
        private static readonly string[] s_excludedPropertyTypes =
        {
            "PPtr<MonoScript>",
            "ArraySize",
        };

        /// <summary>
        /// Property names to ignore when collection properties.
        /// </summary>
        private static readonly string[] s_excludedPropertyNames =
        {
            "m_Script",
        };


        /// <summary>
        /// Get all visible properties of a serialized object.
        /// </summary>
        /// <param name="target">The target serialized object.</param>
        /// <returns>Returns a list of public serialized properties.</returns>
        public static List<SerializedProperty> GetAllVisibleProperties(SerializedObject target)
        {
            List<SerializedProperty> properties = new List<SerializedProperty> ();

            using (SerializedProperty iterator = target.GetIterator ())
            {
                if (iterator.NextVisible (true))
                {
                    do
                    {
                        string name = iterator.name;
                        string type = iterator.type;

                        if (s_excludedPropertyNames.Contains (name))
                        {
                            continue;
                        }

                        if (s_excludedPropertyTypes.Contains (type))
                        {
                            continue;
                        }

                        properties.Add (target.FindProperty (iterator.name));
                    }
                    while (iterator.NextVisible (false));
                }
            }

            return properties;
        }

        /// <summary>
        /// Draw a <see cref="SerializedProperty"/> that will automatically apply changes .
        /// </summary>
        /// <param name="property">The property to draw.</param>
        /// <param name="showChildren">Should children of this property been shown.</param>
        /// <param name="options">Optional layout options.</param>
        public static void AutoProperty(SerializedProperty property, bool showChildren, params EditorGUILayout[] options)
        {
            EditorGUI.BeginChangeCheck ();

            EditorGUILayout.PropertyField (property, showChildren);

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties ();
            }
        }
    }
}
