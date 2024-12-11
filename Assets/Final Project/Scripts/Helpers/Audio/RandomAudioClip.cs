using Shared.Editor;
using Shared.Helpers;
using UnityEngine;

namespace ArcadeGame.Helpers.Audio
{
    /// <summary>
    ///     Plays a random audio clip at request
    /// </summary>
    public class RandomAudioClip : Shared.Behaviour
    {
        [SerializeField, DependsUpon("!useSoundKeyInstead")] private AudioClip[] clips;
        [SerializeField] private bool useSoundKeyInstead;
        [SerializeField, DependsUpon("useSoundKeyInstead")] private string[] clipKeys;

        /// <summary>
        ///     Plays a random audio clip.
        /// </summary>
        public void PlayRandomAudio()
        {
            if(useSoundKeyInstead)
                SoundManager.PlayAudioClip(clipKeys.SelectRandom());
            else
                SoundManager.PlayAudioClip(clips.SelectRandom());
        }
    }
}