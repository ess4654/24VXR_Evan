using ArcadeGame.Views;
using Shared.Helpers;
using UnityEngine;

namespace Assets.Final_Project.Scripts.Views.Abstract
{
    /// <summary>
    ///     Handles the animation & sound of all jackpot lights for the arcade.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class LightJackpotFlasher : LightFlasher
    {
        private AudioSource sirenSound;

        protected virtual void Awake()
        {
            sirenSound = GetComponent<AudioSource>();
        }

        #region ENGINE

        protected override void OnFlashingStart()
        {
            sirenSound.loop = true;
            sirenSound.Play();
        }

        protected override async void OnFlashingStop()
        {
            //stop the siren sound
            sirenSound.loop = false;

            //wait for the current siren sound to complete before we reset the loop and stop the audio source
            await Timer.WaitForSeconds(sirenSound.clip.length);
            sirenSound.Stop();
            sirenSound.loop = true;
        }

        #endregion
    }
}