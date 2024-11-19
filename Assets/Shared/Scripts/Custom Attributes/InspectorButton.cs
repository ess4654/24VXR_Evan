using System;
using UnityEngine;

namespace Shared.Editor
{
    /// <summary>
    ///     This attribute can only be applied to fields because its
    ///     associated <see cref="PropertyDrawer"/> only operates on fields (either
    ///     public or tagged with the [<see cref="SerializeField"/>] attribute) in
    ///     the target <see cref="MonoBehaviour"/>.
    /// </summary>
    /// <remarks>
    ///     Note: the function invoked must be of type void and contain no parameters.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public const float DefaultButtonWidth = 200;

        public readonly string MethodName;

        public float ButtonWidth => _buttonWidth;
        private readonly float _buttonWidth = DefaultButtonWidth;

        /// <summary>
        ///     Add to a serialized variable to convert it to a button.
        /// </summary>
        /// <param name="methodName">Name of the void method invoked when the button is clicked.</param>
        public InspectorButtonAttribute(string methodName) =>
            MethodName = methodName;

        /// <summary>
        ///     Add to a serialized variable to convert it to a button.
        /// </summary>
        /// <param name="methodName">Name of the void method invoked when the button is clicked.</param>
        /// <param name="buttonWidth">Width of the button shown in the inspector.</param>
        public InspectorButtonAttribute(string methodName, float buttonWidth)
        {
            MethodName = methodName;
            _buttonWidth = buttonWidth;
        }
    }
}