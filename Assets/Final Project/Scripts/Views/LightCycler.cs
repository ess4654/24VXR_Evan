using Shared.Helpers;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Cycles through lights in succession turning one off, and the next on.
    /// </summary>
    public class LightCycler : ArcadeLightAnimator
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private float delay = 0.25f;

        private bool isCycling;
        private bool reverseDirection;
        private int currentIndex;

        #endregion

        #region METHODS

        private void Awake()
        {
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
                {
                    //turn the current light off
                    UpdateLight(currentIndex, false);

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
                }

                //Log("Running");

                //yield return new WaitForSeconds(delay);
                //yield return new WaitForEndOfFrame();
                await Timer.WaitForSeconds(delay);
                await Timer.WaitForFrame();
            }
        }

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

        #endregion
    }
}