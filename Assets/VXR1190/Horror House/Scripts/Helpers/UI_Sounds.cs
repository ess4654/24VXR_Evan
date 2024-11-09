using Shared;
using UnityEngine;

namespace HorrorHouse.Helpers
{
    /// <summary>
    ///     Handles the playback of UI sounds.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class UI_Sounds : Singleton<UI_Sounds>
    {
        private AudioSource source;

        #region METHODS

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<AudioSource>();
        }

        /// <summary>
        ///     Plays the sound FX on the UI audio channel.
        /// </summary>
        /// <param name="clip">Clip to play.</param>
        public static void PlaySoundFX(AudioClip clip)
        {
            if(Instance && clip != null)
                Instance.source.PlayOneShot(clip);
        }

        #endregion
    }
}