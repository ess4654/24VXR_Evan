using Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcadeGame.Helpers.Audio
{
    /// <summary>
    ///     Controls and manages all the game audio for the arcade game.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        protected override bool DontDestroy => true;

        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Container used to tie audio with a string key.
        /// </summary>
        [System.Serializable]
        public struct AudioField
        {
            public string key;
            public AudioClip clip;
        }

        [SerializeField] private List<AudioField> audioFiles;

        private AudioSource uiSource;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            uiSource = GetComponent<AudioSource>();
        }

        #region METHODS

        /// <summary>
        ///     Plays the audio clip in 2D space.
        /// </summary>
        /// <param name="key">Key of the audio source to play.</param>
        public static void PlayAudioClip(string key)
        {
            var file = Instance.audioFiles.FirstOrDefault(x => x.key == key);
            if(file.clip != null)
                Instance.uiSource.PlayOneShot(file.clip);
        }

        #endregion
    }
}