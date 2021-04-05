using System.Reflection;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    interface IMethodDrawer
    {
        MethodButtonAttribute Attribute { get; }
        Object Target { get; }
        MethodInfo MethodInfo { get; }

        void OnGUI();
    }
}
