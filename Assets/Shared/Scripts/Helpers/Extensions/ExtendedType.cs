using System;
using System.Collections.Generic;

namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Extends the functionality of System.Type
    /// </summary>
    public static class ExtendedType
    {
        /// <summary>
        ///     Gets the type of all scripts that this type inherits from.
        /// </summary>
        /// <param name="type">Reference to the type.</param>
        /// <returns>List of inherited types</returns>
        public static List<Type> GetParentTypes(this Type type)
        {
            var parents = new List<Type>();
            if(type == null) return parents;

            var p = type.BaseType;
            while (p != null)
            {
                parents.Add(p);
                p = p.BaseType;
            }

            return parents;
        }
    }
}