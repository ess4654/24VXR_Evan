using Shared.Editor;
using Shared.Helpers;
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

        [SerializeField, ReadOnly] private bool isFlashing;
        [SerializeField, Min(2)] private int numberFlashes = 10;

        /// <summary>
        ///     Flashes all lights on/off for the given length of time.
        /// </summary>
        /// <param name="time">Amount of time to flash the light.</param>
        /// <returns>Completed flashing task</returns>
        public async Task FlashAll(float time)
        {
            if (isFlashing) return; //we are already flashing
            isFlashing = true;

            //calculate time
            var completeTime = Time.time + time;
            float delay = time / numberFlashes;

            bool newState = true;
            if (lights[0].GetComponentInChildren<Light>())
                newState = !lights[0].GetComponentInChildren<Light>().enabled;

            while (this && Time.time < completeTime)
            {
                UpdateAllLights(newState);
                newState = !newState;

                await Timer.WaitForSeconds(delay);
                await Timer.WaitForFrame();
            }

            //reset the flasher
            UpdateAllLights(false);
            isFlashing = false ;
        }

        #region DEBUGGING
        [Debugging]
        [SerializeField] float flashTime = 3f;
        [SerializeField, InspectorButton("TestFlash")] bool m_Flash;
        public async void TestFlash() => await FlashAll(flashTime);
        #endregion
    }
}