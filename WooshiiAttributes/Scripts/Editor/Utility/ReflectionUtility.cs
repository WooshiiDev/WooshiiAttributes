using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    /// <summary>
    /// Utility methods to collect Reflection data
    /// </summary>
    public static class ReflectionUtility
    {
        // - BindingFlags

        private const BindingFlags DEFAULT_FLAGS = BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        // - Types

        /// <summary>
        /// Get the subclasses of a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="avoidAbstract">Should abstract types be ignored.</param>
        /// <returns>Returns a collection of subclasses found.</returns>
        public static IEnumerable<Type> GetTypeSubclasses(Type type, bool avoidAbstract = true)
        {
            return type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type));
        }

        // - Fields

        /// <summary>
        /// Get a field on a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the field if one is found.</returns>
        public static FieldInfo GetField(Object type, string name, BindingFlags flags = DEFAULT_FLAGS)
        {
            return GetField(type.GetType(), name, flags);
        }

        /// <summary>
        /// Get a field on a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the field if one is found.</returns>
        public static FieldInfo GetField(Type type, string name, BindingFlags flags = DEFAULT_FLAGS)
        {
            return type.GetField(name, flags);
        }

        /// <summary>
        /// Get a collection of fields for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of fields found.</returns>
        public static IEnumerable<FieldInfo> GetFields(Object type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return GetFields(type.GetType(), flags);
        }

        /// <summary>
        /// Get a collection of fields for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of fields found.</returns>
        public static IEnumerable<FieldInfo> GetFields(Type type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return type.GetFields(flags);
        }

        /// <summary>
        /// Get a collection of fields for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="condition">The required condition for a valid field.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of fields found.</returns>
        public static IEnumerable<FieldInfo> GetFields(Object type, Func<FieldInfo, bool> condition, BindingFlags flags = DEFAULT_FLAGS)
        {
            return GetFields(type.GetType(), flags).Where(condition);
        }

        /// <summary>
        /// Get a collection of fields for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="condition">The required condition for a valid field.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of fields found.</returns>
        public static IEnumerable<FieldInfo> GetFields(Type type, Func<FieldInfo, bool> condition, BindingFlags flags = DEFAULT_FLAGS)
        {
            return type.GetFields(flags).Where(condition);
        }

        // - Properties

        /// <summary>
        /// Get a collection of properties for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of properties found.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(Object type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return GetProperties(type.GetType(), DEFAULT_FLAGS);
        }

        /// <summary>
        /// Get a collection of properties for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of properties found.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return type.GetProperties(DEFAULT_FLAGS);
        }

        // - Methods

        /// <summary>
        /// Get a collection of methods for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of methods found.</returns>
        public static IEnumerable<MethodInfo> GetMethods(Object type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return GetMethods(type.GetType(), flags);
        }

        /// <summary>
        /// Get a collection of methods for a given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="flags">The reflection flags.</param>
        /// <returns>Returns the collection of methods found.</returns>
        public static IEnumerable<MethodInfo> GetMethods(Type type, BindingFlags flags = DEFAULT_FLAGS)
        {
            return type.GetMethods(flags);
        }
    }
}
