using Shared.Helpers.Extensions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the shooter arcade cabinet.
    /// </summary>
    public class ShooterMachine : ShooterArcadeCabinetBase
    {
        /// <summary>
        ///     Called when the machine is interacted with passing the player standing position.
        /// </summary>
        public static event Action<int> OnInteraction;

        [SerializeField] private string gameScene;

        /// <summary>
        ///     The position the player is standing at the machine.
        /// </summary>
        private int playerPosition = -1;

        protected override void Awake()
        {
            base.Awake();
            
            //load arcade machine scene as additive
            if(!gameScene.IsNullOrWhiteSpace())
                SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
        }

        /// <summary>
        ///     Interact with the shooter game.
        /// </summary>
        /// <param name="standingPosition">Standing position of the player.</param>
        public void Interact(int standingPosition)
        {
            playerPosition = standingPosition;
            OnInteraction?.Invoke(playerPosition); //subscribed event
            base.Interact();
        }
    }
}