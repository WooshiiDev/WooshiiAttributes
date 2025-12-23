using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WooshiiAttributes
{
    [GUIDrawer(typeof(NativePropertyAttribute))]
    public class NativePropertyDrawer : GUIDrawerBase<PropertyInfo>
    {
        protected object _target;
        protected NativePropertyAttribute _attribute;
        protected PropertyInfo _property;

        protected PropertyType _propertyType;
        protected readonly string _displayName;

        public NativePropertyDrawer(NativePropertyAttribute attribute, object target, PropertyInfo property) : base(property) 
        {
            _target = target;
            _attribute = attribute;
            _property = property;

            _propertyType = TypeUtility.GetPropertyTypeFromType(property.PropertyType);
            _displayName = string.Format("(Property) {0}", property.Name);
        }

        public override void OnGUI()
        {
            bool canRead = _property.CanRead;
            bool canWrite = _property.CanWrite;

            object value = null;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(true);

                switch (_propertyType)
                {
                    case PropertyType.INVALID:

                        break;

                    // Standard

                    case PropertyType.OBJECT:

                        break;

                    case PropertyType.BOOLEAN:
                        value = EditorGUILayout.Toggle(_displayName, GetValueType<bool>());
                        break;

                    case PropertyType.STRING:
                        value = EditorGUILayout.TextField(_displayName, GetValueType<string>());
                        break;

                    case PropertyType.INTEGER:
                        value = EditorGUILayout.IntField(_displayName, GetValueType<int>());
                        break;

                    case PropertyType.FLOAT:
                        value = EditorGUILayout.FloatField(_displayName, GetValueType<float>());
                        break;

                    case PropertyType.DOUBLE:
                        value = EditorGUILayout.DoubleField(_displayName, GetValueType<double>());
                        break;

                    case PropertyType.LONG:
                        value = EditorGUILayout.LongField(_displayName, GetValueType<long>());
                        break;

                    case PropertyType.ENUM:
                        value = EditorGUILayout.EnumPopup(_displayName, GetValueType<Enum>());
                        break;

                    // Unity Types

                    case PropertyType.UNITY_OBJECT:
                        value = EditorGUILayout.ObjectField(_displayName, GetValueType<Object>(), _property.PropertyType, false);
                        break;

                    case PropertyType.VECTOR2:
                        value = EditorGUILayout.Vector2Field(_displayName, GetValueType<Vector2>());
                        break;

                    case PropertyType.VECTOR3:
                        value = EditorGUILayout.Vector3Field(_displayName, GetValueType<Vector3>());
                        break;

                    case PropertyType.VECTOR4:
                        value = EditorGUILayout.Vector4Field(_displayName, GetValueType<Vector4>());
                        break;

                    case PropertyType.VECTOR2INT:
                        value = EditorGUILayout.Vector2IntField(_displayName, GetValueType<Vector2Int>());
                        break;

                    case PropertyType.VECTOR3INT:
                        value = EditorGUILayout.Vector3IntField(_displayName, GetValueType<Vector3Int>());
                        break;

                    // Colour

                    case PropertyType.COLOR:
                        value = EditorGUILayout.ColorField(_displayName, GetValueType<Color>());
                        break;

                    case PropertyType.GRADIENT:
                        value = EditorGUILayout.GradientField(_displayName, GetValueType<Gradient>());
                        break;

                    // Others

                    case PropertyType.LAYER_MASK:
                        value = (LayerMask)EditorGUILayout.MaskField(
                            _displayName, GetValueType<LayerMask>(), InternalEditorUtility.layers);
                        break;

                    case PropertyType.ANIMATION_CURVE:
                        value = EditorGUILayout.CurveField(_displayName, GetValueType<AnimationCurve>());
                        break;

                    // Areas

                    case PropertyType.RECT:
                        value = EditorGUILayout.RectField(_displayName, GetValueType<Rect>());
                        break;

                    case PropertyType.RECTINT:
                        value = EditorGUILayout.RectIntField(_displayName, GetValueType<RectInt>());
                        break;

                    case PropertyType.BOUNDS:
                        value = EditorGUILayout.BoundsField(_displayName, GetValueType<Bounds>());
                        break;

                    case PropertyType.BOUNDSINT:
                        value = EditorGUILayout.BoundsIntField(_displayName, GetValueType<BoundsInt>());
                        break;

                    default:
                        break;
                }

                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                if (!canWrite || !canRead)
                {
                    return;
                }

                _property.SetValue(_target, value);
            }
        }

        protected T GetValueType<T>()
        {
            if (!_property.CanRead)
            {
                return default;
            }

            object value = _property.GetValue(_target);

            return (T)value;
        }
    }
}