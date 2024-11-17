using ArcadeGame.Data;
using Shared.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame.Controllers.Machines
{
    /// <summary>
    ///     Controls the behaviour of the big wheel arcade game.
    /// </summary>
    public class BigWheel : TicketMachineBase
    {
        public const string Filename = "Big Wheel";

        private const float jackpotChance = Constants.JackpotOdds / 7f; //reduced jackpot chance because there are only 16 ticket spaces
        private const float largeWinningChance = Constants.LargeWinningOdds / 3.5f; //reduced chance of winning big because there are only 16 ticket spaces
        private const float fullRotation = 360f;

        protected Vector3 RotationAxis => Vector3.right;

        #region METHODS

        /// <summary>
        ///     Spins the wheel and awards tickets.
        /// </summary>
        public void Spin()
        {
            //calculate where the wheel will stop & tickets won prior to animation
            var ticketsPair = CalculateTickets();
            var ticketsWon = ticketsPair.Value;
            var indexOfTickets = ticketsPair.Key;

            //Lock the final rotation axis
            var stopRotation = ((float)indexOfTickets / ticketAmounts.Count) * (fullRotation * RotationAxis);
            spinAxis.localEulerAngles = stopRotation;

            AwardTickets(ticketsWon);
        }

        /// <summary>
        ///     Calculates the number of tickets to be won.
        /// </summary>
        /// <returns>The number of tickets won tied to it's index int he array</returns>
        private KeyValuePair<int, int> CalculateTickets()
        {
            var randomIndex = Random.Range(0, ticketAmounts.Count);
            var ticketsWon = ticketAmounts[randomIndex];
            if (ticketsWon == Constants.Jackpot) //we hit the jackpot
            {
                Log("Jackpot Hit");
                if (Random.value <= jackpotChance) //we have beaten the jackpot odds
                {
                    Log("Jackpot Won");
                    return new KeyValuePair<int, int>(randomIndex, currentJackpot);
                }
                else //Recalculate tickets and return
                {
                    return CalculateTickets();
                }
            }

            if(ticketsWon >= largeTicketThreshold) //large number of tickets won
            {
                Log("Large Tickets Hit");
                if (Random.value <= largeWinningChance) //we have beaten the odds of winning big
                {
                    Log("Large Tickets Won");
                    return new KeyValuePair<int, int>(randomIndex, ticketsWon);
                }
                else //Recalculate tickets and return
                {
                    return CalculateTickets();
                }
            }

            return new KeyValuePair<int, int>(randomIndex, ticketsWon);
        }

        #endregion

        #region DEBBUGGING
        //[Debugging]
        [SerializeField, InspectorButton("Spin")] bool m_Spin;
        #endregion
    }
}