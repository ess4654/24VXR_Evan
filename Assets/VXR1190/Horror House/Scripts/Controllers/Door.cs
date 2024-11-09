using HorrorHouse.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Controls the opening/closing of doors.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private List<Animator> animators;

        [Header("Audio")]
        [SerializeField] private AudioSource doorSoundSource;
        [SerializeField] private AudioClip openingSound;
        [SerializeField] private AudioClip closingSound;
        [SerializeField] private AudioClip endSound;

        #region METHODS

        #region ENGINE

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
                Open();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
                Close();
        }

        #endregion

        /// <summary>
        ///     Opens the door.
        /// </summary>
        [ContextMenu("Open")]
        public void Open() => ActivateDoor(true);

        /// <summary>
        ///     Closes the door.
        /// </summary>
        [ContextMenu("Close")]
        public void Close() => ActivateDoor(false);

        /// <summary>
        ///     Activates/Deactivates the door open animation.
        /// </summary>
        /// <param name="active">True to open, false to close.</param>
        private void ActivateDoor(bool active)
        {
            if(animators != null)
            {
                foreach (Animator door in animators)
                {
                    if (door)
                        door.SetBool("Open", active);
                }

                //play the door open sound on the audio source
                if(doorSoundSource)
                {
                    doorSoundSource.clip = active ? openingSound : closingSound;
                    doorSoundSource.Play();

                    if (!active && endSound != null)
                        StartCoroutine(EndSound());
                }
            }
        }

        /// <summary>
        ///     Plays the end sound of a door closing.
        /// </summary>
        /// <returns>Play routine.</returns>
        private IEnumerator EndSound()
        {
            //wait for closing sound to play
            var length = doorSoundSource.clip.length;
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => doorSoundSource.time == 0 || doorSoundSource.time >= length || !doorSoundSource.isPlaying);

            doorSoundSource.clip = endSound;
            doorSoundSource.Play();
        }

        #endregion
    }
}