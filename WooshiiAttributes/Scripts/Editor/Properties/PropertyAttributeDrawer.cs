using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    public class PropertyAttributeDrawer
    {
        public static Dictionary<Type, PropertyType> PropertyTypes = new Dictionary<Type, PropertyType> ()
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

            { typeof(Vector2Int)    , PropertyType.VECTOR2INT },
            { typeof(Vector3Int)    , PropertyType.VECTOR3INT },

            { typeof(Color)         , PropertyType.COLOR },
            { typeof(Gradient)      , PropertyType.GRADIENT },

            { typeof(LayerMask)     , PropertyType.LAYER_MASK },
            { typeof(AnimationCurve), PropertyType.ANIMATION_CURVE },

            { typeof(Rect)          , PropertyType.RECT     },
            { typeof(RectInt)       , PropertyType.RECTINT  },

            { typeof(Bounds)        , PropertyType.BOUNDS   },
            { typeof(BoundsInt)     , PropertyType.BOUNDSINT },

            { typeof(Pose)          , PropertyType.POSE     },
        };

        protected Object _target;

        // State
        protected PropertyType _propertyType;
        protected bool _isArray;

        // Reflection Info
        protected Type _type;
        protected PropertyInfo _propertyInfo;

        public PropertyAttributeDrawer(PropertyInfo property, Type attributeType, Object target)
        {
            _propertyInfo = property;
            _type = attributeType;

            this._target = target;
            _isArray = property.PropertyType.IsArray;

            _propertyType = TypeUtility.GetPropertyTypeFromType (_type);
        }

        public virtual void OnGUI()
        {
            EditorGUILayout.LabelField (_propertyInfo.Name);
        }

        protected T GetValueType<T>()
        {
            if (!_propertyInfo.CanRead)
            {
                return default;
            }

            object value = _propertyInfo.GetValue (_target);

            return (T)value;
        }

    }

    public class PropertyAttributeDrawer<T> : PropertyAttributeDrawer where T : ClassPropertyAttribute
    {
        public PropertyAttributeDrawer(PropertyInfo property, Object target) : base (property, typeof(T), target) { }
    }

    public class ClassPropertyDrawer : PropertyAttributeDrawer<ClassPropertyAttribute>
    {
        private readonly string _displayName;

        public ClassPropertyDrawer(PropertyInfo property, Object target) : base (property, target)
        {
            string actualName = _propertyInfo.Name;
            actualName = actualName.Substring (0, 1).ToUpper () + actualName.Substring (1);

            _displayName = string.Format ("(Property) {0}", actualName);
        }

        public override void OnGUI()
        {
            bool canRead = _propertyInfo.CanRead;
            bool canWrite = _propertyInfo.CanWrite;

            object value = null;

            EditorGUI.BeginChangeCheck ();
            EditorGUILayout.BeginHorizontal ();
            {
                //EditorGUI.BeginDisabledGroup (!canWrite);
                EditorGUI.BeginDisabledGroup (true);

                switch (_propertyType)
                {
                    case PropertyType.INVALID:

                        break;

                    // Standard

                    case PropertyType.OBJECT:
                        
                        break;

                    case PropertyType.BOOLEAN:
                        value = EditorGUILayout.Toggle (_displayName, GetValueType<bool> ());
                        break;

                    case PropertyType.STRING:
                        value = EditorGUILayout.TextField (_displayName, GetValueType<string> ());
                        break;

                    case PropertyType.INTEGER:
                        value = EditorGUILayout.IntField (_displayName, GetValueType<int> ());
                        break;

                    case PropertyType.FLOAT:
                        value = EditorGUILayout.FloatField (_displayName, GetValueType<float> ());
                        break;

                    case PropertyType.DOUBLE:
                        value = EditorGUILayout.DoubleField (_displayName, GetValueType<double> ());
                        break;

                    case PropertyType.LONG:
                        value = EditorGUILayout.LongField (_displayName, GetValueType<long> ());
                        break;

                    case PropertyType.ENUM:
                        value = EditorGUILayout.EnumPopup (_displayName, GetValueType<Enum> ());
                        break;

                    // Unity Types

                    case PropertyType.UNITY_OBJECT:
                        value = EditorGUILayout.ObjectField (_displayName, GetValueType<Object> (), _propertyInfo.PropertyType, false);
                        break;

                    case PropertyType.VECTOR2:
                        value = EditorGUILayout.Vector2Field (_displayName, GetValueType<Vector2> ());
                        break;

                    case PropertyType.VECTOR3:
                        value = EditorGUILayout.Vector3Field (_displayName, GetValueType<Vector3> ());
                        break;

                    case PropertyType.VECTOR4:
                        value = EditorGUILayout.Vector4Field (_displayName, GetValueType<Vector4> ());
                        break;

                    case PropertyType.VECTOR2INT:
                        value = EditorGUILayout.Vector2IntField (_displayName, GetValueType<Vector2Int> ());
                        break;

                    case PropertyType.VECTOR3INT:
                        value = EditorGUILayout.Vector3IntField (_displayName, GetValueType<Vector3Int> ());
                        break;

                    // Colour

                    case PropertyType.COLOR:
                        value = EditorGUILayout.ColorField (_displayName, GetValueType<Color> ());
                        break;

                    case PropertyType.GRADIENT:
                        value = EditorGUILayout.GradientField (_displayName, GetValueType<Gradient> ());
                        break;

                    // Others

                    case PropertyType.LAYER_MASK:
                        value = (LayerMask)EditorGUILayout.MaskField (
                            _displayName, GetValueType<LayerMask> (), InternalEditorUtility.layers);
                        break;

                    case PropertyType.ANIMATION_CURVE:
                        value = EditorGUILayout.CurveField (_displayName, GetValueType<AnimationCurve> ());
                        break;

                    // Areas

                    case PropertyType.RECT:
                        value = EditorGUILayout.RectField (_displayName, GetValueType<Rect> ());
                        break;

                    case PropertyType.RECTINT:
                        value = EditorGUILayout.RectIntField (_displayName, GetValueType<RectInt> ());
                        break;

                    case PropertyType.BOUNDS:
                        value = EditorGUILayout.BoundsField (_displayName, GetValueType<Bounds> ());
                        break;

                    case PropertyType.BOUNDSINT:
                        value = EditorGUILayout.BoundsIntField (_displayName, GetValueType<BoundsInt> ());
                        break;

                    default:
                        break;
                }

                EditorGUI.EndDisabledGroup ();
            }
            EditorGUILayout.EndHorizontal ();
        
            if (EditorGUI.EndChangeCheck())
            {
                if (!canWrite || !canRead)
                {
                    return;
                }

                _propertyInfo.SetValue (_target, value);
            }
        }

    }
}