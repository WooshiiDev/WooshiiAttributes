using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
{
    public class GroupDrawer : GUIDrawerBase
    {
        protected SerializedObject _serializedObject;
        protected Type _attributeType;
        protected List<GUIDrawerBase> _drawers = new List<GUIDrawerBase>();

        public SerializedObject SerializedObject => _serializedObject;
        public Type AttributeType => _attributeType;
        public IEnumerable<GUIDrawerBase> Drawers => _drawers;

        public GUIDrawerBase First
        {
            get
            {
                if (_drawers.Count == 0)
                {
                    return null;
                }
                return _drawers[0];
            }
        }

        public GroupDrawer(SerializedObject serializedObject)
        {
            this._serializedObject = serializedObject;
        }

        public override void OnGUI() { }

        public void RegisterProperty(GUIDrawerBase drawer)
        {
            _drawers.Add(drawer);
        }
    }

    public class GroupDrawer<T> : GroupDrawer
    {
        public T attribute;

        public GroupDrawer(T attribute, SerializedObject serializedObject) : base(serializedObject)
        {
            this.attribute = attribute;
        }
    }
}
