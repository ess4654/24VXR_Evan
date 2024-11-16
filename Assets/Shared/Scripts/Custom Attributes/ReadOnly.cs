using UnityEngine;

namespace Shared.Editor
{
    /// <summary>
    ///     Add this property tag to a variable to
    ///     prevent it from being changed in the inspector
    /// </summary>
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}