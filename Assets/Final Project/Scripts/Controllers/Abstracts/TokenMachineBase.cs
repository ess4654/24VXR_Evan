using ArcadeGame.Data;
using static ArcadeGame.Data.Constants;
using ArcadeGame.Views.Machines;
using System.Collections;
using Shared.Editor;
using Shared.Helpers.Extensions;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Base class used by all arcade machines that require tokens to play.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public abstract class TokenMachineBase : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [Tooltip("This key is used to save/load the number of tokens deposited into the machine.")]
        [SerializeField] protected string saveKey;

        [Header("Machine Settings")]
        [SerializeField] protected GameState gameStateOnPlay;
        [SerializeField, Range(1, MaxMachineTokens)] private int requiredTokens;
        
        [SerializeField] private bool hasTimer;
        [SerializeField, DependsUpon("hasTimer")] protected float gameTime = 30;

        [Header("Animation")]
        [SerializeField] protected ArcadeMachineAnimator animator;

        [Header("HUD")]
        [SerializeField] private string UI_Panel;

        private int depositedTokens;
        private float countdown;
        
        private const string tokenKey = "DepositedTokens";

        #endregion

        #region SETUP
        
        protected virtual void Awake()
        {
            LoadData();
            StartCoroutine(Countdown());
        }

        /// <summary>
        ///     Loads the data for this machine.
        /// </summary>
        protected virtual void LoadData()
        {
            if (!saveKey.IsNullOrWhiteSpace())
                depositedTokens = PlayerPrefs.GetInt(saveKey + tokenKey, 0);
        }

        #endregion

        #region UPDATES

        protected virtual void Update()
        {
            if (GameData.State == gameStateOnPlay)
                OnGameActive(); //engine
        }

        private IEnumerator Countdown()
        {
            while (this)
            {
                if (hasTimer && GameData.State == gameStateOnPlay && countdown > 0)
                {
                    countdown--;
                    OnCountdown(countdown);
                }
                yield return new WaitForSeconds(1);
            }
        }

        #endregion

        #region ENGINE

        /// <summary>
        ///     OnGameStart is called when the arcade game is first interacted with.
        /// </summary>
        protected virtual void OnGameStart() { }

        /// <summary>
        ///     OnGameActive is called every frame if we are engaged with the machine.
        /// </summary>
        protected virtual void OnGameActive() { }

        /// <summary>
        ///     OnCountdown is called every second while the countdown clock is greater than 0, and the game is active.
        /// </summary>
        /// <param name="remainingTime">Remaining amount of time of the countdown clock.</param>
        protected virtual void OnCountdown(float remainingTime) { }

        /// <summary>
        ///     OnGameStop is called when the player has left the arcade game.
        /// </summary>
        protected virtual void OnGameStop() { }

        #endregion

        #region METHODS

        /// <summary>
        ///     Engage in interaction with the machine.
        /// </summary>
        public void Interact()
        {
            GameData.State = gameStateOnPlay;
            ResetCountdown();
            OnGameStart(); //engine
        }

        /// <summary>
        ///     Resets the countdown on the timer clock.
        /// </summary>
        protected void ResetCountdown()
        {
            if (hasTimer && GameData.State == gameStateOnPlay)
                countdown = gameTime + 1;
        }

        /// <summary>
        ///     Leaves the arcade machine and regains control to the player input.
        /// </summary>
        public void Leave()
        {
            countdown = 0;
            GameData.State = GameState.Arcade;
            PlayerMover.Instance.ActivatePlayerInput(true);
            OnGameStop(); //engine
        }

        #endregion
    }
}