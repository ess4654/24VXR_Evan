#if UNITY_EDITOR
using Shared.Helpers.Extensions;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditorInternal;

namespace Shared.Editor
{
    [CustomPropertyDrawer(typeof(DependsUponAttribute))]
    internal sealed class DependsUponAttributeDrawer : PropertyDrawer
    {
        private bool display;
        private UnityEventDrawer eventDrawer = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = (attribute as DependsUponAttribute);
            var attributes = target.attributes;
            var predicates = target.predicates;
            object obj;

            //Attribute check
            display = true;
            for(var i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                bool not = attribute[0] == '!';
                if (not)
                    attribute = attribute[1..];

                var variable = property.serializedObject.FindProperty(attribute);
                if (variable == null) continue;

                obj = i < target.objs.Length ? target.objs[i] : null;

                //determine display value based on variable value of the dependency
                if (variable.propertyType == SerializedPropertyType.Boolean)
                    display &= (obj is bool b) ? (not ? variable.boolValue != b : variable.boolValue == b) : (not ? !variable.boolValue : variable.boolValue);
                else if (variable.propertyType == SerializedPropertyType.Enum && (obj is int || obj is Enum))
                    display &= not ? (variable.enumValueFlag != (int)obj) : (variable.enumValueFlag == (int)obj);
                else if (variable.propertyType == SerializedPropertyType.String)
                    display &= (obj is string || obj is IFormattable) ? (not ? variable.stringValue != obj.ToString() : variable.stringValue == obj.ToString()) : (not ? variable.stringValue.Length == 0 : variable.stringValue.Length > 0);
                else if (variable.propertyType == SerializedPropertyType.Character && obj is char c)
                    display &= not ? variable.contentHash != c : variable.contentHash == c;

                // numbers
                else if (variable.propertyType == SerializedPropertyType.Float)
                    display &= (obj is float f) ? (not ? variable.floatValue != f : variable.floatValue == f) : (not ? variable.floatValue == 0 : variable.floatValue != 0);
                else if (variable.propertyType == SerializedPropertyType.Integer)
                    display &= (obj is int _i) ? (not ? variable.intValue != _i : variable.intValue == _i) : (not ? variable.intValue == 0 : variable.intValue != 0);

                // vectors
                else if (variable.propertyType == SerializedPropertyType.Vector2)
                    display &= (obj is string || obj is IFormattable) ? (not ? variable.vector2Value != ToVector2(obj.ToString()) : variable.vector2Value == ToVector2(obj.ToString())) : (not ? variable.vector2Value == Vector2.zero : variable.vector2Value != Vector2.zero);
                else if (variable.propertyType == SerializedPropertyType.Vector3)
                    display &= (obj is string || obj is IFormattable) ? (not ? variable.vector3Value != ToVector3(obj.ToString()) : variable.vector3Value == ToVector3(obj.ToString())) : (not ? variable.vector3Value == Vector3.zero : variable.vector3Value != Vector3.zero);
                else if (variable.propertyType == SerializedPropertyType.Vector4)
                    display &= (obj is string || obj is IFormattable) ? (not ? variable.vector4Value != ToVector4(obj.ToString()) : variable.vector4Value == ToVector4(obj.ToString())) : (not ? variable.vector4Value == Vector4.zero : variable.vector4Value != Vector4.zero);

                // object reference
                else if (variable.propertyType == SerializedPropertyType.ObjectReference)
                    display &=
                        (obj == null) ?
                        (not ? variable.objectReferenceValue == null : variable.objectReferenceValue != null) :

                        (obj is int _o) ? ((_o == 0 && (not ? variable.objectReferenceValue != null : variable.objectReferenceValue == null)) || (_o != 0 && (not ? variable.objectReferenceValue == null : variable.objectReferenceValue != null))) :
                        (obj is bool _oB && ((!_oB && (not ? variable.objectReferenceValue != null : variable.objectReferenceValue == null)) || (_oB && (not ? variable.objectReferenceValue == null : variable.objectReferenceValue != null))));
            }

            foreach (var predicate in predicates)
                display &= predicate;

            if (display)
            {
                // Use default property drawer.
                if (property.GetTargetObject() is UnityEventBase)
                    eventDrawer.OnGUI(position, property, label);
                else
                    EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (display)
            {
                if (property.GetTargetObject() is UnityEventBase)
                    return eventDrawer.GetPropertyHeight(property, label) + 4;
                return EditorGUI.GetPropertyHeight(property, label) + 4;
            }

            return 0;
        }

        #region VECTOR CONVERSIONS

        private Vector2 ToVector2(string s)
        {
            string[] ss = s.Replace(" ", "").Split(',');
            if (ss.Length < 2) return default;
            if (!float.TryParse(ss[0], out float x) || !float.TryParse(ss[1], out float y))
                return default;
            return new Vector2(x, y);
        }

        private Vector3 ToVector3(string s)
        {
            string[] ss = s.Replace(" ", "").Split(',');
            if (ss.Length < 3) return default;
            if (!float.TryParse(ss[0], out float x)
                || !float.TryParse(ss[1], out float y)
                || !float.TryParse(ss[2], out float z))
                return default;
            return new Vector3(x, y, z);
        }

        private Vector4 ToVector4(string s)
        {
            string[] ss = s.Replace(" ", "").Split(',');
            if (ss.Length < 4) return default;
            if (!float.TryParse(ss[0], out float x)
                || !float.TryParse(ss[1], out float y)
                || !float.TryParse(ss[2], out float z)
                || !float.TryParse(ss[3], out float w))
                return default;
            return new Vector4(x, y, z, w);
        }

        #endregion
    }
}
#endif