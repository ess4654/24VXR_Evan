#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shared.Editor
{
    [CustomPropertyDrawer(typeof(AxisAttribute))]
    public class AxisAttributePropertyDrawer : PropertyDrawer
    {
        private const float lineWidth = 2;
        private const float inputWidth = 52;
        private const float padding = 2f;
        private readonly Color lineColor = new Color(.16f, .16f, .16f, .8f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                EditorGUI.BeginChangeCheck();

                var lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;// EditorGUI.GetPropertyHeight(property, label, false) + EditorGUIUtility.standardVerticalSpacing;
                var controlRect = EditorGUI.PrefixLabel(position, label);
                controlRect.height = lineHeight;
                var line = controlRect;
                line.width = lineWidth;
                
                line.x = controlRect.x - padding;
                line.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.DrawRect(line, lineColor);
                line.x = line.x + controlRect.width - lineWidth - inputWidth + padding;
                EditorGUI.DrawRect(line, lineColor);
                line.x = controlRect.x + ((controlRect.width - inputWidth - padding) / 2f) - (lineWidth / 2f);
                EditorGUI.DrawRect(line, lineColor);

                line.x = controlRect.x - padding;
                line.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.DrawRect(line, lineColor);
                line.x = line.x + controlRect.width - lineWidth - inputWidth + padding;
                EditorGUI.DrawRect(line, lineColor);
                line.x = controlRect.x + ((controlRect.width - inputWidth - padding) / 2f) - (lineWidth / 2f);
                EditorGUI.DrawRect(line, lineColor);

                Vector2 vector = property.vector2Value;

                controlRect.x = 0;
                controlRect.width = position.width;
                controlRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;// lineHeight;// + EditorGUIUtility.standardVerticalSpacing;
                
                vector.x = EditorGUI.Slider(controlRect, "X Axis", vector.x, -1, 1);
                controlRect.y += lineHeight + EditorGUIUtility.standardVerticalSpacing;// lineHeight;// + EditorGUIUtility.standardVerticalSpacing;
                vector.y = EditorGUI.Slider(controlRect, "Y Axis", vector.y, -1, 1);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = vector;
                }
            }
            else //variable is not a Vector2
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 2 * (EditorGUIUtility.singleLineHeight/*(EditorGUI.GetPropertyHeight(property, label, true)*/ + EditorGUIUtility.standardVerticalSpacing);//) + 200;
        }
    }
}
#endif