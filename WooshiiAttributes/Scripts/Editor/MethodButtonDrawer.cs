using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using UnityEditorInternal;

namespace WooshiiAttributes
{
    [GUIDrawer(typeof(MethodButtonAttribute))]
    public class MethodButtonDrawer : GUIDrawerBase<MethodInfo>// IMethodDrawer
    {
        protected struct ParameterData
        {
            public PropertyType type;
            public object value;
            public string name;

            public ParameterData(string _name, object _value, PropertyType _type)
            {
                name = _name;
                value = _value;
                type = _type;
            }
        }

        protected MethodButtonAttribute _attribute;
        public MethodButtonAttribute Attribute => _attribute;

        protected Object _target;
        public Object Target => _target;

        protected MethodInfo _methodInfo;
        public MethodInfo MethodInfo => _methodInfo;

        protected ParameterData[] m_parameterData;

        protected bool hasArguments = false;

        public MethodButtonDrawer(MethodButtonAttribute _attribute, Object _target, MethodInfo _info) : base(_info)
        {
            this._attribute = _attribute;

            this._target = _target;
            this._methodInfo = _info;

            if (_attribute.MethodName == null)
            {
                _attribute.MethodName = _methodInfo.Name;
            }

            // Get parameters and set parameter data
            ParameterInfo[] parameters = _info.GetParameters ();
            int paramLength = parameters.Length;

            hasArguments = paramLength > 0;

            m_parameterData = new ParameterData[parameters.Length];

            // Need to iterate through all args and handle their values
            if (hasArguments)
            {
                for (int i = 0; i < paramLength; i++)
                {
                    ParameterInfo parameterInfo = parameters[i];
                    PropertyType propertyType = TypeUtility.GetPropertyTypeFromType (parameterInfo.ParameterType);

                    object value = null;

                    if (parameterInfo.HasDefaultValue)
                    {
                        value = parameterInfo.DefaultValue;
                    }
                    else
                    {
                        if (!parameterInfo.ParameterType.IsValueType)
                        {

                        }

                        switch (propertyType)
                        {
                            case PropertyType.BOOLEAN:
                                value = default(bool);
                                break;
                            case PropertyType.STRING:
                                value = default (string);
                                break;
                            case PropertyType.INTEGER:
                                value = default (int);
                                break;
                            case PropertyType.FLOAT:
                                value = default (float);
                                break;
                            case PropertyType.DOUBLE:
                                value = default (double);
                                break;
                            case PropertyType.LONG:
                                value = default (long);
                                break;
                            case PropertyType.ENUM:
                                value = default (Enum);
                                break;
                            case PropertyType.UNITY_OBJECT:
                                value = default (Object);
                                break;
                            case PropertyType.VECTOR2:
                                value = Vector2.zero;
                                break;
                            case PropertyType.VECTOR3:
                                value = Vector3.zero;
                                break;
                            case PropertyType.VECTOR4:
                                value = default (Vector4);
                                break;
                            case PropertyType.VECTOR2INT:
                                value = default (Vector2Int);
                                break;
                            case PropertyType.VECTOR3INT:
                                value = default (Vector3Int);
                                break;
                            case PropertyType.COLOR:
                                value = default (Color);
                                break;
                            case PropertyType.GRADIENT:
                                value = default (Gradient);
                                break;
                            case PropertyType.LAYER_MASK:
                                value = default (LayerMask);
                                break;
                            case PropertyType.ANIMATION_CURVE:
                                value = default (AnimationCurve);
                                break;
                            case PropertyType.RECT:
                                value = default (Rect);
                                break;
                            case PropertyType.RECTINT:
                                value = default (RectInt);
                                break;
                            case PropertyType.BOUNDS:
                                value = default (Bounds);
                                break;
                            case PropertyType.BOUNDSINT:
                                value = default (BoundsInt);
                                break;
                            case PropertyType.POSE:
                                value = default (Pose);
                                break;
                            default:
                                break;
                        }
                    }

                    m_parameterData[i] = new ParameterData(parameterInfo.Name, value, propertyType);
                }
            }
        }

