using ArcadeGame.Views.Machines;
using Shared.Editor;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the claw machine.
    /// </summary>
    public class ClawMachine : TokenMachineBase
    {
        public const string Filename = "Claw Machine";

        #region VARIABLE DECLARATIONS

        [SerializeField, Axis] private Vector2 joystickAxis;

        private new ClawMachineAnimator animator => base.animator as ClawMachineAnimator;

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
        private void HandleControllerRotation(Vector2 rotationAxis) => joystickAxis = rotationAxis;

        #endregion

        #region ENGINE

        protected override void OnGameActive()
        {
            animator.AnimateJoystick(in joystickAxis);
        }

        #endregion
    }
}