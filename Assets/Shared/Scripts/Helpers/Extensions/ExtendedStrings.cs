namespace Shared.Helpers.Extensions
{
    /// <summary>
    ///     Extends the functionality of String types
    /// </summary>
    public static class ExtendedStrings
    {
        /// <summary>
        ///     Converts a string into an int.
        /// </summary>
        /// <param name="s">String to convert.</param>
        /// <returns>String as an int.</returns>
        public static int ToInt(this string s)
        {
            if (s == null) return default;

            int.TryParse(s, out int i);

            return i;
        }

        /// <summary>
        ///     Returns whether or not a string is Null or is just Whitespace.
        /// </summary>
        /// <param name="s">Reference to the string.</param>
        /// <returns>True if string is null or whitespace.</returns>
        public static bool IsNullOrWhiteSpace(this string s) =>
            string.IsNullOrWhiteSpace(s);

        /// <summary>
        ///     Returns whether or not a string is Null or is Empty.
        /// </summary>
        /// <param name="s">Reference to the string.</param>
        /// <returns>True if string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string s) =>
            string.IsNullOrEmpty(s);

        /// <summary>
        ///     Returns the string with all instances of the given value removed.
        /// </summary>
        /// <param name="s">Reference to the string.</param>
        /// <param name="value">Substring to remove.</param>
        /// <returns>Filtered string.</returns>
        public static string Erase(this string s, string value) =>
            s != null ? s.Replace(value, "") : string.Empty;

        /// <summary>
        ///     Returns the string with all instances of a series of given values removed.
        /// </summary>
        /// <param name="s">Reference to the string.</param>
        /// <param name="values">Substrings to remove as params.</param>
        /// <returns>Filtered string.</returns>
        public static string EraseAll(this string s, params string[] values)
        {
            if (s == null) return string.Empty;

            foreach (var ss in values)
                s = s.Erase(ss);

            return s;
        }

        /// <summary>
        ///     Check whether or not the value matches regardless of case sensitivity.
        ///     ie. "Fishing Rod" matches and is equivalent to "fishing rod"
        /// </summary>
        /// <param name="s">Reference to the string.</param>
        /// <param name="value">Value to compare.</param>
        /// <returns>True if the value of both strings match.</returns>
        public static bool Matches(this string s, string value) =>
            s != null ? s.ToLower() == value.ToLower() : value == null;
    }
}