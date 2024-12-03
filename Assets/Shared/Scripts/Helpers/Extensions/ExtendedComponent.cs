using UnityEngine;

namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Adds extension methods to components
    /// </summary>
    public static class ExtendedComponent
    {
        /// <summary>
        ///     Removes the attached component from the game object.
        /// </summary>
        /// <typeparam name="T">Type of component to remove.</typeparam>
        /// <param name="c">Reference to the component object.</param>
        /// <returns>Whether the component was successfully removed</returns>
        public static bool RemoveComponent<T>(this Component c)
        where T : Component
        {
            if (c == null) return false;

            if (c.TryGetComponent(out T attachedComponent))
            {
                Object.Destroy(attachedComponent);
                return true;
            }

            return false;
        }
    }
}