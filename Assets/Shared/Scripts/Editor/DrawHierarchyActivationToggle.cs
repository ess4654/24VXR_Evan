/*
 EDITOR TOOLS CLASS
 DRAW HIERARCHY ACTIVATION TOGGLE
 v1.1
 LAST EDITED: TUESDAY MARCH 21, 2023
 COPYRIGHT © TECH SKULL STUDIOS
*/

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
        //private static bool temp_alternatingDrawed;
        //private static InstanceInfo currentItem;
        //private static bool drawedPrefabOverlay;

        static void DrawToggleOnWindowItem(int instanceID, Rect selectionRect)
        {
            //skips early if item is not registered or not valid
            //if (!sceneGameObjects.ContainsKey(instanceID)) return;
            
            //currentItem = sceneGameObjects[instanceID];
            temp_iconsDrawedCount = -1;
            GameObject go = null;
            
            //if (instanceID == firstInstanceID)
            //    temp_alternatingDrawed = currentItem.nestingGroup % 2 == 0;

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