using ArcadeGame.Views.Machines;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Base class used by all token arcade machines.
    /// </summary>
    public abstract class TokenMachineBase : MonoBehaviour
    {
        #region VARIABLE DECLARATIONS

        [Header("Machine Settings")]
        [SerializeField, Range(1, 10)] private int requiredTokens;
        [SerializeField] private bool hasJackpot;
        [SerializeField] private Vector2Int jackpotRange;
        
        [Tooltip("This key is used to save/load the number of tokens deposited into the machine.")]
        [SerializeField] private string saveKey;

        [Header("Animation")]
        [SerializeField] private ArcadeMachineAnimator animator;

        //[Header("Audio Settings")]
        //[SerializeField] private MachineAnimatorBase animator;

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

                    //generate the jackpot amount if it is not saved
                    if (currentJackpot == 0)
                    {
                        currentJackpot = jackpotRange.x == jackpotRange.y ? jackpotRange.x : Random.Range(jackpotRange.x, jackpotRange.y + 1);
                        PlayerPrefs.SetInt(saveKey + jackpotKey, currentJackpot);
                    }
                }
            }
        }

        #endregion

        #region METHODS

        #endregion
    }
}