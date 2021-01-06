using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
    {
    public class GlobalDrawer 
        {
        /// <summary>
        /// The parent <see cref="SerializedObject"/> of the script
        /// </summary>
        public SerializedObject SerializedObject { get; private set; }

        /// <summary>
        /// The attribute type of this drawer
        /// </summary>
        public Type AttributeType { get; private set; }

        /// <summary>
        /// List of all properties this global drawer uses.
        /// </summary>
        public List<SerializedProperty> Properties { get; private set; }

        /// <summary>
        /// List of all attributes this global drawer uses.
        /// </summary>
        public List<GlobalAttribute> Attributes { get; private set; }


        public GlobalDrawer(Type attributeType)
            {
            this.AttributeType = attributeType;

            Properties = new List<SerializedProperty> ();
            Attributes = new List<GlobalAttribute> ();
            }

        public virtual void Initalize(SerializedObject serializedObject)
            {
            this.SerializedObject = serializedObject;
            }

        public void Clear()
            {
            Properties.Clear ();
            Attributes.Clear ();
            }

        /// <summary>
        /// Register a <see cref="SerializedProperty"/> to the collection of properties this drawer uses.
        /// </summary>
        /// <param name="property">The property to register</param>
        public virtual void Register(SerializedProperty property)
            {
            Properties.Add (property);
            }

        /// <summary>
        /// Register a <see cref="GlobalAttribute"/> to the collection of properties this drawer uses.
        /// </summary>
        /// <param name="attribute">The attribute to register</param>
        public virtual void Register(GlobalAttribute attribute)
            {
            Attributes.Add (attribute);
            }

        /// <summary>
        /// Draw the GUI for the Drawer
        /// </summary>
        public void OnGUI()
            {
            EditorGUI.BeginChangeCheck ();
                {
                OnGUI_Internal ();
                }
            if (EditorGUI.EndChangeCheck ())
                {
                SerializedObject.ApplyModifiedProperties ();
                }
            }

        protected virtual void OnGUI_Internal()
            {

            }

        public GlobalDrawer Clone()
            {
            return MemberwiseClone () as GlobalDrawer;
            }
        }
    }
