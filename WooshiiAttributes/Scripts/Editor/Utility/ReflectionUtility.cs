using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{


    public static class ReflectionUtility
    {
        // BindingFlags

        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        // Types

        public static IEnumerable<Type> GetTypeSubclasses(Type _type, bool _avoidAbstract = true)
        {
            return _type.Assembly.GetTypes ().Where (t => t.IsSubclassOf (_type));
        }

        // Fields

        public static FieldInfo GetField(Object _target, string _name, BindingFlags _flags = DefaultFlags)
        {
            return GetField(_target.GetType(), _name);
        }

        public static FieldInfo GetField(Type _type, string _name, BindingFlags _flags = DefaultFlags)
        {
            return _type.GetField (_name, _flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Object _instance, BindingFlags _flags = DefaultFlags)
        {
            return GetFields(_instance.GetType(), _flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Type _type, BindingFlags _flags = DefaultFlags)
        {
            return _type.GetFields (_flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Object _instance, Func<FieldInfo, bool> condition, BindingFlags _flags = DefaultFlags)
        {
            return GetFields (_instance.GetType(), _flags).Where (condition);
        }

        public static IEnumerable<FieldInfo> GetFields(Type _type, Func<FieldInfo, bool> condition, BindingFlags _flags = DefaultFlags)
        {
            return _type.GetFields (_flags).Where (condition);
        }

        // Properties

        public static IEnumerable<PropertyInfo> GetProperties(Object _target, BindingFlags _flags = DefaultFlags)
        {
            return GetProperties (_target.GetType (), DefaultFlags);
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type _type, BindingFlags _flags = DefaultFlags)
        {
            return _type.GetProperties (DefaultFlags);
        }

        /// <summary>
        /// Get the value of the property on the target object
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <param name="_target">The target instance</param>
        /// <param name="_info">The property info we want the value of</param>
        /// <returns></returns>
        public static T GetTargetPropertyValue<T>(object _target, PropertyInfo _info)
        {
            if (_info.CanRead)
            {
                return default;
            }

            object value = _info.GetValue (_target);

            return (T)value; 
        }

        // Methods

        public static IEnumerable<MethodInfo> GetMethods(Object _instance, BindingFlags _flags = DefaultFlags)
        {
            return GetMethods (_instance.GetType (), _flags);
        }

        public static IEnumerable<MethodInfo> GetMethods(Type _type, BindingFlags _flags = DefaultFlags)
        {
            return _type.GetMethods ();
        }

    }
}
