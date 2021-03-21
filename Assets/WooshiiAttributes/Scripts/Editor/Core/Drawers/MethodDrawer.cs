using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class MethodDrawer : IMethodDrawer
    {
        protected MethodButtonAttribute attribute;
        public MethodButtonAttribute Attribute => attribute;

        protected Object target;
        public Object Target => target;

        protected MethodInfo methodInfo;
        public MethodInfo MethodInfo => methodInfo;

        public MethodDrawer(MethodButtonAttribute attribute, Object Target, MethodInfo info)
        {
            this.attribute = attribute;
            this.target = Target;
            this.methodInfo = info;
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
            MethodInfo.Invoke (target, null);
        }
    }
}
