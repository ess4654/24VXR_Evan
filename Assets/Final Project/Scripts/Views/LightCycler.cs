using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Cycles through lights in succession turning one off, and the next on.
    /// </summary>
    public class LightCycler : ArcadeLightAnimator
    {
        public const string Filename = "Light Cycler";

        #region VARIABLE DECLARATIONS

        [SerializeField] private float delay = 0.25f;

        private bool isCycling;
        private bool reverseDirection;
        private int currentIndex;

        #endregion

        #region SETUP
        
        protected override void Awake()
        {
            base.Awake();

            reverseDirection = Random.value < .5f;
            isCycling = true;
            CycleLight();
        }

        /// <summary>
        ///     Cycles through the lights this script is managing.
        /// </summary>
        /// <returns>Coroutine</returns>
        private async Task CycleLight()
        {
            while (this)
            {
                if (isCycling && lights.Length > 1)
                    SkipNextLight();

                await Timer.WaitForSeconds(delay);
                await Timer.WaitForFrame();
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Toggle the cycler on/off.
        /// </summary>
        /// <param name="isCycling">Is the light to be cycled.</param>
        /// <returns>The index of the light currently activated.</returns>
        public int ToggleCycling(bool isCycling)
        {
            this.isCycling = isCycling;
            return currentIndex;
        }

        /// <summary>
        ///     Skips to the next light in sequence.
        /// </summary>
        /// <returns>The new index of the light once skipped</returns>
        public int SkipNextLight()
        {
            //turn the current light off
            var x = currentIndex;
            UpdateLight(x, false);

            if (reverseDirection)
            {
                currentIndex++;
                currentIndex %= lights.Length;
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = lights.Length - 1;
            }

            //turn the next light on
            UpdateLight(currentIndex, true);

            return currentIndex;
        }

        #endregion
    }
}