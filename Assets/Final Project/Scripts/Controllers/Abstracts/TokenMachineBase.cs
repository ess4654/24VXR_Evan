using ArcadeGame.Data;
using static ArcadeGame.Data.Constants;
using ArcadeGame.Views;
using ArcadeGame.Views.Machines;
using Shared.Editor;
using TMPro;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Base class used by all token arcade machines.
    /// </summary>
    [SelectionBase]
    public abstract class TokenMachineBase : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [Header("Machine Settings")]
        [SerializeField] private GameState gameStateOnPlay;
        [SerializeField, Range(1, MaxMachineTokens)] private int requiredTokens;
        
        [SerializeField] private bool hasJackpot;
        [SerializeField, DependsUpon("hasJackpot")] private Vector2Int jackpotRange;
        [SerializeField, DependsUpon("hasJackpot")] private LightFlasher jackpotFlasher;
        [SerializeField, DependsUpon("hasJackpot")] private TextMeshPro[] jackpotLCD;
        
        [SerializeField] private bool hasTimer;
        [SerializeField, DependsUpon("hasTimer")] private float gameTime = 30;
        
        [SerializeField] private bool spinningGame;
        [SerializeField, DependsUpon("spinningGame")] private Transform spinAxis;
        
        [Tooltip("This key is used to save/load the number of tokens deposited into the machine.")]
        [SerializeField] private string saveKey;

        [Header("Animation")]
        [SerializeField] private ArcadeMachineAnimator animator;

        //[Header("Audio Settings")]
        //[SerializeField] private MachineAnimatorBase animator;
        
        [Header("HUD")]
        [SerializeField] private string UI_Panel;

        private int depositedTokens;
        private int currentJackpot;

        private const string tokenKey = "DepositedTokens";
        private const string jackpotKey = "JackpotAmount";

        #endregion

        #region SETUP

        protected virtual void Awake()
        {
            LoadData();
        }

        /// <summary>
        ///     Loads the data for this machine.
        /// </summary>
        protected void LoadData()
        {
            if (!string.IsNullOrWhiteSpace(saveKey))
            {
                depositedTokens = PlayerPrefs.GetInt(saveKey + tokenKey, 0);

                //load the jackpot information if this machine has a jackpot
                if (hasJackpot)
                {
                    currentJackpot = PlayerPrefs.GetInt(saveKey + jackpotKey, 0);

                    if (currentJackpot == 0)
                        ResetJackpot(); //generate the jackpot amount if it is not saved
                    else
                        UpdateJackpotDisplay();
                }
            }
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
        
        /// <summary>
        ///     Resets the jackpot amount.
        /// </summary>
        protected void ResetJackpot()
        {
            currentJackpot = jackpotRange.x == jackpotRange.y ? jackpotRange.x : Random.Range(jackpotRange.x, jackpotRange.y + 1);
            UpdateJackpotDisplay();

            PlayerPrefs.SetInt(saveKey + jackpotKey, currentJackpot);
            PlayerPrefs.Save(); //commit changes
        }

        /// <summary>
        ///     Updates the jackpot LCD display.
        /// </summary>
        private void UpdateJackpotDisplay()
        {
            foreach(var lcd in jackpotLCD)
                lcd.text = currentJackpot.ToString();
        }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("ResetJackpot")] bool m_ResetJackpot;
        #endregion
    }
}