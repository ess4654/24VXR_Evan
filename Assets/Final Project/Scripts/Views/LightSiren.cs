using Assets.Final_Project.Scripts.Views.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame.Views
{
    /// <summary>
    ///     Handles the animation of jackpot sirens for the arcade.
    /// </summary>
    public class LightSiren : LightJackpotFlasher
    {
        #region VARIABLE DECLARTIONS

        [SerializeField] private Light[] spotlights;
        [SerializeField] private List<Transform> spotlightPivots;
        [SerializeField] private float pivotRotationSpeed = 3f;
        
        private List<int> rotationDirections;

        #endregion

        #region SETUP

        protected override void Awake()
        {
            base.Awake();
            ToggleSpotlights(false);
            rotationDirections = new List<int>();
            spotlightPivots.RemoveAll(x => x == null); //remove all null transforms
        }

        #endregion

        #region UPDATES

        private void Update()
        {
            //rotate siren pivots while we are flashing
            if(isFlashing)
            {
                for (var i = 0; i < spotlightPivots.Count; i++)
                {
                    var pivot = spotlightPivots[i];
                    var direction = rotationDirections[i];
                    pivot.Rotate(direction * pivotRotationSpeed * Vector3.up);
                }
            }
        }

        #endregion

        #region ENGINE

        protected override void OnFlashingStart()
        {
            base.OnFlashingStart();

            rotationDirections.Clear();

            //create random directions of rotation for each spotlight pivot point.
            for(var i = 0; i < spotlightPivots.Count; i++)
                rotationDirections.Add(Random.value <= .5f ? 1 : -1);

            ToggleSpotlights(true);
        }

        protected override void OnFlashingStop()
        {
            ToggleSpotlights(false);

            base.OnFlashingStop();
        }

        /// <summary>
        ///     Toggles all spotlights controlled by the siren on/off.
        /// </summary>
        /// <param name="on">Are the spotlights on or off?</param>
        private void ToggleSpotlights(bool on)
        {
            foreach (var light in spotlights)
            {
                if (light != null)
                    light.enabled = on;
            }
        }

        #endregion
    }
}