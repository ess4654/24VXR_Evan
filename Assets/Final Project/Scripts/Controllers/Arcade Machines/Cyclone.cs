using ArcadeGame.Data;
using ArcadeGame.Views;
using Shared.Editor;
using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the cyclone arcade machine.
    /// </summary>
    public sealed class Cyclone : TicketMachineBase
    {
        public const string Filename = "Cyclone";

        #region VARIABLE DECLARATIONS

        [Header("Cyclone Settings")]
        [SerializeField] private LightCycler lightController;

        private int playerPosition = 0;

        private const float jackpotFlashTime = 3.0f;

        #endregion

        #region SETUP

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void HandleInput()
        {
            if(true)
                PressButton();
            else
                ReleaseButton();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Starts the cyclone game.
        /// </summary>
        /// <param name="standingPosition">Standing position of the player.</param>
        public void StartGame(int standingPosition)
        {
            playerPosition = standingPosition;
            GameData.State = gameStateOnPlay;
        }

        /// <summary>
        ///     Presses the button stopping the light.
        /// </summary>
        /// <returns>Completed press button task</returns>
        public async Task PressButton()
        {
            //animate the button press
            animator.PressButton(playerPosition);

            var lightIndex = lightController.ToggleCycling(false); //stop the light in it's current position
            
            //calculate the amount of tickets won.
            var ticketsWon = ticketAmounts[lightIndex];
            if(ticketsWon == -1) //we hit the jackpot
            {
                Log("Jackpot Hit");
                if(Random.value <= Constants.JackpotOdds) //we have beaten the jackpot odds
                {
                    Log("Jackpot Won");
                    AwardTickets(currentJackpot);

                    await jackpotFlasher.FlashAll(jackpotFlashTime); //flash the jackpot light for 3 seconds
                    ResetJackpot(); //create a new jackpot amount
                }
                else //Skip to the next light and recalculate the amount of tickets won
                {
                    lightIndex = lightController.SkipNextLight();
                    AwardTickets(ticketAmounts[lightIndex]);
                }
            }
            else //we did not hit the jackpot
            {
                AwardTickets(ticketsWon);
            }

            await ReleaseButton(); //reset the button and light cycler
            lightController.ToggleCycling(true); //resume light spinning
        }

        /// <summary>
        ///     Releases the button resuming the light movement.
        /// </summary>
        /// <returns>Completed release button task</returns>
        private async Task ReleaseButton()
        {
            await Timer.WaitForSeconds(Constants.ButtonDownTime);

            //animate the button release
            animator.ReleaseButton(playerPosition);
        }

        /// <summary>
        ///     Awards the player with tickets.
        /// </summary>
        /// <param name="ticketsWon">Number of tickets won.</param>
        private void AwardTickets(int ticketsWon)
        {
            Log($"Tickets Won: {ticketsWon}");
            GameEventBroadcaster.BroadcastTicketsWon(ticketsWon);
        }

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("PressButton")] bool m_PressButton;
        #endregion
    }
}