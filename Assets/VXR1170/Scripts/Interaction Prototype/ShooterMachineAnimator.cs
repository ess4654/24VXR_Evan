using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the animations for the shooter arcade game.
    /// </summary>
    public class ShooterMachineAnimator : ShooterCabinetAnimator
    {
        public const string Filename = "Shooter Machine Animator";

        [SerializeField] private GameObject[] gunModels;

        #region METHODS

        /// <summary>
        ///     Toggles the visibility of the gun on/off.
        /// </summary>
        /// <param name="index">Index of the gun to toggle visibility.</param>
        /// <param name="active">Is the gun visible?</param>
        public void ToggleGun(int index, bool active)
        {
            var model = gunModels[index];
            model.SetActive(active);
        }

        #endregion
    }
}