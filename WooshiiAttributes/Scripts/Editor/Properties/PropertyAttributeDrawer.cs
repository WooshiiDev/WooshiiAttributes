using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public enum PropertyType
    {
        INVALID = -1,

        OBJECT = 0,

        BOOLEAN = 1,
        STRING = 2,
        INTEGER = 3,
        FLOAT = 4,
        DOUBLE = 5,
        LONG = 6,

        ENUM = 7,

        UNITY_OBJECT = 8,

        VECTOR2 = 9,
        VECTOR3 = 10,
        VECTOR4 = 11,

        VECTOR2_INT = 12,
        VECTOR3_INT = 13,

        LAYERMASK = 14,

        COLOR = 15,
        GRADIENT = 16,

        ANIMATION_CURVE = 17,

        RECT = 18,
        RECT_INT = 19,

        BOUNDS = 20,
        BOUNDS_INT = 21,

        POSE = 22
    }

    public static class NativePropertyDrawer
    {
        private static Dictionary<Type, PropertyType> PropertyTypes = new Dictionary<Type, PropertyType> ()
        {
            { typeof(bool)          , PropertyType.BOOLEAN  },
            { typeof(string)        , PropertyType.STRING   },
            { typeof(int)           , PropertyType.INTEGER  },
            { typeof(float)         , PropertyType.FLOAT    },
            { typeof(double)        , PropertyType.DOUBLE   },
            { typeof(long)          , PropertyType.LONG     },

            { typeof(Enum)          , PropertyType.ENUM     },

            { typeof(Object)        , PropertyType.UNITY_OBJECT     },

            { typeof(Vector2)       , PropertyType.VECTOR2  },
            { typeof(Vector3)       , PropertyType.VECTOR3  },
            { typeof(Vector4)       , PropertyType.VECTOR4  },

            { typeof(Vector2Int)    , PropertyType.VECTOR2_INT },
            { typeof(Vector3Int)    , PropertyType.VECTOR3_INT },

            { typeof(Color)         , PropertyType.COLOR },
            { typeof(Gradient)      , PropertyType.GRADIENT },

            { typeof(LayerMask)     , PropertyType.LAYERMASK },
            { typeof(AnimationCurve), PropertyType.ANIMATION_CURVE },

            { typeof(Rect)          , PropertyType.RECT     },
            { typeof(RectInt)       , PropertyType.RECT_INT  },

            { typeof(Bounds)        , PropertyType.BOUNDS   },
            { typeof(BoundsInt)     , PropertyType.BOUNDS_INT },

            { typeof(Pose)          , PropertyType.POSE     },
        };

        public static void OnGUI(PropertyInfo property, Object target)
        {
            string displayName = property.Name;
            object value;

            PropertyType type = GetPropertyType (property.PropertyType);

            switch (type)
            {
                case PropertyType.INVALID:

                    break;

                // Standard

                case PropertyType.OBJECT:

                    break;

                case PropertyType.BOOLEAN:
                    value = EditorGUILayout.Toggle (displayName, GetValueType<bool> (property, target));
                    break;

                case PropertyType.STRING:
                    value = EditorGUILayout.TextField (displayName, GetValueType<string> (property, target));
                    break;

                case PropertyType.INTEGER:
                    value = EditorGUILayout.IntField (displayName, GetValueType<int> (property, target));
                    break;

                case PropertyType.FLOAT:
                    value = EditorGUILayout.FloatField (displayName, GetValueType<float> (property, target));
                    break;

                case PropertyType.DOUBLE:
                    value = EditorGUILayout.DoubleField (displayName, GetValueType<double> (property, target));
                    break;

                case PropertyType.LONG:
                    value = EditorGUILayout.LongField (displayName, GetValueType<long> (property, target));
                    break;

                case PropertyType.ENUM:
                    value = EditorGUILayout.EnumPopup (displayName, GetValueType<Enum> (property, target));
                    break;

                // Unity Types

                case PropertyType.UNITY_OBJECT:
                    value = EditorGUILayout.ObjectField (displayName, GetValueType<Object> (property, target), property.PropertyType, false);
                    break;

                case PropertyType.VECTOR2:
                    value = EditorGUILayout.Vector2Field (displayName, GetValueType<Vector2> (property, target));
                    break;

                case PropertyType.VECTOR3:
                    value = EditorGUILayout.Vector3Field (displayName, GetValueType<Vector3> (property, target));
                    break;

                case PropertyType.VECTOR4:
                    value = EditorGUILayout.Vector4Field (displayName, GetValueType<Vector4> (property, target));
                    break;

                case PropertyType.VECTOR2_INT:
                    value = EditorGUILayout.Vector2IntField (displayName, GetValueType<Vector2Int> (property, target));
                    break;

                case PropertyType.VECTOR3_INT:
                    value = EditorGUILayout.Vector3IntField (displayName, GetValueType<Vector3Int> (property, target));
                    break;

                // Colour

                case PropertyType.COLOR:
                    value = EditorGUILayout.ColorField (displayName, GetValueType<Color> (property, target));
                    break;

                case PropertyType.GRADIENT:
                    value = EditorGUILayout.GradientField (displayName, GetValueType<Gradient> (property, target));
                    break;

                // Others

                case PropertyType.LAYERMASK:
                    value = (LayerMask)EditorGUILayout.MaskField (
                        displayName, GetValueType<LayerMask> (property, target), InternalEditorUtility.layers);
                    break;

                case PropertyType.ANIMATION_CURVE:
                    value = EditorGUILayout.CurveField (displayName, GetValueType<AnimationCurve> (property, target));
                    break;

                // Areas

                case PropertyType.RECT:
                    value = EditorGUILayout.RectField (displayName, GetValueType<Rect> (property, target));
                    break;

                case PropertyType.RECT_INT:
                    value = EditorGUILayout.RectIntField (displayName, GetValueType<RectInt> (property, target));
                    break;

                case PropertyType.BOUNDS:
                    value = EditorGUILayout.BoundsField (displayName, GetValueType<Bounds> (property, target));
                    break;

                case PropertyType.BOUNDS_INT:
                    value = EditorGUILayout.BoundsIntField (displayName, GetValueType<BoundsInt> (property, target));
                    break;

                default:
                    break;
            }
        }

        private static T GetValueType<T>(PropertyInfo property, object target)
        {
            if (!property.CanRead)
            {
                return default;
            }

            object value = property.GetValue (target);

            return (T)value;
        }

        private static PropertyType GetPropertyType(Type type)
        {
            if (type == typeof (ValueType))
            {
                return PropertyType.INVALID;
            }

            if (PropertyTypes.ContainsKey (type))
            {
                return PropertyTypes[type];
            }

            return GetPropertyType (type.BaseType);
        }
    }
}