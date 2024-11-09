using HorrorHouse.Data;
using HorrorHouse.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        private void OnEnable()
        {
            GameEventBroadcaster.OnArtifactCollected += CollectArtifact;    
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnArtifactCollected -= CollectArtifact;    
        }

        //Called automatically when an artifact is collected.
        private void CollectArtifact(ArtifactType artifactType)
        {
            collectedArtifacts.Add(artifactType);
            hint.text = $"Lift The Curse\n({collectedArtifacts.Count}/4)";
        }

        [ContextMenu("Lift Curse")]
        public void LiftCurse()
        {
            if(collectedArtifacts.Count == 4 && insideTrigger)
                GameOver();
        }

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