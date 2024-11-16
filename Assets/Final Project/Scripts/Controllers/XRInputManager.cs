using Shared;
using UnityEngine;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Manages XR Input for the arcade game.
    /// </summary>
    public class XRInputManager : Singleton<XRInputManager>
    {
        public const string Filename = "XR Input Manager";
        protected override bool DontDestroy => true;

        #region DELEGATES

        /// <summary>
        ///     An event that contains an axis value from the input.
        /// </summary>
        /// <param name="axis">The input axis.</param>
        public delegate void AxisEvent(Vector2 axis);

        #endregion

        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Called every frame with the rotation axis of the xr controller.
        /// </summary>
        public static event AxisEvent OnControllerRotation;

        #endregion
    }
}