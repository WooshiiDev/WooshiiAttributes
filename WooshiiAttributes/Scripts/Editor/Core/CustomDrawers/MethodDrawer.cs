using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class MethodDrawer : IMethodDrawer
    {
        protected MethodButtonAttribute _attribute;
        public MethodButtonAttribute Attribute => _attribute;

        protected Object _target;
        public Object Target => _target;

        protected MethodInfo _methodInfo;
        public MethodInfo MethodInfo => _methodInfo;

        public MethodDrawer(MethodButtonAttribute _attribute, Object _target, MethodInfo _info)
        {
            this._attribute = _attribute;
            this._target = _target;
            this._methodInfo = _info;

            if (_attribute.MethodName == null)
            {
                _attribute.MethodName = _methodInfo.Name;

            }
        }

        public virtual void OnGUI()
        {
            if (GUILayout.Button(Attribute.MethodName))
            {
                CallMethod ();
            }
        }

        protected void CallMethod()
        {
            MethodInfo.Invoke (_target, null);
        }
    }
}
