using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the claw machine.
    /// </summary>
    public class ClawMachine : TokenMachineBase
    {
        #region VARIABLE DECLARATIONS

        private Vector2 leftControllerAxis;

        #endregion

        #region SETUP

        private void OnEnable()
        {
            XRInputManager.OnControllerRotation += HandleControllerRotation;
        }

        private void OnDisable()
        {
            XRInputManager.OnControllerRotation -= HandleControllerRotation;
        }

        /// <summary>
        ///     Cache the value of the left controller rotation axis for use.
        /// </summary>
        /// <param name="rotationAxis">Axis of rotation.</param>
        private void HandleControllerRotation(Vector2 rotationAxis) => leftControllerAxis = rotationAxis;

        #endregion
    }
}