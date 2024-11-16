#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.Editor
{
    [CustomPropertyDrawer(typeof(DebuggingAttribute))]
    internal sealed class DebuggingAttributeDrawer : GenericPropertyDrawer<DebuggingAttribute>
    {
        protected override void OnGUI(Rect position, DebuggingAttribute attribute, GUIContent label)
        {
            EditorGUILayout.Space();

            var style = new GUIStyle { fontStyle = FontStyle.Bold };
            style.normal.textColor = new Color(1, .3f, .3f);
            EditorGUILayout.LabelField(attribute.header, style);
        }
    }
}
#endif