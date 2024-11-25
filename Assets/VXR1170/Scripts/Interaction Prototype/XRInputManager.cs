using Shared;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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
        public static event AxisEvent OnControllerRotation; //meant to be rotation of the controller, but opted to use the primary axis instead

        /// <summary>
        ///     Called when the trigger button on any controller is pressed.
        /// </summary>
        public static event Action<InputDevice> OnControllerTrigger;

        /// <summary>
        ///     Called when the trigger button on any controller is released.
        /// </summary>
        public static event Action<InputDevice> OnControllerTriggerUp;

        private InputDevice rightController;
        private InputDevice leftController;
        private InputDevice hmd;

        private bool rightTrigger;
        private bool leftTrigger;

        #endregion

        #region METHOD

        private void Update()
        {
            //load devices
            if (!rightController.isValid)
                InitializeDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
            if (!leftController.isValid)
                InitializeDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
            if (!hmd.isValid)
                InitializeDevice(InputDeviceCharacteristics.HeadMounted, ref hmd);

            //get the input from the right device
            bool trigger;
            if (rightController.isValid)
            {
                if(rightController.TryGetFeatureValue(CommonUsages.triggerButton, out trigger))
                {
                    if(trigger)
                    {
                        OnControllerTrigger?.Invoke(rightController);
                        rightTrigger = trigger;
                    }
                    else if(rightTrigger)
                    {
                        OnControllerTriggerUp?.Invoke(rightController);
                        rightTrigger = false;
                    }
                }
            }
            //get the input from the left device
            if (leftController.isValid)
            {
                if(leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rotation))
                {
                    OnControllerRotation?.Invoke(rotation);
                }

                if(leftController.TryGetFeatureValue(CommonUsages.triggerButton, out trigger))
                {
                    if(trigger)
                    {
                        OnControllerTrigger?.Invoke(leftController);
                        leftTrigger = trigger;
                    }
                    else if(leftTrigger)
                    {
                        OnControllerTriggerUp?.Invoke(leftController);
                        leftTrigger = false;
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes the device if it is not valid.
        /// </summary>
        /// <param name="characteristics">Characteristics of the device to retrieve.</param>
        /// <param name="device">Reference to the device.</param>
        private void InitializeDevice(InputDeviceCharacteristics characteristics, ref InputDevice device)
        {
            var devices = new List<InputDevice>();

            //get the desired input devices
            InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

            if (devices.Count > 0)
                device = devices[0];
        }

        #endregion
    }
}