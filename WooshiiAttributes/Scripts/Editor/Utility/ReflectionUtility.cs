using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public static class ReflectionUtility
    {
        public const BindingFlags DEFAULT_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        // - Fields

        /// <summary>
        /// Get a field with the given name in the specified type
        /// </summary>
        /// <param name="_type">The object type the field is in</param>
        /// <param name="_name">The name of the field</param>
        /// <param name="_flags">BindingFlags to find the field</param>
        /// <returns>Returns a field if one is found, otherwise will return null</returns>
        public static FieldInfo GetField(Type _type, string _name, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return _type.GetField (_name, _flags);
        }

        /// <summary>
        /// Get a field with the given name in the specified object
        /// </summary>
        /// <param name="_type">The object type the field is in</param>
        /// <param name="_name">The name of the field</param>
        /// <param name="_flags">BindingFlags to find the field</param>
        /// <returns>Returns a field if one is found, otherwise will return null</returns>
        public static FieldInfo GetField(Object _target, string _name, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return GetField(_target.GetType(), _name);
        }

        public static IEnumerable<FieldInfo> GetFields(Object _instance, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return GetFields(_instance.GetType(), _flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Type _type, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return _type.GetFields (_flags);
        }

        public static IEnumerable<FieldInfo> GetFields(Type _type, Func<FieldInfo, bool> condition, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return _type.GetFields (_flags).Where (condition);
        }
    
        public static IEnumerable<FieldInfo> GetFields(Object _instance, Func<FieldInfo, bool> condition, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return GetFields (_instance.GetType (), _flags).Where (condition);
        }

        // Properties

        public static IEnumerable<PropertyInfo> GetProperties(Object _target, BindingFlags _flags = DEFAULT_FLAGS, Predicate<PropertyInfo> condition = null)
        {
            return GetProperties (_target.GetType (), DEFAULT_FLAGS, condition);
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type _type, BindingFlags _flags = DEFAULT_FLAGS, Predicate<PropertyInfo> condition = null)
        {
            return _type.GetProperties (DEFAULT_FLAGS).Where (prop => condition.Invoke(prop));
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

        // - Methods

        /// <summary>
        /// Get a method with the given name in the specified type
        /// </summary>
        /// <param name="_type">The object type the method is in</param>
        /// <param name="_name">The name of the method</param>
        /// <param name="_flags">BindingFlags to find the method</param>
        /// <returns>Returns a method if one is found, otherwise will return null</returns>
        public static MethodInfo GetMethod(Type _type, string _name, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return _type.GetMethod (_name, _flags);
        }

        /// <summary>
        /// Get a method with the given name in the specified object
        /// </summary>
        /// <param name="_type">The object the method is in</param>
        /// <param name="_name">The name of the method</param>
        /// <param name="_flags">BindingFlags to find the method</param>
        /// <returns>Returns a method if one is found, otherwise will return null</returns>
        public static MethodInfo GetMethod(Object _target, string _name, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return GetMethod (_target.GetType (), _name, _flags);
        }

        public static IEnumerable<MethodInfo> GetMethods(Object _instance, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return GetMethods (_instance.GetType (), _flags);
        }

        public static IEnumerable<MethodInfo> GetMethods(Type _type, BindingFlags _flags = DEFAULT_FLAGS)
        {
            return _type.GetMethods (_flags);
        }

        // - Subtypes

        public static IEnumerable<Type> GetTypeSubclasses(Type _type, bool _avoidAbstract = false)
        {
            if (!_avoidAbstract)
            {
                return _type.Assembly.GetTypes().Where(t => t.IsSubclassOf(_type));
            }

            return _type.Assembly.GetTypes().Where(t => t.IsSubclassOf(_type) && !t.IsAbstract);
        }

        /// <summary>
        /// Get all subtypes for the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the subtypes found.</returns>
        public static Type[] GetSubTypes(Type type)
        {
            // Cannot get subtypes if the given type is null

            if (type == null)
            {
                Debug.Log("Cannot get subtypes for null type.");
                return null;
            }

            // Return the subclasses of the type 

            return GetTypesFromAssembly(GetAssemblyFromType(type)).Where(t => t.IsSubclassOf(type)).ToArray();
        }

        /// <summary>
        /// Get all subtypes for the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="condition">The condition to check subtypes against.</param>
        /// <returns>Returns the subtypes found.</returns>
        public static Type[] GetSubTypes(Type type, Predicate<Type> condition)
        {
            // Cannot get subtypes if the given type is null

            if (type == null)
            {
                Debug.Log("Cannot get subtypes for null type.");
                return null;
            }

            return GetSubTypes(type).Where(t => condition(t)).ToArray();
        }

        /// <summary>
        /// Get all types from all assemblies.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the subtypes found.</returns>
        public static Type[] GetSubTypesFromAssemblies(Type type)
        {
            // Cannot get subtypes if the given type is null

            if (type == null)
            {
                Debug.Log("Cannot get subtypes for null type.");
                return null;
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Loop through all assemblies found and add the found subclasses to the list

            List<Type> types = new List<Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                types.AddRange(GetTypesFromAssembly(assemblies[i], t => t.IsSubclassOf(type)));
            }

            return types.ToArray();
        }

        /// <summary>
        /// Get all types from all assemblies.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the subtypes found.</returns>
        public static Type[] GetSubTypesFromAssemblies(Type type, Predicate<Type> condition)
        {
            return GetSubTypesFromAssemblies(type).Where(t => condition(t)).ToArray();
        }

        // - All Types

        /// <summary>
        /// Get all types from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the types found. If the assembly given is null, so will the array returned.</returns>
        public static Type[] GetTypesFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                Debug.LogError("Cannot find types from a null assembly.");
                return null;
            }

            return assembly.GetTypes();
        }

        /// <summary>
        /// Get all types from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the types found. If the assembly given is null, so will the array returned.</returns>
        public static Type[] GetTypesFromAssembly(Assembly assembly, Predicate<Type> condition)
        {
            return GetTypesFromAssembly(assembly).Where(t => condition(t)).ToArray();
        }

        /// <summary>
        /// Get all types from all assemblies.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the types found.</returns>
        public static Type[] GetTypesFromAssemblies()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Loop through all assemblies found and add the found subclasses to the list

            List<Type> types = new List<Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                types.AddRange(GetTypesFromAssembly(assemblies[i]));
            }

            return types.ToArray();
        }

        /// <summary>
        /// Get all types from all assemblies.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the types found.</returns>
        public static Type[] GetTypesFromAssemblies(Predicate<Type> condition)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Loop through all assemblies found and add the found subclasses to the list

            List<Type> types = new List<Type>();
            for (int i = 0; i < assemblies.Length; i++)
            {
                types.AddRange(GetTypesFromAssembly(assemblies[i]).Where(t => condition(t)));
            }

            return types.ToArray();
        }

        // - Assembly

        /// <summary>
        /// Get the assembly the given type is in.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the assembly for the type. Will return null if the type is null.</returns>
        public static Assembly GetAssemblyFromType(Type type)
        {
            // Cannot get subtypes if the given type is null

            if (type == null)
            {
                Debug.Log("Cannot get subtypes for null type.");
                return null;
            }

            return Assembly.GetAssembly(type);
        }
    }
}
