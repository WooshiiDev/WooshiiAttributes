using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public enum PropertyType
    {
        INVALID     = -1,

        OBJECT      = 0,

        BOOLEAN     = 1,
        STRING      = 2,
        INTEGER     = 3,
        FLOAT       = 4,

        ENUM        = 5,

        VECTOR2     = 6,
        VECTOR3     = 7,

        UNITYOBJ    = 8,
    }

    public class PropertyAttributeDrawer
    {
        public Type type;
        public PropertyInfo propertyInfo;

        public object target;

        public bool isArray;

        public PropertyType propertyType;

        public PropertyAttributeDrawer(PropertyInfo _property, Type _attributeType, object _target)
        {
            propertyInfo = _property;
            type = _attributeType;

            target = _target;
            isArray = _property.PropertyType.IsArray;

            propertyType = GetPropertyType (propertyInfo.PropertyType);
        }

        public virtual void OnGUI()
        {
            EditorGUILayout.LabelField (propertyInfo.Name);
        }

        protected PropertyType GetPropertyType(Type type)
        {
            if (type.IsEnum)
            {
                return PropertyType.ENUM;
            }

            string typeName = type.Name;

            switch (typeName)
            {
                case "Boolean":
                    return PropertyType.BOOLEAN;

                case "String":
                    return PropertyType.STRING;

                case "Int16":
                case "Int32":
                case "Int64":
                    return PropertyType.INTEGER;

                case "Single":
                    return PropertyType.FLOAT;

                case "Vector2":
                    return PropertyType.VECTOR2;

                case "Object":
                    return PropertyType.UNITYOBJ;

                default:
                    return PropertyType.INVALID;
                
            }
        }

        protected PropertyType GetPropertyType(PropertyInfo property)
        {
            return GetPropertyType (property.PropertyType);
        }

        protected T GetValueType<T>()
        {
            if (!propertyInfo.CanRead)
            {
                return default;
            }

            return (T)propertyInfo.GetValue (target);
        }
    }

    public class PropertyAttributeDrawer<T> : PropertyAttributeDrawer where T : ClassPropertyAttribute
    {
        public PropertyAttributeDrawer(PropertyInfo property, object target) : base (property, typeof(T), target)
        {

        }
    }

    public class ClassPropertyDrawer : PropertyAttributeDrawer<ClassPropertyAttribute>
    {
        public ClassPropertyDrawer(PropertyInfo property, object target) : base (property, target)
        {

        }

        public override void OnGUI()
        {
            bool canRead = propertyInfo.CanRead;
            bool canWrite = propertyInfo.CanWrite;

            //string s = string.Format ("{0} [{1}{2}]", prop.Name, canRead ? "get; " : "", canWrite ? "set; " : "");
            //s += "(" + propertyType + ")";

            //if (!canRead)
            //{
            //    //Debug.Log (s + " " + prop.GetValue (target) + " = " + v);

            //    string ss = string.Format ("<{0}>k__BackingField", prop.Name);
            //    var mailField = target.GetType ().GetField (ss,
            //         BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            //    Debug.Log (ss + " : " + mailField);
            //}
            string s = propertyInfo.Name;
            object val = null;

            EditorGUI.BeginChangeCheck ();
            EditorGUILayout.BeginHorizontal ();

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = EditorStyles.boldLabel.CalcSize(new GUIContent("(Property)")).x;
            EditorGUILayout.PrefixLabel ("(Property)", EditorStyles.boldLabel);
            EditorGUIUtility.labelWidth = labelWidth;

            EditorGUI.BeginDisabledGroup (!canWrite);
            switch (propertyType)
            {
                case PropertyType.INVALID:

                    break;

                case PropertyType.OBJECT:
                    
                    break;

                case PropertyType.BOOLEAN:
                    val = EditorGUILayout.Toggle (s, GetValueType<bool>());
                    break;
                case PropertyType.STRING:
                    val = EditorGUILayout.TextField (s, GetValueType<string> ());
                    break;
                case PropertyType.INTEGER:
                    val = EditorGUILayout.IntField (s, GetValueType<int> ());
                    break;
                case PropertyType.FLOAT:
                    val = EditorGUILayout.FloatField (s, GetValueType<float> ());
                    break;
                case PropertyType.ENUM:
                    val = EditorGUILayout.EnumPopup (s, propertyType);
                    break;
                case PropertyType.VECTOR2:
                    val = EditorGUILayout.Vector2Field (s, GetValueType<Vector2> ());
                    break;
                case PropertyType.VECTOR3:
                    val = EditorGUILayout.Vector3Field (s, GetValueType<Vector3> ());
                    break;
                case PropertyType.UNITYOBJ:
                    val = EditorGUILayout.ObjectField (s, null, typeof (Object), true);
                    break;

                default:
                    break;
            }
            EditorGUI.EndDisabledGroup ();
            EditorGUILayout.EndHorizontal ();
        
            if (EditorGUI.EndChangeCheck())
            {
                if (!canWrite || !canRead)
                {
                    return;
                }

                propertyInfo.SetValue (target, val);
            }
        }

    }
}