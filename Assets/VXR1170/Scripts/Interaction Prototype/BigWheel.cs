using ArcadeGame.Data;
using Shared.Editor;
using Shared.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [SerializeField, ReadOnly] private bool isSpinning;
        [SerializeField, Min(1)] private int rotations = 10;
        [SerializeField] private Vector2 spinTimeRange = new Vector2(10, 20);
        [SerializeField] private AnimationCurve spinCurve;
        [SerializeField] private bool reverseDirection;

        protected Vector3 RotationAxis => Vector3.right;
        private Vector3 startingRotation;

        #region METHODS
        
        /// <summary>
        ///     Spins the wheel and awards tickets.
        /// </summary>
        /// <returns>The number off tickets won by the player.</returns>
        public async Task<int> Spin()
        {
            if (isSpinning) return 0;
            isSpinning = true;

            // calculate where the wheel will stop & tickets won prior to animation
            var ticketsPair = CalculateTickets();
            var ticketsWon = ticketsPair.Value;
            var indexOfTickets = ticketsPair.Key;

            // get the rotation values for the animation
            var rotationAxis = RotationAxis;
            var stopAngle = ((float)indexOfTickets / ticketAmounts.Count) * fullRotation;
            var direction = reverseDirection ? -1 : 1;
            var stopRotation = (stopAngle - (reverseDirection ? 0 : fullRotation)) * rotationAxis;
            var spinRotation = stopRotation + (direction * rotations * fullRotation * rotationAxis);
            
            //Log($"Starting Euler: {spinAxis.localRotation.eulerAngles}");
            //Log($"Starting Rotation: {startingRotation}");

            var spinTime = Random.Range(spinTimeRange.x, spinTimeRange.y);
            LeanTween
                .value(0f, 1f, spinTime)
                .setOnUpdate((float t) =>
                {
                    spinAxis.localRotation = Quaternion.Euler(Vector3.LerpUnclamped(startingRotation, spinRotation, t));
                })
                .setOnComplete(() =>
                {
                    //Lock the final rotation axis
                    Quaternion q = spinAxis.localRotation;
                    q.eulerAngles = stopRotation;
                    spinAxis.localRotation = q;

                    //Log($"Stop Axis: {stopRotation}");
                    //Log($"Stop Rotation: {spinAxis.localRotation.eulerAngles}");
                    startingRotation = stopRotation; //set the starting rotation for the next spin
                    isSpinning = false;
                })
                .setEase(spinCurve);

            await Timer.WaitUntil(() => !isSpinning); //wait for spinning to stop
            if (this == null) return 0;

            AwardTickets(ticketsWon);

            return ticketsWon;
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
                    return new KeyValuePair<int, int>(randomIndex, currentJackpot);
                else //Recalculate tickets and return
                    return CalculateTickets();
            }

            if(ticketsWon >= largeTicketThreshold) //large number of tickets won
            {
                Log("Large Tickets Hit");
                if (Random.value <= largeWinningChance) //we have beaten the odds of winning big
                    return new KeyValuePair<int, int>(randomIndex, ticketsWon);
                else //Recalculate tickets and return
                    return CalculateTickets();
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