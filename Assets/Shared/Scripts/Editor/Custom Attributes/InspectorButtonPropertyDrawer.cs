#if UNITY_EDITOR
using Shared.Helpers.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Shared.Editor
{
    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonPropertyDrawer : PropertyDrawer
    {
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private MethodInfo eventMethodInfo = null;
        private bool hasArrays;
        private string methodName;

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
            Rect buttonRect = new(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);

            if (GUI.Button(buttonRect, label.text))
            {
                object invokeObject = prop.serializedObject.targetObject;
                if (invokeObject == null) return;
                Type eventOwnerType = invokeObject.GetType();
                string path = prop.propertyPath;
                hasArrays = path.Contains(".Array.data[");
                methodName = inspectorButtonAttribute.MethodName;
                int indexOfPeriod = path.LastIndexOf('.');
                if (indexOfPeriod > 0)
                    path = path[..indexOfPeriod]; //remove the button variable name

                //Get the field from the invoked object
                var fieldPath = GetPath(path);
                do
                {
                    var pathElement = fieldPath.Dequeue();
                    TraversePath(pathElement, ref eventOwnerType, ref invokeObject, fieldPath);
                } while (fieldPath.Count > 0);

                eventMethodInfo = GetMethod(eventOwnerType);

                if (eventMethodInfo != null)
                    eventMethodInfo.Invoke(invokeObject, null);
                else
                    Debug.LogError(string.Format("[Inspector Button]: Unable to find method {0} in {1}", methodName, eventOwnerType));
            }
        }

        private Queue<string> GetPath(string path) =>
            new(path.Split("."));

        private Queue<Type> GetHierarchy(Type type)
        {
            var parentTypes = type.GetParentTypes();
            parentTypes.Insert(0, type);
            return new Queue<Type>(parentTypes);
        }

        private void TraversePath(string pathElement, ref Type eventOwnerType, ref object invokeObject, Queue<string> path)
        {
            var parentTypes = GetHierarchy(eventOwnerType);

            do
            {
                var type = parentTypes.Dequeue();

                if (hasArrays && pathElement == "Array" && invokeObject is IEnumerable list)
                {
                    int i = 0;
                    var index = path.Dequeue().EraseAll("data[", "]").ToInt();
                    var enumerator = list.GetEnumerator();
                
                    while (enumerator.MoveNext()) //iterate through the list until we get to the item
                    {
                        invokeObject = enumerator.Current;
                        if (index == i) break;
                        i++;
                    }

                    eventOwnerType = invokeObject.GetType();
                    return; //found
                }

                var fieldInfo = type.GetField(pathElement, bindingFlags);
                if(fieldInfo != null)
                {
                    if (fieldInfo.FieldType == typeof(bool)) return; //found inspector button
                    eventOwnerType = fieldInfo.FieldType;
                    invokeObject = fieldInfo.GetValue(invokeObject);
                    return; //found
                }

            } while (parentTypes.Count > 0);
        }

        private MethodInfo GetMethod(Type classType) =>
            classType.GetMethod(methodName, bindingFlags, null, new Type[0], new ParameterModifier[0]);
    }
}
#endif