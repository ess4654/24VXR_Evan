#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Extends the functionality of the MonoBehaviour editor
    /// </summary>
    public static class ExtendedMonoBehaviourEditor
    {
        /// <summary>
        ///     Get the editor icon associated with this MonoBehaviour.
        /// </summary>
        /// <param name="behaviour">Reference to the behaviour.</param>
        /// <returns>Editor icon from behaviour script</returns>
        public static Texture2D GetEditorIcon(this MonoBehaviour behaviour)
        {
            if (behaviour == null) return null;

            //Gets the behaviour icon
            MonoScript ms = MonoScript.FromMonoBehaviour(behaviour);
            #if UNITY_2021_2_OR_NEWER
            return EditorGUIUtility.GetIconForObject(ms);
            #else
            string assetPath = AssetDatabase.GetAssetPath(ms);
            //string metaPath = AssetDatabase.GetTextMetaFilePathFromAssetPath(assetPath);
            //MonoImporter scriptImporter = AssetImporter.GetAtPath(assetPath) as MonoImporter;
            return AssetDatabase.GetCachedIcon(assetPath) as Texture2D;
            #endif
        }
    }
}
#endif