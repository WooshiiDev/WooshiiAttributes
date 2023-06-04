using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    /// <summary>
    /// A collection of <see cref="GUIProperty"/>'s that are then drawn to the inspector.
    /// </summary>
    public class GUIDrawer
    {
        // - Fields

        /// <summary>
        /// The properties this drawer represents.
        /// </summary>
        protected List<GUIProperty> properties = new List<GUIProperty>();

        // - GUI

        /// <summary>
        /// Draw the GUI.
        /// </summary>
        public virtual void OnGUI()
        {
            for (int i = 0; i < properties.Count; i++)
            {
                GUIProperty property = properties[i];
                EditorGUILayout.PropertyField(property.SerializedValue);
            }
        }

        // - Elements

        /// <summary>
        /// Add a property to this drawer.
        /// </summary>
        /// <param name="property">The property to add.</param>
        public void Add(GUIProperty property)
        {
            if (property == null)
            {
                Debug.LogError("Invalid attempt to add null property to GUIDrawer.");
                return;
            }

            if (properties.Contains(property))
            {
                Debug.LogWarning("Invalid attempt to add property that already exists on GUIDrawer.");
                return;
            }

            properties.Add(property);
        }

        /// <summary>
        /// Remove a property from this drawer.
        /// </summary>
        /// <param name="property">The property to remove.</param>
        public void Remove(GUIProperty property)
        {
            if (property == null)
            {
                Debug.LogError("Invalid attempt to remove a null property.");
                return;
            }

            properties.Remove(property);
        }

        /// <summary>
        /// Remove a property from this drawer with the given index.
        /// </summary>
        /// <param name="index">The index of the property.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= properties.Count)
            {
                Debug.LogError("Invalid attempt to remove a property at an out of range index.");
                return;
            }

            properties.RemoveAt(index);
        }
    }

    /// <summary>
    /// Generic version of GUIDrawer.
    /// </summary>
    /// <typeparam name="T">The attribute used to provide draw information.</typeparam>
    public class GUIDrawer<T> : GUIDrawer where T : WooshiiAttribute
    {
        /// <summary>
        /// The attribute used by this GUIDrawer.
        /// </summary>
        public readonly T Attribute;

        /// <summary>
        /// Create a new instance of a GUIDrawer providing the required attribute.
        /// </summary>
        /// <param name="attribute">The attribute that this drawer uses.</param>
        public GUIDrawer(T attribute)
        {
            Attribute = attribute;
        }
    }
}
