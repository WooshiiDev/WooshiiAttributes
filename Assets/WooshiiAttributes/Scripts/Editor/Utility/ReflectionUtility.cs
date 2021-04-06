using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
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
