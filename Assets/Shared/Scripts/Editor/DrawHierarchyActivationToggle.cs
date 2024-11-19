#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    /// <summary>
    ///     Draws an activation toggle box on GameObjects in the Hierarchy.
    /// </summary>
    [InitializeOnLoad]
    public class DrawHierarchyActivationToggle
    {
        static DrawHierarchyActivationToggle() =>
            EditorApplication.hierarchyWindowItemOnGUI += DrawToggleOnWindowItem;

        private static int temp_iconsDrawedCount;

        static void DrawToggleOnWindowItem(int instanceID, Rect selectionRect)
        {
            temp_iconsDrawedCount = -1;
            GameObject go = null;
            
            #region Draw Activation Toggle
            temp_iconsDrawedCount++;

            go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null)
                return;

            var r = new Rect(selectionRect.xMax - 16 * (temp_iconsDrawedCount + 1) - 20, selectionRect.yMin, 16, 16);

            var wasActive = go.activeSelf;
            var isActive = GUI.Toggle(r, wasActive, "");
            if (wasActive != isActive)
            {
                Undo.RecordObject(go, $"Modified Is Active in {go.name}");
                go.SetActive(isActive);
                if (EditorApplication.isPlaying == false)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
                    EditorUtility.SetDirty(go);
                }
            }
            #endregion
        }
    }
}
#endif