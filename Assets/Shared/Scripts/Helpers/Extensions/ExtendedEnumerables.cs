using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared.Helpers
{
    /// <summary>
    ///     Adds extension methods to lists and other enumerables
    /// </summary>
    public static class ExtendedEnumerables
    {
        /// <summary>
        ///     Select a random element from a list of elements.
        /// </summary>
        /// <typeparam name="T">Type of items in the list.</typeparam>
        /// <param name="list">Reference to the list.</param>
        /// <returns>Random item selected from list</returns>
        public static T SelectRandom<T>(this IEnumerable<T> list)
        {
            if (list == null) return default;
            return list.ElementAt(Random.Range(0, list.Count()));
        }
    }
}