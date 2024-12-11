using ArcadeGame.Data;
using System;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Broadcasts game events for the arcade that classes can subscribe to.
    /// </summary>
    public static class GameEventBroadcaster
    {
        #region DELEGATES

        /// <summary>
        ///    An event that contains a game state. 
        /// </summary>
        /// <param name="state">The state being passed.</param>
        public delegate void GameStateAction(GameState state);

        /// <summary>
        ///    An event that contains an integer. 
        /// </summary>
        /// <param name="value">Value of the integer.</param>
        public delegate void IntAction(int value);
        
        /// <summary>
        ///    An event that contains an interaction area. 
        /// </summary>
        /// <param name="area">Area that is being passed.</param>
        public delegate void InteractionAreaAction(InteractionArea area);

        /// <summary>
        ///    An event that contains information about a claw machine prize. 
        /// </summary>
        /// <param name="prizeType">The type of prize passed by the action.</param>
        /// <param name="prizeName">The name of the prize passed by the action.</param>
        public delegate void ClawMachinePrizeAction(ClawMachinePrizes prizeType, string prizeName);

        #endregion

        #region EVENTS

        /// <summary>
        ///     Called when the game state changes.
        /// </summary>
        public static event GameStateAction OnGameStateChanged;

        /// <summary>
        ///     Called when the player collects tokens.
        /// </summary>
        public static event IntAction OnTokensCollected;

        /// <summary>
        ///     Called when the player wins tickets.
        /// </summary>
        public static event IntAction OnTicketsWon;
        
        /// <summary>
        ///     Called if the player wins the jackpot of an arcade machine.
        /// </summary>
        public static event Action OnJackpotWon;

        /// <summary>
        ///     Called when the player enters a region.
        /// </summary>
        public static event InteractionAreaAction OnPlayerEnteredRegion;

        /// <summary>
        ///     Called when the player wins a toy from the claw machine.
        /// </summary>
        public static event ClawMachinePrizeAction OnClawMachinePrizeWon;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcasts event when game state has changed.
        /// </summary>
        /// <param name="newState">New game state.</param>
        public static void BroadcastGameStateChanged(GameState newState) =>
            OnGameStateChanged?.Invoke(newState);

        /// <summary>
        ///     Broadcasts event when tokens are collected.
        /// </summary>
        /// <param name="amountTokens">The amount of tokens collected by the player.</param>
        public static void BroadcastTokensCollected(int amountTokens) =>
            OnTokensCollected?.Invoke(amountTokens);

        /// <summary>
        ///     Broadcasts event when tickets are won.
        /// </summary>
        /// <param name="amountTickets">The amount of tickets won by the player.</param>
        public static void BroadcastTicketsWon(int amountTickets) =>
            OnTicketsWon?.Invoke(amountTickets);

        /// <summary>
        ///     Broadcasts event when the jackpot is won.
        /// </summary>
        public static void BroadcastJackpotWon() =>
            OnJackpotWon?.Invoke();

        /// <summary>
        ///     Broadcasts event when tickets are won.
        /// </summary>
        /// <param name="region">The region the player has entered.</param>
        public static void BroadcastPlayerEnteredRegion(InteractionArea region) =>
            OnPlayerEnteredRegion?.Invoke(region);

        /// <summary>
        ///     Broadcasts event when a prize from the claw machine is won.
        /// </summary>
        /// <param name="prizeType">Type of prize that was won.</param>
        /// <param name="prizeName">name of the prize that was won.</param>
        public static void BroadcastClawMachinePrize(ClawMachinePrizes prizeType, string prizeName) =>
            OnClawMachinePrizeWon?.Invoke(prizeType, prizeName);

        #endregion
    }
}