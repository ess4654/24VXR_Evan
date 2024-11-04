using UnityEngine;

namespace Helpers
{
    /// <summary>
    ///     Adds extension methods to Color struct
    /// </summary>
    public static class ExtendedColor
    {
        /// <summary>
        ///     Changes the alpha value of the referenced color returning a new one.
        /// </summary>
        /// <param name="color">Reference to the color.</param>
        /// <param name="alpha">Alpha value of the color</param>
        /// <returns>New color with updated alpha value</returns>
        public static Color ChangeAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }
    }
}