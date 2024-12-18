﻿#if UNITY_EDITOR
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Extends the functionality of SerializedProperties
    /// </summary>
    public static class ExtendedSerializedProperty
    {
        /// <summary>
        ///     Gets the object the property represents.
        /// </summary>
        /// <param name="prop">Reference to the property.</param>
        /// <returns>The property object.</returns>
        public static object GetTargetObject(this SerializedProperty prop)
        {
            if(prop == null) return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                    obj = GetValue_Imp(obj, element);
            }

            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;

            var type = source.GetType();
            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            if (GetValue_Imp(source, name) is not IEnumerable ie) return null;

            var enm = ie.GetEnumerator();

            for (int i = 0; i <= index; i++)
                if (!enm.MoveNext()) return null;

            return enm.Current;
        }
    }
}
#endif