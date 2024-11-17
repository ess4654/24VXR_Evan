using ArcadeGame.Views;
using Shared.Editor;
using Shared.Helpers.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Base class used by all arcade machines that award tickets.
    /// </summary>
    public abstract class TicketMachineBase : TokenMachineBase
    {
        #region VARIABLE DECALRATIONS

        [SerializeField] private bool spinningGame;
        [SerializeField, DependsUpon("spinningGame")] protected Transform spinAxis;

        [Header("Ticket Settings")]
        [SerializeField] protected int largeTicketThreshold = 100;
        [SerializeField] private bool hasJackpot;
        [SerializeField, DependsUpon("hasJackpot")] private Vector2Int jackpotRange;
        [SerializeField, DependsUpon("hasJackpot")] protected LightFlasher jackpotFlasher;
        [SerializeField, DependsUpon("hasJackpot")] private TextMeshPro[] jackpotLCD;
        
        [SerializeField] protected List<int> ticketAmounts;

        protected int currentJackpot;

        private const string jackpotKey = "JackpotAmount";

        #endregion

        #region SETUP

        protected override void LoadData()
        {
            base.LoadData();

            if (!saveKey.IsNullOrWhiteSpace())
            {
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
            foreach (var lcd in jackpotLCD)
                lcd.text = currentJackpot.ToString();
        }

        /// <summary>
        ///     Awards the player with tickets.
        /// </summary>
        /// <param name="ticketsWon">Number of tickets won.</param>
        protected void AwardTickets(int ticketsWon)
        {
            Log($"Tickets Won: {ticketsWon}");
            GameEventBroadcaster.BroadcastTicketsWon(ticketsWon);
        }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("ResetJackpot")] bool m_ResetJackpot;
        #endregion
    }
}