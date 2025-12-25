using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#pragma warning disable 649, IDE0044

/// <summary>
/// Base ShaderGUI class to add extra functionality to the material preview view.
/// </summary>
public class WooshiiShaderGUI : ShaderGUI
{
    /// <summary>
    /// Full type name for the GUI Preview
    /// </summary>
    private const string GUI_SRC = "PreviewGUI, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    /// <summary>
    /// Full type name for the Model Inspector
    /// </summary>
    private const string MODEL_SRC = "UnityEditor.ModelInspector, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    // - Fields

    private int _selectedMesh = 0;
    private Mesh _targetMesh;

    private bool _isRotating;
    private float _fov;

    private Vector2 _previewDir = new Vector2(120f, -20f);
    private Vector3 _offset;

    private PreviewRenderUtility _previewRenderUtility;

    private FieldInfo _selectedField = null;

    private MethodInfo _renderMeshMethod = null;
    private MethodInfo _dragMethod = null;

    private Type _modelInspectorType = null;
    private Type _previewGUIType = null;

    ~WooshiiShaderGUI()
    {
        //Will leak if the render is not handled after the GUI is disabled
        CleanUpRender ();
    }

    // - Methods

#if UNITY_2018_2_OR_NEWER
    public override void OnClosed(Material material)
    {
        base.OnClosed (material);

        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup ();
            _previewRenderUtility = null;
        }
    }
#endif

    /// <summary>
    /// Draw the custom GUI.
    /// </summary>
    /// <param name="materialEditor">The material editor currently being drawn to.</param>
    /// <param name="properties">The material properties.</param>
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI (materialEditor, properties);
    }

    /// <summary>
    /// Small Preview GUI in the top left corner.
    /// </summary>
    /// <param name="materialEditor">The current material editor.</param>
    /// <param name="rect">The draw rect.</param>
    /// <param name="background">The background style.</param>
    public override void OnMaterialPreviewGUI(MaterialEditor materialEditor, Rect rect, GUIStyle background)
    {
        base.OnMaterialPreviewGUI (materialEditor, rect, background);
    }

    /// <summary>
    /// Handle the GUI of the material preview.
    /// </summary>
    /// <param name="materialEditor">The current material editor.</param>
    /// <param name="rect">The full rect for the interactive preview.</param>
    /// <param name="background">The background data for the interactive preview.</param>
    public override void OnMaterialInteractivePreviewGUI(MaterialEditor materialEditor, Rect rect, GUIStyle background)
    {
        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            EditorGUI.DropShadowLabel (new Rect (rect.x, rect.y, rect.width, 40f), "Material preview \nnot available");
        }
        else
        {
            if (_targetMesh == null)
            {
                base.OnMaterialInteractivePreviewGUI (materialEditor, rect, background);
                return;
            }

            Material mat = materialEditor.target as Material;

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

            _previewDir = (Vector2)_dragMethod.Invoke (_previewGUIType, new object[] { _previewDir, rect });

            if (Event.current.type == EventType.Repaint)
            {
                _previewRenderUtility.BeginPreview (rect, background);
                _renderMeshMethod.Invoke (_modelInspectorType, new object[] { _targetMesh, _previewRenderUtility, mat, null, _previewDir, -1 });
                _previewRenderUtility.EndAndDrawPreview (rect);
            }
        }
    }

    /// <summary>
    /// Handle the GUI of the material preview settings.
    /// </summary>
    /// <param name="materialEditor">The current material editor.</param>
    public override void OnMaterialPreviewSettingsGUI(MaterialEditor materialEditor)
    {
        base.OnMaterialPreviewSettingsGUI (materialEditor);

        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        {
            return;
        }

        EditorGUI.BeginChangeCheck ();

        _targetMesh = (Mesh)EditorGUILayout.ObjectField (_targetMesh, typeof (Mesh), false, GUILayout.MaxWidth (120));
        DrawFOVSlider (materialEditor);

        if (_targetMesh != null)
        {
            if (EditorGUI.EndChangeCheck ())
            {
                if (_selectedField == null)
                {
                    _selectedField = typeof (MaterialEditor).GetField ("m_SelectedMesh", BindingFlags.Instance | BindingFlags.NonPublic);
                }

                //Store mesh selection
                _selectedMesh = (int)_selectedField.GetValue (materialEditor);
            }
        }
    }

    private void CleanUpRender()
    {
        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup ();
            _previewRenderUtility = null;
        }
    }

    private void DrawFOVSlider(MaterialEditor materialEditor)
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