using ArcadeGame.Data;

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

        #endregion

        #region EVENTS

        /// <summary>
        ///     Called when the game state changes.
        /// </summary>
        public static event GameStateAction OnGameStateChanged;

        /// <summary>
        ///     Called when the player wins tickets.
        /// </summary>
        public static event IntAction OnTicketsWon;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcasts event when game state has changed.
        /// </summary>
        /// <param name="newState">New game state.</param>
        public static void BroadcastGameStateChanged(GameState newState) =>
            OnGameStateChanged?.Invoke(newState);

        /// <summary>
        ///     Broadcasts event when tickets are won.
        /// </summary>
        /// <param name="amountTickets">The amount of tickets won by the player.</param>
        public static void BroadcastTicketsWon(int amountTickets) =>
            OnTicketsWon?.Invoke(amountTickets);

        #endregion
    }
}