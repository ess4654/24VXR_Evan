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

        #endregion

        #region EVENTS

        /// <summary>
        ///     Called when the game state changes.
        /// </summary>
        public static event GameStateAction OnGameStateChanged;

        #endregion

        #region BROADCASTERS

        /// <summary>
        ///     Broadcasts event when game state has changed.
        /// </summary>
        /// <param name="newState">New game state.</param>
        public static void BroadcastGameStateChanged(GameState newState) =>
            OnGameStateChanged?.Invoke(newState);

        #endregion
    }
}