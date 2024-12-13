using ArcadeGame.Data;
using ArcadeGame.Views;
using Shared.Editor;
using Shared.Helpers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

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

        /// <summary>
        ///     The position the player is standing at the machine.
        /// </summary>
        private int playerPosition = 0;//-1;

        private const float jackpotFlashTime = 3.0f;

        #endregion

        #region SETUP

        private void OnEnable()
        {
            XRInputManager.OnControllerTrigger += HandleInputDown;
            XRInputManager.OnControllerTrigger += HandleInputUp;
        }

        private void OnDisable()
        {
            XRInputManager.OnControllerTrigger -= HandleInputDown;
            XRInputManager.OnControllerTrigger -= HandleInputUp;
        }

        private void HandleInputDown(InputDevice controllerData)
        {
            PressButton();
        }

        private void HandleInputUp(InputDevice controllerData)
        {
            ReleaseButton();
        }

        #endregion

        #region METHODS

        /// <summary>
        ///     Interact with the cyclone game.
        /// </summary>
        /// <param name="standingPosition">Standing position of the player.</param>
        public void Interact(int standingPosition)
        {
            playerPosition = standingPosition;
            base.Interact();
        }

        /// <summary>
        ///     Presses the button stopping the light.
        /// </summary>
        /// <returns>Completed press button task</returns>
        [ContextMenu("Press Button")]
        public async Task PressButton()
        {
            Log($"{GameData.State} : {gameStateOnPlay}");
            if (GameData.State != gameStateOnPlay) return; //not playing cyclone

            //animate the button press
            animator.PressButton(playerPosition);

            var lightIndex = lightController.ToggleCycling(false); //stop the light in it's current position
            
            //calculate the amount of tickets won.
            var ticketsWon = ticketAmounts[lightIndex];
            if(ticketsWon == Constants.Jackpot) //we hit the jackpot
            {
                Log("Jackpot Hit");
                if(Random.value <= Constants.JackpotOdds) //we have beaten the jackpot odds
                {
                    AwardTickets(currentJackpot);

                    await jackpotFlasher.FlashLightAtIndex(jackpotFlashTime, playerPosition); //flash the jackpot light for 3 seconds
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
                if (ticketsWon >= largeTicketThreshold) //large number of tickets won
                {
                    Log("Large Tickets Hit");
                    if (Random.value <= Constants.LargeWinningOdds) //we have beaten the odds of winning big
                    {
                        AwardTickets(currentJackpot);
                    }
                    else //Skip to the next light and recalculate the amount of tickets won
                    {
                        lightIndex = lightController.SkipNextLight();
                        AwardTickets(ticketAmounts[lightIndex]);
                    }
                }
                else //we did not hit a large winning or jackpot
                {
                    AwardTickets(ticketsWon);
                }
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

        #endregion

        #region DEBUGGING
        [Debugging]
        [SerializeField, InspectorButton("PressButton")] bool m_PressButton;
        #endregion
    }
}