using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

#pragma warning disable 649, IDE0044

public class WooshiiShaderGUI : ShaderGUI
    {

    private bool isRotating;

    // ============ PREVIEW MESH ============
    private int selectedMesh = 0;
    private FieldInfo selectedField = null;
    private Mesh targetMesh;

    private Vector2 m_previewDir = new Vector2 (120f, -20f);
    private PreviewRenderUtility m_previewRenderUtility;
    private float fov;

    // Reflection Fields
    private Type m_modelInspectorType = null;
    private MethodInfo m_renderMeshMethod = null;
    private Type m_previewGUIType = null;
    private MethodInfo m_dragMethod = null;

    private const string GUI_SRC = "PreviewGUI, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
    private const string MODEL_SRC = "UnityEditor.ModelInspector, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

    //Will leak if the render is not handled after the GUI is disabled
    ~WooshiiShaderGUI()
        {
        CleanUpRender ();
        }

    #region Overridden Methods
#if UNITY_2018_2_OR_NEWER
    public override void OnClosed(Material material)
        {
        base.OnClosed (material);

        if (m_previewRenderUtility != null)
            {
            m_previewRenderUtility.Cleanup ();
            m_previewRenderUtility = null;
            }
        }
#endif

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
        base.OnGUI (materialEditor, properties);
        }

    /// <summary>
    /// Small Preview GUI in the top left corner [default]
    /// </summary>
    /// <param name="materialEditor">Current Material Editor</param>
    /// <param name="rect">Full Sized rect of GUI</param>
    /// <param name="background">Background style of GUI</param>
    public override void OnMaterialPreviewGUI(MaterialEditor materialEditor, Rect rect, GUIStyle background)
        {
        base.OnMaterialPreviewGUI (materialEditor, rect, background);
        }

    /// <summary>
    /// Handle the display of the material preview
    /// </summary>
    /// <param name="materialEditor">Current Material Editor</param>
    /// <param name="r">Fullsized rect for the interactive preview</param>
    /// <param name="background">Background data for the interactive preview</param>
    public override void OnMaterialInteractivePreviewGUI(MaterialEditor materialEditor, Rect r, GUIStyle background)
        {
        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            {
            if (Event.current.type != EventType.Repaint)
                return;

            EditorGUI.DropShadowLabel (new Rect (r.x, r.y, r.width, 40f), "Material preview \nnot available");
            }
        else
            {
            if (targetMesh == null)
                {
                base.OnMaterialInteractivePreviewGUI (materialEditor, r, background);
                    return;
                }

            Material mat = materialEditor.target as Material;

            if (m_previewRenderUtility == null)
                {
                m_previewRenderUtility = new PreviewRenderUtility();
                m_previewRenderUtility.AddSingleGO (GameObject.CreatePrimitive (PrimitiveType.Plane));

#if UNITY_2017_1_OR_NEWER
                fov = m_previewRenderUtility.cameraFieldOfView = 30f;
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

            m_previewDir = (Vector2)m_dragMethod.Invoke (m_previewGUIType, new object[] { m_previewDir, r });

            if (Event.current.type == EventType.Repaint)
                {
                m_previewRenderUtility.BeginPreview (r, background);
                m_renderMeshMethod.Invoke (m_modelInspectorType, new object[] { targetMesh, m_previewRenderUtility, mat, null, m_previewDir, -1 });
                m_previewRenderUtility.EndAndDrawPreview (r);
                }

            }
        }

    public override void OnMaterialPreviewSettingsGUI(MaterialEditor materialEditor)
        {
        base.OnMaterialPreviewSettingsGUI (materialEditor);

        if (!ShaderUtil.hardwareSupportsRectRenderTexture)
            return;

        EditorGUI.BeginChangeCheck ();

        targetMesh = (Mesh)EditorGUILayout.ObjectField (targetMesh, typeof (Mesh), false, GUILayout.MaxWidth (120));
        DrawFOVSlider (materialEditor);

        if (targetMesh != null)
            {
            if (EditorGUI.EndChangeCheck())
                {
                if (selectedField == null)
                    selectedField = typeof (MaterialEditor).GetField ("m_SelectedMesh", BindingFlags.Instance | BindingFlags.NonPublic);

                //Store mesh selection
                selectedMesh = (int)selectedField.GetValue (materialEditor);
                }
            }
        }

    #endregion

    private void CleanUpRender()
        {
        if (m_previewRenderUtility != null)
            {
            m_previewRenderUtility.Cleanup ();
            m_previewRenderUtility = null;
            }
        }

    Vector3 offset;

    private void DrawFOVSlider(MaterialEditor materialEditor)
        {
        if (m_previewRenderUtility != null)
            {
            m_previewRenderUtility.lights[0].color = EditorGUILayout.ColorField (m_previewRenderUtility.lights[0].color, GUILayout.MaxWidth (120));
            m_previewRenderUtility.lights[1].color = EditorGUILayout.ColorField (m_previewRenderUtility.lights[1].color, GUILayout.MaxWidth (120));

#if UNITY_2017_1_OR_NEWER
            m_previewRenderUtility.cameraFieldOfView = fov;
#else
			m_previewRenderUtility.m_CameraFieldOfView = fov;
#endif
            }
        }
    }
