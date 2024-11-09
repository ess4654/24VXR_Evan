using Shared;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Receives input from the XR devices.
    /// </summary>
    public class XRInputReceiver : Singleton<XRInputReceiver>
    {
        [SerializeField] private ContinuousMoveProviderBase mover;

        private InputDevice rightController;
        private InputDevice leftController;
        private InputDevice hmd;

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

            //get the trigger input from the right device
            if(rightController.isValid && rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggered) && triggered)
            {
                GameEventBroadcaster.BroadcastTriggerInput(rightController);
            }
            //get the trigger input from the left device
            if (leftController.isValid && leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggered) && triggered)
            {
                GameEventBroadcaster.BroadcastTriggerInput(leftController);
            }
            //get the axis input from the left device
            if (leftController.isValid && leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axis))
            {
                if(mover && mover.enabled)
                    GameEventBroadcaster.BroadcastMovement(axis != Vector2.zero);
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