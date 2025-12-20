using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public static class SerializedUtility
    {
        private static readonly string[] excludedPropertyTypes =
        {
            "PPtr<MonoScript>",
            "ArraySize",
        };

        private static readonly string[] excludedPropertyNames =
        {
            "m_Script",
        };


        /// <summary>
        /// Get all visible properties of a serialized object
        /// </summary>
        /// <param name="_target">The target serialized object</param>
        /// <returns>Returns a list of public serialized properties</returns>
        public static List<SerializedProperty> GetAllVisibleProperties(SerializedObject _target)
        {
            List<SerializedProperty> properties = new List<SerializedProperty> ();

            using (SerializedProperty iterator = _target.GetIterator ())
            {
                if (iterator.NextVisible (true))
                {
                    do
                    {
                        string name = iterator.name;
                        string type = iterator.type;

                        if (excludedPropertyNames.Contains (name))
                        {
                            continue;
                        }

                        if (excludedPropertyTypes.Contains (type))
                        {
                            continue;
                        }

                        properties.Add (_target.FindProperty (iterator.name));
                    }
                    while (iterator.NextVisible (false));
                }
            }

            return properties;
        }

        /// <summary>
        /// Draw a <see cref="SerializedProperty"/> that will automatically apply changes 
        /// </summary>
        /// <param name="_property">The property to draw</param>
        /// <param name="_showChildren">Should children of this property been shown</param>
        /// <param name="_options">Optional layout options</param>
        public static void AutoProperty(SerializedProperty _property, bool _showChildren, params EditorGUILayout[] _options)
        {
            EditorGUI.BeginChangeCheck ();

            EditorGUILayout.PropertyField (_property, _showChildren);

            if (EditorGUI.EndChangeCheck())
            {
                _property.serializedObject.ApplyModifiedProperties ();
            }
        }
    }
}
