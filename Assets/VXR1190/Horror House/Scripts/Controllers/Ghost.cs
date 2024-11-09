using HorrorHouse.Data;
using Shared.Helpers;
using System.Collections;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Manages the ghost that haunts this house.
    /// </summary>
    public class Ghost : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float showChance = .5f;
        [SerializeField] private Renderer mesh;
        
        [Header("Audio")]
        [SerializeField] private AudioClip[] screams;
        [SerializeField] private AudioClip jumpScare;
        [SerializeField] private AudioSource scream;

        private bool visible;

        #region METHODS

        private void Awake() => Hide();

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Constants.TAG_PLAYER) && Random.value <= showChance)
                StartCoroutine(Show());
        }

        /// <summary>
        ///     Shows the ghost.
        /// </summary>
        /// <returns>Asynchronous routine</returns>
        private IEnumerator Show()
        {
            if (!visible)
            {
                //play scream sound
                if(scream && screams.Length > 0)
                {
                    if (jumpScare != null)
                        scream.PlayOneShot(jumpScare);
                    scream.clip = screams.SelectRandom();
                    scream.Play();
                }
                if(mesh)
                    mesh.enabled = true;
                
                visible = true;
                yield return new WaitForSeconds(3);
                Hide();
            }
        }

        /// <summary>
        ///     Hides the ghost from view.
        /// </summary>
        private void Hide()
        {
            if(scream)
                scream.Stop();
            if(mesh)
                mesh.enabled = false;

            visible = false;
        }

        #endregion
    }
}