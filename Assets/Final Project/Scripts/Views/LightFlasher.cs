using Shared.Editor;
using Shared.Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Flashes arcade machine lights on/off.
    /// </summary>
    public class LightFlasher : ArcadeLightAnimator
    {
        public const string Filename = "Light Flasher";

        [SerializeField, ReadOnly] protected bool isFlashing;
        [SerializeField, Min(2)] private int numberFlashes = 10;

        #region METHODS

        /// <summary>
        ///     Flashes all lights on/off for the given length of time.
        /// </summary>
        /// <param name="time">Amount of time to flash the light.</param>
        /// <returns>Completed flashing task</returns>
        public Task FlashAll(float time) =>
            FlashingRoutine(time, newState => UpdateAllLights(newState));

        /// <summary>
        ///     Flashes the given light on/off for the given amount of time.
        /// </summary>
        /// <param name="time">Amount of time to flash the light.</param>
        /// <param name="lightIndex">Index of the light to be flashed.</param>
        /// <returns>Completed flashing task</returns>
        public Task FlashLightAtIndex(float time, int lightIndex) =>
            FlashingRoutine(time, newState => UpdateLight(lightIndex, newState));

        /// <summary>
        ///     Flashing routine handles the starting/stopping of light flashing.
        /// </summary>
        /// <param name="time">Amount of time to flash the light.</param>
        /// <param name="flashingAction"></param>
        /// <returns>Completed flashing task</returns>
        private async Task FlashingRoutine(float time, Action<bool> flashingAction)
        {
            if (isFlashing) return; //we are already flashing

            OnFlashingStart(); //engine
            isFlashing = true;

            //calculate time
            var completeTime = Time.time + time;
            float delay = time / numberFlashes;

            bool newState = true;
            if (lights[0].GetComponentInChildren<Light>())
                newState = !lights[0].GetComponentInChildren<Light>().enabled;

            while (this && Time.time < completeTime)
            {
                flashingAction?.Invoke(newState); 
                newState = !newState;

                await Timer.WaitForSeconds(delay);
                await Timer.WaitForFrame();
            }

            //reset the flasher
            flashingAction?.Invoke(false);
            isFlashing = false;
            OnFlashingStop(); //engine
        }

        #endregion

        #region ENGINE

        /// <summary>
        ///     OnFlashingStart is called when the light starts flashing.
        /// </summary>
        protected virtual void OnFlashingStart() { }

        /// <summary>
        ///     OnFlashingStop is called when the light stops flashing.
        /// </summary>
        protected virtual void OnFlashingStop() { }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField] float flashTime = 3f;
        [SerializeField, InspectorButton("TestFlash")] bool m_Flash;
        public async void TestFlash() => await FlashAll(flashTime);
        #endregion
    }
}