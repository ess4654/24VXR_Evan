using ArcadeGame.Data;
using static ArcadeGame.Data.Constants;
using ArcadeGame.Views.Machines;
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
        [SerializeField, DependsUpon("hasTimer")] private float gameTime = 30;

        [Header("Animation")]
        [SerializeField] protected ArcadeMachineAnimator animator;

        //[Header("Audio Settings")]
        //[SerializeField] private MachineAnimatorBase animator;
        
        [Header("HUD")]
        [SerializeField] private string UI_Panel;

        private int depositedTokens;
        
        private const string tokenKey = "DepositedTokens";

        #endregion

        #region SETUP
        
        protected virtual void Awake()
        {
            LoadData();
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

        #endregion

        #region ENGINE

        /// <summary>
        ///     OnGameActive is called every frame if we are engaged with the machine.
        /// </summary>
        protected virtual void OnGameActive() { }

        #endregion

        #region METHODS
        
        #endregion

        #region DEBUGGING
        #endregion
    }
}