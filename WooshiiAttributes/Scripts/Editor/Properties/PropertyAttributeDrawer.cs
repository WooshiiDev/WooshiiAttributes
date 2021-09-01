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

            PropertyType type = GetPropertyType (property.PropertyType);

            switch (type)
            {
                case PropertyType.INVALID:

                    break;

                // Standard

                case PropertyType.OBJECT:

                    break;

                case PropertyType.BOOLEAN:
                    EditorGUILayout.Toggle (displayName, GetValueType<bool> (property, target));
                    break;

                case PropertyType.STRING:
                    EditorGUILayout.TextField (displayName, GetValueType<string> (property, target));
                    break;

                case PropertyType.INTEGER:
                    EditorGUILayout.IntField (displayName, GetValueType<int> (property, target));
                    break;

                case PropertyType.FLOAT:
                    EditorGUILayout.FloatField (displayName, GetValueType<float> (property, target));
                    break;

                case PropertyType.DOUBLE:
                    EditorGUILayout.DoubleField (displayName, GetValueType<double> (property, target));
                    break;

                case PropertyType.LONG:
                    EditorGUILayout.LongField (displayName, GetValueType<long> (property, target));
                    break;

                case PropertyType.ENUM:
                    EditorGUILayout.EnumPopup (displayName, GetValueType<Enum> (property, target));
                    break;

                // Unity Types

                case PropertyType.UNITY_OBJECT:
                    EditorGUILayout.ObjectField (displayName, GetValueType<Object> (property, target), property.PropertyType, false);
                    break;

                case PropertyType.VECTOR2:
                    EditorGUILayout.Vector2Field (displayName, GetValueType<Vector2> (property, target));
                    break;

                case PropertyType.VECTOR3:
                    EditorGUILayout.Vector3Field (displayName, GetValueType<Vector3> (property, target));
                    break;

                case PropertyType.VECTOR4:
                    EditorGUILayout.Vector4Field (displayName, GetValueType<Vector4> (property, target));
                    break;

                case PropertyType.VECTOR2_INT:
                    EditorGUILayout.Vector2IntField (displayName, GetValueType<Vector2Int> (property, target));
                    break;

                case PropertyType.VECTOR3_INT:
                    EditorGUILayout.Vector3IntField (displayName, GetValueType<Vector3Int> (property, target));
                    break;

                // Colour

                case PropertyType.COLOR:
                    EditorGUILayout.ColorField (displayName, GetValueType<Color> (property, target));
                    break;

                case PropertyType.GRADIENT:
                    EditorGUILayout.GradientField (displayName, GetValueType<Gradient> (property, target));
                    break;

                // Others

                case PropertyType.LAYERMASK:
                    EditorGUILayout.MaskField (displayName, GetValueType<LayerMask> (property, target), InternalEditorUtility.layers);
                    break;

                case PropertyType.ANIMATION_CURVE:
                    EditorGUILayout.CurveField (displayName, GetValueType<AnimationCurve> (property, target));
                    break;

                // Areas

                case PropertyType.RECT:
                    EditorGUILayout.RectField (displayName, GetValueType<Rect> (property, target));
                    break;

                case PropertyType.RECT_INT:
                    EditorGUILayout.RectIntField (displayName, GetValueType<RectInt> (property, target));
                    break;

                case PropertyType.BOUNDS:
                    EditorGUILayout.BoundsField (displayName, GetValueType<Bounds> (property, target));
                    break;

                case PropertyType.BOUNDS_INT:
                    EditorGUILayout.BoundsIntField (displayName, GetValueType<BoundsInt> (property, target));
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

            return (T)property.GetValue (target);
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