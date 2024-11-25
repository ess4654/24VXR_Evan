using ArcadeGame.Data;
using ArcadeGame.Views.Machines;
using Shared.Editor;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the claw machine.
    /// </summary>
    public class ClawMachine : TokenMachineBase
    {
        public const string Filename = "Claw Machine";

        #region VARIABLE DECLARATIONS

        [Space]
        [SerializeField, ReadOnly] private bool droppingClaw;
        [SerializeField, Axis] private Vector2 joystickAxis;

        private new ClawMachineAnimator animator => base.animator as ClawMachineAnimator;

        #endregion

        #region SETUP

        private void OnEnable()
        {
            XRInputManager.OnControllerRotation += HandleControllerRotation;
            XRInputManager.OnControllerTrigger += HandleControllerTrigger;
        }

        private void OnDisable()
        {
            XRInputManager.OnControllerRotation -= HandleControllerRotation;
            XRInputManager.OnControllerTrigger -= HandleControllerTrigger;
        }

        /// <summary>
        ///     Cache the value of the left controller rotation axis for use.
        /// </summary>
        /// <param name="rotationAxis">Axis of rotation.</param>
        private void HandleControllerRotation(Vector2 rotationAxis) => joystickAxis = rotationAxis;

        /// <summary>
        ///     Check if the input from the right controller is received.
        /// </summary>
        /// <param name="device">Device input data.</param>
        private void HandleControllerTrigger(InputDevice device)
        {
            if(device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                DropClaw();
        }

        #endregion

        /// <summary>
        ///     Drops the claw for the machine and waits for all animation to complete.
        /// </summary>
        private async void DropClaw()
        {
            if(droppingClaw) return;

            if (GameData.State == gameStateOnPlay)
            {
                droppingClaw = true;
                
                await animator.AnimateClawDrop(); //drop animation
                if (this == null) return;

                await animator.AnimatePrizeDrop(); //move to drop region and open claw
                if (this == null) return;
                
                await animator.AnimateClawReturn(); //returning to center point
                if (this == null) return;
                
                joystickAxis = Vector2.zero;
                droppingClaw = false;
                ResetCountdown();
            }
        }

        #region ENGINE

        protected override void OnGameActive()
        {
            if(!droppingClaw)
                animator.AnimateJoystick(in joystickAxis);
        }

        protected override void OnCountdown(float remainingTime)
        {
            //Log(remainingTime);
        }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("DropClaw")] bool m_DropClaw;
        #endregion
    }
}