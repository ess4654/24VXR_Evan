using UnityEngine;

namespace Shared.Editor
{
    /// <summary>
    ///     Used to create a debugging region in the inspector.
    /// </summary>
    public class DebuggingAttribute : HeaderAttribute
    {
        public DebuggingAttribute() : base("Debugging") { }
    }
}