using ArcadeGame.Views.Machines;
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
        #region VARIABLE DECLARATIONS

        /// <summary>
        ///     Called when the machine is interacted with passing the player standing position.
        /// </summary>
        public static event Action<int> OnInteraction;

        [SerializeField] private string gameScene;

        private new ShooterMachineAnimator animator => base.animator as ShooterMachineAnimator;

        /// <summary>
        ///     The position the player is standing at the machine.
        /// </summary>
        private int playerPosition = -1;

        #endregion

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
            animator.ToggleGun(playerPosition, false); //toggle the gun model at the position off
            base.Interact();
        }
    }
}