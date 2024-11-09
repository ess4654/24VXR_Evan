using HorrorHouse.Data;
using HorrorHouse.Helpers;
using TMPro;
using UnityEngine;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Controls the behaviour of the religious artifacts.
    /// </summary>
    public class Artifact : MonoBehaviour
    {
        [SerializeField] private ArtifactType type;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private AudioClip collectSound;
        [SerializeField] private bool insideTrigger;

        #region METHODS

        private void Awake()
        {
            if (hintText)
            {
                hintText.text = type.ToString().Replace("_", " ");
                hintText.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
            {
                insideTrigger = true;
                if (hintText)
                    hintText.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
            {
                insideTrigger = false;
                if (hintText)
                    hintText.enabled = false;
            }
        }

        /// <summary>
        ///     Collects the artifact adding it to our library.
        /// </summary>
        [ContextMenu("Collect")]
        public void Collect()
        {
            if (insideTrigger)
            {
                GameEventBroadcaster.BroadcastArtifactCollected(type);
                UI_Sounds.PlaySoundFX(collectSound);
                Destroy(gameObject);
            }
        }

        #endregion
    }
}