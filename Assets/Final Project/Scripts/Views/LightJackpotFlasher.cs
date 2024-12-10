using ArcadeGame.Views;
using Shared.Helpers;
using System.Threading.Tasks;
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

        protected override void Awake()
        {
            base.Awake();
            sirenSound = GetComponent<AudioSource>();
        }

        #region ENGINE

        protected override void OnFlashingStart()
        {
            sirenSound.loop = true;
            sirenSound.Play();
        }

        protected override async Task OnFlashingStop()
        {
            //stop the siren sound
            sirenSound.loop = false;

            //wait for the current siren sound to complete before we reset the loop and stop the audio source
            await Timer.WaitUntil(() => sirenSound.time == 0 || sirenSound.time >= sirenSound.clip.length || !sirenSound.isPlaying);
            sirenSound.Stop();
            sirenSound.loop = true;
        }

        #endregion
    }
}