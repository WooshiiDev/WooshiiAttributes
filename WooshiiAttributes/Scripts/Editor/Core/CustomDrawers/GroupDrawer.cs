using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GroupDrawer : GUIDrawerBase
    {
        protected SerializedObject m_serializedObject;
        public SerializedObject SerializedObject => m_serializedObject;

        protected Type m_attributeType;
        public Type AttributeType => m_attributeType;

        protected List<GUIDrawerBase> m_drawers = new List<GUIDrawerBase>();
        public IEnumerable<GUIDrawerBase> Drawers => m_drawers;
       
        public GUIDrawerBase First
        {
            get
            {
                if (m_drawers.Count == 0)
                {
                    return null;
                }
                return m_drawers[0];
            }
        }

        public GroupDrawer(SerializedObject _serializedObject)
        {
            m_serializedObject = _serializedObject;
        }

        public override void OnGUI() { }

        public void RegisterProperty(GUIDrawerBase drawer)
        {
            m_drawers.Add(drawer);
        }
    }

    public class GroupDrawer<T> : GroupDrawer
    {
        public T attribute;

        public GroupDrawer(T attribute, SerializedObject _serializedObject) : base(_serializedObject)
        {
            this.attribute = attribute;
        }
    }
}
