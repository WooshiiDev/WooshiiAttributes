using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#pragma warning disable 649, IDE0044

public class WooshiiShaderGUI : ShaderGUI
{
    private const string GUI_SRC = "PreviewGUI, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
    private const string MODEL_SRC = "UnityEditor.ModelInspector, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    // State

    private bool _isRotating;

    // Mesh 

    private int _selectedMesh = 0;

    private Mesh _targetMesh;

    private Vector2 _previewDir = new Vector2 (120f, -20f);
    private Vector3 _offset;

    private PreviewRenderUtility _previewRenderUtility;
    private float _fov;

    // Reflection Fields

    private FieldInfo _selectedField = null;

    private MethodInfo _renderMeshMethod = null;
    private MethodInfo _dragMethod = null;

    private Type _modelInspectorType = null;
    private Type _previewGUIType = null;

    //Will leak if the render is not handled after the GUI is disabled

    ~WooshiiShaderGUI()
    {
        CleanUpRender ();
    }

    #region Overridden Methods

#if UNITY_2018_2_OR_NEWER

    public override void OnClosed(Material _material)
    {
        base.OnClosed (_material);

        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup ();
            _previewRenderUtility = null;
        }
    }

#endif

    public override void OnGUI(MaterialEditor _materialEditor, MaterialProperty[] _properties)
    {
        base.OnGUI (_materialEditor, _properties);
    }

    /// <summary>
    /// Small Preview GUI in the top left corner [default]
    /// </summary>
    /// <param name="_materialEditor">Current Material Editor</param>
    /// <param name="_rect">Full Sized rect of GUI</param>
    /// <param name="_background">Background style of GUI</param>
    public override void OnMaterialPreviewGUI(MaterialEditor _materialEditor, Rect _rect, GUIStyle _background)
    {
        base.OnMaterialPreviewGUI (_materialEditor, _rect, _background);
    }

    /// <summary>
    /// Handle the display of the material preview
    /// </summary>
    /// <param name="_materialEditor">Current Material Editor</param>
    /// <param name="_rect">Fullsized rect for the interactive preview</param>
    /// <param name="_background">Background data for the interactive preview</param>
    public override void OnMaterialInteractivePreviewGUI(MaterialEditor _materialEditor, Rect _rect, GUIStyle _background)
    {
        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            EditorGUI.DropShadowLabel (new Rect (_rect.x, _rect.y, _rect.width, 40f), "Material preview \nnot available");
        }
        else
        {
            if (_targetMesh == null)
            {
                base.OnMaterialInteractivePreviewGUI (_materialEditor, _rect, _background);
                return;
            }

            Material mat = _materialEditor.target as Material;

            if (_previewRenderUtility == null)
            {
                _previewRenderUtility = new PreviewRenderUtility ();
                _previewRenderUtility.AddSingleGO (GameObject.CreatePrimitive (PrimitiveType.Plane));

#if UNITY_2017_1_OR_NEWER
                _fov = _previewRenderUtility.cameraFieldOfView = 30f;
#else
			    fov = m_previewRenderUtility.m_CameraFieldOfView = 30f;
#endif
            }

            if (_previewGUIType == null)
            {
                _previewGUIType = Type.GetType (GUI_SRC);
                _dragMethod = _previewGUIType.GetMethod ("Drag2D", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (_modelInspectorType == null)
            {
                _modelInspectorType = Type.GetType (MODEL_SRC);
                _renderMeshMethod = _modelInspectorType.GetMethod ("RenderMeshPreview", BindingFlags.Static | BindingFlags.NonPublic);
            }

            _previewDir = (Vector2)_dragMethod.Invoke (_previewGUIType, new object[] { _previewDir, _rect });

            if (Event.current.type == EventType.Repaint)
            {
                _previewRenderUtility.BeginPreview (_rect, _background);
                _renderMeshMethod.Invoke (_modelInspectorType, new object[] { _targetMesh, _previewRenderUtility, mat, null, _previewDir, -1 });
                _previewRenderUtility.EndAndDrawPreview (_rect);
            }
        }
    }

    public override void OnMaterialPreviewSettingsGUI(MaterialEditor _materialEditor)
    {
        base.OnMaterialPreviewSettingsGUI (_materialEditor);

        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        {
            return;
        }

        EditorGUI.BeginChangeCheck ();

        _targetMesh = (Mesh)EditorGUILayout.ObjectField (_targetMesh, typeof (Mesh), false, GUILayout.MaxWidth (120));
        DrawFOVSlider (_materialEditor);

        if (_targetMesh != null)
        {
            if (EditorGUI.EndChangeCheck ())
            {
                if (_selectedField == null)
                {
                    _selectedField = typeof (MaterialEditor).GetField ("m_SelectedMesh", BindingFlags.Instance | BindingFlags.NonPublic);
                }

                //Store mesh selection
                _selectedMesh = (int)_selectedField.GetValue (_materialEditor);
            }
        }
    }

    #endregion Overridden Methods

    private void CleanUpRender()
    {
        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup ();
            _previewRenderUtility = null;
        }
    }

    private void DrawFOVSlider(MaterialEditor _materialEditor)
    {
        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.lights[0].color = EditorGUILayout.ColorField (_previewRenderUtility.lights[0].color, GUILayout.MaxWidth (120));
            _previewRenderUtility.lights[1].color = EditorGUILayout.ColorField (_previewRenderUtility.lights[1].color, GUILayout.MaxWidth (120));

#if UNITY_2017_1_OR_NEWER
            _previewRenderUtility.cameraFieldOfView = _fov;
#else
			m_previewRenderUtility.m_CameraFieldOfView = fov;
#endif
        }
    }
}