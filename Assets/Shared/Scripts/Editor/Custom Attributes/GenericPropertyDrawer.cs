#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.Editor
{
    /// <summary>
    ///     Base class used by generic property drawers.
    /// </summary>
    /// <typeparam name="T">Type of property attribute this drawer defines.</typeparam>
    internal abstract class GenericPropertyDrawer<T> : PropertyDrawer
    where T : PropertyAttribute
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            OnGUI(position, attribute as T, label);
            EditorGUILayout.PropertyField(property);
        }

        protected virtual void OnGUI(Rect position, T attribute, GUIContent label) { }
    }
}
#endif