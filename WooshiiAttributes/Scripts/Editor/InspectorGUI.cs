using UnityEditor;
using UnityEngine;

namespace WooshiiAttributes
{
    public static class InspectorGUI
    {
        public const float INSPECTOR_MARGIN_X = 2f;

        // Containers

        // --- Standard ---

        public static void BeginContainer(SerializedProperty property, bool includeChildren)
        {
            BeginContainer (GetContainerRect (property, includeChildren));

        }

        public static void BeginContainer(float height)
        {
            BeginContainer (GetContainerRect (height));
        }

        private static void BeginContainer(Rect rect)
        {
            EditorGUILayout.BeginVertical();
            GUI.Box (rect, "", GUI.skin.box);
        }

        // --- Foldout ---

        public static bool BeginContainerFoldout(float height, bool foldout)
        {
            Rect rect = GetContainerRect (height);
            BeginContainer (rect);

            rect.height = 19f;
            foldout = EditorGUI.Foldout (rect, foldout, "", true);

            return foldout;
        }

        public static void EndInspectorContainer()
        {
            EditorGUILayout.EndVertical ();
        }

        // Rects

        public static Rect GetNextRect()
        {
            EditorGUILayout.BeginVertical ();
            EditorGUILayout.EndVertical ();

            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += rect.height;

            return rect;
        }

        private static Rect GetContainerRect(SerializedProperty property, bool includeChildren)
        {
            Rect rect = GetNextRect ();

            rect.x -= INSPECTOR_MARGIN_X;
            rect.height = EditorGUI.GetPropertyHeight (property, includeChildren);

            return rect;
        }

        private static Rect GetContainerRect(float height)
        {
            Rect rect = GetNextRect ();

            rect.x -= INSPECTOR_MARGIN_X;
            rect.width += INSPECTOR_MARGIN_X * 2f;

            rect.height = height;

            return rect;
        }

        // Debug

        public static void GetLastRectDebug()
        {
            DrawRectDebug (GUILayoutUtility.GetLastRect ());
        }

        public static void DrawRectDebug(Rect rect)
        {
            Handles.DrawSolidRectangleWithOutline (rect, Color.clear, Color.grey);
        }
    }
}
