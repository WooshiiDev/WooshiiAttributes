using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public static class TypeUtility
    {
        private readonly static Dictionary<Type, PropertyType> TypeDictionary = new Dictionary<Type, PropertyType> ()
        {
            // Standard Types

            { typeof(bool)          , PropertyType.BOOLEAN },
            { typeof(string)        , PropertyType.STRING},
            { typeof(int)           , PropertyType.INTEGER},
            { typeof(float)         , PropertyType.FLOAT},
            { typeof(double)        , PropertyType.DOUBLE},
            { typeof(long)          , PropertyType.LONG},

            { typeof(Enum)          , PropertyType.ENUM},

            // Unity Standard Types

            { typeof(Object)        , PropertyType.UNITY_OBJECT },

            { typeof(Vector2)       , PropertyType.VECTOR2 },
            { typeof(Vector3)       , PropertyType.VECTOR3 },
            { typeof(Vector4)       , PropertyType.VECTOR4 }, 

            { typeof(Vector2Int)    , PropertyType.VECTOR2_INT},
            { typeof(Vector3Int)    , PropertyType.VECTOR3_INT },

            { typeof(Color)         , PropertyType.COLOR },
            { typeof(Gradient)      , PropertyType.GRADIENT },

            { typeof(LayerMask)     , PropertyType.LAYERMASK},
            { typeof(AnimationCurve), PropertyType.ANIMATION_CURVE},

            // Structs

            { typeof(Rect)          , PropertyType.RECT},
            { typeof(RectInt)       , PropertyType.RECT_INT},

            { typeof(Bounds)        , PropertyType.BOUNDS},
            { typeof(BoundsInt)     , PropertyType.BOUNDS_INT},

            { typeof(Pose)          , PropertyType.POSE},
        };

        public static PropertyType GetPropertyTypeFromType(Type _type)
        {
            if (_type == null)
            {
                return PropertyType.INVALID;
            }

            if (_type.IsEnum)
            {
                return PropertyType.ENUM;
            }

            if (TypeDictionary.ContainsKey(_type))
            {
                return TypeDictionary[_type];
            }

            return PropertyType.INVALID;
        }
    }
}
