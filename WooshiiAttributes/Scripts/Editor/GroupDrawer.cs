using System;
using System.Collections.Generic;
using UnityEditor;

namespace WooshiiAttributes
{
    /// <summary>
    /// Base class for group drawers.
    /// </summary>
    public class GroupDrawer : GUIDrawerBase
    {
        // - Fields
      
        protected SerializedObject _serializedObject;
        protected Type _attributeType;
        protected List<GUIDrawerBase> _drawers = new List<GUIDrawerBase>();

        // - Properties

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
        
        // - Methods

        public override void OnGUI() { }

        /// <summary>
        /// Add a drawer to this group.
        /// </summary>
        /// <param name="drawer">The drawer.</param>
        public void RegisterProperty(GUIDrawerBase drawer)
        {
            _drawers.Add(drawer);
        }
    }

    /// <summary>
    /// Base class for group drawers.
    /// </summary>
    public class GroupDrawer<T> : GroupDrawer
    {
        public T attribute;

        public GroupDrawer(T attribute, SerializedObject serializedObject) : base(serializedObject)
        {
            this.attribute = attribute;
        }
    }
}
