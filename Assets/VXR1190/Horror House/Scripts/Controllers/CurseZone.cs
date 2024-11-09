using HorrorHouse.Data;
using HorrorHouse.Helpers;
using HorrorHouse.Views;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Handles the curse zone & game over trigger.
    /// </summary>
    public class CurseZone : MonoBehaviour
    {
        [SerializeField] private TMP_Text hint;
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private bool insideTrigger;

        private HashSet<ArtifactType> collectedArtifacts = new();
        private bool gameOver;

        #region METHODS

        #region ENGINE

        private void OnEnable()
        {
            GameEventBroadcaster.OnTriggerInput += TryLiftCurse;
            GameEventBroadcaster.OnArtifactCollected += CollectArtifact;    
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnTriggerInput -= TryLiftCurse;
            GameEventBroadcaster.OnArtifactCollected -= CollectArtifact;    
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
            {
                if (collectedArtifacts.Count == 4)
                    HUD.Instance.ActivateHint(true, "Press The Trigger To Lift Curse");
                insideTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.TAG_PLAYER))
            {
                if (collectedArtifacts.Count == 4)
                    HUD.Instance.ActivateHint(false);
                insideTrigger = false;
            }
        }

        #endregion

        //Called automatically when an artifact is collected.
        private void CollectArtifact(ArtifactType artifactType)
        {
            collectedArtifacts.Add(artifactType);
            hint.text = $"Lift The Curse\n({collectedArtifacts.Count}/4)";
        }

        //attempts to lift the curse when inside the curse region
        private void TryLiftCurse(InputDevice _)
        {
            if(insideTrigger)
                LiftCurse();
        }

        /// <summary>
        ///     Lifts the curse if the artifacts are collected.
        /// </summary>
        [ContextMenu("Lift Curse")]
        public void LiftCurse()
        {
            if(collectedArtifacts.Count == 4)
                GameOver();
        }

        /// <summary>
        ///     Triggers the game over event.
        /// </summary>
        [ContextMenu("Game Over")]
        private void GameOver()
        {
            if(!gameOver)
            {
                UI_Sounds.PlaySoundFX(gameOverSound);
                gameOver = true;
                GameEventBroadcaster.BroadcastGameOver();
            }
        }

        #endregion
    }
}