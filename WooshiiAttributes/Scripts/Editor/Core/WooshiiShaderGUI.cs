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

    private bool m_isRotating;

    // Mesh 
    private int m_selectedMesh = 0;

    private Mesh m_targetMesh;

    private Vector2 m_previewDir = new Vector2 (120f, -20f);
    private Vector3 m_offset;

    private PreviewRenderUtility m_previewRenderUtility;
    private float m_fov;

    // Reflection Fields

    private FieldInfo selectedField = null;

    private MethodInfo m_renderMeshMethod = null;
    private MethodInfo m_dragMethod = null;

    private Type m_modelInspectorType = null;
    private Type m_previewGUIType = null;


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

        if (m_previewRenderUtility != null)
        {
            m_previewRenderUtility.Cleanup ();
            m_previewRenderUtility = null;
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
            if (m_targetMesh == null)
            {
                base.OnMaterialInteractivePreviewGUI (_materialEditor, _rect, _background);
                return;
            }

            Material mat = _materialEditor.target as Material;

            if (m_previewRenderUtility == null)
            {
                m_previewRenderUtility = new PreviewRenderUtility ();
                m_previewRenderUtility.AddSingleGO (GameObject.CreatePrimitive (PrimitiveType.Plane));

#if UNITY_2017_1_OR_NEWER
                m_fov = m_previewRenderUtility.cameraFieldOfView = 30f;
#else
			    fov = m_previewRenderUtility.m_CameraFieldOfView = 30f;
#endif
            }

            if (m_previewGUIType == null)
            {
                m_previewGUIType = Type.GetType (GUI_SRC);
                m_dragMethod = m_previewGUIType.GetMethod ("Drag2D", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (m_modelInspectorType == null)
            {
                m_modelInspectorType = Type.GetType (MODEL_SRC);
                m_renderMeshMethod = m_modelInspectorType.GetMethod ("RenderMeshPreview", BindingFlags.Static | BindingFlags.NonPublic);
            }

            m_previewDir = (Vector2)m_dragMethod.Invoke (m_previewGUIType, new object[] { m_previewDir, _rect });

            if (Event.current.type == EventType.Repaint)
            {
                m_previewRenderUtility.BeginPreview (_rect, _background);
                m_renderMeshMethod.Invoke (m_modelInspectorType, new object[] { m_targetMesh, m_previewRenderUtility, mat, null, m_previewDir, -1 });
                m_previewRenderUtility.EndAndDrawPreview (_rect);
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

        m_targetMesh = (Mesh)EditorGUILayout.ObjectField (m_targetMesh, typeof (Mesh), false, GUILayout.MaxWidth (120));
        DrawFOVSlider (_materialEditor);

        if (m_targetMesh != null)
        {
            if (EditorGUI.EndChangeCheck ())
            {
                if (selectedField == null)
                {
                    selectedField = typeof (MaterialEditor).GetField ("m_SelectedMesh", BindingFlags.Instance | BindingFlags.NonPublic);
                }

                //Store mesh selection
                m_selectedMesh = (int)selectedField.GetValue (_materialEditor);
            }
        }
    }

    #endregion Overridden Methods

    private void CleanUpRender()
    {
        if (m_previewRenderUtility != null)
        {
            m_previewRenderUtility.Cleanup ();
            m_previewRenderUtility = null;
        }
    }

    private void DrawFOVSlider(MaterialEditor _materialEditor)
    {
        if (m_previewRenderUtility != null)
        {
            m_previewRenderUtility.lights[0].color = EditorGUILayout.ColorField (m_previewRenderUtility.lights[0].color, GUILayout.MaxWidth (120));
            m_previewRenderUtility.lights[1].color = EditorGUILayout.ColorField (m_previewRenderUtility.lights[1].color, GUILayout.MaxWidth (120));

#if UNITY_2017_1_OR_NEWER
            m_previewRenderUtility.cameraFieldOfView = m_fov;
#else
			m_previewRenderUtility.m_CameraFieldOfView = fov;
#endif
        }
    }
}