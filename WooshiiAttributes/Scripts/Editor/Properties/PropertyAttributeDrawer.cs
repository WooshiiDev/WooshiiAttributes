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

        protected Object m_target;

        // State
        protected PropertyType m_propertyType;
        protected bool m_isArray;

        // Reflection Info
        protected Type m_type;
        protected PropertyInfo m_propertyInfo;

        public PropertyAttributeDrawer(PropertyInfo _property, Type _attributeType, Object _target)
        {
            m_propertyInfo = _property;
            m_type = _attributeType;

            m_target = _target;
            m_isArray = _property.PropertyType.IsArray;

            m_propertyType = TypeUtility.GetPropertyTypeFromType (m_type);
        }

        public virtual void OnGUI()
        {
            EditorGUILayout.LabelField (m_propertyInfo.Name);
        }

        protected T GetValueType<T>()
        {
            if (!m_propertyInfo.CanRead)
            {
                return default;
            }

            object value = m_propertyInfo.GetValue (m_target);

            return (T)value;
        }

    }

    public class PropertyAttributeDrawer<T> : PropertyAttributeDrawer where T : ClassPropertyAttribute
    {
        public PropertyAttributeDrawer(PropertyInfo _property, Object _target) : base (_property, typeof(T), _target) { }
    }

    public class ClassPropertyDrawer : PropertyAttributeDrawer<ClassPropertyAttribute>
    {
        private readonly string displayName;

        public ClassPropertyDrawer(PropertyInfo _property, Object _target) : base (_property, _target)
        {
            string actualName = m_propertyInfo.Name;
            actualName = actualName.Substring (0, 1).ToUpper () + actualName.Substring (1);

            displayName = string.Format ("(Property) {0}", actualName);
        }

        public override void OnGUI()
        {
            bool canRead = m_propertyInfo.CanRead;
            bool canWrite = m_propertyInfo.CanWrite;

            object value = null;

            EditorGUI.BeginChangeCheck ();
            EditorGUILayout.BeginHorizontal ();
            {
                //EditorGUI.BeginDisabledGroup (!canWrite);
                EditorGUI.BeginDisabledGroup (true);

                switch (m_propertyType)
                {
                    case PropertyType.INVALID:

                        break;

                    // Standard

                    case PropertyType.OBJECT:
                        
                        break;

                    case PropertyType.BOOLEAN:
                        value = EditorGUILayout.Toggle (displayName, GetValueType<bool> ());
                        break;

                    case PropertyType.STRING:
                        value = EditorGUILayout.TextField (displayName, GetValueType<string> ());
                        break;

                    case PropertyType.INTEGER:
                        value = EditorGUILayout.IntField (displayName, GetValueType<int> ());
                        break;

                    case PropertyType.FLOAT:
                        value = EditorGUILayout.FloatField (displayName, GetValueType<float> ());
                        break;

                    case PropertyType.DOUBLE:
                        value = EditorGUILayout.DoubleField (displayName, GetValueType<double> ());
                        break;

                    case PropertyType.LONG:
                        value = EditorGUILayout.LongField (displayName, GetValueType<long> ());
                        break;

                    case PropertyType.ENUM:
                        value = EditorGUILayout.EnumPopup (displayName, GetValueType<Enum> ());
                        break;

                    // Unity Types

                    case PropertyType.UNITY_OBJECT:
                        value = EditorGUILayout.ObjectField (displayName, GetValueType<Object> (), m_propertyInfo.PropertyType, false);
                        break;

                    case PropertyType.VECTOR2:
                        value = EditorGUILayout.Vector2Field (displayName, GetValueType<Vector2> ());
                        break;

                    case PropertyType.VECTOR3:
                        value = EditorGUILayout.Vector3Field (displayName, GetValueType<Vector3> ());
                        break;

                    case PropertyType.VECTOR4:
                        value = EditorGUILayout.Vector4Field (displayName, GetValueType<Vector4> ());
                        break;

                    case PropertyType.VECTOR2INT:
                        value = EditorGUILayout.Vector2IntField (displayName, GetValueType<Vector2Int> ());
                        break;

                    case PropertyType.VECTOR3INT:
                        value = EditorGUILayout.Vector3IntField (displayName, GetValueType<Vector3Int> ());
                        break;

                    // Colour

                    case PropertyType.COLOR:
                        value = EditorGUILayout.ColorField (displayName, GetValueType<Color> ());
                        break;

                    case PropertyType.GRADIENT:
                        value = EditorGUILayout.GradientField (displayName, GetValueType<Gradient> ());
                        break;

                    // Others

                    case PropertyType.LAYER_MASK:
                        value = (LayerMask)EditorGUILayout.MaskField (
                            displayName, GetValueType<LayerMask> (), InternalEditorUtility.layers);
                        break;

                    case PropertyType.ANIMATION_CURVE:
                        value = EditorGUILayout.CurveField (displayName, GetValueType<AnimationCurve> ());
                        break;

                    // Areas

                    case PropertyType.RECT:
                        value = EditorGUILayout.RectField (displayName, GetValueType<Rect> ());
                        break;

                    case PropertyType.RECTINT:
                        value = EditorGUILayout.RectIntField (displayName, GetValueType<RectInt> ());
                        break;

                    case PropertyType.BOUNDS:
                        value = EditorGUILayout.BoundsField (displayName, GetValueType<Bounds> ());
                        break;

                    case PropertyType.BOUNDSINT:
                        value = EditorGUILayout.BoundsIntField (displayName, GetValueType<BoundsInt> ());
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

                m_propertyInfo.SetValue (m_target, value);
            }
        }

    }
}