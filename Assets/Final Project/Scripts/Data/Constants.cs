namespace ArcadeGame.Data
{
    /// <summary>
    ///     Global game constants for the arcade game.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Are we testing the game?
        /// </summary>
        public const bool Debugging = true;

        /// <summary>
        ///     The default key to use for saving/loading player prefs.
        /// </summary>
        public const string DefaultSaveFile = "ArcadeGameData";

        public const string TAG_BULLET = "Bullet";
        public const string TAG_CABINET_SCREEN = "Shooter Screen";

        #region TOKEN MACHINES

        /// <summary>
        ///     The maximum amount of tokens that any machine can require to play.
        /// </summary>
        public const int MaxMachineTokens = 10;

        /// <summary>
        ///     The number of tokens the player will start the game with.
        /// </summary>
        public const int StartingTokens = 10;

        /// <summary>
        ///     The amount of time to keep a button pressed down.
        /// </summary>
        public const float ButtonDownTime = 3f;//1.0f;

        #endregion

        #region TICKET MACHINES

        /// <summary>
        ///     Flag indicates jackpot amount.
        /// </summary>
        public const int Jackpot = -1;

        /// <summary>
        ///     The odds of hitting a jackpot. Makes the game more difficult.
        /// </summary>
        public const float JackpotOdds = .1f;

        /// <summary>
        ///     The odds of winning a large number of tickets.
        /// </summary>
        /// <remarks>
        ///     Note: This is different than the jackpot odds.
        /// </remarks>
        public const float LargeWinningOdds = .25f;

        #endregion
    }
}