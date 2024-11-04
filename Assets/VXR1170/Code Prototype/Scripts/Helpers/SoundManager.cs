using Shared;
using UnityEngine;

namespace FryingPanGame.Helpers
{
    /// <summary>
    ///     Manages sound clips for the frying pan game.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        private AudioSource source;

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<AudioSource>();
        }

        /// <summary>
        ///     Plays the clip on the sound manager.
        /// </summary>
        /// <param name="clip">Reference to the audio clip to play.</param>
        public void PlayClip(AudioClip clip)
        {
            if(clip != null)
                source.PlayOneShot(clip);
        }
    }
}