using System.Collections;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Cycles through lights in succession turning one off, and the next on.
    /// </summary>
    public class LightCycler : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private Material lightOnMaterial;
        [SerializeField] private Material lightOffMaterial;
        [SerializeField] private MeshRenderer[] lights;
        [SerializeField] private float delay = 0.25f;

        private bool isCycling;
        private int currentIndex;

        #endregion

        #region METHODS

        private void Awake()
        {
            isCycling = true;
            StartCoroutine(CycleLight());
        }

        /// <summary>
        ///     Cycles through the lights this script is managing.
        /// </summary>
        /// <returns>Coroutine</returns>
        private IEnumerator CycleLight()
        {
            while (this)
            {
                if (isCycling && lights.Length > 1)
                {
                    //turn the current light off
                    UpdateLight(currentIndex, false);

                    currentIndex++;
                    currentIndex %= lights.Length;

                    //turn the next light on
                    UpdateLight(currentIndex, true);
                }

                yield return new WaitForSeconds(delay);
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        ///     Turn the light at the given index on/off.
        /// </summary>
        /// <param name="index">Index of the light to modify.</param>
        /// <param name="on">Whether the light is on or off.</param>
        private void UpdateLight(int index, bool on)
        {
            var light = lights[index];
            light.material = on ? lightOnMaterial : lightOffMaterial;

            if(light.GetComponentInChildren<Light>())
                light.GetComponentInChildren<Light>().enabled = on;
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