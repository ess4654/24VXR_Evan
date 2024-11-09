using HorrorHouse.Data;
using UnityEngine.XR;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Broadcasts game events that classes can subscribe to for the horror house game.
    /// </summary>
    public static class GameEventBroadcaster
    {
        #region DELEGATES

        /// <summary>
        ///     An event that passes an input device to a method.
        /// </summary>
        /// <param name="device">The input device interacted with.</param>
        public delegate void InputDeviceEvent(InputDevice device);

        /// <summary>
        ///     An event that passes a collected artifact type.
        /// </summary>
        /// <param name="artifactType">Type of artifact that was collected.</param>
        public delegate void CollectArtifactEvent(ArtifactType artifactType);

        /// <summary>
        ///     An event that sends the player location to listeners.
        /// </summary>
        /// <param name="location">The location to broadcast.</param>
        public delegate void PlayerLocationEvent(PlayerLocation location);

        /// <summary>
        ///     An event that triggers a void action.
        /// </summary>
        public delegate void VoidEvent();

        /// <summary>
        ///     An event that triggers a boolean action.
        /// </summary>
        /// <param name="value">Value of the boolean.</param>
        public delegate void BoolEvent(bool value);

        #endregion

        #region SUBSCRIBABLES
        
        /// <summary>
        ///     Called when the trigger is activated on either the right or left controller.
        /// </summary>
        public static event InputDeviceEvent OnTriggerInput;

        /// <summary>
        ///     Called when the player collects an artifact.
        /// </summary>
        public static event CollectArtifactEvent OnArtifactCollected;

        /// <summary>
        ///     Called when the player changes location.
        /// </summary>
        public static event PlayerLocationEvent OnPlayerLocationChanged;
        
        /// <summary>
        ///     Called when the player lifts the curse.
        /// </summary>
        public static event VoidEvent OnGameOver;

        /// <summary>
        ///     Called when the player is moving or standing still.
        /// </summary>
        public static event BoolEvent OnPlayerMoving;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcasts that an input trigger has been pressed.
        /// </summary>
        /// <param name="device">The device the trigger was activated on.</param>
        public static void BroadcastTriggerInput(InputDevice device) =>
            OnTriggerInput?.Invoke(device);

        /// <summary>
        ///     Broadcasts that player has collected an artifact.
        /// </summary>
        /// <param name="artifactType">The type of artifact collected.</param>
        public static void BroadcastArtifactCollected(ArtifactType artifactType) =>
            OnArtifactCollected?.Invoke(artifactType);

        /// <summary>
        ///     Broadcasts that the player has changed location.
        /// </summary>
        /// <param name="location">Location of the player.</param>
        public static void BroadcastPlayerLocation(PlayerLocation location) =>
            OnPlayerLocationChanged?.Invoke(location);

        /// <summary>
        ///     Broadcasts that the curse has been lifted.
        /// </summary>
        public static void BroadcastGameOver() =>
            OnGameOver?.Invoke();

        /// <summary>
        ///     Broadcasts whether the player is moving or not.
        /// </summary>
        /// <param name="moving">True if moving, false if not.</param>
        public static void BroadcastMovement(bool moving) =>
            OnPlayerMoving?.Invoke(moving);

        #endregion
    }
}