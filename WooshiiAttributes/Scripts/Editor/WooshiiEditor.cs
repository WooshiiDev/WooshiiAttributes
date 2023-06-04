using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    [CustomEditor(typeof(Object), true), CanEditMultipleObjects]
    public class WooshiiEditor : Editor
    {
        // - Statics

        private static Dictionary<Type, Type> s_Elements;

        public static Dictionary<Type, Type> Elements
        {
            get
            {
                if (s_Elements == null)
                {
                    s_Elements = new Dictionary<Type, Type>();

                    Type[] elements = ReflectionUtility.GetTypesFromAssemblies(t => t.GetCustomAttribute<RegisterDrawerAttribute>() != null);
                    for (int i = 0; i < elements.Length; i++)
                    {
                        Type element = elements[i];
                        Type attr = element.BaseType.GetGenericArguments()[0];

                        s_Elements.Add(attr, element);
                    }
                }

                return s_Elements;
            }
        }

        // - Fields

        protected GUIProperty[] Properties = new GUIProperty[0];
        protected GUIDrawer[] Drawers = new GUIDrawer[0];

        // - Initialization

        private void OnEnable()
        {
            Properties = GetProperties();
            Drawers = GetDrawers();
        }

        private GUIProperty[] GetProperties()
        {
            IEnumerable<SerializedProperty> props = SerializedUtility.GetAllVisibleProperties(serializedObject);
            List<GUIProperty> foundProperties = new List<GUIProperty>();

            foreach (SerializedProperty prop in props)
            {
                foundProperties.Add(new GUIProperty(prop));
            }
            return foundProperties.ToArray();
        }

        private GUIDrawer[] GetDrawers()
        {
            GUIDrawer currentGroup = null;
            List<GUIDrawer> drawers = new List<GUIDrawer>();
            foreach (GUIProperty property in Properties)
            {
                IEnumerable<WooshiiAttribute> customAttributes = property.Field.GetCustomAttributes<WooshiiAttribute>();

                if (customAttributes.Count() == 0)
                {
                    RegisterDefaultGUI(property);
                    continue;
                }

                WooshiiAttribute attribute = customAttributes.ElementAt(0);
                GUIDrawer drawer;

                if (attribute is EndGroupAttribute)
                {
                    RegisterDefaultGUI(property);
                    currentGroup = null;
                    continue;
                }

                if (!Elements.TryGetValue(attribute.GetType(), out Type drawerType))
                {
                    RegisterDefaultGUI(property);
                    continue;
                }

                // Custom...

                drawer = Activator.CreateInstance(drawerType, attribute) as GUIDrawer;
                drawer.Add(property);
                drawers.Add(drawer);

                if (attribute is GroupAttribute)
                {
                    currentGroup = drawer;
                }
            }

            return drawers.ToArray();

            void RegisterDefaultGUI(GUIProperty property)
            {
                if (currentGroup == null)
                {
                    GUIDrawer drawer = new GUIDrawer();
                    drawer.Add(property);
                    drawers.Add(drawer);
                }
                else
                {
                    currentGroup.Add(property);
                }
            }
        }

        // - GUI

        public sealed override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            OnGUI();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public virtual void OnGUI()
        {
            DrawStandardGUI();
        }

        public void DrawStandardGUI()
        {
            for (int i = 0; i < Drawers.Length; i++)
            {
                Drawers[i].OnGUI();
            }
        }
    }

    public class WooshiiEditor<T> : WooshiiEditor where T : Object
    {
        public T Target => target as T;
    }
}
