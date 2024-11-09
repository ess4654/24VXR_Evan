using System.Collections;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Handles the intro narration for the game.
    /// </summary>
    public class Intro : MonoBehaviour
    {
        [SerializeField] private XROrigin xrRig;
        [SerializeField] private AudioSource narrator;
        [SerializeField] private AudioClip[] narratorClips;
        [SerializeField] private TextMeshProUGUI[] narratorText;

        private float length;

        #region METHODS

        private void Start() => StartCoroutine(Narration());

        private IEnumerator Narration()
        {
            for(var i = 0; i < narratorClips.Length; i++)
            {
                ActivateText(i);
                yield return PlayClip(narratorClips[i]);
            }
            
            //StartGame();
        }

        /// <summary>
        ///     Plays the audio clip of the narrator.
        /// </summary>
        /// <param name="clip">Clip to play.</param>
        /// <returns>Completed play routine.</returns>
        private IEnumerator PlayClip(AudioClip clip)
        {
            length = clip != null ? clip.length : 0;
            narrator.clip = clip;
            narrator.Play();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => narrator.time == 0 || narrator.time >= length || !narrator.isPlaying);
        }

        /// <summary>
        ///     Activates the text for narration.
        /// </summary>
        /// <param name="index">Index of the text to activate.</param>
        private void ActivateText(int index)
        {
            for(var i = 0; i < narratorText.Length; i++)
                if(narratorText[i])
                    narratorText[i].enabled = i == index;
        }

        /// <summary>
        ///     Starts the game, activating the XR Rig.
        /// </summary>
        [ContextMenu("Start Game")]
        private void StartGame()
        {
            if(xrRig)
                xrRig.gameObject.SetActive(true);
            Destroy(gameObject);
        }

        #endregion
    }
}