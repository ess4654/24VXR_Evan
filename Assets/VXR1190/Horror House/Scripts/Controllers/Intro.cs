using System.Collections;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Handles the intro narration for the game.
    /// </summary>
    public class Intro : MonoBehaviour
    {
        [SerializeField] private XROrigin xrRig;
        [SerializeField] private GameObject[] activateOnStart;
        [SerializeField] private AudioSource narrator;
        [SerializeField] private AudioClip[] narratorClips;
        [SerializeField] private TextMeshProUGUI[] narratorText;

        private float length;

        #region METHODS

        private void Start()
        {
            //disable the xr rig
            if (xrRig)
                xrRig.GetComponent<ContinuousMoveProviderBase>().enabled = false;

            //disable world objects
            foreach (var toActivate in activateOnStart)
                if (toActivate)
                    toActivate.SetActive(false);

            StartCoroutine(Narration());
        }

        private IEnumerator Narration()
        {
            yield return new WaitForSeconds(1); //wait 1 second to start

            //wait for all narration clips to play
            for(var i = 0; i < narratorClips.Length; i++)
            {
                ActivateText(i);
                yield return PlayClip(narratorClips[i]);
                Debug.Log($"Done {i}");
            }
            
            StartGame();
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
            
            yield return new WaitUntil(() => narrator.time > 0 && narrator.isPlaying);
            yield return new WaitUntil(() => narrator.time == 0 || narrator.time >= length || !narrator.isPlaying);
            yield return new WaitForSeconds(1.5f);
            
            narrator.Stop();
            narrator.time = 0;
        }

        /// <summary>
        ///     Activates the text for narration.
        /// </summary>
        /// <param name="index">Index of the text to activate.</param>
        private void ActivateText(int index)
        {
            for(var i = 0; i < narratorText.Length; i++)
                if(narratorText[i])
                    narratorText[i].gameObject.SetActive(i == index);
        }

        /// <summary>
        ///     Starts the game, activating the XR Rig.
        /// </summary>
        [ContextMenu("Start Game")]
        private void StartGame()
        {
            //enable the xr rig
            if(xrRig)
                xrRig.GetComponent<ContinuousMoveProviderBase>().enabled = true;

            //enable world objects
            foreach (var toActivate in activateOnStart)
                if (toActivate)
                    toActivate.SetActive(true);

            gameObject.SetActive(false);
        }

        #endregion
    }
}