        public override void OnGUI()
        {
            if (hasArguments)
            {
                EditorGUILayout.BeginVertical (GUI.skin.box);
                {
                    if (GUILayout.Button (Attribute.MethodName))
                    {
                        CallMethod ();
                    }

                    for (int i = 0; i < m_parameterData.Length; i++)
                    {
                        EditorGUI.BeginChangeCheck ();

                        ParameterData arg = m_parameterData[i];
                        string name = arg.name;
                        object value = arg.value;

                        switch (arg.type)
                        {
                            case PropertyType.INVALID:

                                break;

                            // Standard

                            case PropertyType.OBJECT:

                                break;

                            case PropertyType.BOOLEAN:
                                value = EditorGUILayout.Toggle (name, (bool)value);
                                break;

                            case PropertyType.STRING:
                                value = EditorGUILayout.TextField (name, (string)value);
                                break;

                            case PropertyType.INTEGER:
                                value = EditorGUILayout.IntField (name, (int)value);
                                break;

                            case PropertyType.FLOAT:
                                value = EditorGUILayout.FloatField (name, (float)value);
                                break;

                            case PropertyType.DOUBLE:
                                value = EditorGUILayout.DoubleField (name, (double)value);
                                break;

                            case PropertyType.LONG:
                                value = EditorGUILayout.LongField (name, (long)value);
                                break;

                            case PropertyType.ENUM:
                                value = EditorGUILayout.EnumPopup (name, (Enum)value);
                                break;

                            // Unity Types

                            case PropertyType.UNITY_OBJECT:
                                value = EditorGUILayout.ObjectField (name, (Object)value, value.GetType(), false);
                                break;

                            case PropertyType.VECTOR2:
                                value = EditorGUILayout.Vector2Field (name, (Vector2)value);
                                break;

                            case PropertyType.VECTOR3:
                                value = EditorGUILayout.Vector3Field (name, (Vector3)value);
                                break;

                            case PropertyType.VECTOR4:
                                value = EditorGUILayout.Vector4Field (name, (Vector4)value);
                                break;

                            case PropertyType.VECTOR2INT:
                                value = EditorGUILayout.Vector2IntField (name, (Vector2Int)value);
                                break;

                            case PropertyType.VECTOR3INT:
                                value = EditorGUILayout.Vector3IntField (name, (Vector3Int)value);
                                break;

                            // Colour

                            case PropertyType.COLOR:
                                value = EditorGUILayout.ColorField (name, (Color)value);
                                break;

                            case PropertyType.GRADIENT:

                                if (value == null)
                                {
                                    arg.value = value = new Gradient ();
                                    m_parameterData[i] = arg;
                                }

                                value = EditorGUILayout.GradientField (name, (Gradient)value);
                                break;

                            // Others

                            case PropertyType.LAYER_MASK:
                                value = (LayerMask)EditorGUILayout.MaskField (name, (LayerMask)value, InternalEditorUtility.layers);
                                break;

                            case PropertyType.ANIMATION_CURVE:
                                if (value == null)
                                {
                                    arg.value = value = new AnimationCurve ();
                                    m_parameterData[i] = arg;
                                }

                                value = EditorGUILayout.CurveField (name, (AnimationCurve)value);
                                break;

                            // Areas

                            case PropertyType.RECT:
                                value = EditorGUILayout.RectField (name, (Rect)value);
                                break;

                            case PropertyType.RECTINT:
                                value = EditorGUILayout.RectIntField (name,(RectInt)value);
                                break;

                            case PropertyType.BOUNDS:
                                value = EditorGUILayout.BoundsField (name, (Bounds)value);
                                break;

                            case PropertyType.BOUNDSINT:
                                value = EditorGUILayout.BoundsIntField (name, (BoundsInt)value);
                                break;

                            default:
                                break;

                        }

                        if (EditorGUI.EndChangeCheck ())
                        {
                            arg.value = value;
                            m_parameterData[i] = arg;
                        }
                    }
                 
                }
                EditorGUILayout.EndHorizontal ();
            }
            else
            {
                if (GUILayout.Button (Attribute.MethodName))
                {
                    CallMethod ();
                }
            }
        }

        protected void CallMethod()
        {
            object[] parameterValues = new object[m_parameterData.Length];

            for (int i = 0; i < parameterValues.Length; i++)
            {
                parameterValues[i] = m_parameterData[i].value;
            }

            MethodInfo.Invoke (_target, parameterValues);
        }
    }
}
