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
        [SerializeField] private AudioClip[] randomSounds;

        private AudioSource source;
        private float length;

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
            if (randomSounds.Length == 0)
                throw new System.Exception($"No sound clips to select from on {name}");

            yield return new WaitUntil(() => source.time == 0);

            var randomClip = randomSounds.SelectRandom();
            source.clip = randomClip;
            length = randomClip.length;
            source.Play();

            yield return new WaitUntil(() => source.time >= length);
            StartCoroutine(PlayRandomSound());
        }
    }
}