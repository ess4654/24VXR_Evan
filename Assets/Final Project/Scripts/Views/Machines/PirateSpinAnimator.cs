using ArcadeGame.Helpers.Audio;
using Shared.Editor;
using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcadeGame.Views.Machines
{
    /// <summary>
    ///     Controls the animations for the pirate spin arcade game.
    /// </summary>
    public class PirateSpinAnimator : ArcadeMachineAnimator
    {
        public const string Filename = "Pirate Spin Animator";

        #region VARIABLE DECLARATIONS

        [SerializeField] private string startSpinSoundKey;
        [SerializeField] private RandomAudioPlaylist spinSoundController;
        [SerializeField, Range(0, 1)] private float currentSpinRate;
        [SerializeField] private float minimumSpinDelay = 1;
        [SerializeField] private float maximumSpinDelay = 1;

        private bool isSpinning;

        #endregion

        public async Task Spin()
        {
            if (isSpinning) return;
            isSpinning = true;

            //play bell sound to start the spin routine
            var bellClip = SoundManager.PlayAudioClip(startSpinSoundKey);
            await Timer.WaitForSeconds(bellClip.length);
            await Timer.WaitForSeconds(.2f);

            //start spin
            spinSoundController.Mute(false);

            await Timer.WaitForSeconds(4f);

            //finish spin
            spinSoundController.Mute(true);
            isSpinning = false;
        }

        private void UpdateSpinRate() =>
            spinSoundController.Delay = Mathf.Lerp(maximumSpinDelay, minimumSpinDelay, currentSpinRate);

        [Debugging]
        [SerializeField, InspectorButton("Spin")] bool m_Spin;
    }
}