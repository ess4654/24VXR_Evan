#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Extends the functionality of the Component editor
    /// </summary>
    public static class ExtendedComponentEditor
    {
        /// <summary>
        ///     Get the editor icon associated with this Component.
        /// </summary>
        /// <param name="component">Reference to the component.</param>
        /// <returns>Editor icon from component</returns>
        public static Texture2D GetEditorIcon(this Component component)
        {
            if (component == null) return null;
            return EditorGUIUtility.ObjectContent(component, component.GetType()).image as Texture2D;
        }
    }
}
#endif