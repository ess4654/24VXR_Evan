using HorrorHouse.Controllers;
using HorrorHouse.Data;
using Shared;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HorrorHouse.Views
{
    /// <summary>
    ///     Manages the HUD of the Horror House game.
    /// </summary>
    public class HUD : Singleton<HUD>
    {
        [SerializeField] private InventoryButton bookUI;
        [SerializeField] private InventoryButton waterUI;
        [SerializeField] private InventoryButton steakUI;
        [SerializeField] private InventoryButton crossUI;
        [SerializeField] private TextMeshProUGUI gameOverText;

        private HashSet<ArtifactType> collectedArtifacts = new();

        #region METHODS

        protected override void Awake()
        {
            base.Awake();
            if(gameOverText)
                gameOverText.enabled = false;
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnArtifactCollected += ArtifactCollected;
            GameEventBroadcaster.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnArtifactCollected -= ArtifactCollected;
            GameEventBroadcaster.OnGameOver -= GameOver;
        }

        /// <summary>
        ///     Activates the artifact icon in the UI.
        /// </summary>
        /// <param name="artifactType">Type of artifact collected.</param>
        private void ArtifactCollected(ArtifactType artifactType)
        {
            if(!collectedArtifacts.Contains(artifactType))
            {
                switch(artifactType)
                {
                    case ArtifactType.Bible:
                        if (bookUI)
                            bookUI.Activate();
                        break;

                    case ArtifactType.Holy_Water:
                        if(waterUI)
                            waterUI.Activate();
                        break;

                    case ArtifactType.Exorcist_Stake:
                        if(steakUI)
                            steakUI.Activate();
                        break;

                    case ArtifactType.Cross:
                        if (crossUI)
                            crossUI.Activate();
                        break;
                }
            }

            collectedArtifacts.Add(artifactType);
        }

        /// <summary>
        ///     Called when the curse is lifted.
        /// </summary>
        private void GameOver()
        {
            if (gameOverText)
                gameOverText.enabled = true;
        }

        #endregion
    }
}