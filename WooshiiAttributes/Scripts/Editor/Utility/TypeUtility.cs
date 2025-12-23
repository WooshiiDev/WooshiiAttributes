using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public static class TypeUtility
    {
        private readonly static Dictionary<Type, PropertyType> s_typeDictionary = new Dictionary<Type, PropertyType> ()
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

            { typeof(Vector2Int)    , PropertyType.VECTOR2INT},
            { typeof(Vector3Int)    , PropertyType.VECTOR3INT },

            { typeof(Color)         , PropertyType.COLOR },
            { typeof(Gradient)      , PropertyType.GRADIENT },

            { typeof(LayerMask)     , PropertyType.LAYER_MASK},
            { typeof(AnimationCurve), PropertyType.ANIMATION_CURVE},

            // Structs

            { typeof(Rect)          , PropertyType.RECT},
            { typeof(RectInt)       , PropertyType.RECTINT},

            { typeof(Bounds)        , PropertyType.BOUNDS},
            { typeof(BoundsInt)     , PropertyType.BOUNDSINT},

            { typeof(Pose)          , PropertyType.POSE},
        };

        public static PropertyType GetPropertyTypeFromType(Type type)
        {
            if (type == null)
            {
                return PropertyType.INVALID;
            }

            if (type.IsEnum)
            {
                return PropertyType.ENUM;
            }

            if (s_typeDictionary.ContainsKey(type))
            {
                return s_typeDictionary[type];
            }

            return PropertyType.INVALID;
        }
    }

    public enum PropertyType
    {
        // --- Standard Types ---

        INVALID = -1,

        OBJECT = 0,

        BOOLEAN = 1,
        STRING = 2,

        INTEGER = 3,
        FLOAT = 4,
        DOUBLE = 5,
        LONG = 6,

        ENUM = 7,

        // --- Unity Types ---

        UNITY_OBJECT = 8,

        VECTOR2 = 9,
        VECTOR3 = 10,
        VECTOR4 = 11,

        VECTOR2INT = 12,
        VECTOR3INT = 13,

        COLOR = 14,
        GRADIENT = 15,

        LAYER_MASK = 16,
        ANIMATION_CURVE = 17,

        // Other unity types - find through serializable properties

        RECT = 18,
        RECTINT = 19,

        BOUNDS = 20,
        BOUNDSINT = 21,

        POSE = 22
    }
}
