using Shared.Helpers;
using System.Collections;
using UnityEngine;

namespace HorrorHouse.Helpers
{
    /// <summary>
    ///     Plays a random audio clip from the list,
    ///     when done, it continues to select new random clips to play.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlaylist : MonoBehaviour
    {
        [SerializeField] private float playbackTime;
        [SerializeField] private float length;
        [SerializeField] private AudioClip[] randomSounds;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            StartCoroutine(PlayRandomSound());
        }

        /// <summary>
        ///     Plays a random sound from the list of available clips.
        /// </summary>
        /// <returns>Asynchronous routine</returns>
        private IEnumerator PlayRandomSound()
        {
            //if no audio clips are found, throw an error
            if (randomSounds.Length == 0)
                throw new System.Exception($"No sound clips to select from on {name}");

            //select a random audio clip to play
            var randomClip = randomSounds.SelectRandom();
            source.clip = randomClip;
            length = randomClip.length;
            source.Play();

            yield return new WaitForEndOfFrame();

            //wait for playback time to exceed the clip length or reset to 0
            yield return new WaitUntil(() =>
            {
                playbackTime = source.time;
                return playbackTime >= length || playbackTime == 0 || !source.isPlaying;
            });

            StartCoroutine(PlayRandomSound());
        }
    }
